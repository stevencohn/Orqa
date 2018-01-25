namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;

    public class XmlSelectionInfo
    {
        // Methods
        public XmlSelectionInfo()
        {
            this.options = EditConsts.DefaultSelectionOptions;
            this.foreColor = EditConsts.DefaultHighlightForeColor.Name;
            this.backColor = EditConsts.DefaultHighlightBackColor.Name;
            this.inActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor.Name;
            this.inActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor.Name;
        }

        public XmlSelectionInfo(Selection Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Selection Owner)
        {
            this.owner = Owner;
            this.SelectionRect = this.selectionRect;
            this.SelectionType = this.selectionType;
            this.Options = this.options;
            this.ForeColor = this.foreColor;
            this.BackColor = this.backColor;
            this.InActiveForeColor = this.inActiveForeColor;
            this.InActiveBackColor = this.inActiveBackColor;
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

        public string ForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.foreColor;
                }
                return XmlHelper.SerializeColor(this.owner.ForeColor);
            }
            set
            {
                this.foreColor = value;
                if (this.owner != null)
                {
                    this.owner.ForeColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public string InActiveBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.inActiveBackColor;
                }
                return XmlHelper.SerializeColor(this.owner.InActiveBackColor);
            }
            set
            {
                this.inActiveBackColor = value;
                if (this.owner != null)
                {
                    this.owner.InActiveBackColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public string InActiveForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.inActiveForeColor;
                }
                return XmlHelper.SerializeColor(this.owner.InActiveForeColor);
            }
            set
            {
                this.inActiveForeColor = value;
                if (this.owner != null)
                {
                    this.owner.InActiveForeColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public SelectionOptions Options
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

        public Rectangle SelectionRect
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selectionRect;
                }
                return this.owner.SelectionRect;
            }
            set
            {
                this.selectionRect = value;
                if (this.owner != null)
                {
                    this.owner.SelectionRect = value;
                }
            }
        }

        public River.Orqa.Editor.SelectionType SelectionType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selectionType;
                }
                return this.owner.SelectionType;
            }
            set
            {
                this.selectionType = value;
                if (this.owner != null)
                {
                    this.owner.SelectionType = value;
                }
            }
        }


        // Fields
        private string backColor;
        private string foreColor;
        private string inActiveBackColor;
        private string inActiveForeColor;
        private SelectionOptions options;
        private Selection owner;
        private Rectangle selectionRect;
        private River.Orqa.Editor.SelectionType selectionType;
    }
}

