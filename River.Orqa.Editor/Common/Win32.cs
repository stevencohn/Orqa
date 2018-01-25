namespace River.Orqa.Editor.Common
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class Win32
    {
        // Methods
        public Win32()
        {
        }

        [DllImport("gdi32.dll")]
        private static extern bool Arc(IntPtr DC, int LeftRect, int TopRect, int RightRect, int BottomRect, int XStartArc, int YStartArc, int XEndArc, int YEndArc);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr DestDC, int X, int Y, int Width, int Height, IntPtr SrcDC, int XSrc, int YSrc, int Rop);

        [DllImport("USER32.dll")]
        public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("uxtheme.dll")]
        public static extern int CloseThemeData(int htheme);

        public static int ColorToGdiColor(Color color)
        {
            int num1 = color.ToArgb() & 0xffffff;
            return (((num1 >> 0x10) + (num1 & 65280)) + ((num1 & 0xff) << 0x10));
        }

        [DllImport("user32.dll")]
        public static extern bool CreateCaret(IntPtr Handle, IntPtr HBitmap, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr DC, int Width, int Height);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr DC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int Style, int Width, int Color);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRectRgnIndirect(ref GdiRect R);

        public static IntPtr CreateRectRgnIndirect(Rectangle Rect)
        {
            GdiRect rect1 = new GdiRect(Rect.Left, Rect.Top, Rect.Right, Rect.Bottom);
            return Win32.CreateRectRgnIndirect(ref rect1);
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int color);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr DC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr p1);

        [DllImport("user32.dll")]
        public static extern bool DestroyCaret();

        public static bool DrawArc(IntPtr DC, int X, int Y, int Width, int Height, int startAngle, int sweepAngle)
        {
            double num1 = (startAngle * 3.1415926535897931) / 180;
            double num2 = (sweepAngle * 3.1415926535897931) / 180;
            int num3 = X + ((int) (((1 + Math.Cos(num1)) * Width) / 2));
            int num4 = Y + ((int) (((1 - Math.Sin(num1)) * Height) / 2));
            int num5 = X + ((int) (((1 + Math.Cos(num2)) * Width) / 2));
            int num6 = Y + ((int) (((1 - Math.Sin(num2)) * Height) / 2));
            return Win32.Arc(DC, X, Y, X + Width, Y + Height, num3, num4, num5, num6);
        }

        [DllImport("user32.dll", CharSet=CharSet.Unicode)]
        private static extern bool DrawFocusRect(IntPtr DC, ref GdiRect R);

        public static bool DrawFocusRect(IntPtr DC, int X1, int Y1, int X2, int Y2)
        {
            GdiRect rect1 = new GdiRect(X1, Y1, X2, Y2);
            return Win32.DrawFocusRect(DC, ref rect1);
        }

        [DllImport("user32.dll", CharSet=CharSet.Unicode)]
        private static extern int DrawText(IntPtr DC, string S, int Len, ref GdiRect R, int Format);

        public static int DrawText(IntPtr DC, string S, int Len, ref Rectangle R, int Format)
        {
            GdiRect rect1 = new GdiRect(R);
            return Win32.DrawText(DC, S, Len, ref rect1, Format);
        }

        [DllImport("uxtheme.dll")]
        public static extern bool DrawThemeBackground(int theme, IntPtr hdc, int partID, int stateID, ref GdiRect rect, int clipRect);

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr DC, int L, int T, int R, int B);

        public static int ExtTextOut(IntPtr DC, Rectangle R, int X, int Y, int Options, string S, int Len)
        {
            GdiRect rect1 = new GdiRect(R);
            return Win32.ExtTextOut(DC, X, Y, Options, ref rect1, S, Len, IntPtr.Zero);
        }

        [DllImport("gdi32.dll", CharSet=CharSet.Unicode)]
        private static extern int ExtTextOut(IntPtr DC, int X, int Y, int Options, ref GdiRect R, string S, int Len, IntPtr Buffer);

        public static int ExtTextOut(IntPtr DC, int X, int Y, int Width, int Height, int Options, string S, int Len)
        {
            GdiRect rect1 = new GdiRect(X, Y, X + Width, Y + Height);
            return Win32.ExtTextOut(DC, X, Y, Options, ref rect1, S, Len, IntPtr.Zero);
        }

        public static int ExtTextOut(IntPtr DC, int X, int Y, int Width, int Height, int Options, string S, int Len, int[] spacings)
        {
            int num4;
            if (spacings == null)
            {
                return Win32.ExtTextOut(DC, X, Y, Width, Height, Options, S, Len);
            }
            int num1 = spacings.GetLength(0);
            int num2 = (Len > 0) ? Len : S.Length;
            GdiRect rect1 = new GdiRect(X, Y, X + Width, Y + Height);
            IntPtr ptr1 = Marshal.AllocHGlobal((int) (num2 * 4));
            try
            {
                for (int num3 = 0; num3 < num2; num3++)
                {
                    Marshal.WriteInt32(ptr1, (int) (num3 * 4), (num3 < num1) ? spacings[num3] : 0);
                }
                num4 = Win32.ExtTextOut(DC, X, Y, Options, ref rect1, S, Len, ptr1);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr1);
            }
            return num4;
        }

        [DllImport("user32.dll")]
        private static extern int FillRect(IntPtr DC, ref GdiRect R, IntPtr Brush);

        public static int FillRect(IntPtr DC, Rectangle R, IntPtr Brush)
        {
            GdiRect rect1 = new GdiRect(R);
            return Win32.FillRect(DC, ref rect1, Brush);
        }

        public static int FillRect(IntPtr DC, int X, int Y, int W, int H, IntPtr Brush)
        {
            GdiRect rect1 = new GdiRect(X, Y, X + W, Y + H);
            return Win32.FillRect(DC, ref rect1, Brush);
        }

        [DllImport("user32.dll")]
        public static extern int FrameRect(IntPtr DC, ref GdiRect R, IntPtr Brush);

        public static int FrameRect(IntPtr DC, Rectangle R, IntPtr Brush)
        {
            GdiRect rect1 = new GdiRect(R);
            return Win32.FrameRect(DC, ref rect1, Brush);
        }

        public static int FrameRect(IntPtr DC, int X, int Y, int W, int H, IntPtr Brush)
        {
            GdiRect rect1 = new GdiRect(X, Y, X + W, Y + H);
            return Win32.FrameRect(DC, ref rect1, Brush);
        }

        public static string GetClassName(IntPtr hWnd)
        {
            string text1;
            int num1 = 0xff;
            IntPtr ptr1 = Marshal.AllocHGlobal((int) (num1 + 1));
            try
            {
                int num2 = Win32.GetClassName(hWnd, ptr1, num1);
                text1 = (num2 > 0) ? Marshal.PtrToStringAnsi(ptr1, num2) : string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr1);
            }
            return text1;
        }

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, IntPtr lpClassName, int nMaxCount);

        [DllImport("gdi32.dll")]
        public static extern int GetClipRgn(IntPtr Handle, IntPtr Rgn);

        [DllImport("uxtheme.dll", CharSet=CharSet.Unicode)]
        public static extern int GetCurrentThemeName(IntPtr ThemeFileName, int MaxNameChars, IntPtr ColorName, int MaxColorName, int Dummy1, int Dummy2);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr DC, int Index);

        [DllImport("gdi32.dll")]
        public static extern int GetGraphicsMode(IntPtr DC);

        public static Point GetScreenDpi()
        {
            Point point1;
            IntPtr ptr1 = Win32.GetDC(IntPtr.Zero);
            try
            {
                point1 = new Point(Win32.GetDeviceCaps(ptr1, 0x58), Win32.GetDeviceCaps(ptr1, 90));
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, ptr1);
            }
            return point1;
        }

        [DllImport("user32.dll")]
        private static extern int GetScrollInfo(IntPtr hWnd, int BarFlag, ref ScrollInfo scrollInfo);

        public static int GetScrollPos(IntPtr Handle, int Code)
        {
            ScrollInfo info1;
            info1.cbSize = 0x1c;
            info1.fMask = 0x10;
            info1.nMin = 0;
            info1.nMax = 0;
            info1.nPage = 0;
            info1.nPos = 0;
            info1.nTrackPos = 0;
            Win32.GetScrollInfo(Handle, Code, ref info1);
            return info1.nTrackPos;
        }

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        public static string GetText(IntPtr hWnd)
        {
            string text1;
            int num1 = 0xff;
            IntPtr ptr1 = Marshal.AllocHGlobal((int) (num1 + 1));
            try
            {
                int num2 = (int) Win32.SendMessage(hWnd, 13, (IntPtr) num1, ptr1);
                text1 = (num2 > 0) ? Marshal.PtrToStringAnsi(ptr1, num2) : string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr1);
            }
            return text1;
        }

        [DllImport("gdi32.dll", CharSet=CharSet.Unicode)]
        private static extern bool GetTextExtentPoint32(IntPtr DC, string S, int Count, ref GdiSize Size);

        public static bool GetTextExtentPoint32(IntPtr DC, string S, int Count, ref Size Size)
        {
            GdiSize size1 = new GdiSize(0, 0);
            bool flag1 = Win32.GetTextExtentPoint32(DC, S, Count, ref size1);
            Size.Width = size1.cx;
            Size.Height = size1.cy;
            return flag1;
        }

        [DllImport("gdi32.dll")]
        public static extern int GetTextMetrics(IntPtr DC, ref TextMetrics Metrics);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeAppProperties();

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeColor(int htheme, int partID, int stateID, int propID, out ColorRef colorRef);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemePartSize(int htheme, IntPtr hdc, int partID, int stateID, int rect, int sizeType, out GdiSize size);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr Handle);

        public static short HiWord(IntPtr Value)
        {
            return (short) (Value.ToInt32() >> 0x10);
        }

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DrawEx(IntPtr Handle, int Index, IntPtr DC, int X, int Y, int Dx, int Dy, int Bk, int Fr, int Style);

        public static bool InitCommonControls(int icc)
        {
            INITCOMMONCONTROLSEX initcommoncontrolsex1;
            initcommoncontrolsex1 = new INITCOMMONCONTROLSEX();
            initcommoncontrolsex1.dwSize = 0x10;
            initcommoncontrolsex1.dwICC = icc;
            return Win32.InitCommonControlsEx(ref initcommoncontrolsex1);
        }

        [DllImport("comctl32.dll")]
        private static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX iic);

        [DllImport("gdi32.dll")]
        public static extern int IntersectClipRect(IntPtr DC, int L, int T, int R, int B);

        [DllImport("uxtheme.dll")]
        public static extern bool IsAppThemed();

        [DllImport("uxtheme.dll")]
        public static extern bool IsThemeActive();

        [DllImport("gdi32.dll")]
        public static extern bool LineTo(IntPtr DC, int X, int Y);

        public static short LoWord(IntPtr Value)
        {
            return (short) Value.ToInt32();
        }

        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx(IntPtr DC, int X, int Y, IntPtr P);

        [DllImport("uxtheme.dll", CharSet=CharSet.Unicode)]
        public static extern int OpenThemeData(int hwnd, string classList);

        [DllImport("gdi32.dll")]
        private static extern bool PolyBezier(IntPtr DC, IntPtr Points, int Count);

        public static bool PolyBezier(IntPtr DC, Point[] Points, int Count)
        {
            bool flag1;
            IntPtr ptr1 = Marshal.AllocHGlobal((int) (Count * 8));
            try
            {
                for (int num1 = 0; num1 < Count; num1++)
                {
                    Marshal.WriteInt32(ptr1, (int) (num1 * 8), Points[num1].X);
                    Marshal.WriteInt32(ptr1, (int) ((num1 * 8) + 4), Points[num1].Y);
                }
                flag1 = Win32.PolyBezier(DC, ptr1, Count);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr1);
            }
            return flag1;
        }

        [DllImport("gdi32.dll")]
        private static extern bool Polygon(IntPtr DC, IntPtr Points, int Count);

        public static bool Polygon(IntPtr DC, Point[] Points, int Count)
        {
            bool flag1;
            IntPtr ptr1 = Marshal.AllocHGlobal((int) (Count * 8));
            try
            {
                for (int num1 = 0; num1 < Count; num1++)
                {
                    Marshal.WriteInt32(ptr1, (int) (num1 * 8), Points[num1].X);
                    Marshal.WriteInt32(ptr1, (int) ((num1 * 8) + 4), Points[num1].Y);
                }
                flag1 = Win32.Polygon(DC, ptr1, Count);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr1);
            }
            return flag1;
        }

        [DllImport("USER32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool RoundRect(IntPtr DC, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        private static extern bool ScaleViewportExtEx(IntPtr DC, int Xnum, int Xdenom, int Ynum, int Ydenom, ref GdiSize size);

        public static bool ScaleViewportExtEx(IntPtr DC, int Xnum, int Xdenom, int Ynum, int Ydenom, ref Size size)
        {
            GdiSize size1 = new GdiSize(size.Width, size.Height);
            bool flag1 = Win32.ScaleViewportExtEx(DC, Xnum, Xdenom, Ynum, Ydenom, ref size1);
            size.Width = size1.cx;
            size.Height = size1.cy;
            return flag1;
        }

        public static void ScrollWindow(IntPtr Handle, int X, int Y, Rectangle R)
        {
            GdiRect rect1 = new GdiRect(R);
            Win32.ScrollWindow(Handle, X, Y, ref rect1, 0);
        }

        [DllImport("user32.dll")]
        private static extern bool ScrollWindow(IntPtr Handle, int X, int Y, ref GdiRect R, int ClipR);

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr Handle, IntPtr Rgn);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr DC, IntPtr Obj);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("gdi32.dll")]
        public static extern int SetBkColor(IntPtr DC, int Color);

        [DllImport("gdi32.dll")]
        public static extern int SetBkMode(IntPtr DC, int bkMode);

        [DllImport("user32.dll")]
        public static extern bool SetCaretPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr Handle);

        [DllImport("gdi32.dll")]
        public static extern int SetGraphicsMode(IntPtr DC, int Mode);

        public static int SetPixel(IntPtr DC, int X, int Y, Color Color)
        {
            return Win32.SetPixel(DC, X, Y, Win32.ColorToGdiColor(Color));
        }

        [DllImport("gdi32.dll")]
        private static extern int SetPixel(IntPtr DC, int X, int Y, int Color);

        public static void SetScrollBarInfo(IntPtr Handle, int Size, int PageSize, int Code)
        {
            if (Size < PageSize)
            {
                Size = PageSize;
            }
            Win32.SetScrollSize(Handle, Code, 0, Size, PageSize);
        }

        [DllImport("user32.dll")]
        private static extern int SetScrollInfo(IntPtr hWnd, int BarFlag, ref ScrollInfo scrollInfo, bool Redraw);

        public static void SetScrollPos(IntPtr Handle, int Code, int Pos)
        {
            ScrollInfo info1;
            info1.cbSize = 0x1c;
            info1.fMask = 4;
            info1.nMin = 0;
            info1.nMax = 0;
            info1.nPage = 0;
            info1.nPos = Pos;
            info1.nTrackPos = 0;
            Win32.SetScrollInfo(Handle, Code, ref info1, true);
        }

        private static void SetScrollSize(IntPtr Handle, int Code, int MinPos, int MaxPos, int PageSize)
        {
            ScrollInfo info1;
            info1.cbSize = 0x1c;
            info1.fMask = 3;
            info1.nMin = MinPos;
            info1.nMax = MaxPos;
            info1.nPage = (uint) PageSize;
            info1.nPos = 0;
            info1.nTrackPos = 0;
            Win32.SetScrollInfo(Handle, Code, ref info1, true);
        }

        public static void SetText(IntPtr hWnd, string Text)
        {
            Win32.SendMessage(hWnd, 12, IntPtr.Zero, Marshal.StringToHGlobalAnsi(Text));
        }

        [DllImport("gdi32.dll")]
        public static extern int SetTextColor(IntPtr DC, int Color);

        [DllImport("gdi32.dll")]
        private static extern bool SetViewportExtEx(IntPtr DC, int nXExtent, int nYExtent, ref GdiSize size);

        public static bool SetViewportExtEx(IntPtr DC, int nXExtent, int nYExtent, ref Size size)
        {
            GdiSize size1 = new GdiSize(size.Width, size.Height);
            bool flag1 = Win32.SetViewportExtEx(DC, nXExtent, nYExtent, ref size1);
            size.Width = size1.cx;
            size.Height = size1.cy;
            return flag1;
        }

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("USER32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookHandler lpfn, int hMod, int dwThreadId);

        [DllImport("gdi32.dll")]
        public static extern bool SetWorldTransform(IntPtr DC, ref XFORM p2);

        [DllImport("user32.dll")]
        public static extern bool ShowCaret(IntPtr Handle);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int Flags);

        [DllImport("USER32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);


        // Fields
        public const int ETO_CLIPPED = 4;
        public const int ETO_OPAQUE = 2;
        public const int GM_ADVANCED = 2;
        public const int GM_COMPATIBLE = 1;
        public const int GWL_WNDPROC = -4;
        public const int HWND_TOPMOST = -1;
        public const int LOGPIXELSX = 0x58;
        public const int LOGPIXELSY = 90;
        public const int OPAQUE = 2;
        public const int PS_SOLID = 0;
        public const int RGN_AND = 1;
        public const int RGN_COPY = 5;
        public const int RGN_DIFF = 4;
        public const int RGN_MAX = 5;
        public const int RGN_MIN = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
        public const int SB_BOTTOM = 7;
        public const int SB_ENDSCROLL = 8;
        public const int SB_HORZ = 0;
        public const int SB_LEFT = 6;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINELEFT = 0;
        public const int SB_LINERIGHT = 1;
        public const int SB_LINEUP = 0;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGERIGHT = 3;
        public const int SB_PAGEUP = 2;
        public const int SB_RIGHT = 7;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_VERT = 1;
        public const int SCRCOPY = 0xcc0020;
        public const int Scroll_Size = 0x1c;
        public const int SIF_PAGE = 2;
        public const int SIF_POS = 4;
        public const int SIF_RANGE = 1;
        public const int SIF_TRACKPOS = 0x10;
        public const int SM_CXHSCROLL = 0x15;
        public const int SM_CYVSCROLL = 20;
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        public const int SWP_NOACTIVATE = 0x10;
        public const int SWP_NOMOVE = 2;
        public const int SWP_NOSIZE = 1;
        public const int SWP_NOZORDER = 4;
        public const int SWP_SHOWWINDOW = 0x40;
        public const int TMPF_DEVICE = 8;
        public const int TMPF_FIXED_PITCH = 1;
        public const int TMPF_TRUETYPE = 4;
        public const int TMPF_VECTOR = 2;
        public const int TRANSPARENT = 1;
        public const int WH_CALLWNDPROC = 4;
        public const int WH_GETMESSAGE = 3;
        public const int WH_MOUSE = 7;
        public const int WM_CHAR = 0x102;
        public const int WM_CLOSE = 0x10;
        public const int WM_CLOSEDROPPED = 0x401;
        public const int WM_DESTROY = 2;
        public const int WM_GETTEXT = 13;
        public const int WM_HSCROLL = 0x114;
        public const int WM_INITDIALOG = 0x110;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MAXCLICK = 520;
        public const int WM_MAXNCCLICK = 0xa9;
        public const int WM_MINCLICK = 0x201;
        public const int WM_MINNCCLICK = 0xa1;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_NCACTIVATE = 0x86;
        public const int WM_NCDESTROY = 130;
        public const int WM_NCLBUTTONDOWN = 0xa1;
        public const int WM_NCPAINT = 0x85;
        public const int WM_SETCURSOR = 0x20;
        public const int WM_SETTEXT = 12;
        public const int WM_SIZE = 5;
        public const int WM_SYSKEYDOWN = 260;
        public const int WM_VSCROLL = 0x115;
        public const int WS_BORDER = 0x800000;
        public const int WS_EX_CLIENTEDGE = 0x200;
        public const int WS_HSCROLL = 0x100000;
        public const int WS_VSCROLL = 0x200000;

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        public struct ColorRef
        {
            public byte rgbRed;
            public byte rgbGreen;
            public byte rgbBlue;
            public byte rgbReserved;
            public System.Drawing.Color Color
            {
                get
                {
                    return System.Drawing.Color.FromArgb(this.rgbRed, this.rgbGreen, this.rgbBlue);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdiPoint
        {
            public int X;
            public int Y;
            public GdiPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdiRect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public GdiRect(int L, int T, int R, int B)
            {
                this.left = L;
                this.top = T;
                this.right = R;
                this.bottom = B;
            }
            public GdiRect(Rectangle R)
            {
                this.left = R.Left;
                this.top = R.Top;
                this.right = R.Right;
                this.bottom = R.Bottom;
            }
            public Rectangle ToRectangle()
            {
                return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdiSize
        {
            public int cx;
            public int cy;
            public GdiSize(int cX, int cY)
            {
                this.cx = cX;
                this.cy = cY;
            }
            public GdiSize(Size size)
            {
                this.cx = size.Width;
                this.cy = size.Height;
            }
            public Size ToSize()
            {
                return new Size(this.cx, this.cy);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct INITCOMMONCONTROLSEX
        {
            // Fields
            [FieldOffset(8)]
            public int dwICC;
            [FieldOffset(0)]
            public int dwSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Win32.GdiPoint Pt;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct ScrollInfo
        {
            // Fields
            [FieldOffset(0)]
            public uint cbSize;
            [FieldOffset(4)]
            public uint fMask;
            [FieldOffset(12)]
            public int nMax;
            [FieldOffset(8)]
            public int nMin;
            [FieldOffset(0x10)]
            public uint nPage;
            [FieldOffset(20)]
            public int nPos;
            [FieldOffset(0x18)]
            public int nTrackPos;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct TextMetrics
        {
            // Fields
            [FieldOffset(4)]
            public int Accent;
            [FieldOffset(20)]
            public int AveCharWidth;
            [FieldOffset(0x39)]
            public byte BreakChar;
            [FieldOffset(0x34)]
            public byte CharSet;
            [FieldOffset(0x2e)]
            public byte DefaultChar;
            [FieldOffset(8)]
            public int Descent;
            [FieldOffset(0x24)]
            public int DigitizedAspectX;
            [FieldOffset(40)]
            public int DigitizedAspectY;
            [FieldOffset(0x10)]
            public int ExternalLeading;
            [FieldOffset(0x2c)]
            public byte FirstChar;
            [FieldOffset(0)]
            public int Height;
            [FieldOffset(12)]
            public int InternalLeading;
            [FieldOffset(0x3a)]
            public byte Italic;
            [FieldOffset(0x2d)]
            public byte LastChar;
            [FieldOffset(0x18)]
            public int MaxCharWidth;
            [FieldOffset(0x20)]
            public int Overhang;
            [FieldOffset(0x33)]
            public byte PitchAndFamily;
            [FieldOffset(0x34)]
            private int Reserved;
            [FieldOffset(50)]
            public byte StruckOut;
            [FieldOffset(0x31)]
            public byte Underlined;
            [FieldOffset(0x1c)]
            public int Weight;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct XFORM
        {
            // Methods
            public XFORM(float M11, float M12, float M21, float M22, float Dx, float Dy)
            {
                this.eM11 = M11;
                this.eM12 = M12;
                this.eM21 = M21;
                this.eM22 = M22;
                this.eDx = Dx;
                this.eDy = Dy;
            }


            // Fields
            [FieldOffset(0x10)]
            public float eDx;
            [FieldOffset(20)]
            public float eDy;
            [FieldOffset(0)]
            public float eM11;
            [FieldOffset(4)]
            public float eM12;
            [FieldOffset(8)]
            public float eM21;
            [FieldOffset(12)]
            public float eM22;
        }
    }
}

