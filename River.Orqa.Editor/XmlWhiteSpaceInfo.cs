namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public class XmlWhiteSpaceInfo
    {
        // Methods
        public XmlWhiteSpaceInfo()
        {
            this.tabSymbol = EditConsts.TabSymbol;
            this.spaceSymbol = EditConsts.SpaceSymbol;
            this.eolSymbol = EditConsts.EolSymbol;
            this.eofSymbol = EditConsts.EofSymbol;
            this.symbolColor = EditConsts.DefaultWhiteSpaceForeColor.Name;
        }

        public XmlWhiteSpaceInfo(WhiteSpace Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(WhiteSpace Owner)
        {
            this.owner = Owner;
            this.Visible = this.visible;
            this.TabSymbol = this.tabSymbol;
            this.SpaceSymbol = this.spaceSymbol;
            this.EolSymbol = this.eolSymbol;
            this.EofSymbol = this.eofSymbol;
            this.SymbolColor = this.symbolColor;
        }


        // Properties
        public char EofSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.eofSymbol;
                }
                return this.owner.EofSymbol;
            }
            set
            {
                this.eofSymbol = value;
                if (this.owner != null)
                {
                    this.owner.EofSymbol = value;
                }
            }
        }

        public char EolSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.eolSymbol;
                }
                return this.owner.EolSymbol;
            }
            set
            {
                this.eolSymbol = value;
                if (this.owner != null)
                {
                    this.owner.EolSymbol = value;
                }
            }
        }

        public char SpaceSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.spaceSymbol;
                }
                return this.owner.SpaceSymbol;
            }
            set
            {
                this.spaceSymbol = value;
                if (this.owner != null)
                {
                    this.owner.SpaceSymbol = value;
                }
            }
        }

        public string SymbolColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.symbolColor;
                }
                return XmlHelper.SerializeColor(this.owner.SymbolColor);
            }
            set
            {
                this.symbolColor = value;
                if (this.owner != null)
                {
                    this.owner.SymbolColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public char TabSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.tabSymbol;
                }
                return this.owner.TabSymbol;
            }
            set
            {
                this.tabSymbol = value;
                if (this.owner != null)
                {
                    this.owner.TabSymbol = value;
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
        private char eofSymbol;
        private char eolSymbol;
        private WhiteSpace owner;
        private char spaceSymbol;
        private string symbolColor;
        private char tabSymbol;
        private bool visible;
    }
}

