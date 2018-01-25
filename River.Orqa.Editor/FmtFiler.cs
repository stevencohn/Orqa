namespace River.Orqa.Editor
{
    using System;
    using System.IO;

    public class FmtFiler
    {
        // Methods
        public FmtFiler(ISyntaxEdit Owner, ExportFormat Format)
        {
            this.format = ExportFormat.Text;
            this.owner = Owner;
            this.format = Format;
        }

        public void Read(TextReader Reader)
        {
            FmtImporter importer1 = null;
            switch (this.format)
            {
                case ExportFormat.Text:
                {
                    this.owner.Lines.LoadStream(Reader);
                    break;
                }
                case ExportFormat.Rtf:
                {
                    importer1 = new RtfImporter();
                    break;
                }
                case ExportFormat.Html:
                {
                    importer1 = new HtmlImporter();
                    break;
                }
                case ExportFormat.Xml:
                {
                    importer1 = new XmlImporter();
                    break;
                }
            }
            if (importer1 != null)
            {
                importer1.Read(this.owner, Reader);
            }
        }

        public void Write(TextWriter Writer)
        {
            FmtExporter exporter1 = null;
            switch (this.format)
            {
                case ExportFormat.Text:
                {
                    foreach (StrItem item1 in this.owner.Lines)
                    {
                        Writer.WriteLine(item1.String);
                    }
                    goto Label_0086;
                }
                case ExportFormat.Rtf:
                {
                    break;
                }
                case ExportFormat.Html:
                {
                    exporter1 = new HtmlExporter();
                    goto Label_0086;
                }
                case ExportFormat.Xml:
                {
                    exporter1 = new XmlExporter();
                    goto Label_0086;
                }
                default:
                {
                    goto Label_0086;
                }
            }
            exporter1 = new RtfExporter();
        Label_0086:
            if (exporter1 != null)
            {
                exporter1.Write(this.owner, Writer);
            }
        }


        // Fields
        private ExportFormat format;
        private ISyntaxEdit owner;
    }
}

