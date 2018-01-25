namespace River.Orqa.Editor
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot("SyntaxEdit")]
    public class XmlSyntaxEditInfo
    {
        // Methods
        public XmlSyntaxEditInfo()
        {
            this.wantTabs = true;
            this.wantReturns = true;
        }

        public XmlSyntaxEditInfo(SyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(SyntaxEdit Owner)
        {
            this.owner = Owner;
            this.HideCaret = this.hideCaret;
            this.DisableColorPaint = this.disableColorPaint;
            this.DisableSyntaxPaint = this.disableSyntaxPaint;
            this.WantTabs = this.wantTabs;
            this.WantReturns = this.wantReturns;
            this.DisplayStrings = this.displayStrings;
            this.Selection = this.selection;
            this.Gutter = this.gutter;
            this.Margin = this.margin;
            this.LineStyles = this.lineStyles;
            this.LineSeparator = this.lineSeparator;
            this.Printing = this.printing;
            this.WhiteSpace = this.whiteSpace;
            this.TextSource = this.textSource;
            this.Scrolling = this.scrolling;
            this.Outlining = this.outlining;
            this.HyperText = this.hyperText;
            this.Spelling = this.spelling;
            this.Pages = this.pages;
            this.Transparent = this.transparent;
        }


        // Properties
        public bool DisableColorPaint
        {
            get
            {
                if (this.owner == null)
                {
                    return this.disableColorPaint;
                }
                return this.owner.DisableColorPaint;
            }
            set
            {
                this.disableColorPaint = value;
                if (this.owner != null)
                {
                    this.owner.DisableColorPaint = value;
                }
            }
        }

        public bool DisableSyntaxPaint
        {
            get
            {
                if (this.owner == null)
                {
                    return this.disableSyntaxPaint;
                }
                return this.owner.DisableSyntaxPaint;
            }
            set
            {
                this.disableSyntaxPaint = value;
                if (this.owner != null)
                {
                    this.owner.DisableSyntaxPaint = value;
                }
            }
        }

        public XmlDisplayStringsInfo DisplayStrings
        {
            get
            {
                if (this.owner == null)
                {
                    return this.displayStrings;
                }
                return (XmlDisplayStringsInfo) ((River.Orqa.Editor.DisplayStrings) this.owner.DisplayLines).XmlInfo;
            }
            set
            {
                this.displayStrings = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.DisplayStrings) this.owner.DisplayLines).XmlInfo = value;
                }
            }
        }

        public XmlGutterInfo Gutter
        {
            get
            {
                if (this.owner == null)
                {
                    return this.gutter;
                }
                return (XmlGutterInfo) ((River.Orqa.Editor.Gutter) this.owner.Gutter).XmlInfo;
            }
            set
            {
                this.gutter = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.Gutter) this.owner.Gutter).XmlInfo = value;
                }
            }
        }

        public bool HideCaret
        {
            get
            {
                if (this.owner == null)
                {
                    return this.hideCaret;
                }
                return this.owner.HideCaret;
            }
            set
            {
                this.hideCaret = value;
                if (this.owner != null)
                {
                    this.owner.HideCaret = value;
                }
            }
        }

        public XmlHyperTextExInfo HyperText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.hyperText;
                }
                return (XmlHyperTextExInfo) ((HyperTextEx) this.owner.HyperText).XmlInfo;
            }
            set
            {
                this.hyperText = value;
                if (this.owner != null)
                {
                    ((HyperTextEx) this.owner.HyperText).XmlInfo = value;
                }
            }
        }

        public XmlLineSeparatorInfo LineSeparator
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineSeparator;
                }
                return (XmlLineSeparatorInfo) ((River.Orqa.Editor.LineSeparator) this.owner.LineSeparator).XmlInfo;
            }
            set
            {
                this.lineSeparator = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.LineSeparator) this.owner.LineSeparator).XmlInfo = value;
                }
            }
        }

        public XmlLineStylesInfo LineStyles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineStyles;
                }
                return (XmlLineStylesInfo) ((LineStylesEx) this.owner.LineStyles).XmlInfo;
            }
            set
            {
                this.lineStyles = value;
                if (this.owner != null)
                {
                    ((LineStylesEx) this.owner.LineStyles).XmlInfo = value;
                }
            }
        }

        public XmlMarginInfo Margin
        {
            get
            {
                if (this.owner == null)
                {
                    return this.margin;
                }
                return (XmlMarginInfo) ((River.Orqa.Editor.Margin) this.owner.Margin).XmlInfo;
            }
            set
            {
                this.margin = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.Margin) this.owner.Margin).XmlInfo = value;
                }
            }
        }

        public XmlOutliningInfo Outlining
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlining;
                }
                return (XmlOutliningInfo) ((River.Orqa.Editor.Outlining) this.owner.Outlining).XmlInfo;
            }
            set
            {
                this.outlining = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.Outlining) this.owner.Outlining).XmlInfo = value;
                }
            }
        }

        public XmlEditPagesInfo Pages
        {
            get
            {
                if (this.owner == null)
                {
                    return this.Pages;
                }
                return (XmlEditPagesInfo) ((EditPages) this.owner.Pages).XmlInfo;
            }
            set
            {
                this.pages = value;
                if (this.owner != null)
                {
                    ((EditPages) this.owner.Pages).XmlInfo = value;
                }
            }
        }

        public XmlPrintingInfo Printing
        {
            get
            {
                if (this.owner == null)
                {
                    return this.printing;
                }
                return (XmlPrintingInfo) ((River.Orqa.Editor.Printing) this.owner.Printing).XmlInfo;
            }
            set
            {
                this.printing = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.Printing) this.owner.Printing).XmlInfo = value;
                }
            }
        }

        public XmlScrollingInfo Scrolling
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scrolling;
                }
                return (XmlScrollingInfo) ((River.Orqa.Editor.Scrolling) this.owner.Scrolling).XmlInfo;
            }
            set
            {
                this.scrolling = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.Scrolling) this.owner.Scrolling).XmlInfo = value;
                }
            }
        }

        public XmlSelectionInfo Selection
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selection;
                }
                return (XmlSelectionInfo) ((River.Orqa.Editor.Selection) this.owner.Selection).XmlInfo;
            }
            set
            {
                this.selection = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.Selection) this.owner.Selection).XmlInfo = value;
                }
            }
        }

        public XmlSpellingInfo Spelling
        {
            get
            {
                if (this.owner == null)
                {
                    return this.spelling;
                }
                return (XmlSpellingInfo) ((River.Orqa.Editor.Spelling) this.owner.Spelling).XmlInfo;
            }
            set
            {
                this.spelling = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.Spelling) this.owner.Spelling).XmlInfo = value;
                }
            }
        }

        public XmlTextSourceInfo TextSource
        {
            get
            {
                if (this.owner == null)
                {
                    return this.textSource;
                }
                return (XmlTextSourceInfo) ((River.Orqa.Editor.TextSource) this.owner.Source).XmlInfo;
            }
            set
            {
                this.textSource = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.TextSource) this.owner.Source).XmlInfo = value;
                }
            }
        }

        public bool Transparent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.transparent;
                }
                return this.owner.Transparent;
            }
            set
            {
                this.transparent = value;
                if (this.owner != null)
                {
                    this.owner.Transparent = value;
                }
            }
        }

        public bool WantReturns
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wantReturns;
                }
                return this.owner.WantReturns;
            }
            set
            {
                this.wantReturns = value;
                if (this.owner != null)
                {
                    this.owner.WantReturns = value;
                }
            }
        }

        public bool WantTabs
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wantTabs;
                }
                return this.owner.WantTabs;
            }
            set
            {
                this.wantTabs = value;
                if (this.owner != null)
                {
                    this.owner.WantTabs = value;
                }
            }
        }

        public XmlWhiteSpaceInfo WhiteSpace
        {
            get
            {
                if (this.owner == null)
                {
                    return this.whiteSpace;
                }
                return (XmlWhiteSpaceInfo) ((River.Orqa.Editor.WhiteSpace) this.owner.WhiteSpace).XmlInfo;
            }
            set
            {
                this.whiteSpace = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.WhiteSpace) this.owner.WhiteSpace).XmlInfo = value;
                }
            }
        }


        // Fields
        private bool disableColorPaint;
        private bool disableSyntaxPaint;
        private XmlDisplayStringsInfo displayStrings;
        private XmlGutterInfo gutter;
        private bool hideCaret;
        private XmlHyperTextExInfo hyperText;
        private XmlLineSeparatorInfo lineSeparator;
        private XmlLineStylesInfo lineStyles;
        private XmlMarginInfo margin;
        private XmlOutliningInfo outlining;
        private SyntaxEdit owner;
        private XmlEditPagesInfo pages;
        private XmlPrintingInfo printing;
        private XmlScrollingInfo scrolling;
        private XmlSelectionInfo selection;
        private XmlSpellingInfo spelling;
        private XmlTextSourceInfo textSource;
        private bool transparent;
        private bool wantReturns;
        private bool wantTabs;
        private XmlWhiteSpaceInfo whiteSpace;
    }
}

