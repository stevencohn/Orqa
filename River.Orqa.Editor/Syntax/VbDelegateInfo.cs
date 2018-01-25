namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class VbDelegateInfo : DelegateInfo, IVbDelegateInfo, IDelegateInfo, ISyntaxTypeInfo, ISyntaxInfo, IHasParams, IAttributes, IVbScope
    {
        // Methods
        public VbDelegateInfo()
        {
        }

        public VbDelegateInfo(string Name, string DataType, Point Position, VbScope Scope) : base(Name, DataType, Position)
        {
            this.scope = Scope;
        }

        public VbDelegateInfo(string Name, string DataType, Point Position, VbScope Scope, Point AttrPt, string Attributes) : base(Name, DataType, Position, AttrPt, Attributes)
        {
            this.scope = Scope;
        }


        // Properties
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
        private VbScope scope;
    }
}

