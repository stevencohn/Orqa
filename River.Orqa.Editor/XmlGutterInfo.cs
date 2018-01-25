namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;

    public class XmlGutterInfo
    {
        // Methods
        public XmlGutterInfo()
        {
            this.width = EditConsts.DefaultGutterWidth;
            this.backColor = EditConsts.DefaultGutterBackColor.Name;
            this.penColor = EditConsts.DefaultGutterForeColor.Name;
            this.penWidth = 1f;
            this.visible = true;
            this.lineNumbersStart = EditConsts.DefaultLineNumbersStart;
            this.lineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;
            this.lineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;
            this.lineNumbersForeColor = Consts.DefaultControlForeColor.Name;
            this.lineNumbersBackColor = Consts.DefaultControlBackColor.Name;
            this.lineNumbersAlignment = StringFormat.GenericTypographic.Alignment;
            this.options = EditConsts.DefaultGutterOptions;
            this.bookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;
            this.wrapImageIndex = EditConsts.DefaultWrapImageIndex;
        }

        public XmlGutterInfo(Gutter Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Gutter Owner)
        {
            this.owner = Owner;
            this.Width = this.width;
            this.BackColor = this.backColor;
            this.PenColor = this.penColor;
            this.PenWidth = this.penWidth;
            this.Visible = this.visible;
            this.LineNumbersStart = this.lineNumbersStart;
            this.LineNumbersLeftIndent = this.lineNumbersLeftIndent;
            this.LineNumbersRightIndent = this.lineNumbersRightIndent;
            this.LineNumbersForeColor = this.lineNumbersForeColor;
            this.LineNumbersBackColor = this.lineNumbersBackColor;
            this.LineNumbersAlignment = this.lineNumbersAlignment;
            this.Options = this.options;
            this.BookMarkImageIndex = this.bookMarkImageIndex;
            this.WrapImageIndex = this.wrapImageIndex;
            this.DrawLineBookmarks = this.drawLineBookmarks;
        }


        // Properties
        public string BackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.backColor;
                }
                if (!(this.owner.Brush is SolidBrush))
                {
                    return string.Empty;
                }
                return XmlHelper.SerializeColor(((SolidBrush) this.owner.Brush).Color);
            }
            set
            {
                this.backColor = value;
                if ((this.owner != null) && (this.owner.Brush is SolidBrush))
                {
                    ((SolidBrush) this.owner.Brush).Color = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public int BookMarkImageIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.bookMarkImageIndex;
                }
                return this.owner.BookMarkImageIndex;
            }
            set
            {
                this.bookMarkImageIndex = value;
                if (this.owner != null)
                {
                    this.owner.BookMarkImageIndex = value;
                }
            }
        }

        public bool DrawLineBookmarks
        {
            get
            {
                if (this.owner == null)
                {
                    return this.drawLineBookmarks;
                }
                return this.owner.DrawLineBookmarks;
            }
            set
            {
                this.drawLineBookmarks = value;
                if (this.owner != null)
                {
                    this.owner.DrawLineBookmarks = value;
                }
            }
        }

        public StringAlignment LineNumbersAlignment
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersAlignment;
                }
                return this.owner.LineNumbersAlignment;
            }
            set
            {
                this.lineNumbersAlignment = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersAlignment = value;
                }
            }
        }

        public string LineNumbersBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersBackColor;
                }
                return XmlHelper.SerializeColor(this.owner.LineNumbersBackColor);
            }
            set
            {
                this.lineNumbersBackColor = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersBackColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public string LineNumbersForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersForeColor;
                }
                return XmlHelper.SerializeColor(this.owner.LineNumbersForeColor);
            }
            set
            {
                this.lineNumbersForeColor = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersForeColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public int LineNumbersLeftIndent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersLeftIndent;
                }
                return this.owner.LineNumbersLeftIndent;
            }
            set
            {
                this.lineNumbersLeftIndent = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersLeftIndent = value;
                }
            }
        }

        public int LineNumbersRightIndent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersRightIndent;
                }
                return this.owner.LineNumbersRightIndent;
            }
            set
            {
                this.lineNumbersRightIndent = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersRightIndent = value;
                }
            }
        }

        public int LineNumbersStart
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersStart;
                }
                return this.owner.LineNumbersStart;
            }
            set
            {
                this.lineNumbersStart = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersStart = value;
                }
            }
        }

        public GutterOptions Options
        {
            get
            {
                if (this.owner == null)
                {
                    return this.options;
                }
                return this.owner.Options;
            }
            set
            {
                this.options = value;
                if (this.owner != null)
                {
                    this.owner.Options = value;
                }
            }
        }

        public string PenColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penColor;
                }
                return XmlHelper.SerializeColor(this.owner.Pen.Color);
            }
            set
            {
                this.penColor = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Color = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public float PenWidth
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penWidth;
                }
                return this.owner.Pen.Width;
            }
            set
            {
                this.penWidth = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Width = value;
                }
            }
        }

        public bool Visible
        {
            get
            {
                if (this.owner == null)
                {
                    return this.visible;
                }
                return this.owner.Visible;
            }
            set
            {
                this.visible = value;
                if (this.owner != null)
                {
                    this.owner.Visible = value;
                }
            }
        }

        public int Width
        {
            get
            {
                if (this.owner == null)
                {
                    return this.width;
                }
                return this.owner.Width;
            }
            set
            {
                this.width = value;
                if (this.owner != null)
                {
                    this.owner.Width = value;
                }
            }
        }

        public int WrapImageIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wrapImageIndex;
                }
                return this.owner.WrapImageIndex;
            }
            set
            {
                this.wrapImageIndex = value;
                if (this.owner != null)
                {
                    this.owner.WrapImageIndex = value;
                }
            }
        }


        // Fields
        private string backColor;
        private int bookMarkImageIndex;
        private bool drawLineBookmarks;
        private StringAlignment lineNumbersAlignment;
        private string lineNumbersBackColor;
        private string lineNumbersForeColor;
        private int lineNumbersLeftIndent;
        private int lineNumbersRightIndent;
        private int lineNumbersStart;
        private GutterOptions options;
        private Gutter owner;
        private string penColor;
        private float penWidth;
        private bool visible;
        private int width;
        private int wrapImageIndex;
    }
}

