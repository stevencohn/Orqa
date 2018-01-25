namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Dialogs;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Drawing.Printing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
	using System.Threading;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class Printing : IPrinting
    {
        // Events
        [Browsable(false)]
        public event CreatePrintEditEvent CreatePrintEdit;
        [Browsable(false)]
        public event EventHandler Initialized;

        // Methods
        public Printing(ISyntaxEdit Owner)
        {
            this.hookHandle = IntPtr.Zero;
            this.lastWnd = IntPtr.Zero;
            this.owner = Owner;
            this.header = new PageHeader(this.owner.Pages[0]);
            this.footer = new PageHeader(this.owner.Pages[0]);
            this.printerSettings = new System.Drawing.Printing.PrinterSettings();
            this.printDocument = new EditorPrintDocument(this, this.printerSettings);
            this.printDocument.PrinterSettings = this.printerSettings;
            this.options = EditConsts.DefaultPrintOptions;
            this.allowedOptions = EditConsts.DefaultPrintOptions;
        }

        public void Assign(IPrinting Source)
        {
            this.Footer = Source.Footer;
            this.Header = Source.Header;
            this.Options = Source.Options;
            this.AllowedOptions = Source.AllowedOptions;
        }

        private int DialogHook(int ncode, IntPtr wParam, IntPtr lParam)
        {
            if ((ncode == 0) && !this.dialogCalled)
            {
                CWPSTRUCT cwpstruct1 = (CWPSTRUCT) Marshal.PtrToStructure(lParam, typeof(CWPSTRUCT));
                if (cwpstruct1.message == 0x110)
                {
                    this.lastWnd = IntPtr.Zero;
                    Win32.EnumChildWindows(cwpstruct1.hwnd, new EnumChildProc(this.EnumDialogChilds), IntPtr.Zero);
                    if (this.lastWnd != IntPtr.Zero)
                    {
                        Win32.SetText(this.lastWnd, "&Options");
                    }
                    this.dialogCalled = true;
                }
            }
            return Win32.CallNextHookEx(this.hookHandle, ncode, wParam, lParam);
        }

        private void DoHelpRequest(object sender, EventArgs e)
        {
            if ((this.PrintOptionsDialog != null) && (this.ExecutePrintOptionsDialog() == DialogResult.OK))
            {
                this.Options = this.PrintOptionsDialog.Options;
            }
        }

        private bool EnumDialogChilds(IntPtr hwnd, IntPtr lParam)
        {
            if (string.Compare(Win32.GetText(hwnd), "&Help", true) == 0)
            {
                Win32.SetText(hwnd, EditConsts.PrintOptionsButtonText);
                this.lastWnd = IntPtr.Zero;
                return false;
            }
            if (string.Compare(Win32.GetClassName(hwnd), "Button", true) == 0)
            {
                this.lastWnd = hwnd;
            }
            return true;
        }

        public DialogResult ExecutePageSetupDialog()
        {
            DialogResult result1;
            this.printDocument.Init(this.owner);
            try
            {
                result1 = this.PageSetupDialog.ShowDialog();
            }
            catch
            {
                result1 = DialogResult.Cancel;
            }
            return result1;
        }

        public DialogResult ExecutePrintDialog()
        {
            DialogResult result2;
            this.printDocument.Init(this.owner);
            try
            {
                DialogResult result1;
                if (this.PrintDialog.ShowHelp)
                {
                    this.HookPrintDialog();
                    try
                    {
                        result1 = this.PrintDialog.ShowDialog();
                        goto Label_0045;
                    }
                    finally
                    {
                        this.UnhookPrintDialog();
                    }
                }
                result1 = this.PrintDialog.ShowDialog();
            Label_0045:
                result2 = result1;
            }
            catch
            {
                result2 = DialogResult.Cancel;
            }
            return result2;
        }

        public DialogResult ExecutePrintOptionsDialog()
        {
            if (this.PrintOptionsDialog == null)
            {
                return DialogResult.None;
            }
            this.PrintOptionsDialog.Options = this.Options;
            this.PrintOptionsDialog.AllowedOptions = this.AllowedOptions;
            this.PrintOptionsDialog.FileName = this.owner.Source.FileName;
            return this.PrintOptionsDialog.ShowDialog();
        }

        public DialogResult ExecutePrintPreviewDialog()
        {
            DialogResult result1;
            this.printDocument.Init(this.owner);
            try
            {
                result1 = this.PrintPreviewDialog.ShowDialog();
            }
            catch
            {
                result1 = DialogResult.Cancel;
            }
            return result1;
        }

        ~Printing()
        {
            if (this.printDialog != null)
            {
                this.printDialog.Dispose();
            }
            if (this.pageSetupDialog != null)
            {
                this.pageSetupDialog.Dispose();
            }
            if (this.printPreviewDialog != null)
            {
                this.printPreviewDialog.Dispose();
            }
        }

        private void HookPrintDialog()
        {
            this.dialogCalled = false;
            this.hookProc = new HookHandler(this.DialogHook);
			this.hookHandle = Win32.SetWindowsHookEx(4, this.hookProc, 0, Thread.CurrentThread.ManagedThreadId);
        }

        public SyntaxEdit OnCreatePrintEdit()
        {
            if (this.CreatePrintEdit != null)
            {
                CreatePrintEditEventArgs args1 = new CreatePrintEditEventArgs();
                this.CreatePrintEdit(this, args1);
                if (args1.PrintEdit != null)
                {
                    return args1.PrintEdit;
                }
            }
            return new SyntaxEdit();
        }

        public void OnInitialized()
        {
            if (this.Initialized != null)
            {
                this.Initialized(this, EventArgs.Empty);
            }
        }

        public void Print()
        {
            this.printDocument.Init(this.owner);
            this.printDocument.Print();
        }

        public virtual void ResetAllowedOptions()
        {
            this.AllowedOptions = EditConsts.DefaultPrintOptions;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultPrintOptions;
        }

        public bool ShouldSerializeAllowedOptions()
        {
            return (this.allowedOptions != EditConsts.DefaultPrintOptions);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultPrintOptions);
        }

        private void UnhookPrintDialog()
        {
            if (this.hookHandle != IntPtr.Zero)
            {
                Win32.UnhookWindowsHookEx(this.hookHandle);
                this.hookHandle = IntPtr.Zero;
                this.hookProc = null;
            }
        }


        // Properties
        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public PrintOptions AllowedOptions
        {
            get
            {
                if (!this.owner.Selection.IsEmpty())
                {
                    return (this.allowedOptions | PrintOptions.PrintSelection);
                }
                return this.allowedOptions;
            }
            set
            {
                this.allowedOptions = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IPageHeader Footer
        {
            get
            {
                return this.footer;
            }
            set
            {
                this.footer.Assign(value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IPageHeader Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header.Assign(value);
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public PrintOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                this.options = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.PageSetupDialog PageSetupDialog
        {
            get
            {
                if (this.pageSetupDialog == null)
                {
                    this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
                    this.pageSetupDialog.Document = this.printDocument;
                    this.pageSetupDialog.PrinterSettings = this.printerSettings;
                    this.pageSetupDialog.PageSettings = new PageSettings();
                }
                return this.pageSetupDialog;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.PrintDialog PrintDialog
        {
            get
            {
                if (this.printDialog == null)
                {
                    this.printDialog = new System.Windows.Forms.PrintDialog();
                    this.printDialog.Document = this.printDocument;
                    this.printDialog.PrinterSettings = this.printerSettings;
                    this.printDialog.AllowSomePages = true;
                    this.printDialog.ShowHelp = true;
                    this.printDialog.HelpRequest += new EventHandler(this.DoHelpRequest);
                }
                return this.printDialog;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Printing.PrintDocument PrintDocument
        {
            get
            {
                return this.printDocument;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Printing.PrinterSettings PrinterSettings
        {
            get
            {
                return this.printerSettings;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IPrintOptionsDialog PrintOptionsDialog
        {
            get
            {
                if (this.printOptionsDialog == null)
                {
                    this.printOptionsDialog = new DlgPrintOptions();
                }
                return this.printOptionsDialog;
            }
            set
            {
                this.printOptionsDialog = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.PrintPreviewDialog PrintPreviewDialog
        {
            get
            {
                if (this.printPreviewDialog == null)
                {
                    this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
                    this.printPreviewDialog.Document = this.printDocument;
                }
                return this.printPreviewDialog;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlPrintingInfo(this);
            }
            set
            {
                ((XmlPrintingInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private PrintOptions allowedOptions;
        private bool dialogCalled;
        private PageHeader footer;
        private PageHeader header;
        private IntPtr hookHandle;
        private HookHandler hookProc;
        private IntPtr lastWnd;
        private PrintOptions options;
        private ISyntaxEdit owner;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.PrintDialog printDialog;
        private EditorPrintDocument printDocument;
        private System.Drawing.Printing.PrinterSettings printerSettings;
        private IPrintOptionsDialog printOptionsDialog;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
    }
}

