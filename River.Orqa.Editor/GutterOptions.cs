namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum GutterOptions
    {
        // Fields
        None = 0,
        PaintBookMarks = 8,
        PaintLineNumbers = 1,
        PaintLinesBeyondEof = 4,
        PaintLinesOnGutter = 2
    }
}

