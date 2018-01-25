namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(TextSource), "Images.TextSource.bmp")]
    public class TextSource : Component, ITextSource, IEditEx, IEdit, INavigate, IUndo, ISourceNotify, INotifyEx, INotify, INotifier, IImport, IExport, IHyperText, ISpelling, IBraceMatching
    {
        // Events
        [Category("TextSource")]
        public event HyperTextEvent CheckHyperText;
        private event EventHandler notifyHandler;
        [Category("TextSource")]
        public event WordSpellEvent WordSpell;

        // Methods
        public TextSource()
        {
            this.components = null;
            this.fileName = string.Empty;
            this.navigateOptions = EditConsts.DefaultNavigateOptions;
            this.indentOptions = River.Orqa.Editor.IndentOptions.SmartIndent | River.Orqa.Editor.IndentOptions.AutoIndent;
            this.firstChanged = -1;
            this.lastChanged = -1;
            this.undoOptions = EditConsts.DefaultUndoOptions;
            this.saveModifiedIdx = -1;
            this.undoLimitDelta = 0x10;
            this.parserLine = 0;
            this.spellSkipPt = new Point(-1, -1);
            this.openBraces = EditConsts.DefaultOpenBraces;
            this.closingBraces = EditConsts.DefaultClosingBraces;
            this.openBrace = new Point(-1, -1);
            this.closingBrace = new Point(-1, -1);
            this.tempBraceRects = null;
            this.insertStr = string.Empty;
            this.InitializeComponent();
            this.lines = new SyntaxStrings(this);
            this.lines.AddNotifier(this);
            this.undoList = new ArrayList();
            this.redoList = new ArrayList();
            this.edits = new ArrayList();
            this.positionList = new ArrayList();
            this.position = new Point(0, 0);
            this.insertStr = string.Empty;
            this.lineStyles = new River.Orqa.Editor.LineStyles(this);
            this.bookMarks = new River.Orqa.Editor.BookMarks(this);
            this.braceList = new SortList();
            this.braceComparer = new BraceComparer();
            this.bracePointComparer = new BracePointComparer();
			this.hyperTextArgs = new HyperTextEventArgs(String.Empty, false);
			this.wordSpellArgs = new WordSpellEventArgs(String.Empty, true, -1);
        }

        public TextSource(IContainer container) : this()
        {
            container.Add(this);
        }

        public Point AbsolutePositionToTextPoint(int Position)
        {
            Point point1 = new Point(0, 0);
            int num2 = this.lines.Count;
            while (Position > 0)
            {
                int num1 = (point1.Y < num2) ? this.lines.GetLength(point1.Y) : 0;
                if (Position > num1)
                {
                    point1.Y++;
                }
                else
                {
                    point1.X = Position;
                    break;
                }
                Position -= (num1 + 2);
            }
            return point1;
        }

        private void AddBrace(BraceItem Brace)
        {
            int num1;
            this.braceList.FindLast(Brace.Point, out num1, this.bracePointComparer);
            this.braceList.Insert(num1, Brace);
        }

        public void AddNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Combine(this.notifyHandler, new EventHandler(sender.Notification));
        }

        protected internal void AddUndo(UndoOperation Operation, object Data)
        {
            if (this.AllowUndo && (this.undoUpdateCount == 0))
            {
                UndoData data1 = new UndoData(Operation, Data);
                data1.position = this.position;
                data1.reason = this.reason;
                data1.updateCount = this.lockUndoCount;
                data1.undoFlag = this.undoFlag;
                this.GetUndoList().Add(data1);
                if ((!this.redo && (this.undoLimit != 0)) && (this.undoList.Count >= this.undoLimit))
                {
                    this.ApplyUndoLimit();
                }
                this.undoFlag = false;
            }
        }

        private void ApplyUndoLimit()
        {
            if ((this.undoLimit != 0) && (this.undoList.Count >= this.undoLimit))
            {
                int num1 = Math.Min((int) (this.undoLimitDelta + (this.undoList.Count - this.undoLimit)), this.undoList.Count);
                for (int num2 = 0; num2 < num1; num2++)
                {
                    this.undoList.RemoveAt(0);
                }
                this.saveModifiedIdx -= num1;
                if (this.saveModifiedIdx < 0)
                {
                    this.saveModifiedIdx = -1;
                    this.modified = this.GetModified(this.undoList, 0);
                }
            }
        }

        public int BeginUndoUpdate()
        {
            this.lockUndoCount++;
            return this.lockUndoCount;
        }

        public int BeginUpdate()
        {
            return this.BeginUpdate(UpdateReason.Other);
        }

        public int BeginUpdate(UpdateReason Reason)
        {
            if (this.updateCount == 0)
            {
                if ((Reason != UpdateReason.Other) && (Reason != UpdateReason.Navigate))
                {
                    if (this.bracesOptions != River.Orqa.Editor.BracesOptions.None)
                    {
                        this.DoUnhighlightBraces();
                    }
                    this.EndFmtTimer();
                }
                if (Reason != UpdateReason.Other)
                {
                    this.TempUnhighlightBraces();
                }
                this.insertStr = string.Empty;
                this.state = NotifyState.None;
                this.firstChanged = -1;
                this.lastChanged = -1;
                this.oldPosition = this.position;
                this.selectBlockRect = Rectangle.Empty;
                this.reason = Reason;
                this.undoFlag = true;
                this.count = this.lines.Count;
            }
            this.updateCount++;
            return this.updateCount;
        }

        public bool BreakLine()
        {
            if (this.readOnly)
            {
                return false;
            }
            if ((this.position.Y == 0) && (this.lines.Count == 0))
            {
                this.lines.Add(string.Empty);
            }
            if (this.position.Y >= this.lines.Count)
            {
                if ((River.Orqa.Editor.NavigateOptions.BeyondEof & this.navigateOptions) != River.Orqa.Editor.NavigateOptions.None)
                {
                    this.EnsurePosInsideText();
                }
                else
                {
                    return false;
                }
            }
            this.BeginUpdate(UpdateReason.Break);
            this.stringsUpdateCount++;
            try
            {
                string text1 = this.lines[this.position.Y];
                int num1 = text1.Length;
                if (this.position.X < num1)
                {
                    this.lines.Insert(this.position.Y + 1, text1.Substring(this.position.X, num1 - this.position.X));
                    this.lines[this.position.Y] = text1.Substring(0, this.position.X);
                }
                else
                {
                    this.lines.Insert(this.position.Y + 1, string.Empty);
                }
                this.AddUndo(UndoOperation.Break, null);
                this.LinesChanged(this.position.Y, 0x7fffffff, true);
                this.PositionChanged(UpdateReason.Break, -this.position.X, 1);
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public bool CanRedo()
        {
            if (this.AllowUndo)
            {
                return !this.IsEmptyUndoList(this.redoList);
            }
            return false;
        }

        public bool CanUndo()
        {
            if (this.AllowUndo)
            {
                return !this.IsEmptyUndoList(this.undoList);
            }
            return false;
        }

        public void ClearRedo()
        {
            this.redoList.Clear();
        }

        public void ClearUndo()
        {
            this.undoList.Clear();
        }

        public virtual StrItem CreateStrItem(string S)
        {
            return new StrItem(S);
        }

        public bool DeleteBlock(Rectangle Rect)
        {
            if (this.readOnly)
            {
                return false;
            }
            this.bookMarks.BlockDeleting(Rect);
            this.lineStyles.BlockDeleting(Rect);
            if (this.lexer is IFormatText)
            {
                ((IFormatText) this.lexer).BlockDeleting(Rect);
            }
            this.BeginUpdate(UpdateReason.DeleteBlock);
            this.stringsUpdateCount++;
            try
            {
                this.MoveTo(Rect.Left, Rect.Top);
                string text1 = this.lines[Rect.Top];
                int num1 = text1.Length;
                int num2 = Rect.Left - num1;
                if ((Rect.Top == Rect.Bottom) && (num2 > 0))
                {
                    return false;
                }
                string[] textArray1 = new string[(Rect.Bottom - Rect.Top) + 1];
                this.AddUndo(UndoOperation.DeleteBlock, textArray1);
                for (int num3 = Rect.Bottom - 1; num3 > Rect.Top; num3--)
                {
                    textArray1[num3 - Rect.Top] = this.lines[num3];
                    this.lines.RemoveAt(num3);
                }
                if (Rect.Left < num1)
                {
                    string text2;
                    if (Rect.Top == Rect.Bottom)
                    {
                        num1 = Math.Min(Rect.Right, num1) - Rect.Left;
                    }
                    else
                    {
                        num1 = text1.Length - Rect.Left;
                    }
                    textArray1[0] = text1.Substring(Rect.Left, num1);
                    this.lines[Rect.Top] = text2 = text1.Remove(Rect.Left, num1);
                    this.lines[Rect.Top] = text2;
                }
                else
                {
                    textArray1[0] = string.Empty;
                }
                if (Rect.Top != Rect.Bottom)
                {
                    text1 = this.lines[Rect.Top + 1];
                    num1 = Math.Min(text1.Length, Rect.Right);
                    textArray1[Rect.Bottom - Rect.Top] = text1.Substring(0, num1);
                    this.lines[Rect.Top] = this.lines[Rect.Top] + ((num2 > 0) ? new string(' ', num2) : string.Empty) + text1.Remove(0, num1);
                    this.lines.RemoveAt(Rect.Top + 1);
                }
                this.PositionChanged(UpdateReason.DeleteBlock, -(Rect.Right - Rect.Left), -(Rect.Bottom - Rect.Top));
                this.LinesChanged(Rect.Top, 0x7fffffff, true);
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public bool DeleteBlock(int Len)
        {
            Point point1 = this.IncPosition(this.position, Len);
            return this.DeleteBlock(new Rectangle(this.position.X, this.position.Y, point1.X - this.position.X, point1.Y - this.position.Y));
        }

        public bool DeleteLeft(int Len)
        {
            bool flag1;
            if (this.readOnly)
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.Delete);
            try
            {
                Len = Math.Min(Len, this.position.X);
                this.Navigate(-Len, 0);
                flag1 = this.DeleteRight(Len);
            }
            finally
            {
                this.EndUpdate();
            }
            return flag1;
        }

        public bool DeleteRight(int Len)
        {
            if ((this.readOnly || (Len <= 0)) || ((this.position.Y >= this.lines.Count) || (this.position.X >= this.lines.GetLength(this.position.Y))))
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.Delete);
            this.stringsUpdateCount++;
            try
            {
                string text1 = this.lines[this.position.Y];
                Len = Math.Min(Len, (int) (text1.Length - this.position.X));
                this.AddUndo(UndoOperation.Delete, text1.Substring(this.position.X, Len));
                this.lines[this.position.Y] = text1.Remove(this.position.X, Len);
                if (this.LexStateChanged(this.position.Y))
                {
                    this.LinesChanged(this.position.Y, 0x7fffffff, true);
                }
                else
                {
                    this.LinesChanged(this.position.Y, this.position.Y, true);
                }
                this.PositionChanged(UpdateReason.Delete, -Len, 0);
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public int DisableUndo()
        {
            this.undoUpdateCount++;
            return this.undoUpdateCount;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected internal void DoCheckSpelling(string String, ref short[] ColorData, int Line, bool WithUpdate)
        {
            if (String != string.Empty)
            {
                this.InitSpellTable();
                bool flag1 = ((this.state & NotifyState.Edit) != NotifyState.None) && (this.position.Y == Line);
                this.spellSkipPt = flag1 ? this.position : new Point(-1, -1);
                int num1 = 0;
                bool flag2 = false;
                bool flag3 = false;
                int num2 = String.Length;
                if (WithUpdate)
                {
                    this.BeginUpdate(UpdateReason.Other);
                }
                try
                {
                    for (int num3 = 0; num3 < num2; num3++)
                    {
                        flag3 = this.spellTable.ContainsKey(String[num3]) || this.lines.IsDelimiter(String, num3);
                        if (num3 == 0)
                        {
                            flag2 = flag3;
                        }
                        if (flag2 != flag3)
                        {
                            if (flag3 && ((!flag1 || (this.spellSkipPt.X <= num1)) || (this.spellSkipPt.X > num3)))
                            {
                                this.DoCheckSpelling(String, ref ColorData, Line, num1, num3, WithUpdate);
                            }
                            flag2 = flag3;
                            num1 = num3;
                        }
                    }
                    if (flag2 || ((flag1 && (this.spellSkipPt.X > num1)) && (this.spellSkipPt.X <= num2)))
                    {
                        return;
                    }
                    this.DoCheckSpelling(String, ref ColorData, Line, num1, num2, WithUpdate);
                }
                finally
                {
                    if (WithUpdate)
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        private void DoCheckSpelling(string String, ref short[] ColorData, int Line, int Start, int End, bool WithUpdate)
        {
            if ((Start != End) && !this.IsWordCorrect(String.Substring(Start, End - Start), ColorData[Start]))
            {
                StrItem.SetColorFlag(ref ColorData, Start, End - Start, 8, true);
                if (WithUpdate)
                {
                    this.LinesChanged(Line, Line);
                    this.state |= NotifyState.BlockChanged;
                }
            }
        }

        private void DoFormatText()
        {
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                IFormatText text1 = (IFormatText) this.Lexer;
                text1.Strings = this.Lines;
                text1.ReparseText();
                this.state |= (NotifyState.Outline | NotifyState.BlockChanged);
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected internal bool DoHighlightBraces()
        {
            return this.DoHighlightBraces(this.position);
        }

        private bool DoHighlightBraces(Point Position)
        {
            int num1;
            int num2;
            BraceItem item1;
            Point point1 = this.openBrace;
            Point point2 = this.closingBrace;
            Point point3 = Position;
            this.openBrace = new Point(-1, -1);
            this.closingBrace = new Point(-1, -1);
            bool flag1 = false;
            if ((Position.X > 0) && this.braceList.FindExact(new Point(Position.X - 1, Position.Y), out num1, this.bracePointComparer))
            {
                item1 = (BraceItem) this.braceList[num1];
                if (!item1.Open)
                {
                    point3.X--;
                    goto Label_00E4;
                }
            }
            if (this.braceList.FindExact(Position, out num1, this.bracePointComparer))
            {
                item1 = (BraceItem) this.braceList[num1];
                if (item1.Open)
                {
                    point3.X++;
                }
            }
        Label_00E4:
            num2 = -1;
            if (this.FindOpenBrace(ref point3, ref num2))
            {
                Point point4 = point3;
                point4.X++;
                if (this.FindClosingBrace(ref point4, ref num2) && ((((this.bracesOptions & River.Orqa.Editor.BracesOptions.HighlightBounds) == River.Orqa.Editor.BracesOptions.None) || Position.Equals(point3)) || Position.Equals(new Point(point4.X + 1, point4.Y))))
                {
                    this.openBrace = point3;
                    this.closingBrace = point4;
                    flag1 = true;
                }
            }
            bool flag2 = (this.openBrace != point1) || (this.closingBrace != point2);
            if (flag2)
            {
                this.UpdateBrace(point1, false);
                this.UpdateBrace(point2, false);
                this.UpdateBraces();
            }
            if (flag1 && ((this.bracesOptions & River.Orqa.Editor.BracesOptions.TempHighlight) != River.Orqa.Editor.BracesOptions.None))
            {
                this.StartBracesTimer();
            }
            return flag2;
        }

        protected internal void DoHighlightUrls(string String, ref short[] ColorData, int Line)
        {
            int num1 = String.Length;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            this.InitUrlTable();
            while (num2 < num1)
            {
                num4 = num2;
                while ((num2 < num1) && !this.urlTable.ContainsKey(String[num2]))
                {
                    num2++;
                }
                num3 = num2;
                while ((num3 < num1) && this.urlTable.ContainsKey(String[num3]))
                {
                    num3++;
                }
                if ((num3 != num2) && this.IsHyperText(String.Substring(num2, num3 - num2)))
                {
                    StrItem.SetColorFlag(ref ColorData, num2, num3 - num2, 0x10, true);
                }
                num2 = num3;
                if (num2 == num4)
                {
                    num2++;
                }
            }
        }

        private void DoParseBraces(string String, ref short[] ColorData, int Line)
        {
            this.RemoveBraces(Line);
            for (int num1 = 0; num1 < String.Length; num1++)
            {
                char ch1 = String[num1];
                int num2 = Array.IndexOf(this.openBraces, ch1);
                int num3 = Array.IndexOf(this.closingBraces, ch1);
                if (((num2 >= 0) || (num3 >= 0)) && ((this.lexer == null) || !this.lexer.Scheme.IsPlainText(((byte) ColorData[num1]) - 1)))
                {
                    if ((num2 >= 0) && (num2 < this.closingBraces.Length))
                    {
                        this.AddBrace(new BraceItem(num1, Line, num2, true));
                    }
                    else if ((num3 >= 0) && (num3 < this.openBraces.Length))
                    {
                        this.AddBrace(new BraceItem(num1, Line, num3, false));
                    }
                }
            }
        }

        protected internal void DoUnhighlightBraces()
        {
            this.UpdateBrace(this.openBrace, false);
            this.UpdateBrace(this.closingBrace, false);
            this.openBrace = new Point(-1, -1);
            this.closingBrace = new Point(-1, -1);
            this.EndBracesTimer();
        }

        public int EnableUndo()
        {
            this.undoUpdateCount--;
            return this.undoUpdateCount;
        }

        private void EndBracesTimer()
        {
            if (this.bracesTimer != null)
            {
                this.bracesTimer.Enabled = false;
            }
        }

        private void EndFmtTimer()
        {
            if (this.fmtTimer != null)
            {
                this.fmtTimer.Enabled = false;
            }
        }

        public int EndUndoUpdate()
        {
            this.lockUndoCount--;
            if (this.lockUndoCount == 0)
            {
                this.AddUndo(UndoOperation.UndoBlock, null);
            }
            return this.lockUndoCount;
        }

        public int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                if (this.position != this.oldPosition)
                {
                    this.state |= NotifyState.PositionChanged;
                }
                if ((this.state & NotifyState.Edit) != NotifyState.None)
                {
                    if (!this.modified)
                    {
                        this.state |= NotifyState.ModifiedChanged;
                    }
                    this.modified = true;
                    this.lastParsed = Math.Min(this.lastParsed, this.firstChanged);
                }
                if (this.lines.Count != this.count)
                {
                    this.state |= NotifyState.CountChanged;
                }
                if (((this.state & NotifyState.Undo) == NotifyState.None) && (((this.state & NotifyState.Edit) != NotifyState.None) || (((this.state & NotifyState.PositionChanged) != NotifyState.None) && this.UndoNavigations)))
                {
                    this.ClearRedo();
                }
                if (this.state != NotifyState.None)
                {
                    bool flag1 = this.modified;
                    if (this.UndoAfterSave)
                    {
                        if (this.undoList.Count > this.saveModifiedIdx)
                        {
                            this.modified = this.GetModified(this.undoList, this.saveModifiedIdx + 1);
                        }
                        else
                        {
                            this.modified = this.GetModified(this.redoList, (this.redoList.Count + this.undoList.Count) - (this.saveModifiedIdx + 1));
                        }
                    }
                    else
                    {
                        this.modified = this.GetModified(this.undoList, 0);
                    }
                    if (flag1 != this.modified)
                    {
                        this.state |= NotifyState.ModifiedChanged;
                    }
                    this.Notify();
                }
                if (((this.state & NotifyState.Edit) != NotifyState.None) || ((this.state & NotifyState.SyntaxChanged) != NotifyState.None))
                {
                    this.StartFmtTimer();
                }
                if (((this.bracesOptions != River.Orqa.Editor.BracesOptions.None) && ((this.state & NotifyState.PositionChanged) != NotifyState.None)) && ((this.state & NotifyState.Edit) == NotifyState.None))
                {
                    this.DoHighlightBraces();
                }
                if (((this.checkSpelling && ((this.state & NotifyState.PositionChanged) != NotifyState.None)) && ((this.spellSkipPt.Y >= 0) && (this.spellSkipPt.X >= 0))) && ((this.spellSkipPt.Y == this.oldPosition.Y) && (this.oldPosition.Y != this.position.Y)))
                {
                    StrItem item1 = this.lines.GetItem(this.oldPosition.Y);
                    if (item1 != null)
                    {
                        this.DoCheckSpelling(item1.String, ref item1.ColorData, this.oldPosition.Y, true);
                    }
                }
            }
            return this.updateCount;
        }

        private void EnsurePosInsideText()
        {
            while (this.position.Y >= this.lines.Count)
            {
                this.lines.Add(string.Empty);
            }
        }

        ~TextSource()
        {
            if (this.fmtTimer != null)
            {
                this.fmtTimer.Enabled = false;
                this.fmtTimer.Dispose();
            }
            if (this.bracesTimer != null)
            {
                this.bracesTimer.Enabled = false;
                this.bracesTimer.Dispose();
            }
            this.lines.RemoveNotifier(this);
        }

        public bool FindClosingBrace(ref Point Position)
        {
            int num1 = -1;
            return this.FindClosingBrace(ref Position, ref num1);
        }

        private bool FindClosingBrace(ref Point Position, ref int BraceIndex)
        {
            int num1;
            if (this.braceList.FindLast((Point) Position, out num1, this.braceComparer))
            {
                int[] numArray1 = new int[this.openBraces.Length];
                for (int num2 = 0; num2 < numArray1.Length; num2++)
                {
                    numArray1[num2] = 1;
                }
                for (int num3 = num1 + 1; num3 < this.braceList.Count; num3++)
                {
                    int[] numArray2;
                    IntPtr ptr1;
                    BraceItem item1 = (BraceItem) this.braceList[num3];
                    if (item1.Open)
                    {
                        (numArray2 = numArray1)[(int) (ptr1 = (IntPtr) item1.BraceIndex)] = numArray2[(int) ptr1] + 1;
                    }
                    else
                    {
                        (numArray2 = numArray1)[(int) (ptr1 = (IntPtr) item1.BraceIndex)] = numArray2[(int) ptr1] - 1;
                        if ((numArray1[item1.BraceIndex] == 0) && ((BraceIndex < 0) || (BraceIndex == item1.BraceIndex)))
                        {
                            Position = item1.Point;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool FindClosingBrace(ref int X, ref int Y)
        {
            Point point1 = new Point(X, Y);
            if (this.FindClosingBrace(ref point1))
            {
                X = point1.X;
                Y = point1.Y;
                return true;
            }
            return false;
        }

        public bool FindOpenBrace(ref Point Position)
        {
            int num1 = -1;
            return this.FindOpenBrace(ref Position, ref num1);
        }

        private bool FindOpenBrace(ref Point Position, ref int BraceIndex)
        {
            int num1;
            if (this.braceList.FindLast((Point) Position, out num1, this.braceComparer))
            {
                int[] numArray1 = new int[this.openBraces.Length];
                for (int num2 = 0; num2 < numArray1.Length; num2++)
                {
                    numArray1[num2] = 1;
                }
                for (int num3 = num1; num3 >= 0; num3--)
                {
                    int[] numArray2;
                    IntPtr ptr1;
                    BraceItem item1 = (BraceItem) this.braceList[num3];
                    if (!item1.Open)
                    {
                        (numArray2 = numArray1)[(int) (ptr1 = (IntPtr) item1.BraceIndex)] = numArray2[(int) ptr1] + 1;
                    }
                    else
                    {
                        (numArray2 = numArray1)[(int) (ptr1 = (IntPtr) item1.BraceIndex)] = numArray2[(int) ptr1] - 1;
                        if ((numArray1[item1.BraceIndex] == 0) && ((BraceIndex < 0) || (BraceIndex == item1.BraceIndex)))
                        {
                            Position = item1.Point;
                            BraceIndex = item1.BraceIndex;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool FindOpenBrace(ref int X, ref int Y)
        {
            Point point1 = new Point(X, Y);
            if (this.FindOpenBrace(ref point1))
            {
                X = point1.X;
                Y = point1.Y;
                return true;
            }
            return false;
        }

        public void FormatText()
        {
            this.EndFmtTimer();
            if (this.NeedReparseText())
            {
                this.DoFormatText();
            }
        }

        private string GetAutoIndent()
        {
            if ((this.indentOptions & River.Orqa.Editor.IndentOptions.AutoIndent) == River.Orqa.Editor.IndentOptions.None)
            {
                return string.Empty;
            }
            if (((this.indentOptions & River.Orqa.Editor.IndentOptions.SmartIndent) != River.Orqa.Editor.IndentOptions.None) && this.NeedFormatText())
            {
                IOutlineRange range1 = ((IFormatText) this.lexer).GetBlock(this.Position) as IOutlineRange;
                int num1 = (range1 != null) ? (range1.Level + 1) : 0;
                return this.lines.GetIndentString(this.GetTabIndent(num1), 0);
            }
            int num2 = 0;
            string text1 = string.Empty;
            text1 = this.GetPrevIndentStr(out num2);
            if ((this.indentOptions & River.Orqa.Editor.IndentOptions.UsePrevIndent) != River.Orqa.Editor.IndentOptions.None)
            {
                return text1;
            }
            return this.lines.GetIndentString(num2, 0);
        }

        public int GetCharIndexFromPosition(Point Position)
        {
            return this.TextPointToAbsolutePosition(Position);
        }

        private bool GetModified(ArrayList List, int Index)
        {
            Index = Math.Max(0, Index);
            for (int num1 = List.Count - 1; num1 >= Index; num1--)
            {
                UndoData data1 = (UndoData) List[num1];
                if (data1.operation != UndoOperation.Navigate)
                {
                    return true;
                }
            }
            return false;
        }

        public Point GetPositionFromCharIndex(int CharIndex)
        {
            return this.AbsolutePositionToTextPoint(CharIndex);
        }

        private string GetPrevIndentStr(out int indent)
        {
            indent = 0;
            for (int num1 = this.position.Y - 1; num1 >= 0; num1--)
            {
                string text1 = this.Lines[num1];
                if (text1.Trim() != string.Empty)
                {
                    int num2 = text1.Length - text1.TrimStart(null).Length;
                    indent = this.Lines.TabPosToPos(text1, num2);
                    return text1.Substring(0, num2);
                }
            }
            return string.Empty;
        }

        private int GetTabIndent(int Indent)
        {
            return this.lines.TabPosToPos(new string('\t', Indent), Indent);
        }

        protected ArrayList GetUndoList()
        {
            if (this.redo)
            {
                return this.redoList;
            }
            return this.undoList;
        }

        protected internal Point IncPosition(Point Position, int Len)
        {
            int num1 = this.lines.Count;
            int num2 = 0;
            Point point1 = Position;
            while (Len > 0)
            {
                if (Position.Y < num1)
                {
                    num2 = (point1.Y < num1) ? this.lines.GetLength(point1.Y) : 0;
                    if (point1.Y == Position.Y)
                    {
                        num2 -= Position.X;
                    }
                    num2 = Math.Max(num2, 0);
                }
                if (Len > num2)
                {
                    point1.Y++;
                }
                else
                {
                    point1.X = Len;
                    if (Position.Y == point1.Y)
                    {
                        point1.X += Position.X;
                    }
                    break;
                }
                Len -= (num2 + 2);
            }
            return point1;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private void InitSpellTable()
        {
            if (this.spellTable == null)
            {
                this.spellTable = new Hashtable();
                char[] chArray1 = EditConsts.DefaultSpellDelimiters.ToCharArray();
                for (int num1 = 0; num1 < chArray1.Length; num1++)
                {
                    char ch1 = chArray1[num1];
                    this.spellTable.Add(ch1, ch1);
                }
            }
        }

        private void InitUrlTable()
        {
            if (this.urlTable == null)
            {
                this.urlTable = new Hashtable();
                for (char ch1 = 'A'; ch1 <= 'Z'; ch1++)
                {
                    this.urlTable.Add(ch1, ch1);
                }
                for (char ch2 = 'a'; ch2 <= 'z'; ch2++)
                {
                    this.urlTable.Add(ch2, ch2);
                }
                for (char ch3 = '0'; ch3 <= '9'; ch3++)
                {
                    this.urlTable.Add(ch3, ch3);
                }
                char[] chArray1 = EditConsts.DefaultUrlChars.ToCharArray();
                for (int num1 = 0; num1 < chArray1.Length; num1++)
                {
                    char ch4 = chArray1[num1];
                    this.urlTable.Add(ch4, ch4);
                }
            }
        }

        public bool Insert(string String)
        {
            if (this.readOnly)
            {
                return false;
            }
            this.insertStr = String;
            this.BeginUpdate(UpdateReason.Insert);
            this.stringsUpdateCount++;
            try
            {
                this.EnsurePosInsideText();
                string text1 = this.lines[this.position.Y];
                int num1 = text1.Length;
                if (this.position.X > num1)
                {
                    String = this.lines.GetIndentString(this.position.X - num1, this.Lines.TabPosToPos(text1, num1)) + String;
                    this.MoveTo(text1.Length, this.position.Y);
                }
                this.lines[this.position.Y] = text1.Insert(this.position.X, String);
                num1 = String.Length;
                this.AddUndo(UndoOperation.Insert, num1);
                if (this.LexStateChanged(this.position.Y))
                {
                    this.LinesChanged(this.position.Y, 0x7fffffff, true);
                }
                else
                {
                    this.LinesChanged(this.position.Y, this.position.Y, true);
                }
                this.PositionChanged(UpdateReason.Insert, num1, 0);
                if (!this.UndoNavigations || ((this.state & NotifyState.Undo) == NotifyState.None))
                {
                    this.Navigate(num1, 0);
                }
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public bool InsertBlock(ISyntaxStrings Strings)
        {
            if (!this.readOnly)
            {
                return this.InsertBlock(Strings.ToStringArray());
            }
            return false;
        }

        public bool InsertBlock(string[] Strings)
        {
            return this.InsertBlock(Strings, false);
        }

        public bool InsertBlock(string text)
        {
            return this.InsertBlock(StrItem.Split(text));
        }

        public bool InsertBlock(string[] Strings, bool Select)
        {
            if (this.readOnly || (Strings.Length == 0))
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.InsertBlock);
            this.stringsUpdateCount++;
            try
            {
                Point point1 = this.Position;
                this.EnsurePosInsideText();
                string text1 = this.lines[this.position.Y];
                int num1 = text1.Length;
                string text2 = string.Empty;
                if (this.position.X > num1)
                {
                    Strings[0] = this.lines.GetIndentString(this.position.X - num1, this.Lines.TabPosToPos(text1, num1)) + Strings[0];
                    this.MoveTo(num1, this.position.Y);
                }
                num1 = 0;
                string[] textArray1 = Strings;
                for (int num5 = 0; num5 < textArray1.Length; num5++)
                {
                    string text3 = textArray1[num5];
                    num1 += (text3.Length + 2);
                }
                num1 -= 2;
                this.AddUndo(UndoOperation.InsertBlock, num1);
                for (int num2 = 0; num2 < Strings.Length; num2++)
                {
                    if (num2 == 0)
                    {
                        string text4 = this.lines[this.position.Y];
                        num1 = text4.Length;
                        if (this.position.X < num1)
                        {
                            text2 = text4.Substring(this.position.X, num1 - this.Position.X);
                            text4 = text4.Substring(0, this.position.X) + Strings[0];
                        }
                        else
                        {
                            text4 = text4 + Strings[0];
                        }
                        if (num2 == (Strings.Length - 1))
                        {
                            text4 = text4 + text2;
                        }
                        this.lines[this.position.Y] = text4;
                    }
                    else if (num2 == (Strings.Length - 1))
                    {
                        this.lines.Insert(this.position.Y + num2, Strings[num2] + text2);
                    }
                    else
                    {
                        this.lines.Insert(this.position.Y + num2, Strings[num2]);
                    }
                }
                this.LinesChanged(this.position.Y, 0x7fffffff, true);
                int num3 = Strings.Length - 1;
                int num4 = Strings[Strings.Length - 1].Length;
                this.PositionChanged(UpdateReason.InsertBlock, num4, num3);
                if (!this.UndoNavigations || ((this.state & NotifyState.Undo) == NotifyState.None))
                {
                    this.Navigate(num4, num3);
                }
                if (Select)
                {
                    this.state |= NotifyState.SelectBlock;
                    this.selectBlockRect = new Rectangle(point1, new Size(this.position.X - point1.X, this.position.Y - point1.Y));
                }
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public bool InsertFromFile(string File)
        {
            if (this.readOnly)
            {
                return false;
            }
            ISyntaxStrings strings1 = new SyntaxStrings(null);
            strings1.LoadFile(File);
            return this.InsertBlock(strings1);
        }

        protected internal bool IsEmptyUndoList(ArrayList List)
        {
            if (this.UndoNavigations)
            {
                return (List.Count == 0);
            }
            for (int num1 = List.Count - 1; num1 >= 0; num1--)
            {
                UndoData data1 = (UndoData) List[num1];
                if (data1.operation != UndoOperation.Navigate)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsHyperText(string Text)
        {
            bool flag1 = HyperText.IsHyperText(Text);
            if (this.CheckHyperText != null)
            {
                this.hyperTextArgs.Text = Text;
                this.hyperTextArgs.IsHyperText = flag1;
                this.CheckHyperText(this, this.hyperTextArgs);
                flag1 = this.hyperTextArgs.IsHyperText;
            }
            return flag1;
        }

        public bool IsWordCorrect(string Text)
        {
            return this.IsWordCorrect(Text, 0);
        }

        public bool IsWordCorrect(string Text, int ColorStyle)
        {
            bool flag1 = true;
            if (this.WordSpell != null)
            {
                this.wordSpellArgs.Text = Text;
                this.wordSpellArgs.Correct = flag1;
                this.wordSpellArgs.ColorStyle = ColorStyle;
                this.WordSpell(this, this.wordSpellArgs);
                flag1 = this.wordSpellArgs.Correct;
            }
            return flag1;
        }

        private void LexerChanged()
        {
            this.BeginUpdate(UpdateReason.Other);
            try
            {
                this.lastParsed = 0;
                this.state |= NotifyState.SyntaxChanged;
                this.UpdateParsed(0, this.lines.Count - 1);
                this.braceList.Clear();
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected bool LexStateChanged(int Line)
        {
            if (((this.lexer == null) || (Line < 0)) || (Line >= this.lines.Count))
            {
                return false;
            }
            int num1 = this.lines.GetItem(Line).LexState;
            int num2 = ((Line > 0) && ((Line - 1) < this.lines.Count)) ? this.lines.GetItem(Line - 1).LexState : this.lexer.DefaultState;
            StrItem item1 = this.lines.GetItem(Line);
            return (this.ParseText(num2, item1.String, ref item1.ColorData, Line, true) != num1);
        }

        public void LinesChanged(int First, int Last)
        {
            this.LinesChanged(First, Last, false);
        }

        public void LinesChanged(int First, int Last, bool Modified)
        {
            if (this.firstChanged == -1)
            {
                this.firstChanged = First;
            }
            else
            {
                this.firstChanged = Math.Min(this.firstChanged, First);
            }
            this.lastChanged = Math.Max(this.lastChanged, Last);
            if (Modified)
            {
                this.state |= (NotifyState.Modified | NotifyState.Edit);
            }
        }

        public void LoadFile(string FileName)
        {
            this.LoadFile(FileName, null);
        }

        public void LoadFile(string FileName, Encoding Encoding)
        {
            this.lines.LoadFile(FileName, Encoding);
        }

        public void LoadStream(TextReader Reader)
        {
            this.lines.LoadStream(Reader);
        }

        public void MoveTo(Point Position)
        {
            this.MoveTo(Position.X, Position.Y);
        }

        public void MoveTo(int X, int Y)
        {
            this.Navigate(X - this.position.X, Y - this.position.Y);
        }

        public void MoveToChar(int X)
        {
            this.Navigate(X - this.position.X, 0);
        }

        public void MoveToLine(int Y)
        {
            this.Navigate(0, Y - this.position.Y);
        }

        public void Navigate(int DeltaX, int DeltaY)
        {
            if ((DeltaX != 0) || (DeltaY != 0))
            {
                this.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    Point point1 = this.position;
                    this.position.Offset(DeltaX, DeltaY);
                    this.ValidatePosition(ref this.position);
                    if (this.UndoNavigations)
                    {
                        this.AddUndo(UndoOperation.Navigate, new Point(this.position.X - point1.X, this.position.Y - point1.Y));
                    }
                    else
                    {
                        this.AddUndo(UndoOperation.Navigate, null);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public bool NeedCodeCompletion()
        {
            if (this.lexer is IFormatText)
            {
                return ((((IFormatText) this.lexer).Options & FormatTextOptions.CodeCompletion) != FormatTextOptions.None);
            }
            return false;
        }

        public bool NeedFormatText()
        {
            if (this.lexer is IFormatText)
            {
                return ((((IFormatText) this.lexer).Options & FormatTextOptions.SmartIndent) != FormatTextOptions.None);
            }
            return false;
        }

        public bool NeedOutlineText()
        {
            if (this.lexer is IFormatText)
            {
                return ((((IFormatText) this.lexer).Options & FormatTextOptions.Outline) != FormatTextOptions.None);
            }
            return false;
        }

        public bool NeedParse()
        {
            if (((this.lexer == null) && !this.checkSpelling) && !this.highlightUrls)
            {
                return (this.bracesOptions != River.Orqa.Editor.BracesOptions.None);
            }
            return true;
        }

        public bool NeedReparseText()
        {
            if (this.lexer is IFormatText)
            {
                return (((IFormatText) this.lexer).Options != FormatTextOptions.None);
            }
            return false;
        }

        public bool NewLine()
        {
            bool flag2;
            this.BeginUpdate(UpdateReason.Break);
            try
            {
                bool flag1 = this.BreakLine();
                this.MoveTo(0, this.Position.Y + 1);
                string text1 = this.GetAutoIndent();
                if (text1 != string.Empty)
                {
                    this.Insert(text1);
                }
                flag2 = flag1;
            }
            finally
            {
                this.EndUpdate();
            }
            return flag2;
        }

        public bool NewLineAbove()
        {
            bool flag2;
            this.BeginUpdate(UpdateReason.Break);
            try
            {
                this.MoveToChar(0);
                bool flag1 = this.BreakLine();
                string text1 = this.GetAutoIndent();
                if (text1 != string.Empty)
                {
                    this.Insert(text1);
                }
                flag2 = flag1;
            }
            finally
            {
                this.EndUpdate();
            }
            return flag2;
        }

        public bool NewLineBelow()
        {
            bool flag2;
            this.BeginUpdate(UpdateReason.Break);
            try
            {
                this.MoveToChar(this.Lines[this.Position.Y].Length);
                bool flag1 = this.BreakLine();
                this.MoveTo(0, this.Position.Y + 1);
                string text1 = this.GetAutoIndent();
                if (text1 != string.Empty)
                {
                    this.Insert(text1);
                }
                flag2 = flag1;
            }
            finally
            {
                this.EndUpdate();
            }
            return flag2;
        }

        public void Notification(object Sender, EventArgs e)
        {
            if (Sender is ISyntaxStrings)
            {
                this.StringsChanged(Sender, e);
            }
            else if (Sender is ILexer)
            {
                this.LexerChanged();
            }
        }

        public void Notify()
        {
            if (this.notifyHandler != null)
            {
                this.notifyHandler(this, EventArgs.Empty);
            }
        }

        protected void OnFormatting(object source, EventArgs e)
        {
            this.EndFmtTimer();
            if (this.NeedReparseText())
            {
                this.FormatText();
            }
        }

        protected void OnUnhighlightBraces(object source, EventArgs e)
        {
            if (this.bracesOptions != River.Orqa.Editor.BracesOptions.None)
            {
                this.DoUnhighlightBraces();
            }
            this.TempUnhighlightBraces();
        }

        public void ParseString(int Index)
        {
            this.ParseStrings(Index, Index);
        }

        public void ParseStrings(int FromIndex, int ToIndex)
        {
            if (this.NeedParse())
            {
                int num1 = ((FromIndex > 0) && ((FromIndex - 1) < this.lines.Count)) ? this.lines.GetItem(FromIndex - 1).LexState : ((this.lexer != null) ? this.lexer.DefaultState : 0);
                for (int num2 = Math.Max(FromIndex, 0); num2 <= Math.Min(ToIndex, (int) (this.lines.Count - 1)); num2++)
                {
                    StrItem item1 = this.lines.GetItem(num2);
                    if (((item1.State & StrItemState.Parsed) == StrItemState.None) || (item1.PrevLexState != num1))
                    {
                        item1.PrevLexState = num1;
                        num1 = this.ParseText(num1, item1.String, ref item1.ColorData, num2, false);
                        item1.LexState = num1;
                        item1.State |= StrItemState.Parsed;
                    }
                    else
                    {
                        if (this.bracesOptions != River.Orqa.Editor.BracesOptions.None)
                        {
                            this.DoParseBraces(item1.String, ref item1.ColorData, num2);
                        }
                        num1 = item1.LexState;
                    }
                }
            }
        }

        protected int ParseText(int State, string String, ref short[] ColorData, int Line, bool ParseTextOnly)
        {
            this.parserLine = Line;
            int num1 = (this.lexer != null) ? this.lexer.ParseText(State, String, ref ColorData) : State;
            if (!ParseTextOnly)
            {
                if (this.highlightUrls)
                {
                    this.DoHighlightUrls(String, ref ColorData, Line);
                }
                if (this.checkSpelling && (this.WordSpell != null))
                {
                    this.DoCheckSpelling(String, ref ColorData, Line, false);
                }
                if (this.bracesOptions != River.Orqa.Editor.BracesOptions.None)
                {
                    this.DoParseBraces(String, ref ColorData, Line);
                }
            }
            return num1;
        }

        public void ParseToString(int Index)
        {
            if (this.lastParsed < Index)
            {
                this.ParseStrings(this.lastParsed, Index);
                this.lastParsed = Index + 1;
                if ((this.bracesOptions != River.Orqa.Editor.BracesOptions.None) && !this.DoHighlightBraces())
                {
                    this.UpdateBraces();
                }
                if (this.tempBraceRects != null)
                {
                    this.UpdateTempBraces(true);
                }
            }
        }

        protected internal void PositionChanged(UpdateReason Reason, int DeltaX, int DeltaY)
        {
            if (this.notifyHandler != null)
            {
                this.notifyHandler(this, new PositionChangedEventArgs(Reason, DeltaX, DeltaY));
            }
            this.bookMarks.PositionChanged(this.Position.X, this.Position.Y, DeltaX, DeltaY);
            this.lineStyles.PositionChanged(this.Position.X, this.Position.Y, DeltaX, DeltaY);
            if (this.lexer is IFormatText)
            {
                ((IFormatText) this.lexer).PositionChanged(this.Position.X, this.Position.Y, DeltaX, DeltaY, Reason);
            }
            for (int num1 = 0; num1 < this.positionList.Count; num1++)
            {
                Point point1 = (Point) this.positionList[num1];
                SortList.UpdatePos(this.Position.X, this.Position.Y, DeltaX, DeltaY, ref point1, false);
                this.positionList[num1] = point1;
            }
        }

        public void Redo()
        {
            this.redo = !this.redo;
            try
            {
                this.Undo();
            }
            finally
            {
                this.redo = !this.redo;
            }
        }

        private void RemoveBraces(int Line)
        {
            int num1;
            if (this.braceList.FindLast(new Point(0x7fffffff, Line), out num1, this.braceComparer))
            {
                while (num1 >= 0)
                {
                    BraceItem item1 = (BraceItem) this.braceList[num1];
                    if (item1.Point.Y != Line)
                    {
                        return;
                    }
                    this.braceList.RemoveAt(num1);
                    num1--;
                }
            }
        }

        public void RemoveNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Remove(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public virtual void ResetBracesOptions()
        {
            this.BracesOptions = River.Orqa.Editor.BracesOptions.None;
        }

        public virtual void ResetCheckSpelling()
        {
            this.CheckSpelling = false;
        }

        public virtual void ResetClosingBraces()
        {
            this.OpenBraces = EditConsts.DefaultClosingBraces;
        }

        public virtual void ResetHighlightUrls()
        {
            this.HighlightUrls = false;
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

        public virtual void ResetOpenBraces()
        {
            this.OpenBraces = EditConsts.DefaultOpenBraces;
        }

        public virtual void ResetOverWrite()
        {
            this.OverWrite = false;
        }

        public virtual void ResetReadOnly()
        {
            this.ReadOnly = false;
        }

        public virtual void ResetUndoLimit()
        {
            this.UndoLimit = 0;
        }

        public virtual void ResetUndoOptions()
        {
            this.UndoOptions = EditConsts.DefaultUndoOptions;
        }

        public Point RestorePosition(int Index)
        {
            if (Index < this.positionList.Count)
            {
                Point point1 = (Point) this.positionList[Index];
                this.positionList.RemoveAt(Index);
                return point1;
            }
            return new Point(0, 0);
        }

        public void SaveFile(string FileName)
        {
            this.SaveFile(FileName, null);
        }

        public void SaveFile(string FileName, Encoding Encoding)
        {
            this.lines.SaveFile(FileName, Encoding);
            this.Modified = false;
        }

        public void SaveStream(TextWriter Writer)
        {
            this.lines.SaveStream(Writer);
        }

        internal void SetNavigateOptions(River.Orqa.Editor.NavigateOptions NavigateOptions)
        {
            this.navigateOptions = NavigateOptions;
        }

        public bool ShouldSerializeClosingBraces()
        {
            return (this.closingBraces != EditConsts.DefaultClosingBraces);
        }

        public bool ShouldSerializeFileName()
        {
            return (this.fileName != string.Empty);
        }

        public bool ShouldSerializeIndentOptions()
        {
            return (this.indentOptions != EditConsts.DefaultIndentOptions);
        }

        public bool ShouldSerializeLexer()
        {
            return (this.Lexer != null);
        }

        public bool ShouldSerializeNavigateOptions()
        {
            return (this.navigateOptions != EditConsts.DefaultNavigateOptions);
        }

        public bool ShouldSerializeOpenBraces()
        {
            return (this.openBraces != EditConsts.DefaultOpenBraces);
        }

        public bool ShouldSerializeText()
        {
            return (this.lines.Count > 0);
        }

        public bool ShouldSerializeUndoOptions()
        {
            return (this.undoOptions != EditConsts.DefaultUndoOptions);
        }

        private bool SkipNavigations(ArrayList List)
        {
            if (List.Count != 0)
            {
                UndoData data1 = (UndoData) List[List.Count - 1];
                if ((data1.operation == UndoOperation.Navigate) && (data1.data == null))
                {
                    this.BeginUpdate(UpdateReason.Navigate);
                    try
                    {
                        while (List.Count > 0)
                        {
                            data1 = (UndoData) List[List.Count - 1];
                            if ((data1.operation != UndoOperation.Navigate) || (data1.data != null))
                            {
                                goto Label_0095;
                            }
                            this.Undo(data1);
                            List.RemoveAt(List.Count - 1);
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        Label_0095:
            return (List.Count == 0);
        }

        private void StartBracesTimer()
        {
            this.EndBracesTimer();
            if (this.bracesTimer == null)
            {
                this.bracesTimer = new Timer();
                this.bracesTimer.Enabled = false;
                this.bracesTimer.Interval = EditConsts.DefaultBracesDelay;
                this.bracesTimer.Tick += new EventHandler(this.OnUnhighlightBraces);
            }
            this.bracesTimer.Enabled = true;
        }

        private void StartFmtTimer()
        {
            this.EndFmtTimer();
            if (this.NeedReparseText())
            {
                if (this.fmtTimer == null)
                {
                    this.fmtTimer = new Timer();
                    this.fmtTimer.Enabled = false;
                    this.fmtTimer.Interval = EditConsts.DefaultOutlineDelay;
                    this.fmtTimer.Tick += new EventHandler(this.OnFormatting);
                }
                this.fmtTimer.Enabled = true;
            }
        }

        public int StorePosition(Point pt)
        {
            return this.positionList.Add(pt);
        }

        protected void StringsChanged(object Sender, EventArgs ea)
        {
            if (this.stringsUpdateCount == 0)
            {
                this.BeginUpdate(UpdateReason.Other);
                try
                {
                    this.MoveTo(0, 0);
                    this.ClearUndo();
                    this.ClearRedo();
                    this.modified = false;
                    this.LinesChanged(((ISyntaxStrings) Sender).FirstChanged, ((ISyntaxStrings) Sender).LastChanged);
                    this.State |= (((this.State | NotifyState.Edit) | NotifyState.PositionChanged) | NotifyState.ModifiedChanged);
                    this.PositionChanged(UpdateReason.Other, 0, 0);
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void TempHighlightBraces(Rectangle[] Rectangles)
        {
            this.TempBraceRects = Rectangles;
        }

        public void TempUnhighlightBraces()
        {
            this.TempBraceRects = null;
            this.UpdateBraces();
        }

        public int TextPointToAbsolutePosition(Point Position)
        {
            int num1 = 0;
            int num2 = this.lines.Count;
            for (int num3 = 0; num3 < Position.Y; num3++)
            {
                num1 += (((num3 < num2) ? this.lines.GetLength(num3) : 0) + 2);
            }
            if (Position.Y < num2)
            {
                num1 += Math.Min(Position.X, this.lines.GetLength(Position.Y));
            }
            return num1;
        }

        public bool UnBreakLine()
        {
            if ((this.readOnly || (this.position.Y >= (this.lines.Count - 1))) || (this.lines.Count <= 1))
            {
                return false;
            }
            string text1 = this.lines[this.position.Y];
            int num1 = text1.Length;
            if ((this.position.X + 1) < num1)
            {
                return false;
            }
            this.BeginUpdate(UpdateReason.UnBreak);
            this.stringsUpdateCount++;
            try
            {
                num1 = this.position.X - num1;
                if ((num1 >= 0) && (this.lines[this.position.Y + 1] != string.Empty))
                {
                    this.lines[this.position.Y] = text1 + new string(' ', num1) + this.lines[this.position.Y + 1];
                }
                else
                {
                    this.lines[this.position.Y] = text1 + this.lines[this.position.Y + 1];
                }
                this.lines.RemoveAt(this.position.Y + 1);
                this.AddUndo(UndoOperation.UnBreak, null);
                this.LinesChanged(this.position.Y, 0x7fffffff, true);
                this.PositionChanged(UpdateReason.UnBreak, this.position.X, -1);
            }
            finally
            {
                this.stringsUpdateCount--;
                this.EndUpdate();
            }
            return true;
        }

        public void Undo()
        {
            ArrayList list1 = this.GetUndoList();
            if ((this.AllowUndo && (list1.Count > 0)) && !this.readOnly)
            {
                this.redo = !this.redo;
                try
                {
                    if (this.SkipNavigations(list1))
                    {
                        return;
                    }
                    UndoData data1 = (UndoData) list1[list1.Count - 1];
                    UpdateReason reason1 = data1.reason;
                    bool flag1 = data1.operation == UndoOperation.UndoBlock;
                    this.BeginUpdate(reason1);
                    try
                    {
                        this.state |= NotifyState.Undo;
                        while (list1.Count > 0)
                        {
                            data1 = (UndoData) list1[list1.Count - 1];
                            int num1 = data1.updateCount;
                            if ((reason1 != data1.reason) && (num1 == 0))
                            {
                                return;
                            }
                            bool flag2 = data1.undoFlag;
                            if ((data1.operation == UndoOperation.UndoBlock) && !flag1)
                            {
                                return;
                            }
                            this.Undo(data1);
                            list1.RemoveAt(list1.Count - 1);
                            if ((flag2 && !this.GroupUndo) && (num1 <= 0))
                            {
                                return;
                            }
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
                finally
                {
                    this.redo = !this.redo;
                }
            }
        }

        protected void Undo(UndoData undoData)
        {
            if (!this.UndoNavigations && (undoData.operation <= UndoOperation.DeleteBlock))
            {
                this.Position = undoData.position;
            }
            switch (undoData.operation)
            {
                case UndoOperation.Insert:
                {
                    this.DeleteRight((int) undoData.data);
                    return;
                }
                case UndoOperation.Delete:
                {
                    this.Insert((string) undoData.data);
                    return;
                }
                case UndoOperation.Break:
                {
                    this.UnBreakLine();
                    return;
                }
                case UndoOperation.UnBreak:
                {
                    this.BreakLine();
                    return;
                }
                case UndoOperation.InsertBlock:
                {
                    this.DeleteBlock((int) undoData.data);
                    return;
                }
                case UndoOperation.DeleteBlock:
                {
                    this.InsertBlock((string[]) undoData.data, true);
                    return;
                }
                case UndoOperation.Navigate:
                {
                    if (undoData.data == null)
                    {
                        this.AddUndo(UndoOperation.Navigate, null);
                        return;
                    }
                    Point point1 = (Point) undoData.data;
                    point1 = (Point) undoData.data;
                    this.Navigate(-point1.X, -point1.Y);
                    return;
                }
            }
        }

        private void UpdateBrace(Point Point, bool ASet)
        {
            this.UpdateBrace(Point.X, Point.Y, 1, ASet);
        }

        private void UpdateBrace(Rectangle Rect, bool ASet)
        {
            for (int num1 = Rect.Top; num1 <= Rect.Bottom; num1++)
            {
                int num2 = 0;
                int num3 = 0;
                if (num1 == Rect.Top)
                {
                    if (num1 == Rect.Bottom)
                    {
                        num2 = Rect.Left;
                        num3 = Rect.Right;
                    }
                    else
                    {
                        num2 = Rect.Left;
                        num3 = 0x7fffffff;
                    }
                }
                else if (num1 == Rect.Bottom)
                {
                    num2 = 0;
                    num3 = Rect.Right;
                }
                else
                {
                    num2 = 0;
                    num3 = 0x7fffffff;
                }
                this.UpdateBrace(num2, num1, (num3 == 0x7fffffff) ? num3 : (num3 - num2), ASet);
            }
        }

        private void UpdateBrace(int X, int Y, int Len, bool ASet)
        {
            if (Y >= 0)
            {
                StrItem item1 = this.Lines.GetItem(Y);
                if (item1 != null)
                {
                    int num1 = item1.ColorData.Length;
                    if ((X >= 0) && (X < num1))
                    {
                        Len = Math.Min(Len, (int) (num1 - X));
                        if (Len > 0)
                        {
                            this.BeginUpdate(UpdateReason.Other);
                            try
                            {
                                StrItem.SetColorFlag(ref item1.ColorData, X, Len, 0x20, ASet);
                                this.LinesChanged(Y, Y);
                                this.state |= NotifyState.BlockChanged;
                            }
                            finally
                            {
                                this.EndUpdate();
                            }
                        }
                    }
                }
            }
        }

        private void UpdateBraces()
        {
            this.UpdateBrace(this.openBrace, true);
            this.UpdateBrace(this.closingBrace, true);
        }

        protected void UpdateParsed(int FromIndex, int ToIndex)
        {
            for (int num1 = FromIndex; num1 <= Math.Min(ToIndex, (int) (this.lines.Count - 1)); num1++)
            {
                StrItem item1 = this.lines.GetItem(num1);
				unchecked
				{
					item1.State &= ((StrItemState)(-2));
				}
            }
        }

        private void UpdateTempBraces(bool ASet)
        {
            if (this.tempBraceRects != null)
            {
                Rectangle[] rectangleArray1 = this.tempBraceRects;
                for (int num1 = 0; num1 < rectangleArray1.Length; num1++)
                {
                    Rectangle rectangle1 = rectangleArray1[num1];
                    this.UpdateBrace(rectangle1, ASet);
                }
            }
        }

        public void ValidatePosition(ref Point Position)
        {
            if (Position.Y < 0)
            {
                Position.Y = 0;
            }
            if (Position.X < 0)
            {
                Position.X = 0;
            }
            if (((River.Orqa.Editor.NavigateOptions.BeyondEof & this.navigateOptions) == River.Orqa.Editor.NavigateOptions.None) && (Position.Y >= this.lines.Count))
            {
                Position.Y = Math.Max((int) (this.lines.Count - 1), 0);
            }
            if ((River.Orqa.Editor.NavigateOptions.BeyondEol & this.navigateOptions) == River.Orqa.Editor.NavigateOptions.None)
            {
                int num1 = (Position.Y < this.lines.Count) ? this.lines.GetLength(Position.Y) : 0;
                if (Position.X >= num1)
                {
                    Position.X = num1;
                }
            }
        }


        // Properties
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISyntaxEdit ActiveEdit
        {
            get
            {
                return this.activeEdit;
            }
            set
            {
                this.activeEdit = value;
            }
        }

        protected bool AllowUndo
        {
            get
            {
                return ((this.undoOptions & River.Orqa.Editor.UndoOptions.AllowUndo) != ((River.Orqa.Editor.UndoOptions) 0));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IBookMarks BookMarks
        {
            get
            {
                return this.bookMarks;
            }
            set
            {
                this.bookMarks.Assign(value);
            }
        }

        [Category("Braces"), Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), DefaultValue(0)]
        public River.Orqa.Editor.BracesOptions BracesOptions
        {
            get
            {
                return this.bracesOptions;
            }
            set
            {
                if (this.bracesOptions != value)
                {
                    this.bracesOptions = value;
                    this.LexerChanged();
                    if ((value & River.Orqa.Editor.BracesOptions.TempHighlight) == River.Orqa.Editor.BracesOptions.None)
                    {
                        this.UpdateTempBraces(false);
                    }
                    if ((value & River.Orqa.Editor.BracesOptions.Highlight) != River.Orqa.Editor.BracesOptions.None)
                    {
                        this.DoHighlightBraces();
                    }
                }
            }
        }

        [Category("Behavior"), DefaultValue(false)]
        public bool CheckSpelling
        {
            get
            {
                return this.checkSpelling;
            }
            set
            {
                if (this.checkSpelling != value)
                {
                    this.checkSpelling = value;
                    this.LexerChanged();
                }
            }
        }

        [Category("Braces")]
        public char[] ClosingBraces
        {
            get
            {
                return this.closingBraces;
            }
            set
            {
                this.closingBraces = new char[value.Length];
                Array.Copy(value, this.closingBraces, value.Length);
                if (this.bracesOptions != River.Orqa.Editor.BracesOptions.None)
                {
                    this.LexerChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList Edits
        {
            get
            {
                return this.edits;
            }
        }

        [Category("TextSource")]
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int FirstChanged
        {
            get
            {
                return this.firstChanged;
            }
        }

        protected bool GroupUndo
        {
            get
            {
                return ((this.undoOptions & River.Orqa.Editor.UndoOptions.GroupUndo) != ((River.Orqa.Editor.UndoOptions) 0));
            }
        }

        [Category("Behavior"), DefaultValue(false)]
        public bool HighlightUrls
        {
            get
            {
                return this.highlightUrls;
            }
            set
            {
                if (this.highlightUrls != value)
                {
                    this.highlightUrls = value;
                    this.LexerChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), Category("Behavior")]
        public River.Orqa.Editor.IndentOptions IndentOptions
        {
            get
            {
                return this.indentOptions;
            }
            set
            {
                this.indentOptions = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int LastChanged
        {
            get
            {
                return this.lastChanged;
            }
        }

        [Category("TextSource")]
        public ILexer Lexer
        {
            get
            {
                return this.lexer;
            }
            set
            {
                if (this.lexer != value)
                {
                    if (this.lexer is INotify)
                    {
                        ((INotify) this.lexer).RemoveNotifier(this);
                    }
                    this.lexer = value;
                    if (this.lexer is INotify)
                    {
                        ((INotify) this.lexer).AddNotifier(this);
                    }
                    this.LexerChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISyntaxStrings Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                this.lines.Assign(value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ILineStyles LineStyles
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Modified
        {
            get
            {
                return this.modified;
            }
            set
            {
                if (this.modified != value)
                {
                    this.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        this.modified = value;
                        if (!value)
                        {
                            this.saveModifiedIdx = this.GetUndoList().Count - 1;
                        }
                        this.state |= NotifyState.ModifiedChanged;
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), Category("Behavior")]
        public River.Orqa.Editor.NavigateOptions NavigateOptions
        {
            get
            {
                return this.navigateOptions;
            }
            set
            {
                if (this.navigateOptions != value)
                {
                    this.navigateOptions = value;
                    this.ValidatePosition(ref this.position);
                }
            }
        }

        protected internal Point OldPosition
        {
            get
            {
                return this.oldPosition;
            }
        }

        [Category("Braces")]
        public char[] OpenBraces
        {
            get
            {
                return this.openBraces;
            }
            set
            {
                this.openBraces = new char[value.Length];
                Array.Copy(value, this.openBraces, value.Length);
                if (this.bracesOptions != River.Orqa.Editor.BracesOptions.None)
                {
                    this.LexerChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool OverWrite
        {
            get
            {
                return TextSource.overWrite;
            }
            set
            {
                if (TextSource.overWrite != value)
                {
                    this.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        this.State |= (this.State | NotifyState.OverWriteChanged);
                        TextSource.overWrite = value;
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ParserLine
        {
            get
            {
                return this.parserLine;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.MoveTo(value.X, value.Y);
            }
        }

        [DefaultValue(false), Category("Behavior")]
        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
            set
            {
                if (this.readOnly != value)
                {
                    this.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        this.readOnly = value;
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        protected internal Rectangle SelectBlockRect
        {
            get
            {
                return this.selectBlockRect;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public NotifyState State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        [TypeConverter(typeof(CollectionConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("TextSource")]
        public string[] Strings
        {
            get
            {
                string[] textArray1 = new string[this.lines.Count];
                for (int num1 = 0; num1 < this.lines.Count; num1++)
                {
                    textArray1[num1] = this.lines[num1];
                }
                return textArray1;
            }
            set
            {
                this.lines.BeginUpdate();
                try
                {
                    this.lines.Clear();
                    string[] textArray1 = value;
                    for (int num1 = 0; num1 < textArray1.Length; num1++)
                    {
                        string text1 = textArray1[num1];
                        this.lines.Add(text1);
                    }
                }
                finally
                {
                    this.lines.EndUpdate();
                }
            }
        }

        protected internal Rectangle[] TempBraceRects
        {
            get
            {
                return this.tempBraceRects;
            }
            set
            {
                if (this.tempBraceRects != value)
                {
                    this.UpdateTempBraces(false);
                    this.tempBraceRects = value;
                    this.UpdateTempBraces(true);
                    if (value != null)
                    {
                        this.StartBracesTimer();
                    }
                }
            }
        }

        public string Text
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

        protected bool UndoAfterSave
        {
            get
            {
                return ((this.undoOptions & River.Orqa.Editor.UndoOptions.UndoAfterSave) != ((River.Orqa.Editor.UndoOptions) 0));
            }
        }

        [Category("Behavior"), DefaultValue(0)]
        public int UndoLimit
        {
            get
            {
                return this.undoLimit;
            }
            set
            {
                if (this.undoLimit != value)
                {
                    this.undoLimit = value;
                    this.ApplyUndoLimit();
                }
            }
        }

        protected bool UndoNavigations
        {
            get
            {
                return ((this.undoOptions & River.Orqa.Editor.UndoOptions.UndoNavigations) != ((River.Orqa.Editor.UndoOptions) 0));
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), Category("Behavior")]
        public River.Orqa.Editor.UndoOptions UndoOptions
        {
            get
            {
                return this.undoOptions;
            }
            set
            {
                if (this.undoOptions != value)
                {
                    if (((this.undoOptions ^ value) & River.Orqa.Editor.UndoOptions.AllowUndo) != ((River.Orqa.Editor.UndoOptions) 0))
                    {
                        this.ClearUndo();
                        this.ClearRedo();
                    }
                    this.undoOptions = value;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int UndoUpdateCount
        {
            get
            {
                return this.undoUpdateCount;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual object XmlInfo
        {
            get
            {
                return new XmlTextSourceInfo(this);
            }
            set
            {
                ((XmlTextSourceInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private ISyntaxEdit activeEdit;
        private River.Orqa.Editor.BookMarks bookMarks;
        private IComparer braceComparer;
        private SortList braceList;
        private IComparer bracePointComparer;
        private River.Orqa.Editor.BracesOptions bracesOptions;
        private Timer bracesTimer;
        private bool checkSpelling;
        private Point closingBrace;
        private char[] closingBraces;
        private Container components;
        private int count;
        private ArrayList edits;
        private string fileName;
        private int firstChanged;
        private Timer fmtTimer;
        private bool highlightUrls;
        private HyperTextEventArgs hyperTextArgs;
        private River.Orqa.Editor.IndentOptions indentOptions;
        private string insertStr;
        private int lastChanged;
        private int lastParsed;
        private ILexer lexer;
        private SyntaxStrings lines;
        private River.Orqa.Editor.LineStyles lineStyles;
        private int lockUndoCount;
        private bool modified;
        private River.Orqa.Editor.NavigateOptions navigateOptions;
        private Point oldPosition;
        private Point openBrace;
        private char[] openBraces;
        private static bool overWrite;
        private int parserLine;
        private Point position;
        private ArrayList positionList;
        private bool readOnly;
        private UpdateReason reason;
        private bool redo;
        private ArrayList redoList;
        private int saveModifiedIdx;
        private Rectangle selectBlockRect;
        private Point spellSkipPt;
        private Hashtable spellTable;
        private NotifyState state;
        private int stringsUpdateCount;
        private Rectangle[] tempBraceRects;
        private bool undoFlag;
        private int undoLimit;
        private int undoLimitDelta;
        private ArrayList undoList;
        private River.Orqa.Editor.UndoOptions undoOptions;
        private int undoUpdateCount;
        private int updateCount;
        private Hashtable urlTable;
        private WordSpellEventArgs wordSpellArgs;
    }
}

