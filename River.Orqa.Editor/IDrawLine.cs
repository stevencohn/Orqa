namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface IDrawLine
    {
        // Methods
        void DrawLine(int Index, Point Point, Rectangle ClipRect);

        int MeasureLine(int Index, int Pos, int Len);

        int MeasureLine(string Line, ref short[] ColorData, int Pos, int Len);

        int MeasureLine(int Index, int Pos, int Len, int Width, out int Chars);

        int MeasureLine(string Line, ref short[] ColorData, int Pos, int Len, int Width, out int Chars);

        void ResetDisableColorPaint();

        void ResetDisableSyntaxPaint();


        // Properties
        bool DisableColorPaint { get; set; }

        bool DisableSyntaxPaint { get; set; }

    }
}

