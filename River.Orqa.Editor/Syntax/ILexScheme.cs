namespace River.Orqa.Editor.Syntax
{
    using System;

    public interface ILexScheme
    {
        // Methods
        ILexState AddLexState();

        ILexStyle AddLexStyle();


        // Properties
        string Author { get; set; }

        string Copyright { get; set; }

        string Desc { get; set; }

        ILexState[] LexStates { get; set; }

        ILexStyle[] LexStyles { get; set; }

        string Name { get; set; }

        string Version { get; set; }

    }
}

