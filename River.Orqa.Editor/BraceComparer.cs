namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.Drawing;

    internal class BraceComparer : IComparer
    {
        // Methods
        public BraceComparer()
        {
        }

        public int Compare(object x, object y)
        {
            BraceItem item1 = (BraceItem) x;
            Point point1 = item1.Point;
            Point point2 = (Point) y;
            if ((point2.Y <= point1.Y) && ((point2.Y != point1.Y) || (point2.X <= point1.X)))
            {
                return 1;
            }
            return 0;
        }

    }
}

