namespace River.Orqa.Editor.Dialogs
{
    using System;
    using System.IO;

    public interface IPersistentSettings
    {
        // Methods
        void Assign(IPersistentSettings Source);

        void LoadFile(string FileName);

        void LoadStream(TextReader Reader);

        void SaveFile(string FileName);

        void SaveStream(TextWriter Writer);

    }
}

