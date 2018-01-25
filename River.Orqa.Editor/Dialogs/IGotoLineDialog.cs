namespace River.Orqa.Editor.Dialogs
{
    using System;
    using System.Windows.Forms;

    public interface IGotoLineDialog
    {
        // Methods
        DialogResult Execute(object Sender, int Lines, ref int Line);

    }
}

