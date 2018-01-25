namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;
    using System.Xml.Serialization;

    public class LexStyle : ILexStyle
    {
        // Methods
        public LexStyle()
        {
            this.fontStyle = System.Drawing.FontStyle.Regular;
            this.foreColor = Consts.DefaultControlForeColor;
            this.backColor = Color.Empty;
        }

        public LexStyle(ILexScheme Scheme) : this()
        {
            this.scheme = Scheme;
        }

        public void Assign(ILexStyle Source)
        {
            this.name = Source.Name;
            this.desc = Source.Desc;
            this.foreColor = Source.ForeColor;
            this.backColor = Source.BackColor;
            this.fontStyle = Source.FontStyle;
            this.plainText = Source.PlainText;
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = Color.Empty;
        }

        public virtual void ResetFontStyle()
        {
            this.FontStyle = System.Drawing.FontStyle.Regular;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = Consts.DefaultControlForeColor;
        }

        public virtual void ResetPlainText()
        {
            this.PlainText = false;
        }

        public bool ShouldSerializeFontStyle()
        {
            return (this.fontStyle != System.Drawing.FontStyle.Regular);
        }

        public bool ShouldSerializePlainText()
        {
            return this.plainText;
        }

        public bool ShouldSerializeXmlBackColor()
        {
            return (this.backColor != Color.Empty);
        }

        public bool ShouldSerializeXmlForeColor()
        {
            return (this.foreColor != Color.Empty);
        }

        public void Update()
        {
        }


        // Properties
        [XmlIgnore]
        public Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.Update();
                }
            }
        }

        public string Desc
        {
            get
            {
                return this.desc;
            }
            set
            {
                this.desc = value;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                if (this.fontStyle != value)
                {
                    this.fontStyle = value;
                    this.Update();
                }
            }
        }

        [XmlIgnore]
        public Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                if (this.foreColor != value)
                {
                    this.foreColor = value;
                    this.Update();
                }
            }
        }

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

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public bool PlainText
        {
            get
            {
                return this.plainText;
            }
            set
            {
                this.plainText = value;
            }
        }

        [XmlIgnore]
        public ILexScheme Scheme
        {
            get
            {
                return this.scheme;
            }
            set
            {
                this.scheme = value;
            }
        }

        [XmlElement("BackColor")]
        public string XmlBackColor
        {
            get
            {
                return XmlHelper.SerializeColor(this.BackColor);
            }
            set
            {
                this.BackColor = XmlHelper.DeserializeColor(value);
            }
        }

        [XmlElement("ForeColor")]
        public string XmlForeColor
        {
            get
            {
                return XmlHelper.SerializeColor(this.ForeColor);
            }
            set
            {
                this.ForeColor = XmlHelper.DeserializeColor(value);
            }
        }


        // Fields
        private Color backColor;
        private string desc;
        private System.Drawing.FontStyle fontStyle;
        private Color foreColor;
        private int index;
        private string name;
        private bool plainText;
        private ILexScheme scheme;
    }
}

