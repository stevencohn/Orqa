namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum NavigateOptions
    {
        // Fields
        BeyondEof = 2,
        BeyondEol = 1,
        DownAtLineEnd = 8,
        MoveOnRightButton = 0x10,
        None = 0,
        UpAtLineBegin = 4
    }
}

