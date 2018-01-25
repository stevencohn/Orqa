namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface ISpellingEx : ISpelling
    {
        // Methods
        void Assign(ISpellingEx Source);

        void ResetSpellColor();


        // Properties
        Color SpellColor { get; set; }

    }
}

