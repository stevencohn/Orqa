namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Runtime.InteropServices;

    public interface IParse : ILexer, IFormatText
    {
        // Methods
        int NextToken();

        int NextToken(out string str);

        int NextValidToken();

        int NextValidToken(out string str);

        int PeekToken();

        int PeekToken(out string str);

        int PeekValidToken();

        int PeekValidToken(out string str);

        void Reset();

        void Reset(int Line, int Pos, int State);

        void RestoreState();

        void RestoreState(bool Restore);

        void SaveState();


        // Properties
        int CurrentPosition { get; }

        bool Eof { get; }

        int LineIndex { get; }

        int State { get; }

        int Token { get; }

        int TokenPosition { get; }

        string TokenString { get; }

    }
}

