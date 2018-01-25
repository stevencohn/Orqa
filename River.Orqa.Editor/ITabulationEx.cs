namespace River.Orqa.Editor
{
    using System;

    public interface ITabulationEx : ITabulation
    {
        // Methods
        string GetIndentString(int Count, int P);

        int GetPrevTabStop(int Pos);

        int GetTabStop(int Pos);

        string GetTabString(string s);

        int PosToTabPos(string String, int Pos);

        int TabPosToPos(string String, int Pos);

    }
}

