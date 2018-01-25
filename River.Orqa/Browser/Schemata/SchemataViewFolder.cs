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
	// class SchemataViewFolder
	//********************************************************************************************

	internal class SchemataViewFolder : SchemataNode
	{

		private static class Colnum
		{
			public const int ObjectName = 0;
			public const int ObjectID = 1;
			public const int Status = 2;
			public const int ObjectOwner = 3;
			public const int Created = 4;
			public const int lastDDLTime = 5;
			public const int ObjectType = 6;
		}


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataViewFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.restrictions = null;

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Views";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "View Folder");
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
				Logger.WriteSection(schemaName + " VIEWS");

			Statusbar.Message = translator.GetString("DiscoveringViews");

			base.Discover();
		}


		protected override void DoDiscoverWork (object sender, DoWorkEventArgs e)
		{
			string sql =
@"select * from (
 select object_name, object_id, status,
        owner object_owner, created, last_ddl_time, 'VIEW' as object_type
   from SYS.all_objects
	where owner = '" + schemaName + @"'
		and object_type = 'VIEW'
		and subobject_name is NULL
		and object_id not in (select purge_object from recyclebin)
		and not exists (select 1 from all_queue_tables T where T.queue_table = object_name and T.owner = owner)
		and not (object_name like 'AQ%'
            and (object_name != 'AQ$_'|| object_name||'_G'
		     or  object_name != 'AQ$_'|| object_name||'_H'
		     or  object_name != 'AQ$_'|| object_name||'_I'
		     or  object_name != 'AQ$_'|| object_name||'_S'
		     or  object_name != 'AQ$_'|| object_name||'_T'))
  union all
 select object_name, object_id, status,
        S.table_owner object_owner, O.created, O.last_ddl_time, 'SYNONYM' as object_type
   from SYS.all_objects O, SYS.user_synonyms S
  where S.table_owner = O.owner
    and S.table_name = O.object_name
		and user = '" + schemaName + @"'
		and O.object_type = 'VIEW'
		and subobject_name is NULL
    and O.object_id not in (select purge_object from recyclebin))
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

							var view = new SchemataView(
								dbase,
								srvnode,
								reader.GetString(Colnum.ObjectName));

							if (reader.GetString(Colnum.Status) == "INVALID")
								view.ForeColor = System.Drawing.Color.Red;

							view.AddProperty("Name", reader.GetString(Colnum.ObjectName));
							view.AddProperty("Object ID", ((int)reader.GetDecimal(Colnum.ObjectID)).ToString());
							view.AddProperty("Created", reader.GetDateTime(Colnum.Created).ToString());
							view.AddProperty("Last DDL Time", reader.GetDateTime(Colnum.lastDDLTime).ToString());
							view.AddProperty("Status", reader.GetString(Colnum.Status));
							view.AddProperty("Type", reader.GetString(Colnum.ObjectType));
							//view.AddProperty("Text Length", ((int)reader.GetDecimal(4)).ToString());

							discoveries.Add(view);
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
