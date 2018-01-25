namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;

    public interface IRangeInfo : IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        int GetIndentLevel(int Index);


        // Properties
        ISyntaxInfos Comments { get; }

        bool HasBlock { get; set; }

        ISyntaxInfos Regions { get; }

    }
}

