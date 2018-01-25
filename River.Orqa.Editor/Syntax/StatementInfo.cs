namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class StatementInfo : RangeInfo, IStatementInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        public StatementInfo()
        {
        }

        public StatementInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        protected override void Init()
        {
            base.Init();
            base.Visible = false;
        }

    }
}

