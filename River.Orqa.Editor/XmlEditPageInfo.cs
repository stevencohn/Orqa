namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Drawing.Printing;

    public class XmlEditPageInfo
    {
        // Methods
        public XmlEditPageInfo()
        {
            this.horzOffset = EditConsts.DefaultPageHorzOffset;
            this.vertOffset = EditConsts.DefaultPageVertOffset;
            this.paintNumber = true;
        }

        public XmlEditPageInfo(EditPage Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(EditPage Owner)
        {
            this.owner = Owner;
            this.Footer = this.footer;
            this.Header = this.header;
            this.HorzOffset = this.horzOffset;
            this.Landscape = this.landscape;
            this.Margins = this.margins;
            this.PageKind = this.pageKind;
            this.PageSize = this.pageSize;
            this.PaintNumber = this.paintNumber;
            this.VertOffset = this.vertOffset;
        }


        // Properties
        public XmlPageHeaderInfo Footer
        {
            get
            {
                if (this.owner == null)
                {
                    return this.footer;
                }
                return (XmlPageHeaderInfo) ((PageHeader) this.owner.Footer).XmlInfo;
            }
            set
            {
                this.footer = value;
                if (this.owner != null)
                {
                    ((PageHeader) this.owner.Footer).XmlInfo = value;
                }
            }
        }

        public XmlPageHeaderInfo Header
        {
            get
            {
                if (this.owner == null)
                {
                    return this.header;
                }
                return (XmlPageHeaderInfo) ((PageHeader) this.owner.Header).XmlInfo;
            }
            set
            {
                this.header = value;
                if (this.owner != null)
                {
                    ((PageHeader) this.owner.Header).XmlInfo = value;
                }
            }
        }

        public int HorzOffset
        {
            get
            {
                if (this.owner == null)
                {
                    return this.horzOffset;
                }
                return this.owner.HorzOffset;
            }
            set
            {
                this.horzOffset = value;
                if (this.owner != null)
                {
                    this.owner.HorzOffset = value;
                }
            }
        }

        public bool Landscape
        {
            get
            {
                if (this.owner == null)
                {
                    return this.landscape;
                }
                return this.owner.Landscape;
            }
            set
            {
                this.landscape = value;
                if (this.owner != null)
                {
                    this.owner.Landscape = value;
                }
            }
        }

        public System.Drawing.Printing.Margins Margins
        {
            get
            {
                if (this.owner == null)
                {
                    return this.margins;
                }
                return this.owner.Margins;
            }
            set
            {
                this.margins = value;
                if (this.owner != null)
                {
                    this.owner.Margins = value;
                }
            }
        }

        public PaperKind PageKind
        {
            get
            {
                if (this.owner == null)
                {
                    return this.pageKind;
                }
                return this.owner.PageKind;
            }
            set
            {
                this.pageKind = value;
                if (this.owner != null)
                {
                    this.owner.PageKind = value;
                }
            }
        }

        public Size PageSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.pageSize;
                }
                return this.owner.PageSize;
            }
            set
            {
                this.pageSize = value;
                if (this.owner != null)
                {
                    this.owner.PageSize = value;
                }
            }
        }

        public bool PaintNumber
        {
            get
            {
                if (this.owner == null)
                {
                    return this.paintNumber;
                }
                return this.owner.PaintNumber;
            }
            set
            {
                this.paintNumber = value;
                if (this.owner != null)
                {
                    this.owner.PaintNumber = value;
                }
            }
        }

        public int VertOffset
        {
            get
            {
                if (this.owner == null)
                {
                    return this.vertOffset;
                }
                return this.owner.VertOffset;
            }
            set
            {
                this.vertOffset = value;
                if (this.owner != null)
                {
                    this.owner.VertOffset = value;
                }
            }
        }


        // Fields
        private XmlPageHeaderInfo footer;
        private XmlPageHeaderInfo header;
        private int horzOffset;
        private bool landscape;
        private System.Drawing.Printing.Margins margins;
        private EditPage owner;
        private PaperKind pageKind;
        private Size pageSize;
        private bool paintNumber;
        private int vertOffset;
    }
}

