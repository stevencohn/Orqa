namespace River.Orqa.Editor
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DrawInfo
    {
        public string Text;
        public bool Selection;
        public short Style;
        public ColorFlags Flags;
        public int Char;
        public int Line;
        public int GutterImage;
        public int Page;
        public void Init()
        {
            this.Text = string.Empty;
            this.Selection = false;
            this.Style = -1;
            this.Flags = ColorFlags.None;
            this.Char = -1;
            this.Line = -1;
            this.Page = -1;
            this.GutterImage = -1;
        }
    }
}

