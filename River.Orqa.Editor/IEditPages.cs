namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public interface IEditPages
    {
        // Events
        event DrawHeaderEvent DrawHeader;

        // Methods
        IEditPage Add();

        int Add(IEditPage Page);

        int BeginUpdate();

        void CancelDragging();

        void Clear();

        int EndUpdate();

        IEditPage GetPageAt(Point Point);

        IEditPage GetPageAt(int X, int Y);

        IEditPage GetPageAtPoint(Point Point);

        IEditPage GetPageAtPoint(int X, int Y);

        int GetPageIndexAt(Point Point);

        int GetPageIndexAt(int X, int Y);

        int GetPageIndexAtPoint(Point Point);

        int GetPageIndexAtPoint(int X, int Y);

        void InitDefaultPageSettings(PageSettings PageSettings);

        void Paint(ITextPainter Painter, Rectangle Rect);

        void Remove(IEditPage Page);

        void ResetBackColor();

        void ResetBorderColor();

        void ResetDisplayWhiteSpace();

        void ResetPageType();

        void ResetRulerOptions();

        void ResetRulers();

        void ResetRulerUnits();

        void Update();

        void Update(IEditPage Page);

        void Update(IEditPage Page, bool InvalidateOnly);


        // Properties
        Color BackColor { get; set; }

        Color BorderColor { get; set; }

        int Count { get; }

        IEditPage DefaultPage { get; set; }

        bool DisplayWhiteSpace { get; set; }

        int Height { get; }

        IEditRuler HorzRuler { get; }

        IEditPage this[int Index] { get; set; }

        IList Items { get; }

        River.Orqa.Editor.PageType PageType { get; set; }

        River.Orqa.Editor.RulerOptions RulerOptions { get; set; }

        EditRulers Rulers { get; set; }

        River.Orqa.Editor.RulerUnits RulerUnits { get; set; }

        IEditRuler VertRuler { get; }

        int Width { get; }

    }
}

