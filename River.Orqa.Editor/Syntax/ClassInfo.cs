namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class ClassInfo : InterfaceInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public ClassInfo()
        {
        }

        public ClassInfo(string Name, string[] BaseTypes, Point Position, int Level) : base(Name, BaseTypes, Position, Level)
        {
        }

        public ClassInfo(string Name, string[] BaseTypes, Point Position, int Level, Point AttrPt, string Attributes) : this(Name, BaseTypes, Position, Level)
        {
            base.Attributes = Attributes;
            base.AttributePt = AttrPt;
        }

        public override void Clear()
        {
            base.Clear();
            this.fields.Clear();
            this.enums.Clear();
            this.delegates.Clear();
            this.classes.Clear();
            this.structures.Clear();
            this.interfaces.Clear();
        }

        public override ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            ISyntaxInfo info1 = this.classes.FindByName(Name, CaseSensitive);
            if (info1 == null)
            {
                info1 = this.structures.FindByName(Name, CaseSensitive);
            }
            if (info1 == null)
            {
                info1 = this.interfaces.FindByName(Name, CaseSensitive);
            }
            if (info1 == null)
            {
                info1 = this.delegates.FindByName(Name, CaseSensitive);
            }
            if (info1 == null)
            {
                info1 = this.enums.FindByName(Name, CaseSensitive);
            }
            if (info1 == null)
            {
                info1 = this.fields.FindByName(Name, CaseSensitive);
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
            this.fields = new SyntaxInfos();
            this.enums = new SyntaxInfos();
            this.delegates = new SyntaxInfos();
            this.classes = new SyntaxInfos();
            this.structures = new SyntaxInfos();
            this.interfaces = new SyntaxInfos();
            base.ImageIndex = UnitInfo.ClassImage;
        }


        // Properties
        public ISyntaxInfos Classes
        {
            get
            {
                return this.classes;
            }
        }

        public ISyntaxInfos Delegates
        {
            get
            {
                return this.delegates;
            }
        }

        public ISyntaxInfos Enums
        {
            get
            {
                return this.enums;
            }
        }

        public ISyntaxInfos Fields
        {
            get
            {
                return this.fields;
            }
        }

        public ISyntaxInfos Interfaces
        {
            get
            {
                return this.interfaces;
            }
        }

        public ISyntaxInfos Structures
        {
            get
            {
                return this.structures;
            }
        }


        // Fields
        private ISyntaxInfos classes;
        private ISyntaxInfos delegates;
        private ISyntaxInfos enums;
        private ISyntaxInfos fields;
        private ISyntaxInfos interfaces;
        private ISyntaxInfos structures;
    }
}

