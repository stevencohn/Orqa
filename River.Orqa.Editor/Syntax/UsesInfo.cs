namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;

    public class UsesInfo : RangeInfo, IUsesInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        public UsesInfo()
        {
        }

        public UsesInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.uses.Clear();
            base.Position = new Point(-1, -1);
            base.StartPoint = base.Position;
        }

        public override ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            ISyntaxInfo info1 = this.uses.FindByName(Name, CaseSensitive);
            if (info1 == null)
            {
                info1 = base.FindByName(Name, CaseSensitive);
            }
            return info1;
        }

        public override int GetIndentLevel(int Index, Hashtable Breaks)
        {
            return base.Level;
        }

        protected override void Init()
        {
            base.Init();
            this.uses = new SyntaxInfos();
            base.Position = new Point(-1, -1);
            base.StartPoint = base.Position;
        }


        // Properties
        public ISyntaxInfos Uses
        {
            get
            {
                return this.uses;
            }
        }


        // Fields
        private ISyntaxInfos uses;
    }
}

