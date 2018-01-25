namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;

    internal class CollapsedList : RangeList
    {
        // Methods
        public CollapsedList()
        {
            this.realComparer = new RealComparer();
            this.displayComparer = new DisplayComparer();
        }

        public override int Add(object value)
        {
            int num1 = base.Add(value);
            base.TopRange.Add(new RangeList.SortRange((IRange) value, num1));
            return num1;
        }

        public void GetDisplayPoint(ref Point Point)
        {
            CollapsedRange range1 = base.FindRange(Point, this.realComparer) as CollapsedRange;
            if (range1 != null)
            {
                if ((Point.Y == range1.EndPoint.Y) && (Point.X != 0x7fffffff))
                {
                    Point.X += range1.DeltaX;
                }
                if (Point.Y != 0x7fffffff)
                {
                    Point.Y += range1.DeltaY;
                }
            }
        }

        public void GetRealPoint(ref Point Point, bool RangeStart)
        {
            CollapsedRange range1 = base.FindRange(Point, this.displayComparer) as CollapsedRange;
            if (range1 != null)
            {
                int num1 = range1.EndPoint.Y + range1.DeltaY;
                int num2 = range1.EndPoint.X + range1.DeltaX;
                if ((Point.Y == num1) && (Point.X != 0x7fffffff))
                {
                    int num3 = num2;
                    if ((Point.X < num3) && RangeStart)
                    {
                        Point.X = num3 - range1.DisplayText.Length;
                        Point.Y = range1.StartPoint.Y;
                        return;
                    }
                    if (Point.X < num3)
                    {
                        Point.X = num3;
                    }
                    Point.X += (range1.EndPoint.X - num3);
                }
                if (Point.Y != 0x7fffffff)
                {
                    Point.Y += (range1.EndPoint.Y - num1);
                }
            }
        }

        protected override bool UpdatePosition(IRange Range, int X, int Y, int DeltaX, int DeltaY)
        {
            int num1 = Range.EndPoint.X - Range.StartPoint.X;
            int num2 = Range.EndPoint.Y - Range.StartPoint.Y;
            if (!base.UpdatePosition(Range, X, Y, DeltaX, DeltaY))
            {
                return false;
            }
            if ((Range.EndPoint.X - Range.StartPoint.X) == num1)
            {
                return ((Range.EndPoint.Y - Range.StartPoint.Y) != num2);
            }
            return true;
        }


        // Properties
        public int CollapsedCount
        {
            get
            {
                return this.collapsedCount;
            }
            set
            {
                this.collapsedCount = value;
            }
        }


        // Fields
        private int collapsedCount;
        private IComparer displayComparer;
        private IComparer realComparer;

        // Nested Types
        private class DisplayComparer : IComparer
        {
            // Methods
            public DisplayComparer()
            {
            }

            public int Compare(object x, object y)
            {
                CollapsedRange range1 = ((RangeList.SortRange) x).Range as CollapsedRange;
                int num1 = range1.EndPoint.Y + range1.DeltaY;
                int num2 = (range1.EndPoint.X + range1.DeltaX) - Math.Max((int) (range1.DisplayText.Length - 1), 0);
                Point point1 = (Point) y;
                if ((point1.Y >= num1) && ((point1.Y != num1) || (point1.X >= num2)))
                {
                    return 0;
                }
                return 1;
            }

        }

        private class RealComparer : IComparer
        {
            // Methods
            public RealComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Point point1 = ((RangeList.SortRange) x).Range.EndPoint;
                Point point2 = (Point) y;
                if ((point2.Y >= point1.Y) && ((point2.Y != point1.Y) || (point2.X >= point1.X)))
                {
                    return 0;
                }
                return 1;
            }

        }
    }
}

