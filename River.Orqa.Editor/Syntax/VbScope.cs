namespace River.Orqa.Editor.Syntax
{
    using System;

    [Flags]
    public enum VbScope
    {
        // Fields
        Friend = 4,
        None = 0,
        Private = 1,
        Protected = 2,
        Public = 8,
        Static = 0x10
    }
}

