namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CsPropInfo : PropInfo, ICsPropInfo, IPropInfo, ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, ICsScope, ICsModifiers
    {
        // Methods
        public CsPropInfo()
        {
        }

        public CsPropInfo(string Name, string DataType, Point Position, int Level, CsScope Scope, CsModifier Modifier) : base(Name, DataType, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifier;
        }

        public CsPropInfo(string Name, string DataType, Point Position, int Level, CsScope Scope, CsModifier Modifier, Point AttrPt, string Attributes) : base(Name, DataType, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
            this.modifiers = Modifier;
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

