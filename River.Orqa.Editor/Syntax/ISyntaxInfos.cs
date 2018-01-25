#pragma warning disable 0108

namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public interface ISyntaxInfos : IList, ICollection, IEnumerable
    {
        // Methods
        int Add(ISyntaxInfo value);

        ISyntaxInfo FindByName(string Name, bool CaseSensitive);


        // Properties
        ISyntaxInfo this[int index] { get; set; }

    }
}

