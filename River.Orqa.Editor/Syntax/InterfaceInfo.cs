namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class InterfaceInfo : RangeInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public InterfaceInfo()
        {
            this.attributes = string.Empty;
        }

        public InterfaceInfo(string Name, string[] BaseTypes, Point Position, int Level) : base(Name, Position, Level)
        {
            this.attributes = string.Empty;
            this.baseTypes = BaseTypes;
        }

        public InterfaceInfo(string Name, string[] BaseTypes, Point Position, int Level, Point AttrPt, string Attributes) : this(Name, BaseTypes, Position, Level)
        {
            this.attributes = Attributes;
            this.attributePt = AttrPt;
        }

        public override void Clear()
        {
            base.Clear();
            this.baseTypes = null;
            this.properties.Clear();
            this.methods.Clear();
            this.events.Clear();
            this.attributePt = new Point(-1, -1);
            this.attributes = string.Empty;
        }

        public override ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            ISyntaxInfo info1 = this.methods.FindByName(Name, CaseSensitive);
            if (info1 == null)
            {
                info1 = this.properties.FindByName(Name, CaseSensitive);
            }
            if (info1 == null)
            {
                info1 = this.events.FindByName(Name, CaseSensitive);
            }
            if (info1 == null)
            {
                info1 = base.FindByName(Name, CaseSensitive);
            }
            return info1;
        }

        protected override void Init()
        {
            base.Init();
            this.properties = new SyntaxInfos();
            this.methods = new SyntaxInfos();
            this.events = new SyntaxInfos();
            this.attributePt = new Point(-1, -1);
            base.ImageIndex = UnitInfo.InterfaceImage;
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

        public string[] Base
        {
            get
            {
                return this.baseTypes;
            }
            set
            {
                this.baseTypes = value;
            }
        }

        public ISyntaxInfos Events
        {
            get
            {
                return this.events;
            }
        }

        public ISyntaxInfos Methods
        {
            get
            {
                return this.methods;
            }
        }

        public ISyntaxInfos Properties
        {
            get
            {
                return this.properties;
            }
        }


        // Fields
        private Point attributePt;
        private string attributes;
        private string[] baseTypes;
        private ISyntaxInfos events;
        private ISyntaxInfos methods;
        private ISyntaxInfos properties;
    }
}

