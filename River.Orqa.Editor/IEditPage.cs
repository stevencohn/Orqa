namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;
    using System.Drawing.Printing;

    public interface IEditPage
    {
        // Methods
        void Assign(IEditPage Source);

        int BeginUpdate();

        int EndUpdate();

        Rectangle GetBounds(bool IncludeSpace);

        void Invalidate();

        void Paint(ITextPainter Painter);

        void Update();


        // Properties
        Rectangle BoundsRect { get; }

        Rectangle ClientRect { get; }

        int EndLine { get; }

        IPageHeader Footer { get; set; }

        IPageHeader Header { get; set; }

        int HorzOffset { get; set; }

        int Index { get; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }

        bool Landscape { get; set; }

        System.Drawing.Printing.Margins Margins { get; set; }

        IEditPage NextPage { get; }

        Point Origin { get; }

        PaperKind PageKind { get; set; }

        Size PageSize { get; set; }

        bool PaintNumber { get; set; }

        IEditPage PrevPage { get; }

        int StartLine { get; }

        int VertOffset { get; set; }

    }
}

