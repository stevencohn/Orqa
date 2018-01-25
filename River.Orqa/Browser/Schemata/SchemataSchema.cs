//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Schemata tree schema node.
//
// Revision History:
// -Who------------------- -When---------- -What--------------------------------------------------
// Steven M. Cohn			27-May-2002		New
// Steven M. Cohn			27-Feb-2003		Separated from Schemata.cs
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Data;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataSchema
	//********************************************************************************************

	internal class SchemataSchema : SchemataNode
	{
		private SchemataPackageFolder packages;
		private SchemataProcedureFolder procedures;


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataSchema (DatabaseConnection dbase, string text)
		{
			this.dbase = dbase;
			this.srvnode = this;
			this.schemaName = text;
			this.canDrag = true;
			this.canRefresh = true;
			this.hasDiscovery = true;

			string status = GetStatus(text);

			Text = text;
			ImageIndex = SelectedImageIndex = (status.Equals("OPEN")
				? SchemaIcons.Database
				: SchemaIcons.DatabaseLocked);

			Discover();
		}


		//========================================================================================
		// Database
		//========================================================================================

		public DatabaseConnection Database
		{
			get { return dbase; }
		}


		//========================================================================================
		// Discover()
		//		Schemas aren't really discovered.  They only have a pre-defined set of folders
		//		so here we only erase the folder contents to force rediscovery on those.
		//========================================================================================

		internal override void Discover ()
		{
			Nodes.Clear();

			Nodes.Add(new SchemataTableFolder(dbase, this));
			Nodes.Add(new SchemataViewFolder(dbase, this));
			Nodes.Add(new SchemataFunctionFolder(dbase, this));
			Nodes.Add(packages = new SchemataPackageFolder(dbase, this));
			Nodes.Add(new SchemataPackageBodyFolder(dbase, this));
			Nodes.Add(procedures = new SchemataProcedureFolder(dbase, this));
			Nodes.Add(new SchemataSequenceFolder(dbase, this));
			Nodes.Add(new SchemataSynonymFolder(dbase, this));
			Nodes.Add(new SchemataTriggerFolder(dbase, this));
			Nodes.Add(new SchemataTypeFolder(dbase, this));
		}


		//========================================================================================
		// DoDefaultAction()
		//========================================================================================

		internal override void DoDefaultAction ()
		{
			this.Toggle();
		}


		//========================================================================================
		// FindPackage()
		//========================================================================================

		public SchemataPackage FindPackage (string packageName)
		{
			if (!packages.IsDiscovered)
				packages.DiscoverNow();

			int i = 0;
			bool found = false;
			SchemataPackage package = null;
			packageName = packageName.ToLower();

			while ((i < packages.Nodes.Count) && !found)
			{
				package = (SchemataPackage)packages.Nodes[i];
				if (!(found = package.Text.ToLower().Equals(packageName)))
					i++;
			}

			return (found ? package : null);
		}


		//========================================================================================
		// FindProcedure()
		//========================================================================================

		public SchemataProcedure FindProcedure (string procedureName)
		{
			if (!procedures.IsDiscovered)
				procedures.DiscoverNow();

			int i = 0;
			bool found = false;
			SchemataProcedure procedure = null;
			procedureName = procedureName.ToLower();

			while ((i < procedures.Nodes.Count) && !found)
			{
				procedure = (SchemataProcedure)procedures.Nodes[i];
				if (!(found = procedure.Text.ToLower().Equals(procedureName)))
					i++;
			}

			return (found ? procedure : null);
		}


		//========================================================================================
		// GetStatus()
		//========================================================================================

		private string GetStatus (string name)
		{
			string status = null;

			string sql =
				"SELECT user_id, account_status, default_tablespace, profile"
				+ " FROM DBA_USERS"
				+ " WHERE username='" + name + "'";

			Logger.WriteLine(sql);

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				try
				{
					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							status = reader.GetString(1);

							this.AddProperty("User ID", ((int)reader.GetDecimal(0)).ToString());
							this.AddProperty("Status", status);
							this.AddProperty("Default Tablespace", reader.GetString(2));
							this.AddProperty("Profile", reader.GetString(3));
						}

						reader.Close();
					}
				}
				catch (Exception exc)
				{
					River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
				}
			}

			return status;
		}
	
		
		//========================================================================================
		// ToString()
		//		Used by SchemaSelector
		//========================================================================================

		public override string ToString ()
		{
			return this.Parent.Text.ToUpper() + "." + this.Text.ToUpper();
		}
	}
}
