namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class VbIfStatementInfo : VbStatementInfo
    {
        // Methods
        public VbIfStatementInfo()
        {
        }

        public VbIfStatementInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.elsePt = new Point(-1, -1);
            this.elseIfPoints.Clear();
        }

        public override int GetIndentLevel(int Index, Hashtable Breaks)
        {
            if (Index == this.elsePt.Y)
            {
                return base.Level;
            }
            foreach (Point point1 in this.elseIfPoints)
            {
                if (Index == point1.Y)
                {
                    return base.Level;
                }
            }
            return base.GetIndentLevel(Index, Breaks);
        }

        protected override void Init()
        {
            base.Init();
            this.elsePt = new Point(-1, -1);
            this.elseIfPoints = new ArrayList();
        }


        // Properties
        public ArrayList ElseIfPoints
        {
            get
            {
                return this.elseIfPoints;
            }
        }

        public Point ElsePt
        {
            get
            {
                return this.elsePt;
            }
            set
            {
                this.elsePt = value;
            }
        }


        // Fields
        private ArrayList elseIfPoints;
        private Point elsePt;
    }
}

