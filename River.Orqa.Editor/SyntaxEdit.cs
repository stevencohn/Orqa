namespace River.Orqa.Editor
{
	using River.Orqa.Editor.Common;
	using River.Orqa.Editor.Dialogs;
	using River.Orqa.Editor.Syntax;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.ComponentModel.Design;
	using System.Drawing;
	using System.Drawing.Design;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;

	[ToolboxBitmap(typeof(SyntaxEdit), "Images.SyntaxEdit.bmp")]
	public class SyntaxEdit : Control, ISyntaxEdit, IDrawLine, ISearch, ITextSearch, INotifier, 
		ICaret, INavigateEx, INavigate, IEdit, IWordWrap, IFmtExport, IExport, IFmtImport, 
		IImport, ICodeCompletion, IControlProps
	{
		// Events
		[Category("SyntaxEdit")]
		public event HyperTextEvent CheckHyperText
		{
			add
			{
				this.hyperText.CheckHyperText += value;
			}
			remove
			{
				this.hyperText.CheckHyperText -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event CustomDrawEvent CustomDraw;
		[Category("SyntaxEdit")]
		public event DrawHeaderEvent DrawHeader
		{
			add
			{
				this.pages.DrawHeader += value;
			}
			remove
			{
				this.pages.DrawHeader -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event EventHandler GutterClick
		{
			add
			{
				this.gutter.Click += value;
			}
			remove
			{
				this.gutter.Click -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event EventHandler GutterDblClick
		{
			add
			{
				this.gutter.DoubleClick += value;
			}
			remove
			{
				this.gutter.DoubleClick -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event EventHandler HorizontalScroll
		{
			add
			{
				this.scrolling.HorizontalScroll += value;
			}
			remove
			{
				this.scrolling.HorizontalScroll -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event UrlJumpEvent JumpToUrl
		{
			add
			{
				this.hyperText.JumpToUrl += value;
			}
			remove
			{
				this.hyperText.JumpToUrl -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event CodeCompletionEvent NeedCodeCompletion;
		[Category("SyntaxEdit")]
		public event EventHandler SelectionChanged
		{
			add
			{
				this.selection.SelectionChanged += value;
			}
			remove
			{
				this.selection.SelectionChanged -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event NotifyEvent SourceStateChanged;
		[Category("SyntaxEdit")]
		public event EventHandler VerticalScroll
		{
			add
			{
				this.scrolling.VerticalScroll += value;
			}
			remove
			{
				this.scrolling.VerticalScroll -= value;
			}
		}
		[Category("SyntaxEdit")]
		public event WordSpellEvent WordSpell
		{
			add
			{
				this.spelling.WordSpell += value;
			}
			remove
			{
				this.spelling.WordSpell -= value;
			}
		}

		// Methods
		public SyntaxEdit()
		{
			this.components = null;
			this.drawSelection = true;
			this.codeCompletionChars = EditConsts.DefaultCodeCompletionChars;
			this.borderStyle = EditBorderStyle.Fixed3D;
			this.wantReturns = true;
			this.wantTabs = true;
			this.oldLine = -1;
			this.mouseUrl = string.Empty;
			this.stringFormat = StringFormat.GenericTypographic;
			this.internalSource = new TextSource();
			this.Source = this.internalSource;
			this.source = null;
			this.painter = new TextPainter(this);
			this.gutter = new River.Orqa.Editor.Gutter(this);
			this.displayLines = new DisplayStrings(this, this.Lines);
			this.margin = new River.Orqa.Editor.Margin(this);
			this.selection = new River.Orqa.Editor.Selection(this);
			this.pages = new EditPages(this);
			this.pages.Add();
			this.printing = new River.Orqa.Editor.Printing(this);
			this.pages.InitDefaultPrinterSettings(this.printing.PrinterSettings);
			this.whiteSpace = new River.Orqa.Editor.WhiteSpace(this);
			this.lineSeparator = new River.Orqa.Editor.LineSeparator(this);
			this.braces = new River.Orqa.Editor.Braces(this);
			this.hyperText = new HyperTextEx(this);
			this.outlining = new River.Orqa.Editor.Outlining(this);
			this.spelling = new River.Orqa.Editor.Spelling(this);
			this.scrolling = new River.Orqa.Editor.Scrolling(this);
			this.keyList = new River.Orqa.Editor.KeyList(this);
			this.drawInfo = new DrawInfo();
			this.codeCompletionArgs = new CodeCompletionArgs();
			this.customDrawEventArgs = new CustomDrawEventArgs();
			this.notifyEventArgs = new NotifyEventArgs();
			base.QueryContinueDrag += new QueryContinueDragEventHandler(this.QueryEndDrag);
			this.lineStyles = new LineStylesEx(this);
			this.Cursor = Cursors.IBeam;
			this.Font = new Font(FontFamily.GenericMonospace, 10f);
			this.painter.Font = this.Font;
			base.SetStyle(ControlStyles.AllPaintingInWmPaint | (ControlStyles.StandardDoubleClick | (ControlStyles.UserMouse | (ControlStyles.Selectable | (ControlStyles.StandardClick | (ControlStyles.Opaque | ControlStyles.UserPaint))))), true);
			this.BackColor = Consts.DefaultControlBackColor;
			base.Width = 100;
			base.Height = 0x60;
		}

		public SyntaxEdit(IContainer container) : this()
		{
			container.Add(this);
		}

		private void BeginLineEndUpdate()
		{
			this.displayLines.LineEndUpdateCount++;
		}

		protected internal void CancelDragging()
		{
			this.dragMargin = false;
			this.pages.CancelDragging();
			this.margin.CancelDragging();
		}

		public bool CanFindNext()
		{
			return !this.firstSearch;
		}

		public int CharsInWidth()
		{
			return this.CharsInWidth(this.ClientWidth());
		}

		public int CharsInWidth(int Width)
		{
			int num1 = this.painter.FontWidth;
			if (num1 == 0)
			{
				return 0;
			}
			return (Width / num1);
		}

		public int CharsInWidth(int Width, bool Exact)
		{
			int num1 = this.painter.FontWidth;
			if (num1 == 0)
			{
				return 0;
			}
			int num2 = Width / num1;
			if (!Exact && ((Width % num1) != 0))
			{
				num2++;
			}
			return num2;
		}

		private bool CheckCursor(Point Pt)
		{
			River.Orqa.Editor.HitTestInfo info1;
			Cursor cursor1 = null;
			info1 = new River.Orqa.Editor.HitTestInfo();
			this.GetHitTest(Pt, ref info1);
			if ((((info1.HitTest & HitTest.Left) != HitTest.None) || ((info1.HitTest & HitTest.Right) != HitTest.None)) || (((info1.HitTest & HitTest.Above) != HitTest.None) || ((info1.HitTest & HitTest.Below) != HitTest.None)))
			{
				cursor1 = Cursors.Arrow;
			}
			else if ((info1.HitTest & HitTest.Gutter) != HitTest.None)
			{
				if (((info1.HitTest & HitTest.OutlineArea) != HitTest.None) && ((info1.HitTest & HitTest.OutlineImage) == HitTest.None))
				{
					cursor1 = this.LeftArrowCursor();
				}
				else
				{
					cursor1 = Cursors.Arrow;
				}
			}
			else if ((((info1.HitTest & HitTest.Margin) != HitTest.None) && this.Margin.AllowDrag) && ((Control.ModifierKeys & Keys.Control) != Keys.None))
			{
				cursor1 = Cursors.SizeWE;
			}
			else if (((info1.HitTest & HitTest.HyperText) != HitTest.None) && ((Control.ModifierKeys & Keys.Control) != Keys.None))
			{
				cursor1 = Cursors.Hand;
			}
			else if (this.InIncrementalSearch)
			{
				if ((this.searchOptions & SearchOptions.BackwardSearch) != SearchOptions.None)
				{
					cursor1 = this.ReverseIncrementalSearchCursor();
				}
				else
				{
					cursor1 = this.IncrementalSearchCursor();
				}
			}
			else if ((info1.HitTest & HitTest.Selection) != HitTest.None)
			{
				cursor1 = Cursors.Arrow;
			}
			if (cursor1 != null)
			{
				Win32.SetCursor(cursor1.Handle);
			}
			return (cursor1 != null);
		}

		private void CheckIncrementalSeacrh()
		{
			if (!this.incrSearchFlag)
			{
				this.FinishIncrementalSearch();
			}
		}

		private int ClientHeight()
		{
			return this.ClientRect.Height;
		}

		private int ClientWidth()
		{
			return (this.ClientRect.Width - this.gutter.GetWidth());
		}

		protected void CloseCodeCompletionBox(object sender, ClosingEventArgs e)
		{
			if ((e.Accepted && (e.Text != null)) && (e.Text != string.Empty))
			{
				ICodeCompletionWindow window1 = (ICodeCompletionWindow) sender;
				this.InsertTextFromProvider(e.Text, window1.StartPos);
			}
		}

		public void CodeTemplates()
		{
			this.CodeTemplates(this.codeCompletionArgs);
			this.OnNeedCompletion(this.codeCompletionArgs);
		}

		protected virtual void CodeTemplates(CodeCompletionArgs e)
		{
			e.Init(CodeCompletionType.CodeTemplates, this.Position);
		}

		public void CompleteWord()
		{
			if (this.IsValidText(this.Position))
			{
				this.ListMembers(this.codeCompletionArgs, CodeCompletionType.CompleteWord);
				this.OnNeedCompletion(this.codeCompletionArgs);
			}
		}

		protected void CompleteWord(CodeCompletionArgs e)
		{
			this.ListMembers(e, CodeCompletionType.CompleteWord);
		}

		public void CreateCaret()
		{
			if (!this.hideCaret && base.IsHandleCreated)
			{
				Size size1 = this.GetCaretSize(this.Position);
				Win32.CreateCaret(base.Handle, IntPtr.Zero, size1.Width, size1.Height);
				Win32.ShowCaret(base.Handle);
			}
		}

		protected override void CreateHandle()
		{
			base.CreateHandle();
			this.scrolling.UpdateScroll();
			this.pages.UpdateRulers();
		}

		public void DestroyCaret()
		{
			Win32.DestroyCaret();
		}

		private void DisableCodeCompletionTimer()
		{
			if (this.codeCompletionTimer != null)
			{
				this.codeCompletionTimer.Enabled = false;
			}
		}

		public Point DisplayToScreen(int X, int Y)
		{
			return this.DisplayToScreen(X, Y, false);
		}

		public Point DisplayToScreen(int X, int Y, bool Average)
		{
			int num2;
			int num1 = Average ? (this.painter.FontMetrics.AveCharWidth * X) : this.MeasureLine(Y, 0, X);
			if (this.pages.PageType == PageType.PageLayout)
			{
				IEditPage page1 = this.pages.GetPageAt(X, Y);
				Rectangle rectangle1 = page1.ClientRect;
				if (num1 != 0x7fffffff)
				{
					num1 += (rectangle1.Left + this.gutter.GetWidth());
				}
				num2 = (Y - page1.StartLine) * this.painter.FontHeight;
				num2 += rectangle1.Top;
			}
			else
			{
				Rectangle rectangle2 = this.ClientRect;
				num2 = ((Y - this.scrolling.WindowOriginY) * this.painter.FontHeight) + rectangle2.Top;
				if (num1 != 0x7fffffff)
				{
					num1 = ((num1 - (this.painter.FontWidth * this.scrolling.WindowOriginX)) + this.gutter.GetWidth()) + rectangle2.Left;
				}
			}
			return new Point(num1, num2);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		protected void DoCodeCompletion()
		{
			this.DisableCodeCompletionTimer();
			if (this.codeCompletionArgs.Interval == 0)
			{
				this.OnCodeCompletion(this, EventArgs.Empty);
			}
			else
			{
				this.CodeCompletionTimer.Interval = this.codeCompletionArgs.Interval;
				this.CodeCompletionTimer.Enabled = true;
			}
		}

		protected void DoCodeToolTip(string s)
		{
			this.codeCompletionArgs.CompletionType = CodeCompletionType.QuickInfo;
			this.codeCompletionArgs.Interval = Consts.DefaultHintDelay;
			this.codeCompletionArgs.NeedShow = true;
			this.codeCompletionArgs.ToolTip = true;
			this.codeCompletionArgs.Provider = this.GetQuickInfo(s, 0, 0);
			this.DoCodeCompletion();
		}

		protected bool DoFind(string String, SearchOptions Options, Regex Expression, bool Silent)
		{
			bool flag2 = false;
			bool flag1 = (Options & SearchOptions.SelectionOnly) != SearchOptions.None;
			if (flag1 && this.Selection.IsEmpty())
			{
				return false;
			}
			if (this.FirstSearch)
			{
				this.searchText = String;
				this.searchOptions = Options;
				this.searchExpression = Expression;
				if ((SearchOptions.EntireScope & Options) != SearchOptions.None)
				{
					if ((Options & SearchOptions.BackwardSearch) != SearchOptions.None)
					{
						if (flag1)
						{
							this.searchPos = new Point(this.Selection.SelectionRect.Right, this.Selection.SelectionRect.Bottom);
						}
						else
						{
							this.searchPos = new Point(this.Lines[this.Lines.Count - 1].Length, this.Lines.Count - 1);
						}
					}
					else if (flag1)
					{
						this.searchPos = this.Selection.SelectionRect.Location;
					}
					else
					{
						this.searchPos = new Point(0, 0);
					}
				}
				else
				{
					this.searchPos = this.Source.Position;
				}
			}
			else
			{
				this.searchPos = this.Source.Position;
			}
			if (flag1)
			{
				Point point1 = this.Selection.TextToSelectionPoint(this.searchPos);
				ISyntaxStrings strings1 = new SyntaxStrings(null);
				strings1.Text = this.Selection.SelectedText;
				if (strings1.Find(String, Options, Expression, ref point1, out this.searchLen))
				{
					this.searchPos = this.Selection.SelectionToTextPoint(point1);
				}
			}
			else if ((Options & SearchOptions.SearchHiddenText) == SearchOptions.None)
			{
				Point point2 = this.displayLines.PointToDisplayPoint(this.searchPos, false);
				if (this.displayLines.Find(String, Options, Expression, ref point2, out this.searchLen))
				{
					this.searchPos = this.displayLines.DisplayPointToPoint(point2);
				}
			}
			else
			{
				flag2 = this.Find(String, Options, Expression, ref this.searchPos, out this.searchLen);
			}
			if (flag2)
			{
				this.TextFound(Options, this.searchPos, this.searchLen, Silent);
				this.FirstSearch = false;
			}
			return flag2;
		}

		protected void DoFontChanged()
		{
			if (!this.gutter.LineNumbersChanged() || !this.WordWrap)
			{
				this.UpdateWordWrap();
			}
			this.painter.Clear();
			this.painter.Font = this.Font;
			this.UpdateMonospaced();
			this.displayLines.MaxLineChanged();
			this.pages.UpdateRulers();
		}

		protected int DoMarkAll(string String, SearchOptions Options, Regex Expr)
		{
			this.FirstSearch = true;
			int num1 = 0;
			this.Selection.BeginUpdate();
			this.Source.BeginUpdate(UpdateReason.Other);
			try
			{
				while (this.DoFind(String, Options, Expr, true))
				{
					this.Source.BookMarks.SetBookMark(this.searchPos, 0x7fffffff);
					num1++;
				}
			}
			finally
			{
				this.Source.EndUpdate();
				this.Selection.EndUpdate();
			}
			return num1;
		}

		private void DoOutlineText()
		{
			if (this.outlining.AllowOutlining)
			{
				if (((TextSource) this.Source).NeedOutlineText())
				{
					IFormatText text1 = (IFormatText) this.Source.Lexer;
					ArrayList list1 = new ArrayList();
					text1.Outline(list1);
					this.outlining.SetOutlineRanges(list1, true);
					this.MakeVisible(this.Position);
				}
				else
				{
					this.outlining.UnOutline();
				}
			}
		}

		protected internal bool DoProcessKeyMessage(ref Message msg)
		{
			return this.ProcessKeyMessage(ref msg);
		}

		protected bool DoReplace(string String, string ReplaceWith, SearchOptions Options, Regex Expr, bool Silent)
		{
			bool flag1 = false;
			if (!(!this.Source.ReadOnly && this.DoFind(String, Options, Expr, Silent)))
			{
				return flag1;
			}
			if ((SearchOptions.PromptOnReplace & Options) != SearchOptions.None)
			{
				DialogResult result1 = MessageBox.Show(string.Format("Replace this occurrence of {0}", ReplaceWith), "Confirm", MessageBoxButtons.YesNoCancel);
				DialogResult result2 = result1;
				if (result2 != DialogResult.Cancel)
				{
					switch (result2)
					{
						case DialogResult.Yes:
						{
							flag1 = true;
							goto Label_0067;
						}
						case DialogResult.No:
						{
							flag1 = false;
							goto Label_0067;
						}
					}
				}
				else
				{
					return false;
				}
			}
		Label_0067:
			if ((SearchOptions.BackwardSearch & Options) != SearchOptions.None)
			{
				this.Source.DeleteRight(this.searchLen);
				this.Source.Insert(ReplaceWith);
				return flag1;
			}
			this.Source.Navigate(-this.searchLen, 0);
			this.Source.DeleteRight(this.searchLen);
			this.Source.Insert(ReplaceWith);
			return flag1;
		}

		protected int DoReplaceAll(string String, string ReplaceWith, SearchOptions Options, Regex Expr)
		{
			this.FirstSearch = true;
			int num1 = 0;
			this.Source.BeginUpdate(UpdateReason.Other);
			try
			{
				while (this.DoReplace(String, ReplaceWith, Options, Expr, (Options & SearchOptions.PromptOnReplace) == SearchOptions.None))
				{
					num1++;
				}
			}
			finally
			{
				this.Source.EndUpdate();
			}
			return num1;
		}

		protected void DoSourceStateChanged(NotifyState State, int First, int Last)
		{
			if (this.SourceStateChanged != null)
			{
				this.notifyEventArgs.FirstChanged = First;
				this.notifyEventArgs.LastChanged = Last;
				this.notifyEventArgs.State = State;
				this.notifyEventArgs.Update = false;
				this.SourceStateChanged(this, this.notifyEventArgs);
			}
		}

		private void DrawAfterLineEnd(int W, Point Point, LineStyle Ls, bool selLine, int Offset, int ALeft, int ARight, int Index, bool Highlight)
		{
			if (W > Point.X)
			{
				Color color1 = ((Ls != null) && ((Ls.Options & LineStyleOptions.BeyondEol) != LineStyleOptions.None)) ? Ls.GetBackColor(this.BackColor) : ((Highlight && (this.lineSeparator.HighlightBackColor != Color.Empty)) ? this.lineSeparator.HighlightBackColor : this.BackColor);
				bool flag1 = ((this.transparent && !Highlight) && !selLine) && (Ls == null);
				if ((selLine && ((SelectionOptions.SelectBeyondEol & this.selection.Options) != SelectionOptions.None)) || (this.selection.SelectionType == SelectionType.Block))
				{
					int num1 = this.gutter.GetWidth();
					int num2 = this.MeasureLine(Index, 0, ALeft);
					if (num2 != 0x7fffffff)
					{
						num2 -= (Offset - num1);
					}
					int num3 = this.MeasureLine(Index, 0, ARight);
					if (num3 != 0x7fffffff)
					{
						num3 -= (Offset - num1);
					}
					else
					{
						num3 = W;
					}
					int num4 = this.painter.FontHeight;
					if (num3 > Point.X)
					{
						num2 = Math.Max(num2, Point.X);
						if ((num2 > Point.X) && !flag1)
						{
							this.painter.BkColor = color1;
							this.FillRectangleAfterLineEnd(Point.X, Point.Y, num2 - Point.X, num4, Index, false, Highlight);
						}
						this.painter.BkColor = this.selBackColor;
						this.FillRectangleAfterLineEnd(num2, Point.Y, num3 - num2, num4, Index, true, Highlight);
						if ((num3 < W) && !flag1)
						{
							this.painter.BkColor = color1;
							this.FillRectangleAfterLineEnd(num3, Point.Y, W - num3, num4, Index, false, Highlight);
						}
						return;
					}
				}
				if (!flag1)
				{
					this.painter.BkColor = color1;
					this.FillRectangleAfterLineEnd(Point.X, Point.Y, W - Point.X, this.painter.FontHeight, Index, false, Highlight);
				}
			}
		}

		public bool DrawEndLine(Point Point)
		{
			if (!this.whiteSpace.Visible || (this.whiteSpace.EofSymbol == '\0'))
			{
				return false;
			}
			int num1 = this.ClientRect.Width;
			int num2 = this.Lines.Count;
			int num3 = ((River.Orqa.Editor.LineStyles) this.Source.LineStyles).GetLineStyle(num2);
			bool flag1 = this.lineSeparator.NeedHighlightLine(num2, false);
			LineStyle style1 = ((num3 >= 0) && (num3 < this.lineStyles.Count)) ? ((LineStyle) this.lineStyles.GetStyle(num3)) : null;
			Point.X += this.DrawTextFragment(Point, this.whiteSpace.EofString, 0x100, style1, false, this.displayLines.GetCount(), 0, 0, true, flag1);
			this.DrawAfterLineEnd(num1, Point, style1, false, 0, 0, 0, num2, flag1);
			return true;
		}

		private void DrawHighlighter(int ALeft, int ATop, int ARight, int ABottom, int Style, int Char, int Line, string Str, bool InSelection, bool AfterText)
		{
			if (this.lineSeparator.LineColor != Color.Empty)
			{
				this.painter.Color = this.lineSeparator.LineColor;
				Rectangle rectangle1 = new Rectangle(ALeft, ATop, ARight - ALeft, ABottom - ATop);
				DrawState state1 = DrawState.LineHighlight;
				if (!AfterText)
				{
					state1 |= DrawState.Text;
				}
				else
				{
					state1 |= DrawState.BeyondEol;
				}
				if (InSelection)
				{
					state1 |= DrawState.Selection;
				}
				this.drawInfo.Init();
				this.drawInfo.Text = Str;
				this.drawInfo.Line = Line;
				this.drawInfo.Char = Char;
				this.drawInfo.Style = (short) Style;
				if (!this.OnCustomDraw(this.painter, rectangle1, DrawStage.Before, state1, this.drawInfo))
				{
					this.painter.DrawLine(ALeft, ATop, ARight, ATop);
					this.painter.DrawLine(ALeft, ABottom, ARight, ABottom);
				}
				this.OnCustomDraw(this.painter, rectangle1, DrawStage.After, state1, this.drawInfo);
			}
		}

		public void DrawLine(int Index, Point Point, Rectangle ClipRect)
		{
			string text1 = string.Empty;
			short[] numArray1 = null;
			int num1 = this.displayLines.GetDisplayString(Index, ref text1, ref numArray1, true);
			int num2 = ClipRect.Right;
			int num3 = 0;
			int num4 = 0;
			bool flag1 = (this.drawSelection & !this.selection.IsEmpty()) && this.selection.GetSelectionForLine(Index, out num3, out num4);
			bool flag2 = false;
			int num5 = ((River.Orqa.Editor.LineStyles) this.Source.LineStyles).GetLineStyle(num1);
			LineStyle style1 = ((num5 >= 0) && (num5 < this.lineStyles.Count)) ? ((LineStyle) this.lineStyles.GetStyle(num5)) : null;
			int num6 = text1.Length;
			bool flag3 = this.lineSeparator.NeedHighlightLine(Index, true);
			int num7 = this.gutter.GetWidth() - Point.X;
			if (text1 != string.Empty)
			{
				int num8 = numArray1[0];
				int num9 = num8;
				int num10 = 0;
				flag2 = (flag1 && (num3 == 0)) && (num4 > 0);
				bool flag4 = flag2;
				for (int num11 = 1; num11 < num6; num11++)
				{
					num8 = numArray1[num11];
					flag4 = (flag1 && (num3 <= num11)) && (num4 > num11);
					if ((flag4 != flag2) || !this.EqualStyles(num9, num8, true))
					{
						Point.X += this.DrawTextFragment(Point, text1, num9, style1, flag2, Index, num10, num11 - 1, false, flag3);
						if (Point.X >= num2)
						{
							break;
						}
						num9 = num8;
						num10 = num11;
						flag2 = flag4;
					}
				}
				if ((num10 < num6) && (Point.X < num2))
				{
					Point.X += this.DrawTextFragment(Point, text1, num9, style1, flag2, Index, num10, num6 - 1, false, flag3);
				}
			}
			else
			{
				flag2 = (flag1 && ((SelectionOptions.SelectBeyondEol & this.selection.Options) != SelectionOptions.None)) || ((this.selection.SelectionType == SelectionType.Block) && ((flag1 && (num3 == 0)) && (num4 > 0)));
			}
			if (this.whiteSpace.Visible && (this.whiteSpace.EolSymbol != '\0'))
			{
				flag2 = (flag1 && (((SelectionOptions.SelectBeyondEol & this.selection.Options) != SelectionOptions.None) || (this.selection.SelectionType == SelectionType.Block))) && ((flag1 && (num3 <= num6)) && (num4 > num6));
				Point.X += this.DrawTextFragment(Point, this.whiteSpace.EolString, 0x100, style1, flag2, Index, 0, 0, true, flag3);
			}
			this.DrawAfterLineEnd(num2, Point, style1, flag1, num7, num3, num4, Index, flag3);
		}

		private int DrawTextFragment(Point Point, string String, int Style, LineStyle LineStyle, bool InSelection, int Line, int StartChar, int EndChar, bool SpecialSymbol, bool Highlight)
		{
			ColorFlags flags1 = ColorFlags.None;
			ILexStyle style1 = this.GetLexStyle(Style, ref flags1);
			if (this.disableSyntaxPaint)
			{
				style1 = null;
			}
			string text1 = ((StartChar == 0) && (EndChar == -1)) ? String : String.Substring(StartChar, (EndChar - StartChar) + 1);
			if (style1 != null)
			{
				this.painter.FontStyle = this.GetFontStyle(style1.FontStyle, flags1);
				if (InSelection)
				{
					this.painter.Color = this.GetSelectionForeColor(this.GetFontColor(style1.ForeColor, flags1));
					this.painter.BkColor = this.selBackColor;
				}
				else
				{
					if (Highlight && (this.lineSeparator.HighlightForeColor != Color.Empty))
					{
						this.painter.Color = this.lineSeparator.HighlightForeColor;
					}
					else
					{
						this.painter.Color = this.GetFontColor(style1.ForeColor, flags1);
					}
					if (Highlight && (this.lineSeparator.HighlightBackColor != Color.Empty))
					{
						this.painter.BkColor = this.lineSeparator.HighlightBackColor;
					}
					else if (style1.BackColor != Color.Empty)
					{
						this.painter.BkColor = style1.BackColor;
					}
					else
					{
						this.painter.BkColor = this.BackColor;
					}
				}
			}
			else
			{
				this.painter.FontStyle = this.GetFontStyle(this.Font.Style, flags1);
				if (InSelection)
				{
					this.painter.Color = this.selForeColor;
					this.painter.BkColor = this.selBackColor;
				}
				else
				{
					if (Highlight && (this.lineSeparator.HighlightForeColor != Color.Empty))
					{
						this.painter.Color = this.lineSeparator.HighlightForeColor;
					}
					else
					{
						this.painter.Color = this.GetFontColor(this.ForeColor, flags1);
					}
					if (Highlight && (this.lineSeparator.HighlightBackColor != Color.Empty))
					{
						this.painter.BkColor = this.lineSeparator.HighlightBackColor;
					}
					else
					{
						this.painter.BkColor = this.BackColor;
					}
				}
			}
			Color color1 = Color.Empty;
			if (!InSelection && (LineStyle != null))
			{
				this.painter.BkColor = LineStyle.GetBackColor(this.painter.BkColor);
				color1 = LineStyle.GetForeColor(this.painter.Color);
				this.painter.Color = color1;
			}
			int num1 = this.painter.StringWidth(text1);
			Rectangle rectangle1 = new Rectangle(Point.X, Point.Y, num1, this.painter.FontHeight);
			int num2 = -1;
			if (!SpecialSymbol && this.whiteSpace.Visible)
			{
				if ((this.whiteSpace.SpaceSymbol != '\0') && ((flags1 & (ColorFlags.None | ColorFlags.WhiteSpace)) != ColorFlags.None))
				{
					text1 = new string(this.whiteSpace.SpaceSymbol, text1.Length);
					num2 = this.painter.CharWidth(' ', 1);
				}
				else if ((this.whiteSpace.TabSymbol != '\0') && ((flags1 & (ColorFlags.None | ColorFlags.Tabulation)) != ColorFlags.None))
				{
					text1 = new string(this.whiteSpace.TabSymbol, text1.Length);
					num2 = this.painter.CharWidth(' ', 1);
				}
			}
			DrawState state1 = DrawState.Text;
			if (InSelection)
			{
				state1 |= DrawState.Selection;
			}
			if (SpecialSymbol)
			{
				state1 |= DrawState.WhiteSpace;
			}
			bool flag1 = ((this.transparent && !InSelection) && !Highlight) && (LineStyle == null);
			if (flag1)
			{
				this.painter.BkMode = 1;
			}
			this.drawInfo.Init();
			this.drawInfo.Text = text1;
			this.drawInfo.Style = (short) Style;
			this.drawInfo.Line = Line;
			this.drawInfo.Char = StartChar;
			if (!this.OnCustomDraw(this.painter, rectangle1, DrawStage.Before, state1, this.drawInfo))
			{
				this.painter.TextOut(text1, -1, rectangle1, (int) (4 | (this.transparent ? 0 : 2)), num2);
			}
			this.OnCustomDraw(this.painter, rectangle1, DrawStage.After, state1, this.drawInfo);
			if (flag1)
			{
				this.painter.BkMode = 2;
			}
			if (Highlight)
			{
				this.DrawHighlighter(rectangle1.Left, rectangle1.Top - 1, rectangle1.Right, rectangle1.Bottom - 1, Style, StartChar, Line, text1, InSelection, false);
			}
			if ((flags1 & ColorFlags.OutlineSection) != ColorFlags.None)
			{
				Color color2 = this.painter.BkColor;
				this.painter.BkColor = this.outlining.OutlineColor;
				if (!this.OnCustomDraw(this.painter, rectangle1, DrawStage.Before, state1 | DrawState.OutlineButton, this.drawInfo))
				{
					this.painter.DrawRectangle(rectangle1);
				}
				this.OnCustomDraw(this.painter, rectangle1, DrawStage.After, state1 | DrawState.OutlineButton, this.drawInfo);
				this.painter.BkColor = color2;
			}
			if (((this.braces.UseRoundRect && ((flags1 & ColorFlags.Brace) != ColorFlags.None)) && (((flags1 & (ColorFlags.None | ColorFlags.Tabulation)) == ColorFlags.None) && ((flags1 & (ColorFlags.None | ColorFlags.WhiteSpace)) == ColorFlags.None))) && (text1.Trim() != string.Empty))
			{
				Color color3 = this.painter.BkColor;
				this.painter.BkColor = this.braces.BracesColor;
				if (!this.OnCustomDraw(this.painter, rectangle1, DrawStage.Before, state1 | DrawState.Brace, this.drawInfo))
				{
					this.painter.DrawRectangle(rectangle1);
				}
				this.OnCustomDraw(this.painter, rectangle1, DrawStage.After, state1 | DrawState.Brace, this.drawInfo);
				this.painter.BkColor = color3;
			}
			if (this.spelling.CheckSpelling && ((flags1 & ColorFlags.MisSpelledWord) != ColorFlags.None))
			{
				if (!this.OnCustomDraw(this.painter, rectangle1, DrawStage.Before, state1 | DrawState.Spelling, this.drawInfo))
				{
					this.painter.DrawLiveSpell(rectangle1, (color1 != Color.Empty) ? color1 : this.spelling.SpellColor);
				}
				this.OnCustomDraw(this.painter, rectangle1, DrawStage.After, state1 | DrawState.Spelling, this.drawInfo);
			}
			return num1;
		}

		private void EndLineEndUpdate()
		{
			this.displayLines.LineEndUpdateCount--;
		}

		private void EnsureLastLineParsed()
		{
			int num1;
			if (this.pages.PageType == PageType.PageLayout)
			{
				num1 = this.pages.GetPageAtPoint(0, this.ClientHeight()).EndLine;
			}
			else
			{
				num1 = (this.scrolling.WindowOriginY + this.LinesInHeight()) + 1;
			}
			Point point1 = this.displayLines.DisplayPointToPoint(0, num1);
			num1 = point1.Y;
			this.Source.ParseToString(num1 + EditConsts.DefaultParserDelta);
		}

		protected internal bool EqualStyles(int Style1, int Style2, bool UseColors)
		{
			ColorFlags flags1 = ColorFlags.None;
			ColorFlags flags2 = ColorFlags.None;
			ILexStyle style1 = this.GetLexStyle(Style1, ref flags1);
			ILexStyle style2 = this.GetLexStyle(Style2, ref flags2);
			if (this.disableSyntaxPaint)
			{
				style1 = null;
				style2 = null;
			}
			if (flags1 == flags2)
			{
				if (style1 == style2)
				{
					return true;
				}
				if (((style1 != null) && (style2 != null)) && (style1.FontStyle == style2.FontStyle))
				{
					if (!UseColors || this.disableColorPaint)
					{
						return true;
					}
					if (style1.ForeColor == style2.ForeColor)
					{
						return (style1.BackColor == style2.BackColor);
					}
					return false;
				}
			}
			return false;
		}

		private void FillRectangleAfterLineEnd(int X, int Y, int W, int H, int Line, bool InSelection, bool Highlight)
		{
			Rectangle rectangle1 = new Rectangle(X, Y, W, H);
			DrawState state1 = DrawState.BeyondEol;
			if (InSelection)
			{
				state1 |= DrawState.Selection;
			}
			this.drawInfo.Init();
			this.drawInfo.Line = Line;
			if (!this.OnCustomDraw(this.painter, rectangle1, DrawStage.Before, state1, this.drawInfo))
			{
				this.painter.FillRectangle(X, Y, W, H);
			}
			this.OnCustomDraw(this.painter, rectangle1, DrawStage.After, state1, this.drawInfo);
			if (Highlight)
			{
				this.DrawHighlighter(X, Y - 1, X + W, (Y + H) - 1, -1, 0, Line, string.Empty, InSelection, true);
			}
		}

		~SyntaxEdit()
		{
			if (this.codeCompletionTimer != null)
			{
				this.codeCompletionTimer.Dispose();
			}
			this.Source = null;
		}

		public bool Find(string String)
		{
			return this.Find(String, SearchOptions.None, null);
		}

		public bool Find(string String, SearchOptions Options)
		{
			return this.Find(String, Options, null);
		}

		public bool Find(string String, SearchOptions Options, Regex Expression)
		{
			this.FirstSearch = true;
			return this.DoFind(String, Options, Expression, false);
		}

		public bool Find(string String, SearchOptions Options, Regex Expression, ref Point Position, out int Len)
		{
			return this.Source.Lines.Find(String, Options, Expression, ref Position, out Len);
		}

		public bool FindNext()
		{
			if (!this.firstSearch)
			{
				return this.DoFind(this.searchText, (this.searchOptions & ((SearchOptions) (-17))) & ((SearchOptions) (-9)), this.searchExpression, false);
			}
			return false;
		}

		public bool FindNextSelected()
		{
			if (!this.firstSearch && !this.selection.IsEmpty())
			{
				return this.DoFind(this.selection.SelectedText, (this.searchOptions & ((SearchOptions) (-17))) & ((SearchOptions) (-9)), this.searchExpression, false);
			}
			return false;
		}

		public bool FindPrevious()
		{
			if (!this.firstSearch)
			{
				return this.DoFind(this.searchText, (this.searchOptions | SearchOptions.BackwardSearch) & ((SearchOptions) (-17)), this.searchExpression, false);
			}
			return false;
		}

		public bool FindPreviousSelected()
		{
			if (!this.firstSearch && !this.selection.IsEmpty())
			{
				return this.DoFind(this.searchText, (this.searchOptions | SearchOptions.BackwardSearch) & ((SearchOptions) (-17)), this.searchExpression, false);
			}
			return false;
		}

		public void FinishIncrementalSearch()
		{
			if (this.inIncrementalSearch)
			{
				this.searchText = string.Empty;
				this.inIncrementalSearch = false;
				this.Source.BeginUpdate(UpdateReason.Other);
				try
				{
					ITextSource source1 = this.Source;
					source1.State |= NotifyState.IncrementalSearchChanged;
				}
				finally
				{
					this.Source.EndUpdate();
				}
				if (base.IsHandleCreated)
				{
					Win32.SendMessage(base.Handle, 0x20, IntPtr.Zero, IntPtr.Zero);
				}
			}
		}

		protected internal void FinishVerticalNavigate()
		{
			this.vertNavigate = false;
		}

		protected internal Brush GetBrush()
		{
			return this.wndBrush;
		}

		public Size GetCaretSize(Point Position)
		{
			if (this.Source.OverWrite)
			{
				return new Size(this.painter.FontWidth, this.painter.FontHeight);
			}
			return new Size(EditConsts.DefaultCaretWidth, this.painter.FontHeight);
		}

		protected internal Rectangle GetClientRect(bool ExcludeRulers)
		{
			Rectangle rectangle1 = base.ClientRectangle;
			if (!ExcludeRulers || (this.pages.PageType != PageType.PageLayout))
			{
				if ((this.pages.Rulers & EditRulers.Horizonal) != EditRulers.None)
				{
					int num1 = this.pages.HorzRuler.Height;
					rectangle1.Y += num1;
					rectangle1.Height -= num1;
				}
				if ((this.pages.Rulers & EditRulers.Vertical) != EditRulers.None)
				{
					int num2 = this.pages.VertRuler.Width;
					rectangle1.X += num2;
					rectangle1.Width -= num2;
				}
			}
			return rectangle1;
		}

		protected internal Color GetFontColor(Color AColor, ColorFlags State)
		{
			if (this.disableColorPaint)
			{
				return this.ForeColor;
			}
			if (((State & ColorFlags.HyperText) != ColorFlags.None) && (this.hyperText.UrlColor != Color.Empty))
			{
				return this.hyperText.UrlColor;
			}
			if ((((State & (ColorFlags.None | ColorFlags.WhiteSpace)) != ColorFlags.None) || ((State & (ColorFlags.None | ColorFlags.Tabulation)) != ColorFlags.None)) && (this.whiteSpace.SymbolColor != Color.Empty))
			{
				return this.whiteSpace.SymbolColor;
			}
			if ((State & ColorFlags.OutlineSection) != ColorFlags.None)
			{
				return this.outlining.OutlineColor;
			}
			if (((State & ColorFlags.Brace) != ColorFlags.None) && !this.braces.UseRoundRect)
			{
				return this.braces.BracesColor;
			}
			return AColor;
		}

		protected internal FontStyle GetFontStyle(FontStyle Style, ColorFlags State)
		{
			if ((State & ColorFlags.HyperText) != ColorFlags.None)
			{
				return (Style | this.hyperText.UrlStyle);
			}
			if (((State & ColorFlags.Brace) != ColorFlags.None) && !this.braces.UseRoundRect)
			{
				return (Style | this.braces.BracesStyle);
			}
			return Style;
		}

		public void GetHitTest(Point Point, ref River.Orqa.Editor.HitTestInfo HitTestInfo)
		{
			this.GetHitTest(Point.X, Point.Y, ref HitTestInfo);
		}

		public void GetHitTest(int X, int Y, ref River.Orqa.Editor.HitTestInfo HitTestInfo)
		{
			int num1;
			IOutlineRange range1;
			HitTestInfo.InitHitTestInfo();
			Rectangle rectangle1 = this.ClientRect;
			if (X < rectangle1.Left)
			{
				HitTestInfo.HitTest |= HitTest.Left;
			}
			if (X > rectangle1.Right)
			{
				HitTestInfo.HitTest |= HitTest.Right;
			}
			if (Y < rectangle1.Top)
			{
				HitTestInfo.HitTest |= HitTest.Above;
			}
			if (Y > rectangle1.Bottom)
			{
				HitTestInfo.HitTest |= HitTest.Below;
			}
			if (this.gutter.IsMouseOnOutlineArea(X, Y))
			{
				HitTestInfo.HitTest |= HitTest.OutlineArea;
			}
			if (this.gutter.IsMouseOnOutlineImage(X, Y))
			{
				HitTestInfo.HitTest |= HitTest.OutlineImage;
			}
			if (this.gutter.IsMouseOnGutterImage(X, Y, out num1))
			{
				HitTestInfo.HitTest |= HitTest.GutterImage;
				HitTestInfo.GutterImage = num1;
			}
			if (this.pages.PageType != PageType.Normal)
			{
				HitTestInfo.Page = this.Pages.GetPageIndexAtPoint(X, Y);
				if (HitTestInfo.Page >= 0)
				{
					HitTestInfo.HitTest |= HitTest.Page;
					if (((EditPage) this.Pages[HitTestInfo.Page]).WhiteSpaceRect.Contains(X, Y))
					{
						HitTestInfo.HitTest |= HitTest.PageWhiteSpace;
					}
					if (this.margin.Visible && this.margin.Contains(X, Y))
					{
						HitTestInfo.HitTest |= HitTest.Margin;
					}
				}
			}
			else if (this.margin.Visible && this.margin.Contains(X, Y))
			{
				HitTestInfo.HitTest |= HitTest.Margin;
			}
			if (this.outlining.IsMouseOnOutlineButton(X, Y, out range1))
			{
				HitTestInfo.HitTest |= HitTest.OutlineButton;
				HitTestInfo.OutlineRange = range1;
			}
			if (this.gutter.IsMouseOnGutter(X, Y))
			{
				HitTestInfo.HitTest |= HitTest.Gutter;
			}
			else
			{
				this.GetHitTestAtTextPoint(this.ScreenToText(X, Y), ref HitTestInfo);
			}
		}

		public void GetHitTestAtTextPoint(Point Point, ref River.Orqa.Editor.HitTestInfo HitTestInfo)
		{
			this.GetHitTestAtTextPoint(Point.X, Point.Y, ref HitTestInfo);
		}

		public void GetHitTestAtTextPoint(int X, int Y, ref River.Orqa.Editor.HitTestInfo HitTestInfo)
		{
			if ((Y >= 0) && (Y < this.Lines.Count))
			{
				HitTestInfo.Item = this.Lines.GetItem(Y);
				HitTestInfo.String = this.Lines[Y];
				if ((X >= 0) && (X < HitTestInfo.String.Length))
				{
					string text1;
					HitTestInfo.HitTest |= HitTest.Text;
					HitTestInfo.Word = this.Lines.GetTextAt(X, Y);
					if (this.hyperText.IsUrlAtTextPoint(X, Y, out text1, true))
					{
						HitTestInfo.HitTest |= HitTest.HyperText;
						HitTestInfo.Url = text1;
					}
				}
				else
				{
					HitTestInfo.HitTest |= HitTest.BeyondEol;
				}
			}
			else
			{
				HitTestInfo.HitTest |= HitTest.BeyondEof;
			}
			if (this.selection.IsPosInSelection(X, Y))
			{
				HitTestInfo.HitTest |= HitTest.Selection;
			}
		}

		protected internal ILexStyle GetLexStyle(int Style, ref ColorFlags State)
		{
			if (Style >= 0)
			{
				byte num1 = (byte) Style;
				State = (ColorFlags) ((byte) (Style >> 8));
				if ((num1 != 0) && (this.Source.Lexer != null))
				{
					return this.Source.Lexer.Scheme.GetLexStyle(num1 - 1);
				}
			}
			return null;
		}

		private ICodeCompletionProvider GetQuickInfo(string Text, int bStart, int bEnd)
		{
			IQuickInfo info1 = new River.Orqa.Editor.Syntax.QuickInfo();
			info1.Text = Text;
			info1.BoldStart = bStart;
			info1.BoldEnd = bEnd;
			return info1;
		}

		protected internal Region GetRectRegion(Rectangle Rect)
		{
			return this.GetRectRegion(SelectionType.Stream, Rect, false, false);
		}

		protected internal Region GetRectRegion(SelectionType SelectionType, Rectangle Rect, bool AtTopLeftEnd, bool AtBottomRightEnd)
		{
			Region region1 = null;
			if (SelectionType != SelectionType.None)
			{
				int num1 = this.painter.FontHeight;
				Point point1 = this.TextToScreen(Rect.Location, AtTopLeftEnd);
				Point point2 = this.TextToScreen(Rect.Location + Rect.Size, AtBottomRightEnd);
				if ((point1.Y == point2.Y) || (SelectionType == SelectionType.Block))
				{
					int num2 = Math.Min(point1.X, point2.X);
					int num3 = Math.Max(point1.X, point2.X);
					return new Region(new Rectangle(num2, point1.Y, (num3 - num2) + 1, (point2.Y - point1.Y) + num1));
				}
				int num4 = point2.Y - point1.Y;
				if (this.pages.PageType == PageType.PageLayout)
				{
					IEditPage page1 = this.pages.GetPageAtPoint(point1);
					IEditPage page2 = this.pages.GetPageAtPoint(point2);
					Rectangle rectangle1 = page1.ClientRect;
					Rectangle rectangle2 = page2.ClientRect;
					region1 = new Region(new Rectangle(point1.X, point1.Y, (rectangle1.Right - point1.X) + 1, num1));
					int num5 = Math.Min(rectangle1.Left, rectangle2.Left);
					region1.Union(new Rectangle(num5, point1.Y + num1, Math.Max(rectangle1.Right, rectangle2.Right) - num5, num4 - num1));
					region1.Union(new Rectangle(rectangle2.Left, point2.Y, (point2.X - rectangle2.Left) + 1, num1));
					return region1;
				}
				int num6 = this.gutter.GetWidth() + this.ClientRect.Left;
				int num7 = this.ClientRect.Right;
				region1 = new Region(new Rectangle(point1.X, point1.Y, (num7 - point1.X) + 1, num1));
				region1.Union(new Rectangle(num6, point1.Y + num1, (num7 - num6) + 1, num4 - num1));
				region1.Union(new Rectangle(num6, point2.Y, (point2.X - num6) + 1, num1));
			}
			return region1;
		}

		private Color GetSelectionForeColor(Color Color)
		{
			if ((SelectionOptions.UseColors & this.selection.Options) != SelectionOptions.None)
			{
				return Color;
			}
			return this.selForeColor;
		}

		public string GetTextAtCursor()
		{
			return this.Lines.GetTextAt(this.Position);
		}

		public int GetWrapMargin()
		{
			return this.displayLines.GetWrapMargin();
		}

		public void HideScrollHint()
		{
			if ((this.codeCompletionHint != null) && this.codeCompletionHint.Visible)
			{
				this.codeCompletionHint.Close(false);
			}
		}

		public bool IncrementalSearch(string Key, bool DeleteLast)
		{
			bool flag1 = false;
			string text1 = this.searchText;
			int num1 = 0;
			this.incrSearchFlag = true;
			try
			{
				if (DeleteLast)
				{
					this.searchText = this.searchText.Remove(this.searchText.Length - 1, 1);
				}
				else
				{
					this.searchText = this.searchText + Key;
				}
				this.searchPos = this.incrSearchPosition;
				if (this.Find(this.searchText, this.searchOptions, this.searchExpression, ref this.searchPos, out num1))
				{
					flag1 = true;
					this.Position = this.searchPos;
					this.selection.SetSelection(SelectionType.Stream, new Rectangle(this.Position.X, this.Position.Y, num1, 0));
					return flag1;
				}
				if (DeleteLast)
				{
					this.selection.SetSelection(SelectionType.Stream, new Rectangle(this.Position.X, this.Position.Y, 0, 0));
					return flag1;
				}
				this.searchText = text1;
			}
			finally
			{
				this.incrSearchFlag = false;
			}
			return flag1;
		}

		private Cursor IncrementalSearchCursor()
		{
			if (this.incrementalSearchCursor == null)
			{
				this.incrementalSearchCursor = new Cursor(typeof(SyntaxEdit), "Images.IncrementalSearch.cur");
			}
			return this.incrementalSearchCursor;
		}

		private void InitializeComponent()
		{
			this.components = new Container();
		}

		protected void InsertTextFromProvider(string Text, Point StartPos)
		{
			this.Source.BeginUpdate(UpdateReason.Other);
			try
			{
				this.MoveTo(StartPos);
				this.selection.SelectWord();
				this.selection.SetSelection(this.Selection.SelectionType, StartPos, new Point(this.Selection.SelectionRect.Right, this.Selection.SelectionRect.Bottom));
				string[] textArray1 = StrItem.Split(Text);
				Point point1 = new Point(StartPos.X + Text.Length, this.Position.Y);
				int num1 = textArray1[0].IndexOf(EditConsts.DefaultCaretSymbol);
				if (num1 >= 0)
				{
					point1.X = StartPos.X + num1;
					textArray1[0] = textArray1[0].Remove(num1, 1);
				}
				string text1 = (textArray1.Length > 1) ? ((SyntaxStrings) this.Lines).GetIndentString(this.Lines.TabPosToPos(this.Lines[this.Position.Y], StartPos.X), 0) : string.Empty;
				for (int num2 = 1; num2 < textArray1.Length; num2++)
				{
					textArray1[num2] = text1 + textArray1[num2];
					num1 = textArray1[num2].IndexOf(EditConsts.DefaultCaretSymbol);
					if (num1 >= 0)
					{
						textArray1[num2] = textArray1[num2].Remove(num1, 1);
						point1.X = num1;
						point1.Y = this.Position.Y + num2;
					}
				}
				this.Selection.Delete();
				this.Source.InsertBlock(textArray1);
				this.MoveTo(point1);
				this.selection.Clear();
			}
			finally
			{
				this.Source.EndUpdate();
			}
		}

		protected internal void InternalProcessKey(char Char)
		{
			if (this.IsValidText(this.Position))
			{
				this.ProcessCodeCompletion(Char);
				if (((this.Lexer != null) && (this.Lexer is IFormatText)) && ((IFormatText) this.Lexer).IsBlockEnd(Char.ToString()))
				{
					this.Selection.SmartFormatBlock();
				}
			}
		}

		private void InvalidateLine(int Index)
		{
			if (Index >= 0)
			{
				Point point1 = this.DisplayToScreen(0, Index);
				int num1 = this.gutter.GetWidth();
				int num2 = this.ClientRect.Right;
				base.Invalidate(new Rectangle(num1, point1.Y - 1, (num2 - num1) + 1, this.painter.FontHeight + 2));
			}
		}

		private void InvalidateLines(int Char, int Line)
		{
			if (base.IsHandleCreated && ((Line != this.scrolling.WindowOriginY) || (Char != this.scrolling.WindowOriginX)))
			{
				if (((Char != this.scrolling.WindowOriginX) && (Line != this.scrolling.WindowOriginY)) || (((this.Selection.SelectionType != SelectionType.None) || this.transparent) || this.lineSeparator.NeedHighlight()))
				{
					base.Invalidate();
				}
				else
				{
					int num1 = 0;
					int num2 = 0;
					Rectangle rectangle1 = this.ClientRect;
					int num3 = rectangle1.Width;
					int num4 = rectangle1.Height;
					if (Char == this.scrolling.WindowOriginX)
					{
						if (this.pages.PageType == PageType.PageLayout)
						{
							num2 = Line - this.scrolling.WindowOriginY;
						}
						else
						{
							num2 = (Line - this.scrolling.WindowOriginY) * this.painter.FontHeight;
						}
					}
					else if (Line == this.scrolling.WindowOriginY)
					{
						if (this.pages.PageType == PageType.PageLayout)
						{
							num1 = Char - this.scrolling.WindowOriginX;
						}
						else
						{
							num1 = (Char - this.scrolling.WindowOriginX) * this.painter.FontWidth;
						}
					}
					if ((Math.Abs(num1) < (num3 / 2)) && (Math.Abs(num2) < (num4 / 2)))
					{
						Rectangle rectangle2;
						Rectangle rectangle3;
						int num5 = rectangle1.Left + ((this.pages.PageType == PageType.PageLayout) ? 0 : this.gutter.GetWidth());
						if (num2 == 0)
						{
							if (num1 > 0)
							{
								rectangle2 = new Rectangle(num5, rectangle1.Top, num3 - num1, num4);
								rectangle3 = new Rectangle(num5, rectangle1.Top, num1, num4);
							}
							else
							{
								rectangle2 = new Rectangle(num5 - num1, rectangle1.Top, num3 + num1, num4);
								rectangle3 = new Rectangle(num3 + num1, rectangle1.Top, -num1, num4);
							}
						}
						else if (num2 > 0)
						{
							rectangle2 = new Rectangle(rectangle1.Left, rectangle1.Top, num3, num4 - num2);
							rectangle3 = new Rectangle(rectangle1.Left, rectangle1.Top, num3, num2 - rectangle1.Top);
						}
						else
						{
							rectangle2 = new Rectangle(rectangle1.Left, rectangle1.Top - num2, num3, num4 + num2);
							rectangle3 = new Rectangle(rectangle1.Left, num4 + num2, num3, -num2);
						}
						Win32.ScrollWindow(base.Handle, num1, num2, rectangle2);
						base.Invalidate(rectangle3, false);
						base.Update();
					}
					else
					{
						base.Invalidate();
					}
				}
			}
		}

		private void InvalidateWindow(int First, int Last, bool InvalidateGutter)
		{
			Point point2;
			Point point1 = this.TextToScreen(new Point(0, First), false);
			if (Last == 0x7fffffff)
			{
				point2 = new Point(this.ClientRect.Right, this.ClientRect.Bottom);
			}
			else
			{
				point2 = this.TextToScreen(new Point(0x7fffffff, Last), true);
				point2.Y += this.painter.FontHeight;
			}
			if (this.Pages.PageType == PageType.PageLayout)
			{
				point1.X = this.Pages.GetPageAtPoint(point1).BoundsRect.Left;
				point2.X = this.Pages.GetPageAtPoint(point2).BoundsRect.Right;
			}
			else if (InvalidateGutter && (this.gutter.GetWidth() > 0))
			{
				point1.X = this.ClientRect.Left;
				point2.X = this.ClientRect.Right;
			}
			point2.X = base.ClientRectangle.Right;
			base.Invalidate(new Rectangle(point1.X, point1.Y, (point2.X - point1.X) + 1, (point2.Y - point1.Y) + 1));
		}

		private bool IsCodeCompletionWindowFocused()
		{
			if ((this.codeCompletionBox != null) && this.codeCompletionBox.IsFocused())
			{
				return true;
			}
			if (this.codeCompletionHint != null)
			{
				return this.codeCompletionHint.IsFocused();
			}
			return false;
		}

		private bool IsFocused()
		{
			if (!this.Focused)
			{
				return this.IsCodeCompletionWindowFocused();
			}
			return true;
		}

		protected override bool IsInputChar(char charCode)
		{
			return (charCode != '\t');
		}

		protected override bool IsInputKey(Keys keyData)
		{
			Keys keys1 = keyData & Keys.KeyCode;
			if ((keys1 == Keys.Return) && !this.WantReturns)
			{
				return false;
			}
			if ((keys1 == Keys.Tab) && !this.WantTabs)
			{
				return false;
			}
			if (Array.IndexOf(EditConsts.NavKeys, keys1) >= 0)
			{
				return true;
			}
			return base.IsInputKey(keyData);
		}

		public bool IsValidText(Point Position)
		{
			if (this.Lexer != null)
			{
				StrItem item1 = this.Lines.GetItem(Position.Y);
				if (item1 != null)
				{
					if (Position.X == item1.String.Length)
					{
						Position.X--;
					}
					if ((Position.X >= 0) && (Position.X < item1.ColorData.Length))
					{
						return !this.Lexer.Scheme.IsPlainText(((byte) item1.ColorData[Position.X]) - 1);
					}
				}
			}
			return true;
		}

		private Cursor LeftArrowCursor()
		{
			if (this.leftArrowCursor == null)
			{
				this.leftArrowCursor = new Cursor(typeof(SyntaxEdit), "Images.LeftArrow.cur");
			}
			return this.leftArrowCursor;
		}

		public int LinesInHeight()
		{
			return this.LinesInHeight(this.ClientHeight());
		}

		public int LinesInHeight(int Height)
		{
			int num1 = this.painter.FontHeight;
			if (num1 == 0)
			{
				return 0;
			}
			return (Height / num1);
		}

		public void ListMembers()
		{
			if (this.IsValidText(this.Position))
			{
				this.ListMembers(this.codeCompletionArgs, CodeCompletionType.ListMembers);
				this.OnNeedCompletion(this.codeCompletionArgs);
			}
		}

		protected void ListMembers(CodeCompletionArgs e)
		{
			this.ListMembers(e, CodeCompletionType.ListMembers);
		}

		protected virtual void ListMembers(CodeCompletionArgs e, CodeCompletionType CompletionType)
		{
			e.Init(CompletionType, this.Position);
			if (((TextSource) this.Source).NeedCodeCompletion())
			{
				IFormatText text1 = this.Lexer as IFormatText;
				text1.CodeCompletion(this.RemovePlainText(this.Position.Y), this.Position, e);
			}
		}

		public void LoadFile(string FileName)
		{
			this.displayLines.LoadFile(FileName);
		}

		public void LoadFile(string FileName, ExportFormat Format)
		{
			this.LoadFile(FileName, Format, null);
		}

		public void LoadFile(string FileName, Encoding Encoding)
		{
			this.LoadFile(FileName, ExportFormat.Text, Encoding);
		}

		public void LoadFile(string FileName, ExportFormat Format, Encoding Encoding)
		{
			this.displayLines.LoadFile(FileName, Format, Encoding);
		}

		public void LoadStream(TextReader Reader)
		{
			this.displayLines.LoadStream(Reader);
		}

		public void LoadStream(TextReader Reader, ExportFormat Format)
		{
			this.displayLines.LoadStream(Reader, Format);
		}

		public void MakeVisible(Point Position)
		{
			this.MakeVisible(Position, false);
		}

		public void MakeVisible(Point Position, bool CenterLine)
		{
			Size size1;
			this.Outlining.EnsureExpanded(Position);
			Point point1 = this.TextToScreen(Position);
			if (this.pages.PageType == PageType.PageLayout)
			{
				Rectangle rectangle1 = this.ClientRect;
				if (point1.X < rectangle1.Left)
				{
					this.scrolling.WindowOriginX += (point1.X - rectangle1.Left);
				}
				else
				{
					size1 = this.GetCaretSize(Position);
					int num1 = size1.Width;
					if (point1.X > (rectangle1.Right - num1))
					{
						this.scrolling.WindowOriginX += Math.Max((int) ((point1.X - rectangle1.Right) + num1), 1);
					}
				}
				if (point1.Y < rectangle1.Top)
				{
					if (CenterLine)
					{
						this.scrolling.WindowOriginY += (point1.Y - (rectangle1.Height / 2));
					}
					else
					{
						this.scrolling.WindowOriginY += (point1.Y - rectangle1.Top);
					}
				}
				else if (point1.Y > (rectangle1.Height - this.painter.FontHeight))
				{
					if (CenterLine)
					{
						this.scrolling.WindowOriginY += (point1.Y - ((rectangle1.Height - this.painter.FontHeight) / 2));
					}
					else
					{
						this.scrolling.WindowOriginY += ((point1.Y - rectangle1.Height) + this.painter.FontHeight);
					}
				}
			}
			else
			{
				Position = this.displayLines.PointToDisplayPoint(Position);
				int num2 = Position.X - this.scrolling.WindowOriginX;
				int num3 = Position.Y - this.scrolling.WindowOriginY;
				if (num2 < 0)
				{
					this.scrolling.WindowOriginX = Position.X;
				}
				else
				{
					size1 = this.GetCaretSize(Position);
					int num4 = size1.Width;
					if (point1.X > (base.ClientRectangle.Right - num4))
					{
						this.scrolling.WindowOriginX += Math.Max(this.CharsInWidth((point1.X - base.ClientRectangle.Right) + num4, false), 1);
					}
				}
				if (num3 < 0)
				{
					if (CenterLine)
					{
						this.scrolling.WindowOriginY += this.LinesInHeight(point1.Y - (this.ClientHeight() / 2));
					}
					else
					{
						this.scrolling.WindowOriginY = Position.Y;
					}
				}
				else if (num3 > (this.LinesInHeight() - 1))
				{
					if (CenterLine)
					{
						this.scrolling.WindowOriginY += this.LinesInHeight(point1.Y - (this.ClientHeight() / 2));
					}
					else
					{
						this.scrolling.WindowOriginY += ((num3 - this.LinesInHeight()) + 1);
					}
				}
			}
		}

		public int MarkAll(string String)
		{
			return this.DoMarkAll(String, SearchOptions.None, null);
		}

		public int MarkAll(string String, SearchOptions Options)
		{
			return this.DoMarkAll(String, Options, null);
		}

		public int MarkAll(string String, SearchOptions Options, Regex Expression)
		{
			return this.DoMarkAll(String, Options, Expression);
		}

		public int MeasureLine(int Index, int Pos, int Len)
		{
			int num1;
			string text1 = string.Empty;
			short[] numArray1 = null;
			this.displayLines.GetDisplayString(Index, ref text1, ref numArray1, this.Source.Lexer != null);
			return this.MeasureLine(text1, ref numArray1, Pos, Len, -1, out num1, false);
		}

		public int MeasureLine(string Line, ref short[] ColorData, int Pos, int Len)
		{
			int num1;
			return this.MeasureLine(Line, ref ColorData, Pos, Len, -1, out num1, false);
		}

		public int MeasureLine(int Index, int Pos, int Len, int Width, out int Chars)
		{
			string text1 = string.Empty;
			short[] numArray1 = null;
			this.displayLines.GetDisplayString(Index, ref text1, ref numArray1, this.Source.Lexer != null);
			return this.MeasureLine(text1, ref numArray1, Pos, Len, Width, out Chars, true);
		}

		public int MeasureLine(string Line, ref short[] ColorData, int Pos, int Len, int Width, out int Chars)
		{
			return this.MeasureLine(Line, ref ColorData, Pos, Len, Width, out Chars, true);
		}

		private int MeasureLine(string Line, ref short[] ColorData, int Pos, int Len, int Width, out int Chars, bool measureChars)
		{
			Chars = 0;
			if (measureChars)
			{
				if (Width == 0x7fffffff)
				{
					Chars = 0x7fffffff;
					return 0x7fffffff;
				}
				if (Len == 0x7fffffff)
				{
					return 0x7fffffff;
				}
			}
			else if (Len == 0x7fffffff)
			{
				return 0x7fffffff;
			}
			if (Line == string.Empty)
			{
				if (!measureChars)
				{
					return this.painter.CharWidth(' ', Len);
				}
				return this.painter.CharWidth(' ', Width, out Chars);
			}
			int num1 = 0;
			int num2 = 0;
			int num3 = (Len < 0) ? (Line.Length - Pos) : Math.Min(Len, (int) (Line.Length - Pos));
			if ((this.Source.Lexer == null) || this.painter.IsMonoSpaced)
			{
				if (!measureChars)
				{
					if (num3 >= 0)
					{
						num1 = this.painter.StringWidth(Line, 0, num3);
					}
					else
					{
						num1 = this.painter.StringWidth(Line);
					}
					if (Len > num3)
					{
						num1 += this.painter.CharWidth(' ', Len - num3);
					}
					return num1;
				}
				num1 = this.painter.StringWidth(Line, Width, out Chars, true);
			}
			else
			{
				int num4 = ColorData[0];
				int num5 = num4;
				int num6 = Pos;
				for (int num7 = Pos + 1; num7 < (Pos + num3); num7++)
				{
					num4 = ColorData[num7];
					if ((num4 != num5) && !this.EqualStyles(num5, num4, false))
					{
						num1 += this.MeasureTextFragment(Line, num5, num6, num7 - 1, Width - num1, out num2, measureChars);
						Chars += num2;
						num5 = num4;
						if (measureChars && ((Width < num1) || (num2 < (num7 - num6))))
						{
							Width = -1;
							break;
						}
						num6 = num7;
					}
				}
				if ((num6 < (Pos + num3)) && ((measureChars && (Width > num1)) || !measureChars))
				{
					num1 += this.MeasureTextFragment(Line, num5, num6, (Pos + num3) - 1, Width - num1, out num2, measureChars);
					Chars += num2;
				}
				if (!measureChars && (Len > num3))
				{
					ColorFlags flags1 = ColorFlags.None;
					ILexStyle style1 = this.GetLexStyle(num5, ref flags1);
					if (style1 != null)
					{
						this.painter.FontStyle = this.GetFontStyle(style1.FontStyle, flags1);
						num1 += this.painter.CharWidth(' ', Len - num3);
					}
					else
					{
						this.painter.FontStyle = this.GetFontStyle(this.Font.Style, flags1);
						num1 += this.painter.CharWidth(' ', Len - num3);
					}
				}
			}
			if (measureChars && (Width > num1))
			{
				num1 += this.painter.CharWidth(' ', (int) (Width - num1), out num2);
				Chars += num2;
			}
			return num1;
		}

		private int MeasureTextFragment(string String, int Style, int StartChar, int EndChar, int Width, out int Chars, bool measureChars)
		{
			ColorFlags flags1 = ColorFlags.None;
			ILexStyle style1 = this.GetLexStyle(Style, ref flags1);
			Chars = 0;
			if ((style1 != null) && !this.disableSyntaxPaint)
			{
				this.painter.FontStyle = this.GetFontStyle(style1.FontStyle, flags1);
			}
			else
			{
				this.painter.FontStyle = this.GetFontStyle(this.Font.Style, flags1);
			}
			if (measureChars)
			{
				return this.painter.StringWidth(String, StartChar, (EndChar - StartChar) + 1, Width, out Chars);
			}
			return this.painter.StringWidth(String, StartChar, (EndChar - StartChar) + 1);
		}

		public void MoveCharLeft()
		{
			bool flag1 = false;
			Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
			if ((point1.X == 0) && ((River.Orqa.Editor.NavigateOptions.UpAtLineBegin & this.NavigateOptions) != River.Orqa.Editor.NavigateOptions.None))
			{
				point1.X = this.displayLines[point1.Y - 1].Length;
				point1.Y--;
				this.displayLines.AtLineEnd = true;
				flag1 = true;
			}
			else if (point1.X > 0)
			{
				point1.X--;
			}
			if (flag1)
			{
				this.BeginLineEndUpdate();
				this.Source.BeginUpdate(UpdateReason.Navigate);
				try
				{
					ITextSource source1 = this.Source;
					source1.State |= NotifyState.PositionChanged;
					this.MoveTo(this.displayLines.DisplayPointToPoint(point1.X, point1.Y, true, true, false));
					return;
				}
				finally
				{
					this.Source.EndUpdate();
					this.EndLineEndUpdate();
				}
			}
			this.MoveTo(this.displayLines.DisplayPointToPoint(point1.X, point1.Y, true, true, false));
		}

		public void MoveCharRight()
		{
			bool flag1 = false;
			Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
			if ((point1.X >= this.displayLines[point1.Y].Length) && ((this.NavigateOptions & River.Orqa.Editor.NavigateOptions.BeyondEol) == River.Orqa.Editor.NavigateOptions.None))
			{
				if (((this.NavigateOptions & River.Orqa.Editor.NavigateOptions.DownAtLineEnd) != River.Orqa.Editor.NavigateOptions.None) && (((this.NavigateOptions & River.Orqa.Editor.NavigateOptions.BeyondEof) != River.Orqa.Editor.NavigateOptions.None) || (point1.Y < (this.displayLines.GetCount() - 1))))
				{
					point1.Y++;
					point1.X = 0;
					this.displayLines.AtLineEnd = false;
					flag1 = true;
				}
			}
			else
			{
				point1.X++;
			}
			if (flag1)
			{
				this.Source.BeginUpdate(UpdateReason.Navigate);
				try
				{
					ITextSource source1 = this.Source;
					source1.State |= NotifyState.PositionChanged;
					this.MoveTo(this.displayLines.DisplayPointToPoint(point1.X, point1.Y, false, false, true));
					return;
				}
				finally
				{
					this.Source.EndUpdate();
				}
			}
			this.MoveTo(this.displayLines.DisplayPointToPoint(point1.X, point1.Y, false, false, true));
		}

		public void MoveFileBegin()
		{
			this.MoveTo(0, 0);
		}

		public void MoveFileEnd()
		{
			if (this.Lines.Count == 0)
			{
				this.MoveTo(0, 0);
			}
			else
			{
				this.MoveTo(this.Lines.GetLength(this.Lines.Count - 1), this.Lines.Count - 1);
			}
		}

		public void MoveLineBegin()
		{
			Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
			point1.X = 0;
			this.MoveTo(this.displayLines.DisplayPointToPoint(point1));
		}

		public void MoveLineDown()
		{
			this.BeginLineEndUpdate();
			try
			{
				Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
				if (this.pages.PageType == PageType.PageLayout)
				{
					Point point3 = this.DisplayToScreen(point1.X, point1.Y);
					point1.X = point3.X;
				}
				if (this.vertNavigate)
				{
					point1.X = this.vertNavigateX;
				}
				point1.Y++;
				int num1 = (this.pages.PageType == PageType.PageLayout) ? this.ScreenToDisplayX(point1.X, point1.Y) : point1.X;
				bool flag1 = false;
				Point point2 = this.displayLines.DisplayPointToPoint(num1, point1.Y, true, false, false, ref flag1);
				this.displayLines.AtLineEnd = flag1;
				this.MoveTo(point2);
				this.vertNavigate = true;
				this.vertNavigateX = point1.X;
			}
			finally
			{
				this.EndLineEndUpdate();
			}
		}

		public void MoveLineEnd()
		{
			Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
			point1.X = this.displayLines[point1.Y].Length;
			this.BeginLineEndUpdate();
			try
			{
				this.displayLines.AtLineEnd = true;
				this.MoveTo(this.displayLines.DisplayPointToPoint(point1));
			}
			finally
			{
				this.EndLineEndUpdate();
			}
		}

		public void MoveLineUp()
		{
			this.BeginLineEndUpdate();
			try
			{
				Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
				if (point1.Y > 0)
				{
					point1.Y--;
				}
				if (this.pages.PageType == PageType.PageLayout)
				{
					Point point3 = this.DisplayToScreen(point1.X, point1.Y);
					point1.X = point3.X;
				}
				if (this.vertNavigate)
				{
					point1.X = this.vertNavigateX;
				}
				int num1 = (this.pages.PageType == PageType.PageLayout) ? this.ScreenToDisplayX(point1.X, point1.Y) : point1.X;
				bool flag1 = false;
				Point point2 = this.displayLines.DisplayPointToPoint(num1, point1.Y, true, false, false, ref flag1);
				this.displayLines.AtLineEnd = flag1;
				this.MoveTo(point2);
				this.vertNavigate = true;
				this.vertNavigateX = point1.X;
			}
			finally
			{
				this.EndLineEndUpdate();
			}
		}

		private void MovePage(bool Direction)
		{
			this.BeginLineEndUpdate();
			try
			{
				Point point1 = (this.pages.PageType == PageType.PageLayout) ? this.TextToScreen(this.Position) : this.displayLines.PointToDisplayPoint(this.Position);
				if (this.vertNavigate)
				{
					point1.X = this.vertNavigateX;
				}
				int num1 = point1.X;
				if (this.pages.PageType != PageType.PageLayout)
				{
					point1 = this.DisplayToScreen(point1.X, point1.Y);
				}
				int num2 = (this.pages.PageType == PageType.PageLayout) ? this.ClientRect.Height : Math.Max((int) (this.LinesInHeight() - 1), 0);
				if (Direction)
				{
					num2 += this.scrolling.WindowOriginY;
				}
				else
				{
					num2 = this.scrolling.WindowOriginY - num2;
				}
				if (num2 < 0)
				{
					this.scrolling.WindowOriginY = 0;
					this.MoveToLine(0);
				}
				else
				{
					int num3 = this.scrolling.WindowOriginY;
					this.scrolling.SafeSetWindowLine(num2);
					try
					{
						bool flag1 = false;
						point1 = this.ScreenToText(point1.X, point1.Y, ref flag1);
						this.displayLines.AtLineEnd = flag1;
					}
					finally
					{
						this.scrolling.SafeSetWindowLine(num3);
					}
					this.scrolling.WindowOriginY = num2;
					this.MoveTo(point1);
				}
				this.vertNavigate = true;
				this.vertNavigateX = num1;
			}
			finally
			{
				this.EndLineEndUpdate();
			}
		}

		public void MovePageDown()
		{
			this.MovePage(true);
		}

		public void MovePageUp()
		{
			this.MovePage(false);
		}

		public void MoveScreenBottom()
		{
			this.BeginLineEndUpdate();
			try
			{
				Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
				int num1 = point1.X;
				if (this.pages.PageType == PageType.PageLayout)
				{
					Point point3 = this.DisplayToScreen(point1.X, point1.Y);
					num1 = point3.X;
				}
				if (this.vertNavigate)
				{
					num1 = this.vertNavigateX;
				}
				Point point2 = (this.pages.PageType == PageType.PageLayout) ? this.ScreenToDisplay(num1, this.ClientRect.Height) : new Point(num1, this.scrolling.WindowOriginY + Math.Max((int) (this.LinesInHeight() - 1), 0));
				bool flag1 = false;
				point1 = this.displayLines.DisplayPointToPoint(point2.X, point2.Y, true, false, false, ref flag1);
				this.displayLines.AtLineEnd = flag1;
				this.MoveTo(point1);
				this.vertNavigate = true;
				this.vertNavigateX = num1;
			}
			finally
			{
				this.EndLineEndUpdate();
			}
		}

		public void MoveScreenTop()
		{
			this.BeginLineEndUpdate();
			try
			{
				Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
				int num1 = point1.X;
				if (this.pages.PageType == PageType.PageLayout)
				{
					Point point3 = this.DisplayToScreen(point1.X, point1.Y);
					num1 = point3.X;
				}
				if (this.vertNavigate)
				{
					num1 = this.vertNavigateX;
				}
				Point point2 = (this.pages.PageType == PageType.PageLayout) ? this.ScreenToDisplay(num1, 0) : new Point(num1, this.scrolling.WindowOriginY);
				bool flag1 = false;
				point1 = this.displayLines.DisplayPointToPoint(point2.X, point2.Y, true, false, false, ref flag1);
				this.displayLines.AtLineEnd = flag1;
				this.MoveTo(point1);
				this.vertNavigate = true;
				this.vertNavigateX = num1;
			}
			finally
			{
				this.EndLineEndUpdate();
			}
		}

		public void MoveTo(Point Position)
		{
			this.Source.MoveTo(Position);
		}

		public void MoveTo(int X, int Y)
		{
			this.Source.MoveTo(X, Y);
		}

		public void MoveToChar(int X)
		{
			this.Source.MoveToChar(X);
		}

		public void MoveToCloseBrace()
		{
			Point point1 = this.Position;
			if (Array.IndexOf(this.braces.OpenBraces, this.Lines.GetCharAt(this.Position)) >= 0)
			{
				point1.X++;
				if (this.braces.FindClosingBrace(ref point1))
				{
					this.Position = point1;
				}
			}
		}

		public void MoveToLine(int Y)
		{
			this.Source.MoveToLine(Y);
		}

		public void MoveToOpenBrace()
		{
			Point point1 = this.Position;
			if ((Array.IndexOf(this.braces.ClosingBraces, this.Lines.GetCharAt(point1)) >= 0) && this.braces.FindOpenBrace(ref point1))
			{
				this.Position = point1;
			}
		}

		public void MoveWordLeft()
		{
			Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
			string text1 = this.displayLines[point1.Y];
			if (point1.X == 0)
			{
				if (point1.Y > 0)
				{
					point1.Y--;
					text1 = this.displayLines[point1.Y];
					point1.X = text1.Length;
				}
				else
				{
					point1 = new Point(0, 0);
				}
			}
			int num1 = Math.Min(point1.X, text1.Length);
			if (num1 > 0)
			{
				bool flag1 = this.displayLines.IsDelimiter(text1, (int) (num1 - 1));
				while ((num1 > 0) && (this.displayLines.IsDelimiter(text1, (int) (num1 - 1)) == flag1))
				{
					num1--;
				}
			}
			this.MoveTo(this.displayLines.DisplayPointToPoint(num1, point1.Y, false, true, false));
		}

		public void MoveWordRight()
		{
			Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
			string text1 = this.displayLines[point1.Y];
			int num1 = text1.Length;
			if (point1.X >= num1)
			{
				if ((point1.Y + 1) < this.displayLines.GetCount())
				{
					point1.Y++;
					text1 = this.displayLines[point1.Y];
					point1.X = text1.Length - text1.TrimStart(new char[0]).Length;
				}
				else
				{
					point1.X = num1;
				}
				this.MoveTo(this.displayLines.DisplayPointToPoint(point1));
			}
			else
			{
				int num2 = point1.X;
				bool flag1 = this.displayLines.IsDelimiter(text1, num2);
				while ((num2 < num1) && (this.displayLines.IsDelimiter(text1, num2) == flag1))
				{
					num2++;
				}
				if (num2 == point1.X)
				{
					num2++;
				}
				this.MoveTo(this.displayLines.DisplayPointToPoint(num2, point1.Y, false, false, true));
			}
		}

		public void Navigate(int DeltaX, int DeltaY)
		{
			this.Source.Navigate(DeltaX, DeltaY);
		}

		private bool NeedIncrementalSearch(char Key)
		{
			bool flag1 = this.inIncrementalSearch;
			if (flag1)
			{
				this.IncrementalSearch(Key.ToString(), false);
			}
			return flag1;
		}

		public void Notification(object Sender, EventArgs e)
		{
			if (Sender is ITextSource)
			{
				ITextSource source1 = (ITextSource) Sender;
				if (e is PositionChangedEventArgs)
				{
					PositionChangedEventArgs args1 = (PositionChangedEventArgs) e;
					this.PositionChanged(args1.Reason, args1.DeltaX, args1.DeltaY);
				}
				else
				{
					bool flag1 = (source1.State & NotifyState.BookMarkChanged) != NotifyState.None;
					bool flag2 = (source1.State & NotifyState.Edit) != NotifyState.None;
					if ((source1.State & NotifyState.OverWriteChanged) != NotifyState.None)
					{
						this.UpdateCaretMode();
					}
					if ((flag2 || flag1) || ((source1.State & NotifyState.BlockChanged) != NotifyState.None))
					{
						if (flag2)
						{
							this.RescanLines(source1.FirstChanged, source1.LastChanged);
							this.OnTextChanged(EventArgs.Empty);
						}
						this.InvalidateWindow(source1.FirstChanged, source1.LastChanged, (flag1 || this.outlining.AllowOutlining) || ((this.gutter.Options & GutterOptions.PaintLineNumbers) != GutterOptions.None));
					}
					if (((source1.State & NotifyState.PositionChanged) != NotifyState.None) || ((source1.State & NotifyState.GotoBookMark) != NotifyState.None))
					{
						this.CheckIncrementalSeacrh();
						if (source1.ActiveEdit == this)
						{
							this.lineSeparator.TempUnhightlightLine();
							if (this.displayLines.LineEndUpdateCount == 0)
							{
								this.displayLines.AtLineEnd = false;
							}
							this.MakeVisible(this.Position, (source1.State & NotifyState.GotoBookMark) != NotifyState.None);
							this.UpdateCaret();
							this.UpdateSeparator();
							if ((this.selection.UpdateCount == 0) && ((this.selection.Options & SelectionOptions.PersistentBlocks) == SelectionOptions.None))
							{
								this.selection.Clear();
							}
							this.vertNavigate = false;
						}
					}
					if ((((source1.State & NotifyState.SelectBlock) != NotifyState.None) && (source1.ActiveEdit == this)) && ((this.selection.UpdateCount == 0) && ((this.selection.Options & SelectionOptions.PersistentBlocks) == SelectionOptions.None)))
					{
						this.selection.SetSelection(SelectionType.Stream, ((TextSource) source1).SelectBlockRect);
					}
					if ((source1.State & NotifyState.CountChanged) != NotifyState.None)
					{
						if (this.gutter.LineNumbersChanged())
						{
							base.Invalidate();
						}
						this.scrolling.UpdateScroll();
					}
					if ((source1.State & NotifyState.SyntaxChanged) != NotifyState.None)
					{
						this.SyntaxChanged();
					}
					if ((source1.State & NotifyState.Outline) != NotifyState.None)
					{
						this.DoOutlineText();
					}
					this.DoSourceStateChanged(source1.State, source1.FirstChanged, source1.LastChanged);
				}
			}
			else if (Sender == this.displayLines)
			{
				NotifyEventArgs args2 = (NotifyEventArgs) e;
				if (args2.Update || !this.WordWrap)
				{
					this.UpdatePages(args2.FirstChanged);
				}
				if ((args2.FirstChanged == 0) && (args2.LastChanged == 0x7fffffff))
				{
					base.Invalidate();
				}
				else
				{
					this.InvalidateWindow(args2.FirstChanged, args2.LastChanged, (args2.State & NotifyState.Outline) != NotifyState.None);
				}
				if (args2.Update)
				{
					this.scrolling.UpdateScroll();
					this.UpdateCaret();
				}
			}
		}

		protected void OnCodeCompletion(object source, EventArgs e)
		{
			this.DisableCodeCompletionTimer();
			if (this.codeCompletionArgs.Provider != null)
			{
				if (this.codeCompletionArgs.CompletionType == CodeCompletionType.QuickInfo)
				{
					if (this.mouseRange != null)
					{
						Point point1 = base.PointToScreen(this.TextToScreen(this.mouseRange.StartPoint, false));
						point1.Y += (this.painter.FontHeight + EditConsts.DefaultHintOffsetY);
						point1.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
						this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point1, true);
						return;
					}
					if ((this.mouseUrl != string.Empty) && (this.mouseUrl != null))
					{
						Point point2 = base.PointToScreen(this.DisplayToScreen(this.mouseUrlPoint.X, this.mouseUrlPoint.Y));
						point2.Y += (this.painter.FontHeight + EditConsts.DefaultHintOffsetY);
						point2.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
						this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point2, true);
						return;
					}
					if (this.dragMargin)
					{
						if (!this.margin.IsDragging)
						{
							Point point3 = base.PointToClient(Control.MousePosition);
							Point point4 = this.ScreenToDisplay(point3.X, point3.Y);
							point3 = base.PointToScreen(this.DisplayToScreen(this.Margin.Position, point4.Y));
							point3.Y = Control.MousePosition.Y + EditConsts.DefaultHintOffsetY;
							point3.X += EditConsts.DefaultHintOffsetX;
							this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider, point3, true);
						}
						return;
					}
				}
				if (this.codeCompletionArgs.ToolTip)
				{
					this.ShowCodeCompletionHint(this.codeCompletionArgs.Provider);
				}
				else
				{
					this.ShowCodeCompletionBox(this.codeCompletionArgs.Provider);
				}
			}
		}

		public bool OnCustomDraw(ITextPainter Painter, Rectangle Rect, DrawStage DrawStage, DrawState DrawState, DrawInfo DrawInfo)
		{
			if (this.CustomDraw != null)
			{
				this.customDrawEventArgs.DrawStage = DrawStage;
				this.customDrawEventArgs.DrawState = DrawState;
				this.customDrawEventArgs.DrawInfo = DrawInfo;
				this.customDrawEventArgs.Painter = Painter;
				this.customDrawEventArgs.Rect = Rect;
				this.customDrawEventArgs.Handled = false;
				this.CustomDraw(this, this.customDrawEventArgs);
				return this.customDrawEventArgs.Handled;
			}
			return false;
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);
			if (drgevent.Effect != DragDropEffects.None)
			{
				this.needStartDrag = false;
				if (this.selection.SelectionState == SelectionState.Drag)
				{
					if (!this.selection.Move(this.Position, (Control.ModifierKeys & Keys.Control) == Keys.None))
					{
						this.selection.Clear();
					}
					this.selection.SelectionState = SelectionState.None;
				}
				else
				{
					object obj1 = drgevent.Data.GetData(DataFormats.UnicodeText);
					if (obj1 == null)
					{
						obj1 = drgevent.Data.GetData(DataFormats.Text);
					}
					if (obj1 != null)
					{
						this.selection.SelectedText = (string) obj1;
					}
				}
			}
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			drgevent.Effect = DragDropEffects.None;
			base.OnDragOver(drgevent);
			if (drgevent.Effect == DragDropEffects.None)
			{
				if ((!this.Source.ReadOnly && (drgevent.Data != null)) && (drgevent.Data.GetDataPresent(DataFormats.Text) || drgevent.Data.GetDataPresent(DataFormats.UnicodeText)))
				{
					if ((Control.ModifierKeys & Keys.Control) != Keys.None)
					{
						drgevent.Effect = DragDropEffects.Copy;
					}
					else
					{
						drgevent.Effect = DragDropEffects.Move;
					}
					Point point1 = base.PointToClient(Cursor.Position);
					if (this.selection.NeedDragScroll(point1) && this.selection.DoDragScroll(point1))
					{
						return;
					}
					this.selection.BeginUpdate();
					this.BeginLineEndUpdate();
					try
					{
						bool flag1 = false;
						point1 = this.ScreenToText(point1.X, point1.Y, ref flag1);
						this.displayLines.AtLineEnd = flag1;
						this.Position = point1;
						return;
					}
					finally
					{
						this.EndLineEndUpdate();
						this.selection.EndUpdate();
					}
				}
				drgevent.Effect = DragDropEffects.None;
			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.DoFontChanged();
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			this.Source.ActiveEdit = this;
			if (!this.hideCaret)
			{
				this.CreateCaret();
				this.UpdateCaret();
				this.UpdateSeparator();
			}
			this.selection.UpdateSelection();
			base.OnGotFocus(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.CancelDragging();
			}
			this.keyProcessed = false;
			if (this.ProcessKey(e.KeyData))
			{
				this.keyProcessed = true;
				e.Handled = true;
			}
			base.OnKeyDown(e);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (this.keyProcessed)
			{
				this.keyProcessed = false;
				e.Handled = true;
			}
			else if ((!this.NeedIncrementalSearch(e.KeyChar) && (this.keyState == 0)) && (e.KeyChar >= ' '))
			{
				this.ProcessKeyPress(e.KeyChar);
				this.InternalProcessKey(e.KeyChar);
				e.Handled = true;
			}
			base.OnKeyPress(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			this.keyProcessed = false;
		}

		protected override void OnLostFocus(EventArgs e)
		{
			if (!this.IsCodeCompletionWindowFocused())
			{
				if (!this.hideCaret)
				{
					this.DestroyCaret();
				}
				this.UpdateSeparator();
				this.selection.UpdateSelection();
				this.CancelDragging();
				base.OnLostFocus(e);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.needStartDrag = false;
			if (base.CanFocus)
			{
				IOutlineRange range1;
				base.Focus();
				bool flag1 = false;
				Point point1 = this.ScreenToText(e.X, e.Y, ref flag1);
				bool flag2 = (e.Button != MouseButtons.Right) || ((River.Orqa.Editor.NavigateOptions.MoveOnRightButton & this.NavigateOptions) != River.Orqa.Editor.NavigateOptions.None);
				bool flag3 = (e.Button == MouseButtons.Left) && ((SelectionOptions.DisableSelection & this.selection.Options) == SelectionOptions.None);
				bool flag4 = ((e.Button == MouseButtons.Left) && (e.Clicks == 2)) && (e.X >= this.gutter.GetWidth());
				bool flag5 = this.selection.SelectionState == SelectionState.None;
				bool flag6 = flag5 && ((this.selection.IsPosInSelection(point1) && !this.Source.ReadOnly) && ((SelectionOptions.DisableDragging & this.selection.Options) == SelectionOptions.None));
				if (this.gutter.IsMouseOnOutlineImage(e.X, e.Y))
				{
					if (this.outlining.IsExpanded(point1.Y))
					{
						this.outlining.Collapse(point1.Y);
						return;
					}
					if (this.outlining.IsCollapsed(point1.Y))
					{
						this.outlining.Expand(point1.Y);
						return;
					}
				}
				if ((this.outlining.AllowOutlining && ((this.outlining.OutlineOptions & OutlineOptions.DrawButtons) != OutlineOptions.None)) && this.outlining.IsMouseOnOutlineButton(e.X, e.Y, out range1))
				{
					if (flag4)
					{
						this.selection.Clear();
						range1.Visible = true;
						return;
					}
					if (e.Button == MouseButtons.Left)
					{
						this.Position = point1;
						this.selection.SelectionState = SelectionState.Select;
						this.Selection.SetSelection(SelectionType.Stream, range1.StartPoint, range1.EndPoint);
						return;
					}
				}
				if (this.dragMargin && ((Control.ModifierKeys & Keys.Control) != Keys.None))
				{
					this.Margin.IsDragging = true;
					return;
				}
				if ((flag3 && !flag4) && ((Control.ModifierKeys & Keys.Shift) != Keys.None))
				{
					this.selection.SelectionState = SelectionState.Select;
					this.selection.OnSelect(this, null);
				}
				else
				{
					if (flag2)
					{
						this.displayLines.AtLineEnd = flag1;
						this.selection.BeginUpdate();
						this.BeginLineEndUpdate();
						try
						{
							this.Position = point1;
						}
						finally
						{
							this.EndLineEndUpdate();
							this.selection.EndUpdate();
						}
					}
					if (flag3)
					{
						if (flag4)
						{
							if ((SelectionOptions.SelectLineOnDblClick & this.selection.Options) != SelectionOptions.None)
							{
								this.selection.SelectLine();
							}
							else
							{
								this.selection.UpdateSelStart(false);
								this.selection.SelectWord();
								this.selection.StartSelection();
							}
							this.selection.SelectionState = SelectionState.SelectWord;
						}
						else if (flag5)
						{
							if (flag6)
							{
								this.selection.EndSelection();
								this.selection.SelectionState = SelectionState.Drag;
								this.needStartDrag = true;
							}
							else
							{
								this.selection.Clear();
								this.selection.SelectionState = SelectionState.Select;
								this.selection.StartSelection();
							}
							this.selection.UpdateSelStart(false);
						}
					}
					else if ((e.Button == MouseButtons.Right) && (this.selection.SelectionState != SelectionState.None))
					{
						this.selection.EndSelection();
					}
				}
			}
			this.urlAtCursor = this.hyperText.IsUrlAtPoint(e.X, e.Y);
			if (this.gutter.IsMouseOnGutter(e.X, e.Y))
			{
				if (e.Clicks == 1)
				{
					this.gutter.OnClick(new EventArgs());
				}
				else
				{
					this.gutter.OnDoubleClick(new EventArgs());
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (((this.codeCompletionHint != null) && ((this.codeCompletionHint.CompletionFlags & CodeCompletionFlags.CloseOnMouseLeave) != CodeCompletionFlags.None)) && !this.codeCompletionHint.Bounds.Contains(Control.MousePosition))
			{
				this.codeCompletionHint.Close(false);
				this.mouseRange = null;
				this.DisableCodeCompletionTimer();
			}
			if (((this.codeCompletionBox != null) && ((this.codeCompletionBox.CompletionFlags & CodeCompletionFlags.CloseOnMouseLeave) != CodeCompletionFlags.None)) && !this.codeCompletionBox.Bounds.Contains(Control.MousePosition))
			{
				this.codeCompletionBox.Close(false);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.needStartDrag && ((e.Button & MouseButtons.Left) != MouseButtons.None))
			{
				this.StartDragging();
			}
			else
			{
				if (this.outlining.AllowOutlining && ((OutlineOptions.ShowHints & this.Outlining.OutlineOptions) != OutlineOptions.None))
				{
					IOutlineRange range1;
					if (!this.outlining.IsMouseOnOutlineButton(e.X, e.Y, out range1))
					{
						range1 = null;
					}
					if (this.mouseRange != range1)
					{
						if (this.codeCompletionHint != null)
						{
							this.codeCompletionHint.Close(false);
						}
						this.DisableCodeCompletionTimer();
						this.mouseRange = range1;
						if (this.mouseRange != null)
						{
							this.TextToScreen(this.mouseRange.StartPoint);
							this.DoCodeToolTip(this.displayLines.GetOutlineHint(this.mouseRange));
							return;
						}
					}
				}
				if (this.margin.AllowDrag && this.margin.Visible)
				{
					if (this.margin.IsDragging)
					{
						this.margin.DragTo(e.X, e.Y);
						return;
					}
					bool flag1 = false;
					flag1 = this.margin.Contains(e.X, e.Y);
					if (this.dragMargin != flag1)
					{
						if (this.margin.ShowHints)
						{
							if (this.codeCompletionHint != null)
							{
								this.codeCompletionHint.Close(false);
							}
							this.DisableCodeCompletionTimer();
						}
						this.dragMargin = flag1;
						if (this.dragMargin && this.margin.ShowHints)
						{
							this.DoCodeToolTip(EditConsts.DefaultDragMarginHint);
						}
						return;
					}
				}
				if (this.hyperText.HighlightUrls && this.hyperText.ShowHints)
				{
					string text1 = string.Empty;
					if (!this.hyperText.IsUrlAtPoint(e.X, e.Y, out text1, true))
					{
						text1 = string.Empty;
					}
					if (text1 != this.mouseUrl)
					{
						if (this.codeCompletionHint != null)
						{
							this.codeCompletionHint.Close(false);
						}
						this.DisableCodeCompletionTimer();
						this.mouseUrl = text1;
						if ((this.mouseUrl != string.Empty) && (this.mouseUrl != null))
						{
							this.mouseUrlPoint = this.ScreenToDisplay(e.X, e.Y);
							this.DoCodeToolTip(string.Format(EditConsts.DefaultHyperTextHint, this.mouseUrl));
						}
					}
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Button == MouseButtons.Left)
			{
				string text1;
				switch (this.selection.SelectionState)
				{
					case SelectionState.None:
					{
						this.selection.Clear();
						break;
					}
					case SelectionState.Drag:
					{
						if (this.needStartDrag)
						{
							this.selection.Clear();
						}
						this.selection.SelectionState = SelectionState.None;
						break;
					}
					case SelectionState.Select:
					{
						this.selection.EndSelection();
						if (!this.selection.IsValidPos(this.Position))
						{
							this.selection.Clear();
						}
						break;
					}
					case SelectionState.SelectWord:
					{
						this.selection.SelectionState = SelectionState.None;
						this.selection.EndSelection();
						break;
					}
				}
				if ((this.urlAtCursor && (this.selection.SelectionState == SelectionState.None)) && (((Control.ModifierKeys & Keys.Control) != Keys.None) && this.hyperText.IsUrlAtPoint(e.X, e.Y, out text1, true)))
				{
					this.hyperText.UrlJump(text1);
				}
				if (this.margin.IsDragging)
				{
					Point point1 = this.ScreenToDisplay(e.X, e.Y);
					this.margin.Position = point1.X;
					this.CancelDragging();
				}
			}
			this.needStartDrag = false;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			int num1 = -e.Delta / 120;
			if (num1 != 0)
			{
				if (this.pages.PageType == PageType.PageLayout)
				{
					this.scrolling.WindowOriginY += ((num1 * SystemInformation.MouseWheelScrollLines) * this.painter.FontHeight);
				}
				else
				{
					this.scrolling.WindowOriginY += (num1 * SystemInformation.MouseWheelScrollLines);
				}
			}
		}

		protected bool OnNeedCompletion(CodeCompletionArgs e)
		{
			this.DisableCodeCompletionTimer();
			if (this.NeedCodeCompletion != null)
			{
				this.NeedCodeCompletion(this, e);
			}
			if ((((e.CompletionType == CodeCompletionType.CompleteWord) && e.NeedShow) && ((e.SelIndex >= 0) && (e.Provider != null))) && (e.SelIndex < e.Provider.Count))
			{
				e.Handled = true;
				this.InsertTextFromProvider(e.Provider.Strings[e.SelIndex], e.StartPosition);
			}
			if ((!e.Handled && e.NeedShow) && (e.Provider != null))
			{
				this.DoCodeCompletion();
			}
			return e.Handled;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (!pe.ClipRectangle.IsEmpty)
			{
				this.painter.BeginPaint(pe.Graphics);
				try
				{
					this.PaintRulerRect(this.painter);
					Rectangle rectangle1 = this.GetClientRect(true);
					Rectangle rectangle2 = rectangle1;
					rectangle2.Intersect(pe.ClipRectangle);
					switch (this.pages.PageType)
					{
						case PageType.PageBreaks:
						{
							this.PaintWindow(this.painter, this.scrolling.WindowOriginY, rectangle2, rectangle1.Location, 1f, 1f, this.pages.Rulers != EditRulers.None);
							this.pages.Paint(this.painter, rectangle2);
							return;
						}
						case PageType.PageLayout:
						{
							this.pages.Paint(this.painter, rectangle2);
							return;
						}
					}
					this.PaintWindow(this.painter, this.scrolling.WindowOriginY, rectangle2, rectangle1.Location, 1f, 1f, this.pages.Rulers != EditRulers.None);
				}
				finally
				{
					this.painter.EndPaint(pe.Graphics);
				}
			}
		}

		protected override void OnResize(EventArgs pe)
		{
			bool flag1 = this.gutter.LineNumbersChanged();
			base.OnResize(pe);
			if ((!flag1 && this.WordWrap) && ((this.GetWrapMargin() != this.WrapMargin) && (this.pages.PageType != PageType.PageLayout)))
			{
				this.UpdateWordWrap();
			}
			this.pages.UpdateRulers();
			this.scrolling.UpdateScroll();
		}

		private void PaintLineBookMark(ITextPainter Painter, IBookMark BookMark)
		{
			if (!this.outlining.AllowOutlining || this.outlining.IsVisible(new Point(BookMark.Char, BookMark.Line)))
			{
				int num1 = 4;
				Point point1 = this.displayLines.PointToDisplayPoint(BookMark.Char, BookMark.Line, false);
				Point point2 = this.DisplayToScreen(point1.X, point1.Y);
				point2.Y += Painter.FontHeight;
				if ((((point2.X + num1) >= this.gutter.GetWidth()) && ((point2.X - num1) < this.ClientRect.Width)) && ((point2.Y > 0) && ((point2.Y - num1) < this.ClientHeight())))
				{
					Point[] pointArray1 = new Point[3] { new Point(point2.X - num1, point2.Y), new Point(point2.X + num1, point2.Y), new Point(point2.X, point2.Y - num1) } ;
					Rectangle rectangle1 = new Rectangle(point2.X - num1, point2.Y - num1, num1 * 2, num1 * 2);
					this.drawInfo.Init();
					this.drawInfo.Line = point1.Y;
					this.drawInfo.Char = point1.X;
					if (!this.OnCustomDraw(Painter, rectangle1, DrawStage.Before, DrawState.LineBookMark, this.drawInfo))
					{
						Painter.Polygon(pointArray1, this.gutter.LineBookmarksColor);
					}
					this.OnCustomDraw(Painter, rectangle1, DrawStage.After, DrawState.LineBookMark, this.drawInfo);
				}
			}
		}

		private void PaintLineBookMarks(ITextPainter Painter, Rectangle rect)
		{
			BookMarkList list1 = new BookMarkList();
			this.Source.BookMarks.GetBookMarks(rect.Location, new Point(rect.Right, rect.Bottom), list1);
			for (int num1 = 0; num1 < list1.Count; num1++)
			{
				this.PaintLineBookMark(Painter, (IBookMark) list1[num1]);
			}
		}

		private void PaintRulerRect(ITextPainter Painter)
		{
			if (((this.pages.Rulers & EditRulers.Horizonal) != EditRulers.None) && ((this.pages.Rulers & EditRulers.Vertical) != EditRulers.None))
			{
				Rectangle rectangle1 = new Rectangle(0, 0, this.pages.HorzRuler.Left, this.pages.VertRuler.Top);
				Color color1 = Painter.BkColor;
				Color color2 = Painter.PenColor;
				try
				{
					Painter.BkColor = ((EditPages) this.Pages).RulerBackColor;
					Painter.FillRectangle(rectangle1);
					Rectangle rectangle2 = rectangle1;
					rectangle2.Inflate(-4, -4);
					Painter.BkColor = SystemColors.ControlDark;
					Painter.DrawRectangle(rectangle2);
					Painter.ExcludeClipRect(rectangle1.Left, rectangle1.Top, rectangle1.Width, rectangle1.Height);
				}
				finally
				{
					Painter.BkColor = color1;
					Painter.PenColor = color2;
				}
			}
		}

		protected internal void PaintWindow(ITextPainter Painter, int StartLine, Rectangle Rect, Point Location, float ScaleX, float ScaleY, bool SpecialPaint)
		{
			if (SpecialPaint)
			{
				this.painter.Transform(Location.X, Location.Y, ScaleX, ScaleY);
			}
			try
			{
				Rectangle rectangle6;
				this.drawInfo.Init();
				if (this.OnCustomDraw(Painter, Rect, DrawStage.Before, DrawState.Control, this.drawInfo))
				{
					return;
				}
				Rectangle rectangle1 = this.GetClientRect(true);
				Rectangle rectangle2 = new Rectangle(rectangle1.Left, Rect.Top, this.gutter.GetWidth(), Rect.Bottom);
				Point point2 = this.DisplayToScreen(this.margin.Position, StartLine, true);
				Rectangle rectangle3 = new Rectangle(point2.X, Rect.Top, 1, Rect.Height);
				Point point1 = new Point(rectangle2.Width, 0);
				if (this.pages.PageType == PageType.PageLayout)
				{
					rectangle3.Offset(-Location.X, 0);
				}
				else
				{
					point1.X -= (this.painter.FontWidth * this.scrolling.WindowOriginX);
				}
				int num1 = 0;
				if ((this.painter.FontHeight != 0) && (this.pages.PageType != PageType.PageLayout))
				{
					num1 = (Rect.Top - rectangle1.Top) / this.painter.FontHeight;
					point1.Y += (num1 * this.painter.FontHeight);
				}
				num1 += StartLine;
				this.wndBrush = new SolidBrush(this.BackColor);
				int num2 = this.painter.FontHeight;
				if (this.IsFocused())
				{
					this.selBackColor = this.Selection.BackColor;
					this.selForeColor = this.Selection.ForeColor;
					this.drawSelection = true;
				}
				else
				{
					this.drawSelection = (SelectionOptions.HideSelection & this.selection.Options) == SelectionOptions.None;
					this.selBackColor = this.Selection.InActiveBackColor;
					this.selForeColor = this.Selection.InActiveForeColor;
				}
				bool flag1 = (rectangle2.Width > 0) && !(rectangle6 = Rectangle.Intersect(Rect, rectangle2)).IsEmpty;
				bool flag2 = (this.margin.Visible && !this.margin.IsDragging) && !(rectangle6 = Rectangle.Intersect(Rect, rectangle3)).IsEmpty;
				IntPtr ptr1 = this.painter.SaveClip(this.ClientRect);
				Painter.FontStyle = this.Font.Style;
				if (flag1)
				{
					rectangle2.Offset(-rectangle1.Left, -rectangle1.Top);
					this.gutter.Paint(Painter, rectangle2, Location, StartLine);
					this.painter.ExcludeClipRect(rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
				}
				if (flag2)
				{
					rectangle3.Offset(-rectangle1.Left, -rectangle1.Top);
					this.margin.Paint(Painter, rectangle3);
					this.painter.ExcludeClipRect(rectangle3.Left, rectangle3.Top, rectangle3.Width, rectangle3.Height);
				}
				try
				{
					if (SpecialPaint)
					{
						this.painter.IntersectClipRect(Rect.Left - rectangle1.Left, Rect.Top - rectangle1.Top, Rect.Width, Rect.Height);
					}
					this.painter.Color = this.ForeColor;
					this.painter.BkColor = this.BackColor;
					if (this.Source.NeedParse())
					{
						this.EnsureLastLineParsed();
					}
					for (int num3 = num1; num3 < this.displayLines.GetCount(); num3++)
					{
						this.DrawLine(num3, point1, Rect);
						point1.Y += num2;
						if ((point1.Y > Rect.Bottom) || ((point1.Y == Rect.Bottom) && SpecialPaint))
						{
							break;
						}
						if ((this.lineSeparator.Options & SeparatorOptions.SeparateLines) != SeparatorOptions.None)
						{
							if ((this.lineSeparator.Options & SeparatorOptions.SeparateWrapLines) == SeparatorOptions.None)
							{
								point2 = this.displayLines.DisplayPointToPoint(0, num3 + 1);
								if (point2.X != 0)
								{
									goto Label_04A5;
								}
							}
							this.painter.Color = this.lineSeparator.LineColor;
							Rectangle rectangle4 = new Rectangle(Rect.Left, point1.Y, Rect.Width, this.painter.FontHeight);
							this.drawInfo.Init();
							this.drawInfo.Line = num3;
							if (!this.OnCustomDraw(this.painter, rectangle4, DrawStage.Before, DrawState.LineSeparator, this.drawInfo))
							{
								this.painter.DrawLine(Rect.Left, point1.Y - 1, Rect.Right, point1.Y - 1);
							}
							this.OnCustomDraw(this.painter, rectangle4, DrawStage.After, DrawState.LineSeparator, this.drawInfo);
						}
					Label_04A5:;
					}
					if ((point1.Y < Rect.Bottom) && this.DrawEndLine(point1))
					{
						point1.Y += num2;
					}
					if (point1.Y < Rect.Bottom)
					{
						this.painter.BkColor = this.BackColor;
						Rectangle rectangle5 = new Rectangle(Rect.Left, point1.Y, Rect.Width, Rect.Bottom - point1.Y);
						this.drawInfo.Init();
						if (!this.OnCustomDraw(this.painter, rectangle5, DrawStage.Before, DrawState.BeyondEof, this.drawInfo))
						{
							this.painter.FillRectangle(Rect.Left, point1.Y, Rect.Width, Rect.Bottom - point1.Y);
						}
						this.OnCustomDraw(this.painter, rectangle5, DrawStage.After, DrawState.BeyondEof, this.drawInfo);
					}
					if (this.gutter.DrawLineBookmarks)
					{
						this.PaintLineBookMarks(Painter, this.ClientRect);
					}
				}
				finally
				{
					this.painter.RestoreClip(ptr1);
				}
				this.drawInfo.Init();
				this.OnCustomDraw(Painter, Rect, DrawStage.After, DrawState.Control, this.drawInfo);
				this.wndBrush.Dispose();
				this.wndBrush = null;
			}
			finally
			{
				if (SpecialPaint)
				{
					this.painter.EndTransform();
				}
			}
		}

		public void ParameterInfo()
		{
			this.ParameterInfo(this.codeCompletionArgs);
			this.OnNeedCompletion(this.codeCompletionArgs);
		}

		protected virtual void ParameterInfo(CodeCompletionArgs e)
		{
			e.Init(CodeCompletionType.ParameterInfo, this.Position);
			e.ToolTip = true;
			if (((TextSource) this.Source).NeedCodeCompletion())
			{
				IFormatText text1 = this.Lexer as IFormatText;
				text1.CodeCompletion(this.RemovePlainText(this.Position.Y), this.Position, e);
			}
		}

		protected internal void PositionChanged(UpdateReason Reason, int DeltaX, int DeltaY)
		{
			Point point1 = this.Position;
			this.displayLines.PositionChanged(Reason, point1.X, point1.Y, DeltaX, DeltaY);
			this.selection.PositionChanged(this.Position.X, this.Position.Y, DeltaX, DeltaY);
		}

		public override bool PreProcessMessage(ref Message msg)
		{
			if (msg.Msg == 260)
			{
				Keys keys1 = Control.ModifierKeys | (((Keys) msg.WParam.ToInt32()) & Keys.KeyCode);
				if (this.ProcessKey(keys1))
				{
					return true;
				}
			}
			return base.PreProcessMessage(ref msg);
		}

		protected void ProcessCodeCompletion(char Char)
		{
			if (this.codeCompletionChars.IndexOf(Char) >= 0)
			{
				this.codeCompletionArgs.Init(CodeCompletionType.None, this.Position);
				this.codeCompletionArgs.KeyChar = Char;
				if (((TextSource) this.Source).NeedCodeCompletion())
				{
					IFormatText text1 = this.Lexer as IFormatText;
					text1.CodeCompletion(this.RemovePlainText(this.Position.Y), this.Position, this.codeCompletionArgs);
				}
				this.OnNeedCompletion(this.codeCompletionArgs);
			}
			else
			{
				this.DisableCodeCompletionTimer();
			}
		}

		protected bool ProcessKey(Keys KeyData)
		{
			if (this.keyList.ExecuteKey(KeyData, ref this.keyState))
			{
				return true;
			}
			this.keyState = 0;
			return false;
		}

		protected void ProcessKeyPress(char KeyChar)
		{
			if (this.Source.OverWrite)
			{
				this.Source.DeleteRight(1);
			}
			this.selection.InsertString(KeyChar.ToString());
		}

		protected void QueryEndDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (((e.Action == DragAction.Cancel) || (e.Action == DragAction.Drop)) || e.EscapePressed)
			{
				this.needStartDrag = false;
			}
			if ((e.Action == DragAction.Cancel) || e.EscapePressed)
			{
				this.Selection.BeginUpdate();
				try
				{
					this.Source.Position = this.saveDragPos;
				}
				finally
				{
					this.Selection.EndUpdate();
				}
			}
		}

		public void QuickInfo()
		{
			this.QuickInfo(this.codeCompletionArgs);
			this.OnNeedCompletion(this.codeCompletionArgs);
		}

		protected virtual void QuickInfo(CodeCompletionArgs e)
		{
			e.Init(CodeCompletionType.QuickInfo, this.Position);
			e.ToolTip = true;
		}

		Rectangle IControlProps.Bounds
		{
			get { return base.Bounds; }
			set { base.Bounds = value; }
		}

		Rectangle IControlProps.ClientRectangle
		{
			get { return base.ClientRectangle; }
		}

		int IControlProps.Height
		{
			get { return base.Height; }
			set { base.Height = value; }
		}

		bool IControlProps.IsHandleCreated
		{
			get { return base.IsHandleCreated; }
		}

		int IControlProps.Left
		{
			get { return base.Left; }
			set { base.Left = value; }
		}

		Point IControlProps.Location
		{
			get { return base.Location; }
			set { base.Location = value; }
		}

		Control IControlProps.Parent
		{
			get { return base.Parent; }
			set { base.Parent = value; }
		}

		int IControlProps.Top
		{
			get { return base.Top; }
			set { base.Top = value; }
		}

		bool IControlProps.Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		int IControlProps.Width
		{
			get { return base.Width; }
			set { base.Width = value; }
		}

		void IControlProps.Invalidate()
		{
			base.Invalidate();
		}

		void IControlProps.Invalidate(Rectangle rectangle1)
		{
			base.Invalidate(rectangle1);
		}

		Point IControlProps.PointToClient(Point point1)
		{
			return base.PointToClient(point1);
		}

		Point IControlProps.PointToScreen(Point point1)
		{
			return base.PointToScreen(point1);
		}

		void IControlProps.Update()
		{
			base.Update();
		}

		public string RemovePlainText(int Line)
		{
			StrItem item1 = this.Lines.GetItem(Line);
			string text1 = string.Empty;
			if (item1 != null)
			{
				text1 = item1.String;
				LexScheme scheme1 = (this.Lexer != null) ? this.Lexer.Scheme : null;
				if (scheme1 == null)
				{
					return text1;
				}
				text1 = string.Empty;
				for (int num1 = 0; num1 < item1.String.Length; num1++)
				{
					text1 = text1 + (scheme1.IsPlainText(((byte) item1.ColorData[num1]) - 1) ? ' ' : item1.String[num1]);
				}
			}
			return text1;
		}

		public bool Replace(string String, string ReplaceWith)
		{
			return this.Replace(String, ReplaceWith, SearchOptions.None, null);
		}

		public bool Replace(string String, string ReplaceWith, SearchOptions Options)
		{
			return this.Replace(String, ReplaceWith, Options, null);
		}

		public bool Replace(string String, string ReplaceWith, SearchOptions Options, Regex Expression)
		{
			this.FirstSearch = true;
			return this.DoReplace(String, ReplaceWith, Options, Expression, false);
		}

		public int ReplaceAll(string String, string ReplaceWith)
		{
			return this.ReplaceAll(String, ReplaceWith, SearchOptions.None, null);
		}

		public int ReplaceAll(string String, string ReplaceWith, SearchOptions Options)
		{
			return this.ReplaceAll(String, ReplaceWith, Options, null);
		}

		public int ReplaceAll(string String, string ReplaceWith, SearchOptions Options, Regex Expression)
		{
			return this.DoReplaceAll(String, ReplaceWith, Options, Expression);
		}

		private void RescanLines(int FirstLine, int LastLine)
		{
			this.UpdateWordWrap(FirstLine, LastLine);
			this.scrolling.UpdateScroll();
		}

		public virtual void ResetBorderStyle()
		{
			this.BorderStyle = EditBorderStyle.Fixed3D;
		}

		public virtual void ResetDisableColorPaint()
		{
			this.DisableColorPaint = false;
		}

		public virtual void ResetDisableSyntaxPaint()
		{
			this.DisableSyntaxPaint = false;
		}

		public virtual void ResetHideCaret()
		{
			this.HideCaret = false;
		}

		public virtual void ResetIndentOptions()
		{
			this.IndentOptions = EditConsts.DefaultIndentOptions;
		}

		public virtual void ResetModified()
		{
			this.Modified = false;
		}

		public virtual void ResetNavigateOptions()
		{
			this.NavigateOptions = EditConsts.DefaultNavigateOptions;
		}

		public virtual void ResetOverWrite()
		{
			this.OverWrite = false;
		}

		public virtual void ResetReadOnly()
		{
			this.ReadOnly = false;
		}

		public virtual void ResetWantReturns()
		{
			this.WantReturns = true;
		}

		public virtual void ResetWantTabs()
		{
			this.WantTabs = true;
		}

		public virtual void ResetWordWrap()
		{
			this.WordWrap = false;
		}

		public virtual void ResetWrapAtMargin()
		{
			this.WrapAtMargin = false;
		}

		public Point RestorePosition(int Index)
		{
			return this.Source.RestorePosition(Index);
		}

		private Cursor ReverseIncrementalSearchCursor()
		{
			if (this.reverseIncrementalSearchCursor == null)
			{
				this.reverseIncrementalSearchCursor = new Cursor(typeof(SyntaxEdit), "Images.ReverseIncrementalSearch.cur");
			}
			return this.reverseIncrementalSearchCursor;
		}

		public void SaveFile(string FileName)
		{
			this.displayLines.SaveFile(FileName);
		}

		public void SaveFile(string FileName, ExportFormat Format)
		{
			this.SaveFile(FileName, Format, null);
		}

		public void SaveFile(string FileName, Encoding Encoding)
		{
			this.SaveFile(FileName, ExportFormat.Text, Encoding);
		}

		public void SaveFile(string FileName, ExportFormat Format, Encoding Encoding)
		{
			this.displayLines.SaveFile(FileName, Format);
		}

		public void SaveStream(TextWriter Writer)
		{
			this.displayLines.SaveStream(Writer);
		}

		public void SaveStream(TextWriter Writer, ExportFormat Format)
		{
			this.displayLines.SaveStream(Writer, Format);
		}

		public Point ScreenToDisplay(int X, int Y)
		{
			int num1;
			int num2;
			if (this.pages.PageType == PageType.PageLayout)
			{
				IEditPage page1 = this.pages.GetPageAtPoint(X, Y);
				Rectangle rectangle1 = page1.ClientRect;
				X = Math.Min(X, rectangle1.Right);
				X -= rectangle1.Left;
				Y -= rectangle1.Top;
				num1 = Math.Min((int) (Math.Max(this.LinesInHeight(Y), 0) + page1.StartLine), page1.EndLine);
				num2 = X - this.gutter.GetWidth();
			}
			else
			{
				Rectangle rectangle2 = this.ClientRect;
				num1 = this.scrolling.WindowOriginY + this.LinesInHeight(Y - rectangle2.Top);
				num2 = ((-rectangle2.Left + (this.painter.FontWidth * this.scrolling.WindowOriginX)) + X) - this.gutter.GetWidth();
			}
			int num3 = 0;
			this.MeasureLine(num1, 0, -1, num2, out num3);
			return new Point(Math.Max(num3, 0), Math.Max(num1, 0));
		}

		public int ScreenToDisplayX(int X, int Line)
		{
			int num1;
			if (this.pages.PageType == PageType.PageLayout)
			{
				Rectangle rectangle1 = this.pages.GetPageAt(0, Line).ClientRect;
				X -= rectangle1.Left;
				num1 = X - this.gutter.GetWidth();
			}
			else
			{
				num1 = ((-this.ClientRect.Left + (this.painter.FontWidth * this.scrolling.WindowOriginX)) + X) - this.gutter.GetWidth();
			}
			int num2 = 0;
			this.MeasureLine(Line, 0, -1, num1, out num2);
			return Math.Max(num2, 0);
		}

		public Point ScreenToText(Point Position)
		{
			bool flag1 = false;
			return this.ScreenToText(Position.X, Position.Y, ref flag1);
		}

		public Point ScreenToText(int X, int Y)
		{
			bool flag1 = false;
			return this.ScreenToText(X, Y, ref flag1);
		}

		protected internal Point ScreenToText(int X, int Y, ref bool LineEnd)
		{
			Point point1 = this.ScreenToDisplay(X, Y);
			return this.displayLines.DisplayPointToPoint(point1.X, point1.Y, true, false, false, ref LineEnd);
		}

		public void ScrollLineDown()
		{
			if (this.pages.PageType == PageType.PageLayout)
			{
				this.scrolling.WindowOriginY += this.painter.FontHeight;
			}
			else
			{
				this.scrolling.WindowOriginY++;
			}
		}

		public void ScrollLineUp()
		{
			if (this.pages.PageType == PageType.PageLayout)
			{
				this.scrolling.WindowOriginY -= this.painter.FontHeight;
			}
			else
			{
				this.scrolling.WindowOriginY--;
			}
		}

		protected internal void SetNavigateOptions(River.Orqa.Editor.NavigateOptions NavigateOptions)
		{
			((TextSource) this.Source).SetNavigateOptions(NavigateOptions);
		}

		public bool ShouldSerializeCodeCompletionChars()
		{
			return (this.codeCompletionChars != EditConsts.DefaultCodeCompletionChars);
		}

		public bool ShouldSerializeIndentOptions()
		{
			if (this.source == null)
			{
				return (this.IndentOptions != EditConsts.DefaultIndentOptions);
			}
			return false;
		}

		public bool ShouldSerializeNavigateOptions()
		{
			if (this.source == null)
			{
				return (this.NavigateOptions != EditConsts.DefaultNavigateOptions);
			}
			return false;
		}

		public bool ShouldSerializeReadOnly()
		{
			if (this.source == null)
			{
				return this.ReadOnly;
			}
			return false;
		}

		protected internal bool ShouldSerializeSourceProps()
		{
			return (this.source == null);
		}

		public bool ShouldSerializeText()
		{
			return (this.source == null);
		}

		public void ShowCaret(int X, int Y)
		{
			Win32.SetCaretPos(X, Y);
		}

		public void ShowCodeCompletionBox(ICodeCompletionProvider Provider)
		{
			Point point1 = base.PointToScreen(this.TextToScreen(this.Position));
			point1.Y += this.painter.FontHeight;
			this.ShowCodeCompletionBox(Provider, point1);
		}

		public void ShowCodeCompletionBox(ICodeCompletionProvider Provider, Point Pt)
		{
			this.CodeCompletionBox.Provider = Provider;
			this.CodeCompletionBox.StartPos = this.codeCompletionArgs.StartPosition;
			this.CodeCompletionBox.EndPos = this.codeCompletionArgs.EndPosition;
			this.CodeCompletionBox.PopupAt(Pt);
		}

		public void ShowCodeCompletionHint(ICodeCompletionProvider Provider)
		{
			Point point1 = base.PointToScreen(this.TextToScreen(this.Position));
			point1.Y += this.painter.FontHeight;
			this.ShowCodeCompletionHint(Provider, point1);
		}

		public void ShowCodeCompletionHint(ICodeCompletionProvider Provider, Point Pt)
		{
			this.ShowCodeCompletionHint(Provider, Pt, false);
		}

		public void ShowCodeCompletionHint(ICodeCompletionProvider Provider, Point Pt, bool KeepActive)
		{
			this.CodeCompletionHint.Provider = Provider;
			this.CodeCompletionHint.StartPos = this.codeCompletionArgs.StartPosition;
			this.CodeCompletionHint.EndPos = this.codeCompletionArgs.EndPosition;
			if (KeepActive)
			{
				ICodeCompletionHint hint1 = this.CodeCompletionHint;
				hint1.CompletionFlags |= CodeCompletionFlags.KeepActive;
			}
			else
			{
				ICodeCompletionHint hint2 = this.CodeCompletionHint;
				hint2.CompletionFlags &= ((CodeCompletionFlags) (-129));
			}
			this.CodeCompletionHint.PopupAt(Pt);
		}

		public void ShowScrollHint(int Pos)
		{
			Point point2;
			if (this.Pages.PageType == PageType.PageLayout)
			{
				this.CodeCompletionHint.Provider = this.GetQuickInfo(string.Format(EditConsts.PageNofMstr, this.pages.GetPageIndexAtPoint(0, Pos - this.scrolling.WindowOriginY) + 1, this.Pages.Count), 0, EditConsts.PageStr.Length);
			}
			else
			{
				point2 = this.displayLines.DisplayPointToPoint(0, Pos);
				this.CodeCompletionHint.Provider = this.GetQuickInfo(string.Format(EditConsts.LineNofMstr, point2.Y + 1, this.Lines.Count), 0, EditConsts.LineStr.Length);
			}
			this.CodeCompletionHint.StartPos = new Point(-1, -1);
			this.CodeCompletionHint.EndPos = new Point(-1, -1);
			Point point1 = base.PointToScreen(new Point(base.Bounds.Width, 0));
			point1.X -= this.CodeCompletionHint.Width;
			point2 = base.PointToScreen(new Point(base.Bounds.Width, base.Bounds.Height));
			point1.Y = Math.Min((int) (Cursor.Position.Y + this.Painter.FontHeight), point2.Y);
			this.CodeCompletionHint.PopupAt(point1);
			base.Update();
		}

		private void SourceChanged()
		{
			if (this.displayLines != null)
			{
				this.displayLines.Lines = this.Source.Lines;
				this.DoOutlineText();
				this.MakeVisible(this.Position);
				this.UpdateCaret();
				this.scrolling.UpdateScroll();
				this.selection.Clear();
				base.Invalidate();
			}
		}

		protected void StartDragging()
		{
			this.saveDragPos = this.Position;
			if (!this.selection.IsEmpty())
			{
				base.DoDragDrop(this.selection.SelectedText, DragDropEffects.Move | (DragDropEffects.Copy | DragDropEffects.Scroll));
			}
		}

		public void StartIncrementalSearch()
		{
			this.StartIncrementalSearch(false);
		}

		public void StartIncrementalSearch(bool BackwardSearch)
		{
			this.inIncrementalSearch = true;
			this.incrSearchPosition = this.Position;
			this.searchOptions |= SearchOptions.FindTextAtCursor;
			if (BackwardSearch)
			{
				this.searchOptions |= SearchOptions.BackwardSearch;
			}
			else
			{
				this.searchOptions &= ((SearchOptions) (-9));
			}
			this.Source.BeginUpdate(UpdateReason.Other);
			try
			{
				ITextSource source1 = this.Source;
				source1.State |= NotifyState.IncrementalSearchChanged;
			}
			finally
			{
				this.Source.EndUpdate();
			}
			if (base.IsHandleCreated)
			{
				Win32.SendMessage(base.Handle, 0x20, IntPtr.Zero, IntPtr.Zero);
			}
		}

		public int StorePosition(Point pt)
		{
			return this.Source.StorePosition(pt);
		}

		protected void SyntaxChanged()
		{
			if (this.outlining.AllowOutlining && !((TextSource) this.Source).NeedOutlineText())
			{
				this.outlining.UnOutline();
			}
			this.DoFontChanged();
		}

		protected void TextFound(SearchOptions Options, Point Position, int Len, bool Silent)
		{
			if ((SearchOptions.BackwardSearch & Options) != SearchOptions.None)
			{
				this.Source.Position = Position;
			}
			else
			{
				this.Source.Position = new Point(Position.X + Len, Position.Y);
			}
			if (this.selection.UpdateCount == 0)
			{
				this.selection.SetSelection(SelectionType.Stream, new Rectangle(Position.X, Position.Y, Len, 0));
			}
			if ((!Silent && (this.searchDialog != null)) && this.searchDialog.Visible)
			{
				Rectangle rectangle1 = this.selection.GetSelectionRectInPixels();
				rectangle1.Inflate(this.Painter.FontWidth, 0);
				rectangle1.Location = base.PointToScreen(rectangle1.Location);
				this.searchDialog.EnsureVisible(rectangle1);
			}
		}

		public Point TextToScreen(Point Position)
		{
			return this.TextToScreen(Position.X, Position.Y);
		}

		protected internal Point TextToScreen(Point Position, bool LineEnd)
		{
			return this.TextToScreen(Position.X, Position.Y, LineEnd);
		}

		public Point TextToScreen(int X, int Y)
		{
			return this.TextToScreen(X, Y, this.displayLines.AtLineEnd);
		}

		protected internal Point TextToScreen(int X, int Y, bool LineEnd)
		{
			Point point1 = this.displayLines.PointToDisplayPoint(X, Y, LineEnd);
			return this.DisplayToScreen(point1.X, point1.Y);
		}

		protected internal void UpdateCaret()
		{
			if (this.IsFocused())
			{
				Point point1 = this.TextToScreen(this.Position);
				this.ShowCaret(point1.X, point1.Y);
				if (this.pages.Rulers != EditRulers.None)
				{
					this.pages.UpdateRulers();
				}
			}
		}

		protected internal void UpdateCaretMode()
		{
			if (this.IsFocused())
			{
				this.DestroyCaret();
				this.CreateCaret();
				this.UpdateCaret();
			}
		}

		protected void UpdateMonospaced()
		{
			if (this.Lexer != null)
			{
				ILexStyle[] styleArray1 = this.Lexer.Scheme.LexStyles;
				for (int num1 = 0; num1 < styleArray1.Length; num1++)
				{
					ILexStyle style1 = styleArray1[num1];
					this.painter.FontStyle = style1.FontStyle;
				}
			}
		}

		private void UpdatePages(int FirstLine)
		{
			if (this.Pages.PageType != PageType.Normal)
			{
				this.Pages.GetPageAt(this.displayLines.PointToDisplayPoint(0, FirstLine)).Update();
			}
		}

		private void UpdateSeparator()
		{
			if (this.lineSeparator.NeedHighlight())
			{
				this.InvalidateLine(this.oldLine);
				Point point1 = this.displayLines.PointToDisplayPoint(this.Position);
				this.oldLine = point1.Y;
				this.InvalidateLine(this.oldLine);
			}
		}

		public bool UpdateWordWrap()
		{
			return this.displayLines.UpdateWordWrap();
		}

		public bool UpdateWordWrap(int First, int Last)
		{
			return this.displayLines.UpdateWordWrap(First, Last);
		}

		public void ValidatePosition(ref Point Position)
		{
			this.Source.ValidatePosition(ref Position);
		}

		protected internal void WindowOriginChanged(int Line, int Char)
		{
			this.InvalidateLines(Char, Line);
			this.UpdateCaret();
			if (Line != this.scrolling.WindowOriginY)
			{
				if ((this.pages.Rulers & EditRulers.Vertical) != EditRulers.None)
				{
					this.pages.UpdateRulers();
				}
				if (this.gutter.LineNumbersChanged())
				{
					base.Invalidate();
				}
			}
			if (Char != this.scrolling.WindowOriginX)
			{
				if ((this.pages.Rulers & EditRulers.Horizonal) != EditRulers.None)
				{
					this.pages.UpdateRulers();
				}
				this.vertNavigate = false;
			}
		}

		protected override void WndProc(ref Message m)
		{
			int num1 = m.Msg;
			if (num1 != 0x20)
			{
				if (num1 == 0x85)
				{
					base.WndProc(ref m);
					if ((this.BorderStyle == EditBorderStyle.System) && (XPThemes.CurrentTheme != XPThemeName.None))
					{
						IntPtr ptr1 = Win32.GetWindowDC(base.Handle);
						try
						{
							Win32.ExcludeClipRect(ptr1, 2, 2, base.Width - 2, base.Height - 2);
							XPThemes.DrawEditBorder(ptr1, new Rectangle(0, 0, base.Width, base.Height));
							return;
						}
						finally
						{
							Win32.ReleaseDC(base.Handle, ptr1);
						}
					}
					return;
				}
				switch (num1)
				{
					case 0x114:
					{
						m.Result = (IntPtr) 1;
						this.scrolling.DoHorizontalScroll(Win32.LoWord(m.WParam), Win32.GetScrollPos(base.Handle, 0));
						return;
					}
					case 0x115:
					{
						m.Result = (IntPtr) 1;
						this.scrolling.DoVerticalScroll(Win32.LoWord(m.WParam), Win32.GetScrollPos(base.Handle, 1));
						return;
					}
				}
			}
			else if (this.CheckCursor(base.PointToClient(Control.MousePosition)))
			{
				m.Result = (IntPtr) 1;
				return;
			}
			base.WndProc(ref m);
		}


		// Properties
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public override bool AllowDrop
		{
			get
			{
				if (!base.AllowDrop)
				{
					return ((this.selection.Options & SelectionOptions.DisableDragging) == SelectionOptions.None);
				}
				return true;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		[DefaultValue(1), Category("Appearance")]
		public EditBorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.RecreateHandle();
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit")]
		public IBraceMatchingEx Braces
		{
			get
			{
				return this.braces;
			}
			set
			{
				this.braces.Assign(value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public Rectangle ClientRect
		{
			get
			{
				return this.GetClientRect(false);
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICodeCompletionBox CodeCompletionBox
		{
			get
			{
				if (this.codeCompletionBox == null)
				{
					this.codeCompletionBox = new River.Orqa.Editor.CodeCompletionBox(this);
					this.codeCompletionBox.ClosePopup += new ClosePopupEvent(this.CloseCodeCompletionBox);
				}
				return this.codeCompletionBox;
			}
		}

		[Category("SyntaxEdit")]
		public string CodeCompletionChars
		{
			get
			{
				return this.codeCompletionChars;
			}
			set
			{
				this.codeCompletionChars = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICodeCompletionHint CodeCompletionHint
		{
			get
			{
				if (this.codeCompletionHint == null)
				{
					this.codeCompletionHint = new River.Orqa.Editor.CodeCompletionHint(this);
				}
				return this.codeCompletionHint;
			}
		}

		protected internal Timer CodeCompletionTimer
		{
			get
			{
				if (this.codeCompletionTimer == null)
				{
					this.codeCompletionTimer = new Timer();
					this.codeCompletionTimer.Enabled = false;
					this.codeCompletionTimer.Interval = Consts.DefaultHintDelay;
					this.codeCompletionTimer.Tick += new EventHandler(this.OnCodeCompletion);
				}
				return this.codeCompletionTimer;
			}
		}

		protected override System.Windows.Forms.CreateParams CreateParams
		{
			get
			{
				System.Windows.Forms.CreateParams params1 = base.CreateParams;
				switch (this.borderStyle)
				{
					case EditBorderStyle.Fixed3D:
					case EditBorderStyle.System:
					{
						params1.ExStyle |= 0x200;
						return params1;
					}
					case EditBorderStyle.FixedSingle:
					{
						params1.Style |= 0x800000;
						return params1;
					}
				}
				return params1;
			}
		}

		[DefaultValue(false), Category("Appearance")]
		public bool DisableColorPaint
		{
			get
			{
				return this.disableColorPaint;
			}
			set
			{
				if (this.disableColorPaint != value)
				{
					this.disableColorPaint = value;
					base.Invalidate();
				}
			}
		}

		[DefaultValue(false), Category("Appearance")]
		public bool DisableSyntaxPaint
		{
			get
			{
				return this.disableSyntaxPaint;
			}
			set
			{
				if (this.disableSyntaxPaint != value)
				{
					this.disableSyntaxPaint = value;
					base.Invalidate();
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IDisplayStrings DisplayLines
		{
			get
			{
				return this.displayLines;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FirstSearch
		{
			get
			{
				return this.firstSearch;
			}
			set
			{
				if (this.firstSearch != value)
				{
					this.Source.BeginUpdate(UpdateReason.Other);
					try
					{
						this.firstSearch = value;
						ITextSource source1 = this.Source;
						source1.State |= NotifyState.FirstSearchChanged;
					}
					finally
					{
						this.Source.EndUpdate();
					}
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IGotoLineDialog GotoLineDialog
		{
			get
			{
				if (this.gotoLineDialog == null)
				{
					this.gotoLineDialog = new DlgGoto();
				}
				return this.gotoLineDialog;
			}
			set
			{
				this.gotoLineDialog = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit"), TypeConverter(typeof(ExpandableObjectConverter))]
		public IGutter Gutter
		{
			get
			{
				return this.gutter;
			}
			set
			{
				this.gutter.Assign(value);
			}
		}

		[Category("Behavior"), DefaultValue(false)]
		public bool HideCaret
		{
			get
			{
				return this.hideCaret;
			}
			set
			{
				if (this.hideCaret != value)
				{
					this.hideCaret = value;
					if (this.hideCaret)
					{
						this.DestroyCaret();
					}
				}
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public IHyperTextEx HyperText
		{
			get
			{
				return this.hyperText;
			}
			set
			{
				this.hyperText.Assign(value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public string IncrementalSearchString
		{
			get
			{
				if (!this.inIncrementalSearch)
				{
					return string.Empty;
				}
				return this.searchText;
			}
		}

		[Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), Category("Behavior")]
		public River.Orqa.Editor.IndentOptions IndentOptions
		{
			get
			{
				return this.Source.IndentOptions;
			}
			set
			{
				this.Source.IndentOptions = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool InIncrementalSearch
		{
			get
			{
				return this.inIncrementalSearch;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IKeyList KeyList
		{
			get
			{
				return this.keyList;
			}
		}

		[Category("SyntaxEdit"), DefaultValue((string) null)]
		public ILexer Lexer
		{
			get
			{
				return this.Source.Lexer;
			}
			set
			{
				this.Source.Lexer = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public ISyntaxStrings Lines
		{
			get
			{
				return this.Source.Lines;
			}
			set
			{
				this.Source.Lines = value;
			}
		}

		[Category("SyntaxEdit"), TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ILineSeparator LineSeparator
		{
			get
			{
				return this.lineSeparator;
			}
			set
			{
				this.lineSeparator.Assign(value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit"), Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public LineStylesEx LinesStyles
		{
			get
			{
				return this.lineStyles;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public ILineStylesEx LineStyles
		{
			get
			{
				return this.lineStyles;
			}
			set
			{
				this.lineStyles.Assign(value);
			}
		}

		[Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
		// SMC: added new 
		public new IMargin Margin
		{
			get
			{
				return this.margin;
			}
			set
			{
				this.margin.Assign(value);
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Modified
		{
			get
			{
				return this.Source.Modified;
			}
			set
			{
				this.Source.Modified = value;
			}
		}

		[Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), Category("Behavior")]
		public River.Orqa.Editor.NavigateOptions NavigateOptions
		{
			get
			{
				return this.Source.NavigateOptions;
			}
			set
			{
				this.Source.NavigateOptions = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit")]
		public IOutlining Outlining
		{
			get
			{
				return this.outlining;
			}
			set
			{
				this.outlining.Assign(value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool OverWrite
		{
			get
			{
				return this.Source.OverWrite;
			}
			set
			{
				this.Source.OverWrite = value;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit")]
		public IEditPages Pages
		{
			get
			{
				return this.pages;
			}
			set
			{
				if (this.pages != value)
				{
					this.pages.BeginUpdate();
					try
					{
						this.pages.Clear();
						foreach (IEditPage page1 in value.Items)
						{
							this.pages.Add().Assign(page1);
						}
					}
					finally
					{
						this.pages.EndUpdate();
					}
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITextPainter Painter
		{
			get
			{
				return this.painter;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point Position
		{
			get
			{
				return this.Source.Position;
			}
			set
			{
				this.Source.Position = value;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public IPrinting Printing
		{
			get
			{
				return this.printing;
			}
			set
			{
				this.printing.Assign(value);
			}
		}

		[DefaultValue(false), Category("Behavior")]
		public bool ReadOnly
		{
			get
			{
				return this.Source.ReadOnly;
			}
			set
			{
				this.Source.ReadOnly = value;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit")]
		public IScrolling Scrolling
		{
			get
			{
				return this.scrolling;
			}
			set
			{
				this.scrolling.Assign(value);
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ISearchDialog SearchDialog
		{
			get
			{
				if (this.searchDialog == null)
				{
					this.searchDialog = new River.Orqa.Editor.Dialogs.SearchDialog();
				}
				return this.searchDialog;
			}
			set
			{
				this.searchDialog = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SearchLen
		{
			get
			{
				return this.searchLen;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public Point SearchPos
		{
			get
			{
				return this.searchPos;
			}
			set
			{
				this.searchPos = value;
				this.FirstSearch = true;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ISelection Selection
		{
			get
			{
				return this.selection;
			}
			set
			{
				this.selection.Assign(value);
			}
		}

		[Category("SyntaxEdit")]
		public ITextSource Source
		{
			get
			{
				if (this.source == null)
				{
					return this.internalSource;
				}
				return this.source;
			}
			set
			{
				if (this.source != value)
				{
					if (this.source != null)
					{
						this.source.RemoveNotifier(this);
						this.source.ActiveEdit = null;
						this.source.Edits.Remove(this);
					}
					this.source = value;
					if (this.source != null)
					{
						this.source.AddNotifier(this);
						if (this.source.ActiveEdit == null)
						{
							this.source.ActiveEdit = this;
						}
						this.source.Edits.Add(this);
					}
					this.SourceChanged();
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter)), Category("SyntaxEdit")]
		public ISpellingEx Spelling
		{
			get
			{
				return this.spelling;
			}
			set
			{
				this.spelling.Assign(value);
			}
		}

		[Category("SyntaxEdit"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), TypeConverter(typeof(CollectionConverter))]
		public string[] Strings
		{
			get
			{
				ISyntaxStrings strings1 = this.Source.Lines;
				string[] textArray1 = new string[strings1.Count];
				for (int num1 = 0; num1 < strings1.Count; num1++)
				{
					textArray1[num1] = strings1[num1];
				}
				return textArray1;
			}
			set
			{
				ISyntaxStrings strings1 = this.Source.Lines;
				strings1.BeginUpdate();
				try
				{
					strings1.Clear();
					string[] textArray1 = value;
					for (int num1 = 0; num1 < textArray1.Length; num1++)
					{
						string text1 = textArray1[num1];
						strings1.Add(text1);
					}
				}
				finally
				{
					strings1.EndUpdate();
				}
			}
		}

		public override string Text
		{
			get
			{
				return this.Lines.Text;
			}
			set
			{
				this.Lines.Text = value;
			}
		}

		[DefaultValue(false), Category("Appearance")]
		public bool Transparent
		{
			get
			{
				return this.transparent;
			}
			set
			{
				if (this.transparent != value)
				{
					this.transparent = value;
					base.SetStyle(ControlStyles.Opaque, !value);
					base.SetStyle(ControlStyles.OptimizedDoubleBuffer, value);
					base.Invalidate();
				}
			}
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool WantReturns
		{
			get
			{
				return this.wantReturns;
			}
			set
			{
				this.wantReturns = value;
			}
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool WantTabs
		{
			get
			{
				return this.wantTabs;
			}
			set
			{
				this.wantTabs = value;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("SyntaxEdit")]
		public IWhiteSpace WhiteSpace
		{
			get
			{
				return this.whiteSpace;
			}
			set
			{
				this.whiteSpace.Assign(value);
			}
		}

		[Category("Appearance"), DefaultValue(false)]
		public bool WordWrap
		{
			get
			{
				return this.displayLines.WordWrap;
			}
			set
			{
				this.displayLines.WordWrap = value;
			}
		}

		[Category("Appearance"), DefaultValue(false)]
		public bool WrapAtMargin
		{
			get
			{
				return this.displayLines.WrapAtMargin;
			}
			set
			{
				this.displayLines.WrapAtMargin = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public int WrapMargin
		{
			get
			{
				return this.displayLines.WrapMargin;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public virtual object XmlInfo
		{
			get
			{
				return new XmlSyntaxEditInfo(this);
			}
			set
			{
				((XmlSyntaxEditInfo) value).FixupReferences(this);
			}
		}


		// Fields
		private EditBorderStyle borderStyle;
		private River.Orqa.Editor.Braces braces;
		private CodeCompletionArgs codeCompletionArgs;
		private River.Orqa.Editor.CodeCompletionBox codeCompletionBox;
		private string codeCompletionChars;
		private River.Orqa.Editor.CodeCompletionHint codeCompletionHint;
		private Timer codeCompletionTimer;
		private Container components;
		private CustomDrawEventArgs customDrawEventArgs;
		private bool disableColorPaint;
		private bool disableSyntaxPaint;
		private DisplayStrings displayLines;
		private bool dragMargin;
		private DrawInfo drawInfo;
		private bool drawSelection;
		private bool firstSearch;
		private IGotoLineDialog gotoLineDialog;
		private River.Orqa.Editor.Gutter gutter;
		private bool hideCaret;
		private HyperTextEx hyperText;
		private Cursor incrementalSearchCursor;
		private bool incrSearchFlag;
		private Point incrSearchPosition;
		private bool inIncrementalSearch;
		private TextSource internalSource;
		private IKeyList keyList;
		private bool keyProcessed;
		private int keyState;
		private Cursor leftArrowCursor;
		private River.Orqa.Editor.LineSeparator lineSeparator;
		private LineStylesEx lineStyles;
		private River.Orqa.Editor.Margin margin;
		private IOutlineRange mouseRange;
		private string mouseUrl;
		private Point mouseUrlPoint;
		private bool needStartDrag;
		private NotifyEventArgs notifyEventArgs;
		private int oldLine;
		private River.Orqa.Editor.Outlining outlining;
		private EditPages pages;
		private TextPainter painter;
		private River.Orqa.Editor.Printing printing;
		private Cursor reverseIncrementalSearchCursor;
		private Point saveDragPos;
		private River.Orqa.Editor.Scrolling scrolling;
		private ISearchDialog searchDialog;
		private Regex searchExpression;
		private int searchLen;
		private SearchOptions searchOptions;
		private Point searchPos;
		private string searchText;
		private Color selBackColor;
		private River.Orqa.Editor.Selection selection;
		private Color selForeColor;
		private ITextSource source;
		private River.Orqa.Editor.Spelling spelling;
		private StringFormat stringFormat;
		private bool transparent;
		private bool urlAtCursor;
		private bool vertNavigate;
		private int vertNavigateX;
		private bool wantReturns;
		private bool wantTabs;
		private River.Orqa.Editor.WhiteSpace whiteSpace;
		private Brush wndBrush;
	}
}

