namespace River.Orqa.Editor
{
    using System;

    public interface IBookMark
    {
        // Methods
        void Assign(IBookMark Source);


        // Properties
        int Char { get; }

        int Index { get; }

        int Line { get; }

    }
}

