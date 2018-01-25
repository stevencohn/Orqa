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


	//********************************************************************************************
	// class OraTable
	//********************************************************************************************

	internal class OraTable : List<OraRow>
	{

		public OraSchema Schema;


		public int FieldCount
		{
			get
			{
				return Schema.FieldCount;
			}
		}
	}
}
