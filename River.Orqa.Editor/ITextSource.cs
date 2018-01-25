namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;

    public interface ITextSource : IEditEx, IEdit, INavigate, IUndo, ISourceNotify, INotifyEx, INotify, INotifier, IImport, IExport, IHyperText, ISpelling, IBraceMatching
    {
        // Methods
        Point AbsolutePositionToTextPoint(int Position);

        StrItem CreateStrItem(string S);

        int GetCharIndexFromPosition(Point Position);

        Point GetPositionFromCharIndex(int CharIndex);

        bool NeedParse();

        void ParseString(int Index);

        void ParseStrings(int First, int Last);

        void ParseToString(int Index);

        int TextPointToAbsolutePosition(Point Position);


        // Properties
        ISyntaxEdit ActiveEdit { get; set; }

        IBookMarks BookMarks { get; set; }

        IList Edits { get; }

        string FileName { get; set; }

        ILexer Lexer { get; set; }

        ISyntaxStrings Lines { get; set; }

        ILineStyles LineStyles { get; set; }

        int ParserLine { get; }

        string Text { get; set; }

    }
}

