//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Schemata tree Trigger node.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 02-Sep-2013		New
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

	internal class SchemataTrigger : SchemataNode
	{
		private int overload;


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataTrigger (DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.restrictions = null;

			this.hasDiscovery = true;
			this.canCompile = true;
			this.canEdit = true;
			this.canDrag = true;

			Text = text;
			ImageIndex = SelectedImageIndex = SchemaIcons.Trigger;

			this.overload = 0;

			Nodes.Add(new TreeNode());
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

			string sql = "SELECT text"
						+ " FROM dba_source"
						+ " WHERE owner='" + schemaName
						+ "' AND name='" + Text
						+ "' AND type='TRIGGER'"
						+ " ORDER BY line";

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				try
				{
					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							// Modify first line to insert cmd and qualify name
							// We purposefully do not trim the end to preserve the space
							string preamble = reader.GetString(0).Substring("TRIGGER".Length).TrimStart();

							var text = new StringBuilder();
							text.Append("CREATE OR REPLACE TRIGGER "
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

						reader.Close();
					}

					Statusbar.Message = String.Empty;
				}
				catch (Exception exc)
				{
					River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
				}
			}

			if (window != null)
				window.Execute(ParseMode.Sequential);
		}


		//========================================================================================
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string sql = "DROP TRIGGER " + schemaName + "." + Text;
			Logger.WriteLine(sql);

			int count = 0;

			try
			{
				using (var cmd = new OracleCommand(sql, dbase.OraConnection))
				{
					count = cmd.ExecuteNonQuery();
				}

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

			string sql = "SELECT text"
						+ " FROM dba_source"
						+ " WHERE owner = '" + schemaName
						+ "' AND name = '" + Text
						+ "' AND type = 'TRIGGER'"
						+ " ORDER BY line";

			Logger.WriteLine(sql);

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				try
				{
					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						if (reader.FieldCount > 0)
						{
							var text = new StringBuilder();

							if (reader.Read())
							{
								// Modify first line to insert cmd and qualify name
								// We purposefully do not trim the end to preserve the space
								string preamble = reader.GetString(0).Substring("TRIGGER".Length).TrimStart();

								text.Append("CREATE OR REPLACE TRIGGER "
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
					}

					Statusbar.Message = String.Empty;
				}
				catch (Exception exc)
				{
					River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
				}
			}
		}
	}
}
