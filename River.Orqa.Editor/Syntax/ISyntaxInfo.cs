namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public interface ISyntaxInfo
    {
        // Methods
        void Clear();

        ISyntaxInfo FindByName(string Name, bool CaseSensitive);


        // Properties
        Size DeclarationSize { get; set; }

        string Description { get; set; }

        int ImageIndex { get; set; }

        string Name { get; set; }

        Point Position { get; set; }

    }
}

