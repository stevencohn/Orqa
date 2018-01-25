namespace River.Orqa.Editor.Common
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TextPainter : ITextPainter
    {
        // Methods
        public TextPainter(Control Control)
        {
            this.oldMode = 1;
            this.bufferSize = 0;
            this.control = Control;
            this.fontTable = new Hashtable();
            this.brushTable = new Hashtable();
            this.penTable = new Hashtable();
            IntPtr ptr1 = Win32.GetDC(IntPtr.Zero);
            System.Drawing.Font font1 = new System.Drawing.Font(FontFamily.GenericMonospace, 10f);
            try
            {
                this.measureDC = Win32.CreateCompatibleDC(ptr1);
                this.oldMeasureFont = Win32.SelectObject(this.measureDC, font1.ToHfont());
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, ptr1);
            }
            this.Font = font1;
            this.Color = Consts.DefaultControlForeColor;
            this.BkColor = Consts.DefaultControlBackColor;
            this.PenColor = Consts.DefaultControlForeColor;
            this.bkMode = 2;
        }

        public void BeginPaint(Graphics Graphics)
        {
            this.hdc = Graphics.GetHdc();
            this.oldFont = Win32.SelectObject(this.hdc, this.CurrentInfo.HFont);
            this.oldColor = Win32.SetTextColor(this.hdc, Win32.ColorToGdiColor(this.color));
            this.oldBkColor = Win32.SetBkColor(this.hdc, Win32.ColorToGdiColor(this.bkColor));
            this.oldBkMode = Win32.SetBkMode(this.hdc, this.bkMode);
            this.oldMode = Win32.GetGraphicsMode(this.hdc);
            this.InitBrush(true);
            this.InitPen(false);
            this.oldPen = Win32.SelectObject(this.hdc, this.pen);
        }

        public int CharWidth(char Char, int Count)
        {
            return this.CharWidth(this.CurrentInfo, Char, Count);
        }

        private int CharWidth(FontInfo Info, char Char, int Count)
        {
            if (Count == 0x7fffffff)
            {
                return 0x7fffffff;
            }
            if (Count <= 0)
            {
                return 0;
            }
            if (this.IsMonoSpaced)
            {
                return (Count * Info.FontMetrics.AveCharWidth);
            }
            return (Count * Info.CharWidth(Char));
        }

        public int CharWidth(char Char, int Width, out int Count)
        {
            return this.CharWidth(this.CurrentInfo, Char, Width, out Count);
        }

        private int CharWidth(FontInfo Info, char Char, int Width, out int Count)
        {
            if (Width == 0x7fffffff)
            {
                Count = 0x7fffffff;
                return 0x7fffffff;
            }
            int num1 = 0;
            Count = 0;
            if (Width > 0)
            {
                if (this.IsMonoSpaced)
                {
                    num1 = Info.FontMetrics.AveCharWidth;
                }
                else
                {
                    num1 = Info.CharWidth(Char);
                }
                if (num1 > 0)
                {
                    Count = Width / num1;
                    return (Count * num1);
                }
            }
            return 0;
        }

        public void Clear()
        {
            this.fontTable.Clear();
            this.brushTable.Clear();
            this.penTable.Clear();
            this.FreeBuffer();
            this.font = null;
            this.fontInfos = null;
            this.brush = IntPtr.Zero;
        }

        private void ClearBrushes()
        {
            foreach (DictionaryEntry entry1 in this.brushTable)
            {
                Win32.DeleteObject((IntPtr) entry1.Value);
            }
            this.brushTable.Clear();
        }

        private void ClearPens()
        {
            foreach (DictionaryEntry entry1 in this.penTable)
            {
                Win32.DeleteObject((IntPtr) entry1.Value);
            }
            this.penTable.Clear();
        }

        internal int DashStyleToPenStyle(DashStyle PenStyle)
        {
            switch (PenStyle)
            {
                case DashStyle.Dash:
                {
                    return 1;
                }
                case DashStyle.Dot:
                {
                    return 2;
                }
                case DashStyle.DashDot:
                {
                    return 3;
                }
                case DashStyle.DashDotDot:
                {
                    return 4;
                }
            }
            return 0;
        }

        public void DrawDotLine(int X1, int Y1, int X2, int Y2, System.Drawing.Color Color)
        {
            int num1 = 0;
            if (X1 == X2)
            {
                while (num1 < (Y2 - Y1))
                {
                    Win32.SetPixel(this.hdc, X1, num1, Color);
                    num1 += 2;
                }
            }
            else if (Y1 == Y2)
            {
                while (num1 < (X2 - X1))
                {
                    Win32.SetPixel(this.hdc, num1, Y1, Color);
                    num1 += 2;
                }
            }
        }

        public void DrawImage(ImageList Images, int ImageIndex, Rectangle R)
        {
            uint num1 = uint.MaxValue;
            Win32.ImageList_DrawEx(Images.Handle, ImageIndex, this.hdc, R.Left, R.Top, R.Width, R.Height, (int) num1, (int) num1, 1);
        }

        public void DrawLine(int X1, int Y1, int X2, int Y2)
        {
            Win32.MoveToEx(this.hdc, X1, Y1, IntPtr.Zero);
            Win32.LineTo(this.hdc, X2, Y2);
        }

        public void DrawLine(int X1, int Y1, int X2, int Y2, System.Drawing.Color Color, int Width, DashStyle PenStyle)
        {
            IntPtr ptr1 = Win32.CreatePen(this.DashStyleToPenStyle(PenStyle), Width, Win32.ColorToGdiColor(Color));
            IntPtr ptr2 = Win32.SelectObject(this.hdc, ptr1);
            this.DrawLine(X1, Y1, X2, Y2);
            Win32.SelectObject(this.hdc, ptr2);
            Win32.DeleteObject(ptr1);
        }

        public void DrawLiveSpell(Rectangle R, System.Drawing.Color Color)
        {
            int num1 = R.Left - (R.Left % 6);
            int num2 = R.Right % 6;
            int num3 = (num2 != 0) ? (R.Right + (6 - num2)) : R.Right;
            int num4 = (num3 - num1) >> 1;
            if (num4 < 4)
            {
                num4 = 4;
            }
            else
            {
                num2 = (num4 - 4) / 3;
                if (((num4 - 4) % 3) != 0)
                {
                    num2++;
                }
                num4 = 4 + (num2 * 3);
            }
            Point[] pointArray1 = new Point[num4];
            for (int num5 = 0; num5 < num4; num5++)
            {
                pointArray1[num5].X = num1 + (num5 * 2);
                pointArray1[num5].Y = (R.Bottom - 1) + this.SignBezier(num5);
            }
            IntPtr ptr1 = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(Color));
            IntPtr ptr2 = Win32.SelectObject(this.hdc, ptr1);
            Win32.PolyBezier(this.hdc, pointArray1, num4);
            Win32.SelectObject(this.hdc, ptr2);
            Win32.DeleteObject(ptr1);
        }

        public void DrawRectangle(Rectangle R)
        {
            this.DrawRectangle(R.Left, R.Top, R.Width, R.Height);
        }

        public void DrawRectangle(int X, int Y, int Width, int Height)
        {
            Win32.FrameRect(this.hdc, X, Y, Width, Height, this.brush);
        }

        public void DrawRoundRectangle(int Left, int Top, int Right, int Bottom, int Width, int Height)
        {
            Win32.RoundRect(this.hdc, Left, Top, Right, Bottom, Width, Height);
        }

        public void DrawText(string String, int Len, Rectangle R)
        {
            this.DrawText(String, Len, R, 0);
        }

        public void DrawText(string String, int Len, Rectangle R, int Flags)
        {
            if (Len == -1)
            {
                Len = String.Length;
            }
            Win32.DrawText(this.hdc, String, Len, ref R, Flags);
        }

        public void EndPaint(Graphics Graphics)
        {
            Win32.SelectObject(this.hdc, this.oldFont);
            Win32.SetTextColor(this.hdc, this.oldColor);
            Win32.SetBkColor(this.hdc, this.oldBkColor);
            Win32.SetBkMode(this.hdc, this.oldBkMode);
            Win32.SelectObject(this.hdc, this.oldPen);
            this.EndTransform();
            Graphics.ReleaseHdc(this.hdc);
            this.hdc = IntPtr.Zero;
        }

        public void EndTransform()
        {
            if (this.transFormed)
            {
                Win32.XFORM xform1 = new Win32.XFORM(1f, 0f, 0f, 1f, 0f, 0f);
                Win32.SetWorldTransform(this.hdc, ref xform1);
                this.transFormed = false;
            }
        }

        public void ExcludeClipRect(int X, int Y, int Width, int Height)
        {
            Win32.ExcludeClipRect(this.hdc, X, Y, X + Width, Y + Height);
        }

        public void FillRectangle(Rectangle R)
        {
            this.FillRectangle(R.Left, R.Top, R.Width, R.Height);
        }

        public void FillRectangle(int X, int Y, int Width, int Height)
        {
            Win32.FillRect(this.hdc, X, Y, Width, Height, this.brush);
        }

        ~TextPainter()
        {
            Win32.SelectObject(this.measureDC, this.oldMeasureFont);
            this.fontTable.Clear();
            this.ClearBrushes();
            this.ClearPens();
            Win32.DeleteDC(this.hdc);
        }

        private void FreeBuffer()
        {
            this.buffer = null;
            this.bufferSize = 0;
        }

        protected internal int[] GetBuffer(int Len)
        {
            return this.GetBuffer(Len, -1);
        }

        protected internal int[] GetBuffer(int Len, int Space)
        {
            if (!this.IsMonoSpaced && (Space < 0))
            {
                return null;
            }
            if (this.bufferSize < Len)
            {
                this.FreeBuffer();
                this.bufferSize = Len;
                this.buffer = new int[this.bufferSize];
                for (int num1 = 0; num1 < this.bufferSize; num1++)
                {
                    this.buffer[num1] = (Space >= 0) ? Space : this.FontWidth;
                }
            }
            return this.buffer;
        }

        private object GetFontKey(System.Drawing.Font Font)
        {
            return (Font.Name + "|" + Font.Size.ToString());
        }

        private void InitBrush(bool Select)
        {
            object obj1 = this.brushTable[this.bkColor];
            if (obj1 == null)
            {
                this.brush = Win32.CreateSolidBrush(Win32.ColorToGdiColor(this.bkColor));
                this.brushTable.Add(this.BkColor, this.brush);
            }
            else
            {
                this.brush = (IntPtr) obj1;
            }
            if (Select && (this.hdc != IntPtr.Zero))
            {
                Win32.SetBkColor(this.hdc, Win32.ColorToGdiColor(this.bkColor));
            }
        }

        private void InitDC(IntPtr hFont)
        {
            if (this.hdc != IntPtr.Zero)
            {
                Win32.SelectObject(this.hdc, hFont);
            }
        }

        private void InitFont(System.Drawing.Font Font)
        {
            object obj1 = this.GetFontKey(Font);
            this.fontInfos = (FontInfos) this.fontTable[obj1];
            if (this.fontInfos == null)
            {
                this.fontInfos = new FontInfos(this, this.measureDC, Font);
                this.fontTable.Add(obj1, this.fontInfos);
            }
            this.fontInfos.InitStyle(Font.Style);
            this.fontWidth = this.CurrentInfo.FontMetrics.AveCharWidth;
            this.InitDC(this.CurrentInfo.HFont);
        }

        private void InitFont(System.Drawing.FontStyle FontStyle)
        {
            this.fontInfos.InitStyle(FontStyle);
            this.InitDC(this.CurrentInfo.HFont);
        }

        private void InitPen(bool Select)
        {
            object obj1 = this.penTable[this.penColor];
            if (obj1 == null)
            {
                this.pen = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(this.penColor));
                this.penTable.Add(this.penColor, this.pen);
            }
            else
            {
                this.pen = (IntPtr) obj1;
            }
            if (Select && (this.hdc != IntPtr.Zero))
            {
                Win32.SelectObject(this.hdc, this.pen);
            }
        }

        protected internal void InitTextColor()
        {
            if (this.hdc != IntPtr.Zero)
            {
                Win32.SetTextColor(this.hdc, Win32.ColorToGdiColor(this.color));
            }
        }

        public void IntersectClipRect(int X, int Y, int Width, int Height)
        {
            Win32.IntersectClipRect(this.hdc, X, Y, X + Width, Y + Height);
        }

        public void Polygon(Point[] points, System.Drawing.Color Color)
        {
            this.BkColor = Color;
            IntPtr ptr1 = Win32.SelectObject(this.hdc, this.brush);
            IntPtr ptr2 = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(Color));
            IntPtr ptr3 = Win32.SelectObject(this.hdc, ptr2);
            Win32.Polygon(this.hdc, points, points.Length);
            Win32.SelectObject(this.hdc, ptr1);
            Win32.SelectObject(this.hdc, ptr3);
        }

        public void RestoreClip(IntPtr Rgn)
        {
            Win32.SelectClipRgn(this.hdc, Rgn);
            Win32.DeleteObject(Rgn);
        }

        public IntPtr SaveClip(Rectangle Rect)
        {
            IntPtr ptr1 = Win32.CreateRectRgnIndirect(Rect);
            Win32.GetClipRgn(this.hdc, ptr1);
            return ptr1;
        }

        private int SignBezier(int i)
        {
            switch ((i % 3))
            {
                case 0:
                {
                    return -2;
                }
                case 1:
                {
                    return 0;
                }
            }
            return 2;
        }

        public int StringWidth(string String)
        {
            return this.StringWidth(String, 0, -1);
        }

        public int StringWidth(string String, int Pos, int Len)
        {
            return this.StringWidth(this.CurrentInfo, String, Pos, Len);
        }

        private int StringWidth(FontInfo Info, string String, int Pos, int Len)
        {
            if (Len == 0x7fffffff)
            {
                return 0x7fffffff;
            }
            if (Len == -1)
            {
                Len = String.Length;
            }
            if (this.IsMonoSpaced)
            {
                return (Len * Info.FontMetrics.AveCharWidth);
            }
            int num1 = 0;
            for (int num2 = Pos; num2 < (Pos + Len); num2++)
            {
                num1 += Info.CharWidth(String[num2]);
            }
            return num1;
        }

        public int StringWidth(string String, int Width, out int Count, bool Exact)
        {
            return this.StringWidth(String, 0, -1, Width, out Count, Exact);
        }

        public int StringWidth(string String, int Pos, int Len, int Width, out int Count)
        {
            return this.StringWidth(String, Pos, Len, Width, out Count, true);
        }

        public int StringWidth(string String, int Pos, int Len, int Width, out int Count, bool Exact)
        {
            return this.StringWidth(this.CurrentInfo, String, Pos, Len, Width, out Count, Exact);
        }

        private int StringWidth(FontInfo Info, string String, int Pos, int Len, int Width, out int Count, bool Exact)
        {
            if (Len == -1)
            {
                Len = String.Length - Pos;
            }
            if (Width == 0x7fffffff)
            {
                Count = Len;
                return 0x7fffffff;
            }
            Count = 0;
            if (this.IsMonoSpaced)
            {
                int num1 = Info.FontMetrics.AveCharWidth;
                if (num1 == 0)
                {
                    Count = Len;
                }
                else
                {
                    if (Exact)
                    {
                        Count = Width / num1;
                    }
                    else
                    {
                        Count = (int) Math.Round((double) (Width / num1));
                    }
                    Count = Math.Min(Count, Len);
                }
                return (Count * num1);
            }
            for (int num3 = Pos; num3 < (Pos + Len); num3++)
            {
                int num2 = Info.CharWidth(String[num3]);
                Width -= num2;
                if (Width < 0)
                {
                    if (!Exact && (Width > (-num2 / 2)))
                    {
                        Count += 1;
                    }
                    break;
                }
                Count += 1;
            }
            return this.StringWidth(Info, String, Pos, Count);
        }

        public void TextOut(string String, int Len, Rectangle R)
        {
            this.TextOut(String, Len, R, 0);
        }

        public void TextOut(string String, int Len, Rectangle R, int Flags)
        {
            if (Len == -1)
            {
                Len = String.Length;
            }
            Win32.ExtTextOut(this.hdc, R.Left, R.Top, R.Width, R.Height, Flags, String, Len);
        }

        public void TextOut(string String, int Len, int X, int Y)
        {
            this.TextOut(String, Len, new Rectangle(X, Y, 0, 0), 0);
        }

        public void TextOut(string String, int Len, Rectangle R, int Flags, int Space)
        {
            if (Len == -1)
            {
                Len = String.Length;
            }
            Win32.ExtTextOut(this.hdc, R.Left, R.Top, R.Width, R.Height, Flags, String, Len, this.GetBuffer(Len, Space));
        }

        public void TextOut(string String, int Len, int X, int Y, int Flags)
        {
            this.TextOut(String, Len, new Rectangle(X, Y, 0, 0), Flags);
        }

        public void TextOut(string String, int Len, Rectangle R, int X, int Y, int Flags)
        {
            if (Len == -1)
            {
                Len = String.Length;
            }
            Win32.ExtTextOut(this.hdc, R, X, Y, Flags, String, Len);
        }

        public void Transform(int X, int Y, float ScaleX, float ScaleY)
        {
            this.transFormed = true;
            this.oldMode = Win32.SetGraphicsMode(this.hdc, 2);
            Win32.XFORM xform1 = new Win32.XFORM(ScaleX, 0f, 0f, ScaleY, X * ScaleX, Y * ScaleY);
            Win32.SetWorldTransform(this.hdc, ref xform1);
        }


        // Properties
        public System.Drawing.Color BkColor
        {
            get
            {
                return this.bkColor;
            }
            set
            {
                if (this.bkColor != value)
                {
                    this.bkColor = value;
                    this.InitBrush(true);
                }
            }
        }

        public int BkMode
        {
            get
            {
                return this.bkMode;
            }
            set
            {
                if (this.bkMode != value)
                {
                    this.bkMode = value;
                    if (this.hdc != IntPtr.Zero)
                    {
                        Win32.SetBkMode(this.hdc, this.bkMode);
                    }
                }
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                if (this.color != value)
                {
                    this.color = value;
                    this.InitTextColor();
                }
            }
        }

        private FontInfo CurrentInfo
        {
            get
            {
                if (this.fontInfos == null)
                {
                    return null;
                }
                return this.fontInfos.CurrentInfo();
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                if (this.font != value)
                {
                    this.font = value;
                    if (value != null)
                    {
                        this.fontStyle = value.Style;
                        this.InitFont(this.font);
                    }
                }
            }
        }

        public int FontHeight
        {
            get
            {
                return this.fontInfos.FontHeight;
            }
        }

        public Win32.TextMetrics FontMetrics
        {
            get
            {
                return this.CurrentInfo.FontMetrics;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                if (this.fontStyle != value)
                {
                    this.fontStyle = value;
                    this.InitFont(this.fontStyle);
                }
            }
        }

        public int FontWidth
        {
            get
            {
                return this.fontWidth;
            }
        }

        public bool IsMonoSpaced
        {
            get
            {
                return this.fontInfos.IsMonoSpaced;
            }
        }

        public System.Drawing.Color PenColor
        {
            get
            {
                return this.penColor;
            }
            set
            {
                if (this.penColor != value)
                {
                    this.penColor = value;
                    this.InitPen(true);
                }
            }
        }


        // Fields
        private System.Drawing.Color bkColor;
        private int bkMode;
        private IntPtr brush;
        private Hashtable brushTable;
        private int[] buffer;
        private int bufferSize;
        private System.Drawing.Color color;
        private Control control;
        private System.Drawing.Font font;
        private FontInfos fontInfos;
        private System.Drawing.FontStyle fontStyle;
        private Hashtable fontTable;
        private int fontWidth;
        private IntPtr hdc;
        private IntPtr measureDC;
        private int oldBkColor;
        private int oldBkMode;
        private int oldColor;
        private IntPtr oldFont;
        private IntPtr oldMeasureFont;
        private int oldMode;
        private IntPtr oldPen;
        private IntPtr pen;
        private System.Drawing.Color penColor;
        private Hashtable penTable;
        private bool transFormed;

        // Nested Types
        private class FontInfo
        {
            // Methods
            public FontInfo(IntPtr DC, Font Font)
            {
                this.hFont = Font.ToHfont();
                this.dc = DC;
                Win32.SelectObject(this.dc, this.hFont);
                Win32.GetTextMetrics(this.dc, ref this.fontMetrics);
                this.isMonoSpaced = (this.fontMetrics.PitchAndFamily & 1) == 0;
                if (!this.isMonoSpaced)
                {
                    this.InitCharTable();
                }
            }

            public int CharWidth(char Char)
            {
                if (this.isMonoSpaced)
                {
                    return this.fontMetrics.AveCharWidth;
                }
                object obj1 = this.charTable[Char];
                if (obj1 == null)
                {
                    obj1 = this.GetCharWidth(Char);
                    this.charTable.Add(Char, obj1);
                }
                return (int) obj1;
            }

            private int GetCharWidth(char Char)
            {
                Size size1 = new Size(0, 0);
                Win32.GetTextExtentPoint32(this.dc, Char.ToString(), 1, ref size1);
                return size1.Width;
            }

            private void InitCharTable()
            {
                this.charTable = new Hashtable();
                for (int num1 = 0; num1 <= 0xff; num1++)
                {
                    this.charTable.Add((char) ((ushort) num1), this.GetCharWidth((char) ((ushort) num1)));
                }
            }


            // Properties
            public Win32.TextMetrics FontMetrics
            {
                get
                {
                    return this.fontMetrics;
                }
            }

            public IntPtr HFont
            {
                get
                {
                    return this.hFont;
                }
            }

            public bool IsMonoSpaced
            {
                get
                {
                    return this.isMonoSpaced;
                }
            }


            // Fields
            private Hashtable charTable;
            private IntPtr dc;
            private Win32.TextMetrics fontMetrics;
            private IntPtr hFont;
            private bool isMonoSpaced;
        }

        private class FontInfos
        {
            // Methods
            public FontInfos(TextPainter Owner, IntPtr DC, Font Font)
            {
                this.owner = Owner;
                this.font = Font;
                this.dc = DC;
                this.stylesTable = new Hashtable();
            }

            public TextPainter.FontInfo CurrentInfo()
            {
                return this.fontInfo;
            }

            ~FontInfos()
            {
                this.stylesTable.Clear();
            }

            public TextPainter.FontInfo InitStyle(FontStyle Style)
            {
                this.fontInfo = (TextPainter.FontInfo) this.stylesTable[Style];
                if (this.fontInfo == null)
                {
                    Font font1 = this.font;
                    if (this.font.Style != Style)
                    {
                        font1 = new Font(this.font.FontFamily, this.font.Size, Style);
                    }
                    this.fontInfo = new TextPainter.FontInfo(this.dc, font1);
                    this.stylesTable.Add(Style, this.fontInfo);
                    this.UpdateMonoSpaced();
                }
                else
                {
                    Win32.SelectObject(this.dc, this.fontInfo.HFont);
                }
                return this.fontInfo;
            }

            private bool UpdateMonoSpaced()
            {
                bool flag1 = this.isMonoSpaced;
                int num1 = this.fontHeight;
                this.isMonoSpaced = false;
                this.fontHeight = 0;
                if (this.fontInfo != null)
                {
                    this.isMonoSpaced = this.fontInfo.IsMonoSpaced;
                    int num2 = this.fontInfo.FontMetrics.AveCharWidth;
                    IDictionaryEnumerator enumerator1 = this.stylesTable.GetEnumerator();
                    enumerator1.Reset();
                    while (enumerator1.MoveNext())
                    {
                        if (this.isMonoSpaced && (((TextPainter.FontInfo) enumerator1.Value).FontMetrics.AveCharWidth != num2))
                        {
                            this.isMonoSpaced = false;
                        }
                        this.fontHeight = Math.Max(this.fontHeight, ((TextPainter.FontInfo) enumerator1.Value).FontMetrics.Height);
                    }
                }
                if (flag1 == this.isMonoSpaced)
                {
                    return (num1 != this.fontHeight);
                }
                return true;
            }


            // Properties
            public int FontHeight
            {
                get
                {
                    return this.fontHeight;
                }
            }

            public bool IsMonoSpaced
            {
                get
                {
                    return this.isMonoSpaced;
                }
            }


            // Fields
            private IntPtr dc;
            private Font font;
            private int fontHeight;
            private TextPainter.FontInfo fontInfo;
            private bool isMonoSpaced;
            private TextPainter owner;
            private Hashtable stylesTable;
        }
    }
}

