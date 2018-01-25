namespace River.Orqa.Editor
{
    using System;

    public class HyperTextEventArgs : EventArgs
    {
        // Methods
        public HyperTextEventArgs(string AText, bool AIsHyperText)
        {
            this.IsHyperText = false;
            this.Text = AText;
            this.IsHyperText = AIsHyperText;
        }


        // Fields
        public bool IsHyperText;
        public string Text;
    }
}

