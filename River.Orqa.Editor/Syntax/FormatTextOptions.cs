namespace River.Orqa.Editor.Syntax
{
    using System;

    [Flags]
    public enum FormatTextOptions
    {
        // Fields
        CodeCompletion = 3,
        None = 0,
        Outline = 1,
        SmartIndent = 2
    }
}

