namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public interface ILexer
    {
        // Events
        event TextParsedEvent TextParsed;

        // Methods
        void LoadScheme(TextReader Reader);

        void LoadScheme(string FileName);

        int ParseText(int State, string String, ref short[] ColorData);

        int ParseText(int State, string String, ref int Pos, ref int Len, ref int Style);

        void ResetDefaultState();

        void SaveScheme(TextWriter Writer);

        void SaveScheme(string FileName);


        // Properties
        int DefaultState { get; set; }

        LexScheme Scheme { get; set; }

        string XmlScheme { get; set; }

    }
}

