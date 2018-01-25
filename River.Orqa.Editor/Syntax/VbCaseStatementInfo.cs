namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class VbCaseStatementInfo : VbStatementInfo
    {
        // Methods
        public VbCaseStatementInfo()
        {
        }

        public VbCaseStatementInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.casePoints.Clear();
        }

        public override int GetIndentLevel(int Index, Hashtable Breaks)
        {
            foreach (Point point1 in this.casePoints)
            {
                if (Index == point1.Y)
                {
                    return (base.Level + 1);
                }
            }
            if (this.casePoints.Count > 0)
            {
                Point point2 = (Point) this.casePoints[0];
                if ((Index > point2.Y) && (Index < base.EndPoint.Y))
                {
                    return (base.Level + 2);
                }
            }
            return base.GetIndentLevel(Index, Breaks);
        }

        protected override void Init()
        {
            base.Init();
            this.casePoints = new ArrayList();
        }


        // Properties
        public ArrayList CasePoints
        {
            get
            {
                return this.casePoints;
            }
        }


        // Fields
        private ArrayList casePoints;
    }
}

