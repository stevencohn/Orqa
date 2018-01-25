namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;

    internal class OutlineList : RangeList
    {
        // Methods
        public OutlineList(DisplayStrings Owner)
        {
            this.owner = Owner;
            this.collapsedList = new CollapsedList();
        }

        public override bool BlockDeleting(Rectangle Rect)
        {
            bool flag1 = base.BlockDeleting(Rect);
            if (flag1)
            {
                this.UpdateRange(null, false);
            }
            return flag1;
        }

        public void CheckVisible(ref int Index)
        {
            IRange range1 = this.collapsedList.FindInclusiveRange(Index);
            if (range1 != null)
            {
                Index = range1.StartPoint.Y;
            }
        }

        public override void Clear()
        {
            base.Clear();
            this.collapsedList.Clear();
            this.UpdateRange(null, true);
        }

        public int DisplayLineToLine(int Index)
        {
            Point point1 = new Point(0, Index);
            this.GetRealPoint(ref point1, false);
            return point1.Y;
        }

        public IRange FindCollapsedRange(Point Point)
        {
            return this.collapsedList.FindExactRange(Point);
        }

        public IRange FindCollapsedRange(int Index)
        {
            return this.collapsedList.FindExactRange(Index);
        }

        public int GetCollapsedRanges(int Index, IList Ranges)
        {
            return this.collapsedList.GetExactRanges(Ranges, Index);
        }

        public void GetDisplayPoint(ref Point Point)
        {
            IRange range1 = this.collapsedList.FindRange(Point);
            if (range1 != null)
            {
                Point = range1.StartPoint;
            }
            this.collapsedList.GetDisplayPoint(ref Point);
        }

        public void GetRealPoint(ref Point Point, bool RangeStart)
        {
            this.collapsedList.GetRealPoint(ref Point, RangeStart);
        }

        public bool IsVisible(Point Point)
        {
            return (this.collapsedList.FindRange(Point) == null);
        }

        public bool IsVisible(int Index)
        {
            return (this.collapsedList.FindInclusiveRange(Index) == null);
        }

        public int LineToDisplayLine(int Index)
        {
            Point point1 = new Point(0, Index);
            this.GetDisplayPoint(ref point1);
            return point1.Y;
        }

        public override bool PositionChanged(int X, int Y, int DeltaX, int DeltaY)
        {
            bool flag1 = base.PositionChanged(X, Y, DeltaX, DeltaY);
            if (flag1 && this.collapsedList.PositionChanged(X, Y, DeltaX, DeltaY))
            {
                this.UpdateRange(null, false);
            }
            return flag1;
        }

        public override void Update()
        {
            base.Update();
            this.UpdateRange(null, true);
        }

        public void UpdateRange(IOutlineRange Rng, bool Update)
        {
            if (base.UpdateCount <= 0)
            {
                Point point1 = Point.Empty;
                int num1 = 0;
                int num2 = 0;
                this.collapsedList.Clear();
                int num3 = this.owner.Lines.Count;
                this.collapsedList.BeginUpdate();
                try
                {
                    foreach (IOutlineRange range1 in this)
                    {
                        if (range1.Visible || !this.IsVisible(range1.StartPoint))
                        {
                            continue;
                        }
                        num2 += (Math.Min(range1.EndPoint.Y, (int) (num3 - 1)) - Math.Min(range1.StartPoint.Y, (int) (num3 - 1)));
                        if (range1.StartPoint.Y != point1.Y)
                        {
                            num1 = 0;
                        }
                        num1 += ((range1.EndPoint.X - range1.StartPoint.X) - range1.DisplayText.Length);
                        this.collapsedList.Add(new CollapsedRange(null, range1.StartPoint, range1.EndPoint, -num1, -num2, 0, range1.DisplayText));
                        point1 = range1.EndPoint;
                    }
                }
                finally
                {
                    this.collapsedList.EndUpdate();
                }
                this.collapsedList.CollapsedCount = num2;
                if (Update)
                {
                    this.owner.CollapseChanged(Rng);
                }
            }
        }


        // Properties
        public int CollapsedCount
        {
            get
            {
                return this.collapsedList.CollapsedCount;
            }
        }

        public DisplayStrings Owner
        {
            get
            {
                return this.owner;
            }
        }


        // Fields
        private CollapsedList collapsedList;
        private DisplayStrings owner;
    }
}

