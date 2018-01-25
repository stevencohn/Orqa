namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class AccessorInfo : RangeInfo, IAccessorInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        public AccessorInfo()
        {
        }

        public AccessorInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.statements.Clear();
        }

        protected override void Init()
        {
            base.Init();
            this.statements = new SyntaxInfos();
        }


        // Properties
        public ISyntaxInfos Statements
        {
            get
            {
                return this.statements;
            }
        }


        // Fields
        private ISyntaxInfos statements;
    }
}

