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

	internal class SchemataTableFolder : SchemataNode
	{
		private static class Colnum
		{
			public const int ObjectName = 0;
			public const int ObjectID = 1;
			public const int Partitioned = 2;
			public const int IOTType = 3;
			public const int ObjectOwner = 4;
			public const int Created = 5;
			public const int LastDDLTime = 6;
			public const int Generated = 7;
			public const int Status = 8;
			public const int ObjectType = 9;
		}


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataTableFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.restrictions = new string[] { schemaName };

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Tables";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Table Folder");
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
			Logger.WriteSection(this.schemaName + " TABLES");

			Statusbar.Message = "Discovering tables...";

			base.Discover();
		}


		protected override void DoDiscoverWork (object sender, DoWorkEventArgs e)
		{
			string sql =
@"select * from (
 select O.object_name, O.object_id,
        decode(bitand(T.property, 32), 32, 'YES', 'NO') partitioned,
        decode(bitand(t.property, 64), 64, 'IOT',
               decode(bitand(t.property, 512), 512, 'IOT_OVERFLOW',
               decode(bitand(t.flags, 536870912), 536870912, 'IOT_MAPPING', null))) IOT_type,
        O.owner object_owner, O.created, O.last_ddl_time, O.generated, O.status, 'TABLE' object_type
   from SYS.all_objects O, SYS.tab$ T
  where O.owner = '" + schemaName + @"'
    and O.object_id = t.obj#(+)
    and o.object_type = 'TABLE'
    and not exists (select 1 from SYS.all_mviews where mview_name = O.object_name and owner = O.owner)
    and not exists (select 1 from all_queue_tables where queue_table = O.object_name and owner = O.owner)
    and not (object_name like 'AQ$_%_G'
         or  object_name like 'AQ$_%_H'
         or  object_name like 'AQ$_%_I'
         or  object_name like 'AQ$_%_S'
         or  object_name like 'AQ$_%_T')
     and O.object_name not in (select object_name from recyclebin)
     and (user = '" + schemaName + @"' or not object_name like 'BIN$%')
   union all
  select object_name, object_id,
         decode(bitand(T.property, 32), 32, 'YES', 'NO') partitioned,
         decode(bitand(T.property, 64), 64, 'IOT',
                decode(bitand(T.property, 512), 512, 'IOT_OVERFLOW',
                decode(bitand(T.flags, 536870912), 536870912, 'IOT_MAPPING', null))) iot_type,
         S.table_owner object_owner, O.created, O.last_ddl_time, O.generated, O.status, 'SYNONYM' object_type
    from SYS.all_objects O, SYS.user_synonyms S, SYS.tab$ T
   where S.table_owner = O.owner
     and S.table_name  = O.object_name
     and O.object_id   = T.obj#
     and O.object_type = 'TABLE'
     and '" + schemaName + @"' = USER)
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

							var table = new SchemataTable(
								dbase,
								srvnode,
								reader.GetString(Colnum.ObjectName),
								reader.GetString(Colnum.ObjectType));

							if (reader.GetString(Colnum.Status) == "INVALID")
								table.ForeColor = System.Drawing.Color.Red;

							table.AddProperty("Name", reader.GetString(Colnum.ObjectName));
							table.AddProperty("Object ID", ((int)reader.GetDecimal(Colnum.ObjectID)).ToString());
							table.AddProperty("Created", reader.GetDateTime(Colnum.Created).ToString());
							table.AddProperty("Last DDL Time", reader.GetDateTime(Colnum.LastDDLTime).ToString());
							table.AddProperty("Partitioned", reader.GetString(Colnum.Partitioned));
							table.AddProperty("Status", reader.GetString(Colnum.Status));
							table.AddProperty("Type", reader.GetString(Colnum.ObjectType));
							table.AddProperty("Generated", reader.GetString(Colnum.Generated));

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
