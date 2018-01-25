namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface IUsesInfo : IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Properties
        ISyntaxInfos Uses { get; }

    }
}

