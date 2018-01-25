namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ICodeCompletionProvider : IList, ICollection, IEnumerable
    {
        // Events
        event ClosePopupEvent ClosePopup;

        // Methods
        bool ColumnVisible(int Column);

        string GetColumnText(int Index, int Column);


        // Properties
        int ColumnCount { get; }

        string[] Descriptions { get; }

        int[] ImageIndexes { get; }

        ImageList Images { get; set; }

        int SelIndex { get; set; }

        bool ShowDescriptions { get; set; }

        string[] Strings { get; }

    }
}

