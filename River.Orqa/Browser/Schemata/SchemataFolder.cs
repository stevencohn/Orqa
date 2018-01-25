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
	using System.Windows.Forms;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataFolder
	//********************************************************************************************

	internal class SchemataFolder : SchemataNode
	{
		private SchemataType type;


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		public SchemataFolder (MainForm form, ConnectionInfo cinfo, SchemataType type)
		{
			this.Form = form;

			switch (this.type = type)
			{
				case SchemataType.Column:
					Text = "Columns";
					break;

				case SchemataType.Function:
					Text = "Functions";
					restrictions = new string[] { null, schemaName, null, null };
					break;

				case SchemataType.Index:
					Text = "Indexes";
					break;

				case SchemataType.Package:
					Text = "Packages";
					restrictions = new Object[] { null, schemaName, null, null };
					break;

				case SchemataType.PackageBody:
					Text = "Package Bodies";
					restrictions = new Object[] { null, schemaName, null, null };
					break;

				case SchemataType.Procedure:
					Text = "Procedures";
					short ptype = 2;
					restrictions = new Object[] { null, schemaName, null, ptype };
					break;

				case SchemataType.Sequence:
					Text = "Sequences";
					restrictions = new Object[] { null, schemaName, null, null };
					break;

				case SchemataType.SystemTable:
					Text = "System Tables";
					restrictions = new Object[] { null, schemaName, null, "SYSTEM TABLE" };
					break;

				case SchemataType.UserTable:
					Text = "User Tables";
					restrictions = new Object[] { null, schemaName, null, "TABLE" };
					break;

				case SchemataType.View:
					Text = "Views";
					restrictions = new Object[] { null, schemaName, null };
					break;
			}

			ImageIndex			= 2;
			SelectedImageIndex	= 2;

			canRefresh = true;
			hasDiscovery = true;

			Nodes.Add(new TreeNode());
		}

	
		//========================================================================================
		// Configure()
		//		Factory method.  Be aware that this method will be invoked prior to our
		//		own constructor, since this is called from the base default constructor.
		//========================================================================================
		
		protected override void Configure ()
		{
		}


		//========================================================================================
		// Discover()
		//		If an inheritor allows discoveries (HasDiscovery == true), then the inheritor
		//		should override this method to propulate its Nodes collections.
		//========================================================================================

		public virtual void Discover ()
		{
			Nodes.Clear();

			switch (this.type = type)
			{
				case SchemataType.Column:		DiscoverColumns();			break;
				case SchemataType.Function:		DiscoverFunctions();		break;
				case SchemataType.Index:		DiscoverIndexes();			break;
				case SchemataType.Package:		DiscoverPackages();			break;
				case SchemataType.PackageBody:	DiscoverPackageBodies();	break;
				case SchemataType.Procedure:	DiscoverProcedures();		break;
				case SchemataType.Sequence:		DiscoverSequences();		break;
				case SchemataType.SystemTable:	DiscoverTables();			break;
				case SchemataType.UserTable:	DiscoverTables();			break;
				case SchemataType.View:			DiscoverViews();			break;
			}
		}

	
		//========================================================================================
		// DiscoverTables()
		//========================================================================================

		private void DiscoverTables (SchemaDescriptor sd)
		{
			if (Logger.Debugging)
				Logger.WriteSection(restrictions[1] + " " + restrictions[3] + "S");

			msg.Text = "Discovering tables...";

			string tabnam;
			TreeNode node;
			int icon = (sd.restrictions[3].ToString() == "TABLE" ? 3 : 4);
			DataTable table = sd.con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,sd.restrictions);

			if (table.Rows.Count == 0)
			{
				mainForm.SchemaTree.ReleaseWindowLock();
			}
			else for (int i=0; i < table.Rows.Count; i++)
				 {
					 tabnam = (string)(table.Rows[i]["TABLE_NAME"]);
					 node = new TreeNode(tabnam, icon, icon);
					 node.Nodes.Add(new TreeNode());

					 node.Tag = new SchemaDescriptor(
						 sd.srvnode,node,sd.con,sd.schemaName,OraObjects.Table,OraObjects.Table,null);

					 sd.node.Nodes.Add(node);

					 if (Logger.Debugging)
						 Logger.WriteRowData(table.Rows[i]);
				 }

			msg.Text = String.Empty;
		}


		//========================================================================================
		// DoDefaultAction()
		//========================================================================================

		internal virtual void DoDefaultAction ()
		{
			this.Toggle();
		}
	}
}
