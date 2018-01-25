namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct HitTestInfo
    {
        public River.Orqa.Editor.HitTest HitTest;
        public StrItem Item;
        public string String;
        public string Word;
        public string Url;
        public int GutterImage;
        public IOutlineRange OutlineRange;
        public int Page;
        public void InitHitTestInfo()
        {
            this.HitTest = River.Orqa.Editor.HitTest.None;
            this.Item = null;
            this.String = null;
            this.Word = null;
            this.Url = null;
            this.GutterImage = -1;
            this.OutlineRange = null;
            this.Page = -1;
        }
    }
}

