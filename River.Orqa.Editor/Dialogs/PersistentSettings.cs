namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor.Common;
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public class PersistentSettings : IPersistentSettings
    {
        // Methods
        public PersistentSettings()
        {
        }

        public virtual void Assign(IPersistentSettings Source)
        {
        }

        public virtual Type GetXmlType()
        {
            return null;
        }

        public void LoadFile(string FileName)
        {
            try
            {
                Stream stream1 = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    StreamReader reader1 = new StreamReader(stream1);
                    try
                    {
                        this.LoadStream(reader1);
                    }
                    finally
                    {
                        reader1.Close();
                    }
                }
                finally
                {
                    stream1.Close();
                }
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void LoadStream(TextReader Reader)
        {
            XmlSerializer serializer1 = new XmlSerializer(this.GetXmlType());
            try
            {
                this.XmlInfo = serializer1.Deserialize(Reader);
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void SaveFile(string FileName)
        {
            try
            {
                Stream stream1 = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    StreamWriter writer1 = new StreamWriter(stream1);
                    try
                    {
                        this.SaveStream(writer1);
                    }
                    finally
                    {
                        writer1.Close();
                    }
                }
                finally
                {
                    stream1.Close();
                }
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void SaveStream(TextWriter Writer)
        {
            XmlSerializer serializer1 = new XmlSerializer(this.GetXmlType());
            try
            {
                serializer1.Serialize(Writer, this.XmlInfo);
            }
            catch (Exception exception1)
            {
                Writer.Flush();
                ErrorHandler.Error(exception1);
            }
        }


        // Properties
        public virtual object XmlInfo
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

    }
}

