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
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataSynonym
	//********************************************************************************************

	internal class SchemataSynonym : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataSynonym (DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canDescribe = false;
			this.canDelete = true;
			this.canDrag = true;

			Text = text;
			ImageIndex = SelectedImageIndex = SchemaIcons.Synonym;
		}


		//========================================================================================
		// Methods
		//========================================================================================

		//========================================================================================
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string sql = "DROP SYNONYM " + schemaName + "." + Text.Substring(0, Text.IndexOf(" "));

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
			Describe("SYNONYM", Text.Substring(0, Text.IndexOf(" ")), schemaName, window);
		}
	}
}
