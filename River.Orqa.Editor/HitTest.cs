namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum HitTest
    {
        // Fields
        Above = 1,
        Below = 2,
        BeyondEof = 0x80,
        BeyondEol = 0x40,
        Gutter = 0x200,
        GutterImage = 0x800,
        HyperText = 0x8000,
        Left = 4,
        Margin = 0x400,
        None = 0,
        OutlineArea = 0x1000,
        OutlineButton = 0x4000,
        OutlineImage = 0x2000,
        Page = 0x10000,
        PageWhiteSpace = 0x1ffd3,
        Right = 8,
        Selection = 0x20,
        Text = 0x10
    }
}

