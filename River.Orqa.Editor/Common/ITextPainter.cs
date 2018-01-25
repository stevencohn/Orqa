namespace River.Orqa.Editor.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public interface ITextPainter
    {
        // Methods
        void BeginPaint(Graphics Graphics);

        int CharWidth(char Char, int Count);

        int CharWidth(char Char, int Width, out int Count);

        void Clear();

        void DrawDotLine(int X1, int Y1, int X2, int Y2, System.Drawing.Color Color);

        void DrawImage(ImageList Images, int ImageIndex, Rectangle R);

        void DrawLine(int X1, int Y1, int X2, int Y2);

        void DrawLine(int X1, int Y1, int X2, int Y2, System.Drawing.Color Color, int Width, DashStyle PenStyle);

        void DrawLiveSpell(Rectangle R, System.Drawing.Color Color);

        void DrawRectangle(Rectangle R);

        void DrawRectangle(int X, int Y, int Width, int Height);

        void DrawRoundRectangle(int Left, int Top, int Right, int Bottom, int Width, int Height);

        void DrawText(string String, int Len, Rectangle R);

        void DrawText(string String, int Len, Rectangle R, int Flags);

        void EndPaint(Graphics Graphics);

        void EndTransform();

        void ExcludeClipRect(int X, int Y, int Width, int Height);

        void FillRectangle(Rectangle R);

        void FillRectangle(int X, int Y, int Width, int Height);

        void IntersectClipRect(int X, int Y, int Width, int Height);

        void Polygon(Point[] points, System.Drawing.Color Color);

        void RestoreClip(IntPtr Rgn);

        IntPtr SaveClip(Rectangle Rect);

        int StringWidth(string String);

        int StringWidth(string String, int Pos, int Len);

        int StringWidth(string String, int Width, out int Count, bool Exact);

        int StringWidth(string String, int Pos, int Len, int Width, out int Count);

        int StringWidth(string String, int Pos, int Len, int Width, out int Count, bool Exact);

        void TextOut(string String, int Len, Rectangle R);

        void TextOut(string String, int Len, Rectangle R, int Flags);

        void TextOut(string String, int Len, int X, int Y);

        void TextOut(string String, int Len, Rectangle R, int Flags, int Space);

        void TextOut(string String, int Len, int X, int Y, int Flags);

        void TextOut(string String, int Len, Rectangle R, int X, int Y, int Flags);

        void Transform(int X, int Y, float ScaleX, float ScaleY);


        // Properties
        System.Drawing.Color BkColor { get; set; }

        int BkMode { get; set; }

        System.Drawing.Color Color { get; set; }

        System.Drawing.Font Font { get; set; }

        int FontHeight { get; }

        System.Drawing.FontStyle FontStyle { get; set; }

        int FontWidth { get; }

        bool IsMonoSpaced { get; }

        System.Drawing.Color PenColor { get; set; }

    }
}

