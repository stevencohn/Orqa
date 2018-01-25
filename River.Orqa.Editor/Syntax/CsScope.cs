namespace River.Orqa.Editor.Syntax
{
    using System;

    [Flags]
    public enum CsScope
    {
        // Fields
        Extern = 0x10,
        Internal = 8,
        None = 0,
        Private = 1,
        Protected = 2,
        Public = 4
    }
}

