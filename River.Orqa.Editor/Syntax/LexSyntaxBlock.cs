namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Xml.Serialization;

    public class LexSyntaxBlock : ILexSyntaxBlock
    {
        // Methods
        public LexSyntaxBlock()
        {
            this.lexStyleIndex = -1;
            this.leaveStateIndex = -1;
            this.expressions = new ArrayList();
            this.lexResWordSets = new ArrayList();
        }

        public LexSyntaxBlock(ILexScheme Scheme) : this()
        {
            this.scheme = Scheme;
        }

        public int AddExpression(string Expression)
        {
            return this.expressions.Add(Expression);
        }

        public int AddLexResWordSet()
        {
            LexResWordSet set1 = new LexResWordSet(this.scheme, this.CaseSensitive);
            set1.Index = this.lexResWordSets.Count;
            return this.lexResWordSets.Add(set1);
        }

        public int FindResWord(string ResWord)
        {
            for (int num1 = 0; num1 < this.lexResWordSets.Count; num1++)
            {
                if (((LexResWordSet) this.lexResWordSets[num1]).FindResWord(ResWord))
                {
                    return num1;
                }
            }
            return -1;
        }

        protected internal void FixupBlock(ILexScheme Scheme, bool CaseSensitive)
        {
            this.scheme = Scheme;
            this.LexStyleIndex = this.lexStyleIndex;
            this.LeaveStateIndex = this.leaveStateIndex;
            foreach (LexResWordSet set1 in this.lexResWordSets)
            {
                set1.FixupReswordSet(Scheme, CaseSensitive);
            }
        }

        public bool ShouldSerializeResWords()
        {
            return false;
        }

        public bool ShouldSerializeResWordSets()
        {
            return (this.lexResWordSets.Count > 0);
        }

        public bool ShouldSerializeResWordStyle()
        {
            return false;
        }

        public bool ShouldSerializeResWordStyleIndex()
        {
            return (this.ResWordStyleIndex != -1);
        }


        // Properties
        [XmlIgnore]
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
                    foreach (LexResWordSet set1 in this.lexResWordSets)
                    {
                        set1.CaseSensitive = value;
                    }
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

        public string Expression
        {
            get
            {
                switch (this.expressions.Count)
                {
                    case 0:
                    {
                        return string.Empty;
                    }
                    case 1:
                    {
                        return (string) this.expressions[0];
                    }
                }
                StringBuilder builder1 = new StringBuilder();
                foreach (string text1 in this.expressions)
                {
                    builder1.Append(string.Format("({0})", text1) + "|");
                }
                if (builder1.Length > 0)
                {
                    builder1.Remove(builder1.Length - 1, 1);
                }
                return builder1.ToString();
            }
        }

        [XmlArray]
        public string[] Expressions
        {
            get
            {
                string[] textArray1 = new string[this.expressions.Count];
                this.expressions.CopyTo(textArray1);
                return textArray1;
            }
            set
            {
                this.expressions.Clear();
                string[] textArray1 = value;
                for (int num1 = 0; num1 < textArray1.Length; num1++)
                {
                    string text1 = textArray1[num1];
                    this.expressions.Add(text1);
                }
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
        public ILexState LeaveState
        {
            get
            {
                return this.leaveState;
            }
            set
            {
                this.leaveState = value;
            }
        }

        [XmlElement("LeaveState")]
        public int LeaveStateIndex
        {
            get
            {
                if (this.leaveState == null)
                {
                    return this.leaveStateIndex;
                }
                return this.leaveState.Index;
            }
            set
            {
                this.leaveStateIndex = value;
                if ((this.scheme != null) && (value >= 0))
                {
                    this.leaveState = ((LexScheme) this.scheme).GetLexState(value);
                }
            }
        }

        [XmlIgnore]
        public ILexResWordSet[] LexResWordSets
        {
            get
            {
                LexResWordSet[] setArray1 = new LexResWordSet[this.lexResWordSets.Count];
                this.lexResWordSets.CopyTo(setArray1);
                return setArray1;
            }
            set
            {
                this.lexResWordSets.Clear();
                ILexResWordSet[] setArray1 = value;
                for (int num1 = 0; num1 < setArray1.Length; num1++)
                {
                    ILexResWordSet set1 = setArray1[num1];
                    this.lexResWordSets.Add(set1);
                }
            }
        }

        [XmlElement("LexStyle")]
        public int LexStyleIndex
        {
            get
            {
                if (this.style == null)
                {
                    return this.lexStyleIndex;
                }
                return this.style.Index;
            }
            set
            {
                this.lexStyleIndex = value;
                if ((this.scheme != null) && (value >= 0))
                {
                    this.style = ((LexScheme) this.scheme).GetLexStyle(value);
                }
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

        [XmlArray]
        public string[] ResWords
        {
            get
            {
                if (this.lexResWordSets.Count <= 0)
                {
                    return new string[0];
                }
                return ((LexResWordSet) this.lexResWordSets[0]).ResWords;
            }
            set
            {
                if (this.lexResWordSets.Count == 0)
                {
                    if ((value == null) || (value.Length == 0))
                    {
                        return;
                    }
                    this.AddLexResWordSet();
                }
                ((LexResWordSet) this.lexResWordSets[0]).ResWords = value;
            }
        }

        [XmlArray]
        public LexResWordSet[] ResWordSets
        {
            get
            {
                return (LexResWordSet[]) this.LexResWordSets;
            }
            set
            {
                this.LexResWordSets = value;
            }
        }

        [XmlElement("ResWordStyle")]
        public int ResWordStyleIndex
        {
            get
            {
                if (this.lexResWordSets.Count <= 0)
                {
                    return -1;
                }
                return ((LexResWordSet) this.lexResWordSets[0]).ResWordStyleIndex;
            }
            set
            {
                if (this.lexResWordSets.Count == 0)
                {
                    if (value < 0)
                    {
                        return;
                    }
                    this.AddLexResWordSet();
                }
                ((LexResWordSet) this.lexResWordSets[0]).ResWordStyleIndex = value;
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

        [XmlIgnore]
        public ILexStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }


        // Fields
        private bool caseSensitive;
        private string desc;
        private ArrayList expressions;
        private int index;
        private ILexState leaveState;
        private int leaveStateIndex;
        private ArrayList lexResWordSets;
        private int lexStyleIndex;
        private string name;
        private ILexScheme scheme;
        private ILexStyle style;
    }
}

