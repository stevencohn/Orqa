///************************************************************************************************
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
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using River.Orqa.Editor;
	using River.Orqa.Editor.Dialogs;
	using River.Orqa.Editor.Syntax;
	using River.Orqa.Options;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class Editor
	//********************************************************************************************

	internal class Editor : River.Orqa.Editor.SyntaxEdit
	{
		private Translator translator;
		private bool isSaved;
		private bool hadContent;

		// Events

		public event CaretChangedEventHandler CaretChanged;
		public event InsertModeChangedEventHandler InsertModeChanged;
		public event EditorEventHandler Edited;
		public event EventHandler ToggleResults;


		//========================================================================================
		// Main
		//========================================================================================

		internal Editor ()
			: base()
		{
			InitializeEditor();
		}


		internal Editor (IContainer container)
			: base(container)
		{
			InitializeEditor();
		}


		private void InitializeEditor ()
		{
			// inherited
			this.AllowDrop = true;
			this.BackColor = FontsAndColors.PlainTextBackground;
			this.BorderStyle = EditBorderStyle.None;
			//this.Braces.BracesColor = Color.Blue;
			this.Braces.ClosingBraces = new char[] { ')', ']', '}' };
			this.Braces.OpenBraces = new char[] { '(', '[', '{' };
			this.Font = new Font("Lucida Console", 8F);
			this.ForeColor = FontsAndColors.PlainTextForeground;
			this.Gutter.BrushColor = Color.Black;
			this.Gutter.Options |= GutterOptions.PaintLinesOnGutter;
			this.Gutter.LineBookmarksColor = Color.Red;
			this.Gutter.LineNumbersBackColor = Color.Black;
			this.Gutter.LineNumbersForeColor = Color.DarkGray;
			this.Gutter.PenColor = Color.Gray;
			this.HyperText.HighlightUrls = true;
			this.HyperText.UrlColor = Color.Blue;
			this.Lexer = null;
			this.LineSeparator.HighlightForeColor = SystemColors.ScrollBar;
			this.LineSeparator.LineColor = SystemColors.ControlText;
			this.Location = new Point(0, 0);
			this.Margin.PenColor = FontsAndColors.MarginDivider;
			this.Margin.Position = 77;
			this.Margin.Visible = false;
			this.Name = "SyntaxEdit";
			this.Outlining.AllowOutlining = false;
			this.Scrolling.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
			this.Scrolling.ShowScrollHint = true;
			this.Scrolling.SmoothScroll = true;
			this.Selection.BackColor = FontsAndColors.SelectedTextBackground;
			this.Selection.ForeColor = SystemColors.HighlightText;
			this.Selection.InActiveBackColor = SystemColors.InactiveCaption;
			this.Selection.InActiveForeColor = SystemColors.InactiveCaptionText;
			this.TabIndex = 0;
			this.WhiteSpace.SymbolColor = Color.Teal;

			//this.SearchDialog = new SearchDialog();

			this.SourceStateChanged += new NotifyEvent(DoSourceStateChanged);

			// this test is a fix for this.DesignMode not being set yet
			if (System.ComponentModel.LicenseManager.UsageMode
				!= System.ComponentModel.LicenseUsageMode.Designtime)
			{
				translator = new Translator("Query");
			}

			isSaved = true;
			hadContent = false;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		#region Properties

		public bool CanPaste
		{
			get
			{
				return (Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf)) ||
					   (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)) ||
					   (Clipboard.GetDataObject().GetDataPresent(DataFormats.UnicodeText));
			}
		}


		public bool CanUndo
		{
			get { return this.Source.CanUndo(); }
		}


		public int Column
		{
			get { return this.Source.Position.X + 1; }
		}


		public bool HasContent
		{
			get { return (this.Text.Length > 0); }
		}


		public bool HasSelection
		{
			get { return !this.Selection.IsEmpty(); }
		}


		public bool IsColorized
		{
			get
			{
				return !this.DisableSyntaxPaint;
			}

			set
			{
				string styleXml;
				string settingsXml;

				if (value)
				{
					this.DisableColorPaint = false;
					this.DisableSyntaxPaint = false;
					styleXml = "SqlStyle";
					settingsXml = "SqlSettings";
					this.DisableSyntaxPaint = false;
				}
				else
				{
					this.DisableColorPaint = true;
					this.DisableSyntaxPaint = true;
					styleXml = "TextStyle";
					settingsXml = "TextSettings";
					this.DisableSyntaxPaint = true;
				}

				ILexer lexer = new Parser();
				lexer.LoadScheme(new StringReader(translator.GetString(styleXml)));
				this.Source.Lexer = lexer;

				SyntaxSettings settings = new SyntaxSettings();
				settings.LoadStream(new StringReader(translator.GetString(settingsXml)));
				settings.ApplyToEdit(this);
			}
		}


		public bool IsSaved
		{
			get { return isSaved; }
			set { isSaved = value; }
		}


		public int Line
		{
			get { return this.Source.Position.Y + 1; }
		}


		public Point ScrollPosition
		{
			get { return new Point(this.Source.Position.X, this.Source.Position.Y); }
			set { this.Source.Position = value; }
		}

		public Color SelectionColor
		{
			get { return this.Selection.ForeColor; }
			set { this.Selection.ForeColor = value; }
		}

		public int SelectionLength
		{
			get { return this.Selection.SelectionLength; }
		}

		public int SelectionStart
		{
			get { return this.Selection.SelectionStart; }
		}

		public string SelectedText
		{
			get { return this.Selection.SelectedText; }
		}

		#endregion Properties


		//========================================================================================
		// Methods
		//========================================================================================

		public void AppendText (string text)
		{
			this.Text += text;
		}


		public void Cut ()
		{
			this.Selection.Cut();
		}


		public void Copy ()
		{
			this.Selection.Copy();
		}


		public void Clear ()
		{
			this.SelectAll();
			this.Cut();
		}


		public void InsertText (string text)
		{
			this.Source.InsertBlock(text.Split('\n'));
		}


		public void Paste ()
		{
			this.Selection.Paste();
		}


		public void ScrollToCaret ()
		{
		}


		public void Select (int start, int length)
		{
			this.Selection.SelectionStart = start;
			this.Selection.SelectionLength = length;
		}


		public void SelectAll ()
		{
			this.Selection.SelectAll();
		}


		//========================================================================================
		// DoSelectionChanged()
		//		As the mouse is dragged across the control to select or unselect text,
		//		the editor fires the selectionChanged event.  We can capture this event
		//		and re-fire it as a CaretChanged event to give position feedback.
		//========================================================================================

		private void DoSelectionChanged (object sender, EventArgs e)
		{
			if (CaretChanged != null)
				CaretChanged(this, new CaretChangedEventArgs(Line, Column));
		}


		//========================================================================================
		// DoSourceStateChanged()
		//		When general activity occurs in the editor control, it fire a NotifyEvent.
		//		We can capture it to provide feedback for position, dirty-bit, etc.
		//========================================================================================

		private void DoSourceStateChanged (object sender, NotifyEventArgs e)
		{
			if (CaretChanged != null)
				CaretChanged(this, new CaretChangedEventArgs(Line, Column));

			if (isSaved)
			{
				if ((e.State & NotifyState.Edit) > 0)
				{
					isSaved = false;
					if (Edited != null)
						Edited(this, new EditorEventArgs(EditorAction.Unsaved));
				}
			}

			if (e.State == NotifyState.OverWriteChanged)
			{
				if (InsertModeChanged != null)
					InsertModeChanged(this, new InsertModeChangedEventArgs(!this.Source.OverWrite));
			}

			if ((this.Text.Length == 0) && hadContent)
			{
				hadContent = false;
				if (Edited != null)
					Edited(this, new EditorEventArgs(EditorAction.HasNoContent));
			}
			else if ((this.Text.Length > 0) && !hadContent)
			{
				hadContent = true;
				if (Edited != null)
					Edited(this, new EditorEventArgs(EditorAction.HasContent));
			}
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

		protected override void OnDragOver (DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop) ||
				e.Data.GetDataPresent(DataFormats.Text) ||
				e.Data.GetDataPresent(DataFormats.UnicodeText))
			{
				this.Focus();

				if (this.Selection.SelectionState == SelectionState.Drag)
					e.Effect = DragDropEffects.Move;
				else
					e.Effect = DragDropEffects.Copy;

				this.Selection.BeginUpdate();
				try
				{
					Point pos = this.PointToClient(Cursor.Position);
					bool flag = false;
					pos = this.ScreenToText(pos.X, pos.Y, ref flag);
					this.Position = pos;
				}
				finally
				{
					this.Selection.EndUpdate();
				}
			}
			else
			{
				base.OnDragOver(e);
			}
		}


		protected override void OnDragDrop (DragEventArgs e)
		{
			string text = null;

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
			else if (e.Data.GetDataPresent(DataFormats.Text) ||
				e.Data.GetDataPresent(DataFormats.UnicodeText))
			{
				text = (string)e.Data.GetData(DataFormats.UnicodeText);
				if (text == null)
					text = (string)e.Data.GetData(DataFormats.Text);

				if (this.Selection.SelectionState == SelectionState.Drag)
				{
					Point pos = this.Position;
					this.Selection.Cut();
					this.Selection.SelectionState = SelectionState.None;
					this.Position = pos;
				}
			}

			if ((text != null) && (text.Length > 0))
			{
				string[] strings = text.Replace("\r", String.Empty).Split(Environment.NewLine.ToCharArray());
				this.Source.InsertBlock(strings);
			}
		}
	}
}
