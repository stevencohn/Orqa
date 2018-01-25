namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CustomDrawEventArgs : EventArgs
    {
        // Methods
        public CustomDrawEventArgs()
        {
        }


        // Fields
        public River.Orqa.Editor.DrawInfo DrawInfo;
        public River.Orqa.Editor.DrawStage DrawStage;
        public River.Orqa.Editor.DrawState DrawState;
        public bool Handled;
        public ITextPainter Painter;
        public Rectangle Rect;
    }
}

