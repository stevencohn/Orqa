namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Printing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class EditPages : IEditPages
    {
        // Events
        [Browsable(false)]
        public event DrawHeaderEvent DrawHeader;

        // Methods
        public EditPages()
        {
            this.updateCount = 0;
            this.displayWhiteSpace = true;
            this.backColor = EditConsts.DefaultPageBackColor;
            this.borderColor = EditConsts.DefaultPageBorderColor;
            this.pageType = River.Orqa.Editor.PageType.Normal;
            this.defaultMargins = new Margins(0, 0, 0, 0);
            this.items = new ArrayList();
            this.pageComparer = new PageComparer();
            this.pageTextComparer = new PageTextComparer();
            this.rulerUnits = River.Orqa.Editor.RulerUnits.Inches;
            this.rulerOptions = EditConsts.DefaultRulerOptions;
            this.caps = this.InitCaps();
        }

        public EditPages(ISyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        public IEditPage Add()
        {
            IEditPage page1 = new EditPage(this);
            this.Add(page1);
            return page1;
        }

        public int Add(IEditPage Page)
        {
            ((EditPage) Page).Pages = this;
            ((EditPage) Page).Index = this.Count;
            return this.items.Add(Page);
        }

        public int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public void CancelDragging()
        {
            if (this.horzRuler != null)
            {
                this.horzRuler.CancelDragging();
            }
            if (this.vertRuler != null)
            {
                this.vertRuler.CancelDragging();
            }
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        protected internal EditPage GetPage(int Index)
        {
            if (this.Count == 0)
            {
                this.Add();
            }
            if (Index < 0)
            {
                Index = 0;
            }
            while (Index >= this.Count)
            {
                this.Add().Assign(this[this.Count - 2]);
            }
            return (this[Index] as EditPage);
        }

        public IEditPage GetPageAt(Point Point)
        {
            return this.GetPage(this.GetPageIndexAt(Point));
        }

        public IEditPage GetPageAt(int X, int Y)
        {
            return this.GetPage(this.GetPageIndexAt(X, Y));
        }

        public IEditPage GetPageAtPoint(Point Point)
        {
            return this.GetPage(this.GetPageIndexAtPoint(Point));
        }

        public IEditPage GetPageAtPoint(int X, int Y)
        {
            return this.GetPage(this.GetPageIndexAtPoint(X, Y));
        }

        public int GetPageIndexAt(Point Point)
        {
            return this.GetPageIndexAt(Point.X, Point.Y);
        }

        public int GetPageIndexAt(int X, int Y)
        {
            int num1 = this.items.BinarySearch(Y, this.pageTextComparer);
            if (num1 >= 0)
            {
                return num1;
            }
            if (Y < 0)
            {
                return 0;
            }
            return (this.Count - 1);
        }

        public int GetPageIndexAtPoint(Point Point)
        {
            return this.GetPageIndexAtPoint(Point.X, Point.Y);
        }

        public int GetPageIndexAtPoint(int X, int Y)
        {
            int num1 = this.items.BinarySearch(Y, this.pageComparer);
            if (num1 >= 0)
            {
                return num1;
            }
            if ((this.Count > 0) && (Y < ((EditPage) this[0]).PageRect.Top))
            {
                return 0;
            }
            return (this.Count - 1);
        }

        private Size InitCaps()
        {
            Size size1;
            IntPtr ptr1 = Win32.GetDC(IntPtr.Zero);
            try
            {
                size1 = new Size(Win32.GetDeviceCaps(ptr1, 0x58), Win32.GetDeviceCaps(ptr1, 90));
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, ptr1);
            }
            return size1;
        }

        public void InitDefaultPageSettings(PageSettings PageSettings)
        {
            this.InitDefaultPageSettings(PageSettings, true);
        }

        protected internal void InitDefaultPageSettings(PageSettings PageSettings, bool UpdateSize)
        {
            if (UpdateSize)
            {
                try
                {
                    PaperSize size1 = PageSettings.PaperSize;
                    this.defaultPageSize = new Size(size1.Width, size1.Height);
                    this.defaultPageKind = size1.Kind;
                }
                catch
                {
                }
            }
            else
            {
                this.defaultPageSize = new Size((0x33b * this.caps.Width) / 100, (0x491 * this.caps.Height) / 100);
                this.defaultPageKind = PaperKind.Custom;
            }
            this.defaultMargins = PageSettings.Margins;
            this.defaultLandscape = PageSettings.Landscape;
            if (this.Count > 0)
            {
                this.BeginUpdate();
                try
                {
                    for (int num1 = 0; num1 < this.Count; num1++)
                    {
                        IEditPage page1 = this[num1];
                        page1.Margins = this.defaultMargins;
                        page1.PageKind = this.defaultPageKind;
                        page1.PageSize = this.defaultPageSize;
                        page1.Landscape = this.defaultLandscape;
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        protected internal void InitDefaultPrinterSettings(PrinterSettings PrinterSettings)
        {
            this.printerSettings = PrinterSettings;
            this.InitDefaultPageSettings(this.printerSettings.DefaultPageSettings, false);
        }

        public void Invalidate(IEditPage Page)
        {
            if (this.updateCount == 0)
            {
                if (Page != null)
                {
                    this.owner.Invalidate(((EditPage) Page).PageRect);
                }
                else
                {
                    this.owner.Invalidate();
                }
            }
        }

        public void OnDrawHeader(ref string Text)
        {
            if (this.DrawHeader != null)
            {
                DrawHeaderEventArgs args1 = new DrawHeaderEventArgs(Text);
                this.DrawHeader(this, args1);
                if (args1.Handled)
                {
                    Text = args1.Text;
                }
            }
        }

        public void Paint(ITextPainter Painter, Rectangle Rect)
        {
            if (this.Count != 0)
            {
                int num1 = 0;
                this.transparent = (this.owner != null) && this.owner.Transparent;
                for (int num2 = Math.Max(this.GetPageIndexAtPoint(Rect.Location), 0); num2 < this.Count; num2++)
                {
                    IEditPage page1 = this[num2];
                    page1.Paint(Painter);
                    num1 = page1.BoundsRect.Bottom + 1;
                    if (this.displayWhiteSpace)
                    {
                        num1 += 2;
                    }
                    if (num1 >= Rect.Bottom)
                    {
                        return;
                    }
                }
                if ((this.pageType == River.Orqa.Editor.PageType.PageLayout) && (num1 < Rect.Bottom))
                {
                    Rectangle rectangle1 = new Rectangle(Rect.Left, num1, Rect.Width, Rect.Bottom - num1);
                    Color color1 = Painter.BkColor;
                    Painter.BkColor = this.BackColor;
                    try
                    {
                        Painter.FillRectangle(rectangle1);
                    }
                    finally
                    {
                        Painter.BkColor = color1;
                    }
                }
            }
        }

        protected void RecalculatePages()
        {
            this.UpdatePageSize();
            this.Update();
            this.Invalidate(null);
            if (this.owner != null)
            {
                ((SyntaxEdit) this.owner).UpdateCaret();
            }
        }

        public void Remove(IEditPage Page)
        {
            ((EditPage) Page).Pages = null;
            this.items.Remove(Page);
        }

        public void ResetBackColor()
        {
            this.BackColor = EditConsts.DefaultPageBackColor;
        }

        public void ResetBorderColor()
        {
            this.BorderColor = EditConsts.DefaultPageBorderColor;
        }

        public void ResetDisplayWhiteSpace()
        {
            this.DisplayWhiteSpace = true;
        }

        public void ResetPageType()
        {
            this.PageType = River.Orqa.Editor.PageType.Normal;
        }

        public void ResetRulerOptions()
        {
            this.RulerOptions = EditConsts.DefaultRulerOptions;
        }

        public void ResetRulers()
        {
            this.Rulers = EditRulers.None;
        }

        public void ResetRulerUnits()
        {
            this.RulerUnits = River.Orqa.Editor.RulerUnits.Inches;
        }

        protected virtual void RulerChanged(object sender, EventArgs e)
        {
            if ((this.owner != null) && (sender is IEditRuler))
            {
                int num1;
                int num2;
                if (this.owner.Selection.IsEmpty())
                {
                    num1 = this.GetPageIndexAt(this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position));
                    num2 = num1;
                }
                else
                {
                    Rectangle rectangle1 = this.owner.Selection.SelectionRect;
                    num1 = this.GetPageIndexAt(this.owner.DisplayLines.PointToDisplayPoint(rectangle1.Location));
                    num2 = this.GetPageIndexAt(this.owner.DisplayLines.PointToDisplayPoint(rectangle1.Right, rectangle1.Bottom));
                }
                this.BeginUpdate();
                try
                {
                    IEditRuler ruler1 = (IEditRuler) sender;
                    RulerIndent indent1 = ((RulerEventArgs) e).Object as RulerIndent;
                    for (int num3 = Math.Max(num1, 0); num3 <= num2; num3++)
                    {
                        EditPage page1 = this[num3] as EditPage;
                        if (ruler1.Vertical)
                        {
                            if (indent1.Orientation == IndentOrientation.Near)
                            {
                                page1.TopIndent = indent1.Indent;
                            }
                            else
                            {
                                page1.BottomIndent = indent1.Indent;
                            }
                        }
                        else if (indent1.Orientation == IndentOrientation.Near)
                        {
                            page1.LeftIndent = indent1.Indent;
                        }
                        else
                        {
                            page1.RightIndent = indent1.Indent;
                        }
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.backColor != EditConsts.DefaultPageBackColor);
        }

        public bool ShouldSerializeBorderColor()
        {
            return (this.borderColor != EditConsts.DefaultPageBorderColor);
        }

        public bool ShouldSerializeRulerOptions()
        {
            return (this.rulerOptions != EditConsts.DefaultRulerOptions);
        }

        public void Update()
        {
            this.UpdatePages(0, 0x7fffffff, true);
            this.Invalidate(null);
        }

        public void Update(IEditPage Page)
        {
            this.Update(Page, false);
        }

        public void Update(IEditPage Page, bool Changed)
        {
            this.UpdatePages(Page.Index, 0x7fffffff, Changed);
        }

        protected internal void UpdatePages(int StartIndex, int EndIndex, bool Changed)
        {
            if (((this.updateCount <= 0) || !Changed) && (this.pageType != River.Orqa.Editor.PageType.Normal))
            {
                this.updateCount++;
                try
                {
                    Rectangle rectangle1;
                    int num1 = Math.Max(StartIndex, 0);
                    int num2 = this.owner.DisplayLines.GetCount();
                    int num3 = 0;
                    bool flag1 = this.pageType == River.Orqa.Editor.PageType.PageLayout;
                    bool flag2 = this.displayWhiteSpace && flag1;
                    int num4 = 0;
                    if (num1 > 0)
                    {
                        EditPage page1 = (EditPage) this[num1 - 1];
                        rectangle1 = page1.GetBounds(true);
                        num4 = rectangle1.Bottom;
                        num3 = page1.EndLine + 1;
                    }
                    if (Changed)
                    {
                        this.owner.UpdateWordWrap(num3, 0x7fffffff);
                    }
                    int num5 = this.owner.Painter.FontHeight;
                    while (((num3 < num2) || (num2 == 0)) && (num1 <= EndIndex))
                    {
                        EditPage page2 = this.GetPage(num1);
                        page2.StartLine = num3;
                        int num6 = Math.Max((num5 > 0) ? (page2.ClientRect.Height / num5) : 1, 1);
                        page2.EndLine = (num3 + num6) - 1;
                        int num7 = ((this.rulers & EditRulers.Vertical) != EditRulers.None) ? EditConsts.DefaultRulerHeight : 0;
                        int num8 = ((num1 == 0) && ((this.rulers & EditRulers.Horizonal) != EditRulers.None)) ? EditConsts.DefaultRulerHeight : 0;
                        page2.Origin = new Point((flag1 ? page2.HorzOffset : 0) + num7, (num4 + (flag2 ? page2.VertOffset : (((num1 == 0) && flag1) ? 4 : 0))) + num8);
                        rectangle1 = page2.GetBounds(true);
                        num4 += (rectangle1.Height + num8);
                        num3 += num6;
                        if (num3 >= num2)
                        {
                            break;
                        }
                        num1++;
                    }
                    if ((EndIndex == 0x7fffffff) && ((DisplayStrings) this.owner.DisplayLines).AlreadyScanned())
                    {
                        for (int num9 = this.Count - 1; num9 > num1; num9--)
                        {
                            this.items.RemoveAt(num9);
                        }
                    }
                    if (Changed)
                    {
                        this.UpdateRulers();
                    }
                }
                finally
                {
                    this.updateCount--;
                }
            }
        }

        protected internal void UpdatePageSize()
        {
            if ((!this.initialized && (this.printerSettings != null)) && ((this.PageType != River.Orqa.Editor.PageType.Normal) || (this.rulers != EditRulers.None)))
            {
                this.InitDefaultPageSettings(this.printerSettings.DefaultPageSettings, true);
                this.initialized = true;
            }
        }

        protected internal void UpdatePageSize(PaperKind PageKind, ref Size Size)
        {
            if ((this.updateCount == 0) && (this.printerSettings != null))
            {
                foreach (PaperSize size1 in this.printerSettings.PaperSizes)
                {
                    if (size1.Kind == PageKind)
                    {
                        Size = new Size(size1.Width, size1.Height);
                        return;
                    }
                }
            }
        }

        protected internal void UpdateRulers()
        {
            if ((this.owner != null) && (this.rulers != EditRulers.None))
            {
                IEditPage page1 = this.GetPageAt(this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position));
                if (page1 != null)
                {
                    Rectangle rectangle1 = page1.BoundsRect;
                    Rectangle rectangle2 = page1.ClientRect;
                    if ((this.rulers & EditRulers.Horizonal) != EditRulers.None)
                    {
                        IEditRuler ruler1 = this.HorzRuler;
                        ruler1.RulerStart = rectangle1.Left - ruler1.Left;
                        ruler1.RulerWidth = rectangle1.Width;
                        ruler1.PageStart = rectangle2.Left - ruler1.Left;
                        ruler1.PageWidth = rectangle2.Width;
                        ruler1.MarkWidth = this.owner.Painter.FontWidth;
                        ruler1.Update();
                    }
                    if ((this.rulers & EditRulers.Vertical) != EditRulers.None)
                    {
                        IEditRuler ruler2 = this.VertRuler;
                        ruler2.RulerStart = rectangle1.Top - ruler2.Top;
                        ruler2.RulerWidth = rectangle1.Height;
                        ruler2.PageStart = rectangle2.Top - ruler2.Top;
                        ruler2.PageWidth = rectangle2.Height;
                        ruler2.MarkWidth = this.owner.Painter.FontWidth;
                        ruler2.Update();
                    }
                }
            }
        }


        // Properties
        public Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.owner.Invalidate();
                }
            }
        }

        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                if (this.borderColor != value)
                {
                    this.borderColor = value;
                    this.owner.Invalidate();
                }
            }
        }

        protected internal Size Caps
        {
            get
            {
                return this.caps;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        protected internal bool DefaultLandscape
        {
            get
            {
                return this.defaultLandscape;
            }
        }

        protected internal Margins DefaultMargins
        {
            get
            {
                return this.defaultMargins;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public IEditPage DefaultPage
        {
            get
            {
                if (this.Count == 0)
                {
                    this.Add();
                }
                return this[0];
            }
            set
            {
                this.DefaultPage.Assign(value);
            }
        }

        protected internal PaperKind DefaultPageKind
        {
            get
            {
                return this.defaultPageKind;
            }
        }

        protected internal Size DefaultPageSize
        {
            get
            {
                return this.defaultPageSize;
            }
        }

        [DefaultValue(true)]
        public bool DisplayWhiteSpace
        {
            get
            {
                return this.displayWhiteSpace;
            }
            set
            {
                if (this.displayWhiteSpace != value)
                {
                    this.displayWhiteSpace = value;
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int Height
        {
            get
            {
                IEditPage page1 = (this.Count > 0) ? this[this.Count - 1] : this.DefaultPage;
                return ((page1.Origin.Y + page1.BoundsRect.Height) + page1.VertOffset);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IEditRuler HorzRuler
        {
            get
            {
                if (this.horzRuler == null)
                {
                    this.horzRuler = new EditRuler();
                    this.horzRuler.Visible = false;
                    this.horzRuler.Parent = (SyntaxEdit) this.owner;
                    this.horzRuler.Location = new Point(0, 0);
                    this.horzRuler.Width = this.owner.ClientRect.Width;
                    ((EditRuler) this.horzRuler).Anchor = AnchorStyles.Right | (AnchorStyles.Left | AnchorStyles.Top);
                    this.horzRuler.Change += new EventHandler(this.RulerChanged);
                }
                return this.horzRuler;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEditPage this[int Index]
        {
            get
            {
                return (this.items[Index] as IEditPage);
            }
            set
            {
                if (this.items[Index] != value)
                {
                    this.items[Index] = value;
                    ((EditPage) value).Pages = this;
                    this.Update();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList Items
        {
            get
            {
                return this.items;
            }
        }

        protected internal ISyntaxEdit Owner
        {
            get
            {
                return this.owner;
            }
        }

        [DefaultValue(0)]
        public River.Orqa.Editor.PageType PageType
        {
            get
            {
                return this.pageType;
            }
            set
            {
                if (this.pageType != value)
                {
                    this.pageType = value;
                    this.RecalculatePages();
                }
            }
        }

        protected internal Color RulerBackColor
        {
            get
            {
                if (this.horzRuler != null)
                {
                    return this.horzRuler.BackColor;
                }
                if (this.vertRuler != null)
                {
                    return this.vertRuler.BackColor;
                }
                return EditConsts.DefaultRulerBackColor;
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public River.Orqa.Editor.RulerOptions RulerOptions
        {
            get
            {
                return this.rulerOptions;
            }
            set
            {
                if (this.rulerOptions != value)
                {
                    this.rulerOptions = value;
                    if (this.horzRuler != null)
                    {
                        this.horzRuler.Options = value;
                    }
                    if (this.vertRuler != null)
                    {
                        this.vertRuler.Options = value;
                    }
                }
            }
        }

        [DefaultValue(0), Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public EditRulers Rulers
        {
            get
            {
                return this.rulers;
            }
            set
            {
                if (this.rulers != value)
                {
                    this.rulers = value;
                    if ((this.rulers & EditRulers.Horizonal) != EditRulers.None)
                    {
                        this.HorzRuler.Visible = true;
                    }
                    else if (this.horzRuler != null)
                    {
                        this.horzRuler.Visible = false;
                    }
                    if ((this.rulers & EditRulers.Vertical) != EditRulers.None)
                    {
                        this.VertRuler.Visible = true;
                    }
                    else if (this.vertRuler != null)
                    {
                        this.vertRuler.Visible = false;
                    }
                    if (((this.rulers & EditRulers.Horizonal) != EditRulers.None) && ((this.rulers & EditRulers.Vertical) != EditRulers.None))
                    {
                        Rectangle rectangle1 = this.owner.ClientRect;
                        this.VertRuler.Top = this.HorzRuler.Height;
                        this.VertRuler.Height = rectangle1.Height;
                        this.HorzRuler.Left = this.VertRuler.Width;
                        this.HorzRuler.Width = rectangle1.Width;
                    }
                    this.RecalculatePages();
                }
            }
        }

        [DefaultValue(1)]
        public River.Orqa.Editor.RulerUnits RulerUnits
        {
            get
            {
                return this.rulerUnits;
            }
            set
            {
                if (this.rulerUnits != value)
                {
                    this.rulerUnits = value;
                    if (this.horzRuler != null)
                    {
                        this.horzRuler.Units = value;
                    }
                    if (this.vertRuler != null)
                    {
                        this.vertRuler.Units = value;
                    }
                }
            }
        }

        protected internal bool Transparent
        {
            get
            {
                return this.transparent;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEditRuler VertRuler
        {
            get
            {
                if (this.vertRuler == null)
                {
                    this.vertRuler = new EditRuler();
                    this.vertRuler.Vertical = true;
                    this.vertRuler.Visible = false;
                    this.vertRuler.Parent = (SyntaxEdit) this.owner;
                    this.vertRuler.Change += new EventHandler(this.RulerChanged);
                    this.vertRuler.Location = new Point(0, 0);
                    this.vertRuler.Height = this.owner.ClientRect.Height;
                    ((EditRuler) this.vertRuler).Anchor = AnchorStyles.Left | (AnchorStyles.Bottom | AnchorStyles.Top);
                }
                return this.vertRuler;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int Width
        {
            get
            {
                return (this.DefaultPage.BoundsRect.Width + (this.DefaultPage.HorzOffset * 2));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlEditPagesInfo(this);
            }
            set
            {
                ((XmlEditPagesInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private Color backColor;
        private Color borderColor;
        private Size caps;
        private bool defaultLandscape;
        private Margins defaultMargins;
        private PaperKind defaultPageKind;
        private Size defaultPageSize;
        private bool displayWhiteSpace;
        private IEditRuler horzRuler;
        private bool initialized;
        private ArrayList items;
        private ISyntaxEdit owner;
        private IComparer pageComparer;
        private IComparer pageTextComparer;
        private River.Orqa.Editor.PageType pageType;
        private PrinterSettings printerSettings;
        private River.Orqa.Editor.RulerOptions rulerOptions;
        private EditRulers rulers;
        private River.Orqa.Editor.RulerUnits rulerUnits;
        private bool transparent;
        private int updateCount;
        private IEditRuler vertRuler;

        // Nested Types
        private class PageComparer : IComparer
        {
            // Methods
            public PageComparer()
            {
            }

            public int Compare(object x, object y)
            {
                Rectangle rectangle1 = ((EditPage) x).PageRect;
                int num1 = (int) y;
                if (rectangle1.Top > num1)
                {
                    return 1;
                }
                if (rectangle1.Bottom <= num1)
                {
                    return -1;
                }
                return 0;
            }

        }

        private class PageTextComparer : IComparer
        {
            // Methods
            public PageTextComparer()
            {
            }

            public int Compare(object x, object y)
            {
                EditPage page1 = (EditPage) x;
                int num1 = (int) y;
                if (page1.StartLine > num1)
                {
                    return 1;
                }
                if (page1.EndLine < num1)
                {
                    return -1;
                }
                return 0;
            }

        }
    }
}

