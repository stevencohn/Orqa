namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;

    public interface IPropInfo : ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Properties
        IAccessorInfo PropertyGet { get; set; }

        IAccessorInfo PropertySet { get; set; }

    }
}

