namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IGutter
    {
        // Events
        event EventHandler Click;
        event EventHandler DoubleClick;

        // Methods
        void Assign(IGutter Source);

        void OnClick(EventArgs args);

        void OnDoubleClick(EventArgs args);

        void Paint(ITextPainter Painter, Rectangle Rect);

        void Paint(ITextPainter Painter, Rectangle Rect, Point Location, int StartLine);

        void ResetBookMarkImageIndex();

        void ResetBrushColor();

        void ResetDrawLineBookmarks();

        void ResetLineBookmarksColor();

        void ResetLineNumbersAlignment();

        void ResetLineNumbersBackColor();

        void ResetLineNumbersForeColor();

        void ResetLineNumbersLeftIndent();

        void ResetLineNumbersRightIndent();

        void ResetLineNumbersStart();

        void ResetOptions();

        void ResetPenColor();

        void ResetVisible();

        void ResetWidth();

        void ResetWrapImageIndex();


        // Properties
        int BookMarkImageIndex { get; set; }

        System.Drawing.Brush Brush { get; set; }

        Color BrushColor { get; set; }

        bool DrawLineBookmarks { get; set; }

        ImageList Images { get; set; }

        Color LineBookmarksColor { get; set; }

        StringAlignment LineNumbersAlignment { get; set; }

        Color LineNumbersBackColor { get; set; }

        Color LineNumbersForeColor { get; set; }

        int LineNumbersLeftIndent { get; set; }

        int LineNumbersRightIndent { get; set; }

        int LineNumbersStart { get; set; }

        GutterOptions Options { get; set; }

        System.Drawing.Pen Pen { get; set; }

        Color PenColor { get; set; }

        Rectangle Rect { get; }

        bool Visible { get; set; }

        int Width { get; set; }

        int WrapImageIndex { get; set; }

    }
}

