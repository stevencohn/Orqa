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


	//********************************************************************************************
	// enum SchemataTypes
	//********************************************************************************************

	internal enum SchemataTypes
	{
		Column,
		Constraint,
		Empty,
		Folder,
		Function,
		Index,
		Package,
		PackageBody,
		Parameter,
		Procedure,
		Schemata,
		Sequence,
		SystemTable,
		Table,
		UserTable,
		View
	}
}
