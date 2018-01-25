namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class VbStructInfo : StructInfo, IVbStructInfo, IStructInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, IVbScope, IVbModifiers
    {
        // Methods
        public VbStructInfo()
        {
        }

        public VbStructInfo(string Name, string[] BaseTypes, Point Position, int Level, VbScope Scope, VbModifier Modifiers) : base(Name, BaseTypes, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }

        public VbStructInfo(string Name, string[] BaseTypes, Point Position, int Level, VbScope Scope, VbModifier Modifiers, Point AttrPt, string Attributes) : base(Name, BaseTypes, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }


        // Properties
        public VbModifier Modifiers
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
        private VbModifier modifiers;
        private VbScope scope;
    }
}

