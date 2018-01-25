namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum SeparatorOptions
    {
        // Fields
        HideHighlighting = 2,
        HighlightCurrentLine = 1,
        None = 0,
        SeparateLines = 4,
        SeparateWrapLines = 8
    }
}

