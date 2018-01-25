namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class LexState : ILexState
    {
        // Methods
        public LexState()
        {
            this.lexSyntaxBlocks = new ArrayList();
            this.regex = new System.Text.RegularExpressions.Regex(string.Empty);
            this.blockTable = new Hashtable();
        }

        public LexState(ILexScheme Scheme) : this()
        {
            this.scheme = Scheme;
        }

        public ILexSyntaxBlock AddLexSyntaxBlock()
        {
            LexSyntaxBlock block1 = new LexSyntaxBlock(this.scheme);
            block1.CaseSensitive = this.caseSensitive;
            block1.Index = this.lexSyntaxBlocks.Count;
            this.lexSyntaxBlocks.Add(block1);
            return block1;
        }

        protected internal void AssignExpression(string Expr)
        {
            if (this.expression != Expr)
            {
                this.expression = Expr;
                RegexOptions options1 = RegexOptions.IgnorePatternWhitespace | (RegexOptions.Singleline | (RegexOptions.Compiled | RegexOptions.ExplicitCapture));
                if (!this.caseSensitive)
                {
                    options1 |= RegexOptions.IgnoreCase;
                }
                this.regex = new System.Text.RegularExpressions.Regex(Expr, options1);
                this.InitBlockHash();
            }
        }

        private void DoFixupBlocks()
        {
            StringBuilder builder1 = new StringBuilder();
            foreach (LexSyntaxBlock block1 in this.lexSyntaxBlocks)
            {
                block1.CaseSensitive = this.caseSensitive;
                string text1 = block1.Expression;
                if ((text1 != null) && (text1 != string.Empty))
                {
                    builder1.Append(string.Format("(?<{0}>{1})", "_" + block1.Index.ToString(), text1) + "|");
                }
            }
            if (builder1.Length > 0)
            {
                builder1.Remove(builder1.Length - 1, 1);
            }
            this.AssignExpression(builder1.ToString());
        }

        protected internal void FixupBlocks(ILexScheme Scheme)
        {
            this.scheme = Scheme;
            string text1 = string.Empty;
            foreach (LexSyntaxBlock block1 in this.lexSyntaxBlocks)
            {
                block1.FixupBlock(this.scheme, this.CaseSensitive);
            }
            this.DoFixupBlocks();
        }

        protected internal ILexSyntaxBlock GetSyntaxBlock(int SyntaxBlock)
        {
            return (LexSyntaxBlock) this.lexSyntaxBlocks[SyntaxBlock];
        }

        protected internal int GetSyntaxBlocksCount()
        {
            return this.lexSyntaxBlocks.Count;
        }

        private void InitBlockHash()
        {
            this.blockTable.Clear();
            int[] numArray1 = this.regex.GetGroupNumbers();
            int[] numArray2 = numArray1;
            for (int num3 = 0; num3 < numArray2.Length; num3++)
            {
                int num2 = numArray2[num3];
                string text1 = this.regex.GroupNameFromNumber(num2);
                if (((text1 != null) && (text1.Length > 0)) && (text1[0] == '_'))
                {
                    int num1 = int.Parse(text1.Substring(1));
                    this.blockTable.Add(num2, this.lexSyntaxBlocks[num1]);
                }
            }
        }

        public virtual void ResetCaseSensitive()
        {
            this.CaseSensitive = false;
        }


        // Properties
        protected internal Hashtable BlockTable
        {
            get
            {
                return this.blockTable;
            }
        }

        public bool CaseSensitive
        {
            get
            {
                return this.caseSensitive;
            }
            set
            {
                if (this.caseSensitive != value)
                {
                    this.caseSensitive = value;
                    this.DoFixupBlocks();
                }
            }
        }

        public string Desc
        {
            get
            {
                return this.desc;
            }
            set
            {
                this.desc = value;
            }
        }

        [XmlIgnore]
        public string Expression
        {
            get
            {
                return this.expression;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        [XmlIgnore]
        public ILexSyntaxBlock[] LexSyntaxBlocks
        {
            get
            {
                LexSyntaxBlock[] blockArray1 = new LexSyntaxBlock[this.lexSyntaxBlocks.Count];
                this.lexSyntaxBlocks.CopyTo(blockArray1);
                return blockArray1;
            }
            set
            {
                this.lexSyntaxBlocks.Clear();
                ILexSyntaxBlock[] blockArray1 = value;
                for (int num1 = 0; num1 < blockArray1.Length; num1++)
                {
                    LexSyntaxBlock block1 = (LexSyntaxBlock) blockArray1[num1];
                    this.lexSyntaxBlocks.Add(block1);
                }
                this.FixupBlocks(this.scheme);
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        [XmlIgnore]
        public System.Text.RegularExpressions.Regex Regex
        {
            get
            {
                return this.regex;
            }
        }

        [XmlIgnore]
        public ILexScheme Scheme
        {
            get
            {
                return this.scheme;
            }
            set
            {
                this.scheme = value;
            }
        }

        [XmlArray]
        public LexSyntaxBlock[] SyntaxBlocks
        {
            get
            {
                return (LexSyntaxBlock[]) this.LexSyntaxBlocks;
            }
            set
            {
                this.LexSyntaxBlocks = value;
            }
        }


        // Fields
        private Hashtable blockTable;
        private bool caseSensitive;
        private string desc;
        private string expression;
        private int index;
        private ArrayList lexSyntaxBlocks;
        private string name;
        private System.Text.RegularExpressions.Regex regex;
        private ILexScheme scheme;
    }
}

