namespace River.Orqa.Editor
{
    using System;

    public interface INavigateEx : INavigate
    {
        // Methods
        void MoveCharLeft();

        void MoveCharRight();

        void MoveFileBegin();

        void MoveFileEnd();

        void MoveLineBegin();

        void MoveLineDown();

        void MoveLineEnd();

        void MoveLineUp();

        void MovePageDown();

        void MovePageUp();

        void MoveScreenBottom();

        void MoveScreenTop();

        void MoveToCloseBrace();

        void MoveToOpenBrace();

        void MoveWordLeft();

        void MoveWordRight();

        void ScrollLineDown();

        void ScrollLineUp();

    }
}

