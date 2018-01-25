namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public class RulerIndent
    {
        // Methods
        public RulerIndent()
        {
        }

        public RulerIndent(IndentOrientation AOrientation, int AIndent) : this()
        {
            this.orientation = AOrientation;
            this.indent = AIndent;
        }

        public void CancelDragging()
        {
            this.Indent = this.oldIndent;
            this.dragging = false;
        }

        public void DrawIndent(Graphics graph, Rectangle Rect, bool AVertical, Color AIndentBackColor, Color ABackColor)
        {
            Rectangle rectangle1;
            Brush brush1 = new SolidBrush(AIndentBackColor);
            graph.FillRectangle(brush1, Rect);
            brush1.Dispose();
            if (AVertical)
            {
                if (this.orientation == IndentOrientation.Near)
                {
                    rectangle1 = new Rectangle(Rect.Left, Rect.Bottom - EditConsts.DefaultRulerIndentSize, Rect.Width, EditConsts.DefaultRulerIndentSize);
                }
                else
                {
                    rectangle1 = new Rectangle(Rect.Left, Rect.Top, Rect.Width, EditConsts.DefaultRulerIndentSize);
                }
            }
            else if (this.orientation == IndentOrientation.Near)
            {
                rectangle1 = new Rectangle(Rect.Right - EditConsts.DefaultRulerIndentSize, Rect.Top, EditConsts.DefaultRulerIndentSize, Rect.Height);
            }
            else
            {
                rectangle1 = new Rectangle(Rect.Left, Rect.Top, EditConsts.DefaultRulerIndentSize, Rect.Height);
            }
            brush1 = new SolidBrush(ABackColor);
            graph.FillRectangle(brush1, rectangle1);
            brush1.Dispose();
        }


        // Properties
        public bool Dragging
        {
            get
            {
                return this.dragging;
            }
            set
            {
                if (this.dragging != value)
                {
                    this.dragging = value;
                    if (value)
                    {
                        this.oldIndent = this.indent;
                    }
                }
            }
        }

        public int Indent
        {
            get
            {
                return this.indent;
            }
            set
            {
                this.indent = Math.Max(value, 0);
            }
        }

        public IndentOrientation Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;
            }
        }


        // Fields
        private bool dragging;
        private int indent;
        private int oldIndent;
        private IndentOrientation orientation;
    }
}

