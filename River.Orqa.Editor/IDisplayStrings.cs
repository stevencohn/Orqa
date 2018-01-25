namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;

    public interface IDisplayStrings : IEnumerator, IStringList, ICollection, IEnumerable, ITabulation, IWordWrap, ITextSearch, IWordBreak, ICollapsable, IFmtExport, IExport, IFmtImport, IImport
    {
        // Methods
        Point DisplayPointToPoint(Point Point);

        Point DisplayPointToPoint(int X, int Y);

        short[] GetColorData(int Index);

        int GetCount();

        Point PointToDisplayPoint(Point Point);

        Point PointToDisplayPoint(int X, int Y);


        // Properties
        ISyntaxStrings Lines { get; set; }

        int MaxLineWidth { get; }

    }
}

