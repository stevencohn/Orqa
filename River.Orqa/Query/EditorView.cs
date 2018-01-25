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
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using System.Xml.XPath;
	using River.Orqa.Database;
	using River.Orqa.Editor;
	using River.Orqa.Editor.Dialogs;
	using River.Orqa.Options;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class Editor
	//********************************************************************************************

	internal partial class EditorView : UserControl, IEditor
	{
		private Translator translator;			// get lex style from resource assembly
		private Editor editor;					// the editor
		private TextSource textSource;			// text source for editor
		private ResultTarget resultTarget;		// preferred result output target


		// Events

		public event CaretChangedEventHandler CaretChanged;
		public event InsertModeChangedEventHandler InsertModeChanged;
		public event EditorEventHandler Edited;
		public event EventHandler ExecuteQuery;
		public event OpeningOptionsEventHandler OpeningOptions;
		public event EventHandler ParseQuery;
		public event ResultTargetChangedEventHandler ResultTargetChanged;
		public event EventHandler SelectionChanged;
		public event EventHandler ToggleResults;


		//========================================================================================
		// Constructor
		//========================================================================================

		public EditorView ()
		{
			InitializeComponent();

			textSource = new TextSource(this.components);

			editor = new Editor(this.components);
			editor.ContextMenuStrip = contextMenu;
			editor.Dock = DockStyle.Fill;
			editor.Location = new Point(0, 0);
			editor.Name = "SyntaxEdit";
			editor.Source = textSource;

			editor.CaretChanged += new CaretChangedEventHandler(DoCaretChanged);
			editor.InsertModeChanged += new InsertModeChangedEventHandler(DoInsertModeChanged);
			editor.Edited += new EditorEventHandler(DoEdited);
			editor.GotFocus += new EventHandler(DoGotFocus);
			editor.SelectionChanged += new EventHandler(DoSelectionChanged);
			editor.ToggleResults += new EventHandler(DoToggleResults);

			// this test is a fix for this.DesignMode not being set yet
			if (System.ComponentModel.LicenseManager.UsageMode
				!= System.ComponentModel.LicenseUsageMode.Designtime)
			{
				editor.IsColorized = true;
				this.Controls.Add(editor);

				translator = new Translator("Query");
			}
		}


		//========================================================================================
		// Properties
		//========================================================================================

		#region Properties

		public bool CanPaste
		{
			get { return editor.CanPaste; }
		}


		public bool CanRedo
		{
			get { return editor.Source.CanRedo(); }
		}


		public bool CanUndo
		{
			get { return editor.Source.CanUndo(); }
		}


		public int Column
		{
			get { return editor.Column; }
		}


		/// <summary>
		/// Gets a boolean value indicating whether the editor contains executable
		/// content; content is executable if it is SQL script.  Other content, such
		/// as batch files and text files are not executable in Orqa.
		/// </summary>

		public bool IsExecutable
		{
			get { return editor.IsColorized; }
			set { editor.IsColorized = value; }
		}


		public bool IsFocused
		{
			get { return editor.Focused; }
		}


		public bool IsSaved
		{
			get { return editor.IsSaved; }
			set { editor.IsSaved = value; }
		}


		public bool HasContent
		{
			get { return editor.HasContent; }
		}


		public bool HasSelection
		{
			get { return editor.HasSelection; }
		}


		public int Line
		{
			get { return editor.Line; }
		}


		public ResultTarget ResultTarget
		{
			set
			{
				resultsinTextToolStripMenuItem.Checked = (value == ResultTarget.Text);
				resultsinGridToolStripMenuItem.Checked = (value == ResultTarget.Grid);
				resultsinXMLToolStripMenuItem.Checked = (value == ResultTarget.Xml);
				resultTarget = value;
			}
		}


		public string SelectedText
		{
			get
			{
				string text = (editor.SelectionLength == 0 ? editor.Text : editor.SelectedText);

				// SyntaxEdit separates lines with \r\n, so strip out the unecessary "\r"s
				return text.Trim().Replace("\r", String.Empty);
			}
		}


		public int TabStops
		{
			get { return editor.Source.Lines.TabStops[0]; }
			set { editor.Source.Lines.TabStops = new int[1] { value }; }
		}


		public string[] TextLines
		{
			get { return editor.Lines.ToStringArray(); }
		}


		public bool WhitespaceVisible
		{
			get { return editor.WhiteSpace.Visible; }
		}

		#endregion Properties


		//========================================================================================
		// Context menu handlers
		//========================================================================================

		#region Context menu handlers

		private void DoPrepareContextMenu (object sender, CancelEventArgs e)
		{
			cutToolStripMenuItem.Enabled = HasSelection;
			copyToolStripMenuItem.Enabled = HasSelection;
			pasteToolStripMenuItem.Enabled = CanPaste;
			selectAllToolStripMenuItem.Enabled = HasContent;
			clearToolStripMenuItem.Enabled = HasContent;
			parseToolStripMenuItem.Enabled = HasContent && IsExecutable;
			executeToolStripMenuItem.Enabled = HasContent && IsExecutable;

			resultModeToolStripMenuItem.Enabled = IsExecutable;
			toggleResultsPaneToolStripMenuItem.Enabled = IsExecutable;
		}


		private void DoOptions (object sender, EventArgs e)
		{
			if (OpeningOptions != null)
				OpeningOptions(sender, new OpeningOptionsEventArgs("/OrqaOptions/editor/general"));
		}

		#endregion Context menu handlers


		//========================================================================================
		// DoCaretChanged()
		//		Inform the query window that the caret position has changed.
		//========================================================================================

		private void DoCaretChanged (object sender, CaretChangedEventArgs e)
		{
			if (CaretChanged != null)
				CaretChanged(sender, e);
		}


		//========================================================================================
		// DoEdited()
		//========================================================================================

		void DoEdited (object sender, EditorEventArgs e)
		{
			if (Edited != null)
				Edited(sender, e);
		}


		//========================================================================================
		// DoGotFocus()
		//========================================================================================

		private void DoGotFocus (object sender, EventArgs e)
		{
			this.OnGotFocus(e);
			this.OnEnter(e);
		}


		//========================================================================================
		// DoInsertModeChanged()
		//========================================================================================

		private void DoInsertModeChanged (object sender, InsertModeChangedEventArgs e)
		{
			if (InsertModeChanged != null)
				InsertModeChanged(sender, e);
		}


		//========================================================================================
		// DoSelectionChanged()
		//========================================================================================

		private void DoSelectionChanged (object sender, EventArgs e)
		{
			if (SelectionChanged != null)
				SelectionChanged(this, e);
		}


		//========================================================================================
		// DoToggleResults()
		//========================================================================================

		private void DoToggleResults (object sender, EventArgs e)
		{
			if (ToggleResults != null)
				ToggleResults(sender, e);
		}


		//========================================================================================
		// Editing functions
		//========================================================================================

		public void DoUndo (object sender, EventArgs e)
		{
			if (editor.Source.CanUndo())
				editor.Source.Undo();
		}


		public void DoRedo (object sender, EventArgs e)
		{
			if (editor.Source.CanRedo())
				editor.Source.Redo();
		}


		public void DoCut (object sender, EventArgs e)
		{
			editor.Cut();
		}


		public void DoCopy (object sender, EventArgs e)
		{
			editor.Copy();
		}


		private void DoExecute (object sender, EventArgs e)
		{
			if (ExecuteQuery != null)
				ExecuteQuery(sender, e);
		}


		private void DoParse (object sender, EventArgs e)
		{
			if (ParseQuery != null)
				ParseQuery(sender, e);
		}


		public void DoPaste (object sender, EventArgs e)
		{
			editor.Paste();
		}


		public void DoSelectAll (object sender, EventArgs e)
		{
			editor.SelectAll();
		}


		private void DoChangeResultTarget (object sender, EventArgs e)
		{
			if (ResultTargetChanged == null)
				return;

			if (sender == resultsinTextToolStripMenuItem)
			{
				ResultTargetChanged(sender, new ResultTargetChangedEventArgs(ResultTarget.Text));
			}
			else if (sender == resultsinGridToolStripMenuItem)
			{
				ResultTargetChanged(sender, new ResultTargetChangedEventArgs(ResultTarget.Grid));
			}
			else
			{
				ResultTargetChanged(sender, new ResultTargetChangedEventArgs(ResultTarget.Xml));
			}

			editor.Focus();
		}


		public void DoClear (object sender, EventArgs e)
		{
			editor.Clear();
		}


		public void DoFind (object sender, EventArgs e)
		{
			editor.SearchDialog.Execute(editor, false, false);
		}


		public void DoGotoLine (object sender, EventArgs e)
		{
			int line = editor.Position.Y;
			DialogResult result = editor.GotoLineDialog.Execute(editor, editor.Lines.Count, ref line);
			if (result == DialogResult.OK)
				editor.MoveToLine(line);
		}


		public void DoMakeLowercase (object sender, EventArgs e)
		{
			bool noSelection = (editor.SelectionLength == 0);
			if (noSelection)
				editor.Selection.SelectCharRight();

			editor.Selection.SelectedText = editor.SelectedText.ToLower();

			if (noSelection)
				editor.Selection.SelectionLength = 0;
		}


		public void DoMakeUppercase (object sender, EventArgs e)
		{
			bool noSelection = (editor.SelectionLength == 0);
			if (noSelection)
				editor.Selection.SelectCharRight();

			editor.Selection.SelectedText = editor.SelectedText.ToUpper();

			if (noSelection)
				editor.Selection.SelectionLength = 0;
		}


		public void DoToggleWhitespace (object sender, EventArgs e)
		{
			editor.WhiteSpace.Visible = !editor.WhiteSpace.Visible;
		}


		public void InsertText (string text)
		{
			editor.InsertText(text);
		}


		public void LoadFile (string filename)
		{
			editor.Clear();
			editor.LoadFile(filename);
		}


		public void MoveHome ()
		{
			editor.MoveToLine(0);
			editor.MoveToChar(0);
		}


		public void SaveFile (string filename)
		{
			editor.SaveFile(filename, ExportFormat.Text);
			editor.IsSaved = true;
		}


		// IEditor

		public void RevertColor () { }
		public void Write (string text) { }
		public void WriteLine () { }
		public void WriteLine (string text) { }
		public void WriteNote (string text) { }


		//========================================================================================
		// SetOptions()
		//========================================================================================

		public void SetOptions ()
		{
			string fontName = UserOptions.GetString("editor/editorFonts/font/family");
			int fontSize = UserOptions.GetInt("editor/editorFonts/font/size");
			editor.Font = new Font(fontName, fontSize);

			bool beyondEof = UserOptions.GetBoolean("editor/general/beyondEof");
			bool beyondEoln = UserOptions.GetBoolean("editor/general/beyondEoln");
			NavigateOptions navigateOptions = NavigateOptions.DownAtLineEnd | NavigateOptions.UpAtLineBegin;
			if (beyondEof) navigateOptions |= NavigateOptions.BeyondEof;
			if (beyondEoln) navigateOptions |= NavigateOptions.BeyondEol;
			editor.NavigateOptions = navigateOptions;

			editor.IndentOptions = IndentOptions.AutoIndent;

			bool verticalScroll = UserOptions.GetBoolean("editor/general/verticalScroll");
			bool horizontalScroll = UserOptions.GetBoolean("editor/general/horizontalScroll");
			if (verticalScroll && horizontalScroll)
				editor.Scrolling.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
			else if (verticalScroll)
				editor.Scrolling.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
			else if (horizontalScroll)
				editor.Scrolling.ScrollBars = RichTextBoxScrollBars.ForcedHorizontal;
			else
				editor.Scrolling.ScrollBars = RichTextBoxScrollBars.None;

			bool showGutter = UserOptions.GetBoolean("editor/general/showGutter");
			int gutterWidth = UserOptions.GetInt("editor/general/gutterWidth");
			bool lineNumbers = UserOptions.GetBoolean("editor/general/lineNumbers");
			editor.Gutter.Visible = showGutter;
			editor.Gutter.Width = gutterWidth;
			editor.Gutter.Options = (lineNumbers ? GutterOptions.PaintLineNumbers | GutterOptions.PaintLinesOnGutter : GutterOptions.None);

			bool showMargin = UserOptions.GetBoolean("editor/general/showMargin");
			int marginPosition = UserOptions.GetInt("editor/general/marginPosition");
			bool wordWrap = UserOptions.GetBoolean("editor/general/wordWrap");
			bool wrapAtMargin = UserOptions.GetBoolean("editor/general/wrapAtMargin");
			editor.Margin.Visible = showMargin;
			editor.Margin.Position = marginPosition;
			editor.WordWrap = wordWrap;
			editor.WrapAtMargin = wrapAtMargin;

			SyntaxSettings settings = new SyntaxSettings();

			bool keepTabs = UserOptions.GetBoolean("editor/editorTabs/keepTabs");
			int tabSize = UserOptions.GetInt("editor/editorTabs/size");

			editor.Source.Lines.TabStops = new int[1] { tabSize };
			editor.Source.Lines.UseSpaces = !keepTabs;

			// set lex styles

			XPathNavigator nav = UserOptions.OptionsDoc.CreateNavigator();
			nav.MoveToFirstChild();
			nav = nav.SelectSingleNode("editor/editorFonts/lexStyles");

			nav.MoveToFirstChild();
			LexStyleItem item;
			string colorName;

			var foreground = FontsAndColors.PlainTextForeground;
			var background = FontsAndColors.PlainTextBackground;

			do
			{
				if (nav.NodeType == XPathNodeType.Element)
				{
					item = new LexStyleItem();
					item.Name = nav.LocalName;
					item.InternalName = nav.LocalName;

					colorName = nav.GetAttribute("foreColor", nav.NamespaceURI);
					item.ForeColor = Color.FromName(colorName);
					if (!item.ForeColor.IsKnownColor)
					{
						item.ForeColor = Color.FromArgb(unchecked((int)uint.Parse(
							colorName, System.Globalization.NumberStyles.HexNumber)));
					}

					if (item.Name.Equals("whitespace"))
					{
						foreground = item.ForeColor;
					}

					colorName = nav.GetAttribute("backColor", nav.NamespaceURI);
					item.BackColor = Color.FromName(colorName);
					if (!item.BackColor.IsKnownColor)
					{
						item.BackColor = Color.FromArgb(unchecked((int)uint.Parse(
							colorName, System.Globalization.NumberStyles.HexNumber)));
					}

					if (item.Name.Equals("whitespace"))
					{
						background = item.BackColor;
					}

					item.FontStyle
						= (FontStyle)Enum.Parse(
						typeof(FontStyle), nav.GetAttribute("style", nav.NamespaceURI));

					ApplyLeXStyle(item);
				}
			}
			while (nav.MoveToNext(XPathNodeType.Element));

			editor.ForeColor = foreground;
			editor.BackColor = background;
			editor.Refresh();
		}


		private void ApplyLeXStyle (LexStyleItem item)
		{
			River.Orqa.Editor.Syntax.ILexStyle[] styles = editor.Lexer.Scheme.LexStyles;
			bool found = false;
			int i = 0;

			while ((i < styles.Length) && !found)
			{
				if (!(found = styles[i].Name.Equals(item.Name)))
					i++;
			}

			if (found)
			{
				styles[i].BackColor = item.BackColor;
				styles[i].ForeColor = item.ForeColor;
				styles[i].FontStyle = item.FontStyle;
			}
		}
	}
}
