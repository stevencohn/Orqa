namespace River.Orqa.Editor
{
    using System;

    public class XmlDisplayStringsInfo
    {
        // Methods
        public XmlDisplayStringsInfo()
        {
        }

        public XmlDisplayStringsInfo(DisplayStrings Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(DisplayStrings Owner)
        {
            this.owner = Owner;
            this.WordWrap = this.wordWrap;
            this.WrapAtMargin = this.wrapAtMargin;
        }


        // Properties
        public bool WordWrap
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wordWrap;
                }
                return this.owner.WordWrap;
            }
            set
            {
                this.wordWrap = value;
                if (this.owner != null)
                {
                    this.owner.WordWrap = value;
                }
            }
        }

        public bool WrapAtMargin
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wrapAtMargin;
                }
                return this.owner.WrapAtMargin;
            }
            set
            {
                this.wrapAtMargin = value;
                if (this.owner != null)
                {
                    this.owner.WrapAtMargin = value;
                }
            }
        }


        // Fields
        private DisplayStrings owner;
        private bool wordWrap;
        private bool wrapAtMargin;
    }
}

