namespace River.Orqa.Editor
{
    using System;

    public enum UndoOperation
    {
        // Fields
        Break = 2,
        Delete = 1,
        DeleteBlock = 5,
        Insert = 0,
        InsertBlock = 4,
        Navigate = 6,
        UnBreak = 3,
        UndoBlock = 7,
        Unknown = 8
    }
}

