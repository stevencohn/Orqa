namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CsMethodInfo : MethodInfo, ICsMethodInfo, IMethodInfo, IDelegateInfo, ISyntaxTypeInfo, IHasParams, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, ICsScope, ICsModifiers, IHasLocalVars
    {
        // Methods
        public CsMethodInfo()
        {
        }

        public CsMethodInfo(string Name, string DataType, Point Position, int Level, CsScope Scope, CsModifier Modifiers) : base(Name, DataType, Position, Level)
        {
            this.scope = Scope;
            this.modifiers = Modifiers;
        }

        public CsMethodInfo(string Name, string DataType, Point Position, int Level, CsScope Scope, CsModifier Modifiers, Point AttrPt, string Attributes) : base(Name, DataType, Position, Level, AttrPt, Attributes)
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
        private ISyntaxInfos localVars;
        private CsModifier modifiers;
        private CsScope scope;
    }
}

