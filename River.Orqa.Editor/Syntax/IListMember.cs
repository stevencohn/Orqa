namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;

    public interface IListMember : IMethodInfo, IDelegateInfo, ISyntaxTypeInfo, IHasParams, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        object CustomData { get; set; }

        string ParamText { get; set; }

        string Qualifier { get; set; }

    }
}

