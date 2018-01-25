namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;

    public interface IInterfaceInfo : IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        string[] Base { get; set; }

        ISyntaxInfos Events { get; }

        ISyntaxInfos Methods { get; }

        ISyntaxInfos Properties { get; }

    }
}

