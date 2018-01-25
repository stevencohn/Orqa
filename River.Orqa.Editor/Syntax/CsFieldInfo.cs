namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class CsFieldInfo : FieldInfo, ICsFieldInfo, IFieldInfo, ISyntaxTypeInfo, ISyntaxInfo, IAttributes, ICsScope
    {
        // Methods
        public CsFieldInfo()
        {
        }

        public CsFieldInfo(string Name, string DataType, Point Position, CsScope Scope) : base(Name, DataType, Position)
        {
            this.scope = Scope;
        }

        public CsFieldInfo(string Name, string DataType, Point Position, CsScope Scope, Point AttrPt, string Attributes) : base(Name, DataType, Position, AttrPt, Attributes)
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

