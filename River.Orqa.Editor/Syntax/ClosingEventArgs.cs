namespace River.Orqa.Editor.Syntax
{
    using System;

    public class ClosingEventArgs : EventArgs
    {
        // Methods
        public ClosingEventArgs(bool accepted, ICodeCompletionProvider provider)
        {
            this.Accepted = accepted;
            this.Provider = provider;
        }


        // Fields
        public bool Accepted;
        public ICodeCompletionProvider Provider;
        public string Text;
    }
}

