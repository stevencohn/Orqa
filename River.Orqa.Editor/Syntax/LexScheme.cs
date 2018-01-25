namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;

    public class LexScheme : ILexScheme
    {
        // Methods
        public LexScheme()
        {
            this.author = string.Empty;
            this.name = string.Empty;
            this.desc = string.Empty;
            this.copyright = string.Empty;
            this.version = "1.0";
            this.lexStates = new ArrayList();
            this.lexStyles = new ArrayList();
        }

        public ILexState AddLexState()
        {
            LexState state1 = new LexState(this);
            state1.Index = this.lexStates.Count;
            this.lexStates.Add(state1);
            return state1;
        }

        public ILexStyle AddLexStyle()
        {
            LexStyle style1 = new LexStyle(this);
            style1.Index = this.lexStyles.Count;
            this.lexStyles.Add(style1);
            return style1;
        }

        public void ClearScheme()
        {
            this.lexStyles.Clear();
            this.lexStates.Clear();
            this.author = string.Empty;
            this.name = string.Empty;
            this.desc = string.Empty;
            this.copyright = string.Empty;
        }

        private void FixupStates()
        {
            foreach (LexState state1 in this.lexStates)
            {
                state1.FixupBlocks(this);
            }
        }

        private void FixupStyles()
        {
            foreach (LexStyle style1 in this.lexStyles)
            {
                style1.Scheme = this;
            }
        }

        protected internal ILexState GetLexState(int Index)
        {
            return (LexState) this.lexStates[Index];
        }

        protected internal int GetLexStatesCount()
        {
            return this.lexStates.Count;
        }

        public ILexStyle GetLexStyle(int Index)
        {
            return (LexStyle) this.lexStyles[Index];
        }

        protected internal int GetLexStylesCount()
        {
            return this.lexStyles.Count;
        }

        protected internal void Init()
        {
            this.FixupStates();
        }

        public bool IsEmpty()
        {
            if ((((this.lexStyles.Count == 0) && (this.lexStates.Count == 0)) && ((this.author == string.Empty) && (this.name == string.Empty))) && (this.desc == string.Empty))
            {
                return (this.copyright == string.Empty);
            }
            return false;
        }

        public bool IsPlainText(int Style)
        {
            if ((Style >= 0) && (Style < this.lexStyles.Count))
            {
                return this.GetLexStyle(Style).PlainText;
            }
            return false;
        }


        // Properties
        public string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }

        public string Copyright
        {
            get
            {
                return this.copyright;
            }
            set
            {
                this.copyright = value;
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
        public ILexState[] LexStates
        {
            get
            {
                LexState[] stateArray1 = new LexState[this.lexStates.Count];
                this.lexStates.CopyTo(stateArray1);
                return stateArray1;
            }
            set
            {
                this.lexStates.Clear();
                ILexState[] stateArray1 = value;
                for (int num1 = 0; num1 < stateArray1.Length; num1++)
                {
                    ILexState state1 = stateArray1[num1];
                    this.lexStates.Add(state1);
                }
                this.FixupStates();
            }
        }

        [XmlIgnore]
        public ILexStyle[] LexStyles
        {
            get
            {
                LexStyle[] styleArray1 = new LexStyle[this.lexStyles.Count];
                this.lexStyles.CopyTo(styleArray1);
                return styleArray1;
            }
            set
            {
                this.lexStyles.Clear();
                ILexStyle[] styleArray1 = value;
                for (int num1 = 0; num1 < styleArray1.Length; num1++)
                {
                    ILexStyle style1 = styleArray1[num1];
                    this.lexStyles.Add(style1);
                }
                this.FixupStyles();
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
        public LexState[] States
        {
            get
            {
                return (LexState[]) this.LexStates;
            }
            set
            {
                this.LexStates = value;
            }
        }

        [XmlArray]
        public LexStyle[] Styles
        {
            get
            {
                return (LexStyle[]) this.LexStyles;
            }
            set
            {
                this.LexStyles = value;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }


        // Fields
        private string author;
        private string copyright;
        private string desc;
        private ArrayList lexStates;
        private ArrayList lexStyles;
        private string name;
        private string version;
    }
}

