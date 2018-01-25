namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;

    public class RangeInfo : SyntaxInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        public RangeInfo()
        {
        }

        public RangeInfo(string Name, Point Position, int Level) : base(Name, Position)
        {
            this.level = Level;
            this.startPoint = Position;
            this.endPoint = Position;
        }

        public override void Clear()
        {
            base.Clear();
            this.regions.Clear();
            this.comments.Clear();
        }

        public int GetIndentLevel(int Index)
        {
            return this.GetIndentLevel(Index, null);
        }

        public virtual int GetIndentLevel(int Index, Hashtable Breaks)
        {
            if ((Index <= base.Position.Y) || (Index > this.endPoint.Y))
            {
                return this.level;
            }
            if (Index < (base.Position.Y + base.DeclarationSize.Height))
            {
                return (this.level + 1);
            }
            if (this.hasBlock)
            {
                if ((Index == (base.Position.Y + base.DeclarationSize.Height)) || (Index == this.endPoint.Y))
                {
                    return this.level;
                }
                if ((Breaks != null) && Breaks.Contains(Index))
                {
                    return (this.level + 2);
                }
            }
            return (this.level + 1);
        }

        protected override void Init()
        {
            base.Init();
            this.visible = true;
            this.regions = new SyntaxInfos();
            this.comments = new SyntaxInfos();
        }


        // Properties
        public ISyntaxInfos Comments
        {
            get
            {
                return this.comments;
            }
        }

        public virtual string DisplayText
        {
            get
            {
                return "...";
            }
        }

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

        public bool HasBlock
        {
            get
            {
                return this.hasBlock;
            }
            set
            {
                this.hasBlock = value;
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

        public ISyntaxInfos Regions
        {
            get
            {
                return this.regions;
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

        public string Text
        {
            get
            {
                return this.DisplayText;
            }
        }

        public bool Visible
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
        private ISyntaxInfos comments;
        private Point endPoint;
        private bool hasBlock;
        private int level;
        private ISyntaxInfos regions;
        private Point startPoint;
        private bool visible;
    }
}

