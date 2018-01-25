
namespace River.Orqa.Query
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Data;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using River.Orqa.Editor;
	using River.Orqa.Options;


	internal partial class Notepad : UserControl, IEditor, ISearch
	{
		#region DllImports

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern int LockWindowUpdate (IntPtr hWnd);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		private static extern int SendMessage (IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public extern static int SendMessage (IntPtr hWnd, int Msg, int wParam, ref int lParam);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public extern static int SendMessage (IntPtr hWnd, int Msg, int wParam, ref Point lParam);

		public enum Msg
		{
			WM_USER = 0x0400
		}

		public enum EditMsg
		{
			EM_LINEINDEX = 0xbb,
			EM_SETTABSTOPS = 0x00CB,
			EM_SETUNDOLIMIT = Msg.WM_USER + 82,
			EM_SETTEXTMODE = Msg.WM_USER + 89,
			EM_GETSCROLLPOS = Msg.WM_USER + 221,
			EM_SETSCROLLPOS = Msg.WM_USER + 222
		}

		#endregion DllImports

		private readonly string CR = "\r\n"; // System.Environment.NewLine;
		private Color foreColor;


		// Events

		public event OpeningOptionsEventHandler OpeningOptions;
		public event EventHandler Saving;
		public event EventHandler ToggleResults;


		//========================================================================================
		// Constructor
		//========================================================================================

		public Notepad ()
		{
			InitializeComponent();

			pad.SelectionBackColor = FontsAndColors.SelectedTextBackground;

			pad.DragEnter += new DragEventHandler(DoDragEnter);
			pad.DragDrop += new DragEventHandler(DoDragDrop);
			pad.GotFocus += new EventHandler(DoGotFocus);
			pad.TextChanged += new EventHandler(DoTextChanged);
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public new Color ForeColor
		{
			get
			{
				return pad.SelectionColor;
			}

			set
			{
				// save current color for RevertColor()
				foreColor = pad.SelectionColor;
				pad.SelectionColor = value;
			}
		}


		//========================================================================================
		// IEditor
		//========================================================================================

		#region IEditor

		public bool CanPaste
		{
			get
			{
				return (Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf)) ||
					   (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)) ||
					   (Clipboard.GetDataObject().GetDataPresent(DataFormats.UnicodeText));
			}
		}


		public bool CanRedo
		{
			get { return pad.CanRedo; }
		}


		public bool CanUndo
		{
			get { return pad.CanUndo; }
		}


		public int Column
		{
			get
			{
				return (pad.SelectionStart
					- SendMessage(pad.Handle, (int)EditMsg.EM_LINEINDEX, -1, 0)
					+ 1);
			}
		}


		public bool IsFocused
		{
			get { return pad.Focused; }
		}


		public bool IsSaved
		{
			get { return false; }
			set { }
		}


		public bool HasContent
		{
			get { return (pad.TextLength > 0); }
		}


		public bool HasSelection
		{
			get
			{
				return (pad.SelectionLength > 0);
			}
		}


		public int Line
		{
			get
			{
				return (pad.GetLineFromCharIndex(pad.SelectionStart) + 1);
			}
		}


		public void RevertColor ()
		{
			pad.ForeColor = foreColor;
		}


		public Point ScrollPosition
		{
			get
			{
				Point pos = new Point();
				SendMessage(pad.Handle, (int)EditMsg.EM_GETSCROLLPOS, 0, ref pos);
				return pos;
			}

			set
			{
				SendMessage(pad.Handle, (int)EditMsg.EM_SETSCROLLPOS, 0, ref value);
			}
		}


		public int TabStops
		{
			get { return 0; }
			set { }
		}

	
		public string[] TextLines
		{
			get { return pad.Lines; }
		}


		public bool WhitespaceVisible
		{
			get { return false; }
		}


		public void DoCut (object sender, EventArgs e)
		{
			pad.Cut();
		}


		public void DoCopy (object sender, EventArgs e)
		{
			pad.Copy();
		}


		public void DoClear (object sender, EventArgs e)
		{
			pad.Clear();
		}


		public void DoFind (object sender, EventArgs e)
		{
			this.SearchDialog.Execute(this, false, false);
		}


		public void DoGotoLine (object sender, EventArgs e)
		{
			throw new Exception("Notepad.DoGotoLine not implemented");
			//int index = 0;
			//for (int i = 1; (i < line) && (i < this.Lines.Length); i++)
			//{
			//    index += this.Lines[i - 1].Length + 1;
			//}

			//this.Select(index, 0);
		}


		public void DoMakeLowercase (object sender, EventArgs e)
		{
			ChangeCase(true);
		}


		public void DoMakeUppercase (object sender, EventArgs e)
		{
			ChangeCase(false);
		}


		private void ChangeCase (bool toLowercase)
		{
			bool noSelection = (pad.SelectionLength == 0);
			if (noSelection)
				pad.Select(pad.SelectionStart, 1);

			// preserve current positioning
			int caret = pad.SelectionStart;
			int length = pad.SelectionLength;
			Point pos = ScrollPosition;

			// change case
			if (toLowercase)
				pad.SelectedText = pad.SelectedText.ToLower();
			else
				pad.SelectedText = pad.SelectedText.ToUpper();

			// restore current positioning
			if (noSelection)
				pad.Select(caret + 1, 0);
			else
				pad.Select(caret, length);

			ScrollPosition = pos;
		}


		public void DoPaste (object sender, EventArgs e)
		{
			pad.Paste();
		}


		public void DoRedo (object sender, EventArgs e)
		{
			pad.Redo();
		}


		public void DoSelectAll (object sender, EventArgs e)
		{
			pad.SelectAll();
		}


		public void DoToggleWhitespace (object sender, EventArgs e)
		{
		}


		public void DoUndo (object sender, EventArgs e)
		{
			pad.Undo();
		}


		public void SaveFile (string filename)
		{
			pad.SaveFile(filename, RichTextBoxStreamType.PlainText);
		}

		#endregion IEditor

		/// <summary>
		/// Erases all text from the notepad.
		/// </summary>

		public void Clear ()
		{
			pad.Clear();
		}



		/// <summary>
		/// Writes the specified text to the notepad.  The next text will be appended
		/// to this line of text.
		/// </summary>
		/// <param name="text">The text substring to write.</param>

		public void Write (string text)
		{
			pad.AppendText(text);
		}


		/// <summary>
		/// Writes a newline to the notepad.  This will end the current line
		/// and begin a new line.
		/// </summary>

		public void WriteLine ()
		{
			pad.AppendText(CR);
		}


		/// <summary>
		/// Writes a line of text to the notepad and begins a new line.
		/// </summary>
		/// <param name="text">The line of text to write.</param>

		public void WriteLine (string text)
		{
			pad.AppendText(text + CR);
		}


		/// <summary>
		/// Writes a line of text to the notepad and begins a new line.
		/// </summary>
		/// <param name="text">The line of text to write.</param>

		public void WriteNote (string text)
		{
			var savecolor = pad.ForeColor;
			pad.SelectionColor = FontsAndColors.NoteForeground;
			pad.AppendText(text + CR);
			pad.SelectionColor = savecolor;
		}


		private void DoOpeningContextMenu (object sender, CancelEventArgs e)
		{
			cutToolStripMenuItem.Enabled = HasSelection;
			copyToolStripMenuItem.Enabled = HasSelection;
			pasteToolStripMenuItem.Enabled = CanPaste;
			selectAllToolStripMenuItem.Enabled = HasContent;
			clearWindowToolStripMenuItem.Enabled = HasContent;
		}

		private void DoOptions (object sender, EventArgs e)
		{
			if (OpeningOptions != null)
				OpeningOptions(sender, new OpeningOptionsEventArgs("/OrqaOptions/results"));
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
		// Drag/Drop handlers
		//========================================================================================

		private void DoDragEnter (object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text) ||
				e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}


		private void DoDragDrop (object sender, DragEventArgs e)
		{
			string text = String.Empty;

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] names = (string[])e.Data.GetData(DataFormats.FileDrop);
				StringBuilder content = new StringBuilder();

				foreach (string name in names)
				{
					StreamReader reader = new StreamReader(name);
					content.Append(reader.ReadToEnd());
					reader.Close();
					reader = null;
				}

				text = content.ToString();
			}
			else if (e.Data.GetDataPresent(DataFormats.Text))
			{
				text = (string)e.Data.GetData("Text");
			}

			if (text.Length > 0)
			{
				int start = pad.SelectionStart;
				pad.SelectedText = text;

				pad.Select(start, text.Length);
			}
		}


		private void DoGotFocus (object sender, EventArgs e)
		{
			this.OnGotFocus(e);
		}


		private void DoSaveAs (object sender, EventArgs e)
		{
			if (Saving != null)
				Saving(this, e);
		}


		private void DoTextChanged (object sender, EventArgs e)
		{
			this.OnTextChanged(e);
		}


		//========================================================================================
		// Window locking - used to prevent autoScroll
		//========================================================================================

		/// <summary>
		/// Locks the window, disabling all drawing, while report is being generated.
		/// </summary>

		public void Lock ()
		{
			LockWindowUpdate(this.Handle);
		}


		/// <summary>
		/// Unlocks the window, enabling all drawing, allowing the report to finally
		/// be displayed.
		/// </summary>

		public void Unlock ()
		{
			LockWindowUpdate((IntPtr)0);
		}


		//========================================================================================
		// MoveHome()
		//========================================================================================

		/// <summary>
		/// Moves the caret to the first character of the first line.
		/// </summary>

		public void MoveHome ()
		{
			pad.Select(0, 0);
			pad.ScrollToCaret();
		}


		private void DoSaving (object sender, EventArgs e)
		{
			if (Saving != null)
				Saving(this, e);
		}


		//========================================================================================
		// ISearch
		//========================================================================================

		#region ISearch Members
		private string findText = null;
		private SearchOptions findOptions = SearchOptions.None;

		public bool CanFindNext ()
		{
			return HasContent;
		}

		public bool Find (string text)
		{
			return pad.Find(text) != 0;
		}

		public bool Find (string text, SearchOptions options)
		{
			isFirstSearch = !text.Equals(findText);

			findText = text;
			findOptions = options;

			RichTextBoxFinds finds = RichTextBoxFinds.None;
			if ((options & SearchOptions.BackwardSearch) > 0)
				finds |= RichTextBoxFinds.Reverse;
			if ((options & SearchOptions.CaseSensitive) > 0)
				finds |= RichTextBoxFinds.MatchCase;
			if ((options & SearchOptions.WholeWordsOnly) > 0)
				finds |= RichTextBoxFinds.WholeWord;

			int start = pad.SelectionStart;
			if ((options & SearchOptions.EntireScope) > 0)
				start = 0;

			int position = pad.Find(text, start, finds);
			return position != start;
		}

		public bool Find (string text, SearchOptions options, Regex expression)
		{
			if (expression == null)
			{
				return Find(text, options);
			}
			return false;
		}

		public bool FindNext ()
		{
			return Find(findText, findOptions);
		}

		public bool FindNextSelected ()
		{
			return false;
		}

		public bool FindPrevious ()
		{
			return Find(findText, findOptions | SearchOptions.BackwardSearch);
		}

		public bool FindPreviousSelected ()
		{
			return false;
		}

		public void FinishIncrementalSearch ()
		{
			return;
		}

		public string GetTextAtCursor ()
		{
			string text = String.Empty;
			if (pad.SelectionLength > 0)
			{
				text = pad.SelectedText;
			}

			return text;
		}

		public bool IncrementalSearch (string Key, bool DeleteLast)
		{
			return false;
		}

		public int MarkAll (string String)
		{
			return 0;
		}

		public int MarkAll (string String, River.Orqa.Editor.SearchOptions Options)
		{
			return 0;
		}

		public int MarkAll (string String, River.Orqa.Editor.SearchOptions Options, System.Text.RegularExpressions.Regex Expression)
		{
			return 0;
		}

		public bool Replace (string String, string ReplaceWith)
		{
			return false;
		}

		public bool Replace (string String, string ReplaceWith, River.Orqa.Editor.SearchOptions Options)
		{
			return false;
		}

		public bool Replace (string String, string ReplaceWith, River.Orqa.Editor.SearchOptions Options, System.Text.RegularExpressions.Regex Expression)
		{
			return false;
		}

		public int ReplaceAll (string String, string ReplaceWith)
		{
			return 0;
		}

		public int ReplaceAll (string String, string ReplaceWith, River.Orqa.Editor.SearchOptions Options)
		{
			return 0;
		}

		public int ReplaceAll (string String, string ReplaceWith, River.Orqa.Editor.SearchOptions Options, System.Text.RegularExpressions.Regex Expression)
		{
			return 0;
		}

		public void StartIncrementalSearch ()
		{
			return;
		}

		public void StartIncrementalSearch (bool BackwardSearch)
		{
			return;
		}

		bool isFirstSearch = true;
		public bool FirstSearch
		{
			get { return isFirstSearch; }
			set { isFirstSearch = value; }
		}

		public River.Orqa.Editor.Dialogs.IGotoLineDialog GotoLineDialog
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string IncrementalSearchString
		{
			get { return String.Empty; }
		}

		public bool InIncrementalSearch
		{
			get { return false; }
		}

		River.Orqa.Editor.Dialogs.ISearchDialog searchDialog = null;
		public River.Orqa.Editor.Dialogs.ISearchDialog SearchDialog
		{
			get
			{
				if (searchDialog == null)
				{
					searchDialog = new River.Orqa.Editor.Dialogs.SearchDialog();
				}

				return searchDialog;
			}
			set
			{
				searchDialog = value;
			}
		}

		public int SearchLen
		{
			get { return 0; }
		}

		public Point SearchPos
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool Find (
			string text, SearchOptions Options, Regex Expression,
			ref Point Position, out int Len)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
