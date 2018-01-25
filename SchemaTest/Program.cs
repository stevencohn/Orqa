namespace SchemaTest
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;
	using System.IO;
	using Oracle.ManagedDataAccess.Client;


	class Program
	{
		static void Main (string[] args)
		{
			var pwd = ConfigurationManager.AppSettings["everest_password"];

			string dsn = $"User Id=everest;Password={pwd};Data Source=UNIFI";
			string path = @"..\..\..\doc\Schema";
			string ProviderName = path + @"\Everest";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			using (var conn = new OracleConnection(dsn))
			{
				try
				{
					conn.Open();

					//DataTable schema = conn.GetSchema();  // same as MetaDataCollections
					//schema.WriteXml(ProviderName + ".xml");

					//Get MetaDataCollections and write to an XML file.
					//This is equivalent to GetSchema()
					DataTable dtMetadata = conn.GetSchema(DbMetaDataCollectionNames.MetaDataCollections);
					dtMetadata.WriteXml(ProviderName + "_MetaDataCollections.xml");

					//Get Restrictions and write to an XML file.
					DataTable dtRestrictions = conn.GetSchema(DbMetaDataCollectionNames.Restrictions);
					dtRestrictions.WriteXml(ProviderName + "_Restrictions.xml");

					//Get DataSourceInformation and write to an XML file.
					DataTable dtDataSrcInfo = conn.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
					dtDataSrcInfo.WriteXml(ProviderName + "_DataSourceInformation.xml");

					//data types and write to an XML file.
					DataTable dtDataTypes = conn.GetSchema(DbMetaDataCollectionNames.DataTypes);
					dtDataTypes.WriteXml(ProviderName + "_DataTypes.xml");

					//Get ReservedWords and write to an XML file.
					DataTable dtReservedWords = conn.GetSchema(DbMetaDataCollectionNames.ReservedWords);
					dtReservedWords.WriteXml(ProviderName + "_ReservedWords.xml");

					string[] restrictions = new string[] { "EVEREST" };
					DataTable table;

					foreach (DataRow row in dtMetadata.Rows)
					{
						Console.Write(row["CollectionName"] + "...");

						int numberOfRestrictions = (int)row["NumberOfRestrictions"];

						try
						{
							if (numberOfRestrictions > 1)
								table = conn.GetSchema((string)row["CollectionName"], restrictions);
							else
								table = conn.GetSchema((string)row["CollectionName"]);

							table.WriteXml(ProviderName + "_" + row["CollectionName"] + ".xml");
							Console.WriteLine("OK");
						}
						catch
						{
							Console.WriteLine("ERROR");
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
			}
		}
	}
}
