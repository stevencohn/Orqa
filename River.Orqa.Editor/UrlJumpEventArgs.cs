namespace River.Orqa.Editor
{
    using System;

    public class UrlJumpEventArgs : EventArgs
    {
        // Methods
        public UrlJumpEventArgs(string AText, bool AHandled)
        {
            this.Handled = false;
            this.Text = AText;
            this.Handled = AHandled;
        }


        // Fields
        public bool Handled;
        public string Text;
    }
}

