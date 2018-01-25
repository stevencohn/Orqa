namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ToolboxItem(false)]
    public class SourceParser : Parser
    {
        // Methods
        public SourceParser()
        {
            this.currentBlock = null;
            this.InitStyles();
            this.InitSyntax();
            this.Options = FormatTextOptions.CodeCompletion;
            this.lineBreaks = new Hashtable();
        }

        private void AddCodeMembers(IListMembers Provider, IMethodInfo Info)
        {
            this.AddCodeMembers(Info.Params, Provider);
            if (Info is IHasLocalVars)
            {
                this.AddCodeMembers(((IHasLocalVars) Info).LocalVars, Provider);
            }
        }

        private void AddCodeMembers(ISyntaxInfos Elements, IListMembers Provider)
        {
            SortedList list1 = new SortedList(new CaseInsensitiveComparer());
            foreach (ISyntaxInfo info1 in Elements)
            {
                if (list1.IndexOfKey(info1.Name) < 0)
                {
                    list1.Add(info1.Name, info1);
                }
            }
            foreach (DictionaryEntry entry1 in list1)
            {
                ISyntaxInfo info2 = (ISyntaxInfo) entry1.Value;
                IListMember member1 = Provider.AddMember();
                member1.ImageIndex = base.UnitInfo.GetImageIndex(info2);
                member1.Name = info2.Name;
                member1.Description = info2.Description;
            }
        }

        private void AddCodeMembers(MemberInfo[] Elements, IListMembers Provider)
        {
            SortedList list1 = new SortedList(new CaseInsensitiveComparer());
            MemberInfo[] infoArray1 = Elements;
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                MemberInfo info1 = infoArray1[num1];
                if (list1.IndexOfKey(info1.Name) < 0)
                {
                    list1.Add(info1.Name, info1);
                }
            }
            foreach (DictionaryEntry entry1 in list1)
            {
                MemberInfo info2 = (MemberInfo) entry1.Value;
                IListMember member1 = Provider.AddMember();
                member1.ImageIndex = base.UnitInfo.GetImageIndex(info2);
                member1.Name = info2.Name;
            }
        }

        private void AddCodeMembers(IListMembers Provider, IInterfaceInfo Info, bool IsStatic)
        {
            this.AddCodeMembers(Info.Properties, Provider);
            this.AddCodeMembers(Info.Methods, Provider);
            this.AddCodeMembers(Info.Events, Provider);
            if (Info is IClassInfo)
            {
                IClassInfo info1 = (IClassInfo) Info;
                this.AddCodeMembers(info1.Fields, Provider);
                this.AddCodeMembers(info1.Enums, Provider);
                this.AddCodeMembers(info1.Delegates, Provider);
            }
        }

        private void AddCodeMembers(IListMembers Provider, Type type, bool IsStatic)
        {
            BindingFlags flags1 = IsStatic ? (BindingFlags.Public | BindingFlags.Static) : (BindingFlags.Public | BindingFlags.Instance);
            this.AddCodeMembers(type.GetProperties(flags1), Provider);
            this.AddCodeMembers(type.GetMethods(flags1), Provider);
            this.AddCodeMembers(type.GetEvents(flags1), Provider);
            this.AddCodeMembers(type.GetFields(flags1), Provider);
        }

        private void AddStyle(string Name, Color Color)
        {
            this.AddStyle(Name, Color, false);
        }

        private void AddStyle(string Name, Color Color, bool PlainText)
        {
            ILexStyle style1 = base.Scheme.AddLexStyle();
            style1.Name = Name;
            if (Color != Color.Empty)
            {
                style1.ForeColor = Color;
            }
            style1.PlainText = PlainText;
        }

        protected virtual void AfterParseBlock()
        {
        }

        protected virtual bool BeforeParseBlock(ref Point BlockPt)
        {
            bool flag1 = this.SkipToBlockStart();
            if (flag1)
            {
                BlockPt = new Point(this.currentPos, base.LineIndex);
            }
            return flag1;
        }

        public override void CodeCompletion(string Text, Point Position, CodeCompletionArgs e)
        {
            string text2;
            if (((e.CompletionType == CodeCompletionType.ListMembers) || (e.CompletionType == CodeCompletionType.CompleteWord)) || ((e.CompletionType == CodeCompletionType.None) && (e.KeyChar == '.')))
            {
                string text1;
                int num1;
                if (this.GetNameAndTypeFromCode(Text, Position.X, out text1, out text2, out num1))
                {
                    object obj1 = base.UnitInfo.GetObjectClass((text1 == string.Empty) ? base.UnitInfo.SelfName : text1, Position);
                    bool flag1 = false;
                    if ((obj1 == null) && (text1 != string.Empty))
                    {
                        obj1 = base.UnitInfo.GetClassByName(text1);
                        flag1 = true;
                    }
                    IListMembers members1 = null;
                    if (obj1 != null)
                    {
                        members1 = new ListMembers();
                        if (obj1 is IInterfaceInfo)
                        {
                            this.AddCodeMembers(members1, (IInterfaceInfo) obj1, flag1);
                        }
                        else if (obj1 is Type)
                        {
                            this.AddCodeMembers(members1, (Type) obj1, flag1);
                        }
                    }
                    if (text1 == string.Empty)
                    {
                        ArrayList list1 = new ArrayList();
                        base.UnitInfo.Sections.GetRanges(list1, Position);
                        for (int num2 = list1.Count - 1; num2 >= 0; num2--)
                        {
                            if (list1[num2] is IHasParams)
                            {
                                if (members1 == null)
                                {
                                    members1 = new ListMembers();
                                }
                                this.AddCodeMembers(((IHasParams) list1[num2]).Params, members1);
                            }
                            if (list1[num2] is IHasLocalVars)
                            {
                                if (members1 == null)
                                {
                                    members1 = new ListMembers();
                                }
                                this.AddCodeMembers(((IHasLocalVars) list1[num2]).LocalVars, members1);
                            }
                        }
                    }
                    e.Provider = members1;
                }
                if (e.Provider != null)
                {
                    e.Provider.ShowDescriptions = true;
                    e.StartPosition = new Point(num1, Position.Y);
                    int num3 = 0;
                    if (this.FindCodeText(text2, e.Provider, out num3) && (e.CompletionType == CodeCompletionType.CompleteWord))
                    {
                        e.SelIndex = num3;
                    }
                    e.Provider.SelIndex = Math.Max(num3, 0);
                    e.Provider.Images = base.UnitInfo.Images;
                }
            }
            else
            {
                int num4;
                int num5;
                if (((e.CompletionType == CodeCompletionType.ParameterInfo) || ((e.CompletionType == CodeCompletionType.None) && (e.KeyChar == '('))) && this.GetMethodNameFromCode(Text, Position.X, out text2, out num5, out num4))
                {
                    ArrayList list2 = new ArrayList();
                    base.UnitInfo.GetMethods(text2, Position, list2);
                    if (list2.Count > 0)
                    {
                        IParameterInfo info1 = new River.Orqa.Editor.Syntax.ParameterInfo();
                        info1.ShowParams = true;
                        info1.ShowQualifiers = false;
                        foreach (object obj2 in list2)
                        {
                            IListMember member1 = info1.AddMember();
                            if (obj2 is IMethodInfo)
                            {
                                IMethodInfo info2 = (IMethodInfo) obj2;
                                member1.Qualifier = base.UnitInfo.GetMethodQualifier(info2);
                                member1.Name = info2.Name;
                                member1.DataType = info2.DataType;
                                member1.ParamText = base.UnitInfo.GetParamText(info2.Params);
                                continue;
                            }
                            if (obj2 is System.Reflection.MethodInfo)
                            {
                                System.Reflection.MethodInfo info3 = (System.Reflection.MethodInfo) obj2;
                                member1.Qualifier = base.UnitInfo.GetMethodQualifier(info3);
                                member1.DataType = (info3.ReturnType != null) ? info3.ReturnType.Name : string.Empty;
                                member1.Name = info3.Name;
                                member1.ParamText = base.UnitInfo.GetParamText(info3.GetParameters());
                            }
                        }
                        e.Provider = info1;
                    }
                    e.StartPosition = new Point(num5, Position.Y);
                    e.EndPosition = new Point(num4, Position.Y);
                    e.ToolTip = true;
                }
            }
            e.NeedShow = (e.Provider != null) && (e.Provider.Count > 0);
            if (e.CompletionType == CodeCompletionType.None)
            {
                e.Interval = Consts.DefaultHintDelay;
            }
        }

        protected virtual void CommonInitReswords()
        {
            this.reswords.Clear();
        }

        protected virtual void CommonInitRules()
        {
            this.lexerStates.Clear();
            this.RegisterLexerProc(0, this.lexWhitespaceProc);
            this.RegisterLexerProc(0, '!', '\x00ff', this.lexSymbolProc);
            this.RegisterLexerProc(0, 'a', 'z', this.lexIdentifierProc);
            this.RegisterLexerProc(0, 'A', 'Z', this.lexIdentifierProc);
            this.RegisterLexerProc(0, '_', this.lexIdentifierProc);
            this.RegisterLexerProc(0, '0', '9', this.lexNumberProc);
        }

        protected virtual void CommonInitSyntax()
        {
            this.lexerStates = new Hashtable();
            this.reswords = new Hashtable();
            this.lexIdentifierProc = new LexerProc(this.LexIdentifier);
            this.lexSymbolProc = new LexerProc(this.LexSymbol);
            this.lexNumberProc = new LexerProc(this.LexNumber);
            this.lexWhitespaceProc = new LexerProc(this.LexWhitespace);
            this.InitReswords();
            this.InitRules();
            this.StateChanged();
        }

        protected override int DoNextValidToken()
        {
            IRangeInfo info1 = null;
            int num1 = 0;
            if (this.currentBlock == null)
            {
                info1 = base.UnitInfo;
            }
            else if (this.currentBlock is IUnitInfo)
            {
                info1 = (IUnitInfo) this.currentBlock;
            }
            else if (this.currentBlock is IInterfaceInfo)
            {
                info1 = (IInterfaceInfo) this.currentBlock;
                num1 = info1.Level + 1;
            }
            if (info1 == null)
            {
                return base.DoNextValidToken();
            }
            CommentInfo info2 = null;
            int num2 = this.NextToken();
            while (!base.Eof && !this.IsValidToken(num2))
            {
                if (this.IsComment(base.Token))
                {
                    if (info2 == null)
                    {
                        info2 = new CommentInfo(string.Empty, new Point(this.tokenPos, base.LineIndex), num1);
                        info1.Comments.Add(info2);
                    }
                    info2.EndPoint = new Point(this.tokenPos + base.TokenString.Length, base.LineIndex);
                }
                num2 = this.NextToken();
            }
            return num2;
        }

        private bool FindCodeText(string s, ICodeCompletionProvider provider, out int idx)
        {
            idx = -1;
            if (s != string.Empty)
            {
                string[] textArray1 = provider.Strings;
                for (int num1 = 0; num1 < textArray1.Length; num1++)
                {
                    if (textArray1[num1].StartsWith(s))
                    {
                        bool flag1 = false;
                        for (int num2 = num1 + 1; num2 < textArray1.Length; num2++)
                        {
                            if (textArray1[num2].StartsWith(s))
                            {
                                flag1 = true;
                                break;
                            }
                        }
                        idx = num1;
                        return !flag1;
                    }
                }
            }
            return false;
        }

        private LexerState GetLexerState(int State)
        {
            LexerState state1 = (LexerState) this.lexerStates[State];
            if (state1 == null)
            {
                state1 = new LexerState();
                this.lexerStates[State] = state1;
            }
            return state1;
        }

        private bool GetMethodNameFromCode(string Text, int Pos, out string AName, out int StartPos, out int EndPos)
        {
            Text.Substring(0, Math.Min(Text.Length, Pos));
            AName = string.Empty;
            StartPos = (Text == string.Empty) ? -1 : Text.LastIndexOf('(', Math.Min(Pos, (int) (Text.Length - 1)));
            EndPos = -1;
            if (StartPos < 0)
            {
                return false;
            }
            EndPos = Text.IndexOf(')', StartPos);
            AName = Text.Substring(0, StartPos);
            StartPos = 0;
            int num1 = AName.LastIndexOf(' ');
            if (num1 < 0)
            {
                num1 = AName.LastIndexOf('\t');
            }
            if (num1 >= 0)
            {
                AName = AName.Substring(num1 + 1);
                StartPos = num1 + 1;
            }
            return (AName != string.Empty);
        }

        private bool GetNameAndTypeFromCode(string Text, int Pos, out string AType, out string AName, out int StartPos)
        {
            Text.Substring(0, Math.Min(Text.Length, Pos));
            AName = string.Empty;
            AType = string.Empty;
            StartPos = (Text == string.Empty) ? -1 : Text.LastIndexOf('.', Math.Min(Pos, (int) (Text.Length - 1)));
            if (StartPos >= 0)
            {
                AName = Text.Substring(StartPos + 1);
                AType = Text.Substring(0, StartPos);
                StartPos += 1;
                int num1 = AType.LastIndexOf(' ');
                if (num1 < 0)
                {
                    num1 = AType.LastIndexOf('\t');
                }
                if (num1 >= 0)
                {
                    AType = AType.Substring(num1 + 1);
                }
            }
            else if (Pos <= Text.Length)
            {
                StartPos = Pos;
                return true;
            }
            return (AType != string.Empty);
        }

        protected internal int GetReswordToken(string s)
        {
            object obj1 = this.reswords[this.caseSensitive ? s : s.ToLower()];
            if (obj1 == null)
            {
                return 0;
            }
            return (int) obj1;
        }

        protected virtual void InitReswords()
        {
            this.CommonInitReswords();
        }

        protected virtual void InitRules()
        {
            this.CommonInitRules();
        }

        protected virtual void InitStyles()
        {
            base.Scheme.ClearScheme();
            this.AddStyle("idents", SystemColors.ControlText);
            this.AddStyle("numbers", Color.Green);
            this.AddStyle("reswords", Color.Blue);
            this.AddStyle("comments", Color.Green, true);
            this.AddStyle("xml_comments", Color.Silver, true);
            this.AddStyle("symbol", Color.Gray);
            this.AddStyle("whitespace", Color.Empty);
            this.AddStyle("strings", Color.Maroon, true);
            this.AddStyle("directives", Color.Blue);
            base.Scheme.Author = "Quantum Whale LLC.";
            base.Scheme.Copyright = "Copyright (c) 2004, 2005 Quantum Whale LLC.";
        }

        protected virtual void InitSyntax()
        {
            this.CommonInitSyntax();
        }

        protected virtual bool IsBlockEnd(ref int BlockCount)
        {
            string text1 = base.TokenString;
            if ((base.Token == 5) || (base.Token == 2))
            {
                if (this.IsBlockStart(text1))
                {
                    BlockCount += 1;
                }
                else if (this.IsBlockEnd(text1))
                {
                    BlockCount -= 1;
                    if (BlockCount == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool IsComment(int tok)
        {
            if (tok != 3)
            {
                return (tok == 4);
            }
            return true;
        }

        protected virtual bool IsDeclarationEnd(string s)
        {
            return false;
        }

        protected bool IsInterface(ISyntaxInfo Info)
        {
            if (Info is IInterfaceInfo)
            {
                return !(Info is IClassInfo);
            }
            return false;
        }

        protected bool IsReswordOrIdentifier(int Token)
        {
            if (Token != 2)
            {
                return (Token == 0);
            }
            return true;
        }

        protected bool IsSymbolToken(string Symbol)
        {
            if (base.Token == 5)
            {
                return (base.TokenString == Symbol);
            }
            return false;
        }

        protected bool IsSymbolToken(int Token, string TokenStr, string Symbol)
        {
            if (Token == 5)
            {
                return (TokenStr == Symbol);
            }
            return false;
        }

        protected override bool IsValidToken(int tok)
        {
            if (tok != 6)
            {
                return !this.IsComment(tok);
            }
            return false;
        }

        protected virtual int LexIdent()
        {
            this.currentPos++;
            int num1 = this.source.Length;
            while (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                if ((((ch1 < 'a') || (ch1 > 'z')) && ((ch1 < 'A') || (ch1 > 'Z'))) && (((ch1 < '0') || (ch1 > '9')) && (ch1 != '_')))
                {
                    break;
                }
                this.currentPos++;
            }
            return 0;
        }

        protected virtual int LexIdentifier()
        {
            this.LexIdent();
            if (this.reswords.Contains(this.caseSensitive ? base.TokenString : base.TokenString.ToLower()))
            {
                return 2;
            }
            return 0;
        }

        protected virtual int LexNumber()
        {
            this.currentPos++;
            int num1 = this.source.Length;
            while (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                if ((ch1 < '0') || (ch1 > '9'))
                {
                    break;
                }
                this.currentPos++;
            }
            return 1;
        }

        protected virtual int LexSymbol()
        {
            this.currentPos++;
            return 5;
        }

        protected virtual int LexWhitespace()
        {
            this.currentPos++;
            int num1 = this.source.Length;
            while (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                if ((ch1 < '\0') || (ch1 > ' '))
                {
                    break;
                }
                this.currentPos++;
            }
            return 6;
        }

        protected void ParseBlock(ISyntaxInfos Infos, ISyntaxInfo Info, int Level, ParseProc PreProcessProc, ParseProc ProcessProc)
        {
            ISyntaxInfo info1 = this.currentBlock;
            this.currentBlock = Info;
            if (PreProcessProc != null)
            {
                PreProcessProc(Info, Infos, Level + 1);
            }
            if (ProcessProc != null)
            {
                Point point1 = new Point(base.PrevPos, base.PrevLine);
                Point point2 = new Point(this.currentPos, base.LineIndex);
                bool flag1 = false;
                if (this.BeforeParseBlock(ref point2))
                {
                    point1 = new Point(base.PrevPos, base.PrevLine);
                    flag1 = true;
                    int num1 = 0;
                    while (!base.Eof && !this.IsBlockEnd(ref num1))
                    {
                        ProcessProc(Info, Infos, Level + 1);
                    }
                    this.AfterParseBlock();
                    this.NextValidToken();
                }
                if (Info != null)
                {
                    if (Info is IRangeInfo)
                    {
                        IRangeInfo info2 = (IRangeInfo) Info;
                        info2.DeclarationSize = new Size(point2.X - info2.Position.X, point2.Y - info2.Position.Y);
                        info2.StartPoint = point1;
                        info2.EndPoint = new Point(base.PrevPos, base.PrevLine);
                        info2.HasBlock = flag1;
                    }
                    else
                    {
                        Info.DeclarationSize = new Size(point1.X - Info.Position.X, point1.Y - Info.Position.Y);
                    }
                }
            }
            if ((Infos != null) && (Info != null))
            {
                Infos.Add(Info);
            }
            this.currentBlock = info1;
        }

        public override int ParseText(int State, string String, ref short[] ColorData)
        {
            base.State = State;
            this.source = String;
            int num3 = String.Length;
            for (int num4 = 0; num4 < num3; num4 = this.currentPos)
            {
                int num2 = num4;
                LexerProc proc1 = this.lexerState.GetProc(String[num4]);
                if (proc1 == null)
                {
                    num4++;
                    continue;
                }
                if (num2 != num4)
                {
                    for (int num5 = num4; num5 < num2; num5++)
                    {
                        ColorData[num5] = 0;
                    }
                }
                this.currentPos = num4;
                this.tokenPos = num4;
                int num1 = proc1();
                for (int num6 = num4; num6 < Math.Min(this.currentPos, num3); num6++)
                {
                    ColorData[num6] = (byte) (num1 + 1);
                }
            }
            return base.State;
        }

        public override int ParseText(int State, string String, ref int Pos, ref int Len, ref int Style)
        {
            int num1 = String.Length;
            LexerProc proc1 = null;
            while (Pos < num1)
            {
                proc1 = this.lexerState.GetProc(String[Pos]);
                if (proc1 != null)
                {
                    break;
                }
                Pos += 1;
            }
            if (proc1 != null)
            {
                this.currentPos = Pos;
                this.tokenPos = Pos;
                Style = proc1();
                Len = this.currentPos - Pos;
            }
            return base.State;
        }

        protected override void ProcessIndent(IRangeInfo Info)
        {
            for (int num1 = Info.Position.Y; num1 <= Info.EndPoint.Y; num1++)
            {
                base.Indents[num1] = ((RangeInfo) Info).GetIndentLevel(num1, this.lineBreaks);
            }
        }

        protected void RegisterLexerProc(int State, LexerProc Proc)
        {
            this.GetLexerState(State).StateProc = Proc;
        }

        protected void RegisterLexerProc(int State, char Char, LexerProc Proc)
        {
            char[] chArray1 = new char[1] { Char } ;
            this.RegisterLexerProc(State, chArray1, Proc);
        }

        protected void RegisterLexerProc(int State, char[] Chars, LexerProc Proc)
        {
            this.GetLexerState(State).Add(Chars, Proc);
        }

        protected void RegisterLexerProc(int State, char StartChar, char EndChar, LexerProc Proc)
        {
            char[] chArray1 = new char[(EndChar - StartChar) + '\x0001'];
            for (char ch1 = StartChar; ch1 <= EndChar; ch1++)
            {
                chArray1[ch1 - StartChar] = ch1;
            }
            this.RegisterLexerProc(State, chArray1, Proc);
        }

        public override void Reset()
        {
            base.Reset();
            this.lineBreaks.Clear();
        }

        protected bool SkipBrackets(string closeBr)
        {
            string text1 = null;
            return this.SkipBrackets(closeBr, ref text1);
        }

        protected bool SkipBrackets(string closeBr, ref string Attributes)
        {
            string text1 = base.TokenString;
            int num1 = 1;
            while (!base.Eof)
            {
                int num2 = this.NextValidToken();
                if ((num2 != 5) && (num2 != 2))
                {
                    continue;
                }
                string text2 = base.TokenString;
                if (text2 == closeBr)
                {
                    num1--;
                    if (num1 != 0)
                    {
                        goto Label_005D;
                    }
                    this.NextValidToken();
                    return true;
                }
                if (text2 == text1)
                {
                    num1++;
                }
                else if (this.IsBlockStart(text2) || this.IsBlockEnd(text2))
                {
                    return false;
                }
            Label_005D:
                if (Attributes != null)
                {
                    Attributes = Attributes + text2;
                }
            }
            return false;
        }

        protected bool SkipToBlockStart()
        {
            while (!base.Eof)
            {
                if ((base.Token == 5) || (base.Token == 2))
                {
                    string text1 = base.TokenString;
                    if (this.IsBlockStart(text1))
                    {
                        return true;
                    }
                    if (this.IsDeclarationEnd(text1) || this.IsBlockEnd(text1))
                    {
                        this.NextValidToken();
                        return false;
                    }
                }
                this.NextValidToken();
            }
            return false;
        }

        protected void SkipToNextLine()
        {
            int num1 = base.LineIndex;
            while (!base.Eof)
            {
                this.NextToken();
                if (base.LineIndex != num1)
                {
                    return;
                }
            }
        }

        protected override void StateChanged()
        {
            this.lexerState = this.GetLexerState(base.State);
        }

        protected internal virtual Point ValidatePosition(int X, int Y)
        {
            if (!base.Eof)
            {
                return new Point(X, Y);
            }
            int num1 = base.Strings.Count;
            if (num1 > 0)
            {
                return new Point(base.Strings[num1 - 1].Length, num1 - 1);
            }
            return new Point(0, 0);
        }


        // Properties
        protected bool CaseSensitive
        {
            get
            {
                return this.caseSensitive;
            }
            set
            {
                this.caseSensitive = value;
            }
        }

        protected Hashtable LineBreaks
        {
            get
            {
                return this.lineBreaks;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Editor("River.Orqa.Editor.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), Browsable(true)]
		// SMC: changed virtual to override
		public override FormatTextOptions Options
        {
            get
            {
                return base.Options;
            }
            set
            {
                base.Options = value;
            }
        }

        protected Hashtable Reswords
        {
            get
            {
                return this.reswords;
            }
        }


        // Fields
        private bool caseSensitive;
        protected ISyntaxInfo currentBlock;
        private LexerState lexerState;
        private Hashtable lexerStates;
        protected LexerProc lexIdentifierProc;
        protected LexerProc lexNumberProc;
        protected LexerProc lexSymbolProc;
        protected LexerProc lexWhitespaceProc;
        private Hashtable lineBreaks;
        private Hashtable reswords;
        public const int StateComment = 1;
        public const int StateNormal = 0;
        public const int StateXmlComment = 2;

        // Nested Types
        private class LexerState
        {
            // Methods
            public LexerState()
            {
                this.hash = new Hashtable();
            }

            public void Add(char[] Chars, LexerProc Proc)
            {
                char[] chArray1 = Chars;
                for (int num1 = 0; num1 < chArray1.Length; num1++)
                {
                    char ch1 = chArray1[num1];
                    this.hash[ch1] = Proc;
                }
            }

            public void Clear()
            {
                this.hash.Clear();
            }

            public LexerProc GetProc(char ch)
            {
                LexerProc proc1 = (LexerProc) this.hash[ch];
                if (proc1 == null)
                {
                    proc1 = this.stateProc;
                }
                return proc1;
            }


            // Properties
            public Hashtable Hash
            {
                get
                {
                    return this.hash;
                }
            }

            public LexerProc StateProc
            {
                get
                {
                    return this.stateProc;
                }
                set
                {
                    this.stateProc = value;
                }
            }


            // Fields
            private Hashtable hash;
            private LexerProc stateProc;
        }

        protected delegate void ParseProc(ISyntaxInfo Info, ISyntaxInfos Infos, int Level);

    }
}

