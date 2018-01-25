namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Xml.Serialization;

    [TypeConverter("QWhale.Design.LineStyleConverter, QWhale.Design")]
    public class LineStyle : ILineStyle
    {
        // Methods
        public LineStyle()
        {
            this.foreColor = EditConsts.DefaultLineStyleForeColor;
            this.backColor = EditConsts.DefaultLineStyleBackColor;
            this.imageIndex = -1;
            this.options = EditConsts.DefaultLineStyleOptions;
        }

        public LineStyle(SyntaxEdit Owner)
        {
            this.foreColor = EditConsts.DefaultLineStyleForeColor;
            this.backColor = EditConsts.DefaultLineStyleBackColor;
            this.imageIndex = -1;
            this.options = EditConsts.DefaultLineStyleOptions;
            this.owner = Owner;
        }

        public LineStyle(SyntaxEdit Owner, string AName, Color AForeColor, Color ABackColor, int AImageIndex, LineStyleOptions AOptions)
        {
            this.foreColor = EditConsts.DefaultLineStyleForeColor;
            this.backColor = EditConsts.DefaultLineStyleBackColor;
            this.imageIndex = -1;
            this.options = EditConsts.DefaultLineStyleOptions;
            this.owner = Owner;
            this.name = AName;
            this.foreColor = AForeColor;
            this.backColor = ABackColor;
            this.imageIndex = AImageIndex;
            this.options = AOptions;
        }

        public void Assign(ILineStyle Source)
        {
            this.BeginUpdate();
            try
            {
                this.Name = Source.Name;
                this.ForeColor = Source.ForeColor;
                this.BackColor = Source.BackColor;
                this.ImageIndex = Source.ImageIndex;
                this.Options = Source.Options;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected void BeginUpdate()
        {
            this.updateCount++;
        }

        protected void EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
        }

        public Color GetBackColor(Color AColor)
        {
            Color color1 = ((this.options & LineStyleOptions.InvertColors) != LineStyleOptions.None) ? this.foreColor : this.backColor;
            if (color1 == Color.Empty)
            {
                return AColor;
            }
            return color1;
        }

        public Color GetForeColor(Color AColor)
        {
            Color color1 = ((this.options & LineStyleOptions.InvertColors) != LineStyleOptions.None) ? this.backColor : this.foreColor;
            if (color1 == Color.Empty)
            {
                return AColor;
            }
            return color1;
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = EditConsts.DefaultLineStyleBackColor;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = EditConsts.DefaultLineStyleForeColor;
        }

        public virtual void ResetImageIndex()
        {
            this.ImageIndex = -1;
        }

        public virtual void ResetOptions()
        {
            this.Options = LineStyleOptions.InvertColors;
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.backColor != EditConsts.DefaultLineStyleBackColor);
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.foreColor != EditConsts.DefaultLineStyleForeColor);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultLineStyleOptions);
        }

        protected void Update()
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
            }
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), XmlElement("BackColor"), Browsable(false)]
        public string BackColorString
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

        [XmlElement("ForeColor"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ForeColorString
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

        [DefaultValue(-1)]
        public int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                if (this.imageIndex != value)
                {
                    this.imageIndex = value;
                    this.Update();
                }
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

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public LineStyleOptions Options
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
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public SyntaxEdit Owner
        {
            get
            {
                return this.owner;
            }
        }


        // Fields
        private Color backColor;
        private Color foreColor;
        private int imageIndex;
        private string name;
        private LineStyleOptions options;
        private SyntaxEdit owner;
        private int updateCount;
    }
}

