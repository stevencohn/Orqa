namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum SearchOptions
    {
        // Fields
        BackwardSearch = 8,
        CaseSensitive = 1,
        EntireScope = 0x20,
        FindTextAtCursor = 0x80,
        None = 0,
        PromptOnReplace = 0x100,
        RegularExpressions = 4,
        SearchHiddenText = 0x40,
        SelectionOnly = 0x10,
        WholeWordsOnly = 2
    }
}

