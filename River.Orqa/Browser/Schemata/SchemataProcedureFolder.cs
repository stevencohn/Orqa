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
	using System.ComponentModel;
	using System.Data;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataProcedureFolder
	//********************************************************************************************

	internal class SchemataProcedureFolder : SchemataNode
	{

		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataProcedureFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase      = dbase;
			this.srvnode    = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.restrictions = null;

			this.canRefresh   = true;
			this.hasDiscovery = true;

			Text = "Procedures";
			ImageIndex = SelectedImageIndex	= SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Procedure Folder");
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
				Logger.WriteSection(schemaName + " PROCEDURES");

			Statusbar.Message = translator.GetString("DiscoveringProcedures");

			base.Discover();
		}


		protected override void DoDiscoverWork (object sender, DoWorkEventArgs e)
		{
			string sql = 
@"SELECT O.object_name, O.created, O.last_ddl_time, O.status, S.text
    FROM dba_objects O
    JOIN dba_source S
      ON S.owner = O.owner
     AND S.name = O.object_name
     AND S.type = O.object_type
     AND S.line = 1
   WHERE O.owner = '" + schemaName + @"'
     AND O.object_type = 'PROCEDURE'
   ORDER BY O.object_name";

			Logger.WriteLine(sql);

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				try
				{
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							Logger.WriteLine(reader.GetString(0));

							var procedure = new SchemataProcedure(dbase, srvnode, reader.GetString(0));

							procedure.AddProperty("Name", reader.GetString(0));
							procedure.AddProperty("Created", reader.GetDateTime(1).ToString());
							procedure.AddProperty("Last DDL Time", reader.GetDateTime(2).ToString());
							procedure.AddProperty("Status", reader.GetString(3));

							string preamble = reader.GetString(4).ToString().Trim().ToLower();
							procedure.IsWrapped = preamble.Contains("wrapped");
							procedure.AddProperty("Wrapped", procedure.IsWrapped ? "Yes" : "No");

							if (reader.GetString(3) == "INVALID")
								procedure.ForeColor = System.Drawing.Color.Red;

							procedure.Nodes.Add(new TreeNode());

							discoveries.Add(procedure);
						}

						reader.Close();
					}
				}
				catch (Exception exc)
				{
					River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
				}
			}
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
