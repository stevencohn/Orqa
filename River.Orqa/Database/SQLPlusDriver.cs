//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Parameter information.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 25-Aug-2013      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;


	//********************************************************************************************
	// class SqlPlusDriver
	//********************************************************************************************

	internal class SqlPlusDriver : CmdDriver
	{
		public const int MissingSqlPlus = int.MaxValue - 100;
		public const int MissingScriptFile = int.MaxValue - 101;

		private DatabaseConnection dbase;


		//========================================================================================
		// Lifecycle
		//========================================================================================

		public SqlPlusDriver (DatabaseConnection dbase)
			: base()
		{
			this.dbase = dbase;
		}


		public SqlPlusDriver (
			DatabaseConnection dbase, DataReceivedEventHandler handler, int timeout = 0)
			: base(handler, timeout)
		{
			this.dbase = dbase;
		}


		public SqlPlusDriver (DatabaseConnection dbase, string outputFilename)
			: base(outputFilename)
		{
			this.dbase = dbase;
		}


		//========================================================================================
		// Methods
		//========================================================================================

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command">The text block of SQL script to run (not a file name)</param>
		/// <param name="arguments">Optional arguments; <i>Not used.</i></param>
		/// <returns></returns>

		public override int Run (string scriptFile, params string[] arguments)
		{
			string sqlplus = FindSqlPlus();
			if (sqlplus == null)
			{
				return MissingSqlPlus;
			}

			string script = GenerateScriptFile(scriptFile);
			if (script == null)
			{
				// signal invalid sql file
				return MissingScriptFile;
			}

			base.environment["NLS_LANG"] = ".AL32UTF8";

			// arguments
			var args = new List<String>();

			// suppress SQLPLUS banner, prompts, and commands
			args.Add("-S");

			if (dbase.Mode.Equals("Normal"))
			{
				args.Add(dbase.UserID + "/" + dbase.Password);
			}
			else
			{
				args.Add(dbase.UserID + "/" + dbase.Password + " as " + dbase.Mode);
			}

			args.Add("@\"" + script + "\"");

			if ((arguments != null) && (arguments.Length > 0))
			{
				args.AddRange(arguments);
			}

			int exitcode = base.Run(sqlplus, args.ToArray());
			return exitcode;
		}


		private string FindSqlPlus ()
		{
			string path = Path.Combine(DatabaseSetup.OracleHome, @"bin\sqlplus.exe");
			if (!File.Exists(path))
			{
				return null;
			}

			return path;
		}


		// Copy the scriptFile contents to a new randomly-named temporary file
		// ensuring that the SQL ends with an "exit;" statement

		private string GenerateScriptFile (string scriptFile)
		{
			string script = null;
			if (File.Exists(scriptFile))
			{
				string sql = File.ReadAllText(scriptFile).Trim();

				script = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".sql");
				using (var writer = new StreamWriter(script, false))
				{
					writer.WriteLine("set serveroutput on;");
					writer.WriteLine(sql);
					writer.WriteLine("/");
					writer.WriteLine("exit;");
				}
			}

			return script;
		}
	}
}