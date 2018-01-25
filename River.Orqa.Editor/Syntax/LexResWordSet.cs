namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;

    public class LexResWordSet : ILexResWordSet
    {
        // Methods
        public LexResWordSet()
        {
            this.resWordStyleIndex = -1;
            this.name = string.Empty;
            this.resWords = new ArrayList();
            this.resWordsHash = new Hashtable();
        }

        public LexResWordSet(ILexScheme Scheme, bool CaseSensitive) : this()
        {
            this.scheme = Scheme;
            this.caseSensitive = CaseSensitive;
        }

        public int AddResWord(string ResWord)
        {
            this.resWords.Add(ResWord);
            string text1 = this.caseSensitive ? ResWord : ResWord.ToLower();
            if (!this.resWordsHash.Contains(text1))
            {
                this.resWordsHash.Add(text1, text1);
            }
            return (this.resWords.Count - 1);
        }

        public void Clear()
        {
            this.resWords.Clear();
            this.resWordsHash.Clear();
        }

        public bool FindResWord(string ResWord)
        {
            return this.resWordsHash.Contains(this.caseSensitive ? ResWord : ResWord.ToLower());
        }

        protected internal void FixupReswordSet(ILexScheme Scheme, bool CaseSensitive)
        {
            this.scheme = Scheme;
            this.CaseSensitive = CaseSensitive;
            this.ResWordStyleIndex = this.resWordStyleIndex;
        }

        protected internal void UpdateResWords()
        {
            this.resWordsHash.Clear();
            foreach (string text2 in this.resWords)
            {
                string text1 = this.caseSensitive ? text2 : text2.ToLower();
                if (!this.resWordsHash.Contains(text1))
                {
                    this.resWordsHash.Add(text1, text1);
                }
            }
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
                    this.UpdateResWords();
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
                string[] textArray1 = new string[this.resWords.Count];
                this.resWords.CopyTo(textArray1);
                return textArray1;
            }
            set
            {
                this.Clear();
                string[] textArray1 = value;
                for (int num1 = 0; num1 < textArray1.Length; num1++)
                {
                    string text1 = textArray1[num1];
                    this.AddResWord(text1);
                }
            }
        }

        [XmlIgnore]
        public ILexStyle ResWordStyle
        {
            get
            {
                return this.resWordStyle;
            }
            set
            {
                this.resWordStyle = value;
            }
        }

        [XmlElement("ResWordStyle")]
        public int ResWordStyleIndex
        {
            get
            {
                if (this.resWordStyle == null)
                {
                    return this.resWordStyleIndex;
                }
                return this.resWordStyle.Index;
            }
            set
            {
                this.resWordStyleIndex = value;
                if ((this.scheme != null) && (value >= 0))
                {
                    this.resWordStyle = ((LexScheme) this.scheme).GetLexStyle(value);
                }
            }
        }


        // Fields
        private bool caseSensitive;
        private int index;
        private string name;
        private ArrayList resWords;
        private Hashtable resWordsHash;
        private ILexStyle resWordStyle;
        private int resWordStyleIndex;
        private ILexScheme scheme;
    }
}

