namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CsClassInfo : ClassInfo, ICsClassInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, ICsScope, ICsModifiers
    {
        // Methods
        public CsClassInfo()
        {
        }

        public CsClassInfo(string Name, string[] BaseTypes, Point Position, int Level, CsScope Scope, CsModifier Modifiers) : base(Name, BaseTypes, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }

        public CsClassInfo(string Name, string[] BaseTypes, Point Position, int Level, CsScope Scope, CsModifier Modifiers, Point AttrPt, string Attributes) : base(Name, BaseTypes, Position, Level, AttrPt, Attributes)
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

