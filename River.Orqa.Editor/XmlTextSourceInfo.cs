namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;

    public class XmlTextSourceInfo
    {
        // Methods
        public XmlTextSourceInfo()
        {
            this.navigateOptions = EditConsts.DefaultNavigateOptions;
            this.indentOptions = EditConsts.DefaultIndentOptions;
            BookMark[] markArray1 = new BookMark[0];
            this.bookMarks = markArray1;
            LStyle[] styleArray1 = new LStyle[0];
            this.lineStyles = styleArray1;
            this.undoOptions = EditConsts.DefaultUndoOptions;
            this.bracesOptions = River.Orqa.Editor.BracesOptions.None;
        }

        public XmlTextSourceInfo(TextSource Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(TextSource Owner)
        {
            this.owner = Owner;
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                this.FileName = this.fileName;
                this.Position = this.position;
                this.BookMarks = this.bookMarks;
                this.LineStyles = this.lineStyles;
                this.NavigateOptions = this.navigateOptions;
                this.ReadOnly = this.readOnly;
                this.OverWrite = this.overWrite;
                this.IndentOptions = this.indentOptions;
                this.UndoOptions = this.undoOptions;
                this.UndoLimit = this.undoLimit;
                this.HighlightUrls = this.highlightUrls;
                this.CheckSpelling = this.checkSpelling;
                this.Lexer = this.lexer;
                this.SyntaxStrings = this.syntaxStrings;
                this.BracesOptions = this.bracesOptions;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }


        // Properties
        public BookMark[] BookMarks
        {
            get
            {
                if (this.owner == null)
                {
                    return this.bookMarks;
                }
                BookMark[] markArray1 = new BookMark[this.owner.BookMarks.Count];
                for (int num1 = 0; num1 < this.owner.BookMarks.Count; num1++)
                {
                    markArray1[num1] = (BookMark) this.owner.BookMarks[num1];
                }
                return markArray1;
            }
            set
            {
                this.bookMarks = value;
                if (this.owner != null)
                {
                    this.owner.BookMarks.Clear();
                    BookMark[] markArray1 = value;
                    for (int num1 = 0; num1 < markArray1.Length; num1++)
                    {
                        IBookMark mark1 = markArray1[num1];
                        this.owner.BookMarks.Add(new BookMark(mark1.Line, mark1.Char, mark1.Index));
                    }
                }
            }
        }

        public River.Orqa.Editor.BracesOptions BracesOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.bracesOptions;
                }
                return this.owner.BracesOptions;
            }
            set
            {
                this.bracesOptions = value;
                if (this.owner != null)
                {
                    this.owner.BracesOptions = value;
                }
            }
        }

        public bool CheckSpelling
        {
            get
            {
                if (this.owner == null)
                {
                    return this.checkSpelling;
                }
                return this.owner.CheckSpelling;
            }
            set
            {
                this.checkSpelling = value;
                if (this.owner != null)
                {
                    this.owner.CheckSpelling = value;
                }
            }
        }

        public string FileName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fileName;
                }
                return this.owner.FileName;
            }
            set
            {
                this.fileName = value;
                if (this.owner != null)
                {
                    this.owner.FileName = value;
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

        public River.Orqa.Editor.IndentOptions IndentOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.indentOptions;
                }
                return this.owner.IndentOptions;
            }
            set
            {
                this.indentOptions = value;
                if (this.owner != null)
                {
                    this.owner.IndentOptions = value;
                }
            }
        }

        public XmlLexerInfo Lexer
        {
            get
            {
                if ((this.owner != null) && (this.owner.Lexer != null))
                {
                    return (XmlLexerInfo) ((River.Orqa.Editor.Syntax.Lexer) this.owner.Lexer).XmlInfo;
                }
                return this.lexer;
            }
            set
            {
                this.lexer = value;
                if ((this.owner != null) && (this.owner.Lexer != null))
                {
                    ((River.Orqa.Editor.Syntax.Lexer) this.owner.Lexer).XmlInfo = value;
                }
            }
        }

        public LStyle[] LineStyles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineStyles;
                }
                LStyle[] styleArray1 = new LStyle[this.owner.LineStyles.Count];
                for (int num1 = 0; num1 < this.owner.LineStyles.Count; num1++)
                {
                    styleArray1[num1] = ((River.Orqa.Editor.LineStyles) this.owner.LineStyles).GetLStyle(num1);
                }
                return styleArray1;
            }
            set
            {
                this.lineStyles = value;
                if (this.owner != null)
                {
                    this.owner.LineStyles.Clear();
                    LStyle[] styleArray1 = value;
                    for (int num1 = 0; num1 < styleArray1.Length; num1++)
                    {
                        LStyle style1 = styleArray1[num1];
                        this.owner.LineStyles.Add(new LStyle(style1.Line, style1.Char, style1.Index));
                    }
                }
            }
        }

        public River.Orqa.Editor.NavigateOptions NavigateOptions
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

        public bool OverWrite
        {
            get
            {
                if (this.owner == null)
                {
                    return this.overWrite;
                }
                return this.owner.OverWrite;
            }
            set
            {
                this.overWrite = value;
                if (this.owner != null)
                {
                    this.owner.OverWrite = value;
                }
            }
        }

        public Point Position
        {
            get
            {
                if (this.owner == null)
                {
                    return this.position;
                }
                return this.owner.Position;
            }
            set
            {
                this.position = value;
                if (this.owner != null)
                {
                    this.owner.Position = value;
                }
            }
        }

        public bool ReadOnly
        {
            get
            {
                if (this.owner == null)
                {
                    return this.readOnly;
                }
                return this.owner.ReadOnly;
            }
            set
            {
                this.readOnly = value;
                if (this.owner != null)
                {
                    this.owner.ReadOnly = value;
                }
            }
        }

        public XmlSyntaxStringsInfo SyntaxStrings
        {
            get
            {
                if (this.owner == null)
                {
                    return this.syntaxStrings;
                }
                return (XmlSyntaxStringsInfo) ((River.Orqa.Editor.SyntaxStrings) this.owner.Lines).XmlInfo;
            }
            set
            {
                this.syntaxStrings = value;
                if (this.owner != null)
                {
                    ((River.Orqa.Editor.SyntaxStrings) this.owner.Lines).XmlInfo = value;
                }
            }
        }

        public int UndoLimit
        {
            get
            {
                if (this.owner == null)
                {
                    return this.undoLimit;
                }
                return this.owner.UndoLimit;
            }
            set
            {
                this.undoLimit = value;
                if (this.owner != null)
                {
                    this.owner.UndoLimit = value;
                }
            }
        }

        public River.Orqa.Editor.UndoOptions UndoOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.undoOptions;
                }
                return this.owner.UndoOptions;
            }
            set
            {
                this.undoOptions = value;
                if (this.owner != null)
                {
                    this.owner.UndoOptions = value;
                }
            }
        }


        // Fields
        private BookMark[] bookMarks;
        private River.Orqa.Editor.BracesOptions bracesOptions;
        private bool checkSpelling;
        private string fileName;
        private bool highlightUrls;
        private River.Orqa.Editor.IndentOptions indentOptions;
        private XmlLexerInfo lexer;
        private LStyle[] lineStyles;
        private River.Orqa.Editor.NavigateOptions navigateOptions;
        private bool overWrite;
        private TextSource owner;
        private Point position;
        private bool readOnly;
        private XmlSyntaxStringsInfo syntaxStrings;
        private int undoLimit;
        private River.Orqa.Editor.UndoOptions undoOptions;
    }
}

