//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Executes all statements found in the provided text block and collect results
// and informational messages for reference.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Aug-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Data;
	using System.IO;
	using System.Text;
	using System.Threading;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Options;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class QueryDriver
	//********************************************************************************************

	/// <summary>
	/// Executes all statements found in the provided text block and collect
	/// results and informational messages for reference.
	/// </summary>

	internal class QueryDriver
	{
		private static string RsxUnrecognizedQueryType;	// resource string
		private static string RsxStatementIgnored;		// resource string

		private static int driverCount;					// sequential ID for thread identification

		private int driverID;							// this driverID
		private DatabaseConnection dbase;				// connection use by this driver
		private River.Orqa.Browser.IBrowser browser;	// reference to browser
		private string basePath;						// directory of script files
		private Query query;							// current query, for message capture

		private bool isExecuting;						// true if currently executing (async)
		private bool isCancelled;						// true if cancelled all queries
		private bool isNested;							// true if nesting with external scripts
		private bool isOutputEnabled;					// true if dbms_output is enabled
		private int timeout;							// max time allowed for query (useropt)

		private System.Threading.Thread worker;			// the worker thread
		private System.Threading.Timer timer;			// the timeout timer

		private class Chassis
		{
			public QueryCollection queries;
			public int repeat;
		}


		// Events

		public event QueryCompletedEventHandler QueryCompleted;
		public event QueriesCompletedEventHandler QueriesCompleted;


		//========================================================================================
		// Constructor
		//========================================================================================

		static QueryDriver ()
		{
			var translator = new Translator("Query");
			RsxUnrecognizedQueryType = translator.GetString("UnrecognizedQueryType");
			RsxStatementIgnored = translator.GetString("StatementIgnored");

			driverCount = 0;
		}


		/// <summary>
		/// Initializes a new query driver for the specified connection.  A single
		/// driver is created for each QueryWindow.
		/// </summary>

		public QueryDriver (DatabaseConnection dbase, River.Orqa.Browser.IBrowser browser)
		{
			this.driverID = ++QueryDriver.driverCount;
			this.dbase = dbase;
			this.browser = browser;
			this.isOutputEnabled = UserOptions.GetBoolean("results/general/dbmsOutput");

			Reset();
		}


		/// <summary>
		/// Reinitializes this driver and all of its internal statistics and measurements
		/// for the next set of queries.
		/// </summary>

		public void Reset ()
		{
			this.worker = null;
			this.timer = null;

			this.isExecuting = false;
			this.isCancelled = false;
			this.isNested = false;
			this.timeout = int.MaxValue;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public bool IsCancelled
		{
			get { return isCancelled; }
		}


		/// <summary>
		/// Gets a value indicating if this driver is currently executing a statement.
		/// A driver can executing one statement at a time.
		/// </summary>

		public bool IsExecuting
		{
			get { return isExecuting; }
		}


		//========================================================================================
		// Cancel()
		//========================================================================================

		/// <summary>
		/// Cancels executing multiple statements.  The currently executing query attempts
		/// to abort but may complete before the Cancel takes effect.
		/// </summary>

		public void Cancel ()
		{
			if (worker != null)
			{
				worker.Abort();
				worker = null;
			}

			timer = null;

			isExecuting = false;
			isCancelled = true;
		}


		// Internal timeout callback

		private void Cancel (object state)
		{
			Cancel();
		}


		//========================================================================================
		// Execute()
		//========================================================================================

		/// <summary>
		/// Executes the specified query.  Results and error messages are stored in
		/// the query itself.
		/// </summary>
		/// <param name="query">The Query describing the SQL statement.</param>

		public void Execute (QueryCollection queries, string basePath, int repeat)
		{
			Logger.WriteLine("QueryDriver.Execute count=[" + queries.Count + "]");

			// fetch this each time incase the user changes it.
			this.timeout = UserOptions.GetInt("connections/queryTimeout") * 1000;

			this.isExecuting = true;
			this.basePath = basePath;

			try
			{
				Logger.WriteLine("QueryDriver.Execute Starting driver...");

				worker = new Thread(new ParameterizedThreadStart(Execute));
				worker.Name = String.Format("QueryDriver{0}", driverID);
				worker.IsBackground = true;

				timer = new System.Threading.Timer(
					new TimerCallback(Cancel),
					worker,
					timeout,
					Timeout.Infinite
					);

				var chassis = new Chassis();
				chassis.queries = queries;
				chassis.repeat = repeat;

				worker.Start(chassis);
			}
			catch (Exception exc)
			{
				Dialogs.ExceptionDialog.ShowException(exc);
			}
		}


		//========================================================================================
		// Execute()
		//		This is the thread worker routine.
		//========================================================================================

		private void Execute (object chassisObj)
		{
			Chassis chassis = (Chassis)chassisObj;

			if (UserOptions.RunStatistics)
			{
				chassis.queries.Statistics = new Statistics(dbase);
				chassis.queries.Statistics.Initialize();
			}

			var infoHandler = new OracleInfoMessageEventHandler(CaptureInfoMessage);
			var stateHandler = new StateChangeEventHandler(CaptureStateChange);

			OracleConnection con = dbase.OraConnection;
			con.InfoMessage += infoHandler;
			con.StateChange += stateHandler;

			try
			{
				QueryCollection.Enumerator qe;

				while ((chassis.repeat-- > 0) && !isCancelled)
				{
					qe = chassis.queries.GetEnumerator();
					while (qe.MoveNext() && !isCancelled)
					{
						ExecuteQuery(query = qe.Current);

						if (query.AffectedRecords > 0)
							chassis.queries.TotalRecords += query.AffectedRecords;

						chassis.queries.TotalTicks += query.Ticks;
					}
				}

				if (UserOptions.RunStatistics)
				{
					chassis.queries.Statistics.Summarize();
				}
			}
			finally
			{
				this.isExecuting = false;

				if (QueriesCompleted != null)
				{
					QueriesCompleted(new QueriesCompletedEventArgs(chassis.queries));
				}

				con.InfoMessage -= infoHandler;
				con.StateChange -= stateHandler;
			}
		}


		//========================================================================================
		// ExecuteQuery()
		//========================================================================================

		private void ExecuteQuery (Query query)
		{
			try
			{
				if (UserOptions.RunExplainPlan)
				{
					if (query.CanExplain)
					{
						Explain(query);
					}
				}
				else
				{
					switch (query.QueryType)
					{
						case QueryType.Reader:
							ExecuteReader(query);
							break;

						case QueryType.Nonreader:
							ExecuteNonReader(query);
							break;

						case QueryType.ParsedProcedure:
							ExecuteProcedure(query);
							break;

						case QueryType.Procedure:
							ExecuteProcedure(query);
							break;

						case QueryType.Script:
							ExecuteExternalScript(query);
							break;

						case QueryType.SqlPlus:
							ExecuteSqlPlus(query);
							break;

						case QueryType.Unknown:
							ReportUknown(query);
							break;

						case QueryType.Ignored:
							ReportIgnored(query);
							break;
					}
				}
			}
			finally
			{
				if (!isNested)
				{
					// stop the timeout timer
					timer = null;
				}

				// notify the query window to report results
				if (QueryCompleted != null)
				{
					QueryCompleted(new QueryCompletedEventArgs(query));
				}
			}
		}


		//========================================================================================
		// ExecuteNonReader()
		//========================================================================================

		private void ExecuteNonReader (Query query)
		{
			try
			{
				if (isOutputEnabled)
				{
					SetServerOutputOn();
				}

				int ticks = 0;

				using (var cmd = new OracleCommand())
				{
					cmd.CommandText = query.SQL;
					cmd.CommandType = CommandType.Text;
					cmd.CommandTimeout = timeout;
					cmd.Connection = dbase.OraConnection;

					ticks = Environment.TickCount;

					query.AffectedRecords = cmd.ExecuteNonQuery();
				}

				query.Ticks = Environment.TickCount - ticks;

				if (isOutputEnabled)
				{
					GetOutputLines(query);
				}
			}
			catch (OracleException exc)
			{
				query.AddMessage(exc);
			}
			catch (Exception exc)
			{
				query.AddMessage(exc);
			}
		}


		//========================================================================================
		// ExecuteReader()
		//========================================================================================

		private void ExecuteReader (Query query)
		{
			var cmd = new OracleCommand();
			cmd.CommandText = query.SQL;
			cmd.CommandType = CommandType.Text;
			cmd.CommandTimeout = timeout;
			cmd.Connection = dbase.OraConnection;

			ExecuteCommand(query, cmd);
		}


		private void ExecuteCommand (Query query, OracleCommand cmd)
		{
			/*
			 * 1. read a record
			 * 2. if first record of result (get schema, signal StartResult)
			 * 3. fetch values
			 * 4. signal Row
			 * 5. if no more (signal EndResult)
			 * 6. if next result goto 1
			 * 
			 */
			try
			{
				int ticks = Environment.TickCount;

				using (OracleDataReader reader = cmd.ExecuteReader())
				{
					do
					{
						var table = new OraTable();
						int count = 0;

						while (!isCancelled && reader.Read())
						{
							if (count == 0)
							{
								table.Schema = new OraSchema(reader);
								// EVENT StartResult(query.SQL,schema)
							}

							var row = new OraRow();
							for (int i = 0; i < reader.FieldCount; i++)
							{
								switch (table.Schema.GetDbType(i))
								{
									case OracleDbType.BFile:
									case OracleDbType.Blob:
									case OracleDbType.Clob:
									case OracleDbType.NClob:
									case OracleDbType.Raw:
										// we treat null differently than DbNull
										// when we interpret the results later on...
										row.Add(null);
										break;

									default:
										row.Add(reader.GetValue(i));
										break;
								}
							}

							table.Add(row);

							// EVENT Row(row)

							count++;
						}

						query.Data.Add(table);
						query.AffectedRecords = table.Count;

						// EVENT EndResult(count)

					} while (!isCancelled && reader.NextResult());

					reader.Close();
				}

				query.Ticks = Environment.TickCount - ticks;

				if (isOutputEnabled)
				{
					GetOutputLines(query);
				}
			}
			catch (OracleException exc)
			{
				query.AddMessage(exc);

				if (exc.Message == String.Empty)
				{
					Logger.WriteLine(exc.StackTrace);
				}
				else
				{
					Logger.WriteLine(exc.Message);
				}
			}
			catch (Exception exc)
			{
				query.AddMessage(exc);

				if (exc.Message == String.Empty)
				{
					Logger.WriteLine(exc.StackTrace);
				}
				else
				{
					Logger.WriteLine(exc.Message);
				}
			}
		}


		//========================================================================================
		// ExecuteProcedure()
		//		Called directly from the schema browser when opening a procedure; in which
		//		case the parameters list is generated from RunProcedureDialog.
		//		Also called from InvokeProcedure which grabs a SQL statement from the
		//		query window containing an EXEC command.
		//========================================================================================

		private void ExecuteProcedure (Query query)
		{
			var cmd = new OracleCommand();
			cmd.Connection = dbase.OraConnection;
			cmd.CommandText = query.SQL;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = timeout;

			OracleParameter parameter;

			foreach (Parameter p in query.Parameters)
			{
				parameter = new OracleParameter();

				parameter.ParameterName = p.Name;
				parameter.OracleDbType = p.DataType;
				parameter.Direction = p.Direction;

				if (p.Value == System.DBNull.Value)
					parameter.Value = "null";
				else
					parameter.Value = p.Value;

				if ((p.Direction & (ParameterDirection.Output | ParameterDirection.ReturnValue)) > 0)
				{
					if ((parameter.OracleDbType == OracleDbType.Char) ||
						(parameter.OracleDbType == OracleDbType.NChar) ||
						(parameter.OracleDbType == OracleDbType.Varchar2) ||
						(parameter.OracleDbType == OracleDbType.NVarchar2))
					{
						// reserve enough space for most data!
						parameter.Size = 1000;
					}

					query.HasOutputs = true;
				}

				cmd.Parameters.Add(parameter);
			}

			var infoHandler = new OracleInfoMessageEventHandler(CaptureInfoMessage);
			var stateHandler = new StateChangeEventHandler(CaptureStateChange);
			dbase.OraConnection.InfoMessage += infoHandler;
			dbase.OraConnection.StateChange += stateHandler;

			try
			{
				ExecuteCommand(query, cmd);

				if (query.HasOutputs)
				{
					for (int i = 0; i < cmd.Parameters.Count; i++)
					{
						parameter = cmd.Parameters[i];
						if ((parameter.Direction &
							(ParameterDirection.Output | ParameterDirection.ReturnValue)) > 0)
						{
							query.Parameters[i].Value = parameter.Value;
						}
					}
				}
			}
			catch (OracleException exc)
			{
				query.AddMessage(exc);

				if (exc.Message == String.Empty)
					Logger.WriteLine(exc.StackTrace);
				else
					Logger.WriteLine(exc.Message);
			}
			catch (Exception exc)
			{
				query.AddMessage(exc);

				if (exc.Message == String.Empty)
					Logger.WriteLine(exc.StackTrace);
				else
					Logger.WriteLine(exc.Message);
			}
			finally
			{
				dbase.OraConnection.InfoMessage -= infoHandler;
				dbase.OraConnection.StateChange -= stateHandler;
			}
		}


		//========================================================================================
		// ExecuteExternalScript()
		//========================================================================================

		private void ExecuteExternalScript (Query query)
		{
			isNested = true;

			try
			{
				string filnam = query.SQL.Substring(1);
				if (filnam[filnam.Length - 1] == ';')
					filnam = filnam.Substring(0, filnam.Length - 1);

				if (Path.GetDirectoryName(filnam) == String.Empty)
				{
					filnam = basePath + "\\" + filnam;
				}

				StreamReader reader = File.OpenText(filnam);
				string text = reader.ReadToEnd().Trim();
				reader.Close();

				if (text.Length == 0)
					return;

				string savePath = basePath;
				basePath = Path.GetDirectoryName(filnam);

				// create nested query collection processing...

				var queries = new QueryCollection();
				var parser = new StatementParser();
				StatementCollection statements = parser.Parse(text);

				// build collection of parsed queries
				Query q;
				System.Collections.Specialized.StringEnumerator e = statements.GetEnumerator();
				while (e.MoveNext())
				{
					q = new Query(e.Current);
					parser.ParseStatement(dbase, q, browser);
					queries.Add(q);
				}
				//[end create nested]

				// execute query collection

				try
				{
					foreach (Query q2 in queries)
					{
						ExecuteQuery(q2);
					}
				}
				catch (Exception exc)
				{
					Dialogs.ExceptionDialog.ShowException(exc);
				}
				finally
				{
					basePath = savePath;
				}
			}
			catch (Exception exc)
			{
				query.AddMessage(exc);
			}

			isNested = false;
		}


		//========================================================================================
		// ExecuteSqlPlus()
		//========================================================================================

		private void ExecuteSqlPlus (Query query)
		{
			// generate SQL file from query SQL
			string sqlfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Logger.WriteLine("SQL*Plus script is " + sqlfile);
			using (var writer = new StreamWriter(sqlfile))
			{
				writer.WriteLine(query.SQL);
			}

			// capture output to tmpfile
			string tmpfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Logger.WriteLine("SQL*Plus output is " + tmpfile);

			// run the query
			var driver = new SqlPlusDriver(dbase, tmpfile);
			int exitcode = driver.Run(sqlfile);

			// now stream tmpfile to report file to untabify
			string report = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Logger.WriteLine("report outfile is " + report);

			using (var writer = new StreamWriter(report, false))
			{
				writer.WriteLine("set linesize 120;");
				writer.WriteLine("set serveroutput on;");

				using (var reader = new StreamReader(tmpfile))
				{
					string line;
					while (!reader.EndOfStream)
					{
						line = reader.ReadLine().TrimEnd();
						if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
						{
							writer.WriteLine();
						}
						else
						{
							if (line.StartsWith("ERROR at") || line.StartsWith("ORA-"))
							{
								query.AddMessage(line);
							}

							writer.WriteLine(line.Untabify(8));
						}
					}
				}
			}

			query.OutputFilename = report;
		}


		//========================================================================================
		// SetServerOutputOn()
		// GetOutputLines()
		//========================================================================================

		private void SetServerOutputOn ()
		{
			using (OracleCommand cmd = new OracleCommand())
			{
				cmd.CommandText = "dbms_output.enable";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandTimeout = timeout;
				cmd.Connection = dbase.OraConnection;

				// do not try/catch here... let it bubble up
				cmd.ExecuteNonQuery();
			}
		}


		private void GetOutputLines (Query query)
		{
			string lines = null;

			string sql = "declare"
				+ "  isDone number;"
				+ "  line varchar2(255);"
				+ "  buffer long;"
				+ "begin"
				+ "  loop"
				+ "    exit when isDone = 1;"
				+ "    dbms_output.get_line(line, isDone);"
				+ "    buffer := buffer || line || chr(10);"
				+ "  end loop;"
				+ "  :pBuffer := buffer;"
				+ "end;";

			Logger.WriteLine(sql);

			try
			{
				using (var cmd = new OracleCommand(sql, dbase.OraConnection))
				{
					using (var par = new OracleParameter("pBuffer", OracleDbType.Long, 1000000))
					{
						par.Direction = ParameterDirection.Output;
						cmd.Parameters.Add(par);

						cmd.ExecuteNonQuery();

						if (par.Value != DBNull.Value)
						{
							lines = par.Value.ToString().Trim();
							if (lines.Length > 0)
							{
								query.OutputLines.Add(lines);
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				query.AddMessage(exc);
			}
		}


		//========================================================================================
		// GetUserErrors()
		//========================================================================================

		public static MessageCollection GetUserErrors (DatabaseConnection dbase)
		{
			string CR = Environment.NewLine;
			var messages = new MessageCollection();

			string sql =
				"SELECT name, line, position, text"
				+ " FROM User_Errors"
				+ " ORDER BY name, line, position";

			try
			{
				OracleConnection con = dbase.OraConnection;
				using (var cmd = new OracleCommand(sql, con))
				{
					using (var da = new OracleDataAdapter())
					{
						da.SelectCommand = cmd;
						var ds = new DataSet();

						if (con.State != ConnectionState.Open)
							con.Open();

						int count = da.Fill(ds);

						if (count > 0)
						{
							var msg = new StringBuilder();

							string divider = CR
								+ "-".PadRight(15, '-')			// name
								+ " -".PadRight(8, '-')			// line
								+ " -".PadRight(7, '-')			// position
								+ " -".PadRight(80, '-');		// text

							msg.Append(CR
								+ "Name".PadRight(15, ' ')
								+ " Line".PadRight(8, ' ')
								+ " Offset".PadRight(7, ' ')
								+ " Text");

							msg.Append(divider);

							DataRow row = null;
							for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
							{
								row = ds.Tables[0].Rows[i];

								msg.Append(CR
									+ row["name"].ToString().PadRight(16, ' ')
									+ row["line"].ToString().PadRight(8, ' ')
									+ row["position"].ToString().PadRight(7, ' ')
									+ row["text"].ToString()
									);

								if (i < (ds.Tables[0].Rows.Count - 1))
									msg.Append(CR + divider);
							}

							messages.Add(
								new Message(Message.MessageType.Error, msg.ToString()));
						}
					}
				}
			}
			catch (Exception exc)
			{
				if (exc.Message == String.Empty)
					messages.Add(
						new Message(Message.MessageType.Error, exc.StackTrace));
				else
					messages.Add(
						new Message(Message.MessageType.Error, exc.Message));
			}

			return messages;
		}


		//========================================================================================
		// Explain()
		//========================================================================================

		private void Explain (Query query)
		{
			string tableName = UserOptions.GetString("connections/planTable");

			using (var transaction = dbase.OraConnection.BeginTransaction())
			{
				if (EnsurePlanTable(query, tableName))
				{
					string planID = "ORQA_" + DateTime.Now.Ticks.ToString("X");

					string text
						= "EXPLAIN PLAN SET statement_id='" + planID
						+ "' INTO " + dbase.DefaultSchema + "." + tableName
						+ " FOR " + query.SQL;

					try
					{
						using (var cmd = new OracleCommand(text, dbase.OraConnection))
						{
							// explain the query
							cmd.ExecuteNonQuery();

							// load the explanation
							// we use these colulmns: level, operation, optoins, object_name, cost, cardinality, bytes

							cmd.CommandText
								= "SELECT level-1 AS \"level\","
								+ " level, operation, options, object_name, cost, cardinality, bytes"
								//+ " statement_id, timestamp, remarks, operation, options, object_node, object_name,"
								//+ " object_instance, object_type, optimizer, search_columns, id, parent_id, position,"
								//+ " cost, cardinality, bytes, other_tag, partition_start, partition_stop, partition_id,"
								//+ " other, distribution"
								+ " FROM " + dbase.DefaultSchema + "." + tableName
								+ " CONNECT BY PRIOR id=parent_id"
								+ " START WITH id=0"
								+ " ORDER BY id";

							var adapter = new OracleDataAdapter();
							adapter.SelectCommand = cmd;

							query.Plan = new DataSet();
							adapter.Fill(query.Plan);
						}
					}
					catch (OracleException exc)
					{
						query.AddMessage(exc);
					}
					catch (Exception exc)
					{
						if (exc.Message == String.Empty)
							query.AddMessage(new Message(Message.MessageType.Error, exc.StackTrace));
						else
							query.AddMessage(new Message(Message.MessageType.Error, exc.Message));
					}
				}

				transaction.Commit();
			}
		}


		//========================================================================================
		// EnsurePlanTable()
		//========================================================================================

		private bool EnsurePlanTable (Query query, string tableName)
		{
			bool confirmed = false;

			string sql
				= "SELECT table_name"
				+ " FROM All_Tables"
				+ " WHERE owner='" + dbase.DefaultSchema
				+ "'  AND table_name='" + tableName
				+ "'";

			using (var cmd = new OracleCommand(sql, dbase.OraConnection))
			{
				using (var reader = cmd.ExecuteReader())
				{
					confirmed = reader.Read();
					reader.Close();
				}

				if (!confirmed)
				{
					sql = River.Orqa.Properties.Resources.ExplainPlanTable;
					var parser = new StatementParser();
					StatementCollection statements = parser.Parse(sql.Replace("PLAN_TABLE", tableName));

					cmd.CommandText = statements[0];

					try
					{
						int count = cmd.ExecuteNonQuery();
						confirmed = true;
					}
					catch (OracleException exc)
					{
						query.AddMessage(exc);
					}
					catch (Exception exc)
					{
						if (exc.Message == String.Empty)
							query.AddMessage(new Message(Message.MessageType.Error, exc.StackTrace));
						else
							query.AddMessage(new Message(Message.MessageType.Error, exc.Message));
					}
				}
			}

			return confirmed;
		}


		//========================================================================================
		// Misc Reporters
		//========================================================================================

		private void CaptureInfoMessage (object sender, OracleInfoMessageEventArgs e)
		{
			query.AddMessage(e.Message, e.Errors, dbase);
		}


		private void CaptureStateChange (object sender, StateChangeEventArgs e)
		{
			if (query != null) // if no statement in editor (comments only perhaps)
				query.AddMessage(e.OriginalState, e.CurrentState);
		}


		private void ReportUknown (Query query)
		{
			string s = query.SQL;
			if (s.Length > 40)
				s = s.Substring(0, 40) + "...";

			query.AddMessage(String.Format(RsxUnrecognizedQueryType, s));
		}


		private void ReportIgnored (Query query)
		{
			string s = query.SQL;
			if (s.Length > 40)
				s = s.Substring(0, 40) + "...";

			query.AddMessage(String.Format(RsxStatementIgnored, s));
		}
	}
}
