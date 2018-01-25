namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SyntaxSettings : PersistentSettings, ISyntaxSettings, IPersistentSettings
    {
        // Methods
        public SyntaxSettings()
        {
            this.lexStyles = null;
            this.showMargin = true;
            this.showGutter = true;
            this.highlightUrls = true;
            this.gutterWidth = EditConsts.DefaultGutterWidth;
            this.marginPos = EditConsts.DefaultMarginPosition;
            int[] numArray1 = new int[1] { EditConsts.DefaultTabStop } ;
            this.tabStops = numArray1;
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f);
            this.navigateOptions = EditConsts.DefaultNavigateOptions;
            this.scrollBars = RichTextBoxScrollBars.Both;
            this.selectionOptions = EditConsts.DefaultSelectionOptions;
            this.gutterOptions = EditConsts.DefaultGutterOptions;
            this.outlineOptions = EditConsts.DefaultOutlineOptions;
            this.lexStyles = new LexStyle[LexStyleItems.Items.Length];
            for (int num1 = 0; num1 < this.lexStyles.Length; num1++)
            {
                LexStyle style1 = new LexStyle();
                LexStyleItem item1 = LexStyleItems.Items[num1];
                style1.Name = item1.InternalName;
                style1.Desc = item1.Name;
                style1.ForeColor = item1.ForeColor;
                style1.BackColor = item1.BackColor;
                style1.FontStyle = item1.FontStyle;
                style1.PlainText = item1.PlainText;
                this.lexStyles[num1] = style1;
            }
        }

        public void ApplyToEdit(SyntaxEdit SyntaxEdit)
        {
            SyntaxEdit.Source.BeginUpdate(UpdateReason.Other);
            try
            {
                SyntaxEdit.Outlining.AllowOutlining = this.AllowOutlining;
                SyntaxEdit.Outlining.OutlineOptions = this.OutlineOptions;
                SyntaxEdit.Font = this.Font;
                SyntaxEdit.Gutter.Options = this.GutterOptions;
                SyntaxEdit.Gutter.Width = this.GutterWidth;
                SyntaxEdit.Margin.Position = this.MarginPos;
                SyntaxEdit.NavigateOptions = this.NavigateOptions;
                SyntaxEdit.Scrolling.ScrollBars = this.ScrollBars;
                SyntaxEdit.Selection.Options = this.SelectionOptions;
                SyntaxEdit.Gutter.Visible = this.ShowGutter;
                SyntaxEdit.Margin.Visible = this.ShowMargin;
                SyntaxEdit.Source.Lines.TabStops = this.TabStops;
                SyntaxEdit.Source.Lines.UseSpaces = this.UseSpaces;
                SyntaxEdit.WordWrap = this.WordWrap;
                this.CopyStyles(this.LexStyles, this.GetLexStyles(SyntaxEdit));
                SyntaxEdit.Spelling.SpellColor = this.LexStyles[this.LexStyles.Length - 1].ForeColor;
                SyntaxEdit.HyperText.HighlightUrls = this.HighlightUrls;
                SyntaxEdit.HyperText.UrlColor = this.LexStyles[this.LexStyles.Length - 2].ForeColor;
                SyntaxEdit.HyperText.UrlStyle = this.LexStyles[this.LexStyles.Length - 2].FontStyle;
                SyntaxEdit.Gutter.LineNumbersForeColor = this.lexStyles[this.LexStyles.Length - 3].ForeColor;
            }
            finally
            {
                SyntaxEdit.Source.EndUpdate();
            }
            SyntaxEdit.Invalidate();
        }

        public override void Assign(IPersistentSettings Source)
        {
            if (Source is SyntaxSettings)
            {
                SyntaxSettings settings1 = (SyntaxSettings) Source;
                this.font = new System.Drawing.Font(settings1.Font, settings1.Font.Style);
                this.navigateOptions = settings1.navigateOptions;
                this.scrollBars = settings1.scrollBars;
                this.selectionOptions = settings1.selectionOptions;
                this.gutterOptions = settings1.GutterOptions;
                this.outlineOptions = settings1.OutlineOptions;
                this.showMargin = settings1.showMargin;
                this.showGutter = settings1.showGutter;
                this.highlightUrls = settings1.highlightUrls;
                this.allowOutlining = settings1.allowOutlining;
                this.useSpaces = settings1.useSpaces;
                this.wordWrap = settings1.wordWrap;
                this.gutterWidth = settings1.GutterWidth;
                this.marginPos = settings1.marginPos;
                this.TabStops = settings1.TabStops;
                for (int num1 = 0; num1 < settings1.LexStyles.Length; num1++)
                {
                    this.LexStyles[num1].Assign(settings1.LexStyles[num1]);
                }
            }
        }

        private void CopyStyles(ILexStyle[] FromStyles, ILexStyle[] ToStyles)
        {
            if ((FromStyles != null) && (ToStyles != null))
            {
                ILexStyle[] styleArray1 = FromStyles;
                for (int num2 = 0; num2 < styleArray1.Length; num2++)
                {
                    ILexStyle style2 = styleArray1[num2];
                    int num1 = this.GetStyleByName(ToStyles, style2.Name);
                    if (num1 >= 0)
                    {
                        ILexStyle style1 = ToStyles[num1];
                        style1.ForeColor = style2.ForeColor;
                        style1.BackColor = style2.BackColor;
                        style1.FontStyle = style2.FontStyle;
                    }
                }
            }
        }

        private ILexStyle[] GetLexStyles(ISyntaxEdit SyntaxEdit)
        {
            if (SyntaxEdit.Source.Lexer == null)
            {
                return null;
            }
            return SyntaxEdit.Source.Lexer.Scheme.Styles;
        }

        private int GetStyleByName(ILexStyle[] Styles, string Name)
        {
            for (int num1 = 0; num1 < Styles.Length; num1++)
            {
                if (string.Compare(Styles[num1].Name, Name, true) == 0)
                {
                    return num1;
                }
            }
            return -1;
        }

        public override System.Type GetXmlType()
        {
            return typeof(XmlSyntaxSettingsInfo);
        }

        public bool IsBackColorEnabled(int Index)
        {
            if (Index != (this.lexStyles.Length - 1))
            {
                return (Index != (this.lexStyles.Length - 2));
            }
            return false;
        }

        public bool IsDescriptionEnabled(int Index)
        {
            if (Index != (this.lexStyles.Length - 1))
            {
                return (Index != (this.lexStyles.Length - 2));
            }
            return false;
        }

        public bool IsFontStyleEnabled(int Index)
        {
            return (Index != (this.lexStyles.Length - 1));
        }

        public void LoadFromEdit(SyntaxEdit SyntaxEdit)
        {
            this.Font = SyntaxEdit.Font;
            this.GutterOptions = SyntaxEdit.Gutter.Options;
            this.GutterWidth = SyntaxEdit.Gutter.Width;
            this.HighlightUrls = SyntaxEdit.HyperText.HighlightUrls;
            this.MarginPos = SyntaxEdit.Margin.Position;
            this.NavigateOptions = SyntaxEdit.NavigateOptions;
            this.AllowOutlining = SyntaxEdit.Outlining.AllowOutlining;
            this.OutlineOptions = SyntaxEdit.Outlining.OutlineOptions;
            this.ScrollBars = SyntaxEdit.Scrolling.ScrollBars;
            this.SelectionOptions = SyntaxEdit.Selection.Options;
            this.ShowGutter = SyntaxEdit.Gutter.Visible;
            this.ShowMargin = SyntaxEdit.Margin.Visible;
            this.TabStops = SyntaxEdit.Source.Lines.TabStops;
            this.UseSpaces = SyntaxEdit.Source.Lines.UseSpaces;
            this.WordWrap = SyntaxEdit.WordWrap;
            this.CopyStyles(this.GetLexStyles(SyntaxEdit), this.lexStyles);
            this.lexStyles[this.LexStyles.Length - 1].ForeColor = SyntaxEdit.Spelling.SpellColor;
            this.lexStyles[this.LexStyles.Length - 2].ForeColor = SyntaxEdit.HyperText.UrlColor;
            this.lexStyles[this.LexStyles.Length - 2].FontStyle = SyntaxEdit.HyperText.UrlStyle;
            this.lexStyles[this.LexStyles.Length - 3].ForeColor = SyntaxEdit.Gutter.LineNumbersForeColor;
        }


        // Properties
        public bool AllowOutlining
        {
            get
            {
                return this.allowOutlining;
            }
            set
            {
                this.allowOutlining = value;
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        public River.Orqa.Editor.GutterOptions GutterOptions
        {
            get
            {
                return this.gutterOptions;
            }
            set
            {
                this.gutterOptions = value;
            }
        }

        public int GutterWidth
        {
            get
            {
                return this.gutterWidth;
            }
            set
            {
                this.gutterWidth = value;
            }
        }

        public bool HighlightUrls
        {
            get
            {
                return this.highlightUrls;
            }
            set
            {
                this.highlightUrls = value;
            }
        }

        public LexStyle[] LexStyles
        {
            get
            {
                return this.lexStyles;
            }
            set
            {
                this.lexStyles = new LexStyle[value.Length];
                Array.Copy(value, this.lexStyles, value.Length);
            }
        }

        public int MarginPos
        {
            get
            {
                return this.marginPos;
            }
            set
            {
                this.marginPos = value;
            }
        }

        public River.Orqa.Editor.NavigateOptions NavigateOptions
        {
            get
            {
                return this.navigateOptions;
            }
            set
            {
                this.navigateOptions = value;
            }
        }

        public River.Orqa.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                return this.outlineOptions;
            }
            set
            {
                this.outlineOptions = value;
            }
        }

        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return this.scrollBars;
            }
            set
            {
                this.scrollBars = value;
            }
        }

        public River.Orqa.Editor.SelectionOptions SelectionOptions
        {
            get
            {
                return this.selectionOptions;
            }
            set
            {
                this.selectionOptions = value;
            }
        }

        public bool ShowGutter
        {
            get
            {
                return this.showGutter;
            }
            set
            {
                this.showGutter = value;
            }
        }

        public bool ShowMargin
        {
            get
            {
                return this.showMargin;
            }
            set
            {
                this.showMargin = value;
            }
        }

        public int[] TabStops
        {
            get
            {
                return this.tabStops;
            }
            set
            {
                this.tabStops = new int[value.Length];
                Array.Copy(value, this.tabStops, value.Length);
            }
        }

        public bool UseSpaces
        {
            get
            {
                return this.useSpaces;
            }
            set
            {
                this.useSpaces = value;
            }
        }

        public bool WordWrap
        {
            get
            {
                return this.wordWrap;
            }
            set
            {
                this.wordWrap = value;
            }
        }

        public override object XmlInfo
        {
            get
            {
                return new XmlSyntaxSettingsInfo(this);
            }
            set
            {
                ((XmlSyntaxSettingsInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private bool allowOutlining;
        private System.Drawing.Font font;
        private River.Orqa.Editor.GutterOptions gutterOptions;
        private int gutterWidth;
        private bool highlightUrls;
        private LexStyle[] lexStyles;
        private int marginPos;
        private River.Orqa.Editor.NavigateOptions navigateOptions;
        private River.Orqa.Editor.OutlineOptions outlineOptions;
        private RichTextBoxScrollBars scrollBars;
        private River.Orqa.Editor.SelectionOptions selectionOptions;
        private bool showGutter;
        private bool showMargin;
        private int[] tabStops;
        private bool useSpaces;
        private bool wordWrap;
    }
}

