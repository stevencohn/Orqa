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
	// class SchemataSynonymFolder
	//********************************************************************************************

	internal class SchemataSynonymFolder : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataSynonymFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase      = dbase;
			this.srvnode    = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh   = true;
			this.hasDiscovery = true;

			Text = "Synonyms";
			ImageIndex = SelectedImageIndex	= SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Synonym Folder");
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
			if (Logger.IsEnabled)
				Logger.WriteSection(schemaName + " SYNONYMS");

			Statusbar.Message = "Discovering synonyms...";

			Nodes.Clear();

			string sql
				=  "SELECT synonym_name, table_owner, table_name, db_link"
				+  "  FROM All_Synonyms"
				+  " WHERE owner='" + schemaName
				+ "' ORDER BY synonym_name";

			OracleCommand cmd = new OracleCommand(sql,dbase.OraConnection);
			OracleDataAdapter da = new OracleDataAdapter();
			da.SelectCommand = cmd;
			DataSet ds = new DataSet();

			try
			{
				int count = da.Fill(ds);

				if (count == 0)
				{
					// TODO: ((SchemaTree)this.TreeView).UnlockWindow();
				}
				else
				{
					if (Logger.IsEnabled)
					{
						Logger.WriteSection(Parent.Parent.Text.ToUpper() + " SYNONYMS");
						for (int r=0; r < ds.Tables[0].Rows.Count; r++)
						{
							Logger.WriteRowData(ds.Tables[0].Rows[r]);
						}
					}

					SchemataSynonym synonym;
					StringBuilder text;
					string dbLink;
					string name;
					string tableName;
					string tableOwner;

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						name = row["synonym_name"].ToString().ToLower();
						tableName = row["table_name"].ToString().ToLower();
						tableOwner = row["table_owner"].ToString().ToLower();

						text = new StringBuilder(name);
						text.Append(" (");
						text.Append(tableOwner);
						text.Append(".");
						text.Append(tableName);

						if (row["db_link"] != System.DBNull.Value)
						{
							dbLink = row["db_link"].ToString();
							if (dbLink.Length > 0)
							{
								text.Append("@");
								text.Append(dbLink);
							}
						}
						else
						{
							dbLink = String.Empty;
						}

						text.Append(")");

						synonym = new SchemataSynonym(dbase,srvnode,text.ToString());
						synonym.AddProperty("Name", name);
						synonym.AddProperty("Table Name", tableName);
						synonym.AddProperty("Table Owner", tableOwner);
						synonym.AddProperty("Owner", schemaName);
						synonym.AddProperty("DB Link", dbLink);

						Nodes.Add(synonym);
					}
				}

				ds = null;

				Statusbar.Message = String.Empty;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

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
