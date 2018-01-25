namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class VbPropInfo : PropInfo, IVbPropInfo, IPropInfo, ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, IVbScope, IVbModifiers, IHasParams
    {
        // Methods
        public VbPropInfo()
        {
        }

        public VbPropInfo(string Name, string DataType, Point Position, int Level, VbScope Scope, VbModifier Modifier) : base(Name, DataType, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifier;
        }

        public VbPropInfo(string Name, string DataType, Point Position, int Level, VbScope Scope, VbModifier Modifier, Point AttrPt, string Attributes) : base(Name, DataType, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
            this.modifiers = Modifier;
        }

        public override void Clear()
        {
            base.Clear();
            this.methodParams.Clear();
        }

        protected override void Init()
        {
            base.Init();
            this.methodParams = new SyntaxInfos();
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

        public ISyntaxInfos Params
        {
            get
            {
                return this.methodParams;
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
        private ISyntaxInfos methodParams;
        private VbModifier modifiers;
        private VbScope scope;
    }
}

