namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface ICaret
    {
        // Methods
        void CreateCaret();

        void DestroyCaret();

        Size GetCaretSize(Point Position);

        void ResetHideCaret();

        void ShowCaret(int X, int Y);


        // Properties
        bool HideCaret { get; set; }

    }
}

