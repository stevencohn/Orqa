namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CsNamespaceInfo : NamespaceInfo, ICsNamespaceInfo, INamespaceInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public CsNamespaceInfo()
        {
        }

        public CsNamespaceInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.uses.Clear();
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

        protected override void Init()
        {
            base.Init();
            this.uses = new UsesInfo();
        }


        // Properties
        public IUsesInfo Uses
        {
            get
            {
                return this.uses;
            }
        }


        // Fields
        private IUsesInfo uses;
    }
}

