namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CsInterfaceInfo : InterfaceInfo, ICsInterfaceInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, ICsScope
    {
        // Methods
        public CsInterfaceInfo()
        {
        }

        public CsInterfaceInfo(string Name, string[] BaseTypes, Point Position, int Level, CsScope Scope) : base(Name, BaseTypes, Position, Level)
        {
            this.scope = Scope;
        }

        public CsInterfaceInfo(string Name, string[] BaseTypes, Point Position, int Level, CsScope Scope, Point AttrPt, string Attributes) : base(Name, BaseTypes, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
        }


        // Properties
        public CsScope Scope
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
        private CsScope scope;
    }
}

