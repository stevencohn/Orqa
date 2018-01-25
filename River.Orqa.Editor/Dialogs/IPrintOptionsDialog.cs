namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using System;
    using System.Windows.Forms;

    public interface IPrintOptionsDialog
    {
        // Methods
        void ResetAllowedOptions();

        void ResetOptions();

        DialogResult ShowDialog();


        // Properties
        PrintOptions AllowedOptions { get; set; }

        string FileName { get; set; }

        PrintOptions Options { get; set; }

    }
}

