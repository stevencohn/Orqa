namespace River.Orqa.Editor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class Spelling : ISpellingEx, ISpelling
    {
        // Events
        [Browsable(false)]
        public event WordSpellEvent WordSpell
        {
            add
            {
                if (this.owner != null)
                {
                    this.owner.Source.WordSpell += value;
                }
            }
            remove
            {
                if (this.owner != null)
                {
                    this.owner.Source.WordSpell -= value;
                }
            }
        }

        // Methods
        public Spelling(ISyntaxEdit Owner)
        {
            this.spellColor = EditConsts.DefaultSpellForeColor;
            this.owner = Owner;
        }

        public void Assign(ISpellingEx Source)
        {
            this.CheckSpelling = Source.CheckSpelling;
            this.SpellColor = Source.SpellColor;
        }

        public bool IsWordCorrect(string Text)
        {
            if (this.owner == null)
            {
                return true;
            }
            return this.owner.Source.IsWordCorrect(Text);
        }

        public virtual void ResetCheckSpelling()
        {
            this.CheckSpelling = false;
        }

        public virtual void ResetSpellColor()
        {
            this.SpellColor = EditConsts.DefaultSpellForeColor;
        }

        public bool ShouldSerializeSpellColor()
        {
            return (this.spellColor != EditConsts.DefaultSpellForeColor);
        }


        // Properties
        [DefaultValue(false)]
        public bool CheckSpelling
        {
            get
            {
                if (this.owner == null)
                {
                    return false;
                }
                return this.owner.Source.CheckSpelling;
            }
            set
            {
                if (this.owner != null)
                {
                    this.owner.Source.CheckSpelling = value;
                }
            }
        }

        public Color SpellColor
        {
            get
            {
                return this.spellColor;
            }
            set
            {
                if (this.spellColor != value)
                {
                    this.spellColor = value;
                    if (this.CheckSpelling || (this.owner != null))
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public object XmlInfo
        {
            get
            {
                return new XmlSpellingInfo(this);
            }
            set
            {
                ((XmlSpellingInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private ISyntaxEdit owner;
        private Color spellColor;
    }
}

