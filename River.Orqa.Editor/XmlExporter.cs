namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Xml.Serialization;

    public class XmlExporter : FmtExporter
    {
        // Methods
        public XmlExporter()
        {
        }

        protected override void WriteContent()
        {
            this.WriteProps();
        }

        private void WriteProps()
        {
            XmlSerializer serializer1 = new XmlSerializer(typeof(XmlSyntaxEditInfo));
            try
            {
                serializer1.Serialize(this.writer, ((SyntaxEdit) this.owner).XmlInfo);
            }
            catch (Exception exception1)
            {
                this.writer.Flush();
                ErrorHandler.Error(exception1);
            }
        }

    }
}

