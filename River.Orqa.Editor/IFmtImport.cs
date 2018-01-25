namespace River.Orqa.Editor
{
    using System;
    using System.IO;
    using System.Text;

    public interface IFmtImport : IImport
    {
        // Methods
        void LoadFile(string FileName, ExportFormat Format);

        void LoadFile(string FileName, ExportFormat Format, Encoding Encoding);

        void LoadStream(TextReader Reader, ExportFormat Format);

    }
}

