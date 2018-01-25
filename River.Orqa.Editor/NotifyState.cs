namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum NotifyState
    {
        // Fields
        BlockChanged = 0x40,
        BookMarkChanged = 0x80,
        CountChanged = 2,
        Edit = 0x800,
        FirstSearchChanged = 0x20000,
        GotoBookMark = 0x8000,
        IncrementalSearchChanged = 0x100,
        Modified = 0x1000,
        ModifiedChanged = 8,
        None = 0,
        Outline = 0x2000,
        OverWriteChanged = 4,
        PositionChanged = 1,
        ReadOnlyChanged = 0x20,
        SearcRectChanged = 0x200,
        SelectBlock = 0x10000,
        SyntaxChanged = 0x10,
        Undo = 0x400,
        WordWrap = 0x4000
    }
}

