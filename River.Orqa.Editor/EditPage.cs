namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Printing;

    public class EditPage : IEditPage
    {
        // Methods
        public EditPage(EditPages Pages)
        {
            this.horzOffset = EditConsts.DefaultPageHorzOffset;
            this.vertOffset = EditConsts.DefaultPageVertOffset;
            this.paintNumber = true;
            this.startLine = 0;
            this.endLine = 0;
            this.pages = Pages;
            this.header = new PageHeader(this);
            this.footer = new PageHeader(this);
            this.origin = new Point(0, 0);
            this.pageSize = this.pages.DefaultPageSize;
            this.margins = new System.Drawing.Printing.Margins(this.pages.DefaultMargins.Left, this.pages.DefaultMargins.Right, this.pages.DefaultMargins.Top, this.pages.DefaultMargins.Bottom);
            this.pageKind = this.pages.DefaultPageKind;
            this.landscape = this.pages.DefaultLandscape;
            this.InitMargins();
            this.caps = Pages.Caps;
        }

        public void Assign(IEditPage Source)
        {
            this.BeginUpdate();
            try
            {
                this.PageKind = Source.PageKind;
                this.PageSize = Source.PageSize;
                this.Landscape = Source.Landscape;
                this.Margins = Source.Margins;
                this.HorzOffset = Source.HorzOffset;
                this.VertOffset = Source.VertOffset;
                this.Header = Source.Header;
                this.Footer = Source.Footer;
                this.PaintNumber = Source.PaintNumber;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public int BeginUpdate()
        {
            if (this.pages == null)
            {
                return 0;
            }
            return this.pages.BeginUpdate();
        }

        public int EndUpdate()
        {
            if (this.pages == null)
            {
                return 0;
            }
            return this.pages.EndUpdate();
        }

        protected internal void FillBoundsRect(ITextPainter Painter, Rectangle OuterRect, Rectangle InnerRect, Color Color, bool Transparent)
        {
            if (((!Transparent && !InnerRect.IsEmpty) && !OuterRect.IsEmpty) && !InnerRect.IsEmpty)
            {
                Color color1 = Painter.BkColor;
                Painter.BkColor = Color;
                try
                {
                    this.FillRect(Painter, OuterRect, new Rectangle(OuterRect.Left, OuterRect.Top, InnerRect.Left - OuterRect.Left, OuterRect.Height));
                    this.FillRect(Painter, OuterRect, new Rectangle(InnerRect.Right, OuterRect.Top, OuterRect.Right - InnerRect.Right, OuterRect.Height));
                    this.FillRect(Painter, OuterRect, new Rectangle(InnerRect.Left, OuterRect.Top, InnerRect.Width, InnerRect.Top - OuterRect.Top));
                    this.FillRect(Painter, OuterRect, new Rectangle(InnerRect.Left, InnerRect.Bottom, InnerRect.Width, OuterRect.Bottom - InnerRect.Bottom));
                }
                finally
                {
                    Painter.BkColor = color1;
                }
            }
        }

        private void FillRect(ITextPainter Painter, Rectangle R1, Rectangle R2)
        {
            Rectangle rectangle1 = Rectangle.Intersect(R1, R2);
            if (!rectangle1.IsEmpty)
            {
                Painter.FillRectangle(rectangle1);
            }
        }

        private void FrameRect(ITextPainter Painter, ref Rectangle Rect, Color Color, Color BackColor, bool DrawBorder)
        {
            Color color1 = Painter.BkColor;
            Color color2 = Painter.PenColor;
            Painter.BkColor = Color;
            Painter.PenColor = Color;
            try
            {
                Painter.DrawRectangle(Rect);
                Painter.DrawLine(Rect.Right, Rect.Top + 1, Rect.Right, Rect.Bottom + 1);
                Painter.PenColor = BackColor;
                Painter.DrawLine(Rect.Right, Rect.Top, Rect.Right, Rect.Top + 1);
                if (DrawBorder || this.IsLastPage)
                {
                    Painter.PenColor = Color;
                    Painter.DrawLine(Rect.Left + 1, Rect.Bottom, Rect.Right, Rect.Bottom);
                    Painter.PenColor = BackColor;
                    Painter.DrawLine(Rect.Left, Rect.Bottom, Rect.Left + 1, Rect.Bottom);
                }
                if (DrawBorder)
                {
                    Painter.PenColor = Color;
                    Painter.DrawLine(Rect.Right + 1, Rect.Top + 1, Rect.Right + 1, Rect.Bottom + 2);
                    Painter.DrawLine(Rect.Left + 1, Rect.Bottom + 1, Rect.Right + 1, Rect.Bottom + 1);
                    Painter.PenColor = BackColor;
                    Painter.DrawLine(Rect.Right + 1, Rect.Top, Rect.Right + 1, Rect.Top + 1);
                    Painter.DrawLine(Rect.Left, Rect.Bottom + 1, Rect.Left, Rect.Bottom + 2);
                }
                Rect.Width++;
                if (DrawBorder || this.IsLastPage)
                {
                    Rect.Height++;
                }
                if (DrawBorder)
                {
                    Rect.Width++;
                    Rect.Height++;
                }
            }
            finally
            {
                Painter.PenColor = color2;
                Painter.BkColor = color1;
            }
        }

        public Rectangle GetBounds(bool IncludeSpace)
        {
            Rectangle rectangle1;
            if (this.landscape)
            {
                rectangle1 = new Rectangle(this.origin.X, this.origin.Y, this.pageSize.Height, this.pageSize.Width);
            }
            else
            {
                rectangle1 = new Rectangle(this.origin.X, this.origin.Y, this.pageSize.Width, this.pageSize.Height);
            }
            if (this.pages != null)
            {
                int num1 = this.pages.Owner.Painter.FontHeight;
                if (this.pages.PageType == PageType.PageLayout)
                {
                    if (!this.pages.DisplayWhiteSpace)
                    {
                        rectangle1.Height -= (this.bottomIndent + this.topIndent);
                        if (num1 != 0)
                        {
                            rectangle1.Height = (rectangle1.Height / num1) * num1;
                        }
                        rectangle1.Height += 8;
                        if (IncludeSpace)
                        {
                            int num2 = (this.index == 0) ? 4 : 0;
                            rectangle1 = new Rectangle(rectangle1.Left - this.horzOffset, rectangle1.Top - num2, rectangle1.Width + this.horzOffset, (rectangle1.Height + num2) + 1);
                        }
                        return rectangle1;
                    }
                    if (IncludeSpace)
                    {
                        rectangle1 = new Rectangle(rectangle1.Left - this.horzOffset, rectangle1.Top - this.vertOffset, rectangle1.Width + this.horzOffset, (rectangle1.Height + this.vertOffset) + 3);
                    }
                    return rectangle1;
                }
                if (num1 != 0)
                {
                    rectangle1.Height = (rectangle1.Height / num1) * num1;
                }
            }
            return rectangle1;
        }

        private void InitMargins()
        {
            this.leftIndent = (this.margins.Left * this.caps.Width) / 100;
            this.rightIndent = (this.margins.Right * this.caps.Width) / 100;
            this.topIndent = (this.margins.Top * this.caps.Height) / 100;
            this.bottomIndent = (this.margins.Bottom * this.caps.Height) / 100;
        }

        public void Invalidate()
        {
            if (this.pages != null)
            {
                this.pages.Invalidate(this);
            }
        }

        public void Paint(ITextPainter Painter)
        {
            if ((this.pages != null) && (this.pages.PageType != PageType.Normal))
            {
                DrawInfo info1;
                SyntaxEdit edit1 = (SyntaxEdit) this.pages.Owner;
                Rectangle rectangle1 = this.ClientRect;
                info1 = new DrawInfo();
                if (this.pages.PageType == PageType.PageBreaks)
                {
                    Rectangle rectangle2 = edit1.ClientRect;
                    rectangle1.X = rectangle2.Left + ((Gutter) edit1.Gutter).GetWidth();
                    rectangle1.Width = rectangle2.Right - rectangle1.Left;
                    if (!this.IsLastPage)
                    {
                        info1.Init();
                        info1.Page = this.Index;
                        if (edit1.OnCustomDraw(Painter, rectangle1, DrawStage.Before, DrawState.PageBorder, info1))
                        {
                            return;
                        }
                        Painter.DrawLine(rectangle1.Left, rectangle1.Bottom - 1, rectangle1.Right, rectangle1.Bottom - 1, this.pages.BorderColor, 1, DashStyle.Dot);
                        edit1.OnCustomDraw(Painter, rectangle1, DrawStage.After, DrawState.PageBorder, info1);
                    }
                }
                else
                {
                    Rectangle rectangle3 = this.BoundsRect;
                    info1.Init();
                    info1.Page = this.Index;
                    if (!edit1.OnCustomDraw(Painter, rectangle3, DrawStage.Before, DrawState.PageBorder, info1))
                    {
                        bool flag1 = this.pages.DisplayWhiteSpace;
                        int num1 = edit1.Painter.FontHeight;
                        if (num1 != 0)
                        {
                            rectangle1.Height = (rectangle1.Height / num1) * num1;
                        }
                        Rectangle rectangle4 = this.PaintRect;
                        rectangle3.Inflate(1, 1);
                        this.FrameRect(Painter, ref rectangle3, this.pages.BorderColor, this.pages.BackColor, flag1);
                        this.FillBoundsRect(Painter, rectangle4, rectangle3, this.pages.BackColor, false);
                        rectangle3 = this.BoundsRect;
                        this.FillBoundsRect(Painter, rectangle3, rectangle1, edit1.BackColor, this.pages.Transparent);
                        if ((this.pages == null) || this.pages.DisplayWhiteSpace)
                        {
                            int num2 = Math.Max((int) (rectangle1.Left - this.header.Offset.X), rectangle3.Left);
                            int num3 = Math.Min((int) ((rectangle1.Left + rectangle1.Width) + this.header.Offset.X), rectangle3.Right);
                            Rectangle rectangle5 = new Rectangle(num2, Math.Max((int) (rectangle1.Top - (this.header.Offset.Y + this.header.Font.Height)), this.BoundsRect.Top), num3 - num2, this.header.Font.Height);
                            this.header.Paint(Painter, rectangle5, this.Index, this.pages.Count, false);
                            num2 = Math.Max((int) (rectangle1.Left - this.footer.Offset.X), rectangle3.Left);
                            num3 = Math.Min((int) ((rectangle1.Left + rectangle1.Width) + this.footer.Offset.X), rectangle3.Right);
                            rectangle5 = new Rectangle(num2, Math.Min((int) (rectangle1.Bottom + this.footer.Offset.Y), (int) (rectangle3.Bottom - this.footer.Font.Height)), num3 - num2, this.footer.Font.Height);
                            this.footer.Paint(Painter, rectangle5, this.Index, this.pages.Count, this.paintNumber);
                        }
                        edit1.OnCustomDraw(Painter, rectangle3, DrawStage.After, DrawState.PageBorder, info1);
                    }
                    edit1.PaintWindow(Painter, this.startLine, new Rectangle(0, 0, rectangle1.Width, rectangle1.Height), rectangle1.Location, 1f, 1f, true);
                }
            }
        }

        private void ScrollRect(ref Rectangle Rect)
        {
            if (this.pages != null)
            {
                ISyntaxEdit edit1 = this.pages.Owner;
                if (this.pages.PageType == PageType.PageLayout)
                {
                    Rect.Offset(-edit1.Scrolling.WindowOriginX, -edit1.Scrolling.WindowOriginY);
                }
                else
                {
                    Rect.Offset(-edit1.Scrolling.WindowOriginX * edit1.Painter.FontWidth, -edit1.Scrolling.WindowOriginY * edit1.Painter.FontHeight);
                }
            }
        }

        public bool ShouldSerializeHorzOffset()
        {
            return (this.horzOffset != EditConsts.DefaultPageHorzOffset);
        }

        public bool ShouldSerializeLandscape()
        {
            if (this.pages != null)
            {
                return (this.landscape != this.pages.DefaultLandscape);
            }
            return true;
        }

        public bool ShouldSerializeMargins()
        {
            if (((this.pages != null) && (this.margins.Left == this.pages.DefaultMargins.Left)) && ((this.margins.Right == this.pages.DefaultMargins.Right) && (this.margins.Top == this.pages.DefaultMargins.Top)))
            {
                return (this.margins.Bottom != this.pages.DefaultMargins.Bottom);
            }
            return true;
        }

        public bool ShouldSerializePageKind()
        {
            if (this.pages != null)
            {
                return (this.pageKind != this.pages.DefaultPageKind);
            }
            return true;
        }

        public bool ShouldSerializePageSize()
        {
            if ((this.pages != null) && (this.pageSize.Width == this.pages.DefaultPageSize.Width))
            {
                return (this.pageSize.Height != this.pages.DefaultPageSize.Height);
            }
            return true;
        }

        public bool ShouldSerializeVertOffset()
        {
            return (this.vertOffset != EditConsts.DefaultPageVertOffset);
        }

        public void Update()
        {
            this.Update(false);
        }

        public void Update(bool Changed)
        {
            if (this.pages != null)
            {
                this.pages.Update(this, Changed);
            }
        }


        // Properties
        protected internal int BottomIndent
        {
            get
            {
                return this.bottomIndent;
            }
            set
            {
                if (this.bottomIndent != value)
                {
                    this.bottomIndent = value;
                    this.margins.Bottom = (value * 100) / this.caps.Height;
                }
            }
        }

        [Browsable(false)]
        public Rectangle BoundsRect
        {
            get
            {
                Rectangle rectangle1 = this.GetBounds(false);
                this.ScrollRect(ref rectangle1);
                return rectangle1;
            }
        }

        [Browsable(false)]
        public Rectangle ClientRect
        {
            get
            {
                Rectangle rectangle1 = this.BoundsRect;
                if ((this.pages != null) && (this.pages.PageType == PageType.PageLayout))
                {
                    bool flag1 = this.pages.DisplayWhiteSpace;
                    rectangle1 = new Rectangle(rectangle1.X + this.leftIndent, rectangle1.Y + (flag1 ? this.topIndent : 4), rectangle1.Width - (this.leftIndent + this.rightIndent), rectangle1.Height - (flag1 ? (this.topIndent + this.bottomIndent) : 8));
                }
                return rectangle1;
            }
        }

        protected internal int DisplayWidth
        {
            get
            {
                return (this.ClientRect.Width - ((this.pages != null) ? ((Gutter) this.pages.Owner.Gutter).GetWidth() : 0));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int EndLine
        {
            get
            {
                return this.endLine;
            }
            set
            {
                this.endLine = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IPageHeader Footer
        {
            get
            {
                return this.footer;
            }
            set
            {
                this.footer.Assign(value);
                this.Invalidate();
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IPageHeader Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header.Assign(value);
                this.Invalidate();
            }
        }

        public int HorzOffset
        {
            get
            {
                return this.horzOffset;
            }
            set
            {
                if (this.HorzOffset != value)
                {
                    this.HorzOffset = value;
                    this.Update();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        [Browsable(false)]
        public bool IsFirstPage
        {
            get
            {
                return (this.index == 0);
            }
        }

        [Browsable(false)]
        public bool IsLastPage
        {
            get
            {
                if (this.pages != null)
                {
                    return (this.index == (this.pages.Count - 1));
                }
                return false;
            }
        }

        public bool Landscape
        {
            get
            {
                return this.landscape;
            }
            set
            {
                if (this.landscape != value)
                {
                    this.landscape = value;
                    this.Update(true);
                }
            }
        }

        protected internal int LeftIndent
        {
            get
            {
                return this.leftIndent;
            }
            set
            {
                if (this.leftIndent != value)
                {
                    this.leftIndent = value;
                    this.margins.Left = (value * 100) / this.caps.Width;
                }
            }
        }

        public System.Drawing.Printing.Margins Margins
        {
            get
            {
                return this.margins;
            }
            set
            {
                if ((this.margins != value) && (value != null))
                {
                    this.margins.Left = value.Left;
                    this.margins.Top = value.Top;
                    this.margins.Right = value.Right;
                    this.margins.Bottom = value.Bottom;
                    this.InitMargins();
                    this.Update(true);
                }
            }
        }

        [Browsable(false)]
        public IEditPage NextPage
        {
            get
            {
                if ((this.pages != null) && (this.index < (this.pages.Count - 1)))
                {
                    return this.pages[this.index + 1];
                }
                return null;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }

        public PaperKind PageKind
        {
            get
            {
                return this.pageKind;
            }
            set
            {
                if (this.pageKind != value)
                {
                    this.pageKind = value;
                    if (this.pages != null)
                    {
                        this.pages.UpdatePageSize(this.PageKind, ref this.pageSize);
                    }
                    this.Update(true);
                }
            }
        }

        [Browsable(false)]
        public Rectangle PageRect
        {
            get
            {
                Rectangle rectangle1 = this.GetBounds(true);
                this.ScrollRect(ref rectangle1);
                return rectangle1;
            }
        }

        protected internal EditPages Pages
        {
            get
            {
                return this.pages;
            }
            set
            {
                this.pages = value;
            }
        }

        public Size PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                if (!this.pageSize.Equals(value))
                {
                    this.pageSize = value;
                    this.Update(true);
                }
            }
        }

        [DefaultValue(true)]
        public bool PaintNumber
        {
            get
            {
                return this.paintNumber;
            }
            set
            {
                if (this.paintNumber != value)
                {
                    this.paintNumber = value;
                    this.Invalidate();
                }
            }
        }

        [Browsable(false)]
        public Rectangle PaintRect
        {
            get
            {
                Rectangle rectangle1 = this.PageRect;
                if (this.pages != null)
                {
                    rectangle1.X = 0;
                    Rectangle rectangle2 = ((SyntaxEdit) this.pages.Owner).GetClientRect(true);
                    rectangle1.Width = rectangle2.Width;
                }
                return rectangle1;
            }
        }

        [Browsable(false)]
        public IEditPage PrevPage
        {
            get
            {
                if ((this.index > 0) && (this.pages != null))
                {
                    return this.pages[this.index - 1];
                }
                return null;
            }
        }

        protected internal int RightIndent
        {
            get
            {
                return this.rightIndent;
            }
            set
            {
                if (this.rightIndent != value)
                {
                    this.rightIndent = value;
                    this.margins.Right = (value * 100) / this.caps.Width;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int StartLine
        {
            get
            {
                return this.startLine;
            }
            set
            {
                this.startLine = value;
            }
        }

        protected internal int TopIndent
        {
            get
            {
                return this.topIndent;
            }
            set
            {
                if (this.topIndent != value)
                {
                    this.topIndent = value;
                    this.margins.Top = (value * 100) / this.caps.Height;
                }
            }
        }

        public int VertOffset
        {
            get
            {
                return this.vertOffset;
            }
            set
            {
                if (this.vertOffset != value)
                {
                    this.vertOffset = value;
                    this.Update();
                }
            }
        }

        [Browsable(false)]
        public Rectangle WhiteSpaceRect
        {
            get
            {
                Rectangle rectangle1 = this.BoundsRect;
                if (this.pages == null)
                {
                    return rectangle1;
                }
                if (this.pages.PageType == PageType.PageLayout)
                {
                    bool flag1 = this.pages.DisplayWhiteSpace;
                    return new Rectangle(rectangle1.Left, rectangle1.Bottom - (flag1 ? this.vertOffset : (this.vertOffset / 2)), rectangle1.Width, this.vertOffset);
                }
                return new Rectangle(0, 0, 0, 0);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlEditPageInfo(this);
            }
            set
            {
                ((XmlEditPageInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private int bottomIndent;
        private Size caps;
        private int endLine;
        private PageHeader footer;
        private PageHeader header;
        private int horzOffset;
        private int index;
        private bool landscape;
        private int leftIndent;
        private System.Drawing.Printing.Margins margins;
        private Point origin;
        private PaperKind pageKind;
        private EditPages pages;
        private Size pageSize;
        private bool paintNumber;
        private int rightIndent;
        private int startLine;
        private int topIndent;
        private int vertOffset;
    }
}

