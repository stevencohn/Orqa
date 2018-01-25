namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum OutlineOptions
    {
        // Fields
        DrawButtons = 4,
        DrawLines = 2,
        DrawOnGutter = 1,
        None = 0,
        ShowHints = 8
    }
}

