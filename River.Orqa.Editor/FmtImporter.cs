namespace River.Orqa.Editor
{
    using System;
    using System.IO;

    public class FmtImporter
    {
        // Methods
        public FmtImporter()
        {
        }

        public virtual void Read(ISyntaxEdit Owner, TextReader Reader)
        {
            this.owner = Owner;
            this.reader = Reader;
            this.ReadHeader();
            this.ReadContent();
        }

        protected virtual void ReadContent()
        {
        }

        protected virtual void ReadHeader()
        {
        }


        // Fields
        protected ISyntaxEdit owner;
        protected TextReader reader;
    }
}

