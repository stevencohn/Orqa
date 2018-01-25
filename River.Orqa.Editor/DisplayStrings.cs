namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class DisplayStrings : IDisplayStrings, IEnumerator, IStringList, ICollection, IEnumerable, ITabulation, IWordWrap, ITextSearch, IWordBreak, ICollapsable, IFmtExport, IExport, IFmtImport, IImport
    {
        // Methods
        public DisplayStrings(ISyntaxEdit Owner, ISyntaxStrings Strings)
        {
            this.currentIndex = 0;
            this.outlineOptions = EditConsts.DefaultOutlineOptions;
            this.lastWrapped = -1;
            this.lastDisplayIndex = -1;
            this.lastWrapIndex = -1;
            char[] chArray1 = new char[1] { '\t' } ;
            this.tabArray = chArray1;
            this.maxLineIndex = -1;
            this.maxLineFlag = true;
            this.owner = Owner;
            this.outlineList = new OutlineList(this);
            this.wrapList = new WrapList(this);
            this.Lines = Strings;
            this.collapsedList = new ArrayList();
        }

        protected internal bool AlreadyScanned()
        {
            if (this.WordWrap)
            {
                return (this.lastWrapped >= ((this.lines.Count - this.outlineList.CollapsedCount) - 1));
            }
            return true;
        }

        protected void ApplyWhiteSpace(ref string Line, ref short[] Data)
        {
            for (int num1 = 0; num1 < Line.Length; num1++)
            {
                if (Line[num1] == ' ')
                {
                    StrItem.SetColorFlag(ref Data, num1, 1, 1, true);
                }
            }
        }

        protected internal void BlockDeleting(Rectangle Rect)
        {
            if (this.allowOutlining)
            {
                this.outlineList.BlockDeleting(Rect);
            }
        }

        private void CheckPage(IEditPages pages, ref int page, int DisplayLine, ref int Margin)
        {
            if (pages[page].EndLine < DisplayLine)
            {
                page += 1;
                ((EditPages) pages).UpdatePages(page, page, false);
                if (!this.wrapAtMargin)
                {
                    Margin = ((EditPage) pages[page]).DisplayWidth;
                }
            }
        }

        private void ClearLastLine()
        {
            this.lastWrapped = -1;
            this.lastWrapIndex = -1;
            this.lastDisplayIndex = -1;
        }

        public void Collapse(int Index)
        {
            ArrayList list1 = new ArrayList();
            this.GetOutlineRanges(list1, Index);
            this.outlineList.BeginUpdate();
            try
            {
                foreach (IOutlineRange range1 in list1)
                {
                    if ((range1.StartPoint.Y == Index) && range1.Visible)
                    {
                        range1.Visible = false;
                    }
                }
            }
            finally
            {
                this.outlineList.EndUpdate();
            }
        }

        protected internal void CollapseChanged(IOutlineRange Range)
        {
            int num1 = 0;
            int num2 = 0x7fffffff;
            if (this.wordWrap)
            {
                if (Range != null)
                {
                    num1 = Range.StartPoint.Y;
                    if (Range.StartPoint.Y == Range.EndPoint.Y)
                    {
                        num2 = Range.EndPoint.Y;
                    }
                    this.UpdateWordWrap(Range.StartPoint.Y, 0x7fffffff);
                }
                else
                {
                    this.UpdateWordWrap(0, 0x7fffffff);
                }
            }
            this.owner.Notification(this, new NotifyEventArgs(NotifyState.Outline, num1, num2, true));
            this.MaxLineChanged();
        }

        public void CopyTo(Array array, int index)
        {
            this.lines.CopyTo(array, index);
        }

        public Point DisplayPointToPoint(Point Point)
        {
            return this.DisplayPointToPoint(Point.X, Point.Y, false, false, false);
        }

        public Point DisplayPointToPoint(int X, int Y)
        {
            bool flag1 = false;
            return this.DisplayPointToPoint(X, Y, false, false, false, ref flag1);
        }

        protected internal Point DisplayPointToPoint(int X, int Y, bool WrapEnd, bool RangeStart, bool TabEnd)
        {
            bool flag1 = false;
            return this.DisplayPointToPoint(X, Y, WrapEnd, RangeStart, TabEnd, ref flag1);
        }

        protected internal Point DisplayPointToPoint(int X, int Y, bool WrapEnd, bool RangeStart, bool TabEnd, ref bool LineEnd)
        {
            Point point1 = new Point(X, Y);
            LineEnd = false;
            if (this.wordWrap)
            {
                this.wrapList.GetRealPoint(ref point1, WrapEnd, ref LineEnd);
            }
            if (point1.X != 0x7fffffff)
            {
                string text1 = string.Empty;
                short[] numArray1 = null;
                this.GetOutlineString(this.allowOutlining ? this.outlineList.DisplayLineToLine(point1.Y) : point1.Y, ref text1, ref numArray1, false, false);
                point1.X = ((SyntaxStrings) this.lines).PosToTabPos(text1, point1.X, TabEnd);
            }
            if (this.allowOutlining)
            {
                this.outlineList.GetRealPoint(ref point1, RangeStart);
            }
            return point1;
        }

        public void EnsureExpanded(Point Point)
        {
            if (this.allowOutlining)
            {
                ArrayList list1 = new ArrayList();
                this.GetOutlineRanges(list1, Point);
                bool flag1 = false;
                foreach (IOutlineRange range1 in list1)
                {
                    if (!range1.Visible && !range1.StartPoint.Equals(Point))
                    {
                        flag1 = true;
                        break;
                    }
                }
                if (flag1)
                {
                    this.outlineList.BeginUpdate();
                    try
                    {
                        foreach (IOutlineRange range2 in list1)
                        {
                            if (!range2.Visible && !range2.StartPoint.Equals(Point))
                            {
                                range2.Visible = true;
                            }
                        }
                    }
                    finally
                    {
                        this.outlineList.EndUpdate();
                    }
                }
            }
        }

        public void EnsureExpanded(int Index)
        {
            if (this.allowOutlining)
            {
                ArrayList list1 = new ArrayList();
                this.GetOutlineRanges(list1, Index);
                bool flag1 = false;
                foreach (IOutlineRange range1 in list1)
                {
                    if (!range1.Visible)
                    {
                        flag1 = true;
                        break;
                    }
                }
                if (flag1)
                {
                    this.outlineList.BeginUpdate();
                    try
                    {
                        foreach (IOutlineRange range2 in list1)
                        {
                            range2.Visible = true;
                        }
                    }
                    finally
                    {
                        this.outlineList.EndUpdate();
                    }
                }
            }
        }

        private void EnsureWrapped(int Index, bool Wrapped)
        {
            if ((Wrapped && (Index > this.lastWrapIndex)) || (!Wrapped && (Index > this.lastDisplayIndex)))
            {
                if (Index != 0x7fffffff)
                {
                    Index += EditConsts.DefaultWrapDelta;
                }
                int num1 = ((this.lastWrapped + 1) > 0) ? (this.PointToDisplayPoint(0x7fffffff, this.lastWrapped).Y + 1) : 0;
                this.WrapLines(this.lastWrapped + 1, Index, num1);
            }
        }

        public void Expand(int Index)
        {
            ArrayList list1 = new ArrayList();
            this.GetOutlineRanges(list1, Index);
            this.outlineList.BeginUpdate();
            try
            {
                foreach (IOutlineRange range1 in list1)
                {
                    if ((range1.StartPoint.Y == Index) && !range1.Visible)
                    {
                        range1.Visible = true;
                    }
                }
            }
            finally
            {
                this.outlineList.EndUpdate();
            }
        }

        public bool Find(string String, SearchOptions Options, Regex Expression, ref Point Position, out int Len)
        {
            return SyntaxStrings.Find(this, this.lines.DelimTable, String, Options, Expression, ref Position, out Len);
        }

        public void FullCollapse()
        {
            this.FullCollapse(this.outlineList.GetRanges());
        }

        public void FullCollapse(IList Ranges)
        {
            this.outlineList.BeginUpdate();
            try
            {
                foreach (IOutlineRange range1 in Ranges)
                {
                    range1.Visible = false;
                }
            }
            finally
            {
                this.outlineList.EndUpdate();
            }
        }

        public void FullExpand()
        {
            this.FullExpand(this.outlineList.GetRanges());
        }

        public void FullExpand(IList Ranges)
        {
            if (Ranges.Count > 0)
            {
                this.outlineList.BeginUpdate();
                try
                {
                    foreach (IOutlineRange range1 in Ranges)
                    {
                        range1.Visible = true;
                    }
                }
                finally
                {
                    this.outlineList.EndUpdate();
                }
            }
        }

        public short[] GetColorData(int Index)
        {
            string text1 = string.Empty;
            short[] numArray1 = null;
            this.GetDisplayString(Index, ref text1, ref numArray1, true);
            return numArray1;
        }

        public int GetCount()
        {
            return ((this.lines.Count + this.wrapList.Count) - this.outlineList.CollapsedCount);
        }

        protected internal int GetDisplayString(int Index, ref string Line, ref short[] Data, bool NeedData)
        {
            int num1 = Index;
            if ((this.wordWrap && (this.wrapList.Count > 0)) || this.allowOutlining)
            {
                Point point1 = new Point(0, Index);
                if (this.wordWrap)
                {
                    bool flag1 = false;
                    this.wrapList.GetRealPoint(ref point1, false, ref flag1);
                }
                if (this.allowOutlining)
                {
                    this.outlineList.GetRealPoint(ref point1, false);
                }
                this.GetOutlineString(point1.Y, ref Line, ref Data, NeedData, true);
                num1 = point1.Y;
                if (this.wordWrap)
                {
                    int num2 = 0;
                    int num3 = 0x7fffffff;
                    this.GetWrapBounds(Index, ref num2, ref num3);
                    if ((num2 != 0) || (num3 != 0x7fffffff))
                    {
                        this.GetSubString(num2, num3, ref Line, ref Data, NeedData && (Data != null));
                    }
                }
                return num1;
            }
            this.GetString(Index, ref Line, ref Data, NeedData, true);
            return num1;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        private int GetLineWidth(int Index)
        {
            if (Index < 0)
            {
                return 0;
            }
            string text1 = string.Empty;
            short[] numArray1 = null;
            this.GetDisplayString(Index, ref text1, ref numArray1, true);
            return this.owner.MeasureLine(text1, ref numArray1, 0, -1);
        }

        public string GetOutlineHint(IOutlineRange Range)
        {
            string text1 = string.Empty;
            int num1 = 0;
            int num2 = Math.Min((int) (this.lines.Count - 1), Range.EndPoint.Y);
            for (int num3 = Range.StartPoint.Y; num3 <= num2; num3++)
            {
                string text2 = this.lines[num3];
                if (num3 == Range.EndPoint.Y)
                {
                    text2 = text2.Substring(0, Math.Min(text2.Length, Range.EndPoint.X));
                }
                if (num3 == Range.StartPoint.Y)
                {
                    if (Range.StartPoint.X >= text2.Length)
                    {
                        goto Label_010A;
                    }
                    text2 = text2.Substring(Math.Min(Range.StartPoint.X, text2.Length));
                }
                text1 = (text1 == string.Empty) ? text2 : (text1 + Consts.CRLF + text2);
                if (num3 == num2)
                {
                    break;
                }
                num1++;
                if (num1 >= EditConsts.MaxHintWindowCount)
                {
                    text1 = text1 + EditConsts.DottedText;
                    break;
                }
            Label_010A:;
            }
            return text1;
        }

        public int GetOutlineLevel(Point Point)
        {
            IOutlineRange range1 = this.GetOutlineRange(Point);
            if (range1 == null)
            {
                return 0;
            }
            return (range1.Level + 1);
        }

        public IOutlineRange GetOutlineRange(Point Point)
        {
            if (this.allowOutlining)
            {
                return (this.outlineList.FindRange(Point) as IOutlineRange);
            }
            return null;
        }

        public IOutlineRange GetOutlineRange(int Index)
        {
            if (this.allowOutlining)
            {
                return (this.outlineList.FindRange(Index) as IOutlineRange);
            }
            return null;
        }

        public int GetOutlineRanges(IList Ranges)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(Ranges);
            }
            else
            {
                Ranges.Clear();
            }
            return Ranges.Count;
        }

        public int GetOutlineRanges(IList Ranges, Point Point)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(Ranges, Point);
            }
            else
            {
                Ranges.Clear();
            }
            return Ranges.Count;
        }

        public int GetOutlineRanges(IList Ranges, int Index)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(Ranges, Index);
            }
            else
            {
                Ranges.Clear();
            }
            return Ranges.Count;
        }

        public int GetOutlineRanges(IList Ranges, Point StartPoint, Point EndPoint)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(Ranges, StartPoint, EndPoint);
            }
            else
            {
                Ranges.Clear();
            }
            return Ranges.Count;
        }

        private void GetOutlineString(int Index, ref string Line, ref short[] Data, bool NeedData, bool ApplyTabs)
        {
            if (this.allowOutlining)
            {
                this.outlineList.CheckVisible(ref Index);
                if (this.GetString(Index, ref Line, ref Data, NeedData, false))
                {
                    int num1 = Line.Length;
                    this.outlineList.GetCollapsedRanges(Index, this.collapsedList);
                    for (int num2 = this.collapsedList.Count - 1; num2 >= 0; num2--)
                    {
                        IOutlineRange range1 = (IOutlineRange) this.collapsedList[num2];
                        string text1 = range1.DisplayText;
                        int num3 = 0;
                        int num4 = 0;
                        int num5 = Math.Min(Line.Length, num1);
                        num3 = Math.Min(range1.StartPoint.X, num5);
                        if (range1.EndPoint.Y == Index)
                        {
                            num4 = Math.Min((int) (num5 - num3), (int) (range1.EndPoint.X - range1.StartPoint.X));
                            Line = Line.Remove(num3, num4).Insert(num3, text1);
                            if (NeedData)
                            {
                                short[] numArray1 = new short[(Data.Length - num4) + text1.Length];
                                Array.Copy(Data, 0, numArray1, 0, num3);
                                Array.Copy(Data, (int) (num3 + num4), numArray1, (int) (num3 + text1.Length), (int) ((Data.Length - num4) - num3));
                                Data = numArray1;
                                StrItem.SetColorFlag(ref Data, num3, text1.Length, 4, true);
                            }
                        }
                        else
                        {
                            string text2 = string.Empty;
                            short[] numArray2 = null;
                            if (this.GetString(range1.EndPoint.Y, ref text2, ref numArray2, NeedData, false))
                            {
                                if (range1.EndPoint.X < text2.Length)
                                {
                                    Line = Line.Substring(0, Math.Min(num3, Line.Length)) + text1 + text2.Substring(range1.EndPoint.X);
                                }
                                else
                                {
                                    Line = Line.Substring(0, Math.Min(num3, Line.Length)) + text1;
                                }
                                if (NeedData)
                                {
                                    int num6 = Math.Max((int) (numArray2.Length - range1.EndPoint.X), 0);
                                    short[] numArray3 = new short[(num3 + text1.Length) + num6];
                                    Array.Copy(Data, 0, numArray3, 0, num3);
                                    if (num6 > 0)
                                    {
                                        Array.Copy(numArray2, range1.EndPoint.X, numArray3, (int) (num3 + text1.Length), num6);
                                    }
                                    Data = numArray3;
                                    StrItem.SetColorFlag(ref Data, num3, text1.Length, 4, true);
                                }
                            }
                        }
                    }
                }
                if (ApplyTabs)
                {
                    ((SyntaxStrings) this.lines).GetTabString(ref Line, ref Data, NeedData && (Data != null));
                }
            }
            else
            {
                this.GetString(Index, ref Line, ref Data, NeedData, ApplyTabs);
            }
        }

        protected internal bool GetString(int Index, ref string Line, ref short[] Data, bool NeedData, bool ApplyTabs)
        {
            if ((Index < 0) || (Index >= this.lines.Count))
            {
                return false;
            }
            StrItem item1 = this.lines.GetItem(Index);
            Line = item1.String;
            if (NeedData)
            {
                Data = item1.ColorData;
                this.ApplyWhiteSpace(ref Line, ref Data);
            }
            if (ApplyTabs)
            {
                ((SyntaxStrings) this.lines).GetTabString(ref Line, ref Data, NeedData);
            }
            return true;
        }

        private void GetSubString(int P, int Len, ref string Line, ref short[] Data, bool NeedData)
        {
            int num1 = Line.Length;
            P = Math.Min(P, num1);
            Len = Math.Min(Len, (int) (num1 - P));
            Line = Line.Substring(P, Len);
            if (NeedData)
            {
                short[] numArray1 = new short[Len];
                Array.Copy(Data, P, numArray1, 0, Len);
                Data = numArray1;
            }
        }

        public string GetTextAt(Point Position)
        {
            return this.GetTextAt(Position.Y, Position.X);
        }

        public string GetTextAt(int Line, int Pos)
        {
            int num1;
            int num2;
            string text1 = this[Line];
            if (this.GetWord(text1, Pos, out num1, out num2))
            {
                return text1.Substring(num1, (num2 - num1) + 1);
            }
            return string.Empty;
        }

        public bool GetWord(int Index, int Pos, out int Left, out int Right)
        {
            return this.GetWord(this[Index], Pos, out Left, out Right);
        }

        public bool GetWord(string String, int Pos, out int Left, out int Right)
        {
            return this.lines.GetWord(String, Pos, out Left, out Right);
        }

        private void GetWrapBounds(int Index, ref int P, ref int Len)
        {
            if (this.wordWrap)
            {
                this.wrapList.GetWrapBounds(Index, ref P, ref Len);
            }
        }

        public int GetWrapMargin()
        {
            return this.wrapMargin;
        }

        public bool IsCollapsed(int Index)
        {
            IOutlineRange range1 = this.GetOutlineRange(Index);
            if ((range1 != null) && (range1.StartPoint.Y == Index))
            {
                return !range1.Visible;
            }
            return false;
        }

        public bool IsDelimiter(char ch)
        {
            return this.lines.IsDelimiter(ch);
        }

        public bool IsDelimiter(int Index, int Pos)
        {
            return this.lines.IsDelimiter(this[Index], Pos);
        }

        public bool IsDelimiter(string String, int Pos)
        {
            return this.lines.IsDelimiter(String, Pos);
        }

        public bool IsExpanded(int Index)
        {
            IOutlineRange range1 = this.GetOutlineRange(Index);
            if (range1 != null)
            {
                return range1.Visible;
            }
            return false;
        }

        public bool IsVisible(Point Point)
        {
            return this.outlineList.IsVisible(Point);
        }

        public bool IsVisible(int Index)
        {
            return this.outlineList.IsVisible(Index);
        }

        protected void LinesChanged()
        {
            this.UpdateWordWrap();
            this.MaxLineChanged();
        }

        public void LoadFile(string FileName)
        {
            this.LoadFile(FileName, ExportFormat.Text, null);
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
            try
            {
				using (var stream1 = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					using (var reader1 = (Encoding != null) ? new StreamReader(stream1, Encoding) : new StreamReader(stream1))
					{
						this.LoadStream(reader1, Format);
						reader1.Close();
					}
					stream1.Close();
				}
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void LoadStream(TextReader Reader)
        {
            this.LoadStream(Reader, ExportFormat.Text);
        }

        public void LoadStream(TextReader Reader, ExportFormat Format)
        {
            var filer1 = new FmtFiler(this.owner, Format);
            filer1.Read(Reader);
        }

        protected internal void MaxLineChanged()
        {
            this.maxLineFlag = true;
        }

        private void MaxLineChanged(UpdateReason Reason, int X, int Y, int DeltaX, int DeltaY)
        {
            int num3;
            int num4;
            if (this.maxLineFlag || !this.NeedMaxLine())
            {
                return;
            }
            Point point1 = this.PointToDisplayPoint(X, Y);
            int num1 = point1.Y;
            switch (Reason)
            {
                case UpdateReason.Insert:
                {
                    int num2 = this.GetLineWidth(num1);
                    if (this.maxLineWidth < num2)
                    {
                        this.maxLineWidth = num2;
                        this.maxLineIndex = num1;
                    }
                    return;
                }
                case UpdateReason.Delete:
                {
                    if (num1 == this.maxLineIndex)
                    {
                        this.maxLineFlag = true;
                    }
                    return;
                }
                case UpdateReason.Break:
                {
                    if (num1 != this.maxLineIndex)
                    {
                        if (num1 < this.maxLineIndex)
                        {
                            this.maxLineIndex++;
                        }
                        return;
                    }
                    this.maxLineFlag = true;
                    return;
                }
                case UpdateReason.UnBreak:
                {
                    int num7 = this.GetLineWidth(num1);
                    if (this.maxLineWidth >= num7)
                    {
                        if (num1 < this.maxLineIndex)
                        {
                            this.maxLineIndex--;
                            if (this.maxLineIndex >= 0)
                            {
                                return;
                            }
                            this.maxLineFlag = true;
                        }
                        return;
                    }
                    this.maxLineWidth = num7;
                    this.maxLineIndex = num1;
                    return;
                }
                case UpdateReason.DeleteBlock:
                {
                    point1 = this.PointToDisplayPoint((int) (X - DeltaX), (int) (Y - DeltaY));
                    int num6 = point1.Y;
                    if ((this.maxLineIndex >= num1) && (this.maxLineIndex <= num6))
                    {
                        this.maxLineFlag = true;
                    }
                    return;
                }
                case UpdateReason.InsertBlock:
                {
                    point1 = this.PointToDisplayPoint((int) (X + DeltaX), (int) (Y + DeltaY));
                    num3 = point1.Y;
                    num4 = num1;
                    goto Label_00B1;
                }
                default:
                {
                    this.maxLineFlag = true;
                    return;
                }
            }
        Label_00B1:
            if (num4 > num3)
            {
                return;
            }
            int num5 = this.GetLineWidth(num4);
            if (this.maxLineWidth < num5)
            {
                this.maxLineWidth = num5;
                this.maxLineIndex = num1;
            }
            num4++;
            goto Label_00B1;
        }

        public bool MoveNext()
        {
            this.currentIndex++;
            return (this.currentIndex < this.GetCount());
        }

        private bool NeedMaxLine()
        {
            if (this.owner.WordWrap)
            {
                return false;
            }
            if (this.owner.Scrolling.ScrollBars != RichTextBoxScrollBars.Both)
            {
                return (this.owner.Scrolling.ScrollBars == RichTextBoxScrollBars.Horizontal);
            }
            return true;
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint)
        {
            return this.Outline(StartPoint, EndPoint, this.GetOutlineLevel(StartPoint), EditConsts.DefaultOutlineText);
        }

        public IOutlineRange Outline(int First, int Last)
        {
            return this.Outline(new Point(0, First), new Point(0x7fffffff, Last), this.GetOutlineLevel(new Point(0, First)), EditConsts.DefaultOutlineText);
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint, int Level)
        {
            return this.Outline(StartPoint, EndPoint, Level, EditConsts.DefaultOutlineText);
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint, string OutlineText)
        {
            return this.Outline(StartPoint, EndPoint, this.GetOutlineLevel(StartPoint), OutlineText);
        }

        public IOutlineRange Outline(int First, int Last, int Level)
        {
            return this.Outline(new Point(0, First), new Point(0x7fffffff, Last), Level, EditConsts.DefaultOutlineText);
        }

        public IOutlineRange Outline(int First, int Last, string OutlineText)
        {
            return this.Outline(new Point(0, First), new Point(0x7fffffff, Last), this.GetOutlineLevel(new Point(0, First)), OutlineText);
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint, int Level, string OutlineText)
        {
            if (this.allowOutlining)
            {
                IOutlineRange range1 = new OutRange(this.outlineList, StartPoint, EndPoint, Level, OutlineText);
                this.outlineList.Add(range1);
                return range1;
            }
            return null;
        }

        public IOutlineRange Outline(int First, int Last, int Level, string OutlineText)
        {
            return this.Outline(new Point(0, First), new Point(0x7fffffff, Last), Level, OutlineText);
        }

        public Point PointToDisplayPoint(Point Point)
        {
            return this.PointToDisplayPoint(Point.X, Point.Y, this.atLineEnd);
        }

        protected internal Point PointToDisplayPoint(Point Point, bool LineEnd)
        {
            return this.PointToDisplayPoint(Point.X, Point.Y, LineEnd);
        }

        public Point PointToDisplayPoint(int X, int Y)
        {
            return this.PointToDisplayPoint(X, Y, this.atLineEnd);
        }

        protected internal Point PointToDisplayPoint(int X, int Y, bool LineEnd)
        {
            Point point1 = new Point(X, Y);
            if (this.allowOutlining)
            {
                this.outlineList.GetDisplayPoint(ref point1);
            }
            if (point1.X != 0x7fffffff)
            {
                string text1 = string.Empty;
                short[] numArray1 = null;
                this.GetOutlineString(Y, ref text1, ref numArray1, false, false);
                point1.X = this.lines.TabPosToPos(text1, point1.X);
            }
            if (this.wordWrap)
            {
                this.wrapList.GetDisplayPoint(ref point1, LineEnd);
            }
            return point1;
        }

        protected internal void PositionChanged(UpdateReason Reason, int X, int Y, int DeltaX, int DeltaY)
        {
            if (this.allowOutlining)
            {
                this.outlineList.PositionChanged(X, Y, DeltaX, DeltaY);
            }
            this.MaxLineChanged(Reason, X, Y, DeltaX, DeltaY);
        }

        private void RecalcMaxLine()
        {
            int num1 = 0;
            this.maxLineWidth = 0;
            this.ScanToEnd(false);
            for (int num2 = 0; num2 < this.GetCount(); num2++)
            {
                num1 = this.GetLineWidth(num2);
                if (this.maxLineWidth < num1)
                {
                    this.maxLineWidth = num1;
                    this.maxLineIndex = num2;
                }
            }
            this.maxLineFlag = false;
        }

        public void Reset()
        {
            this.currentIndex = 0;
        }

        public virtual void ResetAllowOutlining()
        {
            this.AllowOutlining = false;
        }

        public virtual void ResetDelimiters()
        {
            this.Delimiters = EditConsts.DefaultDelimiters.ToCharArray();
        }

        public void ResetOutlineOptions()
        {
            this.OutlineOptions = EditConsts.DefaultOutlineOptions;
        }

        public virtual void ResetTabStops()
        {
            int[] numArray1 = new int[1] { EditConsts.DefaultTabStop } ;
            this.TabStops = numArray1;
        }

        public virtual void ResetUseSpaces()
        {
            this.UseSpaces = false;
        }

        public virtual void ResetWordWrap()
        {
            this.WordWrap = false;
        }

        public virtual void ResetWrapAtMargin()
        {
            this.WrapAtMargin = false;
        }

        public void SaveFile(string FileName)
        {
            this.SaveFile(FileName, ExportFormat.Text, null);
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
            try
            {
                Stream stream1 = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    StreamWriter writer1 = (Encoding != null) ? new StreamWriter(stream1, Encoding) : new StreamWriter(stream1);
                    try
                    {
                        this.SaveStream(writer1, Format);
                    }
                    finally
                    {
                        writer1.Close();
                    }
                }
                finally
                {
                    stream1.Close();
                }
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void SaveStream(TextWriter Writer)
        {
            this.SaveStream(Writer, ExportFormat.Text);
        }

        public void SaveStream(TextWriter Writer, ExportFormat Format)
        {
            FmtFiler filer1 = new FmtFiler(this.owner, Format);
            filer1.Write(Writer);
        }

        protected internal void ScanToEnd(bool ParseToEnd)
        {
            if (this.wordWrap)
            {
                this.EnsureWrapped(this.lines.Count - 1, false);
            }
            if (ParseToEnd)
            {
                this.owner.Source.ParseToString(this.owner.Lines.Count - 1);
            }
        }

        public void SetOutlineRanges(IList Ranges)
        {
            this.SetOutlineRanges(Ranges, false);
        }

        public void SetOutlineRanges(IList Ranges, bool PreserveVisible)
        {
            this.outlineList.BeginUpdate();
            try
            {
                ArrayList list1 = new ArrayList();
                foreach (IOutlineRange range1 in Ranges)
                {
                    bool flag1 = range1.Visible;
                    if (PreserveVisible)
                    {
                        if (flag1)
                        {
                            flag1 = this.outlineList.FindCollapsedRange(range1.StartPoint) == null;
                        }
                        else
                        {
                            IRange range2 = this.outlineList.FindExactRange(range1.StartPoint);
                            flag1 = (range2 != null) && ((IOutlineRange) range2).Visible;
                        }
                    }
                    list1.Add(new OutRange(this.outlineList, range1.StartPoint, range1.EndPoint, range1.Level, range1.DisplayText, flag1));
                }
                this.outlineList.Clear();
                foreach (IOutlineRange range3 in list1)
                {
                    this.outlineList.Add(range3);
                }
            }
            finally
            {
                this.outlineList.EndUpdate();
            }
        }

        public void ToggleOutlining()
        {
            this.ToggleOutlining(this.outlineList.GetRanges(), this.GetOutlineRange(this.owner.Position));
        }

        public void ToggleOutlining(IList Ranges, IOutlineRange Range)
        {
            if (Ranges.Count != 0)
            {
                if (Range == null)
                {
                    Range = (IOutlineRange) Ranges[0];
                }
                if (Range.Visible)
                {
                    this.FullCollapse(Ranges);
                }
                else
                {
                    this.FullExpand(Ranges);
                }
            }
        }

        public void UnOutline()
        {
            this.outlineList.Clear();
        }

        public void UnOutline(Point Point)
        {
            this.outlineList.RemoveRange(Point);
        }

        public void UnOutline(int Index)
        {
            this.outlineList.RemoveRange(Index);
        }

        private void UnWrapLines(int First, int Last)
        {
            this.ClearLastLine();
            if ((First == 0) && (Last == 0x7fffffff))
            {
                this.wrapList.Clear();
            }
            else
            {
                if (this.allowOutlining)
                {
                    First = this.outlineList.LineToDisplayLine(First);
                    Last = this.outlineList.LineToDisplayLine(Last);
                }
                this.wrapList.ClearLines(First, Last);
            }
        }

        public bool UpdateWordWrap()
        {
            return this.UpdateWordWrap(0, 0x7fffffff);
        }

        public bool UpdateWordWrap(int First, int Last)
        {
            bool flag1;
            if (this.wordWrap)
            {
                int num1 = this.wrapList.Count;
                int num2 = (First > 0) ? (this.PointToDisplayPoint(0x7fffffff, (int) (First - 1)).Y + 1) : 0;
                this.UnWrapLines(First, Last);
                this.WrapLines(First, Last, num2);
                flag1 = (First != Last) || (this.wrapList.Count != num1);
            }
            else
            {
                flag1 = this.wrapList.Count != 0;
                this.UnWrapLines(0, 0x7fffffff);
            }
            if (flag1)
            {
                Last = 0x7fffffff;
            }
            this.WordWrapUpdated(flag1, First, Last);
            return flag1;
        }

        private void WordWrapUpdated(bool AUpdate, int First, int Last)
        {
            this.owner.Notification(this, new NotifyEventArgs(NotifyState.WordWrap, First, Last, AUpdate));
        }

        protected virtual bool WrapLine(int Index, IEditPages pages, ref int page, int DisplayIndex, ref int DisplayLine, ref int Margin, bool NeedData)
        {
            if (this.allowOutlining && !this.IsVisible(Index))
            {
                return false;
            }
            string text1 = string.Empty;
            short[] numArray1 = null;
            this.GetOutlineString(Index, ref text1, ref numArray1, NeedData, true);
            int num1 = 0;
            int num2 = text1.Length;
            if (num2 != 0)
            {
                while (num1 < num2)
                {
                    int num3;
                    if (page >= 0)
                    {
                        this.CheckPage(pages, ref page, DisplayLine, ref Margin);
                    }
                    this.owner.MeasureLine(text1, ref numArray1, num1, -1, Margin, out num3);
                    if ((num1 + num3) < num2)
                    {
                        for (int num4 = num3; num4 > 0; num4--)
                        {
                            if (this.IsDelimiter(text1, (int) ((num1 + num4) - 1)))
                            {
                                num3 = num4;
                                break;
                            }
                        }
                    }
                    if (num3 <= 0)
                    {
                        num3 = 1;
                    }
                    num1 += num3;
                    if (num1 < num2)
                    {
                        this.wrapList.AddItem(DisplayIndex, num1);
                    }
                    DisplayLine += 1;
                }
            }
            else
            {
                if (page >= 0)
                {
                    this.CheckPage(pages, ref page, DisplayLine, ref Margin);
                }
                DisplayLine += 1;
            }
            return true;
        }

        private void WrapLines(int First, int Last, int DisplayLine)
        {
            this.wrapMargin = this.WrapMargin;
            bool flag1 = (this.owner.Source.Lexer != null) && !this.owner.Painter.IsMonoSpaced;
            if (flag1)
            {
                this.owner.Source.ParseToString(Last);
            }
            int num1 = Math.Max(First, 0);
            int num2 = Math.Min(Last, (int) (this.lines.Count - 1));
            int num3 = num1;
            if (this.allowOutlining)
            {
                this.outlineList.CheckVisible(ref num1);
                num3 = this.outlineList.LineToDisplayLine(num1);
            }
            int num4 = 0;
            int num5 = this.owner.ClientRect.Height;
            int num6 = -1;
            IEditPages pages1 = this.owner.Pages;
            if (pages1.PageType != PageType.Normal)
            {
                if (num1 > 0)
                {
                    num6 = pages1.GetPageAt(0, DisplayLine).Index;
                }
                else
                {
                    num6 = 0;
                }
                if (!this.wrapAtMargin)
                {
                    this.wrapMargin = ((EditPage) pages1[num6]).DisplayWidth;
                }
            }
            else
            {
                int num7 = this.owner.Painter.FontHeight;
                num4 = (this.owner.Scrolling.WindowOriginY + ((num7 != 0) ? (this.owner.Height / num7) : 0)) + EditConsts.DefaultWrapDelta;
            }
            for (int num8 = num1; num8 <= num2; num8++)
            {
                if (this.WrapLine(num8, pages1, ref num6, num3, ref DisplayLine, ref this.wrapMargin, flag1))
                {
                    num3++;
                    this.lastWrapped = num8;
                    this.lastDisplayIndex = num3;
                    if (Last == 0x7fffffff)
                    {
                        if (pages1.PageType != PageType.Normal)
                        {
                            if (pages1[num6].BoundsRect.Top <= num5)
                            {
                                goto Label_019A;
                            }
                            break;
                        }
                        if (num3 >= num4)
                        {
                            break;
                        }
                    }
                }
            Label_019A:;
            }
            this.lastWrapIndex = this.wrapList.LineToDisplayLine(this.lastDisplayIndex);
        }


        // Properties
        public bool AllowOutlining
        {
            get
            {
                return this.allowOutlining;
            }
            set
            {
                if (this.allowOutlining != value)
                {
                    this.allowOutlining = value;
                    if (!value)
                    {
                        this.UnOutline();
                    }
                    else
                    {
                        this.owner.Outlining.OutlineText();
                    }
                }
                if (this.owner != null)
                {
                    this.owner.Invalidate();
                }
            }
        }

        protected internal bool AtLineEnd
        {
            get
            {
                return this.atLineEnd;
            }
            set
            {
                this.atLineEnd = value;
            }
        }

        public int Count
        {
            get
            {
                this.ScanToEnd(false);
                return this.GetCount();
            }
        }

        public object Current
        {
            get
            {
                if ((this.currentIndex >= 0) && (this.currentIndex < this.GetCount()))
                {
                    return this[this.currentIndex];
                }
                return null;
            }
        }

        public char[] Delimiters
        {
            get
            {
                return this.lines.Delimiters;
            }
            set
            {
                this.lines.Delimiters = value;
            }
        }

        public string DelimiterString
        {
            get
            {
                return this.lines.DelimiterString;
            }
            set
            {
                this.lines.DelimiterString = value;
            }
        }

        public Hashtable DelimTable
        {
            get
            {
                return this.lines.DelimTable;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this.lines.IsSynchronized;
            }
        }

        public string this[int index]
        {
            get
            {
                string text1 = string.Empty;
                short[] numArray1 = null;
                this.GetDisplayString(index, ref text1, ref numArray1, false);
                return text1;
            }
        }

        protected internal int LineEndUpdateCount
        {
            get
            {
                return this.lineEndUpdateCount;
            }
            set
            {
                this.lineEndUpdateCount = value;
            }
        }

        public ISyntaxStrings Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                if (this.lines != value)
                {
                    this.lines = value;
                    this.LinesChanged();
                }
            }
        }

        public int MaxLineWidth
        {
            get
            {
                if (this.NeedMaxLine() && this.maxLineFlag)
                {
                    this.RecalcMaxLine();
                }
                return this.maxLineWidth;
            }
        }

        public River.Orqa.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                return this.outlineOptions;
            }
            set
            {
                if (this.outlineOptions != value)
                {
                    this.outlineList.BeginUpdate();
                    try
                    {
                        this.outlineOptions = value;
                    }
                    finally
                    {
                        this.outlineList.EndUpdate();
                    }
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.lines.SyncRoot;
            }
        }

        public int[] TabStops
        {
            get
            {
                return this.lines.TabStops;
            }
            set
            {
                this.lines.TabStops = value;
            }
        }

        public bool UseSpaces
        {
            get
            {
                return this.lines.UseSpaces;
            }
            set
            {
                this.lines.UseSpaces = value;
            }
        }

        public bool WordWrap
        {
            get
            {
                return this.wordWrap;
            }
            set
            {
                if (this.wordWrap != value)
                {
                    this.wordWrap = value;
                    this.UpdateWordWrap();
                }
            }
        }

        public bool WrapAtMargin
        {
            get
            {
                return this.wrapAtMargin;
            }
            set
            {
                if (this.wrapAtMargin != value)
                {
                    this.wrapAtMargin = value;
                    if (this.wordWrap)
                    {
                        this.UpdateWordWrap();
                    }
                }
            }
        }

        public int WrapMargin
        {
            get
            {
                if (this.wrapAtMargin)
                {
                    return (this.owner.Margin.Position * this.owner.Painter.FontWidth);
                }
                return (this.owner.ClientRect.Width - ((Gutter) this.owner.Gutter).GetWidth());
            }
        }

        public object XmlInfo
        {
            get
            {
                return new XmlDisplayStringsInfo(this);
            }
            set
            {
                ((XmlDisplayStringsInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private bool allowOutlining;
        private bool atLineEnd;
        private ArrayList collapsedList;
        private int currentIndex;
        private int lastDisplayIndex;
        private int lastWrapIndex;
        private int lastWrapped;
        private int lineEndUpdateCount;
        private ISyntaxStrings lines;
        private bool maxLineFlag;
        private int maxLineIndex;
        private int maxLineWidth;
        private OutlineList outlineList;
        private River.Orqa.Editor.OutlineOptions outlineOptions;
        private ISyntaxEdit owner;
        private char[] tabArray;
        private bool wordWrap;
        private bool wrapAtMargin;
        private WrapList wrapList;
        private int wrapMargin;

        // Nested Types
        private class DisplayWrapComparer : IComparer
        {
            // Methods
            public DisplayWrapComparer(SortList List)
            {
                this.list = List;
            }

            public int Compare(object x, object y)
            {
                Point point1 = (Point) x;
                return Math.Max((int) (((point1.Y + this.list.CompareIndex) + 1) - ((int) y)), 0);
            }


            // Fields
            private SortList list;
        }

        private class LineWrapComparer : IComparer
        {
            // Methods
            public LineWrapComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = (Point) x;
                return (point1.Y - ((int) y));
            }

        }

        private class RealWrapComparer : IComparer
        {
            // Methods
            public RealWrapComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = (Point) x;
                Point point2 = (Point) y;
                int num1 = point1.Y - point2.Y;
                if (num1 == 0)
                {
                    return Math.Max((int) (point1.X - point2.X), 0);
                }
                return Math.Max(num1, 0);
            }

        }

        private class WrapComparer : IComparer
        {
            // Methods
            public WrapComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = (Point) x;
                Point point2 = (Point) y;
                int num1 = point1.Y - point2.Y;
                if (num1 == 0)
                {
                    num1 = point1.X - point2.X;
                }
                return num1;
            }

        }

        private class WrapList : SortList
        {
            // Methods
            public WrapList(DisplayStrings Owner)
            {
                this.owner = Owner;
                this.wrapComparer = new DisplayStrings.WrapComparer();
                this.lineWrapComparer = new DisplayStrings.LineWrapComparer();
                this.realWrapComparer = new DisplayStrings.RealWrapComparer();
                this.displayWrapComparer = new DisplayStrings.DisplayWrapComparer(this);
            }

            public Point AddItem(int Line, int Char)
            {
                int num1;
                Point point1 = new Point(Char, Line);
                if (base.FindExact(point1, out num1, this.wrapComparer))
                {
                    this[num1] = point1;
                    return point1;
                }
                this.Insert(num1, point1);
                return point1;
            }

            public void ClearLines(int First, int Last)
            {
                int num1;
                base.FindLast(Last, out num1, this.lineWrapComparer);
                for (int num3 = Math.Min(num1, (int) (this.Count - 1)); num3 >= 0; num3--)
                {
                    Point point1 = (Point) this[num3];
                    int num2 = point1.Y;
                    if ((num2 >= First) && (num2 <= Last))
                    {
                        this.RemoveAt(num3);
                    }
                    else if (num2 < First)
                    {
                        return;
                    }
                }
            }

            public int DisplayLineToLine(int Index)
            {
                int num1;
                if (base.FindLast(Index, out num1, this.displayWrapComparer))
                {
                    return (Index - (num1 + 1));
                }
                return Index;
            }

            public void GetDisplayPoint(ref Point Point, bool LineEnd)
            {
                int num1;
                this.owner.EnsureWrapped(Point.Y, false);
                if (base.FindLast((Point) Point, out num1, this.realWrapComparer))
                {
                    Point point1 = (Point) this[num1];
                    if ((LineEnd && (point1.X == Point.X)) && (point1.Y == Point.Y))
                    {
                        if (num1 > 0)
                        {
                            point1 = (Point) this[num1 - 1];
                            if (point1.Y == Point.Y)
                            {
                                Point.X -= point1.X;
                            }
                        }
                        Point.Y += num1;
                    }
                    else
                    {
                        if ((point1.Y == Point.Y) && (Point.X != 0x7fffffff))
                        {
                            Point.X -= point1.X;
                        }
                        Point.Y += (num1 + 1);
                    }
                }
            }

            private void GetPosAndLen(int Index, int idx, ref int P, ref int Len)
            {
                if (idx >= 0)
                {
                    Point point1 = (Point) this[idx];
                    if (point1.Y == ((Index - idx) - 1))
                    {
                        P = point1.X;
                    }
                }
                if ((idx + 1) < this.Count)
                {
                    Point point2 = (Point) this[idx + 1];
                    if (point2.Y == ((Index - idx) - 1))
                    {
                        Len = point2.X - P;
                    }
                }
            }

            public void GetRealPoint(ref Point Point, bool CheckEnd, ref bool LineEnd)
            {
                int num1;
                this.owner.EnsureWrapped(Point.Y, true);
                int num2 = Point.Y;
                if (base.FindLast(Point.Y, out num1, this.displayWrapComparer))
                {
                    Point point1 = (Point) this[num1];
                    Point.Y -= (num1 + 1);
                    if ((point1.Y == Point.Y) && (Point.X != 0x7fffffff))
                    {
                        Point.X += point1.X;
                    }
                }
                else
                {
                    num1 = -1;
                }
                LineEnd = false;
                int num3 = 0;
                int num4 = 0x7fffffff;
                this.GetPosAndLen(num2, num1, ref num3, ref num4);
                if (num4 != 0x7fffffff)
                {
                    LineEnd = Point.X >= (num3 + num4);
                    if (CheckEnd && LineEnd)
                    {
                        Point.X = num3 + num4;
                    }
                }
            }

            public void GetWrapBounds(int Index, ref int P, ref int Len)
            {
                if (this.Count > 0)
                {
                    int num1;
                    if (!base.FindLast(Index, out num1, this.displayWrapComparer))
                    {
                        num1 = -1;
                    }
                    this.GetPosAndLen(Index, num1, ref P, ref Len);
                }
            }

            public int LineToDisplayLine(int Index)
            {
                int num1;
                Point point1 = new Point(0x7fffffff, Index);
                if (base.FindLast(point1, out num1, this.realWrapComparer))
                {
                    return ((Index + num1) + 1);
                }
                return Index;
            }


            // Fields
            private IComparer displayWrapComparer;
            private IComparer lineWrapComparer;
            private DisplayStrings owner;
            private IComparer realWrapComparer;
            private IComparer wrapComparer;
        }
    }
}

