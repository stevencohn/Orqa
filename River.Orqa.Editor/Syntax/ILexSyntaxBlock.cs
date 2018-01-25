namespace River.Orqa.Editor.Syntax
{
    using System;

    public interface ILexSyntaxBlock
    {
        // Methods
        int AddExpression(string Expression);

        int AddLexResWordSet();

        int FindResWord(string ResWord);


        // Properties
        string Desc { get; set; }

        string Expression { get; }

        string[] Expressions { get; set; }

        int Index { get; }

        ILexState LeaveState { get; set; }

        ILexResWordSet[] LexResWordSets { get; set; }

        string Name { get; set; }

        ILexStyle Style { get; set; }

    }
}

