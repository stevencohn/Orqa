namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface IBookMarks : IList, ICollection, IEnumerable
    {
        // Methods
        void Assign(IBookMarks Source);

        void ClearAllBookMarks();

        void ClearBookMark(int BookMark);

        void ClearBookMarks(int Line);

        bool FindBookMark(int BookMark, out Point Point);

        int GetBookMark(int Line);

        int GetBookMark(Point StartPoint, Point EndPoint);

        int GetBookMarks(Point StartPoint, Point EndPoint, IList List);

        void GotoBookMark(int BookMark);

        void GotoNextBookMark();

        void GotoPrevBookMark();

        int NextBookMark();

        void SetBookMark(Point Point, int BookMark);

        void SetBookMark(int Line, int BookMark);

        void ToggleBookMark();

        void ToggleBookMark(int BookMark);

        void ToggleBookMark(Point Position, int BookMark);

        void ToggleBookMark(int Line, int BookMark);

    }
}

