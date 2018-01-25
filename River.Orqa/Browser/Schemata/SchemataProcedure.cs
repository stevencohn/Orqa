//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Schemata tree Table  node.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 27-Feb-2003		Separated from Schemata.cs
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Data;
	using System.Text;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataProcedure
	//********************************************************************************************

	internal class SchemataProcedure : SchemataNode
	{
		private int overload;


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataProcedure (DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.restrictions = null;

			this.hasDiscovery = true;
			this.canCompile = true;
			this.canDrag = true;
			this.canOpen = true;

			Text = text;
			ImageIndex = SelectedImageIndex = SchemaIcons.StoredProcedure;

			this.overload = 0;

			Nodes.Add(new TreeNode());
		}


		//========================================================================================
		// Properties
		//========================================================================================

		internal override bool CanDelete
		{
			get { return (Parent is SchemataProcedureFolder); }
		}

		internal override bool CanEdit
		{
			get { return (Parent is SchemataProcedureFolder); }
		}

		internal override bool CanRefresh
		{
			get { return (Parent is SchemataPackage); }
		}

		internal int Overload
		{
			get { return overload; }
			set { overload = value; }
		}

		internal SchemataParameter[] Parameters
		{
			get
			{
				if (!isDiscovered)
					Discover();

				SchemataParameter[] pars = new SchemataParameter[this.Nodes.Count];
				for (int i = 0; i < pars.Length; i++)
				{
					pars[i] = (SchemataParameter)this.Nodes[i];
				}

				return pars;
			}
		}


		//========================================================================================
		// Methods
		//========================================================================================

		//========================================================================================
		// Compile()
		//		If an inheritor allows compilation (CanCompile == true), then the inheritor
		//		should override this method to compile itself.
		//========================================================================================

		internal override void Compile (River.Orqa.Query.QueryWindow window)
		{
			Statusbar.Message = "Compiling...";

			OracleCommand cmd = new OracleCommand(
				"SELECT text"
				+ " FROM dba_source"
				+ " WHERE owner='" + schemaName
				+ "' AND name='" + Text
				+ "' AND type='PROCEDURE'"
				+ " ORDER BY line",
				dbase.OraConnection
				);

			try
			{
				OracleDataReader reader = cmd.ExecuteReader();

				if (reader.FieldCount > 0)
				{
					StringBuilder text = new StringBuilder();

					if (reader.Read())
					{
						// Modify first line to insert cmd and qualify name
						// We purposefully do not trim the end to preserve the space
						string preamble = reader.GetString(0).Substring("PROCEDURE".Length).TrimStart();

						if (preamble.IndexOf("wrapped") > 0)
						{
							window.Close();
							window = null;

							MessageBox.Show(
								"Unable to compile procedure; contents are wrapped.",
								"Wrapped Content",
								MessageBoxButtons.OK,
								MessageBoxIcon.Information
								);
						}
						else
						{
							text.Append("CREATE OR REPLACE PROCEDURE "
								+ schemaName + "." + preamble
								);

							// append rest of content
							while (reader.Read())
							{
								text.Append(reader.GetString(0));
							}

							window.InsertText(text.ToString());
							window.IsSaved = true;
							window.SetTitle(schemaName + "." + this.Text);
							window.MoveHome();
						}
					}
				}

				reader.Close();
				reader.Dispose();
				reader = null;

				Statusbar.Message = String.Empty;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			cmd.Dispose();
			cmd = null;

			if (window != null)
				window.Execute(ParseMode.Sequential);
		}


		//========================================================================================
		// Discover()
		//========================================================================================

		internal override void Discover ()
		{
			if (Logger.IsEnabled)
				Logger.WriteSection(Text + " PARAMETERS");

			Statusbar.Message = "Discovering " + Text + " parameters...";

			string sql =
				"SELECT owner, object_name, package_name, object_id, overload,"
				+ " COALESCE(argument_name, 'return_value') AS argument_name,"
				+ " position, sequence, default_value, default_length, in_out,"
				+ " data_length, data_precision, data_scale, radix, character_set_name,"
				+ " type_owner, type_subname, type_link, char_length, char_used,"
				+ " data_type, pls_type, type_name,"
				+ " CASE COALESCE(pls_type, data_type)"
				+ "   WHEN 'UNDEFINED' THEN type_name"
				+ "   ELSE coalesce(pls_type, data_type)"
				+ " END AS dtype"
				+ " FROM All_Arguments"
				+ " WHERE owner = '" + schemaName + "'"
				+ " AND package_name " + (Parent is SchemataPackage ? "='" + Parent.Text + "'" : " IS NULL")
				+ " AND object_name = '" + Text + "'"
				+ " AND data_level = 0"
				+ " ORDER BY position";

			Logger.WriteLine(sql);

			Nodes.Clear();

			OracleCommand cmd = new OracleCommand(sql, dbase.OraConnection);
			OracleDataAdapter da = new OracleDataAdapter();
			da.SelectCommand = cmd;
			DataSet ds = new DataSet();

			try
			{
				int count = da.Fill(ds);

				if (Logger.IsEnabled)
				{
					Logger.WriteSection(Text.ToUpper() + " ARGUMENTS");
					for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
					{
						Logger.WriteRowData(ds.Tables[0].Rows[r]);
					}
				}

				if (count > 0)
				{
					StringBuilder text;
					SchemataParameter parameter;
					ParameterDirection direction;
					object overload;
					string argumentName;
					string dtype;
					string dataType;
					string inout;
					int position;
					int imageIndex;

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						if ((overload = row["OVERLOAD"]) != DBNull.Value)
						{
							string ov = overload.ToString();
							if (int.Parse(ov) != this.overload)
							{
								continue;
							}
						}

						argumentName = row["ARGUMENT_NAME"].ToString().ToLower();

						dtype = row["DTYPE"].ToString().ToLower();
						dataType = row["DATA_TYPE"].ToString();

						if (dtype.Equals("VARCHAR2") && dataType.Equals("NVARCHAR2"))
							dtype = dataType.ToLower();
						else if (dtype.Equals("CHAR") && dataType.Equals("NCHAR"))
							dtype = dataType.ToLower();

						text = new StringBuilder(argumentName);
						text.Append(" (" + dtype);

						inout = row["IN_OUT"].ToString();
						position = (int)(decimal)row["POSITION"];

						if (position == 0)
						{
							text.Append(", Return Value)");
							imageIndex = SchemaIcons.ReturnValue;
							direction = ParameterDirection.ReturnValue;
						}
						else if (inout.Equals("OUT"))
						{
							text.Append(", Output)");
							imageIndex = SchemaIcons.ParameterOut;
							direction = ParameterDirection.Output;
						}
						else if (inout.Equals("IN/OUT"))
						{
							text.Append(", In/Out)");
							imageIndex = SchemaIcons.ParameterIn;
							direction = ParameterDirection.InputOutput;
						}
						else // if (inout.Equals("IN"))
						{
							text.Append(", Input)");
							imageIndex = SchemaIcons.ParameterIn;
							direction = ParameterDirection.Input;
						}

						parameter = new SchemataParameter(
							this.dbase, this.srvnode, text.ToString(),
							argumentName, dtype, direction);

						parameter.ImageIndex = parameter.SelectedImageIndex = imageIndex;

						// PROCEDURE_CATALOG
						parameter.AddProperty("Owner", row["OWNER"].ToString());
						parameter.AddProperty("Procedure Name", row["OBJECT_NAME"].ToString());
						parameter.AddProperty("Package Name", row["PACKAGE_NAME"].ToString());
						parameter.AddProperty("Object ID", row["OBJECT_ID"].ToString());
						parameter.AddProperty("Argument Name", argumentName);
						parameter.AddProperty("Data Type", dataType);
						parameter.AddProperty("PLS Type", row["PLS_TYPE"].ToString());
						parameter.AddProperty("Type Name", row["TYPE_NAME"].ToString());
						parameter.AddProperty("Position", position.ToString());
						parameter.AddProperty("Sequence", row["SEQUENCE"].ToString());
						parameter.AddProperty("Default Value", row["DEFAULT_VALUE"].ToString());
						parameter.AddProperty("Default Length", row["DEFAULT_LENGTH"].ToString());
						parameter.AddProperty("In/Out", row["IN_OUT"].ToString());
						parameter.AddProperty("Data Length", row["DATA_LENGTH"].ToString());
						parameter.AddProperty("Data Precision", row["DATA_PRECISION"].ToString());
						parameter.AddProperty("Data Scale", row["DATA_SCALE"].ToString());
						parameter.AddProperty("Radix", row["RADIX"].ToString());
						parameter.AddProperty("Char Set Name", row["CHARACTER_SET_NAME"].ToString());
						parameter.AddProperty("Char Length", row["CHAR_LENGTH"].ToString());
						parameter.AddProperty("Char Used", row["CHAR_USED"].ToString());

						Nodes.Add(parameter);
					}
				}

				ds = null;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			Statusbar.Message = String.Empty;

			isDiscovered = true;
		}


		//========================================================================================
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string sql = "DROP PROCEDURE " + schemaName + "." + Text;

			int count = 0;

			try
			{
				OracleCommand cmd = new OracleCommand(sql, dbase.OraConnection);
				count = cmd.ExecuteNonQuery();

				((SchemataNode)this.Parent).Refresh();
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			return (count != 0);
		}


		//========================================================================================
		// Edit()
		//========================================================================================

		internal override void Edit (River.Orqa.Query.QueryWindow window)
		{
			Statusbar.Message = "Reading text...";

			OracleCommand cmd = new OracleCommand(
				  "SELECT text"
				+ " FROM dba_source"
				+ " WHERE owner='" + schemaName
				+ "' AND name='" + Text
				+ "' AND type='PROCEDURE'"
				+ " ORDER BY line",
				dbase.OraConnection
				);

			try
			{
				OracleDataReader reader = cmd.ExecuteReader();

				if (reader.FieldCount > 0)
				{
					StringBuilder text = new StringBuilder();

					if (reader.Read())
					{
						// Modify first line to insert cmd and qualify name
						// We purposefully do not trim the end to preserve the space
						string preamble = reader.GetString(0).Substring("PROCEDURE".Length).TrimStart();

						text.Append("CREATE OR REPLACE PROCEDURE "
							+ schemaName + "." + preamble
							);

						// append rest of content
						while (reader.Read())
						{
							text.Append(reader.GetString(0));
						}

						window.InsertText(text.ToString());
						window.IsSaved = true;
						window.SetTitle(schemaName + "." + this.Text);
					}
				}

				reader.Close();
				reader.Dispose(); reader = null;

				Statusbar.Message = String.Empty;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			cmd.Dispose();
			cmd = null;
		}
	}
}
