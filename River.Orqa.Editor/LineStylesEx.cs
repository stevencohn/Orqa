namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;

    [Serializable]
    public class LineStylesEx : ArrayList, ILineStylesEx, IList, ICollection, IEnumerable
    {
        // Methods
        public LineStylesEx(SyntaxEdit Owner)
        {
            this.owner = Owner;
        }

        public int Add(LineStyle value)
        {
            return base.Add(value);
        }

        public int AddLineStyle()
        {
            return this.Add(new LineStyle(this.owner));
        }

        public int AddLineStyle(string Name, Color ForeColor, Color BackColor, int ImageIndex, LineStyleOptions Options)
        {
            return this.Add(new LineStyle(this.owner, Name, ForeColor, BackColor, ImageIndex, Options));
        }

        public void AddRange(LineStyle[] values)
        {
            base.AddRange(values);
        }

        public void Assign(ILineStylesEx Source)
        {
            this.Clear();
            foreach (LineStyle style1 in Source)
            {
                this.AddLineStyle(style1.Name, style1.ForeColor, style1.BackColor, style1.ImageIndex, style1.Options);
            }
        }

        public ILineStyle GetLineStyle(int Index)
        {
            return this[Index];
        }

        protected internal ILineStyle GetStyle(int Index)
        {
            return this[Index];
        }

        public int IndexOfName(string Name)
        {
            for (int num1 = 0; num1 < this.Count; num1++)
            {
                if (this[num1].Name == Name)
                {
                    return num1;
                }
            }
            return -1;
        }


        // Properties
		// SMC: added new
        public new LineStyle this[int index]
        {
            get
            {
                return (LineStyle) base[index];
            }
            set
            {
                ((LineStyle) base[index]).Assign(value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlLineStylesInfo(this);
            }
            set
            {
                ((XmlLineStylesInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private SyntaxEdit owner;
    }
}

