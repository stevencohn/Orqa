namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public interface IControlProps
    {
        // Methods
        void Invalidate();

        void Invalidate(Rectangle Rect);

        Point PointToClient(Point p);

        Point PointToScreen(Point p);

        void Update();


        // Properties
        Color BackColor { get; set; }

        Image BackgroundImage { get; set; }

        Rectangle Bounds { get; set; }

        Rectangle ClientRectangle { get; }

        bool Focused { get; }

        System.Drawing.Font Font { get; set; }

        Color ForeColor { get; set; }

        IntPtr Handle { get; }

        int Height { get; set; }

        bool IsHandleCreated { get; }

        int Left { get; set; }

        Point Location { get; set; }

        Control Parent { get; set; }

        int Top { get; set; }

        bool Visible { get; set; }

        int Width { get; set; }

    }
}

