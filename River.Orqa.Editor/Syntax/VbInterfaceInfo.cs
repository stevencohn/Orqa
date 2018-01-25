namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class VbInterfaceInfo : InterfaceInfo, IVbInterfaceInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, IVbScope
    {
        // Methods
        public VbInterfaceInfo()
        {
        }

        public VbInterfaceInfo(string Name, string[] BaseTypes, Point Position, int Level, VbScope Scope) : base(Name, BaseTypes, Position, Level)
        {
            this.scope = Scope;
        }

        public VbInterfaceInfo(string Name, string[] BaseTypes, Point Position, int Level, VbScope Scope, Point AttrPt, string Attributes) : base(Name, BaseTypes, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
        }


        // Properties
        public VbScope Scope
        {
            get
            {
                return this.scope;
            }
            set
            {
                this.scope = value;
            }
        }


        // Fields
        private VbScope scope;
    }
}

