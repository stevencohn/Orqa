namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface IEnumInfo : ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        ISyntaxInfos Fields { get; }

    }
}

