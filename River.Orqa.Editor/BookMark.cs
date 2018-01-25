namespace River.Orqa.Editor
{
    using System;

    public class BookMark : IBookMark
    {
        // Methods
        public BookMark()
        {
            this.line = 0;
            this.pos = 0;
            this.index = 0;
        }

        public BookMark(int ALine, int AChar, int AIndex)
        {
            this.line = ALine;
            this.pos = AChar;
            this.index = AIndex;
        }

        public void Assign(IBookMark Source)
        {
            this.Line = Source.Line;
            this.Char = Source.Char;
            this.Index = Source.Index;
        }


        // Properties
        public int Char
        {
            get
            {
                return this.pos;
            }
            set
            {
                this.pos = value;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        public int Line
        {
            get
            {
                return this.line;
            }
            set
            {
                this.line = value;
            }
        }


        // Fields
        private int index;
        private int line;
        private int pos;
    }
}

