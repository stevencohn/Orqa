namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum UndoOptions
    {
        // Fields
        AllowUndo = 2,
        GroupUndo = 4,
        UndoAfterSave = 0x10,
        UndoNavigations = 8
    }
}

