namespace River.Orqa.Editor
{
    using System;
    using System.IO;
    using System.Text;

    public interface IExport
    {
        // Methods
        void SaveFile(string FileName);

        void SaveFile(string FileName, Encoding Encoding);

        void SaveStream(TextWriter Writer);

    }
}

