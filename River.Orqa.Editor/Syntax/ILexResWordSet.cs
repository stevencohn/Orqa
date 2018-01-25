namespace River.Orqa.Editor.Syntax
{
    using System;

    public interface ILexResWordSet
    {
        // Methods
        int AddResWord(string ResWord);

        void Clear();

        bool FindResWord(string ResWord);


        // Properties
        int Index { get; }

        string Name { get; set; }

        string[] ResWords { get; set; }

        ILexStyle ResWordStyle { get; set; }

    }
}

