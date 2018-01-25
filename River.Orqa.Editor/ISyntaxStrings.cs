#pragma warning disable 0108

namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    public interface ISyntaxStrings : IStringList, ICollection, IEnumerable, ITextSearch, IExport, IImport, ITabulationEx, ITabulation, IWordBreak, INotifyEx, INotify
    {
        // Methods
        int Add(string value);

        void Changed(int Index);

        void Changed(int First, int Last);

        void Clear();

        bool Contains(string value);

        char GetCharAt(Point Position);

        char GetCharAt(int X, int Y);

        StrItem GetItem(int Index);

        int GetLength(int Index);

        int IndexOf(string value);

        void Insert(int index, string value);

        void Remove(string value);

        void RemoveAt(int index);

        IList ToArrayList();

        string[] ToStringArray();


        // Properties
        string this[int Index] { get; }

        ITextSource Source { get; }

        string Text { get; set; }

    }
}

