namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class CsDelegateInfo : DelegateInfo, ICsDelegateInfo, IDelegateInfo, ISyntaxTypeInfo, ISyntaxInfo, IHasParams, IAttributes, ICsScope
    {
        // Methods
        public CsDelegateInfo()
        {
        }

        public CsDelegateInfo(string Name, string DataType, Point Position, CsScope Scope) : base(Name, DataType, Position)
        {
            this.scope = Scope;
        }

        public CsDelegateInfo(string Name, string DataType, Point Position, CsScope Scope, Point AttrPt, string Attributes) : base(Name, DataType, Position, AttrPt, Attributes)
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

