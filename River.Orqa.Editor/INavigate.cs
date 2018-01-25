namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface INavigate
    {
        // Methods
        void MoveTo(Point Position);

        void MoveTo(int X, int Y);

        void MoveToChar(int X);

        void MoveToLine(int Y);

        void Navigate(int DeltaX, int DeltaY);

        void ResetNavigateOptions();

        Point RestorePosition(int Index);

        int StorePosition(Point pt);

        void ValidatePosition(ref Point Position);


        // Properties
        River.Orqa.Editor.NavigateOptions NavigateOptions { get; set; }

        Point Position { get; set; }

    }
}

