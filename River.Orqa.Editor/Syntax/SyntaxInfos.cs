namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class SyntaxInfos : ArrayList, ISyntaxInfos, IList, ICollection, IEnumerable
    {
        // Methods
        public SyntaxInfos()
        {
        }

        public int Add(ISyntaxInfo value)
        {
            return base.Add(value);
        }

        public ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            foreach (ISyntaxInfo info1 in this)
            {
                if (string.Compare(info1.Name, Name, !CaseSensitive) == 0)
                {
                    return info1;
                }
            }
            return null;
        }


        // Properties
		// SMC: added new
        public new ISyntaxInfo this[int index]
        {
            get
            {
                return (ISyntaxInfo) base[index];
            }
            set
            {
                base[index] = value;
            }
        }

    }
}

