namespace River.Orqa.Editor.Syntax
{
    using System;

    [Flags]
    public enum CsModifier
    {
        // Fields
        Abstract = 8,
        Const = 2,
        Explicit = 0x100,
        Final = 0x4000,
        Implicit = 0x200,
        New = 0x800,
        None = 0,
        Operator = 0x1000,
        Override = 1,
        Partial = 0x2000,
        Readonly = 0x80,
        Sealed = 0x10,
        Static = 0x20,
        Synchronized = 0x8000,
        Unsafe = 0x40,
        Virtual = 4,
        Volatile = 0x400
    }
}

