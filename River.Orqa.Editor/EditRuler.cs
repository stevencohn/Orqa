namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class EditRuler : Control, IEditRuler, IControlProps
    {
        // Events
        public event EventHandler Change;

        // Methods
        public EditRuler()
        {
            this.drawX = -1;
            this.vertical = false;
            this.pageStart = 0x55;
            this.pageWidth = 0x1a9;
            this.rulerStart = 0x18;
            this.rulerWidth = 600;
            this.markWidth = 8;
            this.units = EditConsts.DefaultRulerUnits;
            this.options = EditConsts.DefaultRulerOptions;
            this.leftIndent = new RulerIndent(IndentOrientation.Near, this.pageStart - this.rulerStart);
            this.rightIndent = new RulerIndent(IndentOrientation.Far, (this.rulerWidth - this.pageWidth) - (this.pageStart - this.rulerStart));
            base.Width = 200;
            base.Height = EditConsts.DefaultRulerHeight;
            this.Font = new Font(EditConsts.DefaultRulerFontName, EditConsts.DefaultRulerFontSize);
            this.Cursor = Cursors.Default;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | (ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint), true);
            this.linePen = new Pen(Color.Black, 1f);
            this.linePen.DashStyle = DashStyle.DashDotDot;
            this.dpi = Win32.GetScreenDpi();
            this.BackColor = EditConsts.DefaultRulerBackColor;
            this.IndentBackColor = EditConsts.DefaultRulerIndentBackColor;
            this.UpdateRuler();
        }

        public void Assign(IEditRuler Source)
        {
            this.Vertical = Source.Vertical;
            this.PageStart = Source.PageStart;
            this.PageWidth = Source.PageWidth;
            this.RulerStart = Source.RulerStart;
            this.RulerWidth = Source.RulerWidth;
            this.MarkWidth = Source.MarkWidth;
            this.Units = Source.Units;
            this.Options = Source.Options;
        }

        public virtual void CancelDragging()
        {
            if (this.leftIndent.Dragging)
            {
                this.leftIndent.CancelDragging();
                this.pageWidth += ((this.pageStart - this.rulerStart) - this.leftIndent.Indent);
                this.pageStart = this.rulerStart + this.leftIndent.Indent;
                if (this.drawX != -1)
                {
                    this.DrawLine(this.drawX, true);
                }
                this.drawX = -1;
                base.Invalidate();
            }
            if (this.rightIndent.Dragging)
            {
                this.rightIndent.CancelDragging();
                this.pageWidth = (this.rulerWidth - this.rightIndent.Indent) - (this.pageStart - this.rulerStart);
                if (this.drawX != -1)
                {
                    this.DrawLine(this.drawX, true);
                }
                this.drawX = -1;
                base.Invalidate();
            }
        }

        private bool CheckCursor(Point Pt)
        {
            if ((this.options & RulerOptions.AllowDrag) != RulerOptions.None)
            {
                Rectangle rectangle1 = this.GetIndentHitRect(this.leftIndent);
                if (!rectangle1.Contains(Pt))
                {
                    rectangle1 = this.GetIndentHitRect(this.rightIndent);
                    if (!rectangle1.Contains(Pt))
                    {
                        goto Label_005B;
                    }
                }
                Cursor cursor1 = this.vertical ? Cursors.SizeNS : Cursors.SizeWE;
                Win32.SetCursor(cursor1.Handle);
                return true;
            }
        Label_005B:
            return false;
        }

        private void DrawLine(int X, bool Erase)
        {
            if (((this.options & RulerOptions.DisplayDragLine) != RulerOptions.None) && (base.Parent != null))
            {
                this.drawX = X;
                if (base.Parent != null)
                {
                    X += (this.Vertical ? base.Top : base.Left);
                    Point point1 = this.vertical ? new Point(0, X) : new Point(X, 0);
                    Point point2 = this.vertical ? new Point(base.Parent.ClientSize.Width, X) : new Point(X, base.Parent.ClientSize.Height);
                    Size size1 = this.vertical ? new Size(base.Parent.ClientSize.Width, 1) : new Size(1, base.Parent.ClientSize.Height);
                    if (Erase)
                    {
                        base.Parent.Invalidate(new Rectangle(point1, size1));
                    }
                    else
                    {
                        Graphics graphics1 = base.Parent.CreateGraphics();
                        try
                        {
                            graphics1.DrawLine(this.linePen, point1, point2);
                        }
                        finally
                        {
                            graphics1.Dispose();
                        }
                    }
                }
            }
        }

        private void DrawRuler(Graphics graphics, bool Direction, Rectangle Rect)
        {
            if (this.vertical)
            {
                graphics.RotateTransform(-90f);
                graphics.TranslateTransform(0f, (float) (Rect.Top + Rect.Bottom), MatrixOrder.Append);
            }
            float single1 = this.GetRulerStep();
            int num1 = this.vertical ? Rect.Top : Rect.Left;
            int num2 = this.vertical ? Rect.Bottom : Rect.Right;
            float single2 = Direction ? ((float) num1) : ((float) (num2 - (((int) this.Font.Size) / 2)));
            int num3 = 1;
            int num5 = 0;
            string text1 = string.Empty;
            PointF tf1 = PointF.Empty;
            PointF tf2 = PointF.Empty;
            SizeF ef1 = SizeF.Empty;
            while (Direction ? (single2 < ((num2 - single1) - (((int) this.Font.Size) / 2))) : (single2 > (num1 + single1)))
            {
                int num4;
                single2 = Direction ? (single2 + single1) : (single2 - single1);
                if ((num3 % 4) == 0)
                {
                    num4 = this.Font.Height;
                }
                else if ((num3 % 2) == 0)
                {
                    num4 = this.vertical ? (Rect.Width / 3) : (Rect.Height / 3);
                }
                else
                {
                    num4 = 1;
                }
                num5 = this.vertical ? (Rect.Left + ((Rect.Width - num4) / 2)) : (Rect.Top + ((Rect.Height - num4) / 2));
                tf1 = new PointF(single2, (float) num5);
                tf2 = new PointF(single2, (float) (num5 + num4));
                if ((num3 % 4) == 0)
                {
                    int num6 = num3 / 4;
                    text1 = num6.ToString();
                    ef1 = graphics.MeasureString(text1, this.Font);
                    tf1.X -= ((int) (ef1.Width / 2f));
                    graphics.DrawString(text1, this.Font, Brushes.Black, tf1);
                }
                else
                {
                    graphics.DrawLine(Pens.Black, tf1, tf2);
                }
                num3++;
            }
            graphics.ResetTransform();
        }

        ~EditRuler()
        {
            this.linePen.Dispose();
        }

        private int GetDiscreteInterval(int X, int PrevIndent)
        {
            int num1 = X;
            int num2 = 0;
            int num3 = this.vertical ? this.dpi.Y : this.dpi.X;
            if ((this.options & RulerOptions.Discrete) != RulerOptions.None)
            {
                switch (this.units)
                {
                    case RulerUnits.Milimeters:
                    {
                        if (X <= PrevIndent)
                        {
                            num2 = (int) Math.Ceiling((double) (((num1 * 0xfe) * 4) / (num3 * 100)));
                            goto Label_007B;
                        }
                        num2 = (int) Math.Floor((double) (((num1 * 0xfe) * 4) / (num3 * 100)));
                        goto Label_007B;
                    }
                    case RulerUnits.Inches:
                    {
                        if (X <= PrevIndent)
                        {
                            num2 = (int) Math.Ceiling((double) ((num1 * 4) / num3));
                            goto Label_00B1;
                        }
                        num2 = (int) Math.Floor((double) ((num1 * 4) / num3));
                        goto Label_00B1;
                    }
                }
            }
            return num1;
        Label_007B:
            return (int) Math.Round((double) (((num2 * num3) * 100) / 0x3f8));
        Label_00B1:
            return (int) Math.Round((double) ((num2 * num3) / 4));
        }

        private Rectangle GetIndentHitRect(RulerIndent Indent)
        {
            Rectangle rectangle1 = Rectangle.Empty;
            if (this.vertical)
            {
                if (Indent.Orientation == IndentOrientation.Near)
                {
                    return new Rectangle(6, (this.rulerStart + Indent.Indent) - EditConsts.DefaultRulerHitWidth, 0x10, EditConsts.DefaultRulerHitWidth * 2);
                }
                return new Rectangle(6, (this.pageStart + this.pageWidth) - EditConsts.DefaultRulerHitWidth, 0x10, EditConsts.DefaultRulerHitWidth * 2);
            }
            if (Indent.Orientation == IndentOrientation.Near)
            {
                return new Rectangle((this.rulerStart + Indent.Indent) - EditConsts.DefaultRulerHitWidth, 6, EditConsts.DefaultRulerHitWidth * 2, 0x10);
            }
            return new Rectangle((this.pageStart + this.pageWidth) - EditConsts.DefaultRulerHitWidth, 6, EditConsts.DefaultRulerHitWidth * 2, 0x10);
        }

        private int GetIndentPos(RulerIndent Ind)
        {
            if (Ind.Orientation == IndentOrientation.Near)
            {
                return (this.rulerStart + Ind.Indent);
            }
            return ((this.rulerStart + this.rulerWidth) - Ind.Indent);
        }

        private Rectangle GetIndentRect(RulerIndent Indent)
        {
            Rectangle rectangle1 = Rectangle.Empty;
            if (this.vertical)
            {
                if (Indent.Orientation == IndentOrientation.Near)
                {
                    return new Rectangle(6, this.rulerStart, 0x10, Indent.Indent);
                }
                return new Rectangle(6, this.pageWidth + this.pageStart, 0x10, Indent.Indent);
            }
            if (Indent.Orientation == IndentOrientation.Near)
            {
                return new Rectangle(this.rulerStart, 6, Indent.Indent, 0x10);
            }
            return new Rectangle(this.pageWidth + this.pageStart, 6, Indent.Indent, 0x10);
        }

        private Rectangle GetLeftRulerRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.rulerStart, 0x10, this.pageStart - this.rulerStart);
            }
            return new Rectangle(this.rulerStart, 6, this.pageStart - this.rulerStart, 0x10);
        }

        private Rectangle GetPageRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.pageStart, 0x10, this.pageWidth);
            }
            return new Rectangle(this.pageStart, 6, this.pageWidth, 0x10);
        }

        private Rectangle GetRightRulerRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.pageStart, 0x10, this.rulerWidth - (this.pageStart - this.rulerStart));
            }
            return new Rectangle(this.pageStart, 6, this.rulerWidth - (this.pageStart - this.rulerStart), 0x10);
        }

        private RulerHitTest GetRulerHitTest(int X, int Y)
        {
            Point point1 = new Point(X, Y);
            Rectangle rectangle1 = this.GetIndentRect(this.leftIndent);
            if (rectangle1.Contains(point1))
            {
                return RulerHitTest.LeftIndent;
            }
            rectangle1 = this.GetIndentRect(this.rightIndent);
            if (rectangle1.Contains(point1))
            {
                return RulerHitTest.RightIndent;
            }
            rectangle1 = this.GetPageRect();
            if (rectangle1.Contains(point1))
            {
                return RulerHitTest.Page;
            }
            return RulerHitTest.None;
        }

        private Rectangle GetRulerRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.rulerStart, 0x10, this.rulerWidth);
            }
            return new Rectangle(this.rulerStart, 6, this.rulerWidth, 0x10);
        }

        private float GetRulerStep()
        {
            int num1 = this.vertical ? this.dpi.Y : this.dpi.X;
            switch (this.units)
            {
                case RulerUnits.Milimeters:
                {
                    return ((num1 * 100f) / 1016f);
                }
                case RulerUnits.Inches:
                {
                    return (((float) num1) / 4f);
                }
            }
            return (float) this.markWidth;
        }

        private void IndentDown(RulerIndent Indent)
        {
            Indent.Dragging = true;
        }

        private void IndentMove(RulerIndent Indent, Point Pt)
        {
            int num1 = this.vertical ? Pt.Y : Pt.X;
            if (Indent.Orientation == IndentOrientation.Near)
            {
                num1 = Math.Min(num1, (int) ((this.pageStart + this.pageWidth) - EditConsts.DefaultRulerMinWidth));
                num1 = Math.Max((int) (num1 - this.rulerStart), EditConsts.DefaultRulerIndentSize);
                Indent.Indent = this.GetDiscreteInterval(num1, Indent.Indent);
                this.pageWidth += ((this.pageStart - this.rulerStart) - Indent.Indent);
                this.pageStart = this.rulerStart + Indent.Indent;
            }
            else
            {
                num1 = Math.Max(num1, (int) (this.pageStart + EditConsts.DefaultRulerMinWidth));
                num1 = Math.Max((int) ((this.rulerStart + this.rulerWidth) - num1), EditConsts.DefaultRulerIndentSize);
                Indent.Indent = this.GetDiscreteInterval(num1, Indent.Indent);
                this.pageWidth = (this.rulerWidth - Indent.Indent) - (this.pageStart - this.rulerStart);
            }
            num1 = this.GetIndentPos(Indent);
            if (this.drawX != num1)
            {
                if (this.drawX != -1)
                {
                    this.DrawLine(this.drawX, true);
                }
                this.DrawLine(num1, false);
            }
            base.Invalidate();
        }

        private void IndentUp(RulerIndent Indent)
        {
            Indent.Dragging = false;
            if (this.drawX != -1)
            {
                this.DrawLine(this.drawX, true);
            }
        }

        protected virtual void OnChange(object Sender)
        {
            if (this.Change != null)
            {
                this.Change(this, new RulerEventArgs(Sender));
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if ((base.Parent != null) && base.Parent.CanFocus)
            {
                base.Parent.Focus();
            }
            this.drawX = -1;
            if ((this.options & RulerOptions.AllowDrag) != RulerOptions.None)
            {
                Point point1 = new Point(e.X, e.Y);
                Rectangle rectangle1 = this.GetIndentHitRect(this.leftIndent);
                if (rectangle1.Contains(point1))
                {
                    this.IndentDown(this.leftIndent);
                }
                else
                {
                    rectangle1 = this.GetIndentHitRect(this.rightIndent);
                    if (rectangle1.Contains(point1))
                    {
                        this.IndentDown(this.rightIndent);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point1 = new Point(e.X, e.Y);
            if (this.leftIndent.Dragging)
            {
                this.IndentMove(this.leftIndent, point1);
            }
            if (this.rightIndent.Dragging)
            {
                this.IndentMove(this.rightIndent, point1);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.leftIndent.Dragging)
            {
                this.OnChange(this.leftIndent);
            }
            this.IndentUp(this.leftIndent);
            if (this.rightIndent.Dragging)
            {
                this.OnChange(this.rightIndent);
            }
            this.IndentUp(this.rightIndent);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics graphics1 = pe.Graphics;
            graphics1.FillRectangle(SystemBrushes.Window, this.GetPageRect());
            this.leftIndent.DrawIndent(graphics1, this.GetIndentRect(this.leftIndent), this.vertical, this.IndentBackColor, this.BackColor);
            this.rightIndent.DrawIndent(graphics1, this.GetIndentRect(this.rightIndent), this.vertical, this.IndentBackColor, this.BackColor);
            this.DrawRuler(graphics1, this.vertical, this.GetLeftRulerRect());
            this.DrawRuler(graphics1, !this.vertical, this.GetRightRulerRect());
        }

		Rectangle IControlProps.Bounds
		{
			get { return base.Bounds; }
			set { base.Bounds = value; }
		}

		Rectangle IControlProps.ClientRectangle
		{
			get { return base.ClientRectangle; }
		}

		int IControlProps.Height
		{
			get { return base.Height; }
			set { base.Height = value; }
		}

		bool IControlProps.IsHandleCreated
		{
			get { return base.IsHandleCreated; }
		}

		int IControlProps.Left
		{
			get { return base.Left; }
			set { base.Left = value; }
		}

		Point IControlProps.Location
		{
			get { return base.Location; }
			set { base.Location = value; }
		}

		Control IControlProps.Parent
		{
			get { return base.Parent; }
			set { base.Parent = value; }
		}

		int IControlProps.Top
		{
			get { return base.Top; }
			set { base.Top = value; }
		}

		bool IControlProps.Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		int IControlProps.Width
		{
			get { return base.Width; }
			set { base.Width = value; }
		}


        void IControlProps.Invalidate()
        {
            base.Invalidate();
        }

        void IControlProps.Invalidate(Rectangle rectangle1)
        {
            base.Invalidate(rectangle1);
        }

        Point IControlProps.PointToClient(Point point1)
        {
            return base.PointToClient(point1);
        }

        Point IControlProps.PointToScreen(Point point1)
        {
            return base.PointToScreen(point1);
        }



        void IControlProps.Update()
        {
            base.Update();
        }

        public void ResetIndentBackColor()
        {
            this.IndentBackColor = EditConsts.DefaultInfoBackColor;
        }

        public void ResetOptions()
        {
            this.Options = EditConsts.DefaultRulerOptions;
        }

        public void ResetUnits()
        {
            this.Units = EditConsts.DefaultRulerUnits;
        }

        private void UpdateIndents()
        {
            this.leftIndent.Indent = this.pageStart - this.rulerStart;
            this.rightIndent.Indent = (this.rulerWidth - this.pageWidth) - (this.pageStart - this.rulerStart);
        }

        private void UpdateRuler()
        {
            if (this.vertical)
            {
                base.Width = EditConsts.DefaultRulerHeight;
            }
            else
            {
                base.Height = EditConsts.DefaultRulerHeight;
            }
            this.UpdateIndents();
            base.Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 0x20) && this.CheckCursor(base.PointToClient(Control.MousePosition)))
            {
                m.Result = (IntPtr) 1;
            }
            else
            {
                base.WndProc(ref m);
            }
        }


        // Properties
        public Color IndentBackColor
        {
            get
            {
                return this.indentBackColor;
            }
            set
            {
                if (this.indentBackColor != value)
                {
                    this.indentBackColor = value;
                    base.Invalidate();
                }
            }
        }

        public bool IsDragging
        {
            get
            {
                if (!this.leftIndent.Dragging)
                {
                    return this.rightIndent.Dragging;
                }
                return true;
            }
        }

        public int MarkWidth
        {
            get
            {
                return this.markWidth;
            }
            set
            {
                if (this.markWidth != value)
                {
                    this.markWidth = value;
                    base.Invalidate();
                }
            }
        }

        public RulerOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    base.Invalidate();
                }
            }
        }

        public int PageStart
        {
            get
            {
                return this.pageStart;
            }
            set
            {
                if (this.pageStart != value)
                {
                    this.pageStart = value;
                    this.UpdateIndents();
                    base.Invalidate();
                }
            }
        }

        public int PageWidth
        {
            get
            {
                return this.pageWidth;
            }
            set
            {
                if (this.pageWidth != value)
                {
                    this.pageWidth = value;
                    this.UpdateIndents();
                    base.Invalidate();
                }
            }
        }

        public int RulerStart
        {
            get
            {
                return this.rulerStart;
            }
            set
            {
                if (this.rulerStart != value)
                {
                    this.rulerStart = value;
                    this.UpdateIndents();
                    base.Invalidate();
                }
            }
        }

        public int RulerWidth
        {
            get
            {
                return this.rulerWidth;
            }
            set
            {
                if (this.rulerWidth != value)
                {
                    this.rulerWidth = value;
                    this.UpdateIndents();
                    base.Invalidate();
                }
            }
        }

        public RulerUnits Units
        {
            get
            {
                return this.units;
            }
            set
            {
                if (this.units != value)
                {
                    this.units = value;
                    base.Invalidate();
                }
            }
        }

        public bool Vertical
        {
            get
            {
                return this.vertical;
            }
            set
            {
                if (this.vertical != value)
                {
                    this.vertical = value;
                    this.UpdateRuler();
                }
            }
        }


        // Fields
        private const int cPointsPerInt = 4;
        private const int cRulerHeight = 0x10;
        private const int cRulerTop = 6;
        private const int cTenthMMPerInch = 0xfe;
        private Point dpi;
        private int drawX;
        private Color indentBackColor;
        private RulerIndent leftIndent;
        private Pen linePen;
        private int markWidth;
        private RulerOptions options;
        private int pageStart;
        private int pageWidth;
        private RulerIndent rightIndent;
        private int rulerStart;
        private int rulerWidth;
        private RulerUnits units;
        private bool vertical;

        // Nested Types
        private enum RulerHitTest
        {
            // Fields
            LeftIndent = 1,
            None = 0,
            Page = 3,
            RightIndent = 2
        }
    }
}

