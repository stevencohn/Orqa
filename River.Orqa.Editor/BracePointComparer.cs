namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.Drawing;

    internal class BracePointComparer : IComparer
    {
        // Methods
        public BracePointComparer()
        {
        }

        public int Compare(object x, object y)
        {
            BraceItem item1 = (BraceItem) x;
            Point point1 = item1.Point;
            Point point2 = (Point) y;
            int num1 = point1.Y - point2.Y;
            if (num1 == 0)
            {
                num1 = point1.X - point2.X;
            }
            return num1;
        }

    }
}

