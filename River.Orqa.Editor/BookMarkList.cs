namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;

    public class BookMarkList : SortList
    {
        // Methods
        public BookMarkList()
        {
            this.lineComparer = new LineComparer();
            this.pointComparer = new River.Orqa.Editor.BookMarkList.PointComparer();
            this.bookMarkComparer = new BookMarkComparer();
            this.rangeComparer = new River.Orqa.Editor.BookMarkList.RangeComparer();
        }

        protected internal void BlockDeleting(Rectangle Rect)
        {
            int num1;
            base.FindFirst(Rect.Bottom, out num1, this.lineComparer);
            if (num1 >= 0)
            {
                for (int num2 = Math.Min(num1, (int) (this.Count - 1)); num2 >= 0; num2--)
                {
                    BookMark mark1 = (BookMark) this[num2];
                    if (mark1.Line < Rect.Top)
                    {
                        return;
                    }
                    if (this.ShouldDelete(mark1, Rect))
                    {
                        this.RemoveAt(num2);
                    }
                }
            }
        }

        public int FindByIndex(int Index)
        {
            for (int num1 = 0; num1 < this.Count; num1++)
            {
                if (((IBookMark) this[num1]).Index == Index)
                {
                    return num1;
                }
            }
            return -1;
        }

        public int FindByLine(int Line)
        {
            int num1;
            if (base.FindFirst(Line, out num1, this.lineComparer))
            {
                return num1;
            }
            return -1;
        }

        protected internal void InternalClear(int Index)
        {
            int num1 = this.FindByIndex(Index);
            if (num1 >= 0)
            {
                this.RemoveAt(num1);
            }
        }

        protected internal void InternalClearLine(int Line)
        {
            int num1 = 0;
            if (base.FindLast(Line, out num1, this.lineComparer))
            {
                for (int num2 = num1; num2 >= 0; num2--)
                {
                    if (((IBookMark) this[num1]).Line != Line)
                    {
                        return;
                    }
                    this.RemoveAt(num2);
                }
            }
        }

        protected internal int InternalGet(int Line)
        {
            int num1;
            if (base.FindFirst(Line, out num1, this.lineComparer))
            {
                return ((IBookMark) this[num1]).Index;
            }
            return -1;
        }

        protected internal int InternalGet(Point StartPoint, Point EndPoint)
        {
            int num1;
            River.Orqa.Editor.Common.Range range1 = new River.Orqa.Editor.Common.Range(StartPoint, EndPoint);
            if (base.FindFirst(range1, out num1, this.rangeComparer))
            {
                return ((IBookMark) this[num1]).Index;
            }
            return -1;
        }

        protected internal int InternalGet(Point StartPoint, Point EndPoint, IList List)
        {
            int num1;
            List.Clear();
            River.Orqa.Editor.Common.Range range1 = new River.Orqa.Editor.Common.Range(StartPoint, EndPoint);
            if (base.FindFirst(range1, out num1, this.rangeComparer))
            {
                for (int num2 = num1; num2 < this.Count; num2++)
                {
                    if (this.rangeComparer.Compare(this[num2], range1) != 0)
                    {
                        break;
                    }
                    List.Add(this[num2]);
                }
            }
            return List.Count;
        }

        protected internal void InternalSet(int Line, int Index)
        {
            int num1;
            if (base.FindFirst(Line, out num1, this.lineComparer))
            {
                this.RemoveAt(num1);
            }
            this.Insert(num1, this.NewBookMark(Line, 0, Index));
        }

        protected internal void InternalSet(int Line, int Char, int Index)
        {
            int num1;
            if (base.FindFirst(new Point(Char, Line), out num1, this.pointComparer))
            {
                this.RemoveAt(num1);
            }
            this.Insert(num1, this.NewBookMark(Line, Char, Index));
        }

        protected internal virtual BookMark NewBookMark(int Line, int Char, int Index)
        {
            return new BookMark(Line, Char, Index);
        }

        protected internal void PositionChanged(int X, int Y, int DeltaX, int DeltaY)
        {
            int num1;
            base.FindFirst(Y, out num1, this.lineComparer);
            if (num1 >= 0)
            {
                bool flag1 = false;
                for (int num2 = num1; num2 < this.Count; num2++)
                {
                    BookMark mark1 = (BookMark) this[num2];
                    Point point1 = new Point(mark1.Char, mark1.Line);
                    if (SortList.UpdatePos(X, Y, DeltaX, DeltaY, ref point1, true))
                    {
                        if (mark1.Index != 0x7fffffff)
                        {
                            mark1.Char = point1.X;
                        }
                        mark1.Line = point1.Y;
                        flag1 = true;
                    }
                }
                if (flag1)
                {
                    this.Sort(this.bookMarkComparer);
                }
            }
        }

        protected internal virtual bool ShouldDelete(BookMark bm, Rectangle Rect)
        {
            if (bm.Index != 0x7fffffff)
            {
                return SortList.InsideBlock(new Point(bm.Char, bm.Line), Rect);
            }
            if (SortList.InsideBlock(new Point(0, bm.Line), Rect))
            {
                return SortList.InsideBlock(new Point(0x7fffffff, bm.Line), Rect);
            }
            return false;
        }


        // Fields
        protected IComparer bookMarkComparer;
        protected IComparer lineComparer;
        protected IComparer pointComparer;
        protected IComparer rangeComparer;

        // Nested Types
        private class BookMarkComparer : IComparer
        {
            // Methods
            public BookMarkComparer()
            {
            }

            public int Compare(object x, object y)
            {
                IBookMark mark1 = (IBookMark) x;
                IBookMark mark2 = (IBookMark) y;
                int num1 = mark1.Line - mark2.Line;
                if (num1 == 0)
                {
                    num1 = mark1.Char - mark2.Char;
                }
                if (num1 == 0)
                {
                    num1 = mark1.Index - mark2.Index;
                }
                return num1;
            }

        }

        private class LineComparer : IComparer
        {
            // Methods
            public LineComparer()
            {
            }

            public int Compare(object x, object y)
            {
                return (((IBookMark) x).Line - ((int) y));
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
                IBookMark mark1 = (IBookMark) x;
                Point point1 = (Point) y;
                int num1 = mark1.Line - point1.Y;
                if (num1 == 0)
                {
                    num1 = mark1.Char - point1.X;
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
                IBookMark mark1 = (IBookMark) x;
                Point point1 = ((IRange) y).StartPoint;
                Point point2 = ((IRange) y).EndPoint;
                if ((mark1.Line < point1.Y) || ((mark1.Line == point1.Y) && (mark1.Char < point1.X)))
                {
                    return -1;
                }
                if ((mark1.Line <= point2.Y) && ((mark1.Line != point2.Y) || (mark1.Char < point2.X)))
                {
                    return 0;
                }
                return 1;
            }

        }
    }
}

