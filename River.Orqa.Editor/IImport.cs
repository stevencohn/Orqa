namespace River.Orqa.Editor
{
    using System;
    using System.IO;
    using System.Text;

    public interface IImport
    {
        // Methods
        void LoadFile(string FileName);

        void LoadFile(string FileName, Encoding Encoding);

        void LoadStream(TextReader Reader);

    }
}

