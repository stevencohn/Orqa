namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public class XmlLineSeparatorInfo
    {
        // Methods
        public XmlLineSeparatorInfo()
        {
            this.highlightBackColor = EditConsts.DefaultLineSeparatorColor.Name;
            this.highlightForeColor = string.Empty;
            this.lineColor = EditConsts.DefaultLineSeparatorLineColor.Name;
        }

        public XmlLineSeparatorInfo(LineSeparator Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(LineSeparator Owner)
        {
            this.owner = Owner;
            this.Options = this.options;
            this.HighlightBackColor = this.highlightBackColor;
            this.HighlightForeColor = this.highlightForeColor;
            this.LineColor = this.lineColor;
        }


        // Properties
        public string HighlightBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.highlightBackColor;
                }
                return XmlHelper.SerializeColor(this.owner.HighlightBackColor);
            }
            set
            {
                this.highlightBackColor = value;
                if (this.owner != null)
                {
                    this.owner.HighlightBackColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public string HighlightForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.highlightForeColor;
                }
                return XmlHelper.SerializeColor(this.owner.HighlightForeColor);
            }
            set
            {
                this.highlightForeColor = value;
                if (this.owner != null)
                {
                    this.owner.HighlightForeColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public string LineColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineColor;
                }
                return XmlHelper.SerializeColor(this.owner.LineColor);
            }
            set
            {
                this.lineColor = value;
                if (this.owner != null)
                {
                    this.owner.LineColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public SeparatorOptions Options
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


        // Fields
        private string highlightBackColor;
        private string highlightForeColor;
        private string lineColor;
        private SeparatorOptions options;
        private LineSeparator owner;
    }
}

