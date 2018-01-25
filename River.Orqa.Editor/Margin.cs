namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class Margin : IMargin
    {
        // Methods
        public Margin()
        {
            this.position = EditConsts.DefaultMarginPosition;
            this.visible = true;
            this.drawX = -1;
            this.drawY = -1;
            this.pen = new System.Drawing.Pen(EditConsts.DefaultMarginForeColor, 1f);
        }

        public Margin(ISyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        public void Assign(IMargin Source)
        {
            this.BeginUpdate();
            try
            {
                this.Pen.Width = Source.Pen.Width;
                this.Pen.Color = Source.Pen.Color;
                this.Position = Source.Position;
                this.Visible = Source.Visible;
                this.AllowDrag = Source.AllowDrag;
                this.ShowHints = Source.ShowHints;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected internal void BeginUpdate()
        {
            this.updateCount++;
        }

        public void CancelDragging()
        {
            this.IsDragging = false;
        }

        public bool Contains(int X, int Y)
        {
            if (this.owner != null)
            {
                if (this.owner.Pages.PageType == PageType.PageLayout)
                {
                    IEditPage page1 = this.owner.Pages.GetPageAtPoint(X, Y);
                    if ((page1 == null) || !page1.ClientRect.Contains(X, Y))
                    {
                        return false;
                    }
                }
                Point point1 = this.owner.ScreenToDisplay(X, Y);
                Point point2 = ((SyntaxEdit) this.owner).DisplayToScreen(this.Position, point1.Y, true);
                int num1 = point2.X;
                if (X > (num1 - EditConsts.DefaultRulerHitWidth))
                {
                    return (X <= (num1 + EditConsts.DefaultRulerHitWidth));
                }
            }
            return false;
        }

        public void DragTo(int X, int Y)
        {
            if (this.drawX >= 0)
            {
                this.DrawLine(this.drawX, this.drawY, true);
            }
            this.DrawLine(X, Y, false);
        }

        protected internal void DrawLine(int X, int Y, bool Erase)
        {
            this.drawX = X;
            this.drawY = Y;
            if (this.owner != null)
            {
                Rectangle rectangle1;
                SyntaxEdit edit1 = (SyntaxEdit) this.owner;
                if (edit1.Pages.PageType == PageType.PageLayout)
                {
                    IEditPage page1 = edit1.Pages.GetPageAtPoint(X, Y);
                    if (page1 == null)
                    {
                        return;
                    }
                    rectangle1 = page1.ClientRect;
                }
                else
                {
                    rectangle1 = edit1.ClientRect;
                }
                if (Erase)
                {
                    edit1.Invalidate(new Rectangle(X, rectangle1.Top, 1, rectangle1.Height));
                }
                else
                {
                    Graphics graphics1 = edit1.CreateGraphics();
                    try
                    {
                        graphics1.DrawLine(this.pen, X, rectangle1.Top, X, (int) (rectangle1.Bottom - 1));
                    }
                    finally
                    {
                        graphics1.Dispose();
                    }
                }
            }
        }

        protected internal void EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update(true);
            }
        }

        ~Margin()
        {
            this.pen.Dispose();
        }

        public void Paint(ITextPainter Painter, Rectangle Rect)
        {
            if ((this.owner != null) && !this.isDragging)
            {
                Painter.DrawLine(Rect.Left, 0, Rect.Left, Rect.Bottom, this.pen.Color, (int) this.pen.Width, this.pen.DashStyle);
            }
        }

        public void ResetAllowDrag()
        {
            this.AllowDrag = false;
        }

        public virtual void ResetPenColor()
        {
            this.PenColor = EditConsts.DefaultMarginForeColor;
        }

        public virtual void ResetPosition()
        {
            this.Position = EditConsts.DefaultMarginPosition;
        }

        public void ResetShowHints()
        {
            this.ShowHints = false;
        }

        public virtual void ResetVisible()
        {
            this.Visible = true;
        }

        public bool ShouldSerializePenColor()
        {
            return (this.PenColor != EditConsts.DefaultMarginForeColor);
        }

        public bool ShouldSerializePosition()
        {
            return (this.position != EditConsts.DefaultMarginPosition);
        }

        protected internal void Update()
        {
            this.Update(false);
        }

        protected internal void Update(bool NeedUpdate)
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
                if ((NeedUpdate && this.owner.WordWrap) && this.owner.WrapAtMargin)
                {
                    this.owner.UpdateWordWrap();
                }
            }
        }


        // Properties
        public bool AllowDrag
        {
            get
            {
                return this.allowDrag;
            }
            set
            {
                this.allowDrag = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool IsDragging
        {
            get
            {
                return this.isDragging;
            }
            set
            {
                if (this.isDragging != value)
                {
                    this.isDragging = value;
                    if (this.drawX >= 0)
                    {
                        this.DrawLine(this.drawX, this.drawY, true);
                    }
                    this.owner.Invalidate();
                    this.drawX = -1;
                    this.drawY = -1;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public System.Drawing.Pen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                if (this.pen != value)
                {
                    this.pen = value;
                    this.Update();
                }
            }
        }

        public Color PenColor
        {
            get
            {
                return this.pen.Color;
            }
            set
            {
                this.pen.Color = value;
            }
        }

        public int Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.Update(true);
                }
            }
        }

        public bool ShowHints
        {
            get
            {
                return this.showHints;
            }
            set
            {
                this.showHints = value;
            }
        }

        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public object XmlInfo
        {
            get
            {
                return new XmlMarginInfo(this);
            }
            set
            {
                ((XmlMarginInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private bool allowDrag;
        private int drawX;
        private int drawY;
        private bool isDragging;
        private ISyntaxEdit owner;
        private System.Drawing.Pen pen;
        private int position;
        private bool showHints;
        private int updateCount;
        private bool visible;
    }
}

