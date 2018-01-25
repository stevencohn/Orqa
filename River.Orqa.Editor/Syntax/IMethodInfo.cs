namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface IMethodInfo : IDelegateInfo, ISyntaxTypeInfo, IHasParams, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        ISyntaxInfos Statements { get; }

    }
}

