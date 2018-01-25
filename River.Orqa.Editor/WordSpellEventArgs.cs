namespace River.Orqa.Editor
{
    using System;

    public class WordSpellEventArgs : EventArgs
    {
        // Methods
        public WordSpellEventArgs(string AText, bool ACorrect, int AColorStyle)
        {
            this.Correct = true;
            this.Text = AText;
            this.Correct = ACorrect;
            this.ColorStyle = AColorStyle;
        }


        // Fields
        public int ColorStyle;
        public bool Correct;
        public string Text;
    }
}

