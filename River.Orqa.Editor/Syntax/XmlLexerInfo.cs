namespace River.Orqa.Editor.Syntax
{
    using System;

    public class XmlLexerInfo
    {
        // Methods
        public XmlLexerInfo()
        {
            this.scheme = new LexScheme();
        }

        public XmlLexerInfo(Lexer Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Lexer Owner)
        {
            this.owner = Owner;
            this.owner.BeginUpdate();
            try
            {
                this.Scheme = this.scheme;
                this.DefaultState = this.defaultState;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }


        // Properties
        public int DefaultState
        {
            get
            {
                if (this.owner == null)
                {
                    return this.defaultState;
                }
                return this.owner.DefaultState;
            }
            set
            {
                this.defaultState = value;
                if (this.owner != null)
                {
                    this.owner.DefaultState = value;
                }
            }
        }

        public LexScheme Scheme
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scheme;
                }
                return this.owner.Scheme;
            }
            set
            {
                this.scheme = value;
                if (this.owner != null)
                {
                    this.owner.Scheme = value;
                }
            }
        }


        // Fields
        private int defaultState;
        private Lexer owner;
        private LexScheme scheme;
    }
}

