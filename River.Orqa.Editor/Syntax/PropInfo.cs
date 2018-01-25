namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class PropInfo : RangeInfo, IPropInfo, ISyntaxTypeInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public PropInfo()
        {
            this.dataType = string.Empty;
            this.attributes = string.Empty;
        }

        public PropInfo(string Name, string DataType, Point Position, int Level) : base(Name, Position, Level)
        {
            this.dataType = string.Empty;
            this.attributes = string.Empty;
            this.dataType = DataType;
        }

        public PropInfo(string Name, string DataType, Point Position, int Level, Point AttrPt, string Attributes) : this(Name, DataType, Position, Level)
        {
            this.attributes = Attributes;
            this.attributePt = AttrPt;
        }

        public override void Clear()
        {
            base.Clear();
            this.attributePt = new Point(-1, -1);
            this.attributes = string.Empty;
            this.propertyGet = null;
            this.propertySet = null;
        }

        protected override void Init()
        {
            base.Init();
            this.attributePt = new Point(-1, -1);
            base.ImageIndex = UnitInfo.PropImage;
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

        public IAccessorInfo PropertyGet
        {
            get
            {
                return this.propertyGet;
            }
            set
            {
                this.propertyGet = value;
            }
        }

        public IAccessorInfo PropertySet
        {
            get
            {
                return this.propertySet;
            }
            set
            {
                this.propertySet = value;
            }
        }


        // Fields
        private Point attributePt;
        private string attributes;
        private string dataType;
        private IAccessorInfo propertyGet;
        private IAccessorInfo propertySet;
    }
}

