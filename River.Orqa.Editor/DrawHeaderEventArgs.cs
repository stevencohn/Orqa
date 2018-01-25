namespace River.Orqa.Editor
{
    using System;

    public class DrawHeaderEventArgs : EventArgs
    {
        // Methods
        public DrawHeaderEventArgs(string ATag)
        {
            this.Handled = false;
            this.Tag = ATag;
        }


        // Fields
        public bool Handled;
        public string Tag;
        public string Text;
    }
}

