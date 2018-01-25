namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    internal class CollapsedRange : OutRange
    {
        // Methods
        public CollapsedRange(OutlineList Owner, Point AStartPt, Point AEndPt, int ADeltaX, int ADeltaY, int ALevel, string AText) : base(Owner, AStartPt, AEndPt, ALevel, AText)
        {
            this.DeltaX = ADeltaX;
            this.DeltaY = ADeltaY;
        }


        // Fields
        public int DeltaX;
        public int DeltaY;
    }
}

