namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class StringList : ArrayList, IStringList, ICollection, IEnumerable
    {
        // Methods
        public StringList()
        {
        }


        // Properties
		// SMC: added new
        public new string this[int Index]
        {
            get
            {
                return (string) base[Index];
            }
        }

    }
}

