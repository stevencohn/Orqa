namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Runtime.InteropServices;

    public class Outlining : IOutlining, ICollapsable
    {
        // Methods
        public Outlining(ISyntaxEdit Owner)
        {
            this.outlineColor = EditConsts.DefaultOutlineForeColor;
            this.owner = Owner;
            if (this.owner != null)
            {
                this.displayLines = (DisplayStrings) this.owner.DisplayLines;
            }
        }

        public void Assign(IOutlining Source)
        {
            this.AllowOutlining = Source.AllowOutlining;
            this.OutlineOptions = Source.OutlineOptions;
            this.OutlineColor = Source.OutlineColor;
        }

        public void Collapse(int Index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.Collapse(Index);
            }
        }

        public void EnsureExpanded(Point Point)
        {
            if (this.displayLines != null)
            {
                this.displayLines.EnsureExpanded(Point);
            }
        }

        public void EnsureExpanded(int Index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.EnsureExpanded(Index);
            }
        }

        public void Expand(int Index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.Expand(Index);
            }
        }

        public string GetOutlineHint(IOutlineRange Range)
        {
            if (this.displayLines == null)
            {
                return string.Empty;
            }
            return this.displayLines.GetOutlineHint(Range);
        }

        public IOutlineRange GetOutlineRange(Point Point)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.GetOutlineRange(Point);
        }

        public IOutlineRange GetOutlineRange(int Index)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.GetOutlineRange(Index);
        }

        public int GetOutlineRanges(IList Ranges)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(Ranges);
        }

        public int GetOutlineRanges(IList Ranges, Point Point)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(Ranges, Point);
        }

        public int GetOutlineRanges(IList Ranges, int Index)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(Ranges, Index);
        }

        public int GetOutlineRanges(IList Ranges, Point StartPoint, Point EndPoint)
        {
            if (this.displayLines == null)
            {
                return 0;
            }
            return this.displayLines.GetOutlineRanges(Ranges, StartPoint, EndPoint);
        }

        public bool IsCollapsed(int Index)
        {
            if (this.displayLines == null)
            {
                return false;
            }
            return this.displayLines.IsCollapsed(Index);
        }

        public bool IsExpanded(int Index)
        {
            if (this.displayLines == null)
            {
                return false;
            }
            return this.displayLines.IsExpanded(Index);
        }

        protected internal bool IsMouseOnOutlineButton(int X, int Y, out IOutlineRange Range)
        {
            Range = null;
            if (((this.owner != null) && this.AllowOutlining) && ((this.OutlineOptions & River.Orqa.Editor.OutlineOptions.DrawButtons) != River.Orqa.Editor.OutlineOptions.None))
            {
                Rectangle rectangle1;
                if (this.owner.Pages.PageType == PageType.PageLayout)
                {
                    IEditPage page1 = this.owner.Pages.GetPageAtPoint(X, Y);
                    rectangle1 = page1.ClientRect;
                }
                else
                {
                    rectangle1 = this.owner.ClientRect;
                }
                int num1 = ((Gutter) this.owner.Gutter).GetWidth();
                rectangle1.X += num1;
                rectangle1.Width -= num1;
                if (rectangle1.Contains(X, Y))
                {
                    Point point1 = this.owner.ScreenToDisplay(X, Y);
                    point1 = this.displayLines.DisplayPointToPoint(point1.X, point1.Y, false, true, false);
                    Range = this.displayLines.GetOutlineRange(point1);
                    if (Range != null)
                    {
                        return !Range.Visible;
                    }
                }
            }
            return false;
        }

        public bool IsVisible(Point Point)
        {
            if (this.displayLines == null)
            {
                return true;
            }
            return this.displayLines.IsVisible(Point);
        }

        public bool IsVisible(int Index)
        {
            if (this.displayLines == null)
            {
                return true;
            }
            return this.displayLines.IsVisible(Index);
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(StartPoint, EndPoint);
        }

        public IOutlineRange Outline(int First, int Last)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(First, Last);
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint, int Level)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(StartPoint, EndPoint, Level);
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint, string OutlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(StartPoint, EndPoint, OutlineText);
        }

        public IOutlineRange Outline(int First, int Last, int Level)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(First, Last, Level);
        }

        public IOutlineRange Outline(int First, int Last, string OutlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(First, Last, OutlineText);
        }

        public IOutlineRange Outline(Point StartPoint, Point EndPoint, int Level, string OutlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(StartPoint, EndPoint, Level, OutlineText);
        }

        public IOutlineRange Outline(int First, int Last, int Level, string OutlineText)
        {
            if (this.displayLines == null)
            {
                return null;
            }
            return this.displayLines.Outline(First, Last, Level, OutlineText);
        }

        public void OutlineText()
        {
            if ((this.AllowOutlining && (this.owner != null)) && ((TextSource) this.owner.Source).NeedOutlineText())
            {
                IFormatText text1 = (IFormatText) this.owner.Source.Lexer;
                text1.Strings = this.owner.Lines;
                text1.ReparseText();
                ArrayList list1 = new ArrayList();
                text1.Outline(list1);
                this.SetOutlineRanges(list1, true);
            }
        }

        public virtual void ResetAllowOutlining()
        {
            this.AllowOutlining = false;
        }

        public virtual void ResetOutlineColor()
        {
            this.OutlineColor = EditConsts.DefaultOutlineForeColor;
        }

        public virtual void ResetOutlineOptions()
        {
            this.OutlineOptions = EditConsts.DefaultOutlineOptions;
        }

        public void SetOutlineRanges(IList Ranges)
        {
            if (this.displayLines != null)
            {
                this.displayLines.SetOutlineRanges(Ranges);
            }
        }

        public void SetOutlineRanges(IList Ranges, bool PreserveVisible)
        {
            if (this.displayLines != null)
            {
                this.displayLines.SetOutlineRanges(Ranges, PreserveVisible);
            }
        }

        public bool ShouldSerializeOutlineColor()
        {
            return (this.outlineColor != EditConsts.DefaultOutlineForeColor);
        }

        public bool ShouldSerializeOutlineOptions()
        {
            return (this.OutlineOptions != EditConsts.DefaultOutlineOptions);
        }

        public void ToggleOutlining()
        {
            if (this.displayLines != null)
            {
                this.displayLines.ToggleOutlining();
            }
        }

        public void UnOutline()
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline();
            }
        }

        public void UnOutline(Point Point)
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline(Point);
            }
        }

        public void UnOutline(int Index)
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline(Index);
            }
        }

        public void UnOutlineText()
        {
            if (this.displayLines != null)
            {
                this.displayLines.UnOutline();
            }
        }


        // Properties
        [DefaultValue(false)]
        public bool AllowOutlining
        {
            get
            {
                if (this.displayLines == null)
                {
                    return false;
                }
                return this.displayLines.AllowOutlining;
            }
            set
            {
                if (this.displayLines != null)
                {
                    this.displayLines.AllowOutlining = value;
                }
            }
        }

        public Color OutlineColor
        {
            get
            {
                return this.outlineColor;
            }
            set
            {
                if (this.outlineColor != value)
                {
                    this.outlineColor = value;
                    if (this.owner != null)
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        [Category("SyntaxEdit"), Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public River.Orqa.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                if (this.displayLines == null)
                {
                    return River.Orqa.Editor.OutlineOptions.None;
                }
                return this.displayLines.OutlineOptions;
            }
            set
            {
                if (this.displayLines != null)
                {
                    this.displayLines.OutlineOptions = value;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlOutliningInfo(this);
            }
            set
            {
                ((XmlOutliningInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private DisplayStrings displayLines;
        private Color outlineColor;
        private ISyntaxEdit owner;
    }
}

