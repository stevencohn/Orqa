namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum IndentOptions
    {
        // Fields
        AutoIndent = 1,
        None = 0,
        SmartIndent = 2,
        UsePrevIndent = 8
    }
}

