namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;

    public interface IQuickInfo : ICodeCompletionProvider, IList, ICollection, IEnumerable
    {
        // Properties
        int BoldEnd { get; set; }

        int BoldStart { get; set; }

        string Text { get; set; }

    }
}

