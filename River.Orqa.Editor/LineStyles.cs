namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;

    public class LineStyles : BookMarkList, ILineStyles, IList, ICollection, IEnumerable
    {
        // Methods
        public LineStyles(ITextSource Owner)
        {
            this.owner = Owner;
        }

        public void Assign(ILineStyles Source)
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                this.Clear();
                foreach (LStyle style1 in Source)
                {
                    this.Add(this.NewBookMark(style1.Line, style1.Char, style1.Index));
                }
                this.owner.LinesChanged(0, 0x7fffffff);
                this.owner.State |= NotifyState.BookMarkChanged;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        public int GetLineStyle(int Line)
        {
            return base.InternalGet(Line);
        }

        public LStyle GetLStyle(int Index)
        {
            return (base[Index] as LStyle);
        }

        protected internal override BookMark NewBookMark(int Line, int Char, int Index)
        {
            return new LStyle(Line, Char, Index);
        }

        public void RemoveLineStyle(int Line)
        {
            int num1 = base.FindByLine(Line);
            if (num1 >= 0)
            {
                if (this.owner != null)
                {
                    this.owner.BeginUpdate(UpdateReason.Other);
                    try
                    {
                        this.RemoveAt(num1);
                        this.owner.State |= NotifyState.BookMarkChanged;
                        this.owner.LinesChanged(Line, Line);
                        return;
                    }
                    finally
                    {
                        this.owner.EndUpdate();
                    }
                }
                this.RemoveAt(num1);
            }
        }

        public void SetLineStyle(int Line, int Style)
        {
            if (this.owner != null)
            {
                this.owner.BeginUpdate(UpdateReason.Other);
                try
                {
                    int num1 = base.FindByLine(Line);
                    if (num1 >= 0)
                    {
                        ((BookMark) base[num1]).Index = Style;
                    }
                    else
                    {
                        base.InternalSet(Line, Style);
                    }
                    this.owner.LinesChanged(Line, Line);
                    this.owner.State |= NotifyState.BookMarkChanged;
                    return;
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
            int num2 = base.FindByLine(Line);
            if (num2 >= 0)
            {
                ((BookMark) base[num2]).Index = Style;
            }
            else
            {
                base.InternalSet(Line, Style);
            }
        }

        protected internal override bool ShouldDelete(BookMark bm, Rectangle Rect)
        {
            if (SortList.InsideBlock(new Point(0, bm.Line), Rect))
            {
                return SortList.InsideBlock(new Point(0x7fffffff, bm.Line), Rect);
            }
            return false;
        }

        public void ToggleLineStyle(int Line, int Style)
        {
            if (base.FindByLine(Line) >= 0)
            {
                this.RemoveLineStyle(Line);
            }
            else
            {
                this.SetLineStyle(Line, Style);
            }
        }


        // Fields
        private ITextSource owner;
    }
}

