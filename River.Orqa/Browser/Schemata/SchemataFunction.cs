//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Schemata Function node.
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
	// class SchemataFunction
	//********************************************************************************************

	internal class SchemataFunction : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataFunction (DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			// Restrictions: Owner, Name
			this.restrictions = new string[] { schemaName, text };

			this.canDelete = true;
			this.canEdit = true;
			this.canRefresh = true;
			this.canDrag = true;
			this.hasDiscovery = true;

			Text = text;
			ImageIndex = SelectedImageIndex = SchemaIcons.Function;

			Nodes.Add(new TreeNode());
		}


		//========================================================================================
		// Methods
		//========================================================================================

		//========================================================================================
		// Discover()
		//========================================================================================

		internal override void Discover ()
		{
			if (Logger.IsEnabled)
				Logger.WriteSection(Text + " PARAMETERS");

			Statusbar.Message = "Discovering " + Text + " parameters...";

			Nodes.Clear();

			//DataTable table = dbase.OleConnection.GetOleDbSchemaTable(
			//    OleDbSchemaGuid.Procedure_Parameters, restrictions);

			//if (table.Rows.Count == 0)
			//{
			//    // TODO: ((SchemaTree)this.TreeView).UnlockWindow();
			//}
			//else
			//{
			//    int partyp;
			//    long maxlen;
			//    object omaxlen;
			//    TreeNode node;
			//    StringBuilder text;

			//    for (int i = 0; i < table.Rows.Count; i++)
			//    {
			//        text = new StringBuilder(table.Rows[i]["PARAMETER_NAME"].ToString().ToLower());
			//        text.Append(" (" + table.Rows[i]["TYPE_NAME"].ToString().Replace("PL/SQL ", String.Empty).ToLower());

			//        omaxlen = table.Rows[i]["CHARACTER_MAXIMUM_LENGTH"];
			//        if (omaxlen != System.DBNull.Value)
			//        {
			//            maxlen = (long)(omaxlen);
			//            if ((maxlen > 0) && (maxlen < 4290000000))
			//                text.Append("(" + maxlen + ")");
			//        }

			//        partyp = (int)(table.Rows[i]["PARAMETER_TYPE"]);
			//        switch (partyp)
			//        {
			//            case 1: text.Append(", Input)"); break;
			//            case 2: text.Append(", In/Out)"); break;
			//            case 3: text.Append(", Output)"); break;
			//            case 4: text.Append(", Return Value)"); break;

			//            default:
			//                text.Append(")");
			//                break;
			//        }

			//        node = new TreeNode(text.ToString(), 9, 9);

			//        Nodes.Add(node);

			//        if (Logger.IsEnabled)
			//            Logger.WriteRowData(table.Rows[i]);
			//    }
			//}

			Statusbar.Message = String.Empty;

			isDiscovered = true;
		}


		//========================================================================================
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string sql = "DROP FUNCTION " + schemaName + "." + Text;

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
				+ "' AND type='FUNCTION'"
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
						string preamble = reader.GetString(0).Substring("FUNCTION".Length).TrimStart();

						if (preamble.IndexOf("wrapped") > 0)
						{
							MessageBox.Show(
								"Unable to display function; contents are wrapped.",
								"Wrapped Content",
								MessageBoxButtons.OK,
								MessageBoxIcon.Information
								);
						}
						else
						{
							text.Append("CREATE OR REPLACE FUNCTION "
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
