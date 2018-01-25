namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface IClassInfo : IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        ISyntaxInfos Classes { get; }

        ISyntaxInfos Delegates { get; }

        ISyntaxInfos Enums { get; }

        ISyntaxInfos Fields { get; }

        ISyntaxInfos Interfaces { get; }

        ISyntaxInfos Structures { get; }

    }
}

