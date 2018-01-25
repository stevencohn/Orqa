namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface ILineSeparator
    {
        // Methods
        void Assign(ILineSeparator Source);

        bool NeedHide();

        bool NeedHighlight();

        void ResetHighlightBackColor();

        void ResetHighlightForeColor();

        void ResetLineColor();

        void ResetOptions();

        void TempHightlightLine(int Index);

        void TempUnhightlightLine();


        // Properties
        Color HighlightBackColor { get; set; }

        Color HighlightForeColor { get; set; }

        Color LineColor { get; set; }

        SeparatorOptions Options { get; set; }

    }
}

