namespace River.Orqa.Editor
{
    using System;
    using System.IO;
    using System.Text;

    public interface IFmtExport : IExport
    {
        // Methods
        void SaveFile(string FileName, ExportFormat Format);

        void SaveFile(string FileName, ExportFormat Format, Encoding Encoding);

        void SaveStream(TextWriter Writer, ExportFormat Format);

    }
}

