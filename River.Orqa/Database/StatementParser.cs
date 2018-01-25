//************************************************************************************************
// Copyright © 2002 Steven M. Cohn. All Rights Reserved.
//
// Given a block of text, build a collection of SQL statements that are ready
// to be executed against a data source provider.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Data;
	using System.Text;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Browser;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class StatementParser
	//********************************************************************************************

	/// <summary>
	/// Given a block of text, build a collection of SQL statements that
	/// are ready to be executed against a data source provider.
	/// <para>
	/// A single StatementParser instance can be reused and instances are
	/// thread-safe.  Ideally, each QueryWindow will create one parser and
	/// reuse that since a QueryWindow can only execute one query at a time.
	/// </para>
	/// </summary>

	internal class StatementParser
	{
		// static data
		private static readonly char Newline = '\n';

		private static StringCollection complexCmds;	// tokens that flag complex blocks
		private static StringCollection blockCmds;		// block starting token
		private static StringCollection methodCmds;		// package/procedure/function
		private static StringCollection executeCmds;	// execute stored procedure commands
		private static StringCollection specialCmds;	// specialized commands
		private static Dictionary<string, QueryType> queryVerbs;

		// localize format strings
		private static string parseCommentError;
		private static string connectionStateChange;

		// instance data
		private string text;							// original sql text
		private string sql;								// the clean sql text
		private List<int> map;							// char map from sql to text
		private List<int> delimeters;					// valid semi-colon statement delimeters

		private bool isOptimized;						// true if optimized (no comments, etc)

		// Events

		public event NotificationEventHandler ParseNotification;


		//========================================================================================
		// Constructors
		//========================================================================================

		static StatementParser ()
		{
			complexCmds = new StringCollection();
			complexCmds.AddRange(new string[] { "ALTER", "CREATE", "REPLACE" });

			blockCmds = new StringCollection();
			blockCmds.AddRange(new string[] { "BEGIN", "CASE", "IF", "LOOP" });

			methodCmds = new StringCollection();
			methodCmds.AddRange(new string[] { "PACKAGE", "PROCEDURE", "FUNCTION" });

			executeCmds = new StringCollection();
			executeCmds.AddRange(new string[] { "EXEC", "EXECUTE" });

			specialCmds = new StringCollection();
			specialCmds.AddRange(new string[] { "GRANT", "REVOKE" });

			queryVerbs = new Dictionary<string, QueryType>();
			queryVerbs.Add("EXIT", QueryType.Ignored);
			queryVerbs.Add("PURGE", QueryType.Nonreader);
			queryVerbs.Add("SET", QueryType.Set);
			queryVerbs.Add("START", QueryType.Script);

			Translator translator = new Translator("Orqa");
			parseCommentError = translator.GetString("ParseCommentError");
			connectionStateChange = translator.GetString("ConnectionStateChange");
			translator = null;
		}


		/// <summary>
		/// Initiailzes a new parser.
		/// </summary>

		public StatementParser ()
		{
			this.isOptimized = false;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets or sets a value indicating if the content is compressed by removing
		/// all comments and unecessary whitespace.  This is typically set using the
		/// value of the connections/optimizeSql User Options string.
		/// </summary>

		public bool IsOptimized
		{
			get { return isOptimized; }
			set { isOptimized = value; }
		}


		//========================================================================================
		// Parse()
		//========================================================================================

		/// <summary>
		/// This is the main entry point to parse a block of text.  The block may contain
		/// one or more statements.  The resultant StatementCollection is an array of
		/// strings; each string must then be parsed using the ParseStatement method to
		/// derive controlling objects such as stored procedure parameters.
		/// </summary>
		/// <param name="content">Text to parse into one or more statements.</param>
		/// <returns>A StatementCollection containing the parsed statements</returns>

		public StatementCollection Parse (string content)
		{
			this.map = new List<int>();
			this.delimeters = new List<int>();

			this.text = content;
			this.sql = Cleanup(content);

			var statements = new StatementCollection();

			Parse(0, 0, statements);

			return statements;
		}


		//========================================================================================
		// Cleanup()
		//		Strip all comments, newlines and unnecessary whitespace.
		//========================================================================================

		#region Cleanup

		private enum CleaningState
		{
			Text,
			String,									// String allow single or multi-line
			MComment,
			SComment
		}


		private string Cleanup (string content)
		{
			int i = 0;								// char buffer index
			int line = 1;							// current line
			int col = 1;							// current column
			int max = content.Length - 1;			// last char to process
			char p = Char.MinValue;
			char c = Char.MinValue;

			CleaningState state = CleaningState.Text;

			StringBuilder cleantext = new StringBuilder();

			while (i <= max)
			{
				c = content[i];
				switch (state)
				{
					case CleaningState.Text:
						if ((c == '-') && (i < max))
						{
							char next = content[i + 1];

							if (next == '-')
							{
								if (ParseNotification != null)
								{
									ParseNotification(new NotificationEventArgs(new Message(
										Message.MessageType.Info,
										String.Format(parseCommentError, line, col)
										)));
								}

								state = CleaningState.SComment;
								i++;
								col++;
							}
							else
							{
								cleantext.Append(c);
								map.Add(i);
							}
						}
						else if ((c == '/') && (i < max))
						{
							char next = content[i + 1];

							if (next == '*')
							{
								state = CleaningState.MComment;
								i++;
								col++;
							}
							else
							{
								cleantext.Append(c);
								map.Add(i);
							}
						}
						else if (c == '\'')
						{
							state = CleaningState.String;
							cleantext.Append(c);
							map.Add(i);
						}
						else
						{
							if (c == Newline)
							{
								c = ' ';
								col = 1;
								line++;
							}
							else if (c == ';')
							{
								delimeters.Add(cleantext.Length);
							}

							if (!(Char.IsWhiteSpace(c) && Char.IsWhiteSpace(p)))
							{
								cleantext.Append(c);
								map.Add(i);
							}
						}
						break;

					case CleaningState.String:
						if (c == '\'')
						{
							state = CleaningState.Text;
						}
						else if (c == Newline)
						{
							col = 1;
							line++;
						}
						cleantext.Append(c);
						map.Add(i);
						break;

					case CleaningState.MComment:
						if ((c == '/') && (p == '*'))
						{
							state = CleaningState.Text;
							c = p;
							i++;
							col++;
						}
						break;

					case CleaningState.SComment:
						if (c == Newline)
						{
							state = CleaningState.Text;
							//query.Append(c=' ');
							col = 1;
							line++;
						}
						break;
				}

				p = c;

				i++;
				col++;
			}

			return cleantext.ToString().Trim();
		}

		#endregion Cleanup


		//========================================================================================
		// Parse()
		//========================================================================================

		#region Parse

		private void Parse (int start, int delimIndex, StatementCollection statements)
		{
			if ((sql == null) || (sql.Length == 0) || (start >= sql.Length))
				return;

			if (sql[start] == ' ')
				start++;

			// extract first keyword in query

			string word;
			int space = sql.IndexOf(' ', start);
			if (space < 0)
			{
				// found last word.  now strip the final semicolon
				int end = sql.IndexOf(';', start);
				if (end < 0)
					word = sql.Substring(start).Trim();
				else
					word = sql.Substring(start, end - start).Trim();
			}
			else
			{
				word = sql.Substring(start, sql.IndexOf(' ', start) - start).ToUpper();
			}

			// test extract keyword for important delimeters

			if (word.Equals("/"))
			{
				// ignore the / command
				Parse(start + 2, delimIndex, statements);
			}
			else if (word.Equals("EXIT"))
			{
				// ignore the EXIT command
				Parse(start + 5, delimIndex, statements);
			}
			else if (complexCmds.Contains(word) && IsPackageDef(start + word.Length + 1))
			{
				// parse multi-depth DDL
				ParseComplex(ref start, statements);
				if (start < sql.Length)
					Parse(start, delimIndex, statements);
			}
			else
			{
				// TODO: use start-delimeter[] to determine end point rather than manually
				// looking for the next semi-colon (which might be embedded in a string)

				// simple query, seek end
				int end = IndexOfUnquoted(sql, start, ';');

				string query;
				if (end < 0)
					query = sql.Substring(start);
				else
					query = sql.Substring(start, end - start);

				statements.Add(query.Trim());

				if (end < 0)
					return;

				end++;
				if (end < sql.Length)
					Parse(end, delimIndex+1, statements);
			}
		}


		private int IndexOfUnquoted (string content, int start, char c)
		{
			int index = start;
			bool found = false;
			bool inQuotes = false;

			while ((index < content.Length) && !found)
			{
				if (inQuotes)
				{
					if (content[index] == '\'')
					{
						inQuotes = false;
					}
				}
				else
				{
					if (content[index] == '\'')
					{
						inQuotes = true;
					}
					else if (content[index] == c)
					{
						found = true;
						break;
					}
				}

				index++;
			}

			return (found ? index : -1);
		}


		//========================================================================================
		// IsPackageDef()
		//========================================================================================

		private bool IsPackageDef (int start)
		{
			string word = sql.Substring(start, sql.IndexOf(' ', start) - start).ToUpper();
			if (methodCmds.Contains(word))
				return true;

			if (word.Equals("OR"))
			{
				start += word.Length + 1;
				word = sql.Substring(start, sql.IndexOf(' ', start) - start).ToUpper();
				if (word.Equals("REPLACE"))
				{
					start += word.Length + 1;
					word = sql.Substring(start, sql.IndexOf(' ', start) - start).ToUpper();
					if (methodCmds.Contains(word))
						return true;
				}
			}

			return false;
		}


		//========================================================================================
		// ParseComplex()
		//========================================================================================

		private void ParseComplex (ref int start, StatementCollection statements)
		{
			int i = start;
			int saveStart = start;
			int depth = 0;
			bool found = false;
			bool inString = false;
			char c = Char.MinValue;

			var token = new StringBuilder();
			string word;

			while ((i < sql.Length) && !found)
			{
				c = sql[i];

				if (inString)
				{
					if (c == '\'')
						inString = false;

					token.Append(c);
				}
				else if (c == ';')
				{
					if (depth < 0)
						found = true;
					else if (token.Length > 0)
					{
						word = token.ToString().ToUpper();
						if (word.Equals("END"))
							depth--;

						if (depth < 0)
							found = true;
					}

					token.Length = 0;
				}
				else if (Char.IsWhiteSpace(c))
				{
					word = token.ToString().ToUpper();

					if (blockCmds.Contains(word))
					{
						depth++;
					}
					else if (word.Equals("END"))
					{
						depth--;
					}

					token.Length = 0;
				}
				else
				{
					if (c == '\'')
						inString = true;

					token.Append(c);
				}

				i++;
			}

			var query = new StringBuilder();

			if (found)
			{
				query.Append(sql.Substring(start, i - start));
				if (query[query.Length - 1] != ';')
					query.Append(';');

				start = i + 1;
			}
			else
			{
				query.Append(sql.Substring(start));
				if (query[query.Length - 1] != ';')
					query.Append(';');

				start = sql.Length;
			}

			if (isOptimized)
			{
				statements.Add(query.ToString());
			}
			else
			{
				int a = (int)map[saveStart];
				int b = (int)map[i - 1];

				query.Length = 0;
				query.Append(text.Substring(a, b - a + 1));
				if (query[query.Length - 1] != ';')
					query.Append(';');

				statements.Add(query.ToString());
			}
		}

		#endregion Parse


		//========================================================================================
		// ParseStatement()
		//========================================================================================

		#region ParseStatement

		/// <summary>
		/// Uses the Microsoft .NET Provider for Oracle to parse the SQL text
		///	and determine the statement type of the embedded command.
		/// </summary>
		/// <param name="dbase">The connection to use to evaluate the statement.</param>
		/// <param name="query">The query to populate with information.</param>

		public void ParseStatement (
			DatabaseConnection dbase, Query query, IBrowser browser)
		{
			query.StatementType = StatementType.Unknown;
			query.QueryType = QueryType.Unknown;

			string sql = query.SQL.Trim();

			if (sql[0] == '@')
			{
				query.QueryType = QueryType.Script;
				return;
			}

			int space = sql.IndexOf(' ');
			string verb = sql.Substring(0, (space < 0 ? sql.Length : space)).ToUpper();

			if (queryVerbs.ContainsKey(verb))
			{
				query.QueryType = queryVerbs[verb];
				return;
			}

			// EXEC, EXECUTE
			if (executeCmds.Contains(verb))
			{
				ParseExecuteStatement(query, browser);
				return;
			}

			StatementType stype;
			if (Enum.TryParse<StatementType>(verb, true, out stype))
			{
				query.StatementType = stype;
				query.QueryType = (query.StatementType == StatementType.Select ? QueryType.Reader : QueryType.Nonreader);
			}
			else
			{
				string upper = sql.ToUpper();

				int i = 0;
				bool found = false;
				while ((i < specialCmds.Count) && !found)
				{
					if (!(found = (upper.IndexOf(specialCmds[i]) >= 0)))
						i++;
				}

				// TODO: check for procedure name

				query.QueryType = (found ? QueryType.Nonreader : QueryType.Unknown);
			}


			/*
			//// Microsoft's provider doesn't like the timeout parameter, so mask it out
			//int timeout = UserOptions.GetInt("connections/loginTimeout");
			//UserOptions.SetValue("connections/loginTimeout", 0);

			System.Data.OracleClient.OracleConnection con
				= new System.Data.OracleClient.OracleConnection(dbase.BasicConnectionString);

			//// restore the timeout parameter for other callers
			//UserOptions.SetValue("connections/loginTimeout", timeout);

			try
			{
				con.InfoMessage += new System.Data.OracleClient.OracleInfoMessageEventHandler(DoCatchOracleInfoMessage);
				con.StateChange += new StateChangeEventHandler(DoCatchStateChange);

				con.Open();

				System.Data.OracleClient.OracleCommand cmd
					= new System.Data.OracleClient.OracleCommand(query.SQL, con);

				cmd.Prepare();

				Type t = typeof(System.Data.OracleClient.OracleCommand);
				int stype = (int)t.InvokeMember("StatementType",
					BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					cmd,
					new object[] { }
					);

				cmd = null;

				if (Enum.IsDefined(typeof(StatementType), stype))
				{
					query.StatementType = (StatementType)stype;
					query.QueryType = (query.StatementType == StatementType.Select ? QueryType.Reader : QueryType.Nonreader);
				}
				else
				{
					string upper = query.SQL.ToUpper();

					int i = 0;
					bool found = false;
					while ((i < specialCmds.Count) && !found)
					{
						if (!(found = (upper.IndexOf(specialCmds[i]) >= 0)))
							i++;
					}

					// TODO: check for procedure name

					query.QueryType = (found ? QueryType.Nonreader : QueryType.Unknown);
				}
			}
			catch (OracleException exc)
			{
				query.QueryType = QueryType.Invalid;

				Message message = new Message(Message.MessageType.Error, exc.Message);
				query.AddMessage(message);

				if (ParseNotification != null)
					ParseNotification(new NotificationEventArgs(message));

				Logger.WriteLine(exc.Message);
			}
			catch (Exception exc)
			{
				query.QueryType = QueryType.Invalid;

				Message message = new Message(Message.MessageType.Error, exc.Message);
				query.AddMessage(message);

				if (ParseNotification != null)
					ParseNotification(new NotificationEventArgs(message));

				Logger.WriteLine(exc.Message);
			}
			finally
			{
				if (con != null)
				{
					con.Close();
					con.Dispose();
					con = null;
				}
			}
			*/
		}


		//----------------------------------------------------------------------------------------
		// DocatchOracleInfoMessage()
		//----------------------------------------------------------------------------------------

		private void DoCatchOracleInfoMessage (
			object sender, System.Data.OracleClient.OracleInfoMessageEventArgs e)
		{
			if (ParseNotification != null)
			{
				ParseNotification(new NotificationEventArgs(new Message(
					Message.MessageType.Info,
					"[" + e.Code + "] " + e.Message
					)));
			}
		}


		//----------------------------------------------------------------------------------------
		// DoCatchStateChange()
		//----------------------------------------------------------------------------------------

		private void DoCatchStateChange (object sender, StateChangeEventArgs e)
		{
			if (ParseNotification != null)
			{
				ParseNotification(new NotificationEventArgs(new Message(
					Message.MessageType.State,
					String.Format(connectionStateChange, e.OriginalState, e.CurrentState)
					)));
			}
		}

		//========================================================================================
		// ParseExecuteStatement()
		//========================================================================================

		internal void ParseExecuteStatement (Query query, IBrowser browser)
		{
			string[] args;									// user specified arguments
			string procnam = ParseExecuteStatement(query.SQL, out args);

			SchemataParameter[] pars
				= (SchemataParameter[])browser.Invoke(
				new FindProcDelegate(browser.FindProcParameters), new object[] { procnam });

			if (pars == null)
			{
				query.AddMessage(new Message(Message.MessageType.Error,
					"Error discovering procedure '" + procnam + "'"));

				query.QueryType = QueryType.Unknown;
				return;
			}

			HybridDictionary parameters = new HybridDictionary();
			foreach (SchemataParameter par in pars)
			{
				parameters.Add(par.Name.ToLower(), new Parameter(par));
			}

			object[] parlist = new object[parameters.Count];
			parameters.Values.CopyTo(parlist, 0);

			if (procnam != null)
			{
				if ((args != null) && (args.Length > 0))
				{
					string[] parts;
					string pname;
					string pvalue;
					Parameter parameter;

					for (int i = 0; (i < args.Length) && (i < pars.Length); i++)
					{
						if (args[i].IndexOf('=') > 0)
						{
							parts = args[i].Trim().Substring(1).Split(new char[] { '=' }, 2);

							pname = parts[0].Trim().ToLower();
							pvalue = parts[1].Trim();
						}
						else
						{
							pname = ((Parameter)(parlist[i])).Name;
							pvalue = args[i].Trim();
						}

						if (pvalue[0] == '\'') pvalue = pvalue.Substring(1);
						if (pvalue[pvalue.Length - 1] == '\'') pvalue = pvalue.Substring(0, pvalue.Length - 1);

						parameter = (Parameter)parameters[pname];
						if (parameter != null)
						{
							parameter.Value = pvalue;
						}
					}
				}
			}

			ParameterCollection list = new ParameterCollection();
			foreach (Parameter par in parameters.Values)
			{
				list.Add(par);
			}

			query.SQL = procnam;
			query.Parameters = list;
			query.QueryType = QueryType.ParsedProcedure;
		}


		//========================================================================================
		// ParseExecuteStatement()
		//========================================================================================

		/// <summary>
		/// Extracts the procedure name and individual argument name/value pairs from
		/// the specified statement text.
		/// </summary>
		/// <param name="statement">The statement to parse.</param>
		/// <param name="args">An array to which is written the parameter name/value pairs.</param>
		/// <returns>The name of the stored procedure.</returns>
		/// <remarks>
		/// Handles processing our custom exec statements of the form:
		/// <para>
		/// EXEC[UTE] [(] procnam @input1=value, @input2=vlaue, @@output1, ... [)]
		/// </para>
		/// </remarks>

		private string ParseExecuteStatement (string statement, out string[] args)
		{
			int i = 0;
			int start = 0;
			bool found = false;

			if (statement.Trim().ToLower().StartsWith("exec"))
			{
				// might be either "exec" or "execute" to just find first space
				found = false;
				while ((i < statement.Length) && !found)
				{
					if (!(found = Char.IsWhiteSpace(statement[i])))
						i++;
				}

				start = ++i;
			}

			// skip name
			found = false;
			while ((i < statement.Length) && !found)
			{
				if (Char.IsWhiteSpace(statement[i]) || (statement[i] == '('))
					found = true;
				else
					i++;
			}

			if (!found)
			{
				args = new string[0];
				return statement.Substring(start);
			}

			string procnam = statement.Substring(start, i - start);

			bool hasParen = (statement[i] == '(');
			if (!hasParen)
			{
				// skip whitespace
				found = false;
				while ((i < statement.Length) && !found)
				{
					if (!(found = !Char.IsWhiteSpace(statement[i])))
						i++;
				}
			}

			if (hasParen = (statement[i] == '('))
			{
				i++;
			}

			// parse arguments on commas (but ignore commas in quoted values)

			string argseg = statement.Substring(i);
			bool inQuote = false;
			start = i;
			ArrayList arglist = new ArrayList();
			found = false;						// found used to discover last ")"

			while ((i < statement.Length) && !found)
			{
				if (inQuote)
				{
					if (statement[i] == '\'')
						inQuote = false;
				}
				else if (statement[i] == '\'')
				{
					inQuote = true;
				}
				else if (statement[i] == ',')
				{
					arglist.Add(statement.Substring(start, i - start));
					start = i + 1;
				}
				else if (hasParen && ((statement[i] == ')') || (statement[i] == ';')))
				{
					arglist.Add(statement.Substring(start, i - start));
					found = true;
				}

				i++;
			}

			if ((i > start) && !found)
			{
				// capture last one
				arglist.Add(statement.Substring(start, i - start));
			}

			args = new string[arglist.Count];
			arglist.CopyTo(args);

			return procnam;
		}

		#endregion ParseStatement


		//========================================================================================
		// BuildProcedureStatement()
		//========================================================================================

		#region BuildProcedureStatement

		/// <summary>
		/// Given a query containing a parameter collection, generates the SQL statement
		/// required to invoke a stored procedure.  The parameters would have been created
		/// using the ParametersDialog after "opening" a SchemataProcedure.
		/// </summary>
		/// <param name="query">The query containing the objectified parameters.</param>
		/// <returns>A string capable of invoking the stored procedure.</returns>

		public string BuildProcedureStatement (Query query)
		{
			StringBuilder sql = new StringBuilder("EXEC " + query.SQL);
			string val = null;
			int i = 0;

			foreach (Parameter parameter in query.Parameters)
			{
				if (parameter.Value == System.DBNull.Value)
					val = "null";
				else
				{
					if ((parameter.DataType == OracleDbType.Char) ||
						(parameter.DataType == OracleDbType.NChar) ||
						(parameter.DataType == OracleDbType.NVarchar2) ||
						(parameter.DataType == OracleDbType.Varchar2))
					{
						val = "'" + parameter.Value + "'";
					}
					else
					{
						val = parameter.Value.ToString();
					}
				}

				//if (parameter.Direction != ParameterDirection.Output)
				{
					sql.Append(i == 0 ? " " : ", ");

					if ((parameter.Direction & ParameterDirection.Input) > 0)
					{
						sql.Append("@" + parameter.Name + "=" + val);
					}
					else
					{
						// won't run if uncomment the 'if' statement above to filter out
						// Output parameters!
						sql.Append("@@" + parameter.Name);
					}
				}

				i++;
			}

			sql.Append(";");

			return sql.ToString();
		}

		#endregion BuildProcedureStatement
	}
}
