namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum StrItemState : byte
    {
        // Fields
        None = 0,
        Parsed = 1,
        ReadOnly = 2
    }
}

