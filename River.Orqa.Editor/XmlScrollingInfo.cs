namespace River.Orqa.Editor
{
    using System;
    using System.Windows.Forms;

    public class XmlScrollingInfo
    {
        // Methods
        public XmlScrollingInfo()
        {
            this.scrollBars = RichTextBoxScrollBars.Both;
            this.defaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;
        }

        public XmlScrollingInfo(Scrolling Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Scrolling Owner)
        {
            this.owner = Owner;
            this.WindowOriginX = this.windowOriginX;
            this.WindowOriginY = this.windowOriginY;
            this.ScrollBars = this.scrollBars;
            this.DefaultHorzScrollSize = this.defaultHorzScrollSize;
        }


        // Properties
        public int DefaultHorzScrollSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.defaultHorzScrollSize;
                }
                return this.owner.DefaultHorzScrollSize;
            }
            set
            {
                this.defaultHorzScrollSize = value;
                if (this.owner != null)
                {
                    this.owner.DefaultHorzScrollSize = value;
                }
            }
        }

        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scrollBars;
                }
                return this.owner.ScrollBars;
            }
            set
            {
                this.scrollBars = value;
                if (this.owner != null)
                {
                    this.owner.ScrollBars = value;
                }
            }
        }

        public int WindowOriginX
        {
            get
            {
                if (this.owner == null)
                {
                    return this.windowOriginX;
                }
                return this.owner.WindowOriginX;
            }
            set
            {
                this.windowOriginX = value;
                if (this.owner != null)
                {
                    this.owner.WindowOriginX = value;
                }
            }
        }

        public int WindowOriginY
        {
            get
            {
                if (this.owner == null)
                {
                    return this.windowOriginY;
                }
                return this.owner.WindowOriginY;
            }
            set
            {
                this.windowOriginY = value;
                if (this.owner != null)
                {
                    this.owner.WindowOriginY = value;
                }
            }
        }


        // Fields
        private int defaultHorzScrollSize;
        private Scrolling owner;
        private RichTextBoxScrollBars scrollBars;
        private int windowOriginX;
        private int windowOriginY;
    }
}

