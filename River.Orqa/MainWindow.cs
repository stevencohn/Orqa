//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// The Orqa main window.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using UI = River.Orqa.Controls;
	using River.Orqa.Database;
	using River.Orqa.Dialogs;
	using River.Orqa.Options;
	using River.Orqa.Query;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class MainForm
	//********************************************************************************************

	internal partial class MainWindow : Form, ICommander
	{
		private UI.Docking.DockingManager dockmgr;		// docking manager
		private Browser.Browser browser;				// object browser dock window
		private Browser.SchemaProperties properties;	// object browser dock window
		private MruManager mru;							// recent file sub menu
		private string traceWindowTitle;				// localized title of trace dock window
		private string browserWindowTitle;				// localized title of browser dock window
		private string propsWindowTitle;				// localized title of browser dock window
		private Size childSize;							// optimal child size
		private Translator translator;					// localization translator
		private bool deferExecution;					// defer exec during toolbar drop-down
		private string firstFilename;					// filename from command line

		private QueryWindow newchild;					// temporary holder to pass around

		private CommandGroup advGroup;					// advanced edit group
		private CommandGroup conGroup;					// connections count group
		private CommandGroup exeGroup;					// executing group
		private CommandGroup pasGroup;					// pasteable group
		private CommandGroup redGroup;					// redo group
		private CommandGroup savGroup;					// saveable group
		private CommandGroup selGroup;					// selection group
		private CommandGroup sqlGroup;					// sql group
		private CommandGroup txtGroup;					// content group
		private CommandGroup undGroup;					// undo group
		private CommandGroup checkers;					// checker group
		private IWorker commandWorker;					// active command target


		//========================================================================================
		// Constructor
		//========================================================================================

		public MainWindow ()
		{
			InitializeComponent();

			translator = new Translator("Orqa");
			browser = new Browser.Browser();
			browser.Size = new Size(320, this.ClientSize.Height);
			browser.Commander = this;

			properties = new Browser.SchemaProperties();

			if (!this.DesignMode)
			{
				browserWindowTitle = translator.GetString("BrowserWindowTitle");
				propsWindowTitle = translator.GetString("PropertiesWindowTitle");
				traceWindowTitle = translator.GetString("TraceWindowTitle");
			}

			BuildCommandGroups();

			SetupDockingEnvironment();
			RestorePosition();

			browser.BrowserActionRequested += new Browser.BrowserTreeEventHandler(DoBrowserActionRequested);
			browser.SchemaSelected += new Browser.BrowserSelectorEventHandler(DoSchemaSelected);
			browser.ScriptOpen += new Browser.ScriptOpenHandler(DoScriptOpen);
			browser.ScriptSelected += new Browser.ScriptSelectionHandler(DoScriptSelected);
			browser.TemplateSelected += new Browser.TemplateSelectionHandler(DoTemplateSelected);

			mru = new MruManager(fileToolStripMenuItem, new MruCallback(LoadRecentFile));
			mru.Load();

			firstFilename = null;

			Statusbar.Strip = statusbar;
			Statusbar.Commander = this;

			this.deferExecution = false;
		}


		public MainWindow (Database.DatabaseConnection con)
			: this()
		{
			if (con != null)
			{
				browser.AddConnection(con);
			}

			OpenQueryWindow(con);
		}


		public MainWindow (Database.DatabaseConnection con, string filename)
			: this()
		{
			if (con != null)
			{
				browser.AddConnection(con);
			}

			OpenQueryWindow(con);

			firstFilename = filename;
		}


		protected override void OnLoad (EventArgs e)
		{
			CalculateOptimalChildSizes();

			if (firstFilename != null)
			{
				LoadRecentFile(firstFilename);
			}
		}


		//========================================================================================
		// Initialization()
		//========================================================================================

		#region Initialization

		//========================================================================================
		// SetupDockingEnvironment()
		//========================================================================================

		private void SetupDockingEnvironment ()
		{
			dockmgr = new UI.Docking.DockingManager(this, UI.Common.VisualStyle.IDE);

			// must disable InsideFill so the DockingManager does not create an opaque
			// background through which we cannot drag/drop files
			dockmgr.InsideFill = false;

			// OuterControl must be set to the first control in the this.Controls collection
			// to NOT move away; otherwise, for example, dockmgr is allowed to dock above the
			// menu bar or below the status bar!  Remember this.Controls is reverse-ordered.

			dockmgr.OuterControl = toolbar;

			dockmgr.ContentHidden += new UI.Docking.DockingManager.ContentHandler(DoContentHidden);
			dockmgr.ContentShown += new UI.Docking.DockingManager.ContentHandler(DoContentShown);

			UI.Docking.Content browse = new UI.Docking.Content(dockmgr, browser, browserWindowTitle, dockImages, 0);
			browse.DisplaySize = new Size(browser.Width, (int)(this.Size.Height * .75));
			dockmgr.Contents.Add(browse);
			UI.Docking.WindowContent browseContent
				= dockmgr.AddContentWithState(browse, UI.Docking.State.DockLeft);

			UI.Docking.Content props = new UI.Docking.Content(dockmgr, properties, propsWindowTitle, dockImages, 1);
			props.DisplaySize = new Size(browser.Width, this.Size.Height - browse.DisplaySize.Height);
			dockmgr.Contents.Add(props);
			dockmgr.AddContentToZone(props, browseContent.ParentZone, 1);

			UI.Docking.Content debug = new UI.Docking.Content(dockmgr, Logger.Instance, traceWindowTitle, dockImages, 2);
			dockmgr.Contents.Add(debug);
			dockmgr.AddContentWithState(debug, UI.Docking.State.DockBottom);

			dockmgr.HideContent(props);
			dockmgr.HideContent(debug);
		}


		private void DoContentHidden (UI.Docking.Content c, EventArgs e)
		{
			if (c.Title.Equals(browserWindowTitle))
			{
				showHideObjectBrowserToolStripMenuItem.Text = translator.GetString("ShowObjectBrowserMenu");
				showHideObjectBrowserToolStripMenuItem.Checked = false;
				browserToolStripButton.Checked = false;
			}
			else if (c.Title.Equals(propsWindowTitle))
			{
				viewPropsToolStripMenuItem.Text = translator.GetString("ShowPropertiesMenu");
				viewPropsToolStripMenuItem.Checked = false;
			}
			else if (c.Title.Equals(traceWindowTitle))
			{
				Logger.IsEnabled = false;
				showDebuggerToolStripMenuItem.Text = translator.GetString("ShowDebuggerMenu");
				showDebuggerToolStripMenuItem.Checked = false;
			}
		}


		private void DoContentShown (UI.Docking.Content c, EventArgs e)
		{
			if (c.Title.Equals(browserWindowTitle))
			{
				showHideObjectBrowserToolStripMenuItem.Text = translator.GetString("HideObjectBrowserMenu");
				showHideObjectBrowserToolStripMenuItem.Checked = true;
				browserToolStripButton.Checked = true;
			}
			else if (c.Title.Equals(propsWindowTitle))
			{
				viewPropsToolStripMenuItem.Text = translator.GetString("HidePropertiesMenu");
				viewPropsToolStripMenuItem.Checked = true;
			}
			else if (c.Title.Equals(traceWindowTitle))
			{
				Logger.IsEnabled = true;
				showDebuggerToolStripMenuItem.Text = translator.GetString("HideDebuggerMenu");
				showDebuggerToolStripMenuItem.Checked = true;
			}
		}


		//========================================================================================
		// BuildCommandGroups()
		//========================================================================================

		private void BuildCommandGroups ()
		{
			advGroup = new CommandGroup();
			conGroup = new CommandGroup();
			exeGroup = new CommandGroup();
			pasGroup = new CommandGroup();
			redGroup = new CommandGroup();
			savGroup = new CommandGroup();
			selGroup = new CommandGroup();
			sqlGroup = new CommandGroup();
			txtGroup = new CommandGroup();
			undGroup = new CommandGroup();
			checkers = new CommandGroup();

			// File
			conGroup.Join(disconnectToolStripMenuItem, disconnectToolStripButton);
			conGroup.Join(disconnectAllToolStripMenuItem);
			savGroup.Join(saveToolStripMenuItem, saveToolStripButton);
			txtGroup.Join(saveAsToolStripMenuItem);
			savGroup.Join(saveAllQueriesToolStripMenuItem);
			txtGroup.Join(printToolStripMenuItem);
			txtGroup.Join(printPreviewToolStripMenuItem);
			txtGroup.Join(pageSetupToolStripMenuItem);

			// Edit
			undGroup.Join(undoToolStripMenuItem, undoToolStripButton);
			redGroup.Join(redoRtoolStripMenuItem, redoToolStripButton);
			selGroup.Join(cutToolStripMenuItem, cutToolStripButton);
			selGroup.Join(copyToolStripMenuItem, copyToolStripButton);
			pasGroup.Join(pasteToolStripMenuItem, pasteToolStripButton);
			txtGroup.Join(clearWindowToolStripMenuItem, clearToolStripButton);
			txtGroup.Join(findToolStripMenuItem, findToolStripButton);
			txtGroup.Join(selectAllToolStripMenuItem);
			txtGroup.Join(gotoLineToolStripMenuItem);
			txtGroup.Join(makeLowercaseToolStripMenuItem);
			txtGroup.Join(makeUppercaseToolStripMenuItem);
			advGroup.Join(toggleWhitespaceToolStripMenuItem);
			checkers.AddChecker("ToggleWhitespace", toggleWhitespaceToolStripMenuItem);

			// Query
			conGroup.Join(changeDatabaseToolStripMenuItem);
			txtGroup.Join(parseToolStripMenuItem, parseToolStripButton);
			txtGroup.Join(executeToolStripMenuItem, executeToolStripButton);
			txtGroup.Join(executeasWrappedProcedureToolStripMenuItem);
			txtGroup.Join(executeasAnonymousBlockToolStripMenuItem);
			txtGroup.Join(executeSqlPlusMenuItem);
			txtGroup.Join(executeRepeatedToolStripMenuItem);
			exeGroup.Join(cancelExecutingQueryToolStripMenuItem, cancelExecutingQueryToolStripButton);
			txtGroup.Join(showExecutionPlanToolStripMenuItem, showExecutionPlanStripButton);
			txtGroup.Join(showClientStatisticsToolStripMenuItem, showClientStatisticsToolStripButton);
			conGroup.Join(resultsinTextToolStripMenuItem);
			conGroup.Join(resultsinGridToolStripMenuItem);
			conGroup.Join(resultsinXMLToolStripMenuItem);

			// SQL Group
			sqlGroup.Join(parseToolStripMenuItem, parseToolStripButton);
			sqlGroup.Join(executeToolStripMenuItem, executeToolStripButton);
			sqlGroup.Join(executeasWrappedProcedureToolStripMenuItem);
			sqlGroup.Join(executeasAnonymousBlockToolStripMenuItem);
			sqlGroup.Join(executeSqlPlusMenuItem);
			sqlGroup.Join(executeRepeatedToolStripMenuItem);
			sqlGroup.Join(cancelExecutingQueryToolStripMenuItem, cancelExecutingQueryToolStripButton);
			sqlGroup.Join(showExecutionPlanToolStripMenuItem, showExecutionPlanStripButton);
			sqlGroup.Join(showClientStatisticsToolStripMenuItem, showClientStatisticsToolStripButton);
			sqlGroup.Join(resultsinTextToolStripMenuItem);
			sqlGroup.Join(resultsinGridToolStripMenuItem);
			sqlGroup.Join(resultsinXMLToolStripMenuItem);
			sqlGroup.Join(showresultspaneToolStripMenuItem, showResultsPaneToolStripButton);

			// Tools
			conGroup.Join(purgeRecycleBinToolStripMenuItem);
			conGroup.Join(showOpenCursorsToolStripMenuItem);
			conGroup.Join(showOpenTransactionsToolStripMenuItem);
			conGroup.Join(showUserErrorsToolStripMenuItem);

			// Window
			conGroup.Join(showresultspaneToolStripMenuItem, showResultsPaneToolStripButton);
			conGroup.Join(cascadeToolStripMenuItem);
			conGroup.Join(tileHorizontalToolStripMenuItem);
			conGroup.Join(tileVerticalToolStripMenuItem);
			checkers.AddChecker("ToggleResultsPane", showresultspaneToolStripMenuItem);
			checkers.AddChecker("ToggleResultsPane", showResultsPaneToolStripButton);
		}


		// ICommander implementation

		public CommandGroup AdvancedControls { get { return advGroup; } }
		public CommandGroup ConnectControls { get { return conGroup; } }
		public CommandGroup ExecuteControls { get { return exeGroup; } }
		public CommandGroup PasteControls { get { return pasGroup; } }
		public CommandGroup RedoControls { get { return redGroup; } }
		public CommandGroup SaveControls { get { return savGroup; } }
		public CommandGroup SelectControls { get { return selGroup; } }
		public CommandGroup SqlControls { get { return sqlGroup; } }
		public CommandGroup TextControls { get { return txtGroup; } }
		public CommandGroup UndoControls { get { return undGroup; } }


		public void SetChecker (string name, bool isChecked)
		{
			checkers.SetChecker(name, isChecked);
		}


		public void SetDefaults ()
		{
			commandWorker = (IWorker)(QueryWindow)ActiveMdiChild;
			if (commandWorker != null)
			{
				commandWorker.UpdateCommander();
			}
			else
			{
				advGroup.IsEnabled = false;
				conGroup.IsEnabled = false;
				exeGroup.IsEnabled = false;
				pasGroup.IsEnabled = false;
				redGroup.IsEnabled = false;
				savGroup.IsEnabled = false;
				selGroup.IsEnabled = false;
				sqlGroup.IsEnabled = false;
				txtGroup.IsEnabled = false;
				undGroup.IsEnabled = false;
			}
		}


		public void SetResultTarget (ResultTarget target)
		{
			resultsinTextToolStripMenuItem.Checked = (target == ResultTarget.Text);
			resultsinGridToolStripMenuItem.Checked = (target == ResultTarget.Grid);
			resultsinXMLToolStripMenuItem.Checked = (target == ResultTarget.Xml);

			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.ResultTarget = target;
		}


		public void SetWorker (IWorker worker)
		{
			commandWorker = worker;
		}


		//========================================================================================
		// RestorePosition()
		//========================================================================================

		private void RestorePosition ()
		{
			bool undefined = true;

			if (UserOptions.GetBoolean("general/savePosition"))
			{
				Rectangle location = UserOptions.LoadScreenPosition();
				if (!location.Equals(Rectangle.Empty))
				{
					this.Height = location.Height;
					this.Width = location.Width;
					this.Top = location.Top;
					this.Left = location.Left;

					undefined = false;
				}
			}

			Rectangle area = Screen.PrimaryScreen.WorkingArea;

			if (undefined)
			{
				if (area.Width < 2000)
				{
					// set full screen minus first icon column width for smaller screens...

					int icons = SystemInformation.IconSize.Width
						+ SystemInformation.IconSpacingSize.Width;

					this.Height = area.Height;			// (int)(area.Height * 0.9);
					this.Width = area.Width - icons;	// (int)(area.Width * 0.8);
					this.Top = 0;						// (area.Height - this.Height) / 2;
					this.Left = icons;					// (area.Width - this.Width) / 2;
				}
				else
				{
					// set slightly smaller for very large screens...

					this.Height = (int)(area.Height * 0.9);
					this.Width = (int)(area.Width * 0.8);
					this.Top = (area.Height - this.Height) / 2;
					this.Left = (area.Width - this.Width) / 2;
				}
			}
			else
			{
				if ((this.Top < 0) || (this.Top > area.Height))
				{
					this.Top = 0;
				}

				if ((this.Left < 0) || (this.Left > area.Width))
				{
					this.Left = 0;
				}
			}

			int width = (int)Math.Round((decimal)(this.Width) * (decimal)0.25);
			if (browser.Width < 300)
			{
				browser.Width = width;
			}
		}



		private void DoWindowResizeEnd(object sender, EventArgs e)
		{
			CalculateOptimalChildSizes();
		}


		//========================================================================================
		// CalculateOptimalChildSizes()
		//		Determine the preferred optimal size for the query window and the
		//		default height of the browser properties window based on the restored
		//		size of the main window.
		//
		//		The optimal query window size is immediately applied to the initial
		//		query window and persisted for later use on new query windows.
		//========================================================================================

		private void CalculateOptimalChildSizes ()
		{
			childSize.Height = (int)(this.ClientSize.Height * 0.80);
			childSize.Width = (int)(this.ClientSize.Width * 0.70);

			this.LayoutMdi(MdiLayout.Cascade);

			for (int i=0; i < this.MdiChildren.Length; i++)
			{
				this.MdiChildren[i].Size = childSize;
			}

			properties.Size = new Size(properties.Size.Width, (int)(this.ClientSize.Height / 6));
		}


		//========================================================================================
		// OnClosing()
		//========================================================================================

		protected override void OnClosing (CancelEventArgs e)
		{
			if (UserOptions.GetBoolean("general/savePosition"))
				UserOptions.SaveScreenPosition(new Rectangle(Left, Top, Width, Height));

			UserOptions.Save();
		}

		#endregion Initialization


		//========================================================================================
		// File menu items
		//========================================================================================

		#region FileMenu

		private void DoFileConnect (object sender, EventArgs e)
		{
			Dialogs.ConnectDialog dialog = new Dialogs.ConnectDialog();
			DialogResult result = dialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				browser.AddConnection(dialog.Connection);
				OpenQueryWindow(dialog.Connection);
			}
		}


		private void DoFileDisconnect (object sender, EventArgs e)
		{
			ActiveMdiChild.Close();
		}


		private void DoFileDisconnectAll (object sender, EventArgs e)
		{
			if (CloseAll())
			{
				ConnectControls.IsEnabled = false;
			}
		}

		
		#region CloseAll
		private bool CloseAll ()
		{
			QueryWindow child;

			// inventory of unsaved windows
			int i = 0;
			bool saved = true;
			while ((i < MdiChildren.Length) && saved)
			{
				child = (QueryWindow)MdiChildren[i];
				if (saved = child.IsSaved)
					i++;
			}

			if (!saved)
			{
				// prompt to save unsaved
				SaveDialog dialog = new SaveDialog();
				dialog.AddUnsavedItems(MdiChildren);

				DialogResult result = dialog.ShowDialog(this);
				if (result == DialogResult.Cancel)
				{
					return false;
				}

				if (result == DialogResult.Yes)
				{
					if (!CloseEach(dialog.UnsavedItems))
					{
						return false;
					}
				}
			}

			// just close the remaining windows
			while (MdiChildren.Length > 0)
			{
				child = (QueryWindow)MdiChildren[0];
				child.IsSaved = true;
				child.Close();
			}

			return true;
		}


		private bool CloseEach (QueryWindow[] items)
		{
			SaveFileDialog dialog = null;
			DialogResult result;
			bool success = true;

			foreach (QueryWindow child in items)
			{
				if (!child.IsSaved)
				{
					if (child.Filename == null)
					{
						if (dialog == null)
						{
							dialog = new SaveFileDialog();
							dialog.DefaultExt = UserOptions.GetString("general/queryExtension");
							dialog.Filter = translator.GetString("SaveQueryDialogFilter");
							dialog.InitialDirectory = UserOptions.GetString("general/queryPath");
							dialog.Title = translator.GetString("SaveQueryDialogTitle");
						}

						result = dialog.ShowDialog(this);
						if (result == DialogResult.OK)
						{
							child.SaveFile(dialog.FileName);
						}
						else if (result == DialogResult.Cancel)
						{
							success = false;
							break;
						}
					}
					else
					{
						child.SaveFile(child.Filename);
					}
				}
			}

			if (dialog != null)
			{
				dialog.Dispose();
				dialog = null;
			}

			return success;
		}

		#endregion CloseAll


		string openPath = null;
		private void DoFileOpen (object sender, EventArgs e)
		{
			string extention = UserOptions.GetString("general/queryExtension");

			string path;
			if (openPath == null)
				path = UserOptions.GetString("general/queryPath");
			else
				path = openPath;

			string curdir = Directory.GetCurrentDirectory();

			using (var dialog = new OpenFileDialog())
			{
				dialog.DefaultExt = extention;
				string sqlprojext = translator.GetString("SqlProjectExtension");

				dialog.Filter = String.Format(
					translator.GetString("OpenFileDialogFilter"), extention, sqlprojext.Substring(1));

				dialog.InitialDirectory = path;

				DialogResult result = dialog.ShowDialog(this);
				if (result == DialogResult.OK)
				{
					openPath = Path.GetDirectoryName(dialog.FileName);
					string extension = Path.GetExtension(dialog.FileName);

					if (extension == sqlprojext)
					{
						browser.OpenProjectFile(dialog.FileName);
					}
					else
					{
						QueryWindow child = (QueryWindow)ActiveMdiChild;
						if ((child == null) || !child.IsSaved)
						{
							DoFileNew(sender, e);

							// aparently the user cancelled the Connect dialog!
							if (ActiveMdiChild == child)
								return;

							// else point to new active child
							child = (QueryWindow)ActiveMdiChild;
						}

						child.LoadFile(dialog.FileName);
					}

					mru.Add(dialog.FileName);
				}
			}

			Directory.SetCurrentDirectory(curdir);
		}


		private void DoFileNew (object sender, EventArgs e)
		{
			Browser.SchemataSchema schema = browser.Schema;

			if (schema == null)
			{
				DoFileConnect(sender, e);
			}
			else
			{
				OpenQueryWindow(schema.Database);
			}

			//if (MdiChildren.Length == 0)
			//{
			//    DoFileConnect(sender, e);
			//}
			//else
			//{
			//    OpenQueryWindow(((QueryWindow)ActiveMdiChild).DatabaseConnection);
			//}
		}


		private void OpenQueryWindow (Database.DatabaseConnection con)
		{
			newchild = new QueryWindow(this, con, browser);
			newchild.Size = childSize;
			newchild.SetTitle();
			newchild.FormClosing += new FormClosingEventHandler(DoClosingQueryWindow);
			newchild.OpeningOptions += new OpeningOptionsEventHandler(DoOpeningOptions);
			newchild.ResultTargetChanged += new ResultTargetChangedEventHandler(SetResultTarget);
			newchild.Saving += new EventHandler(DoFileSave);
			newchild.MdiParent = this;
			newchild.Show();

			AddToWindowsMenu(newchild);

			Statusbar.Connections = MdiChildren.Length;
		}


		private void LoadRecentFile (string filename)
		{
			string sqlprojext = translator.GetString("SqlProjectExtension");
			if (Path.GetExtension(filename).Equals(sqlprojext))
			{
				browser.OpenProjectFile(filename);
			}
			else
			{
				QueryWindow child = (QueryWindow)ActiveMdiChild;
				if ((child == null) || !child.IsSaved)
				{
					DoFileNew(null, null);
					child = (QueryWindow)ActiveMdiChild;
				}

				child.LoadFile(filename);
			}
		}
		

		private void DoClosingQueryWindow (object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.MdiFormClosing)
			{
				// closing an MDI form is a multi-state process: MdiFormClosing occurs first,
				// followed by UserClosing.  So defer this method until UserClosing...
				return;
			}

			QueryWindow child = (QueryWindow)sender;
			if (!child.IsSaved)
			{
				SaveDialog dialog = new SaveDialog();
				dialog.AddItem(child);

				DialogResult result = dialog.ShowDialog(this);

				if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}

				if (result == DialogResult.Yes)
				{
					if (child.Filename == null)
					{
						child.Activate();
						DoFileSaveAs(child, e);
					}
					else
					{
						child.SaveFile(child.Filename);
					}
				}
			}

			Statusbar.Connections = MdiChildren.Length - 1;
			RemoveFromWindowsMenu(child);
		}


		private void DoFileSave (object sender, EventArgs e)
		{
			//QueryWindow child = (QueryWindow)ActiveMdiChild;
			//if (child.Filename == null)
			//{
			//    DoFileSaveAs(sender, e);
			//}
			//else
			//{
			//    child.SaveFile(child.Filename);
			//}

			if (commandWorker.Filename == null)
			{
				DoFileSaveAs(sender, e);
			}
			else
			{
				commandWorker.SaveFile(commandWorker.Filename);
			}
		}


		private void DoFileSaveAs (object sender, EventArgs e)
		{
			using (var dialog = new SaveFileDialog())
			{
				DialogResult result;

				QueryWindow child = (QueryWindow)ActiveMdiChild;
				IEditor pane = child.ActivePane;

				if (pane is EditorView)
				{
					dialog.DefaultExt = UserOptions.GetString("general/queryExtension");
					dialog.Filter = translator.GetString("SaveQueryDialogFilter");
					dialog.InitialDirectory = UserOptions.GetString("general/queryPath");
					dialog.Title = translator.GetString("SaveQueryDialogTitle");

					result = dialog.ShowDialog(this);

					if (result == DialogResult.OK)
					{
						child.SaveFile(dialog.FileName);
					}
				}
				else if (pane != null) // results
				{
					dialog.DefaultExt = UserOptions.GetString("general/resultExtension");
					dialog.Filter = translator.GetString("SaveReportDialogFilter");
					dialog.InitialDirectory = UserOptions.GetString("general/resultPath");
					dialog.Title = translator.GetString("SaveReportDialogTitle");

					result = dialog.ShowDialog(this);

					if (result == DialogResult.OK)
					{
						child.SaveFile(dialog.FileName);
					}
				}
			}
		}


		private void DoFileSaveAll (object sender, EventArgs e)
		{
			QueryWindow child;
			SaveFileDialog dialog = null;
			DialogResult result;

			foreach (Form form in MdiChildren)
			{
				child = (QueryWindow)form;
				if (!child.IsSaved)
				{
					if (child.Filename == null)
					{
						if (dialog == null)
						{
							dialog = new SaveFileDialog();
							dialog.DefaultExt = UserOptions.GetString("general/queryExtension");
							dialog.Filter = translator.GetString("SaveQueryDialogFilter");
							dialog.InitialDirectory = UserOptions.GetString("general/queryPath");
							dialog.Title = translator.GetString("SaveQueryDialogTitle");
						}

						result = dialog.ShowDialog(this);
						if (result == DialogResult.OK)
						{
							child.SaveFile(dialog.FileName);
						}
					}
					else
					{
						child.SaveFile(child.Filename);
					}
				}
			}

			if (dialog != null)
			{
				dialog.Dispose();
				dialog = null;
			}
		}


		private void DoFilePrint (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			IEditor pane = child.ActivePane;
			string[] lines = pane.TextLines;

			Query.Editor printer = CreatePrinter();

			foreach (string line in lines)
			{
				printer.Lines.Add(line);
			}

			printer.Printing.Print();
		}


		private void DoFilePrintPreview (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			IEditor pane = child.ActivePane;
			string[] lines = pane.TextLines;

			Query.Editor printer = CreatePrinter();

			foreach (string line in lines)
			{
				printer.Lines.Add(line);
			}

			printer.Printing.ExecutePrintPreviewDialog();
		}


		private void DoFilePageSetup (object sender, EventArgs e)
		{
			using (Query.Editor printer = CreatePrinter())
			{
				printer.Printing.ExecutePageSetupDialog();
			}
		}



		private Query.Editor CreatePrinter ()
		{
			Query.Editor printer = new Query.Editor();

			printer.Printing.PrintDocument.DefaultPageSettings.Margins.Bottom = 50;
			printer.Printing.PrintDocument.DefaultPageSettings.Margins.Left = 50;
			printer.Printing.PrintDocument.DefaultPageSettings.Margins.Right = 50;
			printer.Printing.PrintDocument.DefaultPageSettings.Margins.Top = 50;

			return printer;
		}


		private void DoFileExit (object sender, EventArgs e)
		{
			if (CloseAll())
			{
				Application.Exit();
			}
		}


		private void DoFormClosing (object sender, FormClosingEventArgs e)
		{
			if (!CloseAll())
			{
				e.Cancel = true;
			}
		}

		#endregion FileMenu


		//========================================================================================
		// Edit menu items
		//========================================================================================

		#region EditMenu

		private void DoEditUndo (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoUndo(sender, e);
		}


		private void DoEditRedo (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoRedo(sender, e);
		}


		private void DoEditCut (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoCut(sender, e);
		}


		private void DoEditCopy (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoCopy(sender, e);
		}


		private void DoEditPaste (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoPaste(sender, e);
		}


		private void DoEditSelectAll (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoSelectAll(sender, e);
		}


		private void DoEditClearWindow (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoClearWindow(sender, e);
		}


		private void DoEditFind (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoFind(sender, e);
		}


		private void DoEditGotoLine (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoGotoLine(sender, e);
		}


		private void DoEditMakeLowercase (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoMakeLowercase(sender, e);
		}


		private void DoEditMakeUppercase (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoMakeUppercase(sender, e);
		}


		private void DoEditToggleWhitespace (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoToggleWhitespace(sender, e);
		}

		#endregion EditMenu


		//========================================================================================
		// View menu items
		//========================================================================================

		#region ViewMenu

		private void DoViewObjectBrowser (object sender, EventArgs e)
		{
			UI.Docking.Content brows = dockmgr.Contents[browserWindowTitle];
			if (brows.Visible)
			{
				dockmgr.HideContent(brows);
				this.Focus();
			}
			else
			{
				dockmgr.ShowContent(brows);
				dockmgr.BringAutoHideIntoView(brows);
			}
		}


		// event handler captured from Browser context menu.
		private void DoShowingProperties (Browser.SchemataNode node)
		{
			DoViewProperties(node, null);
		}


		// event handler captured from MainWindow menu.
		private void DoViewProperties (object sender, EventArgs e)
		{
			UI.Docking.Content props = dockmgr.Contents[propsWindowTitle];
			if (props.Visible)
			{
				dockmgr.HideContent(props);
				this.Focus();
			}
			else
			{
				dockmgr.ShowContent(props);
				dockmgr.BringAutoHideIntoView(props);
			}
		}


		private void DoViewDebugger (object sender, EventArgs e)
		{
			UI.Docking.Content debug = dockmgr.Contents[traceWindowTitle];
			if (debug.Visible)
			{
				dockmgr.HideContent(debug);
				Logger.IsEnabled = false;
				this.Focus();
			}
			else
			{
				dockmgr.ShowContent(debug);
				dockmgr.BringAutoHideIntoView(debug);
				Logger.IsEnabled = true;
			}
		}

		#endregion ViewMenu


		//========================================================================================
		// Query menu items
		//========================================================================================

		#region QueryMenu

		private void DoChangeSchema (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;

			ChangeDbDialog dialog
				= new ChangeDbDialog(browser.GetDatabaseSchemas(child.DatabaseConnection));

			DialogResult result = dialog.ShowDialog(child);

			if (result == DialogResult.OK)
			{
				browser.Schema = dialog.Schema;
				child.SetSchema(dialog.SchemaName);
				//child.DatabaseConnection.DefaultSchema = dialog.SchemaName;
				//child.SetTitle();
			}
		}


		private void DoQueryParse (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.Parse();
		}


		private void DoQueryExecute (object sender, EventArgs e)
		{
			if (!deferExecution)
			{
				QueryWindow child = (QueryWindow)ActiveMdiChild;
				if (child != null)
					child.Execute(ParseMode.None);
			}
		}


		private void DoQueryExecuteSequential (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.Execute(ParseMode.Sequential);
		}


		private void DoQuerySelectExecuteParseMode (object sender, EventArgs e)
		{
			deferExecution = true;
		}


		private void DoQuerySelectExecuteParseModeDone (object sender, EventArgs e)
		{
			deferExecution = false;
		}


		private void DoQueryExecuteWrapped (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.Execute(ParseMode.Wrapped);
		}


		private void DoQueryExecuteBlock (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.Execute(ParseMode.Block);
		}


		private void DoQueryExecuteSqlPlus (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.Execute(ParseMode.SqlPlus);
		}

	
		private void DoQueryExecuteRepeated (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.Execute(ParseMode.Repeat);
		}


		private void DoQueryCancel (object sender, EventArgs e)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
				child.CancelQuery();
		}


		private void DoChangeResultTarget (object sender, EventArgs e)
		{
			if (sender == resultsinTextToolStripMenuItem)
			{
				SetResultTarget(ResultTarget.Text);
			}
			else if (sender == resultsinGridToolStripMenuItem)
			{
				SetResultTarget(ResultTarget.Grid);
			}
			else
			{
				SetResultTarget(ResultTarget.Xml);
			}
		}


		private void SetResultTarget (object sender, EventArgs e)
		{
			// TODO: why?
			if (ActiveMdiChild == null)
				return;

			ResultTarget target = ((QueryWindow)ActiveMdiChild).ResultTarget;
			resultsinTextToolStripMenuItem.Checked = (target == ResultTarget.Text);
			resultsinGridToolStripMenuItem.Checked = (target == ResultTarget.Grid);
			resultsinXMLToolStripMenuItem.Checked = (target == ResultTarget.Xml);
		}


		private void DoQueryExplainPlan (object sender, EventArgs e)
		{
			UserOptions.RunExplainPlan = !UserOptions.RunExplainPlan;
			showExecutionPlanToolStripMenuItem.Checked = UserOptions.RunExplainPlan;
			showExecutionPlanStripButton.Checked = UserOptions.RunExplainPlan;
		}


		private void DoQueryShowStatistics (object sender, EventArgs e)
		{
			UserOptions.RunStatistics = !UserOptions.RunStatistics;
			showClientStatisticsToolStripMenuItem.Checked = UserOptions.RunStatistics;
			showClientStatisticsToolStripButton.Checked = UserOptions.RunStatistics;
		}

		#endregion QueryMenu


		//========================================================================================
		// Tools menu items
		//========================================================================================

		#region ToolsMenu

		private void DoToolsPurgeRecycleBin (object sender, EventArgs e)
		{
			DoFileNew(this, e);
			((QueryWindow)ActiveMdiChild).PurgeRecycleBin();
		}


		private void DoToolOpenSQLTraceReport (object sender, EventArgs e)
		{
			DoFileNew(this, e);
			((QueryWindow)ActiveMdiChild).OpenSQLTraceReport();
		}

	
		private void DoToolsServiceController (object sender, EventArgs e)
		{
			Dialogs.ServiceControllerDialog dialog = new Dialogs.ServiceControllerDialog();
			dialog.ShowDialog(this);
		}


		private void DoToolsShowOpenCursors (object sender, EventArgs e)
		{
			DoFileNew(this, e);
			((QueryWindow)ActiveMdiChild).ShowOpenCursors();
		}

		private void DoToolsShowOpenTransactions (object sender, EventArgs e)
		{
			DoFileNew(this, e);
			((QueryWindow)ActiveMdiChild).ShowOpenTransactions();
		}


		private void DoToolsShowUserErrors (object sender, EventArgs e)
		{
			DoFileNew(this, e);
			((QueryWindow)ActiveMdiChild).ShowUserErrors();
		}


		private void DoToggleSQLTracing (object sender, EventArgs e)
		{
			traceToolStripButton.Checked = !traceToolStripButton.Checked;
			((QueryWindow)ActiveMdiChild).ToggleTracing(traceToolStripButton.Checked);
		}
		
		
		private void DoToolsOptions (object sender, EventArgs e)
		{
			OpenOptionsDialog(null);
		}


		private void DoOpeningOptions (object sender, OpeningOptionsEventArgs e)
		{
			OpenOptionsDialog(e.SheetPath);
		}


		private void OpenOptionsDialog (string sheetName)
		{
			Options.OptionsDialog dialog = new Options.OptionsDialog();

			if (sheetName != null)
				dialog.ActiveSheet = sheetName;

			DialogResult result = dialog.ShowDialog(this);

			if (result == DialogResult.OK)
			{
				browser.SetOptions();

				foreach (Form child in MdiChildren)
				{
					((QueryWindow)child).SetOptions();

				}
			}
		}

		#endregion ToolsMenu


		//========================================================================================
		// Windows menu items
		//========================================================================================

		#region WindowsMenu

		private void DoWindowToggleResultsPane (object sender, EventArgs e)
		{
			((QueryWindow)ActiveMdiChild).DoToggleResults(sender, e);
		}


		private void DoWindowCascade (object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}


		private void DoWindowTileHorizontal (object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}


		private void DoWindowTileVertical (object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}


		private void AddToWindowsMenu (QueryWindow child)
		{
			if (MdiChildren.Length == 1)
			{
				windowToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
			}

			ToolStripMenuItem item = new ToolStripMenuItem(child.Text);
			item.Tag = child;
			item.Click += new EventHandler(DoWindowSelectWindow);
			windowToolStripMenuItem.DropDownItems.Add(item);
		}

		private void DoWindowSelectWindow (object sender, EventArgs e)
		{
			((QueryWindow)((ToolStripMenuItem)sender).Tag).Activate();
		}


		private void RemoveFromWindowsMenu (QueryWindow child)
		{
			int i = 0;
			bool found = false;

			while ((i < windowToolStripMenuItem.DropDownItems.Count) && !found)
			{
				if (! (found = (windowToolStripMenuItem.DropDownItems[i].Tag == child)))
					i++;
			}

			if (found)
			{
				windowToolStripMenuItem.DropDownItems.RemoveAt(i);

				if (MdiChildren.Length == 1)  // ...about to remove child
				{
					// remove separator
					windowToolStripMenuItem.DropDownItems.RemoveAt(
						windowToolStripMenuItem.DropDownItems.Count - 1);
				}
			}
		}

		#endregion WindowsMenu


		//========================================================================================
		// Help menu items
		//========================================================================================

		#region HelpMenu

		private void DoHelp (object sender, EventArgs e)
		{
			Help.ShowHelp(this, "Orqa.chm");
		}


		private void DoHelpAbout (object sender, EventArgs e)
		{
			Dialogs.AboutDialog about = new Dialogs.AboutDialog();
			about.Servers = browser.Servers;
			about.ShowDialog(this);
			about = null;
		}

		#endregion HelpMenu


		//========================================================================================
		// Drag/Drop support
		//========================================================================================

		private void DoDragDrop (object sender, DragEventArgs e)
		{
			string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

			foreach (string path in paths)
			{
				DoFileNew(sender, e);
				var child = (QueryWindow)ActiveMdiChild;
				child.LoadFile(path);
				mru.Add(path);
			}
		}


		private void DoDragEnter (object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

	
		//========================================================================================
		// Browser handlers
		//========================================================================================

		private void DoBrowserActionRequested (River.Orqa.Browser.BrowserTreeEventArgs e)
		{
			switch (e.Action)
			{
				case Browser.BrowserTreeAction.Compile:
					DoFileNew(this, e);
					((QueryWindow)ActiveMdiChild).CompileSchemata(e.Node);
					break;

				case Browser.BrowserTreeAction.Delete:
					break;

				case Browser.BrowserTreeAction.Edit:
					DoFileNew(this, e);
					((QueryWindow)ActiveMdiChild).EditSchemata(e.Node);
					break;

				case Browser.BrowserTreeAction.Open:
					if (e.Node is Browser.SchemataProcedure)
					{
						if (!e.Node.IsDiscovered)
							e.Node.Discover();

						if (ActiveMdiChild == null)
							DoFileNew(this, e);
					}
					else
					{
						DoFileNew(this, e);
					}
					((QueryWindow)ActiveMdiChild).OpenSchemata(e.Node);
					break;

				case Browser.BrowserTreeAction.Properties:
					DoShowingProperties(e.Node);
					break;

				case Browser.BrowserTreeAction.Select:
					properties.SelectedObject = e.Node;
					break;
			}
		}


		private void DoSchemaSelected (string schemaName)
		{
			QueryWindow child = (QueryWindow)ActiveMdiChild;
			if (child != null)
			{
				child.SetSchema(schemaName);
			}
		}


		private void DoScriptOpen ()
		{
			string path;
			if (openPath == null)
				path = UserOptions.GetString("general/queryPath");
			else
				path = openPath;

			string curdir = Directory.GetCurrentDirectory();

			using (var dialog = new OpenFileDialog())
			{
				string sqlext = translator.GetString("SqlProjectExtension");
				dialog.DefaultExt = sqlext.Substring(1);

				dialog.Filter = String.Format(
					translator.GetString("OpenScriptDialogFilter"), sqlext.Substring(1));

				dialog.InitialDirectory = path;

				DialogResult result = dialog.ShowDialog(this);
				if (result == DialogResult.OK)
				{
					openPath = Path.GetDirectoryName(dialog.FileName);
					string extension = Path.GetExtension(dialog.FileName);

					if (extension == sqlext)
					{
						browser.OpenProjectFile(dialog.FileName);
					}
					else
					{
						QueryWindow child = (QueryWindow)ActiveMdiChild;
						if ((child == null) || !child.IsSaved)
						{
							DoFileNew(this, new EventArgs());

							// aparently the user cancelled the Connect dialog!
							if (ActiveMdiChild == child)
								return;

							// else point to new active child
							child = (QueryWindow)ActiveMdiChild;
						}

						child.LoadFile(dialog.FileName);
					}

					mru.Add(dialog.FileName);
				}
			}

			Directory.SetCurrentDirectory(curdir);
		}


		private void DoScriptSelected (string path)
		{
			DoFileNew(null, null);
			newchild.LoadFile(path);
		}


		private void DoTemplateSelected (string content, bool inNewWindow, bool runReport)
		{
			if (inNewWindow)
				DoFileNew(null, null);
			else
				newchild = (QueryWindow)ActiveMdiChild;

			if (inNewWindow && runReport)
			{
				string ext = Path.GetExtension(content).Substring(1);
				string optext = UserOptions.GetString("general/queryExtension");
				if (ext.Equals(optext, StringComparison.InvariantCultureIgnoreCase))
				{
					// .sql files load the sql and run as normal with results
					newchild.LoadFile(content);
					newchild.Execute(ParseMode.None);
				}
				else
				{
					optext = UserOptions.GetString("general/reportExtension");
					if (ext.Equals(optext, StringComparison.InvariantCultureIgnoreCase))
					{
						// .rpt files run as reports in primary/edit pane of query window
						newchild.ExecuteReport(content);
					}
					else
					{
						// else .tpl file is not executable
						newchild.LoadFile(content);
					}
				}
			}
			else
			{
				newchild.LoadFile(content);
			}
		}
	}
}