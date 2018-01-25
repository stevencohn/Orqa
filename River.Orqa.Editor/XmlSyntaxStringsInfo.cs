namespace River.Orqa.Editor
{
    using System;

    public class XmlSyntaxStringsInfo
    {
        // Methods
        public XmlSyntaxStringsInfo()
        {
            int[] numArray1 = new int[1] { EditConsts.DefaultTabStop } ;
            this.tabStops = numArray1;
            string[] textArray1 = new string[0];
            this.lines = textArray1;
            this.delimiters = EditConsts.DefaultDelimiters;
        }

        public XmlSyntaxStringsInfo(SyntaxStrings Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(SyntaxStrings Owner)
        {
            this.owner = Owner;
            this.TabStops = this.tabStops;
            this.UseSpaces = this.useSpaces;
            this.Delimiters = this.delimiters;
            this.Lines = this.lines;
        }


        // Properties
        public string Delimiters
        {
            get
            {
                if (this.owner == null)
                {
                    return this.delimiters;
                }
                return this.owner.DelimiterString;
            }
            set
            {
                this.delimiters = value;
                if (this.owner != null)
                {
                    this.owner.DelimiterString = value;
                }
            }
        }

        public string[] Lines
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lines;
                }
                string[] textArray1 = new string[this.owner.Count];
                for (int num1 = 0; num1 < this.owner.Count; num1++)
                {
                    textArray1[num1] = this.owner[num1];
                }
                return textArray1;
            }
            set
            {
                this.lines = value;
                if (this.owner != null)
                {
                    this.owner.BeginUpdate();
                    try
                    {
                        this.owner.Clear();
                        string[] textArray1 = value;
                        for (int num1 = 0; num1 < textArray1.Length; num1++)
                        {
                            string text1 = textArray1[num1];
                            this.owner.Add(text1);
                        }
                    }
                    finally
                    {
                        this.owner.EndUpdate();
                    }
                }
            }
        }

        public int[] TabStops
        {
            get
            {
                if (this.owner == null)
                {
                    return this.tabStops;
                }
                return this.owner.TabStops;
            }
            set
            {
                this.tabStops = value;
                if (this.owner != null)
                {
                    this.owner.TabStops = value;
                }
            }
        }

        public bool UseSpaces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.useSpaces;
                }
                return this.owner.UseSpaces;
            }
            set
            {
                this.useSpaces = value;
                if (this.owner != null)
                {
                    this.owner.UseSpaces = value;
                }
            }
        }


        // Fields
        private string delimiters;
        private string[] lines;
        private SyntaxStrings owner;
        private int[] tabStops;
        private bool useSpaces;
    }
}

