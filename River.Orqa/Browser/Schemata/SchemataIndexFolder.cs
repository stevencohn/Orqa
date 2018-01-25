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
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataIndexFolder
	//********************************************************************************************

	internal class SchemataIndexFolder : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataIndexFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase      = dbase;
			this.srvnode    = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh   = true;
			this.hasDiscovery = true;

			Text = "Indexes";
			ImageIndex = SelectedImageIndex	= SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Index Folder");
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
			// Owner, Name, TableOwner, TableName
			restrictions = new string[] { schemaName, null, null, Parent.Text };

			if (Logger.IsEnabled)
				Logger.WriteSection(Parent.Text + " INDEXES");

			Statusbar.Message = "Discovering " + Parent.Text + " indexes...";

			Nodes.Clear();

			DataTable table = dbase.OraConnection.GetSchema("Indexes", restrictions);

			DataRowCollection rows = table.Rows;

			if (rows.Count == 0)
			{
				// TODO: ((SchemaTree)this.TreeView).UnlockWindow();
			}
			else
			{
				DataRow row;
				string idxnam;
				string prevName = null;
				SchemataIndex index;
				TreeNode prevNode = null;

				for (int i=0; i < rows.Count; i++)
				{
					row = rows[i];

					idxnam = (string)(row["INDEX_NAME"]);

					if (idxnam != prevName)
					{
						if ((row["UNIQUENESS"] != System.DBNull.Value) && ((string)(row["UNIQUENESS"]) == "UNIQUE"))
						{
							idxnam += " (unique)";
						}

						// TODO: How do we identify a primary key?

						idxnam += " " + ((string)(row["INDEX_NAME"])).ToLower();

						index = new SchemataIndex(dbase,srvnode,idxnam);

						index.CollectProperties(table.Columns, row);

						Nodes.Add(index);

						prevNode = index;
						prevName = (string)(row["INDEX_NAME"]);
					}
					else
					{
						//prevNode.Text += ", " + ((string)(row["COLUMN_NAME"])).ToLower();
					}

					if (Logger.IsEnabled)
						Logger.WriteRowData(table.Rows[i]);
				}
			}

			Statusbar.Message = String.Empty;

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
