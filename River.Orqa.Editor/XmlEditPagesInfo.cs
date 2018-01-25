namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public class XmlEditPagesInfo
    {
        // Methods
        public XmlEditPagesInfo()
        {
            this.backColor = EditConsts.DefaultPageBackColor.Name;
            this.borderColor = EditConsts.DefaultPageBorderColor.Name;
            this.displayWhiteSpace = true;
            this.pageType = River.Orqa.Editor.PageType.Normal;
            this.rulerOptions = EditConsts.DefaultRulerOptions;
            this.rulers = EditRulers.None;
            this.rulerUnits = EditConsts.DefaultRulerUnits;
        }

        public XmlEditPagesInfo(EditPages Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(EditPages Owner)
        {
            this.owner = Owner;
            this.BackColor = this.backColor;
            this.BorderColor = this.borderColor;
            this.DefaultPage = this.defaultPage;
            this.DisplayWhiteSpace = this.displayWhiteSpace;
            this.PageType = this.pageType;
            this.RulerOptions = this.rulerOptions;
            this.Rulers = this.rulers;
            this.RulerUnits = this.rulerUnits;
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
                return XmlHelper.SerializeColor(this.owner.BackColor);
            }
            set
            {
                this.backColor = value;
                if (this.owner != null)
                {
                    this.owner.BackColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public string BorderColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.borderColor;
                }
                return XmlHelper.SerializeColor(this.owner.BorderColor);
            }
            set
            {
                this.borderColor = value;
                if (this.owner != null)
                {
                    this.owner.BorderColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public XmlEditPageInfo DefaultPage
        {
            get
            {
                if (this.owner == null)
                {
                    return this.defaultPage;
                }
                return (XmlEditPageInfo) ((EditPage) this.owner.DefaultPage).XmlInfo;
            }
            set
            {
                this.defaultPage = value;
                if (this.owner != null)
                {
                    ((EditPage) this.owner.DefaultPage).XmlInfo = value;
                }
            }
        }

        public bool DisplayWhiteSpace
        {
            get
            {
                if (this.owner == null)
                {
                    return this.displayWhiteSpace;
                }
                return this.owner.DisplayWhiteSpace;
            }
            set
            {
                this.displayWhiteSpace = value;
                if (this.owner != null)
                {
                    this.owner.DisplayWhiteSpace = value;
                }
            }
        }

        public River.Orqa.Editor.PageType PageType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.pageType;
                }
                return this.owner.PageType;
            }
            set
            {
                this.pageType = value;
                if (this.owner != null)
                {
                    this.owner.PageType = value;
                }
            }
        }

        public River.Orqa.Editor.RulerOptions RulerOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulerOptions;
                }
                return this.owner.RulerOptions;
            }
            set
            {
                this.rulerOptions = value;
                if (this.owner != null)
                {
                    this.owner.RulerOptions = value;
                }
            }
        }

        public EditRulers Rulers
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulers;
                }
                return this.owner.Rulers;
            }
            set
            {
                this.rulers = value;
                if (this.owner != null)
                {
                    this.owner.Rulers = value;
                }
            }
        }

        public River.Orqa.Editor.RulerUnits RulerUnits
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulerUnits;
                }
                return this.owner.RulerUnits;
            }
            set
            {
                this.rulerUnits = value;
                if (this.owner != null)
                {
                    this.owner.RulerUnits = value;
                }
            }
        }


        // Fields
        private string backColor;
        private string borderColor;
        private XmlEditPageInfo defaultPage;
        private bool displayWhiteSpace;
        private EditPages owner;
        private River.Orqa.Editor.PageType pageType;
        private River.Orqa.Editor.RulerOptions rulerOptions;
        private EditRulers rulers;
        private River.Orqa.Editor.RulerUnits rulerUnits;
    }
}

