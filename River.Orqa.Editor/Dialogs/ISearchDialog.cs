namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public interface ISearchDialog
    {
        // Methods
        void DoneSearch(ISearch Search);

        void EnsureVisible(Rectangle Rect);

        DialogResult Execute(ISearch Search, bool IsModal, bool IsReplace);

        void ToggleHiddenText();

        void ToggleMatchCase();

        void ToggleRegularExpressions();

        void ToggleSearchUp();

        void ToggleWholeWord();


        // Properties
        bool Visible { get; set; }

    }
}

