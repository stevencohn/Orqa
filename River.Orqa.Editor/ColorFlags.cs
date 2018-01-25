namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum ColorFlags : byte
    {
        // Fields
        Brace = 0x20,
        HyperText = 0x10,
        MisSpelledWord = 8,
        None = 0,
        OutlineSection = 4,
        Tabulation = 2,
        WhiteSpace = 1
    }
}

