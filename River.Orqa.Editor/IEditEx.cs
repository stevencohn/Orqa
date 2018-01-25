namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface IEditEx : IEdit
    {
        // Methods
        bool BreakLine();

        bool DeleteBlock(Rectangle Rect);

        bool DeleteLeft(int Len);

        bool DeleteRight(int Len);

        bool Insert(string String);

        bool InsertBlock(string[] Strings);

        bool InsertBlock(ISyntaxStrings Strings);

        bool InsertBlock(string Text);

        bool InsertFromFile(string File);

        bool NewLine();

        bool NewLineAbove();

        bool NewLineBelow();

        bool UnBreakLine();

    }
}

