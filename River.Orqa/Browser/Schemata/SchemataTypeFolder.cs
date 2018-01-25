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
	// class SchemataTableFolder
	//********************************************************************************************

	internal class SchemataTypeFolder : SchemataNode
	{
		private static class Colnum
		{
			public const int ObjectName = 0;
			public const int ObjectID = 1;
			public const int Created = 2;
			public const int LastDDLTime = 3;
			public const int Status = 4;
		}


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataTypeFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.restrictions = new string[] { schemaName };

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Types";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Type Folder");
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
			Logger.WriteSection(this.schemaName + " TYPES");

			Statusbar.Message = "Discovering types...";

			base.Discover();
		}


		protected override void DoDiscoverWork (object sender, DoWorkEventArgs e)
		{
			string sql =
@"select object_name, object_id, created, last_ddl_time, status
  from all_objects
 where owner = '" + schemaName + @"'
   and object_type = 'TYPE'
 order by 1";

			Logger.WriteLine(sql);

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				try
				{
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							if (Logger.IsEnabled)
								Logger.WriteLine(schemaName + "." + reader.GetString(0));

							var table = new SchemataType(
								dbase,
								srvnode,
								reader.GetString(Colnum.ObjectName));

							if (reader.GetString(Colnum.Status) == "INVALID")
								table.ForeColor = System.Drawing.Color.Red;

							table.AddProperty("Name", reader.GetString(Colnum.ObjectName));
							table.AddProperty("Object ID", ((int)reader.GetDecimal(Colnum.ObjectID)).ToString());
							table.AddProperty("Created", reader.GetDateTime(Colnum.Created).ToString());
							table.AddProperty("Last DDL Time", reader.GetDateTime(Colnum.LastDDLTime).ToString());
							table.AddProperty("Status", reader.GetString(Colnum.Status));

							discoveries.Add(table);
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
