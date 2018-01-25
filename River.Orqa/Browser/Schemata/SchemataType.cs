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
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataTable
	//********************************************************************************************

	internal class SchemataType : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataType (
			DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canDelete = true;
			this.canEdit = true;
			this.canDrag = true;
			this.canRename = true;

			Text = text;

			ImageIndex = SelectedImageIndex = SchemaIcons.Type;

			//Nodes.Add(new SchemataTypeBody(dbase, srvnode));
		}


		//========================================================================================
		// Methods
		//========================================================================================

		//========================================================================================
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string sql = "DROP TYPE " + schemaName + "." + Text;

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
			Describe("TYPE", Text, schemaName, window);
		}
	}
}
