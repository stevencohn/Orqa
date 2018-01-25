namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public interface IPageHeader
    {
        // Methods
        void Assign(IPageHeader Source);

        int BeginUpdate();

        int EndUpdate();

        void Paint(ITextPainter Painter, Rectangle Rect, int PageIndex, int PageCount, bool PageNumbers);

        void ResetFont();

        void ResetFontColor();

        void ResetOffset();

        void ResetReverseOnEvenPages();

        void Update();


        // Properties
        string CenterText { get; set; }

        System.Drawing.Font Font { get; set; }

        Color FontColor { get; set; }

        string LeftText { get; set; }

        Point Offset { get; set; }

        bool ReverseOnEvenPages { get; set; }

        string RightText { get; set; }

        bool Visible { get; set; }

    }
}

