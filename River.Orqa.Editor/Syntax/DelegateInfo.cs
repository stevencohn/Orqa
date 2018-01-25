namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class DelegateInfo : SyntaxTypeInfo, IDelegateInfo, ISyntaxTypeInfo, ISyntaxInfo, IHasParams, IAttributes
    {
        // Methods
        public DelegateInfo()
        {
            this.attributes = string.Empty;
        }

        public DelegateInfo(string Name, string DataType, Point Position) : base(Name, DataType, Position)
        {
            this.attributes = string.Empty;
        }

        public DelegateInfo(string Name, string DataType, Point Position, Point AttrPt, string Attributes) : this(Name, DataType, Position)
        {
            this.attributes = Attributes;
            this.attributePt = AttrPt;
        }

        public override void Clear()
        {
            base.Clear();
            this.methodParams.Clear();
            this.attributePt = new Point(-1, -1);
            this.attributes = string.Empty;
        }

        public override ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            ISyntaxInfo info1 = this.methodParams.FindByName(Name, CaseSensitive);
            if (info1 == null)
            {
                info1 = base.FindByName(Name, CaseSensitive);
            }
            return info1;
        }

        protected override void Init()
        {
            base.Init();
            this.methodParams = new SyntaxInfos();
            this.attributePt = new Point(-1, -1);
            base.ImageIndex = UnitInfo.DelegateImage;
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

        public ISyntaxInfos Params
        {
            get
            {
                return this.methodParams;
            }
        }


        // Fields
        private Point attributePt;
        private string attributes;
        private ISyntaxInfos methodParams;
    }
}

