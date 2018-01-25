namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface IWordBreak
    {
        // Methods
        string GetTextAt(Point Position);

        string GetTextAt(int Pos, int Line);

        bool GetWord(int Index, int Pos, out int Left, out int Right);

        bool GetWord(string String, int Pos, out int Left, out int Right);

        bool IsDelimiter(char ch);

        bool IsDelimiter(int Index, int Pos);

        bool IsDelimiter(string String, int Pos);

        void ResetDelimiters();


        // Properties
        char[] Delimiters { get; set; }

        string DelimiterString { get; set; }

        Hashtable DelimTable { get; }

    }
}

