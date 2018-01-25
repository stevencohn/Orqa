namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Dialogs;
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public interface ISearch : ITextSearch
    {
        // Methods
        bool CanFindNext();

        bool Find(string String);

        bool Find(string String, SearchOptions Options);

        bool Find(string String, SearchOptions Options, Regex Expression);

        bool FindNext();

        bool FindNextSelected();

        bool FindPrevious();

        bool FindPreviousSelected();

        void FinishIncrementalSearch();

        string GetTextAtCursor();

        bool IncrementalSearch(string Key, bool DeleteLast);

        int MarkAll(string String);

        int MarkAll(string String, SearchOptions Options);

        int MarkAll(string String, SearchOptions Options, Regex Expression);

        bool Replace(string String, string ReplaceWith);

        bool Replace(string String, string ReplaceWith, SearchOptions Options);

        bool Replace(string String, string ReplaceWith, SearchOptions Options, Regex Expression);

        int ReplaceAll(string String, string ReplaceWith);

        int ReplaceAll(string String, string ReplaceWith, SearchOptions Options);

        int ReplaceAll(string String, string ReplaceWith, SearchOptions Options, Regex Expression);

        void StartIncrementalSearch();

        void StartIncrementalSearch(bool BackwardSearch);


        // Properties
        bool FirstSearch { get; set; }

        IGotoLineDialog GotoLineDialog { get; set; }

        string IncrementalSearchString { get; }

        bool InIncrementalSearch { get; }

        ISearchDialog SearchDialog { get; set; }

        int SearchLen { get; }

        Point SearchPos { get; set; }

    }
}

