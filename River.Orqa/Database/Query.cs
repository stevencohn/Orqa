//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Maintains the entire lifetime history of a single executable query including
// result record sets, execution plans and output messages.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2004      New
// 01-Aug-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Data;
	using System.Text;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class Query
	//********************************************************************************************

	/// <summary>
	/// Maintains the entire lifetime history of a single executable query including
	/// result record sets, execution plans and output messages.
	/// </summary>

	internal class Query
	{
		private static readonly string CR = System.Environment.NewLine;
		private static string connectionStateChange;

		private string text;					// SQL text of query
		private ParameterCollection parameters;	// stored procedure parameters
		private bool hasOutputs;				// true if has output parameters
		private bool hideResults;				// true if show messages only

		private QueryType queryType;			// the internal query type
		private StatementType statementType;	// the database statement type
		private int affectedRecords;			// number of records affected by query
		private int ticks;						// temporary internal tick counter

		private OraData odata;					// result recordsets
		private MessageCollection messages;		// captured processing messages
		private StringCollection outputlines;	// captured display lines
		private string outputFilename;			// captured output file

		private DataSet plan;					// explain plan


		//========================================================================================
		// Constructor
		//========================================================================================

		static Query ()
		{
			Translator translator = new Translator("Orqa");
			connectionStateChange = translator.GetString("ConnectionStateChange");
			translator = null;
		}


		/// <summary>
		/// Initialize a new Query for the given connection and SQL text.
		/// </summary>
		/// <param name="con">An open OracleConnection.</param>
		/// <param name="text">The SQL text to execute.</param>

		public Query (string text)
		{
			this.text = text;
			this.parameters = null;
			this.hasOutputs = false;
			this.hideResults = false;

			this.queryType = QueryType.Unknown;
			this.statementType = StatementType.Unknown;

			this.affectedRecords = 0;
			this.ticks = 0;

			this.odata = new OraData();
			this.messages = new MessageCollection();
			this.outputlines = new StringCollection();
			this.outputFilename = null;

			this.plan = null;
		}


		/// <summary>
		/// Initializes a new query for the specified stored procedure and the
		/// given parameter list.
		/// </summary>
		/// <param name="text">The fully qualified name of the procedure to run.</param>
		/// <param name="parameters">The I/O parameters to use</param>

		public Query (string text, ParameterCollection parameters)
			: this(text)
		{
			this.parameters = parameters;
			this.queryType = QueryType.ParsedProcedure;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets or sets the number of records return (SELECT) or affect (INSERT, UPDATE
		/// or DELETE).
		/// </summary>

		public int AffectedRecords
		{
			get { return affectedRecords; }
			set { affectedRecords = value; }
		}


		/// <summary>
		/// Gets the data set returned by a reader query.
		/// </summary>

		public OraData Data
		{
			get { return odata; }
		}


		/// <summary>
		/// Gets a boolean value indicating if the statement type represented by this
		/// query can be explained.
		/// </summary>

		public bool CanExplain
		{
			get
			{
				return (statementType == StatementType.Select) ||
					   (statementType == StatementType.Delete) ||
					   (statementType == StatementType.Insert) ||
					   (statementType == StatementType.Update);
			}
		}


		/// <summary>
		/// Gets the elapsed time consumed by this query as a TimeSpan.
		/// </summary>

		public TimeSpan ElapsedTime
		{
			get { return TimeSpan.FromMilliseconds(ticks); }
		}


		/// <summary>
		/// Gets or sets a boolean value indicating if this query has produced
		/// output parameter values.
		/// </summary>

		public bool HasOutputs
		{
			get { return hasOutputs; }
			set { hasOutputs = value; }
		}


		/// <summary>
		/// Gets or sets a boolean value indicating if the results portion of this query
		/// should be hidden from the ResultsView.  However, any messages will be displayed.
		/// This is used for the Tools\ShowUserErrors command.
		/// </summary>

		public bool HideResults
		{
			get { return hideResults; }
			set { hideResults = value; }
		}


		/// <summary>
		/// Gest a collection of informational messages related to this query.
		/// </summary>

		public MessageCollection Messages
		{
			get { return messages; }
		}


		/// <summary>
		/// 
		/// </summary>

		public string OutputFilename
		{
			get { return outputFilename; }
			set { outputFilename = value; }
		}


		/// <summary>
		/// Gets a collection of DBMS output lines generated by this query.
		/// </summary>

		public StringCollection OutputLines
		{
			get { return outputlines; }
		}


		/// <summary>
		/// Gets the EXPLAIN PLAN describing the execution plan of this query.
		/// This is instantiated locally and updated by reference.
		/// </summary>

		public DataSet Plan
		{
			get { return plan; }
			set { plan = value; }
		}


		/// <summary>
		/// Gets a collection of parameters for stored procedure executions.  This
		/// property returns <b>null</b> if the query does not represent a stored
		/// procedure.
		/// </summary>

		public ParameterCollection Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}


		/// <summary>
		/// Gets or sets the QueryType of this query (e.g. Reader, NonReader, Script, etc.)
		/// </summary>

		public QueryType QueryType
		{
			get { return queryType; }
			set { queryType = value; }
		}


		/// <summary>
		/// Gets or sets the text of this query.  The setter is only used when parsing
		/// an EXEC command to replace the full SQL statement with the stored procedure
		/// name.
		/// </summary>

		public string SQL
		{
			get { return text; }
			set { text = value; }
		}


		/// <summary>
		/// Gets or sets the StatementType of this query (e.g. Delete, Insert, Select, etc.)
		/// </summary>
		public StatementType StatementType
		{
			get { return statementType; }
			set { statementType = value; }
		}


		/// <summary>
		/// Gets or sets the elapsed tick count consumed by this query
		/// </summary>

		public int Ticks
		{
			get { return ticks; }
			set { ticks = value; }
		}


		//========================================================================================
		// AddMessage()
		//========================================================================================

		#region AddMessage

		/// <summary>
		/// Adds a new informational message to the message collection of this query.
		/// </summary>
		/// <param name="msg">The text of the message to add.</param>

		public void AddMessage (string msg)
		{
			messages.Add(new Message(Message.MessageType.Info, msg));
		}


		/// <summary>
		/// Adds a new exception message to the message collection of this query.
		/// </summary>
		/// <param name="exc">The System.Exception to add.</param>

		public void AddMessage (Exception exc)
		{
			// TODO: build in-depth exception message

			StringBuilder msg = new StringBuilder();

			if (!String.IsNullOrEmpty(exc.Message))
			{
				msg.Append(exc.Message);
			}

			msg.Append(exc.StackTrace);
	
			messages.Add(new Message(Message.MessageType.Error, msg.ToString()));
		}


		/// <summary>
		/// Adds a new Oracle exception message to the message collection of this query.
		/// </summary>
		/// <param name="exc">The Oracle exception to add.</param>

		public void AddMessage (OracleException exc, DatabaseConnection dbase)
		{
			string excMessage = (exc.Message == String.Empty ? exc.StackTrace : exc.Message);

			bool hasUserErrors = false;
			StringBuilder msg = new StringBuilder(excMessage);

			foreach (OracleError error in exc.Errors)
			{
				msg.Append(CR + "(" + error.Number + ") " + error.Message);

				if (error.Number == 24344)			// success with compilation error
					hasUserErrors = true;
			}

			messages.Add(new Message(Message.MessageType.Error, msg.ToString()));

			if (hasUserErrors)
				messages.AddRange(QueryDriver.GetUserErrors(dbase));
		}


		/// <summary>
		/// Adds a given message to the message collection of this query.
		/// </summary>
		/// <param name="message">The message to add.</param>

		public void AddMessage (Message message)
		{
			messages.Add(message);
		}


		/// <summary>
		/// Adds an error message and a collection of Oracle errors to the message
		/// collection of this query.
		/// </summary>
		/// <param name="errmsg">An error message to add.</param>
		/// <param name="errors">The OracleErrorCollection to interperet and add.</param>

		public void AddMessage (string errmsg, OracleErrorCollection errors, DatabaseConnection dbase)
		{
			bool hasUserErrors = false;
			StringBuilder msg = new StringBuilder(errmsg);

			foreach (OracleError error in errors)
			{
				msg.Append(CR + "(" + error.Number + ") " + error.Message);

				if (error.Number == 24344)			// success with compilation error
					hasUserErrors = true;
			}

			messages.Add(new Message(Message.MessageType.Info, msg.ToString()));

			if (hasUserErrors)
				messages.AddRange(QueryDriver.GetUserErrors(dbase));
		}


		/// <summary>
		/// Adds a connection state change message to the message collection of this query.
		/// </summary>
		/// <param name="original">The original connection state.</param>
		/// <param name="current">The current connection state.</param>

		public void AddMessage (ConnectionState original, ConnectionState current)
		{
			messages.Add(new Message(
				Message.MessageType.State,
				String.Format(connectionStateChange, original, current)
				));
		}

		#endregion AddMessage
	}
}
