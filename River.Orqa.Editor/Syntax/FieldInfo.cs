namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class FieldInfo : SyntaxTypeInfo, IFieldInfo, ISyntaxTypeInfo, ISyntaxInfo, IAttributes
    {
        // Methods
        public FieldInfo()
        {
            this.attributes = string.Empty;
        }

        public FieldInfo(string Name, string DataType, Point Position) : base(Name, DataType, Position)
        {
            this.attributes = string.Empty;
        }

        public FieldInfo(string Name, string DataType, Point Position, Point AttrPt, string Attributes) : this(Name, DataType, Position)
        {
            this.attributes = Attributes;
            this.attributePt = AttrPt;
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
            base.ImageIndex = UnitInfo.FieldImage;
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

