namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public interface IStringList : ICollection, IEnumerable
    {
        // Properties
        string this[int Index] { get; }

    }
}

