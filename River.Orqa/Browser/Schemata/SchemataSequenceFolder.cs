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
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataSequenceFolder
	//********************************************************************************************

	internal class SchemataSequenceFolder : SchemataNode
	{

		private static class Colnum
		{
			public const int SequenceOwner = 0;
			public const int SequenceName = 1;
			public const int MinValue = 2;
			public const int MaxValue = 3;
			public const int IncrementBy = 4;
			public const int CycleFlag = 5;
			public const int OrderFlag = 6;
			public const int CacheSize = 7;
			public const int LastNumber = 8;
		}
		

		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataSequenceFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Sequences";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Sequence Folder");
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
				Logger.WriteSection(schemaName + " SEQUENCES");

			Statusbar.Message = translator.GetString("DiscoveringSequences");

			base.Discover();
		}


		protected override void DoDiscoverWork (object sender, DoWorkEventArgs e)
		{
			/* SQLDeveloper:
			 * 
			select object_name,object_id
			  from all_objects
			 where object_type = 'SEQUENCE'
			   and owner = :SCHEMA
			   and not (object_name like 'AQ%'
				   and (object_name != 'AQ$_'|| object_name||'_G' 
					or  object_name != 'AQ$_'|| object_name||'_H' 
					or  object_name != 'AQ$_'|| object_name||'_I'
					or  object_name != 'AQ$_'|| object_name||'_S'
					or  object_name != 'AQ$_'|| object_name||'_T'))
			*/

			string sql =
@"SELECT sequence_owner, sequence_name, min_value, max_value, increment_by,
         cycle_flag, order_flag, cache_size, last_number
   FROM all_sequences
  WHERE sequence_owner='" + schemaName + @"'
  ORDER BY sequence_name";

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				try
				{
					using (var reader = cmd.ExecuteReader())
					{
						string name;
						decimal lv;

						while (reader.Read())
						{
							name = reader.GetString(Colnum.SequenceName);
							Logger.WriteLine(schemaName + "." + name);

							var sequence = new SchemataSequence(dbase, srvnode, name);

							sequence.AddProperty("Name", name);
							sequence.AddProperty("Owner", reader.GetString(Colnum.SequenceOwner));

							lv = reader.GetDecimal(Colnum.MinValue);
							sequence.AddProperty("Min Value",
								(lv > long.MaxValue ? "> " + long.MaxValue.ToString() : ((long)lv).ToString()));

							lv = reader.GetDecimal(Colnum.MaxValue);
							sequence.AddProperty("Max Value",
								(lv > long.MaxValue ? "> " + long.MaxValue.ToString() : ((long)lv).ToString()));

							lv = reader.GetDecimal(Colnum.IncrementBy);
							sequence.AddProperty("Increment By",
								(lv > long.MaxValue ? "> " + long.MaxValue.ToString() : ((long)lv).ToString()));

							sequence.AddProperty("Cycle Flag", reader.GetString(Colnum.CycleFlag));
							sequence.AddProperty("Order Flag", reader.GetString(Colnum.OrderFlag));

							sequence.AddProperty("Cache Size",
								((long)reader.GetDecimal(Colnum.CacheSize)).ToString());

							lv = reader.GetDecimal(Colnum.LastNumber);
							sequence.AddProperty("Last Number",
								(lv > long.MaxValue ? "> " + long.MaxValue.ToString() : ((long)lv).ToString()));

							discoveries.Add(sequence);
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
