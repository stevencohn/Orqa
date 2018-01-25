namespace River.Orqa.Editor.Syntax
{
    using System;

    public enum UpdateReason
    {
        // Fields
        Break = 3,
        Delete = 2,
        DeleteBlock = 5,
        Insert = 1,
        InsertBlock = 6,
        Navigate = 0,
        Other = 7,
        UnBreak = 4
    }
}

