//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Represents an Oracle database server connection.  All instances are exposed
// through this one connection.  Multiple Orqa query windows may reuse this one
// connection managed by its reference count.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Data;
	using System.Text;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Options;


	//********************************************************************************************
	// class Database
	//********************************************************************************************

	/// <summary>
	/// Represents an Oracle database server connection.  All instances are exposed
	/// through this one connection.  Multiple Orqa query windows may reuse this one
	/// connection managed by its reference count.
	/// </summary>

	internal class DatabaseConnection
	{
		private MechanismType mechanism;		// connection mechanism employed by this instance

		private string user;					// username for this instance
		private string password;				// password for this instance
		private string host;					// remote host name
		private int port;						// port number
		private string service;					// service name (tns/sid/descriptor)
		private string schema;					// current active schema
		private string dbid;					// unique database ID of installation

		private string connectionString;		// connection string
		private OracleConnection oracon;		// ODP.NET connection
		private string version;					// database version
		private int oraRefCount;				// reference count


		/// <summary>
		/// Indicates the connection mechanism used by this instance.
		/// </summary>

		public enum MechanismType
		{
			/// <summary>
			/// Direct local connection where user name and password are explictly specified
			/// but the service name is inferred from the DatabaseSetup.OracleSid value.
			/// No listener is required for an inferred direct connection.
			/// </summary>

			Direct,

			/// <summary>
			/// Local connection including user name, password and service name as
			/// described by the local tnsnames.ora file and provided by the local listener.
			/// </summary>

			LocalTns,

			/// <summary>
			/// Remote connection specified by user name, password, host, port and service
			/// name.  The service name must be declared in the local tnsnames.ora file
			/// and serviced by a listener running on the remote machine.  This mechanism
			/// can also be used to connect to a local service.
			/// </summary>

			Remote
		}


		//========================================================================================
		// Constructors
		//========================================================================================

		#region Constructors

		/// <summary>
		/// Creates a direct connection.
		/// </summary>
		/// <param name="user">
		/// The User Id used to auhenticate the connection and set the default schema.
		/// </param>
		/// <param name="password">The authentication password.</param>

		public DatabaseConnection (string user, string password)
			: this(user, password, null, 0, null)
		{
			mechanism = MechanismType.Direct;
		}


		/// <summary>
		/// Creates a local TNS connection.
		/// </summary>
		/// <param name="user">
		/// The User Id used to auhenticate the connection and set the default schema.
		/// </param>
		/// <param name="password">The authentication password.</param>
		/// <param name="service">The service name described in the local tnsnames.ora file.</param>

		public DatabaseConnection (string user, string password, string service)
			: this(user, password, null, 0, service)
		{
			mechanism = MechanismType.LocalTns;
		}


		/// <summary>
		/// Creates a remote connection.
		/// </summary>
		/// <param name="user">
		/// The User Id used to auhenticate the connection and set the default schema.
		/// </param>
		/// <param name="password">The authentication password.</param>
		/// <param name="host">The host name or IP address.</param>
		/// <param name="port">The service port number on the host.</param>
		/// <param name="service">The service name available through the listener on the host.</param>

		public DatabaseConnection (
			string user, string password,
			string host, int port, string service)
		{
			mechanism = MechanismType.Remote;

			this.user = user;
			this.password = password;
			this.host = host;
			this.port = port;
			this.service = service;
			this.schema = null;

			this.connectionString = null;
			this.oraRefCount = 0;
			this.version = null;
			this.oracon = null;
		}

		#endregion Constructors


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets a connection string stripped of Oracle-specific pieces.
		/// </summary>

		public string BasicConnectionString
		{
			get
			{
				string cs = ConnectionString;
				if (cs.Contains("DBA Privilege="))
				{
					string[] parts = cs.Split(';');
					var builder = new StringBuilder();
					foreach (string part in parts)
					{
						if (!part.StartsWith("DBA Privilege=", StringComparison.InvariantCultureIgnoreCase))
						{
							builder.Append(part);
							builder.Append(";");
						}
					}

					cs = builder.ToString();
				}

				return cs;
			}
		}


		/// <summary>
		/// Gets the connection string used by this instance.
		/// </summary>

		public string ConnectionString
		{
			get
			{
				if (connectionString == null)
				{
					var dsn = new StringBuilder();

					switch (mechanism)
					{
						case MechanismType.Direct:
							dsn.Append("User Id=");
							dsn.Append(user);
							dsn.Append(";Password=");
							dsn.Append(password);
							break;

						case MechanismType.LocalTns:
							dsn.Append("User ID=" + user);
							dsn.Append(";Password=" + password);
							dsn.Append(";Data Source=" + service);
							break;

						case MechanismType.Remote:
							dsn.Append("User Id=");
							dsn.Append(user);
							dsn.Append(";Password=");
							dsn.Append(password);
							dsn.Append(";Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=");
							dsn.Append(host);
							dsn.Append(")(PORT=");
							dsn.Append(port);
							dsn.Append("))(CONNECT_DATA=(SERVICE_NAME=");
							dsn.Append(service);
							dsn.Append(")))");
							break;
					}

					connectionString = dsn.ToString();
				}

				return connectionString;
			}
		}


		/// <summary>
		/// Gets the uniqu database ID of the Oracle installation.
		/// </summary>

		public string DBID
		{
			get
			{
				if (dbid == null)
					dbid = LookupQuickInfo("SELECT dbid||'' FROM v$database");

				return dbid;
			}
		}


		/// <summary>
		/// Gets or sets the default schema name targetted by this instance.
		/// </summary>

		public string DefaultSchema
		{
			get
			{
				if (schema == null)
					schema = LookupQuickInfo(
						"SELECT SYS_CONTEXT('userenv','CURRENT_SCHEMA') FROM dual");

				return schema;
			}

			set
			{
				using (var cmd = new OracleCommand(
					"ALTER SESSION SET current_schema=" + value,
					OraConnection))
				{
					cmd.ExecuteNonQuery();
				}

				Close(oracon);

				schema = value;
			}
		}


		/// <summary>
		/// Gets the name of the machine hosting the database.  This is reported
		/// by the About box.
		/// </summary>

		public string HostName
		{
			get
			{
				if (host == null)
					host = LookupQuickInfo("SELECT host_name FROM v$instance");

				return host;
			}
		}


		/// <summary>
		/// Gets the authentication mode: Normal, SYSDBA, SYSOPER
		/// </summary>

		public string Mode
		{
			get
			{
				string mode = "Normal";
				if (user.Contains("DBA Privilege="))
				{
					string[] parts = user.Split(';');
					foreach (string part in parts)
					{
						if (part.StartsWith("DBA Privilege="))
						{
							string[] bits = part.Split('=');
							mode = bits[1];
							break;
						}
					}
				}

				return mode;
			}
		}


		/// <summary>
		/// Gets an open Oracle ODP.NET connection.  This connection must be
		/// closed using the DatabaseConnection.Close method since a usage counter
		/// is maintained by the DatabaseConnection class.
		/// </summary>

		public OracleConnection OraConnection
		{
			get
			{
				if (oracon == null)
				{
					var dsn = new StringBuilder(ConnectionString);

					int timeout = UserOptions.GetInt("connections/loginTimeout");
					if (timeout > 0)
					{
						dsn.Append(";Connection timeout=" + timeout);
					}

					oracon = new OracleConnection(dsn.ToString());
					oracon.Open();
				}

				oraRefCount++;

				return oracon;
			}
		}


		internal string Password
		{
			get
			{
				string pwd = password;
				if (pwd.Contains("DBA Privilege="))
				{
					string[] parts = pwd.Split(';');
					var builder = new StringBuilder();
					foreach (string part in parts)
					{
						if (!part.StartsWith("DBA Privilege=", StringComparison.InvariantCultureIgnoreCase))
						{
							builder.Append(part);
							builder.Append(";");
						}
					}

					pwd = builder.ToString();
				}

				return pwd;
			}
		}


		/// <summary>
		/// Gets the service or TNS name
		/// </summary>

		public string ServiceName
		{
			get { return service; }
		}


		/// <summary>
		/// Gets the authentication user ID (username)
		/// </summary>

		internal string UserID
		{
			get
			{
				string uid = user;
				if (uid.Contains("DBA Privilege="))
				{
					string[] parts = uid.Split(';');
					var builder = new StringBuilder();
					foreach (string part in parts)
					{
						if (!part.StartsWith("DBA Privilege=", StringComparison.InvariantCultureIgnoreCase))
						{
							builder.Append(part);
							builder.Append(";");
						}
					}

					uid = builder.ToString();
				}

				return uid;
			}
		}
		

		/// <summary>
		/// Gets the product version of the current database installation.
		/// </summary>

		public string Version
		{
			get
			{
				if (version == null)
				{
					OracleConnection con = OraConnection;
					version = con.ServerVersion;
					Close(con);
				}

				return version;
			}
		}


		//========================================================================================
		// Close()
		//========================================================================================

		/// <summary>
		/// Closes the Oracle ODP.NET connection.
		/// </summary>
		/// <param name="con">We assume this a reference to our own oracon instance.</param>

		public void Close (OracleConnection con)
		{
			if (oraRefCount > 0)
			{
				oraRefCount--;

				if (oraRefCount == 0)
				{
					oracon.Close();
					oracon.Dispose();
					oracon = null;
				}
			}
		}


		//========================================================================================
		// LookupQuickInfo()
		//========================================================================================

		public string LookupQuickInfo (string query)
		{
			string info = null;

			using (var cmd = new OracleCommand(query))
			{
				cmd.Connection = OraConnection;

				using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
				{
					if (reader.Read())
					{
						info = reader.GetString(0);
					}
				}

				Close(cmd.Connection);
			}

			return info;
		}


		//========================================================================================
		// Equals()
		//========================================================================================

		public override bool Equals (object obj)
		{
			return Equals((DatabaseConnection)obj);
		}

		
		public bool Equals (DatabaseConnection dbase)
		{
			if (!user.Equals(dbase.user)) return false;
			if (!password.Equals(dbase.password)) return false;
			if (!host.Equals(dbase.host)) return false;
			if (!port.Equals(dbase.port)) return false;

			if (service == null)
			{
				if (dbase.service != null)
					return false;
			}
			else
			{
				if (!service.Equals(dbase.service))
					return false;
			}

			if (schema == null)
			{
				if (dbase.schema != null)
					return false;
			}
			else
			{
				if (!schema.Equals(dbase.schema))
					return false;
			}

			return true;
		}


		public override int GetHashCode ()
		{
			return 0;
		}
	}
}