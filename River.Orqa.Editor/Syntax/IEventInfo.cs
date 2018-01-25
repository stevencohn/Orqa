namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;

    public interface IEventInfo : ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        IAccessorInfo EventAdd { get; set; }

        IAccessorInfo EventRemove { get; set; }

    }
}

