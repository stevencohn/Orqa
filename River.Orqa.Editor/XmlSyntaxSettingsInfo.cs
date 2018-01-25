namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Dialogs;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class XmlSyntaxSettingsInfo
    {
        // Methods
        public XmlSyntaxSettingsInfo()
        {
            this.navigateOptions = EditConsts.DefaultNavigateOptions;
            this.scrollBars = RichTextBoxScrollBars.Both;
            this.selectionOptions = EditConsts.DefaultSelectionOptions;
            this.gutterOptions = EditConsts.DefaultGutterOptions;
            this.outlineOptions = EditConsts.DefaultOutlineOptions;
            this.showGutter = true;
            this.showMargin = true;
            this.highlightUrls = true;
            this.gutterWidth = EditConsts.DefaultGutterWidth;
            this.marginPos = EditConsts.DefaultMarginPosition;
            int[] numArray1 = new int[1] { EditConsts.DefaultTabStop } ;
            this.tabStops = numArray1;
            LexStyle[] styleArray1 = new LexStyle[0];
            this.lexStyles = styleArray1;
            this.fontName = FontFamily.GenericMonospace.Name;
            this.fontSize = 10f;
            this.fontStyle = System.Drawing.FontStyle.Regular;
        }

        public XmlSyntaxSettingsInfo(SyntaxSettings Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(SyntaxSettings Owner)
        {
            this.owner = Owner;
            this.Font = new System.Drawing.Font(this.fontName, this.fontSize, this.fontStyle);
            this.NavOptions = this.navigateOptions;
            this.ScrollBars = this.scrollBars;
            this.SelOptions = this.selectionOptions;
            this.GutterOptions = this.gutterOptions;
            this.OutlineOptions = this.outlineOptions;
            this.ShowGutter = this.showGutter;
            this.ShowMargin = this.showMargin;
            this.HighlightUrls = this.highlightUrls;
            this.AllowOutlining = this.allowOutlining;
            this.UseSpaces = this.useSpaces;
            this.WordWrap = this.wordWrap;
            this.GutterWidth = this.gutterWidth;
            this.MarginPos = this.marginPos;
            this.TabStops = this.tabStops;
            this.LexStyles = this.lexStyles;
        }


        // Properties
        public bool AllowOutlining
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowOutlining;
                }
                return this.owner.AllowOutlining;
            }
            set
            {
                this.allowOutlining = value;
                if (this.owner != null)
                {
                    this.owner.AllowOutlining = value;
                }
            }
        }

        [XmlIgnore]
        public System.Drawing.Font Font
        {
            get
            {
                if (this.owner == null)
                {
                    return this.font;
                }
                return this.owner.Font;
            }
            set
            {
                this.font = value;
                if (this.owner != null)
                {
                    this.owner.Font = value;
                }
            }
        }

        public string FontName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontName;
                }
                return this.owner.Font.Name;
            }
            set
            {
                this.fontName = value;
            }
        }

        public float FontSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontSize;
                }
                return this.owner.Font.Size;
            }
            set
            {
                this.fontSize = value;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontStyle;
                }
                return this.owner.Font.Style;
            }
            set
            {
                this.fontStyle = value;
            }
        }

        public River.Orqa.Editor.GutterOptions GutterOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.gutterOptions;
                }
                return this.owner.GutterOptions;
            }
            set
            {
                this.gutterOptions = value;
                if (this.owner != null)
                {
                    this.owner.GutterOptions = value;
                }
            }
        }

        public int GutterWidth
        {
            get
            {
                if (this.owner == null)
                {
                    return this.gutterWidth;
                }
                return this.owner.GutterWidth;
            }
            set
            {
                this.gutterWidth = value;
                if (this.owner != null)
                {
                    this.owner.GutterWidth = value;
                }
            }
        }

        public bool HighlightUrls
        {
            get
            {
                if (this.owner == null)
                {
                    return this.highlightUrls;
                }
                return this.owner.HighlightUrls;
            }
            set
            {
                this.highlightUrls = value;
                if (this.owner != null)
                {
                    this.owner.HighlightUrls = value;
                }
            }
        }

        public LexStyle[] LexStyles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lexStyles;
                }
                return this.owner.LexStyles;
            }
            set
            {
                this.lexStyles = value;
                if (this.owner != null)
                {
                    this.owner.LexStyles = new LexStyle[value.Length];
                    Array.Copy(value, this.owner.LexStyles, value.Length);
                }
            }
        }

        public int MarginPos
        {
            get
            {
                if (this.owner == null)
                {
                    return this.marginPos;
                }
                return this.owner.MarginPos;
            }
            set
            {
                this.marginPos = value;
                if (this.owner != null)
                {
                    this.owner.MarginPos = value;
                }
            }
        }

        public NavigateOptions NavOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.navigateOptions;
                }
                return this.owner.NavigateOptions;
            }
            set
            {
                this.navigateOptions = value;
                if (this.owner != null)
                {
                    this.owner.NavigateOptions = value;
                }
            }
        }

        public River.Orqa.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlineOptions;
                }
                return this.owner.OutlineOptions;
            }
            set
            {
                this.outlineOptions = value;
                if (this.owner != null)
                {
                    this.owner.OutlineOptions = value;
                }
            }
        }

        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scrollBars;
                }
                return this.owner.ScrollBars;
            }
            set
            {
                this.scrollBars = value;
                if (this.owner != null)
                {
                    this.owner.ScrollBars = value;
                }
            }
        }

        public SelectionOptions SelOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selectionOptions;
                }
                return this.owner.SelectionOptions;
            }
            set
            {
                this.selectionOptions = value;
                if (this.owner != null)
                {
                    this.owner.SelectionOptions = value;
                }
            }
        }

        public bool ShowGutter
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showGutter;
                }
                return this.owner.ShowGutter;
            }
            set
            {
                this.showGutter = value;
                if (this.owner != null)
                {
                    this.owner.ShowGutter = value;
                }
            }
        }

        public bool ShowMargin
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showMargin;
                }
                return this.owner.ShowMargin;
            }
            set
            {
                this.showMargin = value;
                if (this.owner != null)
                {
                    this.owner.ShowMargin = value;
                }
            }
        }

        public int[] TabStops
        {
            get
            {
                if (this.owner == null)
                {
                    return this.tabStops;
                }
                return this.owner.TabStops;
            }
            set
            {
                this.tabStops = value;
                if (this.owner != null)
                {
                    this.owner.TabStops = new int[value.Length];
                    Array.Copy(value, this.owner.TabStops, value.Length);
                }
            }
        }

        public bool UseSpaces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.useSpaces;
                }
                return this.owner.UseSpaces;
            }
            set
            {
                this.useSpaces = value;
                if (this.owner != null)
                {
                    this.owner.UseSpaces = value;
                }
            }
        }

        public bool WordWrap
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wordWrap;
                }
                return this.owner.WordWrap;
            }
            set
            {
                this.wordWrap = value;
                if (this.owner != null)
                {
                    this.owner.WordWrap = value;
                }
            }
        }


        // Fields
        private bool allowOutlining;
        private System.Drawing.Font font;
        private string fontName;
        private float fontSize;
        private System.Drawing.FontStyle fontStyle;
        private River.Orqa.Editor.GutterOptions gutterOptions;
        private int gutterWidth;
        private bool highlightUrls;
        private LexStyle[] lexStyles;
        private int marginPos;
        private NavigateOptions navigateOptions;
        private River.Orqa.Editor.OutlineOptions outlineOptions;
        private SyntaxSettings owner;
        private RichTextBoxScrollBars scrollBars;
        private SelectionOptions selectionOptions;
        private bool showGutter;
        private bool showMargin;
        private int[] tabStops;
        private bool useSpaces;
        private bool wordWrap;
    }
}

