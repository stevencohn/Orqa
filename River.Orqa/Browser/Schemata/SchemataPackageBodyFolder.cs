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
	// class SchemataPackageBodyFolder
	//********************************************************************************************

	internal class SchemataPackageBodyFolder : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataPackageBodyFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh = true;
			this.hasDiscovery = true;

			Text = "Package Bodies";
			ImageIndex = SelectedImageIndex = SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty("Name", Text);
			this.AddProperty("Type", "Package Body Folder");
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
				Logger.WriteSection(schemaName + " PACKAGE BODIES");

			Statusbar.Message = translator.GetString("DiscoveringPackageBodies");

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
				+ "        WHERE E.type = 'PACKAGE BODY') E"
				+ "    ON E.name = O.object_name"
				+ " WHERE O.owner = '" + schemaName + "'"
				+ "   AND O.object_type = 'PACKAGE BODY'"
				+ " ORDER BY O.object_name";

			OracleCommand cmd = new OracleCommand(sql, dbase.OraConnection);

			try
			{
				SchemataPackageBody packageBody;
				OracleDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					Logger.WriteLine(schemaName + "." + reader.GetString(0));

					packageBody = new SchemataPackageBody(dbase, srvnode, reader.GetString(0));

					packageBody.AddProperty("Name", reader.GetString(0));
					packageBody.AddProperty("Created", reader.GetDateTime(1).ToString());
					packageBody.AddProperty("Last DDL Time", reader.GetDateTime(2).ToString());
					packageBody.AddProperty("Status", reader.GetString(3));

					string preamble = reader.GetString(4).ToString().Trim().ToLower();
					packageBody.IsWrapped = preamble.Contains("wrapped");
					packageBody.AddProperty("Wrapped", packageBody.IsWrapped ? "Yes" : "No");

					if ((int)reader.GetDecimal(5) == 1)
					{
						packageBody.ForeColor = System.Drawing.Color.Red;
					}
					else if (reader.GetString(3) == "INVALID")
					{
						packageBody.ForeColor = System.Drawing.Color.Green;
					}

					discoveries.Add(packageBody);
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
