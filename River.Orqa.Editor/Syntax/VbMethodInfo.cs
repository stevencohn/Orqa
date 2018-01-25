namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class VbMethodInfo : MethodInfo, IVbMethodInfo, IMethodInfo, IDelegateInfo, ISyntaxTypeInfo, IHasParams, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, IVbScope, IVbModifiers, IHasLocalVars
    {
        // Methods
        public VbMethodInfo()
        {
        }

        public VbMethodInfo(string Name, string DataType, Point Position, int Level, VbScope Scope, VbModifier Modifiers) : base(Name, DataType, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }

        public VbMethodInfo(string Name, string DataType, Point Position, int Level, VbScope Scope, VbModifier Modifiers, Point AttrPt, string Attributes) : base(Name, DataType, Position, Level, AttrPt, Attributes)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }

        public override void Clear()
        {
            this.localVars.Clear();
            base.Clear();
        }

        public override ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            ISyntaxInfo info1 = this.localVars.FindByName(Name, CaseSensitive);
            if (info1 != null)
            {
                return info1;
            }
            return base.FindByName(Name, CaseSensitive);
        }

        protected override void Init()
        {
            base.Init();
            this.localVars = new SyntaxInfos();
        }


        // Properties
        public ISyntaxInfos LocalVars
        {
            get
            {
                return this.localVars;
            }
        }

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
        private ISyntaxInfos localVars;
        private VbModifier modifiers;
        private VbScope scope;
    }
}

