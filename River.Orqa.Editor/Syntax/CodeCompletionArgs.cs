namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class CodeCompletionArgs : EventArgs
    {
        // Methods
        public CodeCompletionArgs()
        {
            this.SelIndex = -1;
        }

        public void Init()
        {
            this.CompletionType = CodeCompletionType.None;
            this.Provider = null;
            this.KeyChar = '\0';
            this.ToolTip = false;
            this.Interval = 0;
            this.StartPosition = new Point(-1, -1);
            this.EndPosition = new Point(-1, -1);
            this.Handled = false;
            this.NeedShow = false;
            this.SelIndex = -1;
        }

        public void Init(CodeCompletionType CompletionType, Point Position)
        {
            this.Init();
            this.CompletionType = CompletionType;
            this.StartPosition = Position;
        }


        // Fields
        public CodeCompletionType CompletionType;
        public Point EndPosition;
        public bool Handled;
        public int Interval;
        public char KeyChar;
        public bool NeedShow;
        public ICodeCompletionProvider Provider;
        public int SelIndex;
        public Point StartPosition;
        public bool ToolTip;
    }
}

