namespace River.Orqa.Editor
{
    using System;
    using System.Runtime.CompilerServices;

    public interface ISpelling
    {
        // Events
        event WordSpellEvent WordSpell;

        // Methods
        bool IsWordCorrect(string Text);

        void ResetCheckSpelling();


        // Properties
        bool CheckSpelling { get; set; }

    }
}

