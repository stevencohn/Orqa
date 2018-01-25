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
	// class SchemataPackageFolder
	//********************************************************************************************

	internal class SchemataPackageFolder : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataPackageFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Packages";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Package Folder");
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
				Logger.WriteSection(schemaName + " PACKAGES");

			Statusbar.Message = translator.GetString("DiscoveringPackages");

			base.Discover();
		}


		protected override void DoDiscoverWork (object sender, DoWorkEventArgs e)
		{
			string sql =
				"SELECT O.object_name, O.created, O.last_ddl_time, O.status, S.text,"
				+ "       COALESCE(E.IsError, 0) AS IsError"
				+ "  FROM dba_objects O"
				+ "  JOIN dba_source S"
				+ "    ON S.owner = O.owner"
				+ "   AND S.name = O.object_name"
				+ "   AND S.type = O.object_type"
				+ "   AND S.line = 1"
				+ "  LEFT OUTER JOIN"
				+ "      (SELECT DISTINCT E.name, 1 AS IsError"
				+ "         FROM User_Errors E"
				+ "        WHERE E.type = 'PACKAGE') E"
				+ "    ON E.name = O.object_name"
				+ " WHERE O.owner = '" + schemaName + "'"
				+ "   AND O.object_type = 'PACKAGE'"
				+ " ORDER BY O.object_name";

			OracleCommand cmd = new OracleCommand(sql, dbase.OraConnection);

			try
			{
				SchemataPackage package;
				OracleDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					if (Logger.IsEnabled)
						Logger.WriteLine(schemaName + "." + reader.GetString(0));

					package = new SchemataPackage(dbase, srvnode, reader.GetString(0));

					package.AddProperty("Name", reader.GetString(0));
					package.AddProperty("Created", reader.GetDateTime(1).ToString());
					package.AddProperty("Last DDL Time", reader.GetDateTime(2).ToString());
					package.AddProperty("Status", reader.GetString(3));

					string preamble = reader.GetString(4).ToString().Trim().ToLower();
					package.IsWrapped = preamble.Contains("wrapped");
					package.AddProperty("Wrapped", package.IsWrapped ? "Yes" : "No");

					if ((int)reader.GetDecimal(5) == 1)
					{
						package.ForeColor = System.Drawing.Color.Red;
					}
					else if (reader.GetString(3) == "INVALID")
					{
						package.ForeColor = System.Drawing.Color.Green;
					}

					discoveries.Add(package);
				}

				reader.Close();
				reader.Dispose();
				reader = null;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			cmd.Dispose();
			cmd = null;
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
