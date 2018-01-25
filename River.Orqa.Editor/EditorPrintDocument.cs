namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class EditorPrintDocument : PrintDocument
    {
        // Methods
        public EditorPrintDocument(IPrinting Printing, PrinterSettings Settings)
        {
            this.printing = Printing;
            base.PrinterSettings = Settings;
        }

        public void Init(ISyntaxEdit SyntaxEdit)
        {
            base.PrinterSettings.MinimumPage = 1;
            base.PrinterSettings.FromPage = Math.Max(base.PrinterSettings.FromPage, 1);
            base.PrinterSettings.ToPage = Math.Max(base.PrinterSettings.ToPage, 1);
            this.syntaxEdit = ((Printing) this.printing).OnCreatePrintEdit();
            this.syntaxEdit.Font = SyntaxEdit.Font;
            if (((this.printing.Options & PrintOptions.PrintSelection) != PrintOptions.None) && !SyntaxEdit.Selection.IsEmpty())
            {
                this.syntaxEdit.Lines.Text = SyntaxEdit.Selection.SelectedText;
            }
            else
            {
                this.syntaxEdit.Source = SyntaxEdit.Source;
            }
            this.syntaxEdit.WordWrap = (this.printing.Options & PrintOptions.WordWrap) != PrintOptions.None;
            this.syntaxEdit.Gutter.Assign(SyntaxEdit.Gutter);
            this.syntaxEdit.Gutter.Visible = false;
            this.syntaxEdit.Gutter.Options = ((this.printing.Options & PrintOptions.LineNumbers) != PrintOptions.None) ? GutterOptions.PaintLineNumbers : GutterOptions.None;
            this.syntaxEdit.Gutter.LineNumbersBackColor = this.syntaxEdit.BackColor;
            this.syntaxEdit.Margin.Visible = false;
            this.syntaxEdit.Braces.BracesOptions = BracesOptions.None;
            if ((this.printing.Options & PrintOptions.UseSyntax) == PrintOptions.None)
            {
                this.syntaxEdit.DisableSyntaxPaint = true;
            }
            if ((this.printing.Options & PrintOptions.UseColors) == PrintOptions.None)
            {
                this.syntaxEdit.DisableColorPaint = true;
            }
            this.syntaxEdit.Scrolling.WindowOriginX = 0;
            this.syntaxEdit.Scrolling.WindowOriginY = 0;
            ((Printing) this.printing).OnInitialized();
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            this.startPrinting = true;
            this.page = 0;
            this.pageIndex = 0;
            this.startLine = 0;
        }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
        }

        protected override void OnPrintPage(PrintPageEventArgs ev)
        {
            int num1;
            base.OnPrintPage(ev);
            Rectangle rectangle1 = ev.MarginBounds;
            if (this.startPrinting)
            {
                this.syntaxEdit.Bounds = ev.MarginBounds;
                ((DisplayStrings) this.syntaxEdit.DisplayLines).ScanToEnd(true);
                int num2 = this.syntaxEdit.DisplayLines.GetCount() - 1;
                num1 = Math.Max(this.syntaxEdit.LinesInHeight(), 1);
                this.pages = num2 / num1;
                if ((num2 % num1) != 0)
                {
                    this.pages++;
                }
                this.pageCount = Math.Max(this.pages, 1);
                if (base.PrinterSettings.PrintRange == PrintRange.SomePages)
                {
                    this.pages = Math.Min(this.pages, (int) ((base.PrinterSettings.ToPage - base.PrinterSettings.FromPage) + 1));
                    this.pageIndex = base.PrinterSettings.FromPage - 1;
                    this.startLine += (num1 * this.pageIndex);
                }
                this.pages = Math.Max(this.pages, 1);
                this.startPrinting = false;
            }
            Rectangle rectangle2 = rectangle1;
            num1 = this.syntaxEdit.Painter.FontHeight;
            if (num1 != 0)
            {
                rectangle2.Height = (rectangle2.Height / num1) * num1;
            }
            float single1 = ev.Graphics.DpiX / 100f;
            float single2 = ev.Graphics.DpiY / 100f;
            ITextPainter painter1 = this.syntaxEdit.Painter;
            painter1.BeginPaint(ev.Graphics);
            try
            {
                painter1.Transform(0, 0, single1, single2);
                try
                {
                    if ((this.printing.Options & PrintOptions.UseHeader) != PrintOptions.None)
                    {
                        Rectangle rectangle3 = new Rectangle(rectangle1.Left - this.printing.Header.Offset.X, rectangle1.Top - (this.printing.Header.Font.Height + this.printing.Header.Offset.Y), rectangle1.Width + (this.printing.Header.Offset.X * 2), this.printing.Header.Font.Height);
                        this.printing.Header.Paint(painter1, rectangle3, this.pageIndex, this.pageCount, false);
                    }
                    if (((this.printing.Options & PrintOptions.UseFooter) != PrintOptions.None) || ((this.printing.Options & PrintOptions.PageNumbers) != PrintOptions.None))
                    {
                        Rectangle rectangle4 = new Rectangle(rectangle1.Left - this.printing.Footer.Offset.X, rectangle1.Bottom + this.printing.Footer.Offset.Y, rectangle1.Width + (this.printing.Footer.Offset.X * 2), this.printing.Footer.Font.Height);
                        this.printing.Footer.Paint(painter1, rectangle4, this.pageIndex, this.pageCount, (this.printing.Options & PrintOptions.PageNumbers) != PrintOptions.None);
                    }
                }
                finally
                {
                    painter1.EndTransform();
                }
                ((SyntaxEdit) this.syntaxEdit).PaintWindow(painter1, this.startLine, new Rectangle(new Point(0, 0), rectangle2.Size), rectangle1.Location, single1, single2, true);
                this.startLine += Math.Max(this.syntaxEdit.LinesInHeight(), 1);
                this.page++;
                this.pageIndex++;
                ev.HasMorePages = this.page < this.pages;
            }
            finally
            {
                painter1.EndPaint(ev.Graphics);
            }
        }


        // Fields
        private int page;
        private int pageCount;
        private int pageIndex;
        private int pages;
        private IPrinting printing;
        private int startLine;
        private bool startPrinting;
        private ISyntaxEdit syntaxEdit;
    }
}

