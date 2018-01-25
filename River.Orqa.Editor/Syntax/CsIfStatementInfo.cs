namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class CsIfStatementInfo : CsStatementInfo
    {
        // Methods
        public CsIfStatementInfo()
        {
        }

        public CsIfStatementInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.ifEndPoint = new Point(-1, -1);
            this.elseIfPoint = new Point(-1, -1);
        }

        public override int GetIndentLevel(int Index, Hashtable Breaks)
        {
            if (base.HasBlock && (Index == this.ifEndPoint.Y))
            {
                return base.Level;
            }
            if (Index == this.elseIfPoint.Y)
            {
                return (base.Level + 1);
            }
            return base.GetIndentLevel(Index, Breaks);
        }

        protected override void Init()
        {
            base.Init();
            this.elseIfPoint = new Point(-1, -1);
            this.ifEndPoint = new Point(-1, -1);
        }


        // Properties
        public Point ElseIfPoint
        {
            get
            {
                return this.elseIfPoint;
            }
            set
            {
                this.elseIfPoint = value;
            }
        }

        public Point IfEndPoint
        {
            get
            {
                return this.ifEndPoint;
            }
            set
            {
                this.ifEndPoint = value;
            }
        }


        // Fields
        private Point elseIfPoint;
        private Point ifEndPoint;
    }
}

