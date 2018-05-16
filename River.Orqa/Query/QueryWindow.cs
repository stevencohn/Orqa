//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Manages an intelligent SQL syntax editor
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Query
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using River.Orqa.Browser;
	using River.Orqa.Database;
	using River.Orqa.Editor;
	using River.Orqa.Options;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class QueryWindow
	//********************************************************************************************

	internal partial class QueryWindow : Form, IWorker
	{
		// Resource format strings
		private static string RxCaretStatusText;
		private static string RxCaretStatusTooltip;
		private static string RxExecutionTime;
		private static string RxExecutingStatus;
		private static string RxReadyStatus;
		private static string RxNotExecutableStatus;

		private static int count = 0;					// num of all query windows ever created

		private int queryID;							// ID of this window (from count)
		private bool windowIsValid;						// true until window handle destroyed
		private IEditor activePane;						// active pane, text or results
		private ICommander commander;					// reference to main commander
		private IBrowser browser;						// reference to main browser

		private DatabaseConnection dbase;				// the db connection for this window
		private QueryCollection queries;				// current query collection
		private QueryDriver driver;						// driver for this window

		private string filename;						// real file name
		private string basePath;						// base dir path of filename
		private ResultTarget resultTarget;				// preferred result target format


		// Events

		public event OpeningOptionsEventHandler OpeningOptions;
		public event ResultTargetChangedEventHandler ResultTargetChanged;
		public event EventHandler Saving;
		public event ToggleResultsHandler ToggleResults;


		//========================================================================================
		// Constructor
		//========================================================================================

		static QueryWindow ()
		{
			var translator = new Translator("Query");
			RxCaretStatusText = translator.GetString("CaretStatusText");
			RxCaretStatusTooltip = translator.GetString("CaretStatusTooltip");
			RxExecutionTime = translator.GetString("ExecutionTime");
			RxExecutingStatus = translator.GetString("ExecutingStatus");
			RxReadyStatus = translator.GetString("ReadyStatus");
			RxNotExecutableStatus = translator.GetString("NotExecutableStatusMsg");
			translator = null;
		}


		/// <summary>
		/// Initializes a new QueryWindow for design-time; this default constructor is
		/// required by the VS Forms designer.
		/// </summary>

		public QueryWindow ()
		{
			InitializeComponent();

			if (this.DesignMode)
			{
				return;
			}

			activePane = editorView;

			windowIsValid = true;
			this.HandleDestroyed += new EventHandler(DoHandleDestroyed);

			splitContainer.SplitterDistance = (int)(splitContainer.ClientSize.Height * 0.6);

			// initially hide the results pane
			splitContainer.Panel2Collapsed = true;

			var updateCommanderHandler = new EventHandler(UpdateCommander);
			var resultTargetChangedHandler
				= new ResultTargetChangedEventHandler(DoResultTargetChanged);

			editorView.Enter += updateCommanderHandler;
			editorView.ResultTargetChanged += resultTargetChangedHandler;
			resultsView.Enter += updateCommanderHandler;
			resultsView.TextChanged += updateCommanderHandler;
			//resultsView.ResultTargetChanged += resultTargetChangedHandler;

			queryID = ++count;
			queries = new QueryCollection();

			this.ResultTarget = (ResultTarget)UserOptions.GetInt("results/general/target");
			this.basePath = UserOptions.GetString("general/queryPath");
			this.filename = null;
		}


		/// <summary>
		/// Initializes a new QueryWindow for run-time.
		/// </summary>
		/// <param name="commander">A reference to the main ICommander.</param>
		/// <param name="con">The database connection for this window.</param>

		public QueryWindow (ICommander commander, DatabaseConnection con, IBrowser browser)
			: this()
		{
			SetOptions();

			this.commander = commander;
			this.browser = browser;
			this.dbase = con;

			this.driver = new QueryDriver(con, browser);
			this.driver.QueryCompleted += new QueryCompletedEventHandler(DoQueryCompleted);
			this.driver.QueriesCompleted += new QueriesCompletedEventHandler(DoQueriesCompleted);

			if (con == null)
			{
				userStatus.Text = "(no connection)";
			}
			else
			{
				userStatus.Text = "(" + con.DefaultSchema + ")";
			}
		}


		private void DoWindowActivated (object sender, EventArgs e)
		{
			UpdateCommander(sender, e);
		}


		private void DoWindowLoad (object sender, EventArgs e)
		{
			SetTitle();
			UpdateCommander(sender, e);
		}


		//========================================================================================
		// Properties
		//========================================================================================

		#region Properties

		public IEditor ActivePane
		{
			get { return activePane; }
		}


		public DatabaseConnection DatabaseConnection
		{
			get { return dbase; }
		}


		public string Filename
		{
			get { return filename; }
		}


		public bool IsSaved
		{
			get { return editorView.IsSaved; }
			set { editorView.IsSaved = value; }
		}


		public ResultTarget ResultTarget
		{
			get
			{
				return resultTarget;
			}

			set
			{
				switch (value)
				{
					case ResultTarget.Text:
						textMenuItem.Checked = true;
						gridMenuItem.Checked = false;
						xmlMenuItem.Checked = false;
						formatStatus.Image = textMenuItem.Image;
						break;

					case ResultTarget.Grid:
						textMenuItem.Checked = false;
						gridMenuItem.Checked = true;
						xmlMenuItem.Checked = false;
						formatStatus.Image = gridMenuItem.Image;
						break;

					case ResultTarget.Xml:
						textMenuItem.Checked = false;
						gridMenuItem.Checked = false;
						xmlMenuItem.Checked = true;
						formatStatus.Image = xmlMenuItem.Image;
						break;
				}

				editorView.ResultTarget = value;
				resultsView.ResultTarget = value;
				resultTarget = value;
			}
		}

		#endregion Properties


		//========================================================================================
		// Methods
		//========================================================================================

		#region Methods

		//========================================================================================
		// InsertText()
		//		Used to inject template content.
		//========================================================================================

		public void InsertText (string text)
		{
			editorView.InsertText(text);
		}


		public void ReportMessage (Database.Message message)
		{
			if (splitContainer.Panel2Collapsed)
				splitContainer.Panel2Collapsed = false;

			resultsView.MoveToMessageTab();
			resultsView.ReportMessage(message);
		}


		//========================================================================================
		// LoadFile()
		//========================================================================================

		public void LoadFile (string filename)
		{
			LoadFileInternal(filename);

			if (Path.GetExtension(filename).Equals(".tpl"))
			{
				editorView.IsExecutable = false;
				SetStatusMessage(RxNotExecutableStatus);
			}
		}


		public void LoadReport (string filename)
		{
			LoadFileInternal(filename);
		}


		private void LoadFileInternal (string filename)
		{
			editorView.LoadFile(filename);
			editorView.IsSaved = true;

			resultsView.Clear();

			this.filename = filename;
			this.basePath = Path.GetDirectoryName(filename);

			SetTitle();
		}


		//========================================================================================
		// MoveHome()
		//========================================================================================

		public void MoveHome ()
		{
			editorView.MoveHome();
		}


		//========================================================================================
		// SaveFile()
		//========================================================================================

		public void SaveFile (string filename)
		{
			IEditor editor = activePane;
			editor.SaveFile(filename);

			if (editor == editorView)
			{
				this.filename = filename;
				this.basePath = System.IO.Path.GetDirectoryName(filename);
				SetTitle();
			}

			UpdateCommander(null, null);
		}


		//========================================================================================
		// SetSchema()
		//========================================================================================

		public void SetSchema (string schemaName)
		{
			dbase.DefaultSchema = schemaName;
			SetTitle();
		}


		//========================================================================================
		// SetTitle()
		//========================================================================================

		/// <summary>
		/// Sets the title of the Query Window to represent the query ID, the current
		/// schema, the host name and the filename if any.
		/// </summary>

		public void SetTitle ()
		{
			SetTitle(null);
		}


		/// <summary>
		/// Sets the default title and appends the name of a specific object.
		/// </summary>
		/// <param name="objectName">The name of the object to append.</param>

		public void SetTitle (string objectName)
		{
			StringBuilder title = new StringBuilder("Query " + queryID);
			title.Append(" - ");

			if (dbase == null)
			{
				title.Append("No connection");
			}
			else
			{
				title.Append(dbase.ServiceName);
				title.Append(" @");
				title.Append(dbase.HostName == null ? "local" : dbase.HostName.ToLower());
				title.Append(".");
				title.Append(dbase.DefaultSchema);

				title.Append(" - ");
				if (objectName == null)
				{
					title.Append(filename == null ? "Untitled" : filename);
				}
				else
				{
					title.Append(objectName);
				}

				if (!editorView.IsSaved)
					title.Append("*");
			}

			Text = title.ToString();
		}

		#endregion Methods


		//========================================================================================
		// IEditor implementation
		//========================================================================================

		#region IEditor

		public void DoUndo (object sender, EventArgs e)
		{
			activePane.DoUndo(sender, e);
		}


		public void DoRedo (object sender, EventArgs e)
		{
			activePane.DoRedo(sender, e);
		}


		public void DoCut (object sender, EventArgs e)
		{
			activePane.DoCut(sender, e);
		}


		public void DoCopy (object sender, EventArgs e)
		{
			activePane.DoCopy(sender, e);
		}


		public void DoPaste (object sender, EventArgs e)
		{
			activePane.DoPaste(sender, e);
		}


		public void DoSelectAll (object sender, EventArgs e)
		{
			activePane.DoSelectAll(sender, e);
		}


		public void DoClearWindow (object sender, EventArgs e)
		{
			activePane.DoClear(sender, e);
		}


		public void DoFind (object sender, EventArgs e)
		{
			activePane.DoFind(sender, e);
		}


		public void DoGotoLine (object sender, EventArgs e)
		{
			activePane.DoGotoLine(sender, e);
		}


		public void DoMakeLowercase (object sender, EventArgs e)
		{
			activePane.DoMakeLowercase(sender, e);
		}


		public void DoMakeUppercase (object sender, EventArgs e)
		{
			activePane.DoMakeUppercase(sender, e);
		}


		public void DoResultTargetChanged (object sender, ResultTargetChangedEventArgs e)
		{
			this.ResultTarget = e.ResultTarget;
		}


		public void DoToggleWhitespace (object sender, EventArgs e)
		{
			activePane.DoToggleWhitespace(sender, e);
		}

		#endregion IEditor


		//========================================================================================
		// Statusbar Methods
		//========================================================================================

		#region Statusbar Methods

		public void SetStatusMessage (string text)
		{
			messageStatus.Text = text;
			Application.DoEvents();
		}


		private void SetStatusRows (int rows)
		{
			if (rows == 1)
				rowsStatus.Text = "1 row";
			else
				rowsStatus.Text = rows.ToString() + " rows";

			rowsStatus.ToolTipText = "Affected Rows: " + rowsStatus.Text;
		}


		private void SetStatusTime (TimeSpan span)
		{
			string time = span.ToString();
			int p = time.IndexOf('.');
			if (p > 0)
				time = time.Substring(0, p + 3);

			timerStatus.Text = time;
			timerStatus.ToolTipText = String.Format(RxExecutionTime, timerStatus.Text);

			Application.DoEvents();
		}

		#endregion Statusbar Methods

		//========================================================================================
		// Handlers
		//========================================================================================

		#region Handlers

		private void DoCaretChanged (object sender, CaretChangedEventArgs e)
		{
			caretStatus.Text = String.Format(RxCaretStatusText, e.Line, e.Column);
			caretStatus.ToolTipText = String.Format(RxCaretStatusTooltip, e.Line, e.Column);
		}


		private void DoChangeResultsTarget (object sender, EventArgs e)
		{
			if (ResultTargetChanged == null)
				return;

			if (sender == textMenuItem)
			{
				this.ResultTarget = ResultTarget.Text;
			}
			else if (sender == gridMenuItem)
			{
				this.ResultTarget = ResultTarget.Grid;
			}
			else
			{
				this.ResultTarget = ResultTarget.Xml;
			}

			ResultTargetChanged(sender, new ResultTargetChangedEventArgs(this.ResultTarget));
			editorView.Focus();
		}


		private void DoEdited (object sender, EditorEventArgs e)
		{
			switch (e.Action)
			{
				case EditorAction.HasContent:
					commander.TextControls.IsEnabled = true;
					break;

				case EditorAction.HasNoContent:
					commander.TextControls.IsEnabled = false;
					break;

				case EditorAction.Unsaved:
					commander.SaveControls.IsEnabled = true;
					SetTitle();
					break;
			}

			commander.RedoControls.IsEnabled = editorView.CanRedo;
			commander.UndoControls.IsEnabled = editorView.CanUndo;
		}


		private void DoHandleDestroyed (object sender, EventArgs e)
		{
			windowIsValid = false;
		}


		private void DoOpeningOptions (object sender, OpeningOptionsEventArgs e)
		{
			// this event comes from either editorView or resultsView
			// we just need to bubble it up to the MainWindow...

			if (OpeningOptions != null)
				OpeningOptions(sender, e);
		}


		private void DoSaving (object sender, EventArgs e)
		{
			if (Saving != null)
				Saving(sender, e);
		}


		private void DoSelectionChanged (object sender, EventArgs e)
		{
			commander.SelectControls.IsEnabled = editorView.HasSelection;
		}


		public void DoToggleResults (object sender, EventArgs e)
		{
			splitContainer.Panel2Collapsed = !splitContainer.Panel2Collapsed;
			commander.SetChecker("ToggleResultsPane", !splitContainer.Panel2Collapsed);

			if (ToggleResults != null)
				ToggleResults(sender, new ToggleResultsEventArgs(splitContainer.Panel2Collapsed));
		}


		public void UpdateCommander ()
		{
			UpdateCommander(this, new EventArgs());
		}


		private void UpdateCommander (object sender, EventArgs e)
		{
			// activePane is initialized to editorView.  When the QueryWindow is activated,
			// this handler is fired, so we only want to use 'sender' when either editorView
			// or resultsView fires this handler.

			if (sender is IEditor)
			{
				activePane = (IEditor)sender;
			}

			commander.SetWorker(this);

			commander.AdvancedControls.IsEnabled = (activePane == editorView);
			commander.ConnectControls.IsEnabled = true;
			commander.ExecuteControls.IsEnabled = driver.IsExecuting;
			commander.PasteControls.IsEnabled = activePane.CanPaste;
			commander.RedoControls.IsEnabled = activePane.CanRedo;
			commander.SaveControls.IsEnabled = !activePane.IsSaved;
			commander.SelectControls.IsEnabled = activePane.HasSelection;
			commander.TextControls.IsEnabled = activePane.HasContent;
			commander.UndoControls.IsEnabled = activePane.CanUndo;
			commander.SetChecker("ToggleWhitespace", activePane.WhitespaceVisible);
			commander.SetChecker("ToggleResultsPane", !splitContainer.Panel2Collapsed);

			if (!editorView.IsExecutable)
			{
				commander.SqlControls.IsEnabled = false;
			}

			if (ResultTargetChanged != null)
				ResultTargetChanged(sender, new ResultTargetChangedEventArgs(resultTarget));
		}

		#endregion Handlers


		//========================================================================================
		// Query Handling
		//========================================================================================

		//========================================================================================
		// CancelQuery()
		//========================================================================================

		/// <summary>
		/// Cancels the currently executing query collection as soon as possible.
		/// </summary>

		public void CancelQuery ()
		{
			resultsView.Unlock();

			driver.Cancel();
		}


		//========================================================================================
		// CompileSchemata()
		//========================================================================================

		/// <summary>
		/// Eventually invoked as a result of selecting the Compile context menu from a Browser node.
		/// </summary>
		/// <param name="node">The node selected.</param>

		public void CompileSchemata (SchemataNode node)
		{
			node.Compile(this);
		}


		//========================================================================================
		// EditSchemata()
		//========================================================================================

		/// <summary>
		/// Eventually invoked as a result of selecting the Edit context menu from a Browser node.
		/// </summary>
		/// <param name="node">The node selected.</param>

		public void EditSchemata (SchemataNode node)
		{
			node.Edit(this);
		}


		//========================================================================================
		// OpenSchemata()
		//========================================================================================

		/// <summary>
		/// Eventually invoked as a result of selecting the Open context menu from a Browser node.
		/// </summary>
		/// <param name="node">The node selected.</param>

		public void OpenSchemata (SchemataNode node)
		{
			if ((node is SchemataTable) || (node is SchemataView))
			{
				InsertText("SELECT * FROM " + node.SchemaName + "." + Database.Query.Quote(node.Text) + ";\n");
				IsSaved = true;
				SetTitle();
				this.Text += " - " + node.Text;

				splitContainer.SplitterDistance = (int)(splitContainer.ClientSize.Height * 0.25);
				Execute(ParseMode.Sequential, editorView.SelectedText, 1);
			}
			else if (node is SchemataProcedure)
			{
				if (!node.IsDiscovered)
				{
					node.Discover();
				}

				splitContainer.SplitterDistance = (int)(splitContainer.ClientSize.Height * 0.25);
				RunProcedure((SchemataProcedure)node);
			}
		}


		//========================================================================================
		// PurgeRecycleBin()
		//========================================================================================

		/// <summary>
		/// Opens a new query window and executes the PURGE RECYCLEBIN statement.
		/// </summary>

		public void PurgeRecycleBin ()
		{
			InsertText("PURGE RECYCLEBIN;\n");
			IsSaved = true;
			SetTitle();

			if (splitContainer.Panel2Collapsed)
				DoToggleResults(null, null);

			splitContainer.SplitterDistance = (int)(splitContainer.Height * 0.1);

			Execute(ParseMode.Sequential, editorView.SelectedText, 1);
		}


		//========================================================================================
		// OpenSQLTraceReport()
		//========================================================================================

		public void OpenSQLTraceReport ()
		{
			IsSaved = true;
			SetTitle();
			editorView.IsExecutable = false;

			// catch up on Windows messages just to look better
			Application.DoEvents();

			string sql =
				"select P.tracefile from v$process P, v$session S"
				+ " where S.audsid = userenv('sessionid') and S.paddr = P.addr";

			string trcpath = dbase.LookupQuickInfo(sql);
			string repfile = System.IO.Path.Combine(
				System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

			string cmd = System.IO.Path.Combine(DatabaseSetup.OracleHome, @"bin\tkprof.exe");

			var driver = new CmdDriver();
			driver.Run(cmd, trcpath, repfile);

			LoadFile(repfile);
		}


		public void ExecuteReport (string reportScript)
		{
			IsSaved = true;
			SetTitle();
			editorView.IsExecutable = false;
			SetStatusMessage("Generating report...");

			// catch up on Windows messages just to look better
			Application.DoEvents();

			// first report to tmpfile
			string tmpfile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Logger.WriteLine("report tmpfile is " + tmpfile);

			// run the report
			var driver = new SqlPlusDriver(dbase, tmpfile);
			driver.Run(reportScript);

			// now stream tmpfile to report file to untabify
			string report = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Logger.WriteLine("report outfile is " + report);

			using (var writer = new StreamWriter(report, false))
			{
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
							writer.WriteLine(line.Untabify(8));
						}
					}
				}
			}

			SetStatusMessage(String.Empty);

			LoadReport(report);
		}


		//========================================================================================
		// ShowOpenCursors()
		//========================================================================================

		/// <summary>
		/// Opens a new query window and executes the SELECT FROM v$open_cursors statement.
		/// </summary>

		public void ShowOpenCursors ()
		{
			string formattedSQL =
				  "SELECT count(*), sql_text\n"
				+ "  FROM v$open_cursor\n"
				+ " GROUP BY sql_text\n"
				+ " ORDER BY 1 DESC\n";

			InsertText(formattedSQL);

			IsSaved = true;
			SetTitle();

			//Execute(ParseMode.Sequential, editorView.SelectedText, 1);

			Logger.WriteLine("QueryWindow.ShowOpenCursors");
			commander.ExecuteControls.IsEnabled = true;
			SetStatusMessage(RxExecutingStatus);

			resultsView.Clear();

			if (splitContainer.Panel2Collapsed)
				DoToggleResults(null, null);

			splitContainer.SplitterDistance = (int)(splitContainer.Height * 0.2);

			// catch up on Windows messages just to look better
			Application.DoEvents();

			queries.Clear();
			driver.Reset();

			var query = new Database.Query(formattedSQL.Replace("\n", String.Empty));
			var parser = new StatementParser();
			parser.ParseStatement(dbase, query, browser);
			queries.Add(query);

			// execute query collection

			resultsView.Lock();

			driver.Execute(queries, basePath, 1);
		}


		//========================================================================================
		// ShowOpenTransactions()
		//========================================================================================

		/// <summary>
		/// Opens a new query window and executes the SELECT FROM v$transaction statement.
		/// </summary>

		public void ShowOpenTransactions ()
		{
			string formattedSQL =
				  "SELECT vp.spid as pid, S.blocking_session blocker, S.sid, S.serial#,\n"
				+ "       s.osuser, S.username, S.machine, S.program,\n"
				+ "       Q.sql_fulltext as cur_sql, PQ.sql_fulltext as prev_sql,\n"
				+ "       vt.used_urec, vt.start_date\n"
				+ "  FROM v$session S\n"
				+ "  LEFT JOIN v$sqlarea Q  on S.sql_id = Q.sql_id\n"
				+ "  LEFT JOIN v$sqlarea PQ on S.prev_sql_id = PQ.sql_id\n"
				+ "  LEFT JOIN v$process vp on s.paddr = vp.addr\n"
				+ "  LEFT JOIN v$transaction vt on s.saddr = vt.ses_addr\n"
				+ " ORDER BY S.username, S.machine\n";

			InsertText(formattedSQL);

			IsSaved = true;
			SetTitle();

			Logger.WriteLine("QueryWindow.ShowOpenTransactions");
			commander.ExecuteControls.IsEnabled = true;
			SetStatusMessage(RxExecutingStatus);

			resultsView.Clear();

			if (splitContainer.Panel2Collapsed)
				DoToggleResults(null, null);

			splitContainer.SplitterDistance = (int)(splitContainer.Height * 0.25);

			// catch up on Windows messages just to look better
			Application.DoEvents();

			queries.Clear();
			driver.Reset();

			var query = new Database.Query(formattedSQL.Replace("\n", String.Empty));
			var parser = new StatementParser();
			parser.ParseStatement(dbase, query, browser);
			queries.Add(query);

			// execute query collection

			resultsView.Lock();

			driver.Execute(queries, basePath, 1);
		}


		//========================================================================================
		// ShowUserErrors()
		//========================================================================================

		/// <summary>
		/// Opens a new query window and executes the SELECT FROM User_Errors statement.
		/// </summary>

		public void ShowUserErrors ()
		{
			InsertText("SELECT name, line, position, text"
				+ " FROM User_Errors"
				+ " ORDER BY name, line, position;\n");

			IsSaved = true;
			SetTitle();

			//Execute(ParseMode.Sequential, editorView.SelectedText, 1);

			Logger.WriteLine("QueryWindow.ShowUserErrors");
			commander.ExecuteControls.IsEnabled = true;
			SetStatusMessage(RxExecutingStatus);

			resultsView.Clear();

			if (splitContainer.Panel2Collapsed)
				DoToggleResults(null, null);

			splitContainer.SplitterDistance = (int)(splitContainer.Height * 0.2);

			// catch up on Windows messages just to look better
			Application.DoEvents();

			queries.Clear();
			driver.Reset();

			var query = new Database.Query("SELECT name, line, position, text"
				+ " FROM User_Errors"
				+ " ORDER BY name, line, position");

			(new StatementParser()).ParseStatement(dbase, query, browser);
			queries.Add(query);

			query.Messages.AddRange(QueryDriver.GetUserErrors(dbase));
			query.HideResults = true;

			//StatementParser parser = new StatementParser();
			//parser.ParseNotification += new NotificationEventHandler(DoParseNotification);
			//StatementCollection statements = GetStatementCollection(parseMode, text);

			//// build collection of parsed queries

			//Database.Query query;
			//System.Collections.Specialized.StringEnumerator e = statements.GetEnumerator();
			//while (e.MoveNext())
			//{
			//    query = new Database.Query(e.Current);
			//    parser.ParseStatement(dbase, query, browser);
			//    queries.Add(query);
			//}

			// execute query collection

			resultsView.Lock();

			driver.Execute(queries, basePath, 1);
		}


		//========================================================================================
		// ToggleTracing()
		//========================================================================================

		public void ToggleTracing (bool enable)
		{
			IsSaved = true;
			SetTitle();

			commander.ExecuteControls.IsEnabled = true;
			SetStatusMessage(RxExecutingStatus);

			if (splitContainer.Panel2Collapsed)
				DoToggleResults(null, null);

			splitContainer.SplitterDistance = (int)(splitContainer.Height * 0.2);

			// catch up on Windows messages just to look better
			Application.DoEvents();

			string sql =
				"alter session set sql_trace = " + (enable ? "true" : "false");

			Logger.WriteLine("QueryWindow.ToggleTracing");
			Logger.WriteLine(sql);

			if (enable)
				resultsView.WriteLine("\nEnabling SQL tracing...\n" + sql);
			else
				resultsView.WriteLine("\nDisabling SQL tracing...\n" + sql);

			queries.Clear();
			driver.Reset();

			var query = new Database.Query(sql);
			(new StatementParser()).ParseStatement(dbase, query, browser);
			queries.Add(query);

			if (enable)
			{
				sql = "select P.tracefile from v$process P, v$session S"
					+ " where S.audsid = userenv('sessionid') and S.paddr = P.addr";
				Logger.WriteLine(sql);

				query = new Database.Query(sql);

				(new StatementParser()).ParseStatement(dbase, query, browser);
				queries.Add(query);
			}

			// execute query collection

			resultsView.Lock();

			driver.Execute(queries, basePath, 1);
		}


		//========================================================================================
		// Execute()
		//========================================================================================

		#region Execute API

		/// <summary>
		/// Handles the Execute command from local tools such as the popup context menu.
		/// </summary>
		/// <param name="sender">The sender of this command</param>
		/// <param name="e">Information related to the command.</param>

		private void DoExecuteQuery (object sender, EventArgs e)
		{
			Execute(ParseMode.None);
		}


		/// <summary>
		///
		/// </summary>
		/// <param name="algorithm"></param>

		public void Execute (ParseMode parseMode)
		{
			int repeatCount = 1;

			if (parseMode == ParseMode.Repeat)
			{
				Dialogs.ExecuteRepeatedDialog dialog = new Dialogs.ExecuteRepeatedDialog();
				DialogResult result = dialog.ShowDialog(this);

				if (result != DialogResult.Cancel)
				{
					repeatCount = dialog.RepeatCount;
				}
			}

			Execute(parseMode, editorView.SelectedText, repeatCount);
		}

		#endregion Execute API


		private void Execute (ParseMode parseMode, string text, int repeat)
		{
			Logger.WriteLine("QueryWindow.Execute");
			commander.ExecuteControls.IsEnabled = true;
			SetStatusMessage(RxExecutingStatus);

			resultsView.Clear();

			if (splitContainer.Panel2Collapsed)
			{
				DoToggleResults(null, null);
			}

			// catch up on Windows messages just to look better
			Application.DoEvents();

			queries.Clear();
			driver.Reset();

			var parser = new StatementParser();
			parser.ParseNotification += new NotificationEventHandler(DoParseNotification);
			StatementCollection statements = GetStatementCollection(parseMode, text);

			// build collection of parsed queries

			Database.Query query;
			System.Collections.Specialized.StringEnumerator e = statements.GetEnumerator();
			while (e.MoveNext())
			{
				query = new Database.Query(e.Current);
				parser.ParseStatement(dbase, query, browser);

				if (parseMode == ParseMode.SqlPlus)
				{
					query.QueryType = QueryType.SqlPlus;
				}

				queries.Add(query);
			}

			// execute query collection

			resultsView.Lock();

			driver.Execute(queries, basePath, repeat);
		}


		#region Execution Event Handlers

		//----------------------------------------------------------------------------------------
		// Singe query completion handler
		//----------------------------------------------------------------------------------------

		private delegate void QueryCompletedDelegate (Database.Query query);


		private void DoQueryCompleted (QueryCompletedEventArgs e)
		{
			if (this.windowIsValid)
			{
				try
				{
					if (!this.IsDisposed)
						this.Invoke(new QueryCompletedDelegate(DoQueryCompleted), new object[] { e.Query });
				}
				catch (Exception)
				{
					// if the user closes the window prior to QueryCompleted
					// then an exception is thrown here.
				}
			}
		}


		private void DoQueryCompleted (Database.Query query)
		{
			resultsView.Add(query);
			SetStatusTime(query.ElapsedTime);
			SetStatusRows(query.AffectedRecords);
		}


		//----------------------------------------------------------------------------------------
		// Query collection completion handler
		//----------------------------------------------------------------------------------------

		private delegate void QueriesCompletedDelegate (QueryCollection queries);


		private void DoQueriesCompleted (QueriesCompletedEventArgs e)
		{
			if (!this.IsDisposed)
			{
				this.Invoke(
					new QueriesCompletedDelegate(DoQueriesCompleted), new object[] { e.Queries });
			}
		}


		private void DoQueriesCompleted (QueryCollection queries)
		{
			resultsView.MoveHome();
			resultsView.Unlock();

			commander.ExecuteControls.IsEnabled = false;

			SetStatusMessage(RxReadyStatus);
			SetStatusTime(queries.TotalTime);
			SetStatusRows(queries.TotalRecords);

			Statistics statistics = queries.Statistics;
			if (statistics != null)
			{
				resultsView.ReportStatistics(statistics);
			}

			if (queries.HasPlans)
			{
				resultsView.ReportPlans(queries);
			}

			editorView.Focus();
		}


		//----------------------------------------------------------------------------------------
		// Parser notification handlers
		//----------------------------------------------------------------------------------------

		private void DoParseNotification (NotificationEventArgs e)
		{
			resultsView.ReportMessage(e.Message);
		}

		#endregion Execution Event Handlers


		//========================================================================================
		// RunProcedure()
		//========================================================================================

		private void RunProcedure (SchemataProcedure node)
		{
			Dialogs.ParametersDialog dialog = new Dialogs.ParametersDialog(node);
			dialog.FormClosed += new FormClosedEventHandler(DoParametersDialogClosed);
			dialog.Show(this);
		}


		private void DoParametersDialogClosed (object sender, FormClosedEventArgs e)
		{
			Dialogs.ParametersDialog dialog = (Dialogs.ParametersDialog)sender;
			if (dialog.DialogResult == DialogResult.OK)
			{
				queries.Clear();
				driver.Reset();
				resultsView.Clear();

				if (splitContainer.Panel2Collapsed)
					DoToggleResults(null, null);

				// catch up on Windows messages just to look better
				Application.DoEvents();

				SetStatusMessage(RxExecutingStatus);

				Database.Query query = new Database.Query(dialog.ProcedureName, dialog.Parameters);
				queries.Add(query);

				string sql = (new StatementParser()).BuildProcedureStatement(query);
				editorView.InsertText("\n" + sql + "\n\n");

				resultsView.Lock();

				driver.Execute(queries, basePath, 1);
			}
		}


		//========================================================================================
		// Parse()
		//========================================================================================

		private void DoParseQuery (object sender, EventArgs e)
		{
			Parse();
		}


		/// <summary>
		/// Parse the currently selected text for validity against the provider.
		/// </summary>

		public void Parse ()
		{
			var parser = new StatementParser();

			// TODO: ...
		}


		//========================================================================================
		// GetStatementCollection()
		//========================================================================================

		/// <summary>
		/// Builds a statement collection from the selected text and formats it according
		/// to user options.  If there are more one statement, the user may be prompted
		/// to select a specific parse mode.
		/// </summary>
		/// <param name="parseMode">The default or explicit parse mode.</param>
		/// <param name="text">The text to parse into statements.</param>
		/// <returns>A StatementCollection of one or more formatted statements.</returns>

		private StatementCollection GetStatementCollection (ParseMode parseMode, string text)
		{
			var parser = new StatementParser();
			StatementCollection statements = parser.Parse(text);

			if ((parseMode == ParseMode.None) && (statements.Count > 1))
			{
				parseMode = (ParseMode)UserOptions.GetEnumeration(
					"connections/parseMode", typeof(ParseMode));

				if (parseMode == ParseMode.Prompt)
				{
					var dialog = new Dialogs.AlgorithmDialog(statements.Count, false);
					DialogResult result = dialog.ShowDialog(this);

					if (result == DialogResult.Cancel)
					{
						return null;
					}

					parseMode = dialog.Mode;

					if (dialog.StoreSelection)
					{
						UserOptions.SetValue("connections/parseMode", dialog.Mode.ToString());
						UserOptions.Save();
					}
				}
			}


			if (parseMode == ParseMode.Wrapped)
			{
				statements.Wrap(dbase.DefaultSchema);
			}
			else if (parseMode == ParseMode.Block)
			{
				statements.Combine();
			}

			return statements;
		}


		//========================================================================================
		// SetOptions()
		//========================================================================================

		public void SetOptions ()
		{
			editorView.SetOptions();
			resultsView.SetOptions();
		}
	}
}