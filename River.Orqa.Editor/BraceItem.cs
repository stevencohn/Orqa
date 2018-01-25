namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct BraceItem
    {
        public System.Drawing.Point Point;
        public bool Open;
        public int BraceIndex;
        public BraceItem(int X, int Y, int ABraceIndex, bool AOpen)
        {
            this.Point = new System.Drawing.Point(X, Y);
            this.BraceIndex = ABraceIndex;
            this.Open = AOpen;
        }
    }
}

