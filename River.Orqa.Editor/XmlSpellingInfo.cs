namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public class XmlSpellingInfo
    {
        // Methods
        public XmlSpellingInfo()
        {
            this.spellColor = EditConsts.DefaultSpellForeColor.Name;
        }

        public XmlSpellingInfo(Spelling Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(Spelling Owner)
        {
            this.owner = Owner;
            this.SpellColor = this.spellColor;
        }


        // Properties
        public string SpellColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.spellColor;
                }
                return XmlHelper.SerializeColor(this.owner.SpellColor);
            }
            set
            {
                this.spellColor = value;
                if (this.owner != null)
                {
                    this.owner.SpellColor = XmlHelper.DeserializeColor(value);
                }
            }
        }


        // Fields
        private Spelling owner;
        private string spellColor;
    }
}

