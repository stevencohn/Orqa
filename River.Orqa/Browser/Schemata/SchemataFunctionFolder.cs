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
	// class SchemataFunctionFolder
	//********************************************************************************************

	internal class SchemataFunctionFolder : SchemataNode
	{

		private static class Colnum
		{
			public const int ObjectName = 0;
			public const int Created = 1;
			public const int LastDDLTime = 2;
			public const int Status = 3;
		}
		

		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataFunctionFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Functions";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Function Folder");
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
				Logger.WriteSection(schemaName + " FUNCTIONS");

			Statusbar.Message = "Discovering functions...";

			Nodes.Clear();

			string sql =
@"select object_name, created, last_ddl_time, status
    from SYS.dba_objects
   where owner = '" + schemaName + @"'
     and object_type = 'FUNCTION'
     and subobject_name is null
     and object_id not in (select purge_object from recyclebin)
   order by 1";

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				try
				{
					using (var reader = cmd.ExecuteReader())
					{
						string name;

						while (reader.Read())
						{
							name = reader.GetString(Colnum.ObjectName);
							Logger.WriteLine(name);

							var function = new SchemataFunction(dbase, srvnode, name);
							function.AddProperty("Name", name);

							function.AddProperty("Date Created",
								reader.GetDateTime(Colnum.Created).ToString());

							function.AddProperty("Date Modified",
								reader.GetDateTime(Colnum.LastDDLTime).ToString());

							function.AddProperty("Status", reader.GetString(Colnum.Status));

							Nodes.Add(function);
						}

						reader.Close();
					}

					Statusbar.Message = String.Empty;

					//if (Nodes.Count == 0)
					// TODO:	((SchemaTree)this.TreeView).UnlockWindow();

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
