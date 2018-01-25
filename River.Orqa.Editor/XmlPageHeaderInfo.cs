namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Xml.Serialization;

    public class XmlPageHeaderInfo
    {
        // Methods
        public XmlPageHeaderInfo()
        {
            this.visible = true;
            this.fontName = FontFamily.GenericMonospace.Name;
            this.fontSize = 10f;
            this.fontStyle = System.Drawing.FontStyle.Regular;
        }

        public XmlPageHeaderInfo(PageHeader Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(PageHeader Owner)
        {
            this.owner = Owner;
            this.Font = new System.Drawing.Font(this.fontName, this.fontSize, this.fontStyle);
            this.LeftText = this.leftText;
            this.Offset = this.offset;
            this.ReverseOnEvenPages = this.reverseOnEvenPages;
            this.RightText = this.rightText;
            this.Visible = this.visible;
        }


        // Properties
        [XmlIgnore]
        public System.Drawing.Font Font
        {
            get
            {
                if (this.owner == null)
                {
                    return this.font;
                }
                return this.owner.Font;
            }
            set
            {
                this.font = value;
                if (this.owner != null)
                {
                    this.owner.Font = value;
                }
            }
        }

        public string FontName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontName;
                }
                return this.owner.Font.Name;
            }
            set
            {
                this.fontName = value;
            }
        }

        public float FontSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontSize;
                }
                return this.owner.Font.Size;
            }
            set
            {
                this.fontSize = value;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontStyle;
                }
                return this.owner.Font.Style;
            }
            set
            {
                this.fontStyle = value;
            }
        }

        public string LeftText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.leftText;
                }
                return this.owner.LeftText;
            }
            set
            {
                this.leftText = value;
                if (this.owner != null)
                {
                    this.owner.LeftText = value;
                }
            }
        }

        public Point Offset
        {
            get
            {
                if (this.owner == null)
                {
                    return this.offset;
                }
                return this.owner.Offset;
            }
            set
            {
                this.offset = value;
                if (this.owner != null)
                {
                    this.owner.Offset = value;
                }
            }
        }

        public bool ReverseOnEvenPages
        {
            get
            {
                if (this.owner == null)
                {
                    return this.reverseOnEvenPages;
                }
                return this.owner.ReverseOnEvenPages;
            }
            set
            {
                this.reverseOnEvenPages = value;
                if (this.owner != null)
                {
                    this.owner.ReverseOnEvenPages = value;
                }
            }
        }

        public string RightText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rightText;
                }
                return this.owner.RightText;
            }
            set
            {
                this.rightText = value;
                if (this.owner != null)
                {
                    this.owner.RightText = value;
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


        // Fields
        private System.Drawing.Font font;
        private string fontName;
        private float fontSize;
        private System.Drawing.FontStyle fontStyle;
        private string leftText;
        private Point offset;
        private PageHeader owner;
        private bool reverseOnEvenPages;
        private string rightText;
        private bool visible;
    }
}

