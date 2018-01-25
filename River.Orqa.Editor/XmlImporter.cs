namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Xml.Serialization;

    public class XmlImporter : FmtImporter
    {
        // Methods
        public XmlImporter()
        {
        }

        protected override void ReadContent()
        {
            XmlSerializer serializer1 = new XmlSerializer(typeof(XmlSyntaxEditInfo));
            try
            {
                ((SyntaxEdit) this.owner).XmlInfo = serializer1.Deserialize(this.reader);
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

    }
}

