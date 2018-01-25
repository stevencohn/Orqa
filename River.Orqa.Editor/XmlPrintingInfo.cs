namespace River.Orqa.Editor
{
    using System;

    public class XmlPrintingInfo
    {
        // Methods
        public XmlPrintingInfo()
        {
            this.options = EditConsts.DefaultPrintOptions;
            this.allowedOptions = EditConsts.DefaultPrintOptions;
        }

        public XmlPrintingInfo(Printing Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Printing Owner)
        {
            this.owner = Owner;
            this.Options = this.options;
            this.AllowedOptions = this.allowedOptions;
            this.Header = this.header;
            this.Footer = this.footer;
        }


        // Properties
        public PrintOptions AllowedOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowedOptions;
                }
                return this.owner.AllowedOptions;
            }
            set
            {
                this.allowedOptions = value;
                if (this.owner != null)
                {
                    this.owner.AllowedOptions = value;
                }
            }
        }

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

        public PrintOptions Options
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


        // Fields
        private PrintOptions allowedOptions;
        private XmlPageHeaderInfo footer;
        private XmlPageHeaderInfo header;
        private PrintOptions options;
        private Printing owner;
    }
}

