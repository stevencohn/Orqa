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
	// class SchemataPackageBody
	//********************************************************************************************

	internal class SchemataPackageBody : SchemataNode
	{

		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataPackageBody (DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canCompile = true;
			this.canDelete = true;
			this.canEdit = true;
			this.canDrag = true;

			Text = text;
			ImageIndex = SelectedImageIndex = SchemaIcons.Package;
		}


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
				+ "  FROM dba_source"
				+ " WHERE owner='" + schemaName
				+ "'  AND name='" + Text
				+ "'  AND type='PACKAGE BODY'"
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
						string preamble = reader.GetString(0).Substring("PACKAGE BODY".Length).TrimStart();

						if (preamble.IndexOf("wrapped") > 0)
						{
							window.Close();

							MessageBox.Show(
								"Unable to compile package body; contents are wrapped.",
								"Wrapped Content",
								MessageBoxButtons.OK,
								MessageBoxIcon.Information
								);
						}
						else
						{
							text.Append("CREATE OR REPLACE PACKAGE BODY "
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
				reader.Dispose(); reader = null;

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
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string sql = "DROP PACKAGE BODY " + schemaName + "." + Text;

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
				+ "' AND type='PACKAGE BODY'"
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
						string preamble = reader.GetString(0).Substring("PACKAGE BODY".Length).TrimStart();

						text.Append(
							"CREATE OR REPLACE PACKAGE BODY "
							+ schemaName + "." + preamble
							);

						// append rest of content
						while (reader.Read())
						{
							text.Append(reader.GetString(0));
						}

						window.InsertText(text.ToString());
						window.MoveHome();
						window.IsSaved = true;
						window.SetTitle(schemaName + "." + this.Text);
					}
				}

				reader.Close();
				reader.Dispose(); reader = null;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			Statusbar.Message = String.Empty;

			cmd.Dispose();
			cmd = null;
		}
	}
}
