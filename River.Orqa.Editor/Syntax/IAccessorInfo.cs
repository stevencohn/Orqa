namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface IAccessorInfo : IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Properties
        ISyntaxInfos Statements { get; }

    }
}

