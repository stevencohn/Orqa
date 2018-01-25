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
	using Oracle.ManagedDataAccess.Client;


	//********************************************************************************************
	// class OraColumn
	//********************************************************************************************

	internal class OraColumn
	{
		public string ColumnName;
		public Type DataType;
		public Type ProviderType;
		public OracleDbType DbType;
	}
}
