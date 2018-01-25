namespace River.Orqa.Editor
{
    using System;

    public interface IEdit
    {
        // Methods
        void ResetIndentOptions();

        void ResetModified();

        void ResetOverWrite();

        void ResetReadOnly();


        // Properties
        River.Orqa.Editor.IndentOptions IndentOptions { get; set; }

        bool Modified { get; set; }

        bool OverWrite { get; set; }

        bool ReadOnly { get; set; }

    }
}

