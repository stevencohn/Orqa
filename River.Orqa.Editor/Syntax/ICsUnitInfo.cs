namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface ICsUnitInfo : IUnitInfo, INamespaceInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        ISyntaxInfos Namespaces { get; }

        IUsesInfo Uses { get; }

    }
}

