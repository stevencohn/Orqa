namespace River.Orqa.Editor.Syntax
{
    using System;

    public class TextParsedEventArgs : EventArgs
    {
        // Methods
        public TextParsedEventArgs()
        {
        }


        // Fields
        public short[] ColorData;
        public string String;
    }
}

