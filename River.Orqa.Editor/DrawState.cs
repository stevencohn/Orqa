namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum DrawState
    {
        // Fields
        BeyondEof = 0x200,
        BeyondEol = 0x100,
        Brace = 0x80,
        Control = 1,
        Gutter = 0x400,
        GutterImage = 0x800,
        LineBookMark = 0x40,
        LineHighlight = 0x10,
        LineNumber = 0x1000,
        LineSeparator = 0x20,
        None = 0,
        OutlineArea = 0x2000,
        OutlineButton = 0x8000,
        OutlineImage = 0x4000,
        Page = 0x20000,
        PageBorder = 0x80000,
        PageHeader = 0x40000,
        Selection = 4,
        Spelling = 0x10000,
        Text = 2,
        WhiteSpace = 8
    }
}

