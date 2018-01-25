namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface ICsNamespaceInfo : INamespaceInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        IUsesInfo Uses { get; }

    }
}

