namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface IBraceMatching
    {
        // Methods
        bool FindClosingBrace(ref Point Position);

        bool FindClosingBrace(ref int X, ref int Y);

        bool FindOpenBrace(ref Point Position);

        bool FindOpenBrace(ref int X, ref int Y);

        void ResetBracesOptions();

        void ResetClosingBraces();

        void ResetOpenBraces();

        void TempHighlightBraces(Rectangle[] Rectangles);

        void TempUnhighlightBraces();


        // Properties
        River.Orqa.Editor.BracesOptions BracesOptions { get; set; }

        char[] ClosingBraces { get; set; }

        char[] OpenBraces { get; set; }

    }
}

