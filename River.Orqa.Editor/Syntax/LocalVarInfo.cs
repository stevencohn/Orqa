namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class LocalVarInfo : SyntaxTypeInfo, ILocalVarInfo, ISyntaxTypeInfo, ISyntaxInfo
    {
        // Methods
        public LocalVarInfo()
        {
        }

        public LocalVarInfo(string Name, string DataType, Point Position) : base(Name, DataType, Position)
        {
        }

        protected override void Init()
        {
            base.Init();
            base.ImageIndex = UnitInfo.LocalVarImage;
        }

    }
}

