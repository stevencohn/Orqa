namespace River.Orqa.Editor.Common
{
    using System;
    using System.Drawing;

    public class Range : IRange
    {
        // Methods
        public Range(Point AStartPt, Point AEndPt)
        {
            this.startPoint = AStartPt;
            this.endPoint = AEndPt;
        }

        public Range(int X1, int Y1, int X2, int Y2)
        {
            this.startPoint = new Point(X1, Y1);
            this.endPoint = new Point(X2, Y2);
        }


        // Properties
        public Point EndPoint
        {
            get
            {
                return this.endPoint;
            }
            set
            {
                this.endPoint = value;
            }
        }

        public Point StartPoint
        {
            get
            {
                return this.startPoint;
            }
            set
            {
                this.startPoint = value;
            }
        }


        // Fields
        private Point endPoint;
        private Point startPoint;
    }
}

