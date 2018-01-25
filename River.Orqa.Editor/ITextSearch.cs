namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    public interface ITextSearch
    {
        // Methods
        bool Find(string String, SearchOptions Options, Regex Expression, ref Point Position, out int Len);

    }
}

