namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class CsParamInfo : ParamInfo, ICsParamInfo, IParamInfo, ISyntaxTypeInfo, ISyntaxInfo, IAttributes
    {
        // Methods
        public CsParamInfo()
        {
            this.attributes = string.Empty;
        }

        public CsParamInfo(string Name, string DataType, Point Position, string Qualifier) : base(Name, DataType, Position, Qualifier)
        {
            this.attributes = string.Empty;
        }

        public CsParamInfo(string Name, string DataType, Point Position, string Qualifier, Point AttrPt, string Attributes) : base(Name, DataType, Position, Qualifier)
        {
            this.attributes = string.Empty;
            this.attributePt = AttrPt;
            this.attributes = Attributes;
        }

        public override void Clear()
        {
            base.Clear();
            this.attributePt = new Point(-1, -1);
            this.attributes = string.Empty;
        }

        protected override void Init()
        {
            base.Init();
            this.attributePt = new Point(-1, -1);
        }


        // Properties
        public Point AttributePt
        {
            get
            {
                return this.attributePt;
            }
            set
            {
                this.attributePt = value;
            }
        }

        public string Attributes
        {
            get
            {
                return this.attributes;
            }
            set
            {
                this.attributes = value;
            }
        }


        // Fields
        private Point attributePt;
        private string attributes;
    }
}

