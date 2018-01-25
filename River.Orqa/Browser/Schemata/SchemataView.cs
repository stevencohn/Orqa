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
	using System.Data.Common;
	using System.Text;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataView
	//********************************************************************************************

	internal class SchemataView : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataView (DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canDelete = true;
			this.canEdit = true;
			this.canOpen = true;
			this.canDrag = true;
			this.canRename = true;

			Text = text;
			ImageIndex = SelectedImageIndex = SchemaIcons.View;

			Nodes.Add(new SchemataColumnFolder(dbase, srvnode));
			Nodes.Add(new SchemataIndexFolder(dbase, srvnode));
		}


		//========================================================================================
		// Methods
		//========================================================================================

		//========================================================================================
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string sql = "DROP VIEW " + schemaName + "." + Text;

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
			Statusbar.Message = "Reading view text...";

			string sql =
@"SELECT V1.owner, V1.view_name, V1.text, V2.comments
    FROM all_views V1
    JOIN all_tab_comments V2
      ON V2.owner = V1.owner
     AND V2.table_name = V1.view_name
   WHERE V1.view_name = '" + Text + @"'
     AND V1.owner = '" + schemaName + "'";

			Logger.WriteLine(sql);

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				// required to fetch the "text" column which is a LONG
				cmd.InitialLONGFetchSize = int.MaxValue;

				try
				{
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.FieldCount > 0)
						{
							if (reader.Read())
							{
								var text = new StringBuilder();

								text.Append("CREATE OR REPLACE VIEW ");
								text.Append(schemaName.ToUpper());
								text.Append(".");
								text.Append(Text);
								text.Append("\n");
								text.Append("AS ");
								text.Append("\n");

								string body = reader.GetOracleString(2).ToString().TrimStart();
								text.Append(body);

								window.InsertText(text.ToString());
								window.IsSaved = true;
								window.SetTitle(schemaName + "." + this.Text);
							}
						}

						reader.Close();
					}
				}
				catch (Exception exc)
				{
					River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
				}

				Statusbar.Message = String.Empty;
			}
		}
	}
}
