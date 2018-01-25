namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public class XmlOutliningInfo
    {
        // Methods
        public XmlOutliningInfo()
        {
            this.outlineOptions = EditConsts.DefaultOutlineOptions;
            this.outlineColor = EditConsts.DefaultOutlineForeColor.Name;
        }

        public XmlOutliningInfo(Outlining Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Outlining Owner)
        {
            this.owner = Owner;
            this.AllowOutlining = this.allowOutlining;
            this.OutlineOptions = this.outlineOptions;
            this.OutlineColor = this.outlineColor;
        }


        // Properties
        public bool AllowOutlining
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowOutlining;
                }
                return this.owner.AllowOutlining;
            }
            set
            {
                this.allowOutlining = value;
                if (this.owner != null)
                {
                    this.owner.AllowOutlining = value;
                }
            }
        }

        public string OutlineColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlineColor;
                }
                return XmlHelper.SerializeColor(this.owner.OutlineColor);
            }
            set
            {
                this.outlineColor = value;
                if (this.owner != null)
                {
                    this.owner.OutlineColor = XmlHelper.DeserializeColor(value);
                }
            }
        }

        public River.Orqa.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlineOptions;
                }
                return this.owner.OutlineOptions;
            }
            set
            {
                this.outlineOptions = value;
                if (this.owner != null)
                {
                    this.owner.OutlineOptions = value;
                }
            }
        }


        // Fields
        private bool allowOutlining;
        private string outlineColor;
        private River.Orqa.Editor.OutlineOptions outlineOptions;
        private Outlining owner;
    }
}

