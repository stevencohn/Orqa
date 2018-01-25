//************************************************************************************************
//				Copyright © 2002 Steven M. Cohn. All Rights Reserved.
//
// Title:		SchemataServer.cs
//
// Facility:	Microsoft Development Environment 2003	Version 7.1.3088
// Environment:	Microsoft .NET Framework 1.1			Version 1.1.4322
//
// Description:	Root schemata tree node for a server instance.
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
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataServer
	//********************************************************************************************

	internal class SchemataServer : SchemataNode
	{

		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataServer (DatabaseConnection dbase)
		{
			this.dbase = dbase;

			this.restrictions = null;

			this.canRefresh   = true;
			this.canDrag	  = true;
			this.hasDiscovery = true;

			string host = (dbase.HostName == null ? "local" : dbase.HostName.ToLower());
			string svcnam = (dbase.ServiceName == null ? "." : dbase.ServiceName);

			Text = svcnam + " @" + host;
			ImageIndex = SelectedImageIndex	= SchemaIcons.Servers;

			this.AddProperty("Name", Text);
			this.AddProperty("Default Schema", dbase.DefaultSchema);
			this.AddProperty("Host Name", dbase.HostName);
			this.AddProperty("Service Name", dbase.ServiceName);
			this.AddProperty("Version", dbase.Version);
			this.AddProperty("DBID", dbase.DBID);

			// build regular expression to search for encrypted password value
			Regex reg = new Regex(
				@"Password\s*=\s*(.*);?",
				RegexOptions.IgnoreCase
				);

			string connectionString = dbase.ConnectionString;
			Match match = reg.Match(connectionString);
			if (match != null)
			{
				// hide password value (but give length hint ;-)
				connectionString = connectionString.Replace(
					match.Groups[1].Value,
					new string('*', match.Groups[1].Value.Length));
			}

			this.AddProperty("Connection String", connectionString);


			Discover();
		}


		//========================================================================================
		// Discover()
		//		Although hasDiscovery is set to false for this type, we'll use the Discover
		//		method to kick start our Configuration process.
		//========================================================================================

		internal override void Discover ()
		{
			Logger.WriteSection("SCHEMATA");

			Statusbar.Message = "Discovering database schema...";

			Nodes.Clear();

			DataTable schemata = dbase.OraConnection.GetSchema("Users");

			SchemataSchema schema;

			foreach (DataRow row in schemata.Rows)
			{
				schema = new SchemataSchema(dbase, row["NAME"].ToString());

				schema.AddProperty("Name", row["NAME"].ToString());
				//schema.AddProperty("Create Date", row["CREATEDATE"]);

				Nodes.Add(schema);
				Logger.WriteRowData(row);
			}

			Statusbar.Message = String.Empty;

			isDiscovered = true;
		}

	
		//========================================================================================
		// FindSchema()
		//========================================================================================

		internal SchemataSchema FindSchema (string schemaName)
		{
			int i = 0;
			bool found = false;
			SchemataSchema schema = null;
			schemaName = schemaName.ToLower();

			while ((i < this.Nodes.Count) && !found)
			{
				schema = (SchemataSchema)this.Nodes[i];
				if (! (found = schema.Text.ToLower().Equals(schemaName)))
					i++;
			}

			return (found ? schema : null);
		}
	}
}
