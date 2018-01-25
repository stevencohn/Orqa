namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum PrintOptions
    {
        // Fields
        DisplayProgress = 0x100,
        LineNumbers = 1,
        None = 0,
        PageNumbers = 2,
        PrintSelection = 8,
        UseColors = 0x10,
        UseFooter = 0x80,
        UseHeader = 0x40,
        UseSyntax = 0x20,
        WordWrap = 4
    }
}

