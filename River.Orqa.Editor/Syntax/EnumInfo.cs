namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class EnumInfo : RangeInfo, IEnumInfo, ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public EnumInfo()
        {
            this.dataType = string.Empty;
            this.attributes = string.Empty;
        }

        public EnumInfo(string Name, string DataType, Point Position, int Level) : base(Name, Position, Level)
        {
            this.dataType = string.Empty;
            this.attributes = string.Empty;
            this.dataType = DataType;
        }

        public EnumInfo(string Name, string DataType, Point Position, int Level, Point AttrPt, string Attributes) : this(Name, DataType, Position, Level)
        {
            this.Attributes = Attributes;
            this.AttributePt = AttrPt;
        }

        public override void Clear()
        {
            base.Clear();
            this.fields.Clear();
            this.attributePt = new Point(-1, -1);
            this.attributes = string.Empty;
        }

        public override ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            ISyntaxInfo info1 = this.fields.FindByName(Name, CaseSensitive);
            if (info1 == null)
            {
                info1 = base.FindByName(Name, CaseSensitive);
            }
            return info1;
        }

        protected override void Init()
        {
            base.Init();
            this.fields = new SyntaxInfos();
            this.attributePt = new Point(-1, -1);
            base.ImageIndex = UnitInfo.EnumImage;
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

        public ISyntaxInfos Fields
        {
            get
            {
                return this.fields;
            }
        }


        // Fields
        private Point attributePt;
        private string attributes;
        private string dataType;
        private ISyntaxInfos fields;
    }
}

