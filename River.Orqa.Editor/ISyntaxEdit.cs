namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface ISyntaxEdit : IDrawLine, ISearch, ITextSearch, INotifier, ICaret, INavigateEx, INavigate, IEdit, IWordWrap, IFmtExport, IExport, IFmtImport, IImport, ICodeCompletion, IControlProps
    {
        // Events
        event CustomDrawEvent CustomDraw;
        event NotifyEvent SourceStateChanged;

        // Methods
        int CharsInWidth();

        int CharsInWidth(int Width);

        Point DisplayToScreen(int X, int Y);

        void GetHitTest(Point Point, ref HitTestInfo HitTestInfo);

        void GetHitTest(int X, int Y, ref HitTestInfo HitTestInfo);

        void GetHitTestAtTextPoint(Point Point, ref HitTestInfo HitTestInfo);

        void GetHitTestAtTextPoint(int X, int Y, ref HitTestInfo HitTestInfo);

        void HideScrollHint();

        int LinesInHeight();

        int LinesInHeight(int Height);

        void MakeVisible(Point Position);

        void MakeVisible(Point Position, bool CenterLine);

        bool OnCustomDraw(ITextPainter Painter, Rectangle Rect, DrawStage DrawStage, DrawState DrawState, DrawInfo DrawInfo);

        void ResetBorderStyle();

        void ResetWantReturns();

        void ResetWantTabs();

        Point ScreenToDisplay(int X, int Y);

        Point ScreenToText(Point Position);

        Point ScreenToText(int X, int Y);

        void ShowScrollHint(int Pos);

        Point TextToScreen(Point Position);

        Point TextToScreen(int X, int Y);


        // Properties
        EditBorderStyle BorderStyle { get; set; }

        IBraceMatchingEx Braces { get; set; }

        Rectangle ClientRect { get; }

        IDisplayStrings DisplayLines { get; }

        IGutter Gutter { get; set; }

        IHyperTextEx HyperText { get; set; }

        IKeyList KeyList { get; }

        ILexer Lexer { get; set; }

        ISyntaxStrings Lines { get; set; }

        ILineSeparator LineSeparator { get; set; }

        ILineStylesEx LineStyles { get; set; }

        IMargin Margin { get; set; }

        IOutlining Outlining { get; set; }

        IEditPages Pages { get; set; }

        ITextPainter Painter { get; }

        IPrinting Printing { get; set; }

        IScrolling Scrolling { get; set; }

        ISelection Selection { get; set; }

        ITextSource Source { get; set; }

        ISpellingEx Spelling { get; set; }

        bool Transparent { get; set; }

        bool WantReturns { get; set; }

        bool WantTabs { get; set; }

        IWhiteSpace WhiteSpace { get; set; }

    }
}

