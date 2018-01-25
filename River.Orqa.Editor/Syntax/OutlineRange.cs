namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class OutlineRange : Range, IOutlineRange, IRange
    {
        // Methods
        public OutlineRange(Point AStartPt, Point AEndPt, int ALevel, string AText) : base(AStartPt, AEndPt)
        {
            this.text = AText;
            this.visible = true;
            this.level = ALevel;
        }

        public OutlineRange(Point AStartPt, Point AEndPt, int ALevel, string AText, bool AVisible) : base(AStartPt, AEndPt)
        {
            this.text = AText;
            this.visible = AVisible;
            this.level = ALevel;
        }


        // Properties
        public virtual string DisplayText
        {
            get
            {
                return this.text;
            }
        }

        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
            }
        }


        // Fields
        private int level;
        private string text;
        private bool visible;
    }
}

