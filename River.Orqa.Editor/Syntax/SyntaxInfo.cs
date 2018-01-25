namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class SyntaxInfo : ISyntaxInfo
    {
        // Methods
        public SyntaxInfo()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.imageIndex = -1;
            this.Init();
        }

        public SyntaxInfo(string Name, Point Position)
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.imageIndex = -1;
            this.name = Name;
            this.position = Position;
            this.declarationSize = new Size(Name.Length, 0);
            this.Init();
        }

        public virtual void Clear()
        {
        }

        public virtual ISyntaxInfo FindByName(string Name, bool CaseSensitive)
        {
            return null;
        }

        protected virtual void Init()
        {
            this.description = string.Empty;
            this.imageIndex = -1;
        }


        // Properties
        public Size DeclarationSize
        {
            get
            {
                return this.declarationSize;
            }
            set
            {
                this.declarationSize = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                this.imageIndex = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public Point Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }


        // Fields
        private Size declarationSize;
        private string description;
        private int imageIndex;
        private string name;
        private Point position;
    }
}

