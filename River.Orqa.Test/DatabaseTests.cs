//************************************************************************************************
// Copyright © 2002-2005 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.Orqa.UnitTests
{
	using System;
	using System.Configuration;
	using System.Data;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	[TestClass]
	public class DatabaseTests
	{
		#region Basic Database

		[TestMethod, Description("Database Basics")]
		public void DbConnInfo ()
		{
			Console.WriteLine("DatabaseTest");

			var pwd = ConfigurationManager.AppSettings["everest_password"];
			Assert.IsNotNull(pwd, "Must set everest_password in appSettings");

			var db = new DatabaseConnection("everest", pwd);
			Assert.IsNotNull(db);

			string s = db.ConnectionString;
			Assert.IsNotNull(s);
			Console.WriteLine("- ConnectionString=[" + s + "]");

			s = db.DefaultSchema;
			Assert.IsNotNull(s);
			Console.WriteLine("- DefaultSchema=[" + s + "]");

			s = db.HostName;
			Assert.IsNotNull(s);
			Console.WriteLine("- HostName=[" + s + "]");

			s = db.Version;
			Assert.IsNotNull(s);
			Console.WriteLine("- Version=[" + s + "]");
		}


		[TestMethod, Description("Database Basics")]
		public void GetSetupInformation ()
		{
			Assert.IsTrue(DatabaseSetup.IsOracleInstalled);

			if (DatabaseSetup.IsOracleInstalled)
			{
				Assert.IsTrue(DatabaseSetup.IsOdpInstalled);

				Console.WriteLine("- OracleHome: [" + DatabaseSetup.OracleHome + "]");
				Console.WriteLine("- OracleHomeKey: [" + DatabaseSetup.OracleHomeKey + "]");
				Console.WriteLine("- OracleHomeName: [" + DatabaseSetup.OracleHomeName + "]");
				Console.WriteLine("- OracleSid: [" + DatabaseSetup.OracleSid + "]");
			}
		}


		[TestMethod, Description("Database Basics")]
		public void TnsLessConnection ()
		{
			var pwd = ConfigurationManager.AppSettings["everest_password"];
			Assert.IsNotNull(pwd, "Must set everest_password in appSettings");

			using (var con = new OracleConnection($"User Id=everest;Password={pwd};"))
			{
				con.Open();
				con.Close();
			}
		}


		[TestMethod, Description("Database Basics")]
		public void TnsNames ()
		{
			TnsName[] names = Tns.Names;

			Assert.IsNotNull(names);
			Assert.IsTrue(names.Length > 0);

			Console.WriteLine("TNS.RegistryPath=[" + Tns.RegistryPath + "]");
			Console.WriteLine("TNS.TnsNamesOraPath=[" + Tns.TnsNameOraPath + "]");
			Console.WriteLine("TNS.Count=[" + names.Length + "]");

			foreach (TnsName name in names)
			{
				Console.WriteLine("TNS name: " + name.Name + " (" + name.Location + ")");
			}
		}

		#endregion Basic Database


		[TestMethod, Description("Schema Discovery")]
		public void GetSchema1 ()
		{
			var pwd = ConfigurationManager.AppSettings["everest_password"];
			Assert.IsNotNull(pwd, "Must set everest_password in appSettings");

			using (var con = new OracleConnection($"User Id=everest;Password={pwd};"))
			{
				con.Open();

				using (var cmd = con.CreateCommand())
				{
					string sql =
@"select nvl(b.tablespace_name,
       nvl(a.tablespace_name,'UNKNOWN')) ""Tablespace"",
       kbytes_alloc ""Allocated MB"",
       kbytes_alloc-nvl(kbytes_free,0) ""Used MB"",
       nvl(kbytes_free,0) ""Free MB"",
       ((kbytes_alloc-nvl(kbytes_free,0))/kbytes_alloc) ""Used"",
       data_files ""Data Files""
  from ( select sum(bytes)/1024/1024 Kbytes_free,
                max(bytes)/1024/1024 largest,
                tablespace_name
           from sys.dba_free_space
          group by tablespace_name ) a,
       ( select sum(bytes)/1024/1024 Kbytes_alloc,
                tablespace_name,
                count(*) data_files
           from sys.dba_data_files
          group by tablespace_name )b
 where a.tablespace_name (+) = b.tablespace_name
 order by 1";

					cmd.CommandText = sql;
					cmd.CommandType = CommandType.Text;

					bool addToStatementCache = cmd.AddToStatementCache;

					using (var reader = cmd.ExecuteReader())
					{
						do
						{
							long fetchSize = reader.FetchSize;
							Console.WriteLine("reader.FetchSize=[" + fetchSize + "]");

							int fieldCount = reader.FieldCount;
							Console.WriteLine("reader.FieldCount=[" + fieldCount + "]");

							long recordsAffected = reader.RecordsAffected;
							Console.WriteLine("reader.RecordsAffected=[" + recordsAffected + "]");

							long rowSize = reader.RowSize;
							Console.WriteLine("reader.RowSize=[" + rowSize + "]");

							int hiddenFieldcount = reader.HiddenFieldCount;
							Console.WriteLine("reader.HiddenFieldCount=[" + hiddenFieldcount + "]");

							int visibleFieldcount = reader.VisibleFieldCount;
							Console.WriteLine("reader.VisibleFieldCount=[" + visibleFieldcount + "]");


							for (int i = 0; i < fieldCount; i++)
							{
								string name = reader.GetName(i);
								Console.WriteLine("column(" + i + ").GetName=[" + name + "]");

								string dataTypeName = reader.GetDataTypeName(i);
								Console.WriteLine("column(" + i + ").GetDataTypeName=[" + dataTypeName + "]");

								Type fieldType = reader.GetFieldType(i);
								Console.WriteLine("column(" + i + ").GetFieldType=[" + fieldType.ToString() + "]");
							}

							DataTable schemaTable = reader.GetSchemaTable();

							int count = 0;
							while (reader.Read() && (count < 1))
							{
								for (int i = 0; i < fieldCount; i++)
								{
									Type type = null;

									string name = reader.GetName(i);
									var rows = schemaTable.Select("ColumnName='" + name + "'");
									if ((rows != null) && (rows.Length > 0))
									{
										var row = rows[0];
										type = (Type)row["DataType"];
										if (type == typeof(System.Decimal))
										{
											if ((short)row["NumericPrecision"] > 28)
											{
												type = typeof(System.Single); // float
											}
										}
									}

									if (reader.IsDBNull(i))
									{
										Console.WriteLine("value(" + i + ")=[DBNULL]");
									}
									else
									{
										if (type == typeof(System.Single))
										{
											double value = (double)reader.GetOracleDecimal(i);
											string v = value.ToString();
											Console.WriteLine("value(" + i + ")=[" + v + "]");
										}
										else
										{
											object value = reader[i];
											string v = value.ToString();
											Console.WriteLine("value(" + i + ")=[" + v + "]");
										}
									}
								}

								#region SchemaTable columns
								/*
								 [0]	{ColumnName}
								 [1]	{ColumnOrdinal}
								 [2]	{ColumnSize}
								 [3]	{NumericPrecision}
								 [4]	{NumericScale}
								 [5]	{IsUnique}
								 [6]	{IsKey}
								 [7]	{IsRowID}
								 [8]	{BaseColumnName}
								 [9]	{BaseSchemaName}
								[10]	{BaseTableName}
								[11]	{DataType}
								[12]	{ProviderType}
								[13]	{AllowDBNull}
								[14]	{IsAliased}
								[15]	{IsByteSemantic}
								[16]	{IsExpression}
								[17]	{IsHidden}
								[18]	{IsReadOnly}
								[19]	{IsLong}
								[20]	{UdtTypeName}
								*/
								#endregion

								count++;
							}

						} while (reader.NextResult());

						reader.Close();
					}
				}

				con.Close();
			}
		}


		#region Statement Parsing

		[TestMethod, Description("Database Statement Parsing")]
		public void ParseSimple ()
		{
			Console.WriteLine("-- ParseSimple ------------------------------------------------");

			StatementParser parser = new StatementParser();
			StatementCollection statements = parser.Parse("SELECT * FROM dual");
			Assert.AreEqual(1, statements.Count);
			Console.WriteLine("S1=[" + statements[0] + "]");

			statements = parser.Parse("SELECT * FROM dual;");
			Assert.AreEqual(1, statements.Count);
			Console.WriteLine("S2=[" + statements[0] + "]");

			statements = parser.Parse("\nSELECT * FROM dual;\n/\n");
			Assert.AreEqual(1, statements.Count);
			Console.WriteLine("S3=[" + statements[0] + "]");

			statements = parser.Parse("\nSELECT * FROM dual;\nEXIT;\n");
			Assert.AreEqual(1, statements.Count);
			Console.WriteLine("S4=[" + statements[0] + "]");

			statements = parser.Parse("\nSELECT * FROM dual;\n/ SELECT  * FROM other ; \n");
			Assert.AreEqual(2, statements.Count);
			Console.WriteLine("S5=[" + statements[0] + "]");
			Console.WriteLine("S5=[" + statements[1] + "]");
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseCombine ()
		{
			Console.WriteLine("-- ParseCombine -----------------------------------------------");

			StatementParser parser = new StatementParser();
			StatementCollection statements = parser.Parse(
				"SELECT 'a' FROM dual;\nSELECT 'b' FROM dual;\nSELECT 'c' FROM dual;");
			Assert.AreEqual(3, statements.Count);

			string combined = statements.Combine();
			Console.WriteLine("combined=[" + combined + "]");
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseSemiComplex ()
		{
			Console.WriteLine("-- ParseSemiComplex -------------------------------------------");

			string sql =
@"CREATE TABLE SEC_DirectoryNode
(
	nodeID       char(32)      NOT NULL,
	modVersion   int           DEFAULT 1 NOT NULL
);

ALTER TABLE SEC_DirectoryNode ADD
(
	CONSTRAINT PK_SEC_DirectoryNode PRIMARY KEY (nodeID)
);

CREATE INDEX IX_SEC_DirectoryNode_typeID
    ON SEC_DirectoryNode (typeID);";

			StatementParser parser = new StatementParser();
			StatementCollection statements = parser.Parse(sql);
			Assert.AreEqual(3, statements.Count);

			foreach (string s in statements)
			{
				Console.WriteLine("S=[" + s + "]");
			}
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseComplex1 ()
		{
			Console.WriteLine("-- ParseComplex1 ----------------------------------------------");

			string sql =
@"-- ***********************************************************************************************
-- Copyright © 2005 Waters Corporation. All Rights Reserved.
--
-- Everest Security Subsystem Licensing package declaration.
--
-- ***********************************************************************************************

CREATE OR REPLACE PACKAGE SEC_Licensing
AS
	SUBTYPE guid IS char(32);

	SystemLicenseID guid := '0000000000000000000000000000000F';


	-- ===========================================================================================
	-- DeleteLicense()
	--     Delete a license record.  The system license (licenseID = 1) cannot be deleted.
	-- ===========================================================================================
	
	PROCEDURE DeleteLicense (
	    p_licenseID IN char
	    );

END SEC_Licensing;
/";

			StatementParser parser = new StatementParser();
			StatementCollection statements = parser.Parse(sql);
			Assert.AreEqual(1, statements.Count);

			Console.WriteLine("S=[" + statements[0] + "]");
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseComplex2 ()
		{
			Console.WriteLine("-- ParseComplex2 ----------------------------------------------");

			string sql =
@"-- ***********************************************************************************************
-- Copyright © 2005 Waters Corporation. All Rights Reserved.
--
-- Everest Security Subsystem Licensing package implementation.
--
-- ***********************************************************************************************

CREATE OR REPLACE PACKAGE BODY SEC_Licensing
AS
	-- Forward declaration

	PROCEDURE SaveLicense (
	    p_licenseID  IN OUT char,
	    p_name       IN  varchar2,
	    p_license    IN  xmltype,
	    p_modifierID IN  char,
	    p_modVersion IN  int,
		p_newID		 OUT char
	    );


	-- ===========================================================================================
	-- DeleteLicense()
	-- ===========================================================================================

	PROCEDURE DeleteLicense (
	    p_licenseID IN char
	    )
	IS
	BEGIN
	  IF (p_licenseID > SystemLicenseID) THEN
	    BEGIN
	      DELETE SEC_License
	       WHERE licenseID = p_licenseID;
	    END;
	  END IF;
	END DeleteLicense;

END SEC_Licensing;
/";

			StatementParser parser = new StatementParser();
			StatementCollection statements = parser.Parse(sql);
			Assert.AreEqual(1, statements.Count);

			Console.WriteLine("S=[" + statements[0] + "]");
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseComplexOptimized ()
		{
			Console.WriteLine("-- ParseComplexOptimized --------------------------------------");

			string sql =
@"-- ***********************************************************************************************
-- Copyright © 2005 Waters Corporation. All Rights Reserved.
--
-- Everest Security Subsystem Licensing package implementation.
--
-- ***********************************************************************************************

CREATE OR REPLACE PACKAGE BODY SEC_Licensing
AS
	-- Forward declaration

	PROCEDURE SaveLicense (
	    p_licenseID  IN OUT char,
	    p_name       IN  varchar2,
	    p_license    IN  xmltype,
	    p_modifierID IN  char,
	    p_modVersion IN  int,
		p_newID		 OUT char
	    );


	-- ===========================================================================================
	-- DeleteLicense()
	-- ===========================================================================================

	PROCEDURE DeleteLicense (
	    p_licenseID IN char
	    )
	IS
	BEGIN
	  IF (p_licenseID > SystemLicenseID) THEN
	    BEGIN
	      DELETE SEC_License
	       WHERE licenseID = p_licenseID;
	    END;
	  END IF;
	END DeleteLicense;

END SEC_Licensing;
/";

			StatementParser parser = new StatementParser();
			parser.IsOptimized = true;
			StatementCollection statements = parser.Parse(sql);
			Assert.AreEqual(1, statements.Count);

			Console.WriteLine("S=[" + statements[0] + "]");
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseXmlType()
		{
			Console.WriteLine("-- ParseXmlType -----------------------------------------------");

			string sql = @"TEST';' TEST; TEST;";

			StatementParser parser = new StatementParser();
			parser.IsOptimized = true;
			StatementCollection statements = parser.Parse(sql);
			Assert.AreEqual(2, statements.Count);

			sql = 
@"INSERT INTO COR_Field
    (fieldID, fieldVersion, name, displayName, shortName, description, remarks, prefixResID,
    fieldType, fieldSource, sourceName, definition, valuePaths, processingStage, formulaID,
    isStandard, isCataloged, isSearchable, isRetrievable, isLocked, isObsolete)
VALUES
(
    'A91A2D673F9846689E2D4AA9B38D81C5',
    0,
    'BracketGroup',
    NULL,
    NULL,
    NULL,
    NULL,
    'BracketGroup,Waters.Lib.Resources,UDFResources',
    'String',
    'Class',
    NULL,
    xmltype('<definition>
  <columnNo>7</columnNo>
  <concatSeparator>;</concatSeparator>
</definition>'),
    xmltype('<valuePaths>
  <namespace prefix=""udf"">urn:www.waters.com/udf</namespace>
  <path purpose=""catalog"">/item/catalog/bracketGroup</path>
  <path purpose=""default"">quantitationProperties::bracketGroup</path>
  <path purpose=""udf"">quantitationProperties::bracketGroup</path>
  <path purpose=""xpath"">/udf:result/udf:sample/udf:propertyGroups/udf:quantitationProperties/udf:bracketGroup</path>
</valuePaths>'),
    NULL,
    '00000000000000000000000000000000',
    1,
    1,
    1,
    1,
    0,
    0
);";

			parser = new StatementParser();
			parser.IsOptimized = true;
			statements = parser.Parse(sql);
			Assert.AreEqual(1, statements.Count);

			Console.WriteLine("S=[" + statements[0] + "]");
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseWrap ()
		{
			Console.WriteLine("-- ParseWrap --------------------------------------------------");

			StatementParser parser = new StatementParser();
			StatementCollection statements = parser.Parse(
				"SELECT 'a' FROM dual;\nSELECT 'b' FROM dual;\nSELECT 'c' FROM dual;");
			Assert.AreEqual(3, statements.Count);

			string wrapped = statements.Wrap("DefaultSchema");
			Assert.AreEqual(3, statements.Parameters.Count);

			Console.WriteLine("wrapped=[" + wrapped + "]");
		}


		[TestMethod, Description("Database Statement Parsing")]
		public void ParseNotifications ()
		{
			Console.WriteLine("-- ParseNotifications -----------------------------------------");

			StatementParser parser = new StatementParser();
			parser.ParseNotification += new NotificationEventHandler(parser_ParseNotification);
			StatementCollection statements = parser.Parse(
				"SELECT 'a' FROM dual;\nSELECT 'b' FROM dual; -- foo\nSELECT 'c' FROM dual;");
			Assert.AreEqual(3, statements.Count);

			Console.WriteLine("S0=[" + statements[0] + "]");
			Console.WriteLine("S1=[" + statements[1] + "]");
			Console.WriteLine("S2=[" + statements[2] + "]");
		}


		//[TestMethod, Description("Database Statement Parsing")]
		[Ignore]
		public void ParseType ()
		{
			//Console.WriteLine("-- ParseStatement --------------------------------------------------");

			//StatementParser parser = new StatementParser();
			//parser.ParseNotification += new NotificationEventHandler(parser_ParseNotification);

			//var pwd = ConfigurationManager.AppSettings["everest_password"];
			//Assert.IsNotNull(pwd, "Must set everest_password in appSettings");

			//DatabaseConnection dbase = new DatabaseConnection("everest", pwd);

			//QueryType qtype;
			//StatementType stype = parser.ParseStatement(
			//    dbase,
			//    "SELECT 123 FROM dual",
			//    out qtype
			//    );

			//Console.WriteLine("QueryType=[" + qtype + "] StatementType=[" + stype + "]");
		}


		private void parser_ParseNotification (NotificationEventArgs e)
		{
			Console.WriteLine(e.Message.Type.ToString() + " [" + e.Message.Text + "]");
		}

		#endregion Statement Parsing
	}
}
