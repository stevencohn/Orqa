namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Dialogs;
    using System;
    using System.Drawing.Printing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IPrinting
    {
        // Events
        event CreatePrintEditEvent CreatePrintEdit;
        event EventHandler Initialized;

        // Methods
        void Assign(IPrinting Source);

        DialogResult ExecutePageSetupDialog();

        DialogResult ExecutePrintDialog();

        DialogResult ExecutePrintOptionsDialog();

        DialogResult ExecutePrintPreviewDialog();

        void Print();

        void ResetAllowedOptions();

        void ResetOptions();


        // Properties
        PrintOptions AllowedOptions { get; set; }

        IPageHeader Footer { get; set; }

        IPageHeader Header { get; set; }

        PrintOptions Options { get; set; }

        System.Windows.Forms.PageSetupDialog PageSetupDialog { get; }

        System.Windows.Forms.PrintDialog PrintDialog { get; }

        System.Drawing.Printing.PrintDocument PrintDocument { get; }

        System.Drawing.Printing.PrinterSettings PrinterSettings { get; }

        IPrintOptionsDialog PrintOptionsDialog { get; set; }

        System.Windows.Forms.PrintPreviewDialog PrintPreviewDialog { get; }

    }
}

