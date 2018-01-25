namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    [ToolboxItem(false)]
    public class Lexer : Component, ILexer, INotify
    {
        // Events
        public event TextParsedEvent TextParsed;

        // Methods
        public Lexer()
        {
            this.components = null;
            this.updateCount = 0;
            this.InitializeComponent();
            this.textParsedEventArgs = new TextParsedEventArgs();
            this.scheme = new LexScheme();
        }

        public Lexer(IContainer container) : this()
        {
            container.Add(this);
        }

        public void AddNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Combine(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Notify();
            }
            return this.updateCount;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public void LoadScheme(TextReader Reader)
        {
            XmlSerializer serializer1 = new XmlSerializer(typeof(LexScheme));
            try
            {
                this.scheme = (LexScheme) serializer1.Deserialize(Reader);
                this.Update();
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void LoadScheme(string FileName)
        {
            try
            {
                Stream stream1 = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    StreamReader reader1 = new StreamReader(stream1);
                    try
                    {
                        this.LoadScheme(reader1);
                    }
                    finally
                    {
                        reader1.Close();
                    }
                }
                finally
                {
                    stream1.Close();
                }
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void Notify()
        {
            if (this.notifyHandler != null)
            {
                this.notifyHandler(this, EventArgs.Empty);
            }
        }

        protected void OnTextParsed(string String, ref short[] ColorData)
        {
            if (this.TextParsed != null)
            {
                this.textParsedEventArgs.String = String;
                this.textParsedEventArgs.ColorData = ColorData;
                this.TextParsed(this, this.textParsedEventArgs);
            }
        }

        public virtual int ParseText(int State, string String, ref short[] ColorData)
        {
            if ((String != null) && (String != string.Empty))
            {
                int num2;
                int num1 = String.Length;
                if (this.scheme.GetLexStatesCount() == 0)
                {
                    for (num2 = 0; num2 < num1; num2++)
                    {
                        ColorData[num2] = 0;
                    }
                    return State;
                }
                LexState state1 = (LexState) this.scheme.GetLexState(State);
                Match match1 = state1.Regex.Match(String);
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                while (match1.Success)
                {
                    if (match1.Index > num3)
                    {
                        for (num2 = num3; num2 < match1.Index; num2++)
                        {
                            ColorData[num2] = 0;
                        }
                    }
                    num3 = match1.Index;
                    this.StateFromMatch(match1, String.Substring(num3, match1.Length), state1.BlockTable, ref num4, ref num5);
                    if (match1.Length > 0)
                    {
                        for (num2 = num3; num2 < (num3 + match1.Length); num2++)
                        {
                            ColorData[num2] = (byte) (num5 + 1);
                        }
                    }
                    num3 += match1.Length;
                    if (num4 != State)
                    {
                        State = num4;
                        state1 = (LexState) this.scheme.GetLexState(num4);
                        match1 = state1.Regex.Match(String, num3);
                        continue;
                    }
                    match1 = match1.NextMatch();
                }
                this.OnTextParsed(String, ref ColorData);
            }
            return State;
        }

        public virtual int ParseText(int State, string String, ref int Pos, ref int Len, ref int Style)
        {
            Len = 0;
            Style = 0;
            if (String != null)
            {
                LexState state1 = (LexState) this.scheme.GetLexState(State);
                Match match1 = state1.Regex.Match(String, Pos);
                if (match1.Success)
                {
                    Pos = match1.Index;
                    Len = match1.Length;
                    int num1 = State;
                    int num2 = 0;
                    this.StateFromMatch(match1, String.Substring(Pos, Len), state1.BlockTable, ref num1, ref Style);
                    bool flag1 = (Pos + match1.Length) >= String.Length;
                    int num3 = flag1 ? (Pos + match1.Length) : Pos;
                    while ((match1.Length == 0) || flag1)
                    {
                        if (State != num1)
                        {
                            State = num1;
                            state1 = (LexState) this.scheme.GetLexState(State);
                            match1 = state1.Regex.Match(String, num3);
                        }
                        else
                        {
                            match1 = match1.NextMatch();
                        }
                        if (!match1.Success)
                        {
                            break;
                        }
                        if (!flag1)
                        {
                            Pos = match1.Index;
                            Len = match1.Length;
                        }
                        this.StateFromMatch(match1, string.Empty, state1.BlockTable, ref num1, ref num2);
                        if (!flag1)
                        {
                            Style = num2;
                        }
                    }
                    State = num1;
                }
                if (Len == 0)
                {
                    Len = 1;
                }
            }
            return State;
        }

        public void RemoveNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Remove(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public virtual void ResetDefaultState()
        {
            this.DefaultState = 0;
        }

        public void SaveScheme(TextWriter Writer)
        {
            this.SaveScheme(Writer, this.scheme);
        }

        public void SaveScheme(string FileName)
        {
            try
            {
                Stream stream1 = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    StreamWriter writer1 = new StreamWriter(stream1);
                    try
                    {
                        this.SaveScheme(writer1);
                    }
                    finally
                    {
                        writer1.Close();
                    }
                }
                finally
                {
                    stream1.Close();
                }
            }
            catch (Exception exception1)
            {
                ErrorHandler.Error(exception1);
            }
        }

        public void SaveScheme(TextWriter Writer, LexScheme Scheme)
        {
            XmlSerializer serializer1 = new XmlSerializer(typeof(LexScheme));
            try
            {
                serializer1.Serialize(Writer, Scheme);
            }
            catch (Exception exception1)
            {
                Writer.Flush();
                ErrorHandler.Error(exception1);
            }
        }

        public virtual bool ShouldSerializeToCode()
        {
            return true;
        }

        private void StateFromMatch(Match match, string s, Hashtable blocks, ref int state, ref int style)
        {
            style = -1;
            LexSyntaxBlock block1 = null;
            for (int num1 = 0; num1 < match.Groups.Count; num1++)
            {
                if (match.Groups[num1].Success)
                {
                    block1 = (LexSyntaxBlock) blocks[num1];
                    if (block1 != null)
                    {
                        break;
                    }
                }
            }
            if (block1 != null)
            {
                if (block1.LeaveState != null)
                {
                    state = block1.LeaveState.Index;
                }
                if (block1.Style != null)
                {
                    style = block1.Style.Index;
                }
                int num2 = block1.FindResWord(s);
                if (num2 >= 0)
                {
                    ILexStyle style1 = block1.ResWordSets[num2].ResWordStyle;
                    if (style1 != null)
                    {
                        style = style1.Index;
                    }
                }
            }
        }

        protected void Update()
        {
            if (this.updateCount == 0)
            {
                this.Notify();
            }
        }


        // Properties
        [Category("Parser")]
        public int DefaultState
        {
            get
            {
                return this.defaultState;
            }
            set
            {
                if (this.defaultState != value)
                {
                    this.defaultState = value;
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Parser"), Editor("River.Orqa.Editor.Design.SyntaxBuilderEditor, River.Orqa.Editor.Syntax", typeof(UITypeEditor))]
        public LexScheme Scheme
        {
            get
            {
                return this.scheme;
            }
            set
            {
                StringWriter writer1 = new StringWriter();
                this.SaveScheme(writer1, value);
                StringReader reader1 = new StringReader(writer1.ToString());
                this.LoadScheme(reader1);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object XmlInfo
        {
            get
            {
                return new XmlLexerInfo(this);
            }
            set
            {
                ((XmlLexerInfo) value).FixupReferences(this);
            }
        }

        [XmlIgnore, Browsable(false), DesignOnly(true)]
        public string XmlScheme
        {
            get
            {
                string text1;
                try
                {
                    StringWriter writer1 = new StringWriter();
                    this.SaveScheme(writer1);
                    text1 = writer1.ToString();
                }
                catch
                {
                    text1 = string.Empty;
                }
                return text1;
            }
            set
            {
                try
                {
                    StringReader reader1 = new StringReader(value);
                    this.LoadScheme(reader1);
                }
                catch
                {
                }
            }
        }


        // Fields
        private Container components;
        private int defaultState;
        private EventHandler notifyHandler;
        private LexScheme scheme;
        private TextParsedEventArgs textParsedEventArgs;
        private int updateCount;
    }
}

