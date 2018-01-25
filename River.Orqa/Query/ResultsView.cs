//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Manages the results pane of a Query window including text or XML output, grid output,
// status and error messages, statistics and query plan views.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2004      New
// 01-Aug-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Query
{
	using System;
	using System.Collections.Specialized;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using River.Orqa.Database;
	using River.Orqa.Options;
	using River.Orqa.Resources;
	using System.IO;


	//********************************************************************************************
	// class ResultsView
	//********************************************************************************************

	/// <summary>
	/// Manages the result pane of a Query window.
	/// </summary>

	internal partial class ResultsView : UserControl, IEditor
	{
		// resource format strings
		private static string RxCompleted;
		private static string RxRowsAffected;
		private static string RxTotalRowsAffected;
		private static string RxNoMessages;

		private readonly string CR = System.Environment.NewLine;

		private IEditor notepad;				// the currently selected tab
		private ResultTarget resultTarget;		// current output target (text/grid/xml)
		private int resultCount;				// number of queries to report
		private int messageCount;				// number of distinct messages


		// Events

		public event OpeningOptionsEventHandler OpeningOptions;
		public event EventHandler Saving;
		public event EventHandler ToggleResults;


		//========================================================================================
		// Constructor
		//========================================================================================

		static ResultsView ()
		{
			Translator translator = new Translator("Query");
			RxCompleted = translator.GetString("Completed");
			RxNoMessages = translator.GetString("NoMessages");
			RxRowsAffected = translator.GetString("RowsAffected");
			RxTotalRowsAffected = translator.GetString("TotalRowsAffected");
			translator = null;
		}


		public ResultsView ()
		{
			InitializeComponent();

			textpad.Enter += new EventHandler(DoGotFocus);
			textpad.Saving += new EventHandler(DoSaving);
			textpad.TextChanged += new EventHandler(DoTextChanged);
			textpad.ToggleResults += new EventHandler(DoToggleResults);
			grid.Enter += new EventHandler(DoGotFocus);
			msgpad.Enter += new EventHandler(DoGotFocus);
			msgpad.TextChanged += new EventHandler(DoTextChanged);
			msgpad.ToggleResults += new EventHandler(DoToggleResults);
			planView.Enter += new EventHandler(DoGotFocus);
			statisticsView.Enter += new EventHandler(DoGotFocus);

			tabset.TabPages.Remove(gridPage);
			tabset.TabPages.Remove(planPage);
			tabset.TabPages.Remove(statsPage);

			textPage.Selected = true;

			this.notepad = textpad;
			this.resultTarget = ResultTarget.Text;
			this.resultCount = 0;
			this.messageCount = 0;
		}


		private void DoToggleResults (object sender, EventArgs e)
		{
			if (ToggleResults != null)
				ToggleResults(sender, e);
		}


		//========================================================================================
		// IEditor
		//========================================================================================

		#region IEditor

		public bool CanPaste
		{
			get { return (notepad == null ? false : notepad.CanPaste); }
		}


		public bool CanRedo
		{
			get { return (notepad == null ? false : notepad.CanRedo); }
		}


		public bool CanUndo
		{
			get { return (notepad == null ? false : notepad.CanUndo); }
		}


		public int Column
		{
			get { return (notepad == null ? 0 : notepad.Column); }
		}


		public bool IsFocused
		{
			get { return tabset.SelectedTab.Controls[0].Focused; }
		}


		public bool IsSaved
		{
			get { return false; }
			set { }
		}


		public bool HasContent
		{
			get { return (notepad == null ? false : notepad.HasContent); }
		}


		public bool HasSelection
		{
			get { return (notepad == null ? false : notepad.HasSelection); }
		}


		public int Line
		{
			get { return (notepad == null ? 0 : notepad.Line); }
		}


		public ResultTarget ResultTarget
		{
			set
			{
				if (resultTarget != value)
				{
					if ((value != ResultTarget.Grid) && (resultTarget == ResultTarget.Grid))
					{
						tabset.TabPages.Remove(gridPage);
						tabset.TabPages.Insert(0, textPage);
						tabset.SelectedTab = textPage;
					}
					else if (value == ResultTarget.Grid)
					{
						tabset.TabPages.Remove(textPage);
						tabset.TabPages.Insert(0, gridPage);
						tabset.SelectedTab = gridPage;
					}

					if (value == ResultTarget.Text)
					{
						textPage.Icon = Icon.FromHandle(((Bitmap)resultTargetIcons.Images[0]).GetHicon());
						textPage.Title = "Text";
					}
					else if (value == ResultTarget.Xml)
					{
						textPage.Icon = Icon.FromHandle(((Bitmap)resultTargetIcons.Images[2]).GetHicon());
						textPage.Title = "XML";
					}

					resultTarget = value;
				}
			}
		}


		public int TabStops
		{
			get { return notepad.TabStops; }
			set { notepad.TabStops = value; }
		}


		public string[] TextLines
		{
			get { return notepad.TextLines; }
		}


		public bool WhitespaceVisible
		{
			get { return false; }
		}


		public void DoCut (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoCut(sender, e);
		}


		public void DoCopy (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoCopy(sender, e);
		}


		public void DoClear (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoClear(sender, e);
		}


		public void DoFind (object sender, EventArgs e)
		{
			if ((notepad != null) && (notepad is River.Orqa.Editor.ISearch))
			{
				River.Orqa.Editor.ISearch searcher = (River.Orqa.Editor.ISearch)notepad;
				searcher.SearchDialog.Execute(searcher, false, false);
			}
		}


		public void DoGotoLine (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoGotoLine(sender, e);
		}


		public void DoMakeLowercase (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoMakeLowercase(sender, e);
		}


		public void DoMakeUppercase (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoMakeUppercase(sender, e);
		}


		public void DoPaste (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoPaste(sender, e);
		}


		public void DoRedo (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoRedo(sender, e);
		}


		public void DoSelectAll (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoSelectAll(sender, e);
		}


		public void DoToggleWhitespace (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoToggleWhitespace(sender, e);
		}


		public void DoUndo (object sender, EventArgs e)
		{
			if (notepad != null)
				notepad.DoUndo(sender, e);
		}


		public void SaveFile (string filename)
		{
			if (tabset.SelectedTab == textPage)
			{
				textpad.SaveFile(filename);
			}
			else if (tabset.SelectedTab == gridPage)
			{
				Notepad pad = new Notepad();

				ResultFormat format = (ResultFormat)UserOptions.GetEnumeration(
					"results/general/format", typeof(ResultFormat));

				if (format == ResultFormat.ColumnAligned)
					ReportText((Database.Query)grid.Tag, pad);
				else
					ReportDelimitedText((Database.Query)grid.Tag, format, pad);

				pad.SaveFile(filename);
			}
		}

		public void RevertColor () { }

		public void Write (string text)
		{
			if ((resultTarget == Database.ResultTarget.Text)
			 || (resultTarget == Database.ResultTarget.Xml))
			{
				textpad.Write(text);
			}
		}

		public void WriteLine ()
		{
			if ((resultTarget == Database.ResultTarget.Text)
			 || (resultTarget == Database.ResultTarget.Xml))
			{
				textpad.WriteLine();
			}
		}

		public void WriteLine (string text)
		{
			if ((resultTarget == Database.ResultTarget.Text)
			 || (resultTarget == Database.ResultTarget.Xml))
			{
				textpad.WriteLine(text);
			}
		}
	
		public void WriteNote (string text)
		{
		}

		#endregion IEditor


		//========================================================================================
		// Add()
		//========================================================================================

		/// <summary>
		/// Displays the results of the given query.
		/// </summary>
		/// <param name="query">The query to report.</param>

		public void Add (Database.Query query)
		{
			Logger.WriteLine("Adding result with target [" + resultTarget + "]");

			Notepad msgTarget = textpad;

			if ((int)resultTarget < 0)				// TODO: why!?
				resultTarget = ResultTarget.Text;

			if (!query.HideResults)
			{
				if (query.QueryType == QueryType.SqlPlus)
				{
					LoadFile(query.OutputFilename);
				}
				else switch (resultTarget)
				{
					case ResultTarget.Text:
						ResultFormat format = (ResultFormat)UserOptions.GetEnumeration(
							"results/general/format", typeof(ResultFormat));

						if (format == ResultFormat.ColumnAligned)
							ReportText(query);
						else
							ReportDelimitedText(query, format);
						break;

					case ResultTarget.Grid:
						ReportGrid(query);
						msgTarget = msgpad;
						break;

					case ResultTarget.Xml:
						ReportXml(query);
						break;
				}
			}

			if (query.Messages.Count > 0)
			{
				ReportMessages(query.Messages, msgTarget, query.HideResults);
			}
			else if (query.HideResults)
			{
				textpad.WriteNote(CR + RxNoMessages);
			}

			if (UserOptions.GetBoolean("results/general/dbmsOutput"))
				if (query.OutputLines.Count > 0)
					ReportOutputLines(query.OutputLines);

			resultCount++;
		}


		//========================================================================================
		// LoadFile()
		//========================================================================================

		public void LoadFile (string filename)
		{
			using (var reader = new StreamReader(filename))
			{
				string line;
				while (!reader.EndOfStream)
				{
					if ((line = reader.ReadLine()) != null)
					{
						textpad.WriteLine(line);
					}
				}

				reader.Close();
			}
		}


		//========================================================================================
		// ReportText()
		//========================================================================================

		#region ReportText

		private void ReportText (Database.Query query)
		{
			ReportText(query, textpad);
		}


		private void ReportText (Database.Query query, Notepad pad)
		{
			bool outputQuery = UserOptions.GetBoolean("results/general/outputQuery");
			bool printHeader = UserOptions.GetBoolean("results/general/printHeader");
			bool rightAlign = UserOptions.GetBoolean("results/general/rightAlign");
			bool cleanNewlines = UserOptions.GetBoolean("results/general/cleanNewlines");
			bool ratype = rightAlign;
			int totalRows = 0;

			OraData data = query.Data;

			var line = new StringBuilder();
			string text;

			if (resultCount > 0)
			{
				var color = pad.ForeColor;
				pad.ForeColor = Color.FromArgb(0x6E96BE); // "Number-style" blue
				pad.WriteLine(CR + new String((char)0x2550, 100) + CR);
				pad.ForeColor = color;
			}

			if (outputQuery)
				PrintQueryBox(query.SQL, pad);

			if (query.HasOutputs)
				PrintOutputParameters(query, pad);

			for (int t = 0; t < data.Count; t++)
			{
				OraTable table = data[t];

				if (t > 0)
				{
					var color = pad.ForeColor;
					pad.ForeColor = Color.FromArgb(0x6E96BE); // "Number-style" blue
					pad.WriteLine(CR + new String((char)0x2550, 100));
					pad.ForeColor = color;
				}

				int[] widths = null;
				if (table.Count > 0)
				{
					widths = MeasureColumns(table);

					if (printHeader)
					{
						BuildTextHeader(table, widths, pad);
					}

					foreach (OraRow row in table)
					{
						line = new StringBuilder();

						for (int i = 0; i < table.FieldCount; i++)
						{
							if (i > 0)
							{
								line.Append(" ");
							}

							if (row[i] == null)
							{
								text = "...";
							}
							else if (table.Schema[i].DataType == typeof(Byte[]))
							{
								if (row[i].GetType() == typeof(DBNull))
								{
									text = String.Empty;
								}
								else
								{
									Byte[] bytes = (Byte[])row[i];
									text = String.Join(
										String.Empty,
										bytes.Select(b => b.ToString("X2")).ToArray());
								}
							}
							else
							{
								text = row[i].ToString();
							}

							if (cleanNewlines)
							{
								text = text.Replace("\n", String.Empty).Replace("\r", String.Empty);
							}

							if (text.Length > widths[i])
							{
								line.Append(text.Substring(0, widths[i]));
							}
							else
							{
								if (rightAlign)
								{
									TypeCode code = Type.GetTypeCode(row[i].GetType());
									ratype = ((code != TypeCode.Boolean) &&
										(code != TypeCode.Char) &&
										(code != TypeCode.String));
								}

								if (ratype)
									line.Append(new String(' ', widths[i] - text.Length) + text);
								else
									line.Append(text + new String(' ', widths[i] - text.Length));
							}
						}

						pad.WriteLine(line.ToString());
					}
				}

				pad.WriteLine();
				pad.WriteNote(String.Format(RxRowsAffected, table.Count));

				totalRows += table.Count;
			}

			if (data.Count == 0)
			{
				totalRows = query.AffectedRecords;
			}

			if (data.Count != 1)
			{
				string msg;
				if (totalRows < 0)
					msg = RxCompleted;
				else
					msg = String.Format(RxTotalRowsAffected, totalRows);

				pad.WriteNote(CR + msg);
			}
		}


		private int[] MeasureColumns (OraTable table)
		{
			int MAX = UserOptions.GetInt("results/general/maxChar");

			int[] widths = new int[table.Schema.FieldCount];
			int width;
			string text;

			int rowcount = table.Count;

			foreach (OraRow row in table)
			{
				for (int i = 0; i < table.Schema.FieldCount; i++)
				{
					width = table.Schema.GetName(i).Length;

					if (width > MAX)
					{
						width = MAX;
					}
					else
					{
						if (row[i] == null)
						{
							//width = 0;
						}
						else if (table.Schema.GetType(i) == typeof(Byte[]))
						{
							try
							{
								if (row[i].GetType() == typeof(DBNull))
								{
									width = 0;
								}
								else
								{
									// x2 so we provide room to convert bytes to Hex
									width = ((Byte[])row[i]).Length * 2;
								}
							}
							catch (Exception exc)
							{
								//select name, typeicon from con_itemtype;
								string m = exc.Message;
							}
						}
						else
						{
							text = row[i].ToString();
							if (text.Length > width)
							{
								width = (text.Length < MAX ? text.Length : MAX);
							}
						}
					}

					if (widths[i] < width)
					{
						widths[i] = width;
					}
				}
			}

			return widths;
		}


		private void BuildTextHeader (OraTable table, int[] widths, Notepad pad)
		{
			var head = new StringBuilder();
			var line = new StringBuilder();

			for (int i = 0; i < table.Schema.FieldCount; i++)
			{
				if (i > 0)
				{
					head.Append(" ");
					line.Append(" ");
				}

				string name = table.Schema.GetName(i);
				head.Append(name);
				line.Append(new String('-', name.Length));

				if (name.Length < widths[i])
				{
					int extra = widths[i] - name.Length;



					head.Append(new String(' ', extra));
					line.Append(new String('-', extra));
				}
			}

			var savecolor = pad.ForeColor;
			pad.ForeColor = FontsAndColors.IdentifierForeground;
			pad.WriteLine(head.ToString());
			pad.WriteLine(line.ToString());
			pad.ForeColor = savecolor;
		}


		private void PrintQueryBox (string sql, IEditor target)
		{
			const char TopLeft = (char)0x250C;
			const char TopRight = (char)0x2510;
			const char BottomLeft = (char)0x2514;
			const char BottomRight = (char)0x2518;
			const char Horizontal = (char)0x2500;
			const char Vertical = (char)0x2502;

			var box = new StringBuilder();
			string line = new String(Horizontal, sql.Length);

			box.Append(CR + TopLeft + line + TopRight + CR);
			box.Append(Vertical + sql + Vertical + CR);
			box.Append(BottomLeft + line + BottomRight + CR + CR);

			target.Write(box.ToString());
		}


		//========================================================================================
		// ReportDelimitedText()
		//========================================================================================

		private void ReportDelimitedText (Database.Query query, ResultFormat format)
		{
			ReportDelimitedText(query, format, textpad);
		}


		// used by both delimited-text and grid-export...
		private void ReportDelimitedText (Database.Query query, ResultFormat format, Notepad pad)
		{
			bool outputQuery = UserOptions.GetBoolean("results/general/outputQuery");
			bool printHeader = UserOptions.GetBoolean("results/general/printHeader");

			StringBuilder sult = new StringBuilder();
			StringBuilder line = new StringBuilder();
			string delimiter = ",";

			switch (format)
			{
				case ResultFormat.CommaDelimited:
					delimiter = ",";
					break;

				case ResultFormat.TabDelimited:
					delimiter = new String((char)0x09, 1);
					break;

				case ResultFormat.SpaceDelimited:
					delimiter = " ";
					break;

				case ResultFormat.CustomDelimited:
					delimiter = UserOptions.GetString("results/general/delimiter");
					break;
			}

			if (outputQuery)
				PrintQueryBox(query.SQL, pad);

			OraData data = query.Data;

			foreach (OraTable table in data)
			{
				if (printHeader && (table.Count > 0))
				{
					for (int i = 0; i < table.FieldCount; i++)
					{
						if (i > 0) line.Append(delimiter);
						line.Append(table.Schema[i].ColumnName);
					}

					sult.Append(line.ToString() + CR);
				}

				foreach (OraRow row in table)
				{
					line.Length = 0;

					for (int i = 0; i < table.FieldCount; i++)
					{
						if (i > 0) line.Append(delimiter);
						if (row[i] != null)
						{
							line.Append(row[i].ToString());
						}
					}

					sult.Append(line.ToString() + CR);
				}

				pad.Write(sult.ToString());
				pad.WriteNote(CR + "(" + query.AffectedRecords + " row(s) affected)");
			}
		}

		#endregion ReportText


		//========================================================================================
		// ReportGrid()
		//========================================================================================

		#region ReportGrid

		private void ReportGrid (Database.Query query)
		{
			DataGridView g = (resultCount == 0 ? grid : AddGrid());

			if (query.Data.Count > 0)
			{
				OraTable table = query.Data[0];
				for (int c = 0; c < table.Schema.FieldCount; c++)
				{
					g.Columns.Add(table.Schema[c].ColumnName, table.Schema[c].ColumnName);
				}

				foreach (var row in table)
				{
					var values = new List<String>();
					foreach (object value in row)
					{
						if (value == null)
						{
							values.Add("...");
						}
						else
						{
							values.Add(value.ToString());
						}
					}

					g.Rows.Add(values.ToArray());
				}
			}

			g.Tag = query;

			bool outputQuery = UserOptions.GetBoolean("results/general/outputQuery");
			if (outputQuery)
			{
				PrintQueryBox(query.SQL, msgpad);
			}

			msgpad.WriteNote(CR + "(" + query.AffectedRecords + " row(s) affected)");
		}


		private DataGridView AddGrid ()
		{
			DataGridView newgrid = new DataGridView();
			newgrid.AllowUserToAddRows = false;
			newgrid.AllowUserToDeleteRows = false;
			newgrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			newgrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			newgrid.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			newgrid.BorderStyle = BorderStyle.None;
			newgrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
			newgrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			newgrid.ContextMenuStrip = contextMenu;
			newgrid.Dock = DockStyle.Fill;
			newgrid.MultiSelect = false;
			newgrid.Name = "grid" + resultCount;
			newgrid.ReadOnly = true;
			newgrid.RowHeadersVisible = false;
			newgrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			DataGridViewCellStyle style = new DataGridViewCellStyle();
			style.SelectionBackColor = FontsAndColors.SelectedTextBackground;
			//style.SelectionForeColor = Color.Black;
			newgrid.RowsDefaultCellStyle = style;

			newgrid.Enter += new EventHandler(DoGotFocus);

			Control container = gridPage;
			bool found = false;

			while (!found)
			{
				if (!(found = (container.Controls[0] is DataGridView)))
				{
					container = ((SplitContainer)container.Controls[0]).Panel2;
				}
			}

			grid = (DataGridView)container.Controls[0];

			SplitContainer splitter = new SplitContainer();
			splitter.Dock = DockStyle.Fill;
			splitter.Orientation = Orientation.Horizontal;

			container.Controls.Remove(grid);
			container.Controls.Add(splitter);
			splitter.Panel1.Controls.Add(grid);
			splitter.Panel2.Controls.Add(newgrid);

			return newgrid;
		}

		#endregion ReportGrid


		//========================================================================================
		// ReportXml()
		//========================================================================================

		private void ReportXml (Database.Query query)
		{
			if (query.HasOutputs)
				PrintOutputParameters(query, textpad);

			var xml = new StringBuilder(query.Data.GetXml().ToString());
			xml.Replace("&lt;", "<");
			xml.Replace("&gt;", ">");

			textpad.WriteLine(xml.ToString());
			textpad.WriteLine();
			textpad.WriteNote("(" + query.AffectedRecords + " row(s) affected)");
		}


		//========================================================================================
		// PrintOutputParameters()
		//========================================================================================

		private void PrintOutputParameters (Database.Query query, Notepad output)
		{
			var savecolor = output.ForeColor;

			foreach (Database.Parameter parameter in query.Parameters)
			{
				if (((parameter.Direction &
					 (ParameterDirection.Output | ParameterDirection.ReturnValue)) > 0) &&
					 (parameter.DataType != Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor))
				{
					output.ForeColor = Color.FromArgb(0x6E96BE); // "Number-style" blue;
					output.WriteLine(
						"Output parameter: '" + parameter.Name
						+ "' = [" + parameter.Value.ToString().Trim() + "]");
				}
			}

			output.ForeColor = savecolor;
		}


		//========================================================================================
		// ReportMessages()
		//========================================================================================

		#region ReportMessages

		private void ReportMessages (MessageCollection messages, IEditor editor, bool showEmpty)
		{
			Color color = Color.Empty;
			IEditor target = null;

			foreach (Database.Message message in messages)
			{
				switch (message.Type)
				{
					case Database.Message.MessageType.Error:
						color = FontsAndColors.MessageErrorForeground;
						target = editor;
						if (resultTarget == ResultTarget.Grid)
							messagePage.Selected = true;
						break;

					case Database.Message.MessageType.Info:
						color = FontsAndColors.MessageInfoForeground;
						target = editor;
						break;

					case Database.Message.MessageType.State:
						color = FontsAndColors.MessageStatusForeground;
						target = msgpad;
						break;

					case Database.Message.MessageType.User:
						color = FontsAndColors.MessageUserForeground;
						target = editor;
						if (resultTarget == ResultTarget.Grid)
							messagePage.Selected = true;
						break;
				}

				var savecolor = target.ForeColor;
				target.ForeColor = color;
				target.WriteLine("[" + (resultCount + 1) + "] " + message.Text);
				target.ForeColor = savecolor;

				if (target == msgpad)
				{
					messageCount++;
					messagePage.Title = "Messages (" + messageCount + ")";
				}
			}
		}


		/// <summary>
		/// Direct access to message pane.
		/// </summary>
		/// <param name="message">The message to display.</param>

		public void ReportMessage (Database.Message message)
		{
			Color color = Color.Empty;

			switch (message.Type)
			{
				case Database.Message.MessageType.Error:
					color = FontsAndColors.MessageErrorForeground;
					break;

				case Database.Message.MessageType.Info:
					color = FontsAndColors.MessageInfoForeground;
					break;

				case Database.Message.MessageType.State:
					color = FontsAndColors.MessageStatusForeground;
					break;

				case Database.Message.MessageType.User:
					color = FontsAndColors.MessageUserForeground;
					break;
			}

			var savecolor = msgpad.ForeColor;
			msgpad.ForeColor = color;
			msgpad.WriteLine(">> " + message.Text);
			msgpad.ForeColor = savecolor;

		}


		//========================================================================================
		//========================================================================================

		private void ReportOutputLines (StringCollection lines)
		{
			if (lines.Count > 0)
			{
				var savecolor = notepad.ForeColor;
				notepad.ForeColor = FontsAndColors.IdentifierForeground;
				notepad.WriteLine(CR + "OUTPUT:" + CR);

				System.Collections.Specialized.StringEnumerator e = lines.GetEnumerator();
				while (e.MoveNext())
				{
					notepad.Write(e.Current);
				}

				notepad.ForeColor = savecolor;

				messageCount += lines.Count;
				messagePage.Title = "Messages (" + messageCount + ")";
			}
		}

		#endregion ReportMessages


		//========================================================================================
		// Clear()
		//========================================================================================

		public void Clear ()
		{
			textpad.Clear();
			msgpad.Clear();

			if (gridPage.Controls[0] is SplitContainer)
			{
				gridPage.Controls.RemoveAt(0);
				gridPage.Controls.Add(grid);
			}

			grid.DataSource = null;

			planView.Clear();

			if (resultTarget == ResultTarget.Grid)
			{
				gridPage.Selected = true;
			}
			else
			{
				textPage.Selected = true;
			}

			if (!UserOptions.RunStatistics)
				if (tabset.TabPages.Contains(statsPage))
					tabset.TabPages.Remove(statsPage);

			if (!UserOptions.RunExplainPlan)
				if (tabset.TabPages.Contains(planPage))
					tabset.TabPages.Remove(planPage);

			resultCount = 0;
			messageCount = 0;
			messagePage.Title = "Messages";
		}


		//========================================================================================
		// MoveHome()
		//========================================================================================

		public void MoveHome ()
		{
			if (resultTarget == ResultTarget.Grid)
			{
				//bool isEmpty = (grid.DataSource == null);
				//if (!isEmpty)
				//    isEmpty = (((DataSet)grid.DataSource).Tables[0].Rows.Count == 0);

				//if (isEmpty)
				//{
				//    msgTab.Selected = !isExplained;
				//    notepad.Select(0, 0);
				//    notepad.ScrollToCaret();
				//}
				//else
				//{
				//    grid.Select(0);
				//}
			}
			else
			{
				textpad.MoveHome();
			}
		}


		public void MoveToMessageTab ()
		{
			messagePage.Selected = true;
		}


		//========================================================================================
		// ReportPlans()
		//========================================================================================

		public void ReportPlans (QueryCollection queries)
		{
			if (!tabset.Contains(planPage))
			{
				// always append to end of tab set
				tabset.TabPages.Add(planPage);
			}

			planView.ReportPlans(queries);
			planPage.Selected = true;
		}


		//========================================================================================
		// ReportStatistics()
		//========================================================================================

		public void ReportStatistics (Database.Statistics statistics)
		{
			if (!tabset.Contains(statsPage))
			{
				if (tabset.Contains(planPage))
				{
					tabset.TabPages.Insert(2, statsPage);
				}
				else
				{
					tabset.TabPages.Add(statsPage);
				}
			}

			statisticsView.Populate(statistics);
		}


		//========================================================================================
		// DoGotFocus()
		//========================================================================================

		private void DoGotFocus (object sender, EventArgs e)
		{
			this.OnGotFocus(e);
		}


		//========================================================================================
		// DoOpeningOptions()
		//========================================================================================

		private void DoOpeningOptions (object sender, OpeningOptionsEventArgs e)
		{
			if (OpeningOptions != null)
			{
				OpeningOptions(sender, e);
			}
		}


		//========================================================================================
		// DoSaving()
		//========================================================================================

		private void DoSaving (object sender, EventArgs e)
		{
			if (Saving != null)
			{
				Saving(this, e);
			}
		}


		//========================================================================================
		// DoOptions()
		//========================================================================================

		private void DoOptions (object sender, EventArgs e)
		{
			DoOpeningOptions(sender, new OpeningOptionsEventArgs("/OrqaOptions/results"));
		}


		//========================================================================================
		// DoTabIndexChanged()
		//========================================================================================

		private void DoTabIndexChanged (object sender, EventArgs e)
		{
			if (tabset.SelectedTab == textPage)
			{
				notepad = textpad;
			}
			else if (tabset.SelectedTab == messagePage)
			{
				notepad = msgpad;
			}
			else
			{
				notepad = null;
			}
		}


		private void DoTextChanged (object sender, EventArgs e)
		{
			this.OnTextChanged(e);
		}


		//========================================================================================
		// Window locking - used to prevent autoScroll
		//========================================================================================

		public void Lock ()
		{
			if (resultTarget != ResultTarget.Grid)
			{
				textpad.Lock();
			}
		}


		public void Unlock ()
		{
			textpad.Unlock();
		}


		//========================================================================================
		// SetOptions()
		//========================================================================================

		public void SetOptions ()
		{
			/*
			<results resx="ResultsOptions" icon="3" ref="results/general">
			  <resultFonts resx="ResultsFontOptions" icon="5" sheet="River.Orqa.Options.ResultsFontSheet">
			    <textFont family="Lucida Console" size="8" backColor="Window" foreColor="WindowText" style="0" resx="TextFont" />
			    <gridFont family="Tahoma" size="8" backColor="Window" foreColor="WindowText" style="0" resx="GridFont" />
			    <errorMsgFont family="Lucida Console" size="8" backColor="Window" foreColor="Red" style="0" resx="ErrorMsgFont" />
			    <infoMsgFont family="Lucida Console" size="8" backColor="Window" foreColor="WindowText" style="0" resx="InfoMsgFont" />
			    <userMsgFont family="Lucida Console" size="8" backColor="Window" foreColor="Red" style="0" resx="UserMsgFont" />
			    <stateMsgFont family="Lucida Console" size="8" backColor="Window" foreColor="Green" style="0" resx="StateMsgFont" />
			  </resultFonts>
			</results>
			 */
		}
	}
}
