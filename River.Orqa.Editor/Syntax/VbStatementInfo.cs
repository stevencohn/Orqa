namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class VbStatementInfo : StatementInfo, IHasLocalVars
    {
        // Methods
        public VbStatementInfo()
        {
        }

        public VbStatementInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.localVars.Clear();
        }

        public override ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            ISyntaxInfo info1 = this.localVars.FindByName(Name, CaseSensitive);
            if (info1 == null)
            {
                info1 = base.FindByName(Name, CaseSensitive);
            }
            return info1;
        }

        protected override void Init()
        {
            base.Init();
            this.localVars = new SyntaxInfos();
        }


        // Properties
        public ISyntaxInfos LocalVars
        {
            get
            {
                return this.localVars;
            }
        }


        // Fields
        private ISyntaxInfos localVars;
    }
}

