namespace River.Orqa.Editor
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IHyperText
    {
        // Events
        event HyperTextEvent CheckHyperText;

        // Methods
        bool IsHyperText(string Text);

        void ResetHighlightUrls();


        // Properties
        bool HighlightUrls { get; set; }

    }
}

