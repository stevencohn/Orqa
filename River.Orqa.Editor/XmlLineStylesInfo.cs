namespace River.Orqa.Editor
{
    using System;

    public class XmlLineStylesInfo
    {
        // Methods
        public XmlLineStylesInfo()
        {
        }

        public XmlLineStylesInfo(LineStylesEx Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(LineStylesEx Owner)
        {
            this.owner = Owner;
            this.Styles = this.styles;
        }


        // Properties
        public LineStyle[] Styles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.styles;
                }
                LineStyle[] styleArray1 = new LineStyle[this.owner.Count];
                for (int num1 = 0; num1 < this.owner.Count; num1++)
                {
                    styleArray1[num1] = this.owner[num1];
                }
                return styleArray1;
            }
            set
            {
                this.styles = value;
                if (this.owner != null)
                {
                    this.owner.Clear();
                    LineStyle[] styleArray1 = value;
                    for (int num1 = 0; num1 < styleArray1.Length; num1++)
                    {
                        LineStyle style1 = styleArray1[num1];
                        this.owner.Add(style1);
                    }
                }
            }
        }


        // Fields
        private LineStylesEx owner;
        private LineStyle[] styles;
    }
}

