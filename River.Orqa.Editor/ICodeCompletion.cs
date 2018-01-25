namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface ICodeCompletion
    {
        // Events
        event CodeCompletionEvent NeedCodeCompletion;

        // Methods
        void CodeTemplates();

        void CompleteWord();

        bool IsValidText(Point Position);

        void ListMembers();

        void ParameterInfo();

        void QuickInfo();

        string RemovePlainText(int Line);

        void ShowCodeCompletionBox(ICodeCompletionProvider Provider);

        void ShowCodeCompletionBox(ICodeCompletionProvider Provider, Point Pt);

        void ShowCodeCompletionHint(ICodeCompletionProvider Provider);

        void ShowCodeCompletionHint(ICodeCompletionProvider Provider, Point Pt);


        // Properties
        ICodeCompletionBox CodeCompletionBox { get; }

        string CodeCompletionChars { get; set; }

        ICodeCompletionHint CodeCompletionHint { get; }

    }
}

