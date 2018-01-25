namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class StructInfo : ClassInfo
    {
        // Methods
        public StructInfo()
        {
        }

        public StructInfo(string Name, string[] BaseTypes, Point Position, int Level) : base(Name, BaseTypes, Position, Level)
        {
        }

        public StructInfo(string Name, string[] BaseTypes, Point Position, int Level, Point AttrPt, string Attributes) : this(Name, BaseTypes, Position, Level)
        {
            base.Attributes = Attributes;
            base.AttributePt = AttrPt;
        }

        protected override void Init()
        {
            base.Init();
            base.ImageIndex = UnitInfo.StructImage;
        }

    }
}

