namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CsStructInfo : StructInfo, ICsStructInfo, IStructInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, ICsScope, ICsModifiers
    {
        // Methods
        public CsStructInfo()
        {
        }

        public CsStructInfo(string Name, string[] BaseTypes, Point Position, int Level, CsScope Scope, CsModifier Modifiers) : base(Name, BaseTypes, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }

        public CsStructInfo(string Name, string[] BaseTypes, Point Position, int Level, CsScope Scope, CsModifier Modifiers, Point AttrPt, string Attributes) : base(Name, BaseTypes, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }


        // Properties
        public CsModifier Modifiers
        {
            get
            {
                return this.modifiers;
            }
            set
            {
                this.modifiers = value;
            }
        }

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
        private CsModifier modifiers;
        private CsScope scope;
    }
}

