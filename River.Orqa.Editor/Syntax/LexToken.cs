namespace River.Orqa.Editor.Syntax
{
    using System;

    public enum LexToken
    {
        // Fields
        Comment = 3,
        Directive = 8,
        Identifier = 0,
        Number = 1,
        Resword = 2,
        String = 7,
        Symbol = 5,
        Whitespace = 6,
        XmlComment = 4
    }
}

