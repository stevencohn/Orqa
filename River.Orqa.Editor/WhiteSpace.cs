namespace River.Orqa.Editor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class WhiteSpace : IWhiteSpace
    {
        // Methods
        public WhiteSpace()
        {
            this.tabSymbol = EditConsts.TabSymbol;
            this.spaceSymbol = EditConsts.SpaceSymbol;
            this.eolSymbol = EditConsts.EolSymbol;
            this.eofSymbol = EditConsts.EofSymbol;
            this.symbolColor = EditConsts.DefaultWhiteSpaceForeColor;
            this.spaceString = new string(this.spaceSymbol, 1);
            this.tabString = new string(this.tabSymbol, 1);
            this.eolString = new string(this.eolSymbol, 1);
            this.eofString = new string(this.eofSymbol, 1);
        }

        public WhiteSpace(ISyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        public void Assign(IWhiteSpace Source)
        {
            this.BeginUpdate();
            try
            {
                this.SpaceSymbol = Source.SpaceSymbol;
                this.TabSymbol = Source.TabSymbol;
                this.EofSymbol = Source.EofSymbol;
                this.EolSymbol = Source.EolSymbol;
                this.Visible = Source.Visible;
                this.SymbolColor = Source.SymbolColor;
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

        public virtual void ResetEofSymbol()
        {
            this.EofSymbol = EditConsts.EofSymbol;
        }

        public virtual void ResetEolSymbol()
        {
            this.EolSymbol = EditConsts.EolSymbol;
        }

        public virtual void ResetSpaceSymbol()
        {
            this.SpaceSymbol = EditConsts.SpaceSymbol;
        }

        public virtual void ResetSymbolColor()
        {
            this.SymbolColor = EditConsts.DefaultWhiteSpaceForeColor;
        }

        public virtual void ResetTabSymbol()
        {
            this.TabSymbol = EditConsts.TabSymbol;
        }

        public virtual void ResetVisible()
        {
            this.Visible = false;
        }

        public bool ShouldSerializeEofSymbol()
        {
            return (this.eofSymbol != EditConsts.EofSymbol);
        }

        public bool ShouldSerializeEolSymbol()
        {
            return (this.eolSymbol != EditConsts.EolSymbol);
        }

        public bool ShouldSerializeSpaceSymbol()
        {
            return (this.spaceSymbol != EditConsts.SpaceSymbol);
        }

        public bool ShouldSerializeSymbolColor()
        {
            return (this.symbolColor != EditConsts.DefaultWhiteSpaceForeColor);
        }

        public bool ShouldSerializeTabSymbol()
        {
            return (this.tabSymbol != EditConsts.TabSymbol);
        }

        protected void Update()
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }


        // Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string EofString
        {
            get
            {
                return this.eofString;
            }
        }

        public char EofSymbol
        {
            get
            {
                return this.eofSymbol;
            }
            set
            {
                if (this.eofSymbol != value)
                {
                    this.eofSymbol = value;
                    this.eofString = new string(this.eofSymbol, 1);
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string EolString
        {
            get
            {
                return this.eolString;
            }
        }

        public char EolSymbol
        {
            get
            {
                return this.eolSymbol;
            }
            set
            {
                if (this.eolSymbol != value)
                {
                    this.eolSymbol = value;
                    this.eolString = new string(this.eolSymbol, 1);
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string SpaceString
        {
            get
            {
                return this.spaceString;
            }
        }

        public char SpaceSymbol
        {
            get
            {
                return this.spaceSymbol;
            }
            set
            {
                if (this.spaceSymbol != value)
                {
                    this.spaceSymbol = value;
                    this.spaceString = new string(this.spaceSymbol, 1);
                    this.Update();
                }
            }
        }

        public Color SymbolColor
        {
            get
            {
                return this.symbolColor;
            }
            set
            {
                if (this.symbolColor != value)
                {
                    this.symbolColor = value;
                    this.Update();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TabString
        {
            get
            {
                return this.tabString;
            }
        }

        public char TabSymbol
        {
            get
            {
                return this.tabSymbol;
            }
            set
            {
                if (this.tabSymbol != value)
                {
                    this.tabSymbol = value;
                    this.tabString = new string(this.tabSymbol, 1);
                    this.Update();
                }
            }
        }

        [DefaultValue(false)]
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlWhiteSpaceInfo(this);
            }
            set
            {
                ((XmlWhiteSpaceInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private string eofString;
        private char eofSymbol;
        private string eolString;
        private char eolSymbol;
        private ISyntaxEdit owner;
        private string spaceString;
        private char spaceSymbol;
        private Color symbolColor;
        private string tabString;
        private char tabSymbol;
        private int updateCount;
        private bool visible;
    }
}

