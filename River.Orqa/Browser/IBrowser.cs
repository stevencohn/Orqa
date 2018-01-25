//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;


	internal delegate SchemataParameter[] FindProcDelegate (string procnam);

	internal interface IBrowser
	{
		SchemataParameter[] FindProcParameters (string procName);

		object Invoke (Delegate method, params object[] args);
	}
}
