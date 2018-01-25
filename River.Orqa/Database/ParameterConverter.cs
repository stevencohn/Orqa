
namespace River.Orqa.Database
{
	using System;
	using System.Collections.Generic;
	using Oracle.ManagedDataAccess.Client;
	using Oracle.ManagedDataAccess.Types;


	internal static class ParameterConverter
	{
		public class CustomType
		{
			private string name;
			private Type type;
			private OracleDbType oratype;

			public CustomType (string name, Type type, OracleDbType oratype)
			{
				this.name = name;
				this.type = type;
				this.oratype = oratype;
			}

			public string Name { get { return name; } }
			public Type Type { get { return type; } }
			public OracleDbType OraType { get { return oratype; } }
		}


		public static CustomType Convert (string oratype)
		{
			CustomType custom = null;

			switch (oratype)
			{
				case "anydata":
					custom = new CustomType("anydata", typeof(string), OracleDbType.NVarchar2);
					break;

				case "anytype":
					custom = new CustomType("anytype", typeof(string), OracleDbType.NVarchar2);
					break;

				case "bfile":
					custom = new CustomType("bfile", typeof(string), OracleDbType.BFile);
					break;

				case "binary_double":
					custom = new CustomType("binary double", typeof(double), OracleDbType.Double);
					break;

				case "binary_float":
					custom = new CustomType("binary float", typeof(float), OracleDbType.Decimal);
					break;

				case "binary_integer":
					custom = new CustomType("binary integer", typeof(int), OracleDbType.Int32);
					break;

				case "blob":
					custom = new CustomType("blob", typeof(string), OracleDbType.Blob);
					break;

				case "boolean":
					custom = new CustomType("boolean", typeof(bool), OracleDbType.Int32);
					break;

				case "char":
					// set the type to 'string' to allow for any number of chars
					custom = new CustomType("char", typeof(string), OracleDbType.Char);
					break;

				case "clob":
					custom = new CustomType("clob", typeof(string), OracleDbType.Clob);
					break;

				case "date":
					custom = new CustomType("date", typeof(DateTime), OracleDbType.Date);
					break;

				case "double precision":
					custom = new CustomType("double", typeof(double), OracleDbType.Double);
					break;

				case "float":
					custom = new CustomType("float", typeof(float), OracleDbType.Decimal);
					break;

				case "integer":
					custom = new CustomType("integer", typeof(int), OracleDbType.Int32);
					break;

				case "interval day to second":
					custom = new CustomType("interval ds", typeof(int), OracleDbType.IntervalDS);
					break;

				case "interval year to month":
					custom = new CustomType("interval ym", typeof(int), OracleDbType.IntervalYM);
					break;

				case "long":
					custom = new CustomType("long", typeof(long), OracleDbType.Long);
					break;

				case "long raw":
					custom = new CustomType("long raw", typeof(string), OracleDbType.LongRaw);
					break;

				case "mlslabel":
					custom = new CustomType("mlslabel", typeof(string), OracleDbType.Varchar2);
					break;

				case "number":
					custom = new CustomType("number", typeof(int), OracleDbType.Int32);
					break;

				case "object":
					custom = new CustomType("object", typeof(object), OracleDbType.NClob);
					break;

				case "pl/sql record":
					custom = new CustomType("record", typeof(object), OracleDbType.RefCursor);
					break;

				case "pl/sql table":
					custom = new CustomType("table", typeof(object), OracleDbType.RefCursor);
					break;

				case "pls_integer":
					custom = new CustomType("pls_integer", typeof(int), OracleDbType.Int32);
					break;

				case "raw":
					custom = new CustomType("raw", typeof(string), OracleDbType.Raw);
					break;

				case "real":
					custom = new CustomType("real", typeof(decimal), OracleDbType.Decimal);
					break;

				case "ref":
					custom = new CustomType("ref", typeof(object), OracleDbType.RefCursor);
					break;

				case "ref cursor":
					custom = new CustomType("ref cursor", typeof(object), OracleDbType.RefCursor);
					break;

				case "rowid":
					custom = new CustomType("rowid", typeof(int), OracleDbType.Int32);
					break;

				case "smallint":
					custom = new CustomType("smallint", typeof(short), OracleDbType.Int16);
					break;

				case "standard":
					custom = new CustomType("standard", typeof(int), OracleDbType.Single);
					break;

				case "string":
					custom = new CustomType("string", typeof(string), OracleDbType.NVarchar2);
					break;

				case "table":
					custom = new CustomType("table", typeof(object), OracleDbType.RefCursor);
					break;

				case "time":
					custom = new CustomType("time", typeof(DateTime), OracleDbType.Date);
					break;

				case "time with time zone":
					custom = new CustomType("time_tz", typeof(DateTime), OracleDbType.TimeStampTZ);
					break;

				case "timestamp":
					custom = new CustomType("timestamp", typeof(DateTime), OracleDbType.TimeStamp);
					break;

				case "timestamp with local time zone":
					custom = new CustomType("timestamp_ltz", typeof(DateTime), OracleDbType.TimeStampLTZ);
					break;

				case "timestamp with time zone":
					custom = new CustomType("timestamp_tz", typeof(DateTime), OracleDbType.TimeStampTZ);
					break;

				case "urowid":
					custom = new CustomType("urowid", typeof(int), OracleDbType.Int32);
					break;

				case "varchar2":
					custom = new CustomType("varchar2", typeof(string), OracleDbType.Varchar2);
					break;

				case "varray":
					custom = new CustomType("varray", typeof(string), OracleDbType.NVarchar2);
					break;

				case "xmltype":
					custom = new CustomType("xmltype", typeof(string), OracleDbType.XmlType);
					break;
			}

			return custom;
		}
	}
}
