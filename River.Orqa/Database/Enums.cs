//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Enumerates enumerations.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Jul-2005      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;


	//********************************************************************************************
	// enum ErrorLevel
	//********************************************************************************************

	internal enum ErrorLevel
	{
		Ignore,								// ignore all errors and continue execution
		Weak,								// show error messages but continue execution
		Strict								// terminate execute immediately upon first error
	}


	//********************************************************************************************
	// enum ParseMode
	//********************************************************************************************

	/// <summary>
	/// Indicates how multiple statements should be parsed and invoked.
	/// </summary>

	internal enum ParseMode
	{
		Prompt = 0,							// do not parse multiple statements
		Sequential = 1,						// execute each autonomously in sequence
		Block = 2,							// execute all as an anonymous begin-end block
		Wrapped = 3,						// execute in a wrapped stored procedure
		SqlPlus = 4,						// execute using SQL*Plus subprocess
		Repeat = 5,							// execute statement specified number of times
		None								// unspecified
	}


	//********************************************************************************************
	// enum QueryType
	//********************************************************************************************

	internal enum QueryType
	{
		Ignored,							// ignored, e.g. EXIT
		Invalid,							// exception while parsing
		Reader,								// returns recordset
		Nonreader,							// no return data
		ParsedProcedure,					// query with Parameters
		Procedure,							// procedure, recordset?
		Script,								// external script file
		Set,								// internal set command
		SqlPlus,							// using SQL*Plus subprocess
		Unknown								// unrecognized, skip
	}


	//********************************************************************************************
	// enum ResultFormat
	//********************************************************************************************

	internal enum ResultFormat
	{
		ColumnAligned = 0,
		CommaDelimited = 1,
		TabDelimited = 2,
		SpaceDelimited = 3,
		CustomDelimited = 4
	}


	//********************************************************************************************
	// enum ResultTarget
	//********************************************************************************************

	internal enum ResultTarget
	{
		Text = 0,
		Grid = 1,
		Xml = 2
	}


	//********************************************************************************************
	// enum StatementType
	//********************************************************************************************

	/// <summary>
	/// Parallels the internal System.Data.OracleClient.OCI.STMT enumerator
	/// </summary>

	internal enum StatementType
	{
		Alter = 7,
		Begin = 8,
		Create = 5,
		Declare = 9,
		Delete = 3,
		Drop = 6,
		Explain = 15,
		Insert = 4,
		Select = 1,
		Update = 2,
		Unknown = -1
	}


	//********************************************************************************************
	// enum StatisticClass
	//********************************************************************************************

	[Flags]
	internal enum StatisticClass
	{
		User = 1,
		Redo = 2,
		Enqueue = 4,
		Cache = 8,
		OS = 16,
		Parallel = 32,
		SQL = 64,
		Debug = 128
	}
}
