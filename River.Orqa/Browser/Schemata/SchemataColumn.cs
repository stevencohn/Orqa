//************************************************************************************************
//				Copyright © 2002 Steven M. Cohn. All Rights Reserved.
//
// Title:		SchemataColumn.cs
//
// Facility:	Microsoft Development Environment 2003	Version 7.1.3088
// Environment:	Microsoft .NET Framework 1.1			Version 1.1.4322
//
// Description:	Schemata tree OraColumn node.
//
// Revision History:
// -Who------------------- -When---------- -What--------------------------------------------------
// Steven M. Cohn			27-May-2002		New
// Steven M. Cohn			27-Feb-2003		Separated from Schemata.cs
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataColumn
	//********************************************************************************************

	internal class SchemataColumn : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataColumn (DatabaseConnection dbase, SchemataNode srvnode, string text)
		{
			this.dbase      = dbase;
			this.srvnode    = srvnode;
			this.schemaName = srvnode.SchemaName;
			this.canDelete  = true;
			this.canDrag    = true;
			this.canRename  = true;

			Text = text;
			ImageIndex = SelectedImageIndex	= SchemaIcons.Column;
		}


		//========================================================================================
		// Delete()
		//========================================================================================

		internal override bool Delete ()
		{
			string colnam = Text.Substring(0,Text.IndexOf(" "));
			string tabnam = Parent.Parent.Text;
			string sql = "ALTER TABLE " + tabnam + " DROP COLUMN " + colnam;

			int count = 0;

			try
			{
				OracleCommand cmd = new OracleCommand(sql,dbase.OraConnection);
				count = cmd.ExecuteNonQuery();

				((SchemataNode)this.Parent).Refresh();
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			return (count > 0);
		}


		//========================================================================================
		// Rename()
		//========================================================================================

		internal override void Rename (string name)
		{
			string colnam = Text.Substring(0,Text.IndexOf(" "));
			string tabnam = Parent.Parent.Text;
			string sql = "ALTER TABLE " + tabnam + " RENAME COLUMN " + colnam + " TO " + name;

			OracleConnection con = dbase.OraConnection;

			try
			{
				OracleCommand cmd = new OracleCommand(sql,con);
				cmd.ExecuteNonQuery();

				((SchemataNode)this.Parent).Refresh();
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}
		}
	}
}
