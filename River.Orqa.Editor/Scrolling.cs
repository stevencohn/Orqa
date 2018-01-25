namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Scrolling : IScrolling
    {
        // Events
        [Browsable(false)]
        public event EventHandler HorizontalScroll;
        [Browsable(false)]
        public event EventHandler VerticalScroll;

        // Methods
        public Scrolling(ISyntaxEdit Owner)
        {
            this.scrollBars = RichTextBoxScrollBars.Both;
            this.defaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;
            this.smoothScroll = true;
            this.showScrollHint = false;
            this.owner = Owner;
        }

        public void Assign(IScrolling Source)
        {
            this.ScrollBars = Source.ScrollBars;
            this.DefaultHorzScrollSize = Source.DefaultHorzScrollSize;
            this.WindowOriginX = Source.WindowOriginX;
            this.WindowOriginY = Source.WindowOriginY;
            this.SmoothScroll = Source.SmoothScroll;
            this.ShowScrollHint = Source.ShowScrollHint;
        }

        protected internal void DoHorizontalScroll(int Pos)
        {
            this.DoHorizontalScroll(4, Pos);
        }

        protected internal void DoHorizontalScroll(short Code, int Pos)
        {
            if ((this.scrollUpdateCount <= 0) && (this.owner != null))
            {
                switch (Code)
                {
                    case 0:
                    {
                        this.WindowOriginX -= ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.Painter.FontWidth : 1);
                        return;
                    }
                    case 1:
                    {
                        this.WindowOriginX += ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.Painter.FontWidth : 1);
                        return;
                    }
                    case 2:
                    {
                        this.WindowOriginX -= ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.ClientRect.Width : this.owner.CharsInWidth());
                        return;
                    }
                    case 3:
                    {
                        this.WindowOriginX += ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.ClientRect.Width : this.owner.CharsInWidth());
                        return;
                    }
                    case 4:
                    {
                        this.WindowOriginX = Pos;
                        return;
                    }
                    case 5:
                    {
                        if (this.smoothScroll)
                        {
                            this.WindowOriginX = Pos;
                        }
                        return;
                    }
                    case 6:
                    {
                        this.WindowOriginX = 0;
                        return;
                    }
                    case 7:
                    {
                        this.WindowOriginX = this.ScrollWidth();
                        return;
                    }
                    case 8:
                    {
                        return;
                    }
                }
            }
        }

        protected internal void DoVerticalScroll(int Pos)
        {
            this.DoVerticalScroll(4, Pos);
        }

        protected internal void DoVerticalScroll(short Code, int Pos)
        {
            if ((this.scrollUpdateCount <= 0) && (this.owner != null))
            {
                switch (Code)
                {
                    case 0:
                    {
                        this.WindowOriginY -= ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.Painter.FontHeight : 1);
                        return;
                    }
                    case 1:
                    {
                        this.WindowOriginY += ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.Painter.FontHeight : 1);
                        return;
                    }
                    case 2:
                    {
                        this.WindowOriginY -= ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.ClientRect.Height : this.owner.LinesInHeight());
                        return;
                    }
                    case 3:
                    {
                        this.WindowOriginY += ((this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.ClientRect.Height : this.owner.LinesInHeight());
                        return;
                    }
                    case 4:
                    {
                        this.WindowOriginY = Pos;
                        return;
                    }
                    case 5:
                    {
                        if (this.smoothScroll)
                        {
                            this.WindowOriginY = Pos;
                        }
                        if (this.showScrollHint)
                        {
                            if (this.smoothScroll)
                            {
                                Pos = this.WindowOriginY;
                            }
                            else
                            {
                                Pos = Win32.GetScrollPos(this.owner.Handle, 1);
                            }
                            this.owner.ShowScrollHint(Pos);
                        }
                        return;
                    }
                    case 6:
                    {
                        this.WindowOriginY = 0;
                        return;
                    }
                    case 7:
                    {
                        this.WindowOriginY = this.ScrollHeight();
                        return;
                    }
                    case 8:
                    {
                        if (this.owner.WordWrap)
                        {
                            this.UpdateScroll();
                        }
                        if (this.showScrollHint)
                        {
                            this.owner.HideScrollHint();
                        }
                        return;
                    }
                }
            }
        }

        private void GetScrollCodes(out bool vert, out bool horz, out bool forced)
        {
            vert = false;
            horz = false;
            forced = false;
            switch (this.scrollBars)
            {
                case RichTextBoxScrollBars.Horizontal:
                {
                    horz = true;
                    return;
                }
                case RichTextBoxScrollBars.Vertical:
                {
                    vert = true;
                    return;
                }
                case RichTextBoxScrollBars.Both:
                {
                    vert = true;
                    horz = true;
                    return;
                }
                case RichTextBoxScrollBars.ForcedHorizontal:
                {
                    horz = true;
                    forced = true;
                    return;
                }
                case RichTextBoxScrollBars.ForcedVertical:
                {
                    vert = true;
                    forced = true;
                    return;
                }
                case RichTextBoxScrollBars.ForcedBoth:
                {
                    horz = true;
                    vert = true;
                    forced = true;
                    return;
                }
            }
        }

        protected void OnHorizontalScroll()
        {
            if (this.HorizontalScroll != null)
            {
                this.HorizontalScroll(this.owner, EventArgs.Empty);
            }
        }

        protected void OnVerticalScroll()
        {
            if (this.VerticalScroll != null)
            {
                this.VerticalScroll(this.owner, EventArgs.Empty);
            }
        }

        public virtual void ResetDefaultHorzScrollSize()
        {
            this.DefaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;
        }

        public virtual void ResetScrollBars()
        {
            this.ScrollBars = RichTextBoxScrollBars.Both;
        }

        public virtual void ResetShowScrollHint()
        {
            this.ShowScrollHint = false;
        }

        public virtual void ResetSmoothScroll()
        {
            this.SmoothScroll = true;
        }

        protected internal void SafeSetWindowChar(int Char)
        {
            this.windowOriginX = Char;
        }

        protected internal void SafeSetWindowLine(int Line)
        {
            this.windowOriginY = Line;
        }

        private int ScrollHeight()
        {
            if (this.owner == null)
            {
                return 0;
            }
            if (this.owner.Pages.PageType == PageType.PageLayout)
            {
                return this.owner.Pages.Height;
            }
            return this.owner.DisplayLines.GetCount();
        }

        private int ScrollWidth()
        {
            if (this.owner == null)
            {
                return 0;
            }
            switch (this.owner.Pages.PageType)
            {
                case PageType.PageBreaks:
                {
                    return this.owner.CharsInWidth(this.owner.Pages.Width);
                }
                case PageType.PageLayout:
                {
                    return this.owner.Pages.Width;
                }
            }
            if ((this.scrollBars == RichTextBoxScrollBars.ForcedBoth) || (this.scrollBars == RichTextBoxScrollBars.ForcedHorizontal))
            {
                return this.defaultHorzScrollSize;
            }
            if (this.owner.WordWrap)
            {
                if (this.owner.WrapAtMargin)
                {
                    return this.owner.Margin.Position;
                }
                return this.owner.CharsInWidth();
            }
            int num1 = this.owner.DisplayLines.MaxLineWidth;
            if (this.owner.Painter.FontWidth == 0)
            {
                return this.defaultHorzScrollSize;
            }
            int num2 = num1 / this.owner.Painter.FontWidth;
            if ((num1 % this.owner.Painter.FontWidth) != 0)
            {
                num2++;
            }
            return num2;
        }

        public bool ShouldSerializeDefaultHorzScrollSize()
        {
            return (this.defaultHorzScrollSize != EditConsts.DefaultHorzScrollSize);
        }

        protected internal void UpdateScroll()
        {
            this.WindowOriginY = this.windowOriginY;
            this.UpdateScrollSize();
            this.UpdateScrollPosition();
        }

        protected void UpdateScrollPosition()
        {
            if ((this.owner != null) && this.owner.IsHandleCreated)
            {
                this.scrollUpdateCount++;
                try
                {
                    bool flag1;
                    bool flag2;
                    bool flag3;
                    this.GetScrollCodes(out flag1, out flag2, out flag3);
                    if (flag1)
                    {
                        Win32.SetScrollPos(this.owner.Handle, 1, this.windowOriginY);
                    }
                    if (flag2)
                    {
                        Win32.SetScrollPos(this.owner.Handle, 0, this.windowOriginX);
                    }
                }
                finally
                {
                    this.scrollUpdateCount--;
                }
            }
        }

        protected void UpdateScrollSize()
        {
            if ((this.owner != null) && this.owner.IsHandleCreated)
            {
                bool flag1;
                bool flag2;
                bool flag3;
                int num1 = 0;
                int num2 = 0;
                this.GetScrollCodes(out flag1, out flag2, out flag3);
                if (flag1)
                {
                    num1 = this.ScrollHeight();
                    num2 = (this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.ClientRect.Height : this.owner.LinesInHeight();
                    if (!flag3 && (num1 <= num2))
                    {
                        num2 = -1;
                    }
                    Win32.SetScrollBarInfo(this.owner.Handle, Math.Max((int) (num1 - 1), 0), num2, 1);
                }
                if (flag2)
                {
                    num1 = this.ScrollWidth();
                    num2 = (this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.ClientRect.Width : this.owner.CharsInWidth();
                    if (!flag3 && (num1 <= num2))
                    {
                        num2 = -1;
                    }
                    Win32.SetScrollBarInfo(this.owner.Handle, num1, num2, 0);
                }
            }
        }


        // Properties
        public int DefaultHorzScrollSize
        {
            get
            {
                return this.defaultHorzScrollSize;
            }
            set
            {
                if (this.defaultHorzScrollSize != value)
                {
                    this.defaultHorzScrollSize = value;
                    this.UpdateScroll();
                }
            }
        }

        [DefaultValue(3)]
        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return this.scrollBars;
            }
            set
            {
                if (this.scrollBars != value)
                {
                    this.scrollBars = value;
                    this.UpdateScroll();
                }
            }
        }

        [DefaultValue(false)]
        public bool ShowScrollHint
        {
            get
            {
                return this.showScrollHint;
            }
            set
            {
                this.showScrollHint = value;
            }
        }

        [DefaultValue(true)]
        public bool SmoothScroll
        {
            get
            {
                return this.smoothScroll;
            }
            set
            {
                this.smoothScroll = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int WindowOriginX
        {
            get
            {
                return this.windowOriginX;
            }
            set
            {
                value = Math.Max(value, 0);
                if (this.windowOriginX != value)
                {
                    int num1 = this.windowOriginX;
                    this.windowOriginX = value;
                    this.UpdateScrollPosition();
                    ((SyntaxEdit) this.owner).WindowOriginChanged(this.windowOriginY, num1);
                    this.OnHorizontalScroll();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int WindowOriginY
        {
            get
            {
                return this.windowOriginY;
            }
            set
            {
                value = Math.Max(value, 0);
                if (this.owner != null)
                {
                    if ((NavigateOptions.BeyondEof & this.owner.NavigateOptions) == NavigateOptions.None)
                    {
                        if (this.owner.Pages.PageType == PageType.PageLayout)
                        {
                            int num1 = this.owner.Pages.Height - this.owner.ClientRect.Height;
                            if ((value >= num1) && (num1 >= 0))
                            {
                                value = num1;
                            }
                        }
                        else
                        {
                            int num2 = this.owner.DisplayLines.GetCount() - this.owner.LinesInHeight();
                            if (value >= num2)
                            {
                                value = Math.Max(num2, 0);
                            }
                        }
                    }
                    if (this.windowOriginY != value)
                    {
                        int num3 = this.windowOriginY;
                        this.windowOriginY = value;
                        this.UpdateScrollPosition();
                        ((SyntaxEdit) this.owner).WindowOriginChanged(num3, this.windowOriginX);
                        this.OnVerticalScroll();
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public object XmlInfo
        {
            get
            {
                return new XmlScrollingInfo(this);
            }
            set
            {
                ((XmlScrollingInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private int defaultHorzScrollSize;
        private ISyntaxEdit owner;
        private RichTextBoxScrollBars scrollBars;
        private int scrollUpdateCount;
        private bool showScrollHint;
        private bool smoothScroll;
        private int windowOriginX;
        private int windowOriginY;
    }
}

