//************************************************************************************************
//				Copyright © 2002 Steven M. Cohn. All Rights Reserved.
//
// Title:		SchemataColumnFolder.cs
//
// Facility:	Microsoft Development Environment 2003	Version 7.1.3088
// Environment:	Microsoft .NET Framework 1.1			Version 1.1.4322
//
// Description:	This class implements a SchemataNode that acts like a folder and knows how
//				to display a list of Table/View column objects.
//
// Revision History:
// -Who------------------- -When---------- -What--------------------------------------------------
// Steven M. Cohn			27-May-2002		New
// Steven M. Cohn			27-Feb-2003		Separated from Schemata.cs
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
	// class SchemataColumnFolder
	//********************************************************************************************

	internal class SchemataColumnFolder : SchemataNode
	{

		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataColumnFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Columns";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty(translator.GetString("PColumnFolderName"), Text);
			this.AddProperty(translator.GetString("PColumnFolderType"), "OraColumn Folder");
		}


		//========================================================================================
		// Methods
		//========================================================================================

		//========================================================================================
		// Discover()
		//		If an inheritor allows discoveries (HasDiscovery == true), then the inheritor
		//		should override this method to propulate its Nodes collections.
		//========================================================================================

		internal override void Discover ()
		{
			Statusbar.Message = "Discover " + Parent.Text + " columns...";

			Nodes.Clear();

			string sql;
			string tableName;
			var tableNode = Parent as SchemataTable;
			if ((tableNode != null) && !tableNode.CanDescribe)
			{
				// table node represents a SYNONYM which requires special handling...
				tableName = "ORQA_TMPVIEW";

				try
				{
					// create the temp view in our own schema so there are no permission issues
					sql = "CREATE OR REPLACE VIEW " + tableName
						+ " AS SELECT * FROM " + schemaName + "." + Parent.Text;

					Logger.WriteLine(sql);

					using (var mgr = new OracleCommand(sql, dbase.OraConnection))
					{
						mgr.ExecuteNonQuery();
					}
				}
				catch (Exception exc)
				{
					Logger.Write(exc);
					return;
				}

				sql =
					"SELECT owner, table_name, column_name, data_type, char_length, data_length, nullable"
					+ " FROM all_tab_columns"
					+ " WHERE table_name='" + tableName
					+ "' ORDER BY column_id";
			}
			else
			{
				// table node is a normal table in this schema
				tableName = Parent.Text;

				sql =
					"SELECT owner, table_name, column_name, data_type, char_length, data_length, nullable"
					+ " FROM all_tab_columns"
					+ " WHERE owner='" + schemaName
					+ "' AND table_name='" + tableName
					+ "' ORDER BY column_id";
			}

			Logger.WriteLine(sql);

			var cmd = new OracleCommand(sql, dbase.OraConnection);
			var da = new OracleDataAdapter();
			da.SelectCommand = cmd;
			var ds = new DataSet();

			try
			{
				int count = da.Fill(ds);

				if (Logger.IsEnabled)
				{
					Logger.WriteSection(Parent.Parent.Text.ToUpper() + " COLUMNS");
					for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
					{
						Logger.WriteRowData(ds.Tables[0].Rows[r]);
					}
				}

				if (count == 0)
				{
					//TODO: // TODO: ((SchemaTree)this.TreeView).UnlockWindow();
				}
				else
				{
					StringBuilder text;
					bool isNullable;
					int len;
					SchemataColumn column;

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						text = new StringBuilder(row["column_name"].ToString().ToLower());
						text.Append(" (" + row["data_type"].ToString().ToLower());

						len = (int)(decimal)row["char_length"];
						if (len > 0)
							text.Append("(" + len + ")");

						isNullable = (row["nullable"].ToString()[0] == 'Y');
						text.Append(isNullable ? ", Null)" : ", Not Null)");

						column = new SchemataColumn(dbase, srvnode, text.ToString());

						column.AddProperty(translator.GetString("PColumnOwner"), row["owner"].ToString());
						column.AddProperty(translator.GetString("PColumnTable"), row["table_name"].ToString());
						column.AddProperty(translator.GetString("PColumnName"), row["column_name"].ToString());
						column.AddProperty(translator.GetString("PColumnDataType"), row["data_type"].ToString());
						column.AddProperty(translator.GetString("PColumnCharLength"), row["char_length"].ToString());
						column.AddProperty(translator.GetString("PColumnDataLength"), row["data_length"].ToString());
						column.AddProperty(translator.GetString("PColumnNullable"), row["nullable"].ToString());

						Nodes.Add(column);
					}
				}

				ds = null;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}
			finally
			{
				if (tableName.Equals("ORQA_TMPVIEW"))
				{
					try
					{
						sql = "DROP VIEW " + tableName;
						Logger.WriteLine(sql);

						using (var mgr = new OracleCommand(sql, dbase.OraConnection))
						{
							mgr.ExecuteNonQuery();
						}
					}
					catch (Exception exc)
					{
						River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
					}
				}
			}

			Statusbar.Message = String.Empty;

			cmd.Dispose();
			cmd = null;

			isDiscovered = true;
		}


		//========================================================================================
		// DoDefaultAction()
		//========================================================================================

		internal override void DoDefaultAction ()
		{
			this.Toggle();
		}
	}
}
