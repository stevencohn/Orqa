namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable, DesignerSerializer("River.Orqa.Editor.Design.LexerSerializer, River.Orqa.Editor.Syntax", typeof(CodeDomSerializer)), ToolboxBitmap(typeof(Parser), "Images.Parser.bmp"), ToolboxItem(true)]
    public class Parser : Lexer, IParse, ILexer, IFormatText
    {
        // Events
        public event EventHandler TextReparsed;

        // Methods
        public Parser()
        {
            this.stack = new ArrayList();
            this.indents = new Hashtable();
            this.unitInfo = this.CreateUnitInfo();
        }

        public virtual void BlockDeleting(Rectangle Rect)
        {
            this.unitInfo.BlockDeleting(Rect);
        }

        public virtual void CodeCompletion(string Text, Point Position, CodeCompletionArgs e)
        {
        }

        protected virtual IUnitInfo CreateUnitInfo()
        {
            return new River.Orqa.Editor.Syntax.UnitInfo();
        }

        protected virtual int DoNextValidToken()
        {
            int num1 = this.NextToken();
            while (!this.Eof && !this.IsValidToken(num1))
            {
                num1 = this.NextToken();
            }
            return num1;
        }

        public virtual IRange GetBlock(Point Position)
        {
            return this.unitInfo.FindRange(Position);
        }

        public virtual int GetSmartIndent(int Index)
        {
            object obj1 = this.indents[Index];
            if (obj1 == null)
            {
                return -1;
            }
            return (int) obj1;
        }

        public virtual bool IsBlockEnd(string s)
        {
            return false;
        }

        public virtual bool IsBlockStart(string s)
        {
            return false;
        }

        protected internal bool IsStackEmpty()
        {
            return (this.stack.Count == 0);
        }

        protected virtual bool IsValidToken(int Token)
        {
            return true;
        }

        public virtual int NextToken()
        {
            this.prevToken = this.token;
            this.prevPos = this.currentPos;
            this.prevLine = this.lineIndex;
            this.tokenPos = this.currentPos;
            if (((this.source == null) || (this.currentPos >= this.source.Length)) && !this.UpdateLine())
            {
                this.token = -1;
            }
            else
            {
                int num1 = 0;
                this.State = this.ParseText(this.state, this.source, ref this.tokenPos, ref num1, ref this.token);
                this.currentPos = this.tokenPos + num1;
            }
            return this.token;
        }

        public int NextToken(out string str)
        {
            int num1 = this.NextToken();
            str = this.TokenString;
            return num1;
        }

        public virtual int NextValidToken()
        {
            int num1 = this.currentPos;
            int num2 = this.lineIndex;
            int num3 = this.token;
            int num4 = this.DoNextValidToken();
            this.prevPos = num1;
            this.prevLine = num2;
            this.prevToken = num3;
            return num4;
        }

        public int NextValidToken(out string str)
        {
            int num1 = this.NextValidToken();
            str = this.TokenString;
            return num1;
        }

        public virtual int Outline(IList Ranges)
        {
            foreach (IOutlineRange range1 in this.unitInfo.Sections)
            {
                if (range1.Visible)
                {
                    Ranges.Add(range1);
                }
            }
            return Ranges.Count;
        }

        public int PeekToken()
        {
            int num1;
            this.SaveState();
            try
            {
                num1 = this.NextToken();
            }
            finally
            {
                this.RestoreState();
            }
            return num1;
        }

        public int PeekToken(out string str)
        {
            int num1;
            this.SaveState();
            try
            {
                num1 = this.NextToken();
                str = this.TokenString;
            }
            finally
            {
                this.RestoreState();
            }
            return num1;
        }

        public int PeekValidToken()
        {
            int num1;
            this.SaveState();
            try
            {
                num1 = this.NextValidToken();
            }
            finally
            {
                this.RestoreState();
            }
            return num1;
        }

        public int PeekValidToken(out string str)
        {
            int num2;
            this.SaveState();
            try
            {
                int num1 = this.NextValidToken();
                str = this.TokenString;
                num2 = num1;
            }
            finally
            {
                this.RestoreState();
            }
            return num2;
        }

        public virtual void PositionChanged(int X, int Y, int DeltaX, int DeltaY, UpdateReason Reason)
        {
            this.unitInfo.PositionChanged(X, Y, DeltaX, DeltaY, Reason);
        }

        protected virtual void ProcessIndent(IRangeInfo Info)
        {
            for (int num1 = Info.Position.Y; num1 <= Info.EndPoint.Y; num1++)
            {
                this.indents[num1] = Info.GetIndentLevel(num1);
            }
        }

        protected virtual void ProcessIndents(RangeList Sections)
        {
            foreach (IRangeInfo info1 in Sections.GetRanges())
            {
                this.ProcessIndent(info1);
            }
        }

        public virtual void ReparseText()
        {
            ((River.Orqa.Editor.Syntax.UnitInfo) this.unitInfo).UpdateSections();
            this.UpdateIndents();
            if (this.TextReparsed != null)
            {
                this.TextReparsed(this, EventArgs.Empty);
            }
        }

        public virtual void Reset()
        {
            this.Reset(0, 0, base.DefaultState);
            this.unitInfo.Clear();
        }

        public void Reset(int Line, int Pos, int State)
        {
            this.lineIndex = Line;
            this.token = 0;
            this.prevToken = 0;
            this.prevPos = 0;
            this.prevLine = 0;
            this.tokenPos = Pos;
            this.currentPos = Pos;
            this.State = State;
            this.ResetLine(Line);
        }

        protected virtual void ResetLine(int Line)
        {
            this.source = (((this.strings != null) && (this.lineIndex >= 0)) && (this.lineIndex < this.strings.Count)) ? this.strings[this.lineIndex] : null;
        }

        public virtual void ResetOptions()
        {
            this.Options = FormatTextOptions.None;
        }

        public void RestoreState()
        {
            this.RestoreState(true);
        }

        public void RestoreState(bool Restore)
        {
            if (this.stack.Count > 0)
            {
                if (Restore)
                {
                    ParserState state1 = (ParserState) this.stack[this.stack.Count - 1];
                    this.lineIndex = state1.LineIndex;
                    this.token = state1.Token;
                    this.prevToken = state1.PrevToken;
                    this.prevLine = state1.PrevLine;
                    this.prevPos = state1.PrevPos;
                    this.tokenPos = state1.TokenPos;
                    this.currentPos = state1.CurrentPos;
                    this.State = state1.State;
                    this.ResetLine(this.lineIndex);
                }
                this.stack.RemoveAt(this.stack.Count - 1);
            }
        }

        public void SaveState()
        {
            ParserState state1 = new ParserState();
            state1.LineIndex = this.lineIndex;
            state1.Token = this.token;
            state1.PrevToken = this.prevToken;
            state1.PrevLine = this.prevLine;
            state1.PrevPos = this.prevPos;
            state1.TokenPos = this.tokenPos;
            state1.CurrentPos = this.currentPos;
            state1.State = this.state;
            this.stack.Add(state1);
        }

        protected virtual void StateChanged()
        {
        }

        protected virtual void UpdateIndents()
        {
            this.indents.Clear();
            this.ProcessIndents(this.unitInfo.Sections);
        }

        protected virtual bool UpdateLine()
        {
            this.currentPos = 0;
            this.tokenPos = 0;
            this.source = null;
            while (((this.strings != null) && ((this.source == null) || (this.source == string.Empty))) && (this.lineIndex < this.strings.Count))
            {
                this.lineIndex++;
                this.ResetLine(this.lineIndex);
            }
            if (this.source != null)
            {
                return (this.source != string.Empty);
            }
            return false;
        }


        // Properties
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPosition
        {
            get
            {
                return this.currentPos;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool Eof
        {
            get
            {
                if ((this.token != -1) && (this.strings != null))
                {
                    return (this.lineIndex >= this.strings.Count);
                }
                return true;
            }
        }

        protected Hashtable Indents
        {
            get
            {
                return this.indents;
            }
        }

        protected IStringList InternalStrings
        {
            get
            {
                if (this.internalStrings == null)
                {
                    this.internalStrings = new StringList();
                }
                return this.internalStrings;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int LineIndex
        {
            get
            {
                return this.lineIndex;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string[] Lines
        {
            get
            {
                if (this.strings == null)
                {
                    return null;
                }
                string[] textArray1 = new string[this.strings.Count];
                for (int num1 = 0; num1 < this.strings.Count; num1++)
                {
                    textArray1[num1] = this.strings[num1];
                }
                return textArray1;
            }
            set
            {
                this.strings = this.InternalStrings;
                string[] textArray1 = value;
                for (int num1 = 0; num1 < textArray1.Length; num1++)
                {
                    string text1 = textArray1[num1];
                    ((StringList) this.strings).Add(text1);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual FormatTextOptions Options
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

        protected internal int PrevLine
        {
            get
            {
                return this.prevLine;
            }
        }

        protected internal int PrevPos
        {
            get
            {
                return this.prevPos;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PrevToken
        {
            get
            {
                return this.prevToken;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int State
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.state != value)
                {
                    this.state = value;
                    this.StateChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IStringList Strings
        {
            get
            {
                return this.strings;
            }
            set
            {
                this.strings = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int Token
        {
            get
            {
                return this.token;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int TokenPosition
        {
            get
            {
                return this.tokenPos;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TokenString
        {
            get
            {
                if ((this.source != null) && (this.tokenPos < this.source.Length))
                {
                    return this.source.Substring(this.tokenPos, Math.Min(this.currentPos, this.source.Length) - this.tokenPos);
                }
                return string.Empty;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IUnitInfo UnitInfo
        {
            get
            {
                return this.unitInfo;
            }
        }


        // Fields
        protected int currentPos;
        private Hashtable indents;
        private IStringList internalStrings;
        private int lineIndex;
        private FormatTextOptions options;
        private int prevLine;
        private int prevPos;
        private int prevToken;
        protected string source;
        private ArrayList stack;
        private int state;
        private IStringList strings;
        private int token;
        protected int tokenPos;
        private IUnitInfo unitInfo;

        // Nested Types
        private class ParserState
        {
            // Methods
            public ParserState()
            {
            }


            // Fields
            public int CurrentPos;
            public int LineIndex;
            public int PrevLine;
            public int PrevPos;
            public int PrevToken;
            public int State;
            public int Token;
            public int TokenPos;
        }
    }
}

