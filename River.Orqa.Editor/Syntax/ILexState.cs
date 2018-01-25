namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Text.RegularExpressions;

    public interface ILexState
    {
        // Methods
        ILexSyntaxBlock AddLexSyntaxBlock();

        void ResetCaseSensitive();


        // Properties
        bool CaseSensitive { get; set; }

        string Desc { get; set; }

        string Expression { get; }

        int Index { get; }

        ILexSyntaxBlock[] LexSyntaxBlocks { get; set; }

        string Name { get; set; }

        System.Text.RegularExpressions.Regex Regex { get; }

    }
}

