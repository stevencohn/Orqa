namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public interface IMargin
    {
        // Methods
        void Assign(IMargin Source);

        void CancelDragging();

        bool Contains(int X, int Y);

        void DragTo(int X, int Y);

        void Paint(ITextPainter Painter, Rectangle Rect);

        void ResetAllowDrag();

        void ResetPenColor();

        void ResetPosition();

        void ResetShowHints();

        void ResetVisible();


        // Properties
        bool AllowDrag { get; set; }

        bool IsDragging { get; set; }

        System.Drawing.Pen Pen { get; set; }

        Color PenColor { get; set; }

        int Position { get; set; }

        bool ShowHints { get; set; }

        bool Visible { get; set; }

    }
}

