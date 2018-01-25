namespace River.Orqa.Editor.Common
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class RangeList : SortList
    {
        // Methods
        public RangeList()
        {
            this.topRange = new SortRange(null, -1);
            this.rangeComparer = new RangeComparer();
            this.pointComparer = new PointComparer();
            this.ptComparer = new PtComparer();
            this.pointHitTest = new PointHitTest();
            this.lineHitTest = new LineHitTest();
            this.inclusiveLineHitTest = new InclusiveLineHitTest();
        }

        public override int Add(object value)
        {
            if (this.updateCount == 0)
            {
                int num1;
                IRange range1 = (IRange) value;
                if (base.FindLast(range1, out num1, this.rangeComparer))
                {
                    this.RemoveAt(num1);
                }
                this.Insert(num1, range1);
                return num1;
            }
            this.needSort = true;
            return base.Add(value);
        }

        public int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual bool BlockDeleting(Rectangle Rect)
        {
            bool flag1 = false;
            this.BeginUpdate();
            try
            {
                int num1;
                if (base.FindLast(new Point(Rect.Right, Rect.Bottom), out num1, this.ptComparer))
                {
                    num1++;
                }
                for (int num2 = Math.Min(num1, (int) (this.Count - 1)); num2 >= 0; num2--)
                {
                    IRange range1 = (IRange) this[num2];
                    if (range1.EndPoint.Y < Rect.Top)
                    {
                        return flag1;
                    }
                    if (SortList.InsideBlock(range1.StartPoint, Rect) && SortList.InsideBlock(new Point(Math.Max((int) (range1.EndPoint.X - 1), 0), range1.EndPoint.Y), Rect))
                    {
                        this.RemoveAt(num2);
                        flag1 = true;
                    }
                }
            }
            finally
            {
                this.EndUpdate();
            }
            return flag1;
        }

        public override void Clear()
        {
            base.Clear();
            this.topRange.Clear();
        }

        public int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        public IRange FindExactRange(Point Point)
        {
            IRange range1 = this.FindRange(Point, this.pointHitTest);
            if ((range1 != null) && range1.StartPoint.Equals(Point))
            {
                return range1;
            }
            return null;
        }

        public IRange FindExactRange(int Index)
        {
            IRange range1 = this.FindRange(Index, this.lineHitTest);
            if ((range1 != null) && (range1.StartPoint.Y == Index))
            {
                return range1;
            }
            return null;
        }

        public IRange FindInclusiveRange(int Index)
        {
            return this.FindRange(Index, this.inclusiveLineHitTest);
        }

        public IRange FindRange(Point Point)
        {
            return this.FindRange(Point, this.pointHitTest);
        }

        public IRange FindRange(int Index)
        {
            return this.FindRange(Index, this.lineHitTest);
        }

        protected IRange FindRange(Point Point, IComparer Comparer)
        {
            int num1;
            SortRange range1 = this.topRange;
            IRange range2 = null;
            while (range1.Ranges.FindLast(Point, out num1, Comparer))
            {
                range1 = (SortRange) range1.Ranges[num1];
                range2 = range1.Range;
            }
            return range2;
        }

        protected IRange FindRange(int Index, IComparer Comparer)
        {
            int num1;
            SortRange range1 = this.topRange;
            IRange range2 = null;
            while (range1.Ranges.FindLast(Index, out num1, Comparer))
            {
                range1 = (SortRange) range1.Ranges[num1];
                range2 = range1.Range;
            }
            return range2;
        }

        protected int FindRangeIndex(Point Point, IComparer Comparer)
        {
            int num2;
            SortRange range1 = this.topRange;
            int num1 = -1;
            while (range1.Ranges.FindLast(Point, out num2, Comparer))
            {
                range1 = (SortRange) range1.Ranges[num2];
                num1 = range1.Index;
            }
            return num1;
        }

        protected int FindRangeIndex(int Index, IComparer Comparer)
        {
            int num2;
            SortRange range1 = this.topRange;
            int num1 = -1;
            while (range1.Ranges.FindLast(Index, out num2, Comparer))
            {
                range1 = (SortRange) range1.Ranges[num2];
                num1 = range1.Index;
            }
            return num1;
        }

        public int GetExactRanges(IList Ranges, int Index)
        {
            int num1;
            Ranges.Clear();
            SortRange range1 = this.topRange;
            while (range1.Ranges.FindLast(Index, out num1, this.lineHitTest))
            {
                range1 = (SortRange) range1.Ranges[num1];
                if ((range1.Range != null) && (range1.Range.StartPoint.Y == Index))
                {
                    Ranges.Add(range1.Range);
                }
            }
            return Ranges.Count;
        }

        public IList GetRanges()
        {
            ArrayList list1 = new ArrayList();
            this.GetRanges(list1);
            return list1;
        }

        public int GetRanges(IList Ranges)
        {
            Ranges.Clear();
            foreach (IRange range1 in this)
            {
                Ranges.Add(range1);
            }
            return Ranges.Count;
        }

        public int GetRanges(IList Ranges, Point Point)
        {
            int num1;
            Ranges.Clear();
            SortRange range1 = this.topRange;
            while (range1.Ranges.FindLast(Point, out num1, this.pointHitTest))
            {
                range1 = (SortRange) range1.Ranges[num1];
                if (range1.Range != null)
                {
                    Ranges.Add(range1.Range);
                }
            }
            return Ranges.Count;
        }

        public int GetRanges(IList Ranges, int Index)
        {
            int num1;
            Ranges.Clear();
            SortRange range1 = this.topRange;
            while (range1.Ranges.FindLast(Index, out num1, this.lineHitTest))
            {
                range1 = (SortRange) range1.Ranges[num1];
                if (range1.Range != null)
                {
                    Ranges.Add(range1.Range);
                }
            }
            return Ranges.Count;
        }

        public int GetRanges(IList Ranges, Point StartPoint, Point EndPoint)
        {
            Ranges.Clear();
            River.Orqa.Editor.Common.Range range1 = new River.Orqa.Editor.Common.Range(StartPoint, EndPoint);
            foreach (IRange range2 in this)
            {
                if ((this.pointHitTest.Compare(range1, range2.StartPoint) == 0) && (this.pointHitTest.Compare(range1, range2.EndPoint) == 0))
                {
                    Ranges.Add(range2);
                }
            }
            return Ranges.Count;
        }

        public override void Insert(int index, object value)
        {
            base.Insert(index, value);
            if (this.updateCount == 0)
            {
                this.SortLevels();
            }
        }

        public virtual bool PositionChanged(int X, int Y, int DeltaX, int DeltaY)
        {
            return this.topRange.PositionChanged(X, Y, DeltaX, DeltaY, this, this.pointComparer);
        }

        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);
            if (this.updateCount == 0)
            {
                this.SortLevels();
            }
        }

        public void RemoveRange(Point Point)
        {
            int num1;
            ArrayList list1 = new ArrayList();
            for (SortRange range1 = this.topRange; range1.Ranges.FindLast(Point, out num1, this.pointHitTest); range1 = (SortRange) range1.Ranges[num1])
            {
                if (range1.Index >= 0)
                {
                    list1.Add(range1.Index);
                }
            }
            if (list1.Count > 0)
            {
                this.BeginUpdate();
                try
                {
                    for (int num2 = list1.Count - 1; num2 >= 0; num2--)
                    {
                        this.RemoveAt((int) list1[num2]);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void RemoveRange(int Index)
        {
            int num1;
            ArrayList list1 = new ArrayList();
            for (SortRange range1 = this.topRange; range1.Ranges.FindLast(Index, out num1, this.lineHitTest); range1 = (SortRange) range1.Ranges[num1])
            {
                if (range1.Index >= 0)
                {
                    list1.Add(range1.Index);
                }
            }
            if (list1.Count > 0)
            {
                this.BeginUpdate();
                try
                {
                    for (int num2 = list1.Count - 1; num2 >= 0; num2--)
                    {
                        this.RemoveAt((int) list1[num2]);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public override void Sort()
        {
            if (this.updateCount == 0)
            {
                this.needSort = false;
                this.Sort(this.rangeComparer);
                this.SortLevels();
            }
        }

        protected void SortLevels()
        {
            this.topRange.Clear();
            ArrayList list1 = new ArrayList();
            SortRange range2 = this.topRange;
            for (int num1 = 0; num1 < this.Count; num1++)
            {
                IRange range3 = (IRange) this[num1];
                SortRange range1 = new SortRange(range3, num1);
                while ((list1.Count > 0) && !range2.Contains(range3))
                {
                    range2 = (SortRange) list1[list1.Count - 1];
                    list1.RemoveAt(list1.Count - 1);
                }
                list1.Add(range2);
                range2.Add(range1);
                range2 = range1;
            }
        }

        public virtual void Update()
        {
            if (this.needSort)
            {
                this.Sort();
            }
        }

        protected virtual bool UpdatePosition(IRange Range, int X, int Y, int DeltaX, int DeltaY)
        {
            Point point1 = Range.StartPoint;
            bool flag1 = false;
            if (SortList.UpdatePos(X, Y, DeltaX, DeltaY, ref point1, false))
            {
                Range.StartPoint = point1;
                flag1 = true;
            }
            point1 = Range.EndPoint;
            if (SortList.UpdatePos(X, Y, DeltaX, DeltaY, ref point1, true))
            {
                Range.EndPoint = point1;
                flag1 = true;
            }
            return flag1;
        }


        // Properties
        protected SortRange TopRange
        {
            get
            {
                return this.topRange;
            }
        }

        public int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }


        // Fields
        private IComparer inclusiveLineHitTest;
        private IComparer lineHitTest;
        private bool needSort;
        private IComparer pointComparer;
        private IComparer pointHitTest;
        private IComparer ptComparer;
        private IComparer rangeComparer;
        private SortRange topRange;
        private int updateCount;

        // Nested Types
        private class InclusiveLineHitTest : IComparer
        {
            // Methods
            public InclusiveLineHitTest()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = ((RangeList.SortRange) x).Range.StartPoint;
                Point point2 = ((RangeList.SortRange) x).Range.EndPoint;
                int num1 = (int) y;
                if (num1 <= point1.Y)
                {
                    return 1;
                }
                if (num1 > point2.Y)
                {
                    return -1;
                }
                return 0;
            }

        }

        private class LineHitTest : IComparer
        {
            // Methods
            public LineHitTest()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = ((RangeList.SortRange) x).Range.StartPoint;
                Point point2 = ((RangeList.SortRange) x).Range.EndPoint;
                int num1 = (int) y;
                if (num1 < point1.Y)
                {
                    return 1;
                }
                if (num1 > point2.Y)
                {
                    return -1;
                }
                return 0;
            }

        }

        private class PointComparer : IComparer
        {
            // Methods
            public PointComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = ((RangeList.SortRange) x).Range.StartPoint;
                Point point2 = (Point) y;
                int num1 = point1.Y - point2.Y;
                if (num1 == 0)
                {
                    num1 = point1.X - point2.X;
                }
                return num1;
            }

        }

        private class PointHitTest : IComparer
        {
            // Methods
            public PointHitTest()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = ((RangeList.SortRange) x).Range.StartPoint;
                Point point2 = ((RangeList.SortRange) x).Range.EndPoint;
                Point point3 = (Point) y;
                if ((point3.Y <= point1.Y) && ((point3.Y != point1.Y) || (point3.X < point1.X)))
                {
                    return 1;
                }
                if ((point3.Y < point2.Y) || ((point3.Y == point2.Y) && (point3.X < point2.X)))
                {
                    return 0;
                }
                return -1;
            }

        }

        private class PtComparer : IComparer
        {
            // Methods
            public PtComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = ((IRange) x).StartPoint;
                Point point2 = (Point) y;
                int num1 = point1.Y - point2.Y;
                if (num1 == 0)
                {
                    num1 = point1.X - point2.X;
                }
                return num1;
            }

        }

        private class RangeComparer : IComparer
        {
            // Methods
            public RangeComparer()
            {
            }

            public int Compare(object x, object y)
            {
                IRange range1 = (IRange) x;
                IRange range2 = (IRange) y;
                int num1 = range1.StartPoint.Y - range2.StartPoint.Y;
                if (num1 == 0)
                {
                    num1 = range1.StartPoint.X - range2.StartPoint.X;
                }
                return num1;
            }

        }

        protected class SortRange
        {
            // Methods
            public SortRange(IRange Range, int Index)
            {
                this.index = Index;
                this.range = Range;
                this.ranges = new SortList();
            }

            public int Add(RangeList.SortRange Range)
            {
                return this.ranges.Add(Range);
            }

            public void Clear()
            {
                foreach (RangeList.SortRange range1 in this.ranges)
                {
                    range1.Clear();
                }
                this.ranges.Clear();
            }

            public bool Contains(IRange Range)
            {
                if (this.range == null)
                {
                    return true;
                }
                if ((this.range.StartPoint.Y < Range.StartPoint.Y) || ((this.range.StartPoint.Y == Range.StartPoint.Y) && (this.range.StartPoint.X < Range.StartPoint.X)))
                {
                    if (this.range.EndPoint.Y > Range.EndPoint.Y)
                    {
                        return true;
                    }
                    if (this.range.EndPoint.Y == Range.EndPoint.Y)
                    {
                        return (this.range.EndPoint.X >= Range.EndPoint.X);
                    }
                    return false;
                }
                return false;
            }

            public virtual bool PositionChanged(int X, int Y, int DeltaX, int DeltaY, RangeList List, IComparer Comparer)
            {
                int num1;
                bool flag1 = false;
                Point point1 = new Point(X, Y);
                if (!this.ranges.FindLast(point1, out num1, Comparer))
                {
                    num1--;
                }
                for (int num2 = Math.Max(num1, 0); num2 < this.ranges.Count; num2++)
                {
                    RangeList.SortRange range1 = (RangeList.SortRange) this.ranges[num2];
                    point1 = range1.Range.StartPoint;
                    if ((DeltaY == 0) && (point1.Y > Y))
                    {
                        break;
                    }
                    if (List.UpdatePosition(range1.Range, X, Y, DeltaX, DeltaY))
                    {
                        flag1 = true;
                    }
                    if (range1.PositionChanged(X, Y, DeltaX, DeltaY, List, Comparer))
                    {
                        flag1 = true;
                    }
                }
                return flag1;
            }


            // Properties
            public int Index
            {
                get
                {
                    return this.index;
                }
            }

            public IRange Range
            {
                get
                {
                    return this.range;
                }
            }

            public SortList Ranges
            {
                get
                {
                    return this.ranges;
                }
            }


            // Fields
            private int index;
            private IRange range;
            private SortList ranges;
        }
    }
}

