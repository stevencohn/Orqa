namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class BookMarks : BookMarkList, IBookMarks, IList, ICollection, IEnumerable
    {
        // Methods
        public BookMarks(ITextSource Owner)
        {
            this.owner = Owner;
        }

        public void Assign(IBookMarks Source)
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                this.Clear();
                foreach (IBookMark mark1 in Source)
                {
                    this.Add(this.NewBookMark(mark1.Line, mark1.Char, mark1.Index));
                }
                this.owner.LinesChanged(0, 0x7fffffff);
                this.owner.State |= NotifyState.BookMarkChanged;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        public void ClearAllBookMarks()
        {
            this.Clear();
        }

        public void ClearBookMark(int BookMark)
        {
            int num1 = base.FindByIndex(BookMark);
            if (num1 >= 0)
            {
                this.ClearBookMarkByIndex(num1);
            }
        }

        public void ClearBookMarkByIndex(int Index)
        {
            if (Index >= 0)
            {
                this.owner.BeginUpdate(UpdateReason.Other);
                try
                {
                    int num1 = ((IBookMark) this[Index]).Line;
                    this.RemoveAt(Index);
                    this.owner.State |= NotifyState.BookMarkChanged;
                    this.owner.LinesChanged(num1, num1);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        public void ClearBookMarks(int Line)
        {
            int num1 = 0;
            if (base.FindLast(Line, out num1, this.lineComparer))
            {
                this.owner.BeginUpdate(UpdateReason.Other);
                try
                {
                    for (int num2 = num1; num2 >= 0; num2--)
                    {
                        if (((IBookMark) this[num1]).Line != Line)
                        {
                            break;
                        }
                        this.RemoveAt(num2);
                    }
                    this.owner.State |= NotifyState.BookMarkChanged;
                    this.owner.LinesChanged(Line, Line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        public bool FindBookMark(int BookMark, out Point Point)
        {
            int num1 = base.FindByIndex(BookMark);
            if (num1 >= 0)
            {
                IBookMark mark1 = (IBookMark) this[num1];
                Point = new Point(mark1.Char, mark1.Line);
                return true;
            }
            Point = new Point(0, 0);
            return false;
        }

        public int FindBookMark(int BookMark, int Line)
        {
            int num1;
            if (BookMark == 0x7fffffff)
            {
                for (num1 = base.FindByLine(Line); ((num1 >= 0) && (num1 < this.Count)) && (((IBookMark) this[num1]).Line == Line); num1++)
                {
                    if (((IBookMark) this[num1]).Index == BookMark)
                    {
                        break;
                    }
                }
            }
            else
            {
                num1 = base.FindByIndex(BookMark);
            }
            if (((num1 >= 0) && (num1 < this.Count)) && (((IBookMark) this[num1]).Line == Line))
            {
                return num1;
            }
            return -1;
        }

        public int GetBookMark(int Line)
        {
            return base.InternalGet(Line);
        }

        public int GetBookMark(Point StartPoint, Point EndPoint)
        {
            return base.InternalGet(StartPoint, EndPoint);
        }

        public int GetBookMarks(Point StartPoint, Point EndPoint, IList List)
        {
            return base.InternalGet(StartPoint, EndPoint, List);
        }

        public void GotoBookMark(int BookMark)
        {
            int num1 = base.FindByIndex(BookMark);
            if (num1 >= 0)
            {
                IBookMark mark1 = (IBookMark) this[num1];
                this.owner.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    this.owner.State |= NotifyState.GotoBookMark;
                    this.owner.MoveTo(mark1.Char, mark1.Line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        private void GotoBookMarkIndex(bool Direction)
        {
            int num1 = -1;
            if (Direction)
            {
                for (int num2 = 0; num2 < this.Count; num2++)
                {
                    IBookMark mark1 = (IBookMark) this[num2];
                    if ((mark1.Index == 0x7fffffff) && (mark1.Line > this.owner.Position.Y))
                    {
                        num1 = num2;
                        break;
                    }
                }
                if (num1 == -1)
                {
                    for (int num3 = 0; num3 < this.Count; num3++)
                    {
                        if (((IBookMark) this[num3]).Index == 0x7fffffff)
                        {
                            num1 = num3;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int num4 = this.Count - 1; num4 >= 0; num4--)
                {
                    IBookMark mark2 = (IBookMark) this[num4];
                    if ((mark2.Index == 0x7fffffff) && (mark2.Line < this.owner.Position.Y))
                    {
                        num1 = num4;
                        break;
                    }
                }
                if (num1 == -1)
                {
                    for (int num5 = this.Count - 1; num5 >= 0; num5--)
                    {
                        if (((IBookMark) this[num5]).Index == 0x7fffffff)
                        {
                            num1 = num5;
                            break;
                        }
                    }
                }
            }
            if ((num1 >= 0) && (num1 < this.Count))
            {
                IBookMark mark3 = (IBookMark) this[num1];
                this.owner.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    this.owner.State |= NotifyState.GotoBookMark;
                    this.owner.MoveTo(mark3.Char, mark3.Line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        public void GotoNextBookMark()
        {
            this.GotoBookMarkIndex(true);
        }

        public void GotoPrevBookMark()
        {
            this.GotoBookMarkIndex(false);
        }

        public int NextBookMark()
        {
            int num1 = -1;
            foreach (BookMark mark1 in this)
            {
                if (mark1.Index > num1)
                {
                    num1 = mark1.Index;
                }
            }
            return (num1 + 1);
        }

        public void SetBookMark(Point Point, int BookMark)
        {
            this.SetBookMark(Point.Y, Point.X, BookMark);
        }

        public void SetBookMark(int Line, int BookMark)
        {
            this.SetBookMark(Line, 0, BookMark);
        }

        private void SetBookMark(int Line, int Char, int BookMark)
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                if (BookMark != 0x7fffffff)
                {
                    this.ClearBookMark(BookMark);
                }
                base.InternalSet(Line, Char, BookMark);
                this.owner.LinesChanged(Line, Line);
                this.owner.State |= NotifyState.BookMarkChanged;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        public void ToggleBookMark()
        {
            this.ToggleBookMark(this.owner.Position.Y, 0x7fffffff);
        }

        public void ToggleBookMark(int BookMark)
        {
            this.ToggleBookMark(this.owner.Position, BookMark);
        }

        public void ToggleBookMark(Point Position, int BookMark)
        {
            int num1 = this.FindBookMark(BookMark, Position.Y);
            if (num1 >= 0)
            {
                this.ClearBookMarkByIndex(num1);
            }
            else
            {
                this.SetBookMark(Position.Y, Position.X, BookMark);
            }
        }

        public void ToggleBookMark(int Line, int BookMark)
        {
            int num1 = this.FindBookMark(BookMark, Line);
            if (num1 >= 0)
            {
                this.ClearBookMarkByIndex(num1);
            }
            else
            {
                this.SetBookMark(Line, BookMark);
            }
        }


        // Fields
        private ITextSource owner;
    }
}

