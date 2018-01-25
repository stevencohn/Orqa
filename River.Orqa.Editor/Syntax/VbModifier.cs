namespace River.Orqa.Editor.Syntax
{
    using System;

    [Flags]
    public enum VbModifier
    {
        // Fields
        Mustinherit = 2,
        Mustoverride = 1,
        None = 0,
        Notinheritable = 0x40,
        Notoverridable = 0x20,
        Overloads = 8,
        Overridable = 0x10,
        Overrides = 4,
        Readonly = 0x80,
        Shadows = 0x100,
        Shared = 0x200,
        Writeonly = 0x400
    }
}

