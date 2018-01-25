#pragma warning disable 0108

namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public interface ICodeTemplates : ICodeCompletionProvider, IList, ICollection, IEnumerable
    {
        // Methods
        ICodeTemplate AddTemplate();

        ICodeTemplate InsertTemplate(int Index);


        // Properties
        ICodeTemplate this[int Index] { get; }

    }
}

