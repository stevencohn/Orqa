namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;

    public class XmlHyperTextExInfo
    {
        // Methods
        public XmlHyperTextExInfo()
        {
            this.urlStyle = EditConsts.DefaultUrlFontStyle;
            this.urlColor = EditConsts.DefaultUrlForeColor.Name;
        }

        public XmlHyperTextExInfo(HyperTextEx Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(HyperTextEx Owner)
        {
            this.owner = Owner;
            this.UrlStyle = this.urlStyle;
            this.UrlColor = this.urlColor;
        }


        // Properties
        public string UrlColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.urlColor;
                }
                return XmlHelper.SerializeColor(this.owner.UrlColor);
            }
            set
            {
                this.urlColor = value;
                if (this.owner != null)
                {
                    this.owner.UrlColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public FontStyle UrlStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.urlStyle;
                }
                return this.owner.UrlStyle;
            }
            set
            {
                this.urlStyle = value;
                if (this.owner != null)
                {
                    this.owner.UrlStyle = value;
                }
            }
        }


        // Fields
        private HyperTextEx owner;
        private string urlColor;
        private FontStyle urlStyle;
    }
}

