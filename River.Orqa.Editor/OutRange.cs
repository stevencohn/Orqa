namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;

    internal class OutRange : OutlineRange
    {
        // Methods
        public OutRange(OutlineList Owner, Point AStartPt, Point AEndPt, int ALevel, string AText) : base(AStartPt, AEndPt, ALevel, AText)
        {
            this.owner = Owner;
        }

        public OutRange(OutlineList Owner, Point AStartPt, Point AEndPt, int ALevel, string AText, bool AVisible) : base(AStartPt, AEndPt, ALevel, AText, AVisible)
        {
            this.owner = Owner;
        }


        // Properties
        public override string DisplayText
        {
            get
            {
                if ((this.owner != null) && ((this.owner.Owner.OutlineOptions & OutlineOptions.DrawButtons) == OutlineOptions.None))
                {
                    return string.Empty;
                }
                return base.Text;
            }
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;
                    if (this.owner != null)
                    {
                        this.owner.UpdateRange(this, true);
                    }
                }
            }
        }


        // Fields
        private OutlineList owner;
    }
}

