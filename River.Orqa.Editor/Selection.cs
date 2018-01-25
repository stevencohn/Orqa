namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class Selection : ISelection
    {
        // Events
        [Browsable(false)]
        public event EventHandler SelectionChanged;

        // Methods
        public Selection()
        {
            this.options = EditConsts.DefaultSelectionOptions;
            this.allowedSelectionMode = EditConsts.DefaultSelectionMode;
            this.foreColor = EditConsts.DefaultHighlightForeColor;
            this.backColor = EditConsts.DefaultHighlightBackColor;
            this.inActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor;
            this.inActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor;
            this.oldSelectionRect = Rectangle.Empty;
            this.oldSelectionType = River.Orqa.Editor.SelectionType.None;
            this.selStart = new Point(0, 0);
            this.selEnd = new Point(0, 0);
            this.moveSelection = new KeyEvent(this.MoveSelection);
            this.tabifyLineEvent = new StringEvent(this.TabifyLine);
            this.unTabifyLineEvent = new StringEvent(this.UnTabifyLine);
            this.indentLineEvent = new StringEvent(this.IndentLine);
            this.unIndentLineEvent = new StringEvent(this.UnIndentLine);
            this.lowerCaseLineEvent = new StringEvent(this.LowerCaseLine);
            this.upperCaseLineEvent = new StringEvent(this.UpperCaseLine);
            this.capitalizeLineEvent = new StringEvent(this.CapitalizeLine);
            this.deleteWhiteSpaceEvent = new StringEvent(this.DeleteWhiteSpace);
            this.selTimer = new Timer();
            this.selTimer.Enabled = false;
            this.selTimer.Interval = EditConsts.DefaultSelDelay;
            this.selTimer.Tick += new EventHandler(this.OnSelect);
        }

        public Selection(ISyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        public void Assign(ISelection Source)
        {
            this.Options = Source.Options;
            this.BackColor = Source.BackColor;
            this.ForeColor = Source.ForeColor;
            this.InActiveBackColor = Source.InActiveBackColor;
            this.InActiveForeColor = Source.InActiveForeColor;
            this.SetSelection(Source.SelectionType, Source.SelectionRect);
        }

        public int BeginUpdate()
        {
            if (this.updateCount == 0)
            {
                this.oldSelectionRect = this.selectionRect;
                this.oldSelectionType = this.selectionType;
            }
            this.updateCount++;
            return this.updateCount;
        }

        public bool CanCopy()
        {
            return !this.IsEmpty();
        }

        public bool CanCut()
        {
            if (!this.owner.Source.ReadOnly)
            {
                return !this.IsEmpty();
            }
            return false;
        }

        public bool CanDrag(Point Position)
        {
            if (!this.owner.Source.ReadOnly && ((SelectionOptions.DisableDragging & this.options) == SelectionOptions.None))
            {
                return !this.IsPosInSelection(Position);
            }
            return false;
        }

        public bool CanPaste()
        {
            IDataObject obj1 = Clipboard.GetDataObject();
            if ((obj1 != null) && obj1.GetDataPresent(DataFormats.UnicodeText))
            {
                return true;
            }
            return Clipboard.GetDataObject().GetDataPresent(DataFormats.Text);
        }

        protected bool CanSelectBlock()
        {
            return ((Control.ModifierKeys & Keys.Alt) != Keys.None);
        }

        public void Capitalize()
        {
            this.ChangeBlock(this.capitalizeLineEvent, true, false);
        }

        private string CapitalizeLine(string String)
        {
            char[] chArray1 = String.ToLower().ToCharArray();
            ISyntaxStrings strings1 = this.owner.Lines;
            for (int num1 = 0; num1 < chArray1.Length; num1++)
            {
                bool flag1 = !strings1.IsDelimiter(chArray1[num1]);
                if (num1 != 0)
                {
                    flag1 = flag1 && strings1.IsDelimiter(chArray1[num1 - 1]);
                }
                if (flag1)
                {
                    chArray1[num1] = char.ToUpper(chArray1[num1]);
                }
            }
            return new string(chArray1);
        }

        public void ChangeBlock(StringEvent Action)
        {
            this.ChangeBlock(Action, false, false);
        }

        public void ChangeBlock(StringEvent Action, bool ChangeIfEmpty, bool ExtendFirstLine)
        {
            if (ChangeIfEmpty || !this.IsEmpty())
            {
                string text1;
                ITextSource source1 = this.owner.Source;
                if (this.IsEmpty())
                {
                    text1 = source1.Lines[source1.Position.Y];
                    if (source1.Position.X < text1.Length)
                    {
                        char ch1 = text1[source1.Position.X];
                        source1.BeginUpdate(UpdateReason.Other);
                        try
                        {
                            source1.DeleteRight(1);
                            source1.Insert(Action(ch1.ToString()));
                        }
                        finally
                        {
                            source1.EndUpdate();
                        }
                    }
                }
                else
                {
                    Rectangle rectangle1;
                    int num1 = 0;
                    if (ExtendFirstLine)
                    {
                        rectangle1 = this.SelectionRect;
                        num1 = rectangle1.Left;
                        this.SelectionRect = new Rectangle(0, rectangle1.Top, rectangle1.Right, rectangle1.Height);
                    }
                    StringBuilder builder1 = new StringBuilder();
                    int num2 = this.SelectedCount();
                    for (int num3 = 0; num3 < num2; num3++)
                    {
                        string text2;
                        text1 = this.SelectedString(num3);
                        if (((num3 == (num2 - 1)) && (this.selectionType == River.Orqa.Editor.SelectionType.Stream)) && (this.selectionRect.Right == 0))
                        {
                            text2 = text1;
                        }
                        else
                        {
                            text2 = Action(text1);
                        }
                        if (ExtendFirstLine && (num3 == 0))
                        {
                            num1 += (text2.Length - text1.Length);
                        }
                        builder1.Append(text2);
                        if (num3 < (num2 - 1))
                        {
                            builder1.Append(Consts.CRLF);
                        }
                    }
                    this.SetSelectedText(builder1.ToString(), (this.SelectionType != River.Orqa.Editor.SelectionType.None) ? this.SelectionType : River.Orqa.Editor.SelectionType.Stream);
                    if (ExtendFirstLine)
                    {
                        rectangle1 = this.SelectionRect;
                        this.SetSelection(this.SelectionType, new Point(Math.Max((int) (rectangle1.Left + num1), 0), rectangle1.Top), new Point(rectangle1.Right, rectangle1.Bottom));
                    }
                }
            }
        }

        public void CharTransponse()
        {
            Point point1 = this.owner.Position;
            string text1 = this.owner.Lines[point1.Y];
            if (text1.Length >= 2)
            {
                point1.X = Math.Max(Math.Min(point1.X, (int) (text1.Length - 1)), 1);
                ITextSource source1 = this.owner.Source;
                source1.BeginUpdate(UpdateReason.Insert);
                try
                {
                    char ch1 = text1[point1.X];
                    source1.MoveToChar(point1.X);
                    source1.DeleteRight(1);
                    this.owner.MoveToChar(point1.X - 1);
                    source1.Insert(ch1.ToString());
                    source1.MoveToChar(point1.X + 1);
                }
                finally
                {
                    source1.EndUpdate();
                }
            }
        }

        private void CheckSelectionMode(ref River.Orqa.Editor.SelectionType SelType)
        {
            if (this.allowedSelectionMode == River.Orqa.Editor.AllowedSelectionMode.None)
            {
                SelType = River.Orqa.Editor.SelectionType.None;
            }
            else
            {
                switch (SelType)
                {
                    case River.Orqa.Editor.SelectionType.Stream:
                    {
                        if ((this.allowedSelectionMode & River.Orqa.Editor.AllowedSelectionMode.Stream) == River.Orqa.Editor.AllowedSelectionMode.None)
                        {
                            SelType = River.Orqa.Editor.SelectionType.Block;
                        }
                        return;
                    }
                    case River.Orqa.Editor.SelectionType.Block:
                    {
                        if ((this.allowedSelectionMode & River.Orqa.Editor.AllowedSelectionMode.Block) == River.Orqa.Editor.AllowedSelectionMode.None)
                        {
                            SelType = River.Orqa.Editor.SelectionType.Stream;
                        }
                        return;
                    }
                }
            }
        }

        public void Clear()
        {
            this.SelectionType = River.Orqa.Editor.SelectionType.None;
        }

        public void Copy()
        {
            this.WriteToClipboard();
            if ((this.options & SelectionOptions.DeselectOnCopy) != SelectionOptions.None)
            {
                this.Clear();
            }
        }

        public void Cut()
        {
            this.WriteToClipboard();
            this.Delete();
        }

        public void CutLine()
        {
            if (this.IsEmpty())
            {
                this.SelectLine();
            }
            this.Cut();
        }

        public void Delete()
        {
            if (!this.owner.ReadOnly)
            {
                ITextSource source1 = this.owner.Source;
                source1.BeginUpdate(UpdateReason.Delete);
                try
                {
                    this.BeginUpdate();
                    bool flag1 = this.IsEmpty();
                    try
                    {
                        if (this.selectionType == River.Orqa.Editor.SelectionType.Stream)
                        {
                            ((DisplayStrings) this.owner.DisplayLines).BlockDeleting(this.SelectionRect);
                            source1.DeleteBlock(this.SelectionRect);
                        }
                        else if (!flag1)
                        {
                            ArrayList list1 = new ArrayList();
                            for (int num1 = this.selectionRect.Bottom; num1 >= this.selectionRect.Top; num1--)
                            {
                                this.GetSelectedBounds(num1, list1);
                            }
                            for (int num2 = list1.Count - 1; num2 >= 0; num2--)
                            {
                                IRange range1 = (IRange) list1[num2];
                                source1.MoveTo(range1.StartPoint.X, range1.StartPoint.Y);
                                source1.DeleteRight(range1.EndPoint.X - range1.StartPoint.X);
                                if ((num2 != (list1.Count - 1)) && (this.selectionType != River.Orqa.Editor.SelectionType.Block))
                                {
                                    source1.UnBreakLine();
                                }
                            }
                        }
                    }
                    finally
                    {
                        source1.EndUpdate();
                    }
                    if (!flag1)
                    {
                        this.Clear();
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void DeleteLeft()
        {
            this.DeleteLeft(false);
        }

        protected internal void DeleteLeft(bool DeleteWord)
        {
            if (this.ShouldDeleteBlock())
            {
                this.Delete();
            }
            else
            {
                if (this.owner.Position.X == 0)
                {
                    if (this.owner.Position.Y <= 0)
                    {
                        return;
                    }
                    this.owner.Source.BeginUpdate(UpdateReason.UnBreak);
                    try
                    {
                        this.owner.MoveTo(this.owner.Lines.GetLength(this.owner.Position.Y - 1), this.owner.Position.Y - 1);
                        this.owner.Source.UnBreakLine();
                        return;
                    }
                    finally
                    {
                        this.owner.Source.EndUpdate();
                    }
                }
                if (DeleteWord)
                {
                    this.BeginUpdate();
                    this.owner.Source.BeginUpdate(UpdateReason.Delete);
                    try
                    {
                        this.SelectWordLeft();
                        this.Delete();
                        return;
                    }
                    finally
                    {
                        this.owner.Source.EndUpdate();
                        this.EndUpdate();
                    }
                }
                this.owner.Source.DeleteLeft(1);
            }
        }

        public void DeleteLine()
        {
            if (this.IsEmpty())
            {
                this.SelectLine();
            }
            this.Delete();
        }

        public void DeleteRight()
        {
            this.DeleteRight(false);
        }

        protected internal void DeleteRight(bool DeleteWord)
        {
            if (this.ShouldDeleteBlock())
            {
                this.Delete();
            }
            else if (this.owner.Position.X >= this.owner.Lines.GetLength(this.owner.Position.Y))
            {
                this.owner.Source.UnBreakLine();
            }
            else
            {
                if (DeleteWord)
                {
                    this.owner.Source.BeginUpdate(UpdateReason.Delete);
                    this.BeginUpdate();
                    try
                    {
                        this.SelectWordRight();
                        this.Delete();
                        return;
                    }
                    finally
                    {
                        this.EndUpdate();
                        this.owner.Source.EndUpdate();
                    }
                }
                this.owner.Source.DeleteRight(1);
            }
        }

        public void DeleteWhiteSpace()
        {
            if (this.IsEmpty())
            {
                string text1 = this.owner.Lines[this.owner.Position.Y];
                if ((this.owner.Position.X >= text1.Length) || (this.owner.Position.X > (text1.Length - text1.TrimStart(new char[0]).Length)))
                {
                    return;
                }
                this.SelectLineBegin();
                this.ChangeBlock(this.deleteWhiteSpaceEvent);
                this.Clear();
            }
            else
            {
                this.ChangeBlock(this.deleteWhiteSpaceEvent);
            }
        }

        private string DeleteWhiteSpace(string String)
        {
            string text1 = string.Empty;
            string text2 = String;
            for (int num1 = 0; num1 < text2.Length; num1++)
            {
                char ch1 = text2[num1];
                if ((ch1 != ' ') && (ch1 != '\t'))
                {
                    text1 = text1 + ch1;
                }
            }
            return text1;
        }

        public void DeleteWordLeft()
        {
            this.DeleteLeft(true);
        }

        public void DeleteWordRight()
        {
            this.DeleteRight(true);
        }

        protected internal bool DoDragScroll(Point Pt)
        {
            Rectangle rectangle1 = this.owner.ClientRect;
            int num1 = this.owner.Scrolling.WindowOriginX;
            int num2 = this.owner.Scrolling.WindowOriginY;
            bool flag1 = this.owner.Pages.PageType == PageType.PageLayout;
            int num3 = this.owner.Painter.FontHeight;
            int num4 = this.owner.Painter.FontWidth;
            if ((Pt.Y - num3) < rectangle1.Top)
            {
                IScrolling scrolling1 = this.owner.Scrolling;
                scrolling1.WindowOriginY -= (flag1 ? num3 : 1);
            }
            else if ((Pt.Y + num3) > rectangle1.Bottom)
            {
                IScrolling scrolling2 = this.owner.Scrolling;
                scrolling2.WindowOriginY += (flag1 ? num3 : 1);
            }
            else if ((Pt.X - num4) < rectangle1.Left)
            {
                IScrolling scrolling3 = this.owner.Scrolling;
                scrolling3.WindowOriginX -= (flag1 ? num4 : 1);
            }
            else if ((Pt.X + num4) > rectangle1.Right)
            {
                IScrolling scrolling4 = this.owner.Scrolling;
                scrolling4.WindowOriginX += (flag1 ? num4 : 1);
            }
            if (num1 == this.owner.Scrolling.WindowOriginX)
            {
                return (num2 != this.owner.Scrolling.WindowOriginY);
            }
            return true;
        }

        private int DoInsertString(River.Orqa.Editor.SelectionType SelType, string s, bool InsertLine)
        {
            if (InsertLine)
            {
                if (SelType == River.Orqa.Editor.SelectionType.Block)
                {
                    this.owner.MoveLineDown();
                }
                else
                {
                    this.owner.Source.BreakLine();
                    this.owner.MoveTo(0, this.owner.Position.Y + 1);
                }
            }
            this.owner.Source.Insert(s);
            return s.Length;
        }

        protected void DoSelectionChanged()
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, EventArgs.Empty);
            }
        }

        public void DragTo(Point Position, bool DeleteOrigin)
        {
            this.Move(Position, DeleteOrigin);
        }

        protected internal void EndSelection()
        {
            if (this.selectionState != River.Orqa.Editor.SelectionState.None)
            {
                this.selTimer.Stop();
                this.selectionState = River.Orqa.Editor.SelectionState.None;
                this.selForward = false;
            }
        }

        public int EndUpdate()
        {
            this.updateCount--;
            if ((this.updateCount == 0) && (!this.oldSelectionRect.Equals(this.selectionRect) || (this.oldSelectionType != this.selectionType)))
            {
                this.DoSelectionChanged();
            }
            return this.updateCount;
        }

        ~Selection()
        {
            this.selTimer.Dispose();
        }

        private EventHandlers GetHandlers()
        {
            return ((River.Orqa.Editor.KeyList) this.owner.KeyList).Handlers;
        }

        private int GetSelectedBounds(int Index, ArrayList List)
        {
            int num1;
            int num2;
            switch (this.selectionType)
            {
                case River.Orqa.Editor.SelectionType.Stream:
                {
                    if ((Index > this.selectionRect.Bottom) || (Index < this.selectionRect.Top))
                    {
                        goto Label_01BA;
                    }
                    if (Index != this.selectionRect.Top)
                    {
                        if (Index == this.selectionRect.Bottom)
                        {
                            num1 = 0;
                            num2 = this.selectionRect.Right;
                        }
                        else
                        {
                            num1 = 0;
                            num2 = 0x7fffffff;
                        }
                        break;
                    }
                    if (Index != this.selectionRect.Bottom)
                    {
                        num1 = this.selectionRect.Left;
                        num2 = 0x7fffffff;
                        break;
                    }
                    num1 = this.selectionRect.Left;
                    num2 = this.SelectionRect.Right;
                    break;
                }
                case River.Orqa.Editor.SelectionType.Block:
                {
                    IDisplayStrings strings1 = this.owner.DisplayLines;
                    Point point3 = strings1.PointToDisplayPoint(0, Index);
                    int num3 = point3.Y;
                Label_00E7:
                    point3 = strings1.PointToDisplayPoint(this.owner.Lines.GetLength(Index), Index);
                    if (num3 <= point3.Y)
                    {
                        if (this.GetSelectionForLine(num3, out num1, out num2, false))
                        {
                            Point point1 = strings1.DisplayPointToPoint(num1, num3);
                            Point point2 = strings1.DisplayPointToPoint(num2, num3);
                            if ((Index >= point1.Y) && (Index <= point2.Y))
                            {
                                if (Index == point1.Y)
                                {
                                    if (Index == point2.Y)
                                    {
                                        num1 = point1.X;
                                        num2 = point2.X;
                                    }
                                    else
                                    {
                                        num1 = point1.X;
                                        num2 = 0x7fffffff;
                                    }
                                }
                                else if (Index == point2.Y)
                                {
                                    num1 = 0;
                                    num2 = point2.X;
                                }
                                else
                                {
                                    num1 = 0;
                                    num2 = 0x7fffffff;
                                }
                                List.Add(new River.Orqa.Editor.Common.Range(num1, Index, num2, Index));
                            }
                        }
                        num3++;
                        goto Label_00E7;
                    }
                    goto Label_01BA;
                }
                default:
                {
                    goto Label_01BA;
                }
            }
            List.Add(new River.Orqa.Editor.Common.Range(num1, Index, num2, Index));
        Label_01BA:
            return List.Count;
        }

        public bool GetSelectionForLine(int Index, out int Left, out int Right)
        {
            return this.GetSelectionForLine(Index, out Left, out Right, true);
        }

        protected internal bool GetSelectionForLine(int Index, out int Left, out int Right, bool CheckBounds)
        {
            bool flag1 = false;
            Left = 0;
            Right = 0;
            if (!this.IsEmpty())
            {
                DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
                Point point1 = strings1.PointToDisplayPoint(this.selectionRect.Left, this.selectionRect.Top, this.atTopLeftEnd);
                Point point2 = strings1.PointToDisplayPoint(this.selectionRect.Right, this.selectionRect.Bottom, this.atBottomRightEnd);
                flag1 = (Index >= point1.Y) && (Index <= point2.Y);
                if (flag1)
                {
                    if ((this.selectionType == River.Orqa.Editor.SelectionType.Block) || (point1.Y == point2.Y))
                    {
                        Left = point1.X;
                        Right = point2.X;
                    }
                    else if (Index == point1.Y)
                    {
                        Left = point1.X;
                        Right = 0x7fffffff;
                    }
                    else if (Index == point2.Y)
                    {
                        Left = 0;
                        Right = point2.X;
                    }
                    else
                    {
                        Left = 0;
                        Right = 0x7fffffff;
                    }
                    this.SwapMaxInt(ref Left, ref Right);
                }
            }
            if (!flag1)
            {
                return false;
            }
            if (CheckBounds)
            {
                return (Right > Left);
            }
            return true;
        }

        private Point GetSelectionPoint()
        {
            if (!this.IsEmpty())
            {
                return this.selectionRect.Location;
            }
            return this.owner.Position;
        }

        protected internal Rectangle GetSelectionRectInPixels()
        {
            if (!this.IsEmpty())
            {
                Point point1 = ((SyntaxEdit) this.owner).TextToScreen(this.selectionRect.Location, this.atTopLeftEnd);
                Point point2 = ((SyntaxEdit) this.owner).TextToScreen(this.selectionRect.Location + this.selectionRect.Size, this.atBottomRightEnd);
                return new Rectangle(point1.X, point1.Y, point2.X - point1.X, (point2.Y - point1.Y) + this.owner.Painter.FontHeight);
            }
            return new Rectangle(0, 0, 0, 0);
        }

        private Region GetSelectionRegion(River.Orqa.Editor.SelectionType SelectionType, Rectangle Rect)
        {
            return ((SyntaxEdit) this.owner).GetRectRegion(SelectionType, Rect, this.atTopLeftEnd, this.atBottomRightEnd);
        }

        private int GetTabIndent(int Indent)
        {
            return this.owner.Lines.TabPosToPos(new string('\t', Indent), Indent);
        }

        public void Indent()
        {
            this.ChangeBlock(this.indentLineEvent, false, this.SelectionType != River.Orqa.Editor.SelectionType.Block);
        }

        private string IndentLine(string String)
        {
            ISyntaxStrings strings1 = this.owner.Lines;
            return ((strings1.UseSpaces ? new string(' ', strings1.GetTabStop(0)) : Consts.TabStr) + String);
        }

        public void InsertString(string String)
        {
            this.owner.Source.BeginUpdate(UpdateReason.Insert);
            try
            {
                if (this.ShouldDelete())
                {
                    this.Delete();
                }
                this.owner.Source.Insert(String);
            }
            finally
            {
                this.owner.Source.EndUpdate();
            }
        }

        protected internal void InvalidateSelection()
        {
            if (this.owner.Source.UpdateCount == 0)
            {
                Region region1 = this.GetSelectionRegion(this.selectionType, this.selectionRect);
                if (region1 != null)
                {
                    ((SyntaxEdit) this.owner).Invalidate(region1, false);
                    region1.Dispose();
                }
            }
        }

        public bool IsEmpty()
        {
            return (this.selectionType == River.Orqa.Editor.SelectionType.None);
        }

        public bool IsPosInSelection(Point Position)
        {
            return this.IsPosInSelection(Position.X, Position.Y);
        }

        public bool IsPosInSelection(int X, int Y)
        {
            if (!this.IsEmpty())
            {
                int num1;
                int num2;
                Point point1 = this.owner.DisplayLines.PointToDisplayPoint(X, Y);
                if (this.GetSelectionForLine(point1.Y, out num1, out num2))
                {
                    if (point1.X >= num1)
                    {
                        return (point1.X < num2);
                    }
                    return false;
                }
            }
            return false;
        }

        protected internal bool IsSelectionRectEmpty()
        {
            return this.IsSelectionRectEmpty(this.selectionRect);
        }

        protected internal bool IsSelectionRectEmpty(Rectangle Rect)
        {
            return this.IsSelectionRectEmpty(this.selectionType, Rect);
        }

        protected internal bool IsSelectionRectEmpty(River.Orqa.Editor.SelectionType SelectionType, Rectangle Rect)
        {
            switch (SelectionType)
            {
                case River.Orqa.Editor.SelectionType.Stream:
                {
                    if (Rect.Height < 0)
                    {
                        return true;
                    }
                    if (Rect.Height == 0)
                    {
                        return (Rect.Width <= 0);
                    }
                    return false;
                }
                case River.Orqa.Editor.SelectionType.Block:
                {
                    DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
                    if (Rect.Height < 0)
                    {
                        return true;
                    }
                    Point point1 = strings1.PointToDisplayPoint(Rect.Left, Rect.Top, this.atTopLeftEnd);
                    point1 = strings1.PointToDisplayPoint(Rect.Right, Rect.Bottom, this.atBottomRightEnd);
                    return (point1.X > point1.X);
                }
            }
            return true;
        }

        protected internal bool IsValidPos(Point Position)
        {
            switch (this.selectionType)
            {
                case River.Orqa.Editor.SelectionType.Stream:
                {
                    if ((this.selectionRect.Top == Position.Y) && (this.selectionRect.Left == Position.X))
                    {
                        return true;
                    }
                    if (this.selectionRect.Bottom == Position.Y)
                    {
                        return (this.selectionRect.Right == Position.X);
                    }
                    return false;
                }
                case River.Orqa.Editor.SelectionType.Block:
                {
                    DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
                    Point point1 = strings1.PointToDisplayPoint(this.selectionRect.Left, this.selectionRect.Top, this.atTopLeftEnd);
                    Point point2 = strings1.PointToDisplayPoint(this.selectionRect.Right, this.selectionRect.Bottom, this.atBottomRightEnd);
                    Point point3 = strings1.PointToDisplayPoint(Position);
                    if ((point1.Y != point3.Y) && (point2.Y != point3.Y))
                    {
                        return false;
                    }
                    if (point1.X != point3.X)
                    {
                        return (point2.X == point3.X);
                    }
                    return true;
                }
            }
            return true;
        }

        public void LineTransponse()
        {
            if (this.owner.Position.Y < (this.owner.Lines.Count - 1))
            {
                ITextSource source1 = this.owner.Source;
                source1.BeginUpdate(UpdateReason.Insert);
                try
                {
                    string text1 = this.owner.Lines[this.owner.Position.Y];
                    this.owner.MoveLineBegin();
                    source1.DeleteRight(0x7fffffff);
                    source1.UnBreakLine();
                    this.owner.MoveLineEnd();
                    source1.BreakLine();
                    source1.MoveTo(0, source1.Position.Y + 1);
                    source1.Insert(text1);
                }
                finally
                {
                    source1.EndUpdate();
                }
            }
        }

        public void LowerCase()
        {
            this.ChangeBlock(this.lowerCaseLineEvent, true, false);
        }

        private string LowerCaseLine(string String)
        {
            return String.ToLower();
        }

        public bool Move(Point Position, bool DeleteOrigin)
        {
            bool flag1 = (!this.IsPosInSelection(Position) && !this.owner.Source.ReadOnly) && !this.IsEmpty();
            if (flag1)
            {
                River.Orqa.Editor.SelectionType type1 = this.selectionType;
                ITextSource source1 = this.owner.Source;
                source1.BeginUpdate(UpdateReason.Other);
                try
                {
                    int num1 = source1.StorePosition(Position);
                    string text1 = this.SelectedText;
                    if (DeleteOrigin)
                    {
                        this.Delete();
                    }
                    source1.Position = source1.RestorePosition(num1);
                    this.Clear();
                    this.SetSelectedText(text1, type1);
                }
                finally
                {
                    source1.EndUpdate();
                }
            }
            return flag1;
        }

        private void MoveSelection()
        {
            Point point1 = this.selEnd;
            this.UpdateWordSelection(ref point1, this.selForward);
            if (this.selectionType == River.Orqa.Editor.SelectionType.Block)
            {
                TextSource source1 = this.owner.Source as TextSource;
                NavigateOptions options1 = source1.NavigateOptions;
                source1.SetNavigateOptions(options1 | NavigateOptions.BeyondEol);
                try
                {
                    this.owner.Position = point1;
                    return;
                }
                finally
                {
                    source1.SetNavigateOptions(options1);
                }
            }
            this.owner.Position = point1;
        }

        protected internal bool NeedDragScroll(Point Pt)
        {
            if (this.owner.WordWrap)
            {
                if (this.owner.WrapAtMargin)
                {
                    Point point1 = this.owner.DisplayToScreen(this.owner.Margin.Position, 0);
                    if (Pt.X >= point1.X)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            Rectangle rectangle1 = this.owner.ClientRect;
            Rectangle rectangle2 = rectangle1;
            rectangle2.Inflate(-this.owner.Painter.FontWidth, -this.owner.Painter.FontHeight);
            if (rectangle1.Contains(Pt))
            {
                return !rectangle2.Contains(Pt);
            }
            return false;
        }

        public void NewLine()
        {
            this.owner.Source.NewLine();
        }

        public void NewLineAbove()
        {
            this.owner.Source.NewLineAbove();
        }

        public void NewLineBelow()
        {
            this.owner.Source.NewLineBelow();
        }

        protected internal void OnSelect(object source, EventArgs e)
        {
            bool flag1;
            Point point2;
            DisplayStrings strings1;
            if ((this.selectionState != River.Orqa.Editor.SelectionState.None) && (this.owner.Source.UpdateCount <= 0))
            {
                Point point1 = this.owner.PointToClient(Cursor.Position);
                flag1 = false;
                point2 = ((SyntaxEdit) this.owner).ScreenToText(point1.X, point1.Y, ref flag1);
                if (point2 != this.selEnd)
                {
                    strings1 = this.owner.DisplayLines as DisplayStrings;
                    switch (this.selectionState)
                    {
                        case River.Orqa.Editor.SelectionState.Drag:
                        {
                            if (!this.owner.ClientRect.Contains(point1) || this.NeedDragScroll(point1))
                            {
                                return;
                            }
                            this.BeginUpdate();
                            strings1.LineEndUpdateCount++;
                            try
                            {
                                strings1.AtLineEnd = flag1;
                                this.owner.Position = point2;
                                return;
                            }
                            finally
                            {
                                strings1.LineEndUpdateCount--;
                                this.EndUpdate();
							}
							// SMC: comment out
							//goto Label_00F9;
						}
                        case River.Orqa.Editor.SelectionState.Select:
                        {
                            goto Label_00F9;
                        }
                        case River.Orqa.Editor.SelectionState.SelectWord:
                        {
                            goto Label_0161;
                        }
                    }
                }
            }
            return;
        Label_00F9:
            this.BeginUpdate();
            strings1.LineEndUpdateCount++;
            try
            {
                this.selEnd = point2;
                strings1.AtLineEnd = flag1;
                this.SelectBlock((e != null) ? River.Orqa.Editor.SelectionType.None : this.selectionType, this.CanSelectBlock() ? River.Orqa.Editor.SelectionType.Block : River.Orqa.Editor.SelectionType.Stream, this.selStart, this.moveSelection);
                return;
            }
            finally
            {
                strings1.LineEndUpdateCount--;
                this.EndUpdate();
            }
        Label_0161:
            this.BeginUpdate();
            strings1.LineEndUpdateCount++;
            try
            {
                strings1.AtLineEnd = flag1;
                this.selForward = (this.selEnd.Y > this.selStart.Y) || ((this.selEnd.Y == this.selStart.Y) && (this.selEnd.X >= this.selStart.X));
                Point point3 = this.selStart;
                this.UpdateWordSelection(ref point3, !this.selForward);
                this.selEnd = point2;
                this.SelectBlock((e != null) ? River.Orqa.Editor.SelectionType.None : this.selectionType, this.CanSelectBlock() ? River.Orqa.Editor.SelectionType.Block : River.Orqa.Editor.SelectionType.Stream, point3, this.moveSelection);
            }
            finally
            {
                strings1.LineEndUpdateCount--;
                this.EndUpdate();
            }
        }

        public void Paste()
        {
            object obj1 = Clipboard.GetDataObject().GetData(DataFormats.UnicodeText);
            if (obj1 == null)
            {
                obj1 = Clipboard.GetDataObject().GetData(DataFormats.Text);
            }
            if (obj1 != null)
            {
                object obj2 = Clipboard.GetDataObject().GetData(DataFormats.Serializable);
                this.SetSelectedText((string) obj1, ((obj2 is River.Orqa.Editor.SelectionType) && (((River.Orqa.Editor.SelectionType) obj2) != River.Orqa.Editor.SelectionType.None)) ? ((River.Orqa.Editor.SelectionType) obj2) : River.Orqa.Editor.SelectionType.Stream);
                if ((this.options & SelectionOptions.SmartFormat) != SelectionOptions.None)
                {
                    this.SmartFormat();
                }
                if ((this.options & SelectionOptions.PersistentBlocks) == SelectionOptions.None)
                {
                    this.Clear();
                }
            }
        }

        protected internal void PositionChanged(int X, int Y, int DeltaX, int DeltaY)
        {
            if ((this.updateCount == 0) && !this.IsEmpty())
            {
                Point point1 = this.selectionRect.Location;
                Point point2 = new Point(this.selectionRect.Right, this.selectionRect.Bottom);
                bool flag1 = SortList.UpdatePos(X, Y, DeltaX, DeltaY, ref point1, false);
                bool flag2 = SortList.UpdatePos(X, Y, DeltaX, DeltaY, ref point2, true);
                if (flag1 || flag2)
                {
                    this.SetSelection(this.selectionType, new Rectangle(point1.X, point1.Y, point2.X - point1.X, point2.Y - point1.Y));
                }
            }
        }

        public void ProcessEscape()
        {
            this.SelectionType = River.Orqa.Editor.SelectionType.None;
        }

        public void ProcessShiftTab()
        {
            if (this.IsEmpty())
            {
                this.owner.MoveToChar(this.owner.Lines.GetPrevTabStop(this.owner.Position.X));
            }
            else
            {
                this.UnIndent();
            }
        }

        public void ProcessTab()
        {
            if (this.IsEmpty() || (this.SelectedCount() == 1))
            {
                if (this.owner.Lines.UseSpaces)
                {
                    int num1 = this.owner.Lines.GetTabStop(this.owner.Position.X) - this.owner.Position.X;
                    if (num1 > 0)
                    {
                        this.InsertString(new string(' ', num1));
                    }
                }
                else
                {
                    char ch1 = '\t';
                    this.InsertString(ch1.ToString());
                }
            }
            else
            {
                this.Indent();
            }
        }

        public virtual void ResetAllowedSelectionMode()
        {
            this.AllowedSelectionMode = EditConsts.DefaultSelectionMode;
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = EditConsts.DefaultHighlightBackColor;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = EditConsts.DefaultHighlightForeColor;
        }

        public virtual void ResetInActiveBackColor()
        {
            this.InActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor;
        }

        public virtual void ResetInActiveForeColor()
        {
            this.InActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultSelectionOptions;
        }

        protected internal void RestoreSelection(ITextSource Source, River.Orqa.Editor.SelectionType SelType, int Index1, int Index2, int Index3)
        {
            Source.Position = Source.RestorePosition(Index3);
            if ((Index1 >= 0) && (Index2 >= 0))
            {
                Point point1 = Source.RestorePosition(Index2);
                Point point2 = Source.RestorePosition(Index1);
                this.SetSelection(SelType, point2, point1);
            }
        }

        private string SafeSubString(string S, int Left, int Right)
        {
            if (Left >= S.Length)
            {
                return string.Empty;
            }
            if (Right < S.Length)
            {
                return S.Substring(Left, Right - Left);
            }
            return S.Substring(Left);
        }

        public void SelectAll()
        {
            if (this.owner.Lines.Count > 0)
            {
                this.SetSelection(River.Orqa.Editor.SelectionType.Stream, new Rectangle(0, 0, this.owner.Lines.GetLength(this.owner.Lines.Count - 1), this.owner.Lines.Count - 1));
            }
        }

        private void SelectBlock(River.Orqa.Editor.SelectionType NewSelType, KeyEvent Action)
        {
            this.UpdateSelStart(true);
            this.SelectBlock(this.selectionType, NewSelType, this.selStart, Action);
        }

        private void SelectBlock(River.Orqa.Editor.SelectionType selType, River.Orqa.Editor.SelectionType newSelType, Point Position, KeyEvent Action)
        {
            Rectangle rectangle1 = this.SelectionRect;
            int num1 = Position.X;
            int num2 = Position.Y;
            int num3 = num1;
            int num4 = num2;
            if ((newSelType == River.Orqa.Editor.SelectionType.Block) && !this.owner.Painter.IsMonoSpaced)
            {
                newSelType = River.Orqa.Editor.SelectionType.Stream;
            }
            this.CheckSelectionMode(ref newSelType);
            this.BeginUpdate();
            try
            {
                DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
                if ((selType == River.Orqa.Editor.SelectionType.None) || !this.IsValidPos(Position))
                {
                    rectangle1.X = Position.X;
                    rectangle1.Y = Position.Y;
                    rectangle1.Width = 0;
                    rectangle1.Height = 0;
                }
                Point point1 = strings1.PointToDisplayPoint(num1, num2);
                if (Action != null)
                {
                    if ((newSelType == River.Orqa.Editor.SelectionType.Block) && !this.owner.WordWrap)
                    {
                        TextSource source1 = this.owner.Source as TextSource;
                        NavigateOptions options1 = source1.NavigateOptions;
                        source1.SetNavigateOptions(options1 | NavigateOptions.BeyondEol);
                        try
                        {
                            Action();
                            goto Label_00EE;
                        }
                        finally
                        {
                            source1.SetNavigateOptions(options1);
                        }
                    }
                    Action();
                }
            Label_00EE:
                num3 = this.owner.Position.X;
                num4 = this.owner.Position.Y;
                if (newSelType == River.Orqa.Editor.SelectionType.Block)
                {
                    Point point2 = strings1.PointToDisplayPoint(num3, num4);
                    if (point2.X < point1.X)
                    {
                        int num5 = point1.X;
                        point1.X = point2.X;
                        point2.X = num5;
                    }
                    if (point2.Y < point1.Y)
                    {
                        int num6 = point1.Y;
                        point1.Y = point2.Y;
                        point2.Y = num6;
                    }
                    point1 = strings1.DisplayPointToPoint(point1.X, point1.Y, true, false, false);
                    point2 = strings1.DisplayPointToPoint(point2.X, point2.Y, true, false, false);
                    rectangle1 = new Rectangle(point1.X, point1.Y, point2.X - point1.X, point2.Y - point1.Y);
                }
                else
                {
                    rectangle1 = new Rectangle(num1, num2, num3 - num1, num4 - num2);
                    if ((rectangle1.Height < 0) || ((rectangle1.Width < 0) && (rectangle1.Height == 0)))
                    {
                        this.SwapRect(ref rectangle1, false);
                    }
                }
                if ((this.owner.Source.UpdateCount == 0) && !this.IsEmpty())
                {
                    Region region1 = this.GetSelectionRegion(this.SelectionType, this.SelectionRect);
                    if (region1 != null)
                    {
                        ((SyntaxEdit) this.owner).Invalidate(region1);
                        region1.Dispose();
                    }
                }
                if (newSelType != River.Orqa.Editor.SelectionType.None)
                {
                    if (rectangle1.Location == this.owner.Position)
                    {
                        this.atTopLeftEnd = strings1.AtLineEnd;
                    }
                    if (this.owner.Position == new Point(rectangle1.Right, rectangle1.Bottom))
                    {
                        this.atBottomRightEnd = strings1.AtLineEnd;
                    }
                }
                this.SetSelection(newSelType, rectangle1);
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public void SelectCharLeft()
        {
            this.SelectCharLeft(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectCharLeft(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveCharLeftEvent);
        }

        public void SelectCharRight()
        {
            this.SelectCharRight(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectCharRight(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveCharRightEvent);
        }

        public bool SelectCurrentWord()
        {
            ISyntaxStrings strings1 = this.owner.Lines;
            Point point1 = this.owner.Position;
            while (point1.Y < strings1.Count)
            {
                string text1 = strings1[point1.Y];
                int num1 = text1.Length;
                while ((point1.X < num1) && strings1.IsDelimiter(text1, point1.X))
                {
                    point1.X++;
                }
                if (point1.X < num1)
                {
                    break;
                }
                point1.Y++;
                point1.X = 0;
            }
            if (point1.Y < strings1.Count)
            {
                this.owner.Position = point1;
                this.SelectWord();
            }
            return (point1.Y < strings1.Count);
        }

        public int SelectedCount()
        {
            if (this.selectionType == River.Orqa.Editor.SelectionType.None)
            {
                return 0;
            }
            return ((this.SelectionRect.Bottom - this.SelectionRect.Top) + 1);
        }

        public string SelectedString(int Index)
        {
            ArrayList list1;
            StringBuilder builder1;
            int num2;
            if (!this.IsEmpty())
            {
                int num1 = this.selectionRect.Top;
                list1 = new ArrayList();
                this.GetSelectedBounds(Index + num1, list1);
                if (list1.Count == 0)
                {
                    return null;
                }
                switch (this.SelectionType)
                {
                    case River.Orqa.Editor.SelectionType.Stream:
                    {
                        IRange range2 = (IRange) list1[0];
                        return this.SafeSubString(this.owner.Lines[range2.StartPoint.Y], range2.StartPoint.X, range2.EndPoint.X);
                    }
                    case River.Orqa.Editor.SelectionType.Block:
                    {
                        builder1 = new StringBuilder();
                        num2 = 0;
                        goto Label_00CF;
                    }
                }
            }
            return null;
        Label_00CF:
            if (num2 >= list1.Count)
            {
                return builder1.ToString();
            }
            IRange range1 = (IRange) list1[num2];
            builder1.Append(this.SafeSubString(this.owner.Lines[range1.StartPoint.Y], range1.StartPoint.X, range1.EndPoint.X));
            if (num2 != (list1.Count - 1))
            {
                builder1.Append(Consts.CRLF);
            }
            num2++;
            goto Label_00CF;
        }

        public void SelectFileBegin()
        {
            this.SelectFileBegin(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectFileBegin(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveFileBeginEvent);
        }

        public void SelectFileEnd()
        {
            this.SelectFileEnd(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectFileEnd(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveFileEndEvent);
        }

        public Point SelectionToTextPoint(Point Position)
        {
            switch (this.selectionType)
            {
                case River.Orqa.Editor.SelectionType.Stream:
                {
                    if (Position.Y == 0)
                    {
                        return new Point(Position.X + this.selectionRect.Left, this.selectionRect.Top);
                    }
                    return new Point(Position.X, this.selectionRect.Top + Position.Y);
                }
                case River.Orqa.Editor.SelectionType.Block:
                {
                    return new Point(Position.X + this.selectionRect.Left, Position.Y + this.selectionRect.Top);
                }
            }
            return Position;
        }

        public void SelectLine()
        {
            this.SetSelection(River.Orqa.Editor.SelectionType.Stream, new Rectangle(0, this.owner.Position.Y, 0, 1));
        }

        public void SelectLineBegin()
        {
            this.SelectLineBegin(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectLineBegin(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveLineBeginEvent);
        }

        public void SelectLineDown()
        {
            this.SelectLineDown(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectLineDown(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveLineDownEvent);
        }

        public void SelectLineEnd()
        {
            this.SelectLineEnd(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectLineEnd(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveLineEndEvent);
        }

        public void SelectLineUp()
        {
            this.SelectLineUp(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectLineUp(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveLineUpEvent);
        }

        public bool SelectNextWord()
        {
            ISyntaxStrings strings1 = this.owner.Lines;
            Point point1 = this.owner.Position;
            string text1 = strings1[point1.Y];
            while ((point1.X < text1.Length) && !strings1.IsDelimiter(text1, point1.X))
            {
                point1.X++;
            }
            this.owner.Position = point1;
            return this.SelectCurrentWord();
        }

        public void SelectPageDown()
        {
            this.SelectPageDown(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectPageDown(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().movePageDownEvent);
        }

        public void SelectPageUp()
        {
            this.SelectPageUp(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectPageUp(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().movePageUpEvent);
        }

        public void SelectScreenBottom()
        {
            this.SelectScreenBottom(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectScreenBottom(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveScreenBottomEvent);
        }

        public void SelectScreenTop()
        {
            this.SelectScreenTop(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectScreenTop(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveScreenTopEvent);
        }

        public void SelectToCloseBrace()
        {
            this.SelectBlock((this.SelectionType != River.Orqa.Editor.SelectionType.None) ? this.SelectionType : River.Orqa.Editor.SelectionType.Stream, this.GetHandlers().moveToCloseBraceEvent);
        }

        public void SelectToOpenBrace()
        {
            this.SelectBlock((this.SelectionType != River.Orqa.Editor.SelectionType.None) ? this.SelectionType : River.Orqa.Editor.SelectionType.Stream, this.GetHandlers().moveToOpenBraceEvent);
        }

        public void SelectWord()
        {
            int num1;
            int num2;
            ISyntaxStrings strings1 = this.owner.Lines;
            Point point1 = this.owner.Position;
            if (strings1.GetWord(point1.Y, point1.X, out num1, out num2))
            {
                this.owner.MoveTo(num2 + 1, point1.Y);
                this.SetSelection(River.Orqa.Editor.SelectionType.Stream, new Rectangle(num1, point1.Y, (num2 - num1) + 1, 0));
            }
        }

        public void SelectWordLeft()
        {
            this.SelectWordLeft(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectWordLeft(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveWordLeftEvent);
        }

        public void SelectWordRight()
        {
            this.SelectWordRight(River.Orqa.Editor.SelectionType.Stream);
        }

        public void SelectWordRight(River.Orqa.Editor.SelectionType SelectionType)
        {
            this.SelectBlock(SelectionType, this.GetHandlers().moveWordRightEvent);
        }

        private void SetRectLeft(ref Rectangle Rect, int Left)
        {
            int num1 = Rect.Right - Left;
            Rect.X = Left;
            Rect.Width = num1;
        }

        private void SetRectTop(ref Rectangle Rect, int Top)
        {
            int num1 = Rect.Bottom - Top;
            Rect.Y = Top;
            Rect.Height = num1;
        }

        public void SetSelectedText(string Text, River.Orqa.Editor.SelectionType SelType)
        {
            this.BeginUpdate();
            TextSource source1 = this.owner.Source as TextSource;
            source1.BeginUpdate(UpdateReason.Other);
            NavigateOptions options1 = source1.NavigateOptions;
            source1.SetNavigateOptions(options1 | NavigateOptions.BeyondEol);
            try
            {
                ((SyntaxEdit) this.owner).FinishVerticalNavigate();
                Point point1 = this.IsEmpty() ? source1.Position : this.SelectionRect.Location;
                bool flag1 = !this.IsEmpty() && source1.Position.Equals(point1);
                this.Delete();
                if ((Text == string.Empty) || (Text == null))
                {
                    return;
                }
                if (SelType == River.Orqa.Editor.SelectionType.Stream)
                {
                    source1.InsertBlock(Text);
                    this.SetSelection(River.Orqa.Editor.SelectionType.Stream, point1, this.owner.Position);
                    if (flag1)
                    {
                        this.owner.Position = point1;
                    }
                }
                else
                {
                    source1.Position = point1;
                    source1.BeginUpdate(UpdateReason.Insert);
                    try
                    {
                        string[] textArray1 = StrItem.Split(Text);
                        int num1 = 0;
                        for (int num2 = 0; num2 < textArray1.Length; num2++)
                        {
                            if ((num2 > 0) && (SelType == River.Orqa.Editor.SelectionType.Block))
                            {
                                source1.Navigate(-num1, 0);
                            }
                            num1 = this.DoInsertString(SelType, textArray1[num2], num2 != 0);
                        }
                        this.SetSelection(SelType, point1, this.owner.Position);
                        if (flag1)
                        {
                            this.owner.Position = point1;
                        }
                    }
                    finally
                    {
                        this.owner.Source.EndUpdate();
                    }
                }
            }
            finally
            {
                source1.SetNavigateOptions(options1);
                source1.EndUpdate();
                this.EndUpdate();
            }
        }

        public void SetSelection(River.Orqa.Editor.SelectionType SelectionType, Rectangle SelectionRect)
        {
            this.CheckSelectionMode(ref SelectionType);
            bool flag1 = false;
            this.UpdateSelRect(ref this.selectionType, SelectionType, ref this.selectionRect, SelectionRect, ref flag1);
            if (flag1 && (this.updateCount == 0))
            {
                this.DoSelectionChanged();
            }
        }

        public void SetSelection(River.Orqa.Editor.SelectionType SelectionType, Point SelectionStart, Point SelectionEnd)
        {
            this.SetSelection(SelectionType, new Rectangle(SelectionStart.X, SelectionStart.Y, SelectionEnd.X - SelectionStart.X, SelectionEnd.Y - SelectionStart.Y));
        }

        protected internal bool ShouldDelete()
        {
            if (((this.options & SelectionOptions.OverwriteBlocks) != SelectionOptions.None) && ((this.options & SelectionOptions.PersistentBlocks) == SelectionOptions.None))
            {
                return !this.IsEmpty();
            }
            return false;
        }

        protected internal bool ShouldDeleteBlock()
        {
            if ((this.options & SelectionOptions.PersistentBlocks) == SelectionOptions.None)
            {
                return !this.IsEmpty();
            }
            return false;
        }

        public bool ShouldSerializeAllowedSelectionMode()
        {
            return (this.AllowedSelectionMode != EditConsts.DefaultSelectionMode);
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.backColor != EditConsts.DefaultHighlightBackColor);
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.foreColor != EditConsts.DefaultHighlightForeColor);
        }

        public bool ShouldSerializeInActiveBackColor()
        {
            return (this.inActiveBackColor != EditConsts.DefaultInactiveHighlightBackColor);
        }

        public bool ShouldSerializeInActiveForeColor()
        {
            return (this.inActiveForeColor != EditConsts.DefaultInactiveHighlightForeColor);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultSelectionOptions);
        }

        public void SmartFormat()
        {
            this.SmartFormat(true);
        }

        protected void SmartFormat(bool NeedFormat)
        {
            if (((TextSource) this.owner.Source).NeedFormatText())
            {
                if (this.IsEmpty())
                {
                    this.SelectLine();
                }
                if (!this.IsEmpty())
                {
                    int num1;
                    int num2;
                    int num3;
                    River.Orqa.Editor.SelectionType type1;
                    ITextSource source1 = this.owner.Source;
                    this.BeginUpdate();
                    source1.BeginUpdate(UpdateReason.InsertBlock);
                    this.StoreSelection(source1, out type1, out num1, out num2, out num3);
                    try
                    {
                        if (NeedFormat)
                        {
                            ((TextSource) source1).FormatText();
                        }
                        IFormatText text1 = (IFormatText) this.owner.Source.Lexer;
                        SyntaxStrings strings1 = (SyntaxStrings) this.owner.Lines;
                        for (int num4 = this.selectionRect.Top; num4 <= this.selectionRect.Bottom; num4++)
                        {
                            string text2 = strings1[num4];
                            string text3 = text2.TrimStart(new char[0]);
                            if (text3 != string.Empty)
                            {
                                int num5 = text2.Length - text3.Length;
                                int num6 = text1.GetSmartIndent(num4);
                                if (num6 >= 0)
                                {
                                    string text4 = strings1.GetIndentString(this.GetTabIndent(num6), 0);
                                    if (text4 != text2.Substring(0, num5))
                                    {
                                        if (num5 >= 0)
                                        {
                                            source1.MoveTo(0, num4);
                                            source1.DeleteRight(num5);
                                        }
                                        source1.Insert(text4);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        this.RestoreSelection(source1, type1, num1, num2, num3);
                        source1.EndUpdate();
                        this.EndUpdate();
                    }
                }
            }
        }

        public void SmartFormatBlock()
        {
            if (((this.Options & SelectionOptions.SmartFormat) != SelectionOptions.None) && ((TextSource) this.owner.Source).NeedFormatText())
            {
                TextSource source1 = (TextSource) this.owner.Source;
                IFormatText text1 = (IFormatText) source1.Lexer;
                Point point1 = new Point(Math.Max((int) (source1.Position.X - 1), 0), source1.Position.Y);
                source1.FormatText();
                IRange range1 = text1.GetBlock(point1);
                if (range1 != null)
                {
                    Point point2 = (range1 is ISyntaxInfo) ? ((ISyntaxInfo) range1).Position : range1.StartPoint;
                    this.SetSelection(River.Orqa.Editor.SelectionType.Stream, point2, range1.EndPoint);
                    int num1 = source1.StorePosition(source1.Position);
                    this.SmartFormat(false);
                    if ((this.owner.Braces.BracesOptions != BracesOptions.None) && !this.owner.Braces.UseRoundRect)
                    {
                        Rectangle[] rectangleArray1 = new Rectangle[1] { new Rectangle(this.selectionRect.Left, this.selectionRect.Top, range1.StartPoint.X - this.selectionRect.Left, range1.StartPoint.Y - this.selectionRect.Top) } ;
                        source1.TempHighlightBraces(rectangleArray1);
                    }
                    source1.Position = source1.RestorePosition(num1);
                    source1.DoHighlightBraces();
                    this.Clear();
                }
            }
        }

        public void SmartFormatDocument()
        {
            TextSource source1 = this.owner.Source as TextSource;
            if (source1.NeedFormatText())
            {
                source1.BeginUpdate(UpdateReason.Other);
                try
                {
                    int num1;
                    int num2;
                    int num3;
                    River.Orqa.Editor.SelectionType type1;
                    this.StoreSelection(source1, out type1, out num1, out num2, out num3);
                    this.SelectAll();
                    this.SmartFormat();
                    this.RestoreSelection(source1, type1, num1, num2, num3);
                }
                finally
                {
                    source1.EndUpdate();
                }
            }
        }

        protected internal void StartSelection()
        {
            this.selTimer.Start();
        }

        protected internal void StoreSelection(ITextSource Source, out River.Orqa.Editor.SelectionType selType, out int Index1, out int Index2, out int Index3)
        {
            selType = this.SelectionType;
            if (!this.IsEmpty())
            {
                Index1 = Source.StorePosition(this.selectionRect.Location);
                Index2 = Source.StorePosition(new Point(this.selectionRect.Right, this.selectionRect.Bottom));
            }
            else
            {
                Index1 = -1;
                Index2 = -1;
            }
            Index3 = Source.StorePosition(Source.Position);
        }

        public void SwapAnchor()
        {
            if (!this.IsEmpty())
            {
                this.BeginUpdate();
                try
                {
                    if (this.owner.Position == (this.selectionRect.Location + this.selectionRect.Size))
                    {
                        this.owner.Position = this.selectionRect.Location;
                    }
                    else
                    {
                        this.owner.Position = this.selectionRect.Location + this.selectionRect.Size;
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        private void SwapMaxInt(ref int Left, ref int Right)
        {
            if (Right < Left)
            {
                int num1 = Left;
                Left = Right;
                Right = num1;
            }
        }

        private void SwapRect(ref Rectangle Rect, bool SwapMax)
        {
            Point point1 = Rect.Location;
            Point point2 = Rect.Location + Rect.Size;
            Rect.Location = point2;
            Rect.Width = point1.X - Rect.X;
            Rect.Height = point1.Y - Rect.Y;
        }

        public void Tabify()
        {
            this.ChangeBlock(this.tabifyLineEvent);
        }

        private string TabifyLine(string String)
        {
            string text1 = string.Empty;
            for (int num1 = 0; num1 < String.Length; num1++)
            {
                if (String[num1] == ' ')
                {
                    int num2 = num1;
                    while (((num2 + 1) < String.Length) && (String[num2 + 1] == ' '))
                    {
                        num2++;
                    }
                    text1 = text1 + ((SyntaxStrings) this.owner.Lines).GetIndentString((num2 - num1) + 1, this.owner.Lines.TabPosToPos(String, num1), false);
                    num1 = num2 + 1;
                    continue;
                }
                text1 = text1 + String[num1];
            }
            return text1;
        }

        public Point TextToSelectionPoint(Point Position)
        {
            switch (this.selectionType)
            {
                case River.Orqa.Editor.SelectionType.Stream:
                {
                    if (Position.Y == this.selectionRect.Top)
                    {
                        return new Point(Position.X - this.selectionRect.Left, 0);
                    }
                    return new Point(Position.X, Position.Y - this.selectionRect.Top);
                }
                case River.Orqa.Editor.SelectionType.Block:
                {
                    return new Point(Position.X - this.selectionRect.Left, Position.Y - this.selectionRect.Top);
                }
            }
            return Position;
        }

        public void ToggleOutlining()
        {
            DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
            ArrayList list1 = new ArrayList();
            if (!this.IsEmpty())
            {
                strings1.GetOutlineRanges(list1, this.selectionRect.Location, this.selectionRect.Location + this.selectionRect.Size);
            }
            if (list1.Count == 0)
            {
                IOutlineRange range1 = strings1.GetOutlineRange(this.owner.Position);
                if (range1 != null)
                {
                    list1.Add(range1);
                }
            }
            if (list1.Count != 0)
            {
                strings1.ToggleOutlining(list1, null);
            }
        }

        public void ToggleOverWrite()
        {
            this.owner.Source.OverWrite = !this.owner.Source.OverWrite;
        }

        public void UnIndent()
        {
            this.ChangeBlock(this.unIndentLineEvent, false, this.SelectionType != River.Orqa.Editor.SelectionType.Block);
        }

        private string UnIndentLine(string String)
        {
            if (String != string.Empty)
            {
                ISyntaxStrings strings1 = this.owner.Lines;
                int num1 = strings1.UseSpaces ? strings1.GetTabStop(0) : 1;
                string text1 = String.Substring(0, Math.Min(num1, String.Length));
                num1 = text1.Length - text1.TrimStart(null).Length;
                if (num1 > 0)
                {
                    String = String.Remove(0, num1);
                }
            }
            return String;
        }

        public void UnTabify()
        {
            this.ChangeBlock(this.unTabifyLineEvent);
        }

        private string UnTabifyLine(string String)
        {
            return this.owner.Lines.GetTabString(String);
        }

        protected internal void UpdateSelection()
        {
            if (!this.IsEmpty() && ((((this.options & SelectionOptions.HideSelection) != SelectionOptions.None) || (this.ForeColor != this.InActiveForeColor)) || (this.BackColor != this.InActiveBackColor)))
            {
                this.InvalidateSelection();
            }
        }

        protected void UpdateSelRect(ref River.Orqa.Editor.SelectionType OldSelectionType, River.Orqa.Editor.SelectionType NewSelectionType, ref Rectangle OldRect, Rectangle NewRect, ref bool Changed)
        {
            Changed = false;
            if ((OldSelectionType != NewSelectionType) || (OldRect != NewRect))
            {
                bool flag1 = this.owner.Source.UpdateCount == 0;
                Region region1 = flag1 ? this.GetSelectionRegion(OldSelectionType, OldRect) : null;
                if ((SelectionOptions.DisableSelection & this.options) != SelectionOptions.None)
                {
                    NewSelectionType = River.Orqa.Editor.SelectionType.None;
                }
                else if (this.IsSelectionRectEmpty(NewSelectionType, NewRect))
                {
                    NewSelectionType = River.Orqa.Editor.SelectionType.None;
                }
                if (NewSelectionType == River.Orqa.Editor.SelectionType.None)
                {
                    NewRect = Rectangle.Empty;
                    this.atTopLeftEnd = false;
                    this.atBottomRightEnd = false;
                }
                Changed = true;
                OldRect = NewRect;
                OldSelectionType = NewSelectionType;
                if (flag1)
                {
                    Region region2 = this.GetSelectionRegion(NewSelectionType, NewRect);
                    if (region1 == null)
                    {
                        region1 = region2;
                    }
                    else if (region2 != null)
                    {
                        region1.Union(region2);
                        region2.Dispose();
                    }
                    if (region1 != null)
                    {
                        ((SyntaxEdit) this.owner).Invalidate(region1, false);
                        region1.Dispose();
                    }
                }
            }
        }

        protected internal void UpdateSelStart(bool CheckIfEmpty)
        {
            if ((!CheckIfEmpty || (this.selectionType == River.Orqa.Editor.SelectionType.None)) || !this.IsValidPos(this.owner.Position))
            {
                this.selStart = this.owner.Position;
            }
        }

        private void UpdateWordSelection(ref Point Pt, bool direction)
        {
            int num1;
            int num2;
            if (this.selectionState != River.Orqa.Editor.SelectionState.SelectWord)
            {
                return;
            }
            if (this.selectionType == River.Orqa.Editor.SelectionType.Block)
            {
                TextSource source1 = this.owner.Source as TextSource;
                NavigateOptions options1 = source1.NavigateOptions;
                source1.SetNavigateOptions(options1 | NavigateOptions.BeyondEol);
                try
                {
                    source1.ValidatePosition(ref Pt);
                    goto Label_0055;
                }
                finally
                {
                    source1.SetNavigateOptions(options1);
                }
            }
            this.owner.Source.ValidatePosition(ref Pt);
        Label_0055:
            if (this.owner.Lines.GetWord(Pt.Y, Pt.X, out num1, out num2))
            {
                Pt.X = direction ? (num2 + 1) : num1;
            }
        }

        public void UpperCase()
        {
            this.ChangeBlock(this.upperCaseLineEvent, true, false);
        }

        private string UpperCaseLine(string String)
        {
            return String.ToUpper();
        }

        public void WordTransponse()
        {
            this.owner.Source.BeginUpdate(UpdateReason.Insert);
            try
            {
                Point point1 = this.owner.Position;
                if (this.SelectCurrentWord() && !this.IsEmpty())
                {
                    Rectangle rectangle1 = this.SelectionRect;
                    River.Orqa.Editor.SelectionType type1 = this.SelectionType;
                    string text1 = this.SelectedText;
                    this.SelectNextWord();
                    if (!this.IsEmpty())
                    {
                        Point point2 = this.owner.Position;
                        string text2 = this.SelectedText;
                        this.Delete();
                        this.InsertString(text1);
                        this.owner.MoveTo(rectangle1.Location);
                        this.owner.Selection.SetSelection(type1, rectangle1);
                        this.Delete();
                        this.InsertString(text2);
                        return;
                    }
                }
                this.owner.Position = point1;
            }
            finally
            {
                this.owner.Source.EndUpdate();
            }
        }

        protected void WriteToClipboard()
        {
            IDataObject obj1 = new DataObject();
            obj1.SetData(this.SelectedText);
            if (this.SelectionType != River.Orqa.Editor.SelectionType.None)
            {
                obj1.SetData(DataFormats.Serializable, this.SelectionType);
            }
            Clipboard.SetDataObject(obj1);
        }


        // Properties
        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public River.Orqa.Editor.AllowedSelectionMode AllowedSelectionMode
        {
            get
            {
                return this.allowedSelectionMode;
            }
            set
            {
                if (this.allowedSelectionMode != value)
                {
                    this.allowedSelectionMode = value;
                    this.SetSelection(this.selectionType, this.selectionRect);
                }
            }
        }

        public Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.InvalidateSelection();
                }
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                if (this.foreColor != value)
                {
                    this.foreColor = value;
                    this.InvalidateSelection();
                }
            }
        }

        public Color InActiveBackColor
        {
            get
            {
                return this.inActiveBackColor;
            }
            set
            {
                if (this.inActiveBackColor != value)
                {
                    this.inActiveBackColor = value;
                    this.InvalidateSelection();
                }
            }
        }

        public Color InActiveForeColor
        {
            get
            {
                return this.inActiveForeColor;
            }
            set
            {
                if (this.inActiveForeColor != value)
                {
                    this.inActiveForeColor = value;
                    this.InvalidateSelection();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public SelectionOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    if ((SelectionOptions.DisableSelection & value) != SelectionOptions.None)
                    {
                        this.Clear();
                    }
                    this.InvalidateSelection();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string SelectedText
        {
            get
            {
                if (this.IsEmpty())
                {
                    return string.Empty;
                }
                StringBuilder builder1 = new StringBuilder();
                bool flag1 = true;
                for (int num1 = 0; num1 < this.SelectedCount(); num1++)
                {
                    string text1 = this.SelectedString(num1);
                    if (text1 != null)
                    {
                        if (!flag1)
                        {
                            builder1.Append(Consts.CRLF);
                        }
                        builder1.Append(text1);
                        flag1 = false;
                    }
                }
                return builder1.ToString();
            }
            set
            {
                this.SetSelectedText(value, River.Orqa.Editor.SelectionType.Stream);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int SelectionLength
        {
            get
            {
                return this.SelectedText.Length;
            }
            set
            {
                this.SetSelection(River.Orqa.Editor.SelectionType.Stream, this.GetSelectionPoint(), this.owner.Source.AbsolutePositionToTextPoint(this.SelectionStart + value));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle SelectionRect
        {
            get
            {
                return this.selectionRect;
            }
            set
            {
                this.SetSelection(this.selectionType, value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get
            {
                return this.owner.Source.TextPointToAbsolutePosition(this.GetSelectionPoint());
            }
            set
            {
                this.selectionType = River.Orqa.Editor.SelectionType.None;
                this.owner.Source.MoveTo(this.owner.Source.AbsolutePositionToTextPoint(value));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public River.Orqa.Editor.SelectionState SelectionState
        {
            get
            {
                return this.selectionState;
            }
            set
            {
                this.selectionState = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public River.Orqa.Editor.SelectionType SelectionType
        {
            get
            {
                return this.selectionType;
            }
            set
            {
                this.SetSelection(value, this.selectionRect);
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlSelectionInfo(this);
            }
            set
            {
                ((XmlSelectionInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private River.Orqa.Editor.AllowedSelectionMode allowedSelectionMode;
        private bool atBottomRightEnd;
        private bool atTopLeftEnd;
        private Color backColor;
        private StringEvent capitalizeLineEvent;
        private StringEvent deleteWhiteSpaceEvent;
        private Color foreColor;
        private Color inActiveBackColor;
        private Color inActiveForeColor;
        private StringEvent indentLineEvent;
        private StringEvent lowerCaseLineEvent;
        private KeyEvent moveSelection;
        private Rectangle oldSelectionRect;
        private River.Orqa.Editor.SelectionType oldSelectionType;
        private SelectionOptions options;
        private ISyntaxEdit owner;
        private Rectangle selectionRect;
        private River.Orqa.Editor.SelectionState selectionState;
        private River.Orqa.Editor.SelectionType selectionType;
        private Point selEnd;
        private bool selForward;
        private Point selStart;
        private Timer selTimer;
        private StringEvent tabifyLineEvent;
        private StringEvent unIndentLineEvent;
        private StringEvent unTabifyLineEvent;
        private int updateCount;
        private StringEvent upperCaseLineEvent;
    }
}

