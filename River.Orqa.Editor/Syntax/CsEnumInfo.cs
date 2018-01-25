namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CsEnumInfo : EnumInfo, ICsEnumInfo, IEnumInfo, ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, ICsScope, ICsModifiers
    {
        // Methods
        public CsEnumInfo()
        {
        }

        public CsEnumInfo(string Name, string DataType, Point Position, int Level, CsScope Scope, CsModifier Modifiers) : base(Name, DataType, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }

        public CsEnumInfo(string Name, string DataType, Point Position, int Level, CsScope Scope, CsModifier Modifiers, Point AttrPt, string Attributes) : base(Name, DataType, Position, Level, AttrPt, Attributes)
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

