namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class NamespaceInfo : ClassInfo, INamespaceInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public NamespaceInfo()
        {
        }

        public NamespaceInfo(string Name, Point Position, int Level) : base(Name, null, Position, Level)
        {
        }

    }
}

