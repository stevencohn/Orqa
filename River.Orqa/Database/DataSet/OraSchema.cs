//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 31-Aug-2013      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections.Generic;
	using System.Data.Common;
	using Oracle.ManagedDataAccess.Client;
	using Oracle.ManagedDataAccess.Types;


	//********************************************************************************************
	// class OraSchema
	//********************************************************************************************
	
	internal class OraSchema
	{

		private List<OraColumn> columns;


		//========================================================================================
		// Lifecycle
		//========================================================================================

		public OraSchema (DbDataReader reader)
		{
			this.columns = new List<OraColumn>();

			for (int i = 0; i < reader.FieldCount; i++)
			{
				var column = new OraColumn();
				column.ColumnName = reader.GetName(i);
				column.DataType = reader.GetFieldType(i);
				column.ProviderType = reader.GetProviderSpecificFieldType(i);

				if (column.ProviderType == typeof(OracleBFile))
				{
					column.DbType = OracleDbType.BFile;
				}
				else if (column.ProviderType == typeof(OracleBinary))
				{
					column.DbType = column.DataType == typeof(Double)
						? OracleDbType.BinaryDouble
						: OracleDbType.BinaryFloat;
				}
				else if (column.ProviderType == typeof(OracleBlob))
				{
					column.DbType = OracleDbType.Blob;
				}
				else if (column.ProviderType == typeof(OracleClob))
				{
					column.DbType = OracleDbType.Clob;
				}
				else if (column.ProviderType == typeof(OracleDate))
				{
					column.DbType = OracleDbType.Date;
				}
				else if (column.ProviderType == typeof(OracleDecimal))
				{
					column.DbType = OracleDbType.Decimal;
				}
				else if (column.ProviderType == typeof(OracleIntervalDS))
				{
					column.DbType = OracleDbType.IntervalDS;
				}
				else if (column.ProviderType == typeof(OracleIntervalYM))
				{
					column.DbType = OracleDbType.IntervalYM;
				}
				else if (column.ProviderType == typeof(OracleRefCursor))
				{
					column.DbType = OracleDbType.RefCursor;
				}
				else if (column.ProviderType == typeof(OracleString))
				{
					column.DbType = OracleDbType.Varchar2;
				}
				else if (column.ProviderType == typeof(OracleTimeStamp))
				{
					column.DbType = OracleDbType.TimeStamp;
				}
				else if (column.ProviderType == typeof(OracleTimeStampLTZ))
				{
					column.DbType = OracleDbType.TimeStampLTZ;
				}
				else if (column.ProviderType == typeof(OracleTimeStampTZ))
				{
					column.DbType = OracleDbType.TimeStampTZ;
				}
				// TODO: ManagedDataAccess does not include this type
				//else if (column.ProviderType == typeof(OracleUdt))
				//{
				//	column.DbType = OracleDbType.Object;
				//}
				else if (column.ProviderType == typeof(OracleXmlStream))
				{
					column.DbType = OracleDbType.NClob;
				}
				else if (column.ProviderType == typeof(OracleXmlType))
				{
					column.DbType = OracleDbType.XmlType;
				}

				columns.Add(column);
			}
		}


		//========================================================================================
		// Methods
		//========================================================================================

		public int FieldCount
		{
			get { return columns.Count; }
		}


		public string GetName (int i)
		{
			return columns[i].ColumnName;
		}


		public OracleDbType GetDbType (int i)
		{
			return columns[i].DbType;
		}


		public Type GetType (int i)
		{
			return columns[i].DataType;
		}


		public Type GetProvidertype (int i)
		{
			return columns[i].ProviderType;
		}

		public OraColumn this[int i]
		{
			get { return columns[i]; }
		}
	}
}
