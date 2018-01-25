namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class MethodInfo : RangeInfo, IMethodInfo, IDelegateInfo, ISyntaxTypeInfo, IHasParams, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public MethodInfo()
        {
            this.dataType = string.Empty;
            this.attributes = string.Empty;
        }

        public MethodInfo(string Name, string DataType, Point Position, int Level) : base(Name, Position, Level)
        {
            this.dataType = string.Empty;
            this.attributes = string.Empty;
            this.dataType = DataType;
        }

        public MethodInfo(string Name, string DataType, Point Position, int Level, Point AttrPt, string Attributes) : this(Name, DataType, Position, Level)
        {
            this.attributes = Attributes;
            this.attributePt = AttrPt;
        }

        public override void Clear()
        {
            base.Clear();
            this.methodParams.Clear();
            this.statements.Clear();
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
            this.statements = new SyntaxInfos();
            this.attributePt = new Point(-1, -1);
            base.ImageIndex = UnitInfo.MethodImage;
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

        public string DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                this.dataType = value;
            }
        }

        public ISyntaxInfos Params
        {
            get
            {
                return this.methodParams;
            }
        }

        public ISyntaxInfos Statements
        {
            get
            {
                return this.statements;
            }
        }


        // Fields
        private Point attributePt;
        private string attributes;
        private string dataType;
        private ISyntaxInfos methodParams;
        private ISyntaxInfos statements;
    }
}

