//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Schemata tree Table  node.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 02-Sep-2013		New
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

	internal class SchemataTriggerFolder : SchemataNode
	{

		private static class Colnum
		{
			public const int ObjectName = 0;
			public const int ObjectID = 1;
			public const int TriggerType = 2;
			public const int TriggeringEvent = 3;
			public const int BaseObjectType = 4;
			public const int BaseObject = 5;
			public const int EnabledStatus = 6;
			public const int Status = 7;
		}


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataTriggerFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase      = dbase;
			this.srvnode    = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.restrictions = null;

			this.canRefresh   = true;
			this.hasDiscovery = true;

			Text = translator.GetString("TriggersLabel");
			ImageIndex = SelectedImageIndex	= SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", translator.GetString("TriggerFolderLabel"));
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
				Logger.WriteSection(schemaName + " TRIGGERS");

			Statusbar.Message = translator.GetString("DiscoveringTriggers");

			base.Discover();
		}


		protected override void DoDiscoverWork (object sender, DoWorkEventArgs e)
		{
			string sql =
@"SELECT T.trigger_name,
         O.object_id,
         T.trigger_type,
         T.triggering_event,
         T.base_object_type,
         T.table_name || nvl2(T.column_name, '.'||T.column_name, '') baseObject,
         T.status isEnabled,
         O.status
    FROM SYS.all_objects O, SYS.all_triggers T
   WHERE T.owner = '" + schemaName + @"'
     and O.object_name (+) = T.trigger_name
     and O.owner (+)       = T.owner
     and O.object_type (+) = 'TRIGGER'
     and O.subobject_name (+) is null
     and O.object_id not in (SELECT purge_object FROM recyclebin)";

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

							var trigger = new SchemataTrigger(dbase, srvnode, reader.GetString(0));

							trigger.AddProperty("Name", reader.GetString(Colnum.ObjectName));
							trigger.AddProperty("Object ID", ((int)reader.GetDecimal(Colnum.ObjectID)).ToString());
							trigger.AddProperty("Trigger Type", reader.GetString(Colnum.TriggerType));
							trigger.AddProperty("Triggering Event", reader.GetString(Colnum.TriggeringEvent));

							string baseObjectType = reader.IsDBNull(Colnum.BaseObjectType)
								? String.Empty
								: reader.GetString(Colnum.BaseObjectType);

							trigger.AddProperty("Base Object Type", baseObjectType);

							string baseObject = reader.IsDBNull(Colnum.BaseObject)
								? String.Empty
								: reader.GetString(Colnum.BaseObject);
							
							trigger.AddProperty("Base Object", baseObject);

							string enabled = reader.GetString(Colnum.EnabledStatus);
							string status = reader.GetString(Colnum.Status);
							trigger.AddProperty("Enabled", enabled);
							trigger.AddProperty("Status", status);

							if (status == "INVALID")
							{
								trigger.ForeColor = System.Drawing.Color.Red;
							}
							else if (enabled == "DISABLED")
							{
								trigger.ForeColor = System.Drawing.Color.Gray;
							}

							discoveries.Add(trigger);
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
