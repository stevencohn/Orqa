namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class VbEventInfo : EventInfo, IVbEventInfo, IEventInfo, ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, IVbScope, IVbModifiers
    {
        // Methods
        public VbEventInfo()
        {
        }

        public VbEventInfo(string Name, string DataType, Point Position, int Level, VbScope Scope, VbModifier Modifier) : base(Name, DataType, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = this.Modifiers;
        }

        public VbEventInfo(string Name, string DataType, Point Position, int Level, VbScope Scope, VbModifier Modifier, Point AttrPt, string Attributes) : base(Name, DataType, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
            this.modifiers = this.Modifiers;
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

