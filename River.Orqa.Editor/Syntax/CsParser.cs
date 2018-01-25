namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(CsParser), "Images.CsParser.bmp"), ToolboxItem(true)]
    public class CsParser : SourceParser
    {
        // Methods
        public CsParser()
        {
        }

        protected override IUnitInfo CreateUnitInfo()
        {
            return new CsUnitInfo();
        }

        protected bool GetName(ref string Name, string Delimiter)
        {
            Name = string.Empty;
            while (!base.Eof)
            {
                string text1;
                int num1 = base.NextValidToken(out text1);
                if ((num1 != 0) && !base.IsSymbolToken(num1, text1, Delimiter))
                {
                    break;
                }
                Name = Name + text1;
            }
            return (Name != string.Empty);
        }

        protected bool GetNameAndType(out string AName, out string AType)
        {
            AType = string.Empty;
            if (base.NextValidToken(out AName) != 0)
            {
                return false;
            }
            this.NextValidToken();
            if (this.IsTypeSeparator())
            {
                while (!base.Eof)
                {
                    string text1;
                    int num1 = base.NextValidToken(out text1);
                    if (num1 != 0)
                    {
                        if (num1 == 5)
                        {
                            goto Label_0042;
                        }
                        continue;
                    }
                    AType = AType + text1;
                    continue;
                Label_0042:
                    if ((text1 == ";") || (text1 == "{"))
                    {
                        return true;
                    }
                    if ((text1 != ",") && (text1 != "."))
                    {
                        return false;
                    }
                    AType = AType + text1;
                }
            }
            return true;
        }

        protected bool GetParamTypeAndName(ref string AName, ref string AType, ref Point Position, out string Qualifier)
        {
            base.NextValidToken(out AType);
            Position = new Point(this.tokenPos, base.LineIndex);
            if (base.GetReswordToken(AType) == 8)
            {
                Qualifier = AType;
                base.NextValidToken(out AType);
            }
            else
            {
                Qualifier = string.Empty;
            }
            if (!base.IsReswordOrIdentifier(base.Token))
            {
                return false;
            }
            return (this.GetTypeAndName(ref AName, ref AType) == CsParserType.Param);
        }

        protected void GetScope(out CsScope Scope, out CsModifier Modifiers)
        {
            Scope = CsScope.None;
            Modifiers = CsModifier.None;
            while (base.Token == 2)
            {
                string text1 = base.TokenString;
                SyntaxToken token1 = (SyntaxToken) base.GetReswordToken(text1);
                if (token1 == SyntaxToken.Scope)
                {
                    Scope |= ((CsScope) Enum.Parse(typeof(CsScope), text1, true));
                }
                else
                {
                    if (token1 != SyntaxToken.Modifier)
                    {
                        return;
                    }
                    Modifiers |= ((CsModifier) Enum.Parse(typeof(CsModifier), text1, true));
                }
                this.NextValidToken();
            }
        }

        protected virtual CsParserType GetTypeAndName(ref string AName, ref string AType)
        {
            string text1;
            string text2;
            if (AType == string.Empty)
            {
                base.NextValidToken(out AType);
                if (!base.IsReswordOrIdentifier(base.Token))
                {
                    return CsParserType.None;
                }
            }
            base.NextValidToken(out AName);
            while (AName == ".")
            {
                this.NextValidToken();
                AType = AType + AName + base.TokenString;
                base.NextValidToken(out AName);
            }
            if (AName == "[")
            {
                if (!base.SkipBrackets("]"))
                {
                    return CsParserType.None;
                }
                AName = base.TokenString;
            }
            if (AName == "(")
            {
                AName = AType;
                AType = string.Empty;
                return CsParserType.Method;
            }
            if (base.Token != 0)
            {
                if ((base.Token != 2) || (base.TokenString != "this"))
                {
                    return CsParserType.None;
                }
                base.NextValidToken(out text1);
                if (text1 == "[")
                {
                    if (!base.SkipBrackets("]"))
                    {
                        return CsParserType.None;
                    }
                    text1 = base.TokenString;
                }
            }
            else
            {
                base.NextValidToken(out text1);
            }
            if ((base.Token == 5) && ((text2 = text1) != null))
            {
                text2 = string.IsInterned(text2);
                if (text2 != "(")
                {
                    if (text2 == ";")
                    {
                        return CsParserType.Field;
                    }
                    if (text2 == "=")
                    {
                        if (this.SkipToSymbol(";"))
                        {
                            return CsParserType.Field;
                        }
                    }
                    else
                    {
                        if (text2 == "{")
                        {
                            return CsParserType.Property;
                        }
                        if ((text2 == ",") || (text2 == ")"))
                        {
                            return CsParserType.Param;
                        }
                    }
                }
                else
                {
                    return CsParserType.Method;
                }
            }
            return CsParserType.None;
        }

        protected override void InitReswords()
        {
            base.InitReswords();
            Hashtable hashtable1 = base.Reswords;
            hashtable1.Add("as", SyntaxToken.Operator);
            hashtable1.Add("is", SyntaxToken.Operator);
            hashtable1.Add("sizeof", SyntaxToken.Operator);
            hashtable1.Add("typeof", SyntaxToken.Operator);
            hashtable1.Add("if", SyntaxToken.Statement);
            hashtable1.Add("else", SyntaxToken.Statement);
            hashtable1.Add("switch", SyntaxToken.Statement);
            hashtable1.Add("case", SyntaxToken.Statement);
            hashtable1.Add("do", SyntaxToken.Statement);
            hashtable1.Add("for", SyntaxToken.Statement);
            hashtable1.Add("foreach", SyntaxToken.Statement);
            hashtable1.Add("while", SyntaxToken.Statement);
            hashtable1.Add("break", SyntaxToken.Statement);
            hashtable1.Add("continue", SyntaxToken.Statement);
            hashtable1.Add("default", SyntaxToken.Statement);
            hashtable1.Add("goto", SyntaxToken.Statement);
            hashtable1.Add("return", SyntaxToken.Statement);
            hashtable1.Add("throw", SyntaxToken.Statement);
            hashtable1.Add("try", SyntaxToken.Statement);
            hashtable1.Add("catch", SyntaxToken.Statement);
            hashtable1.Add("finally", SyntaxToken.Statement);
            hashtable1.Add("checked", SyntaxToken.Statement);
            hashtable1.Add("unchecked", SyntaxToken.Statement);
            hashtable1.Add("fixed", SyntaxToken.Statement);
            hashtable1.Add("lock", SyntaxToken.Statement);
            hashtable1.Add("bool", SyntaxToken.DataType);
            hashtable1.Add("byte", SyntaxToken.DataType);
            hashtable1.Add("char", SyntaxToken.DataType);
            hashtable1.Add("decimal", SyntaxToken.DataType);
            hashtable1.Add("double", SyntaxToken.DataType);
            hashtable1.Add("float", SyntaxToken.DataType);
            hashtable1.Add("int", SyntaxToken.DataType);
            hashtable1.Add("long", SyntaxToken.DataType);
            hashtable1.Add("object", SyntaxToken.DataType);
            hashtable1.Add("sbyte", SyntaxToken.DataType);
            hashtable1.Add("short", SyntaxToken.DataType);
            hashtable1.Add("string", SyntaxToken.DataType);
            hashtable1.Add("uint", SyntaxToken.DataType);
            hashtable1.Add("ulong", SyntaxToken.DataType);
            hashtable1.Add("ushort", SyntaxToken.DataType);
            hashtable1.Add("void", SyntaxToken.DataType);
            hashtable1.Add("class", SyntaxToken.Declaration);
            hashtable1.Add("delegate", SyntaxToken.Declaration);
            hashtable1.Add("enum", SyntaxToken.Declaration);
            hashtable1.Add("event", SyntaxToken.Declaration);
            hashtable1.Add("interface", SyntaxToken.Declaration);
            hashtable1.Add("namespace", SyntaxToken.Declaration);
            hashtable1.Add("struct", SyntaxToken.Declaration);
            hashtable1.Add("using", SyntaxToken.Declaration);
            hashtable1.Add("base", SyntaxToken.Resword);
            hashtable1.Add("in", SyntaxToken.Resword);
            hashtable1.Add("stackalloc", SyntaxToken.Resword);
            hashtable1.Add("this", SyntaxToken.Resword);
            hashtable1.Add("null", SyntaxToken.Value);
            hashtable1.Add("false", SyntaxToken.Value);
            hashtable1.Add("true", SyntaxToken.Value);
            hashtable1.Add("extern", SyntaxToken.Scope);
            hashtable1.Add("internal", SyntaxToken.Scope);
            hashtable1.Add("private", SyntaxToken.Scope);
            hashtable1.Add("protected", SyntaxToken.Scope);
            hashtable1.Add("public", SyntaxToken.Scope);
            hashtable1.Add("abstract", SyntaxToken.Modifier);
            hashtable1.Add("override", SyntaxToken.Modifier);
            hashtable1.Add("const", SyntaxToken.Modifier);
            hashtable1.Add("virtual", SyntaxToken.Modifier);
            hashtable1.Add("sealed", SyntaxToken.Modifier);
            hashtable1.Add("static", SyntaxToken.Modifier);
            hashtable1.Add("unsafe", SyntaxToken.Modifier);
            hashtable1.Add("readonly", SyntaxToken.Modifier);
            hashtable1.Add("explicit", SyntaxToken.Modifier);
            hashtable1.Add("implicit", SyntaxToken.Modifier);
            hashtable1.Add("volatile", SyntaxToken.Modifier);
            hashtable1.Add("new", SyntaxToken.Modifier);
            hashtable1.Add("operator", SyntaxToken.Modifier);
            hashtable1.Add("partial", SyntaxToken.Modifier);
            hashtable1.Add("out", SyntaxToken.Param);
            hashtable1.Add("ref", SyntaxToken.Param);
            hashtable1.Add("params", SyntaxToken.Param);
            hashtable1.Add("get", SyntaxToken.Accessor);
            hashtable1.Add("set", SyntaxToken.Accessor);
        }

        protected override void InitRules()
        {
            base.InitRules();
            base.RegisterLexerProc(0, new char[3] { '@', '"', '\'' } , this.lexStringProc);
            base.RegisterLexerProc(0, '/', this.lexCommentProc);
            base.RegisterLexerProc(0, '#', this.lexDefineProc);
            char[] chArray1 = new char[2] { '+', '-' } ;
            base.RegisterLexerProc(0, chArray1, this.lexNumberProc);
            base.RegisterLexerProc(1, this.lexCommentEndProc);
            base.RegisterLexerProc(2, this.lexXmlCommentProc);
            base.RegisterLexerProc(2, '<', this.lexXmlCommentTagProc);
        }

        protected override void InitStyles()
        {
            base.InitStyles();
            base.Scheme.Name = "C#";
            base.Scheme.Desc = "Syntax Scheme for C# Language";
        }

        protected override void InitSyntax()
        {
            this.parseBlockProc = new SourceParser.ParseProc(this.ParseBlock);
            this.parseDelegateProc = new SourceParser.ParseProc(this.ParseDelegate);
            this.parseEventProc = new SourceParser.ParseProc(this.ParseEvent);
            this.parsePropertyProc = new SourceParser.ParseProc(this.ParseProperty);
            this.parseStatementsProc = new SourceParser.ParseProc(this.ParseStatements);
            this.parseEnumProc = new SourceParser.ParseProc(this.ParseEnum);
            this.parseAccessorProc = new SourceParser.ParseProc(this.ParseAccessor);
            this.parseMethodProc = new SourceParser.ParseProc(this.ParseMethod);
            this.lexStringProc = new LexerProc(this.LexString);
            this.lexCommentProc = new LexerProc(this.LexComment);
            this.lexCommentEndProc = new LexerProc(this.LexCommentEnd);
            this.lexDefineProc = new LexerProc(this.LexDefine);
            this.lexXmlCommentProc = new LexerProc(this.LexXmlComment);
            this.lexXmlCommentTagProc = new LexerProc(this.LexXmlCommentTag);
            base.CaseSensitive = true;
            base.InitSyntax();
        }

        public override bool IsBlockEnd(string s)
        {
            return (s == "}");
        }

        private bool IsBlockStart()
        {
            if (base.Token == 5)
            {
                return (base.TokenString == "{");
            }
            return false;
        }

        public override bool IsBlockStart(string s)
        {
            return (s == "{");
        }

        protected override bool IsDeclarationEnd(string s)
        {
            return (s == ";");
        }

        private bool IsRegionEnd()
        {
            if (base.Token == 8)
            {
                return (base.TokenString == "#endregion");
            }
            return false;
        }

        protected virtual bool IsTypeSeparator()
        {
            return base.IsSymbolToken(":");
        }

        protected virtual int LexComment()
        {
            this.currentPos++;
            int num1 = this.source.Length;
            if (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                char ch2 = ch1;
                if (ch2 != '*')
                {
                    if (ch2 != '/')
                    {
                        goto Label_0099;
                    }
                    int num2 = this.currentPos + 1;
                    if ((num2 < num1) && (this.source[num2] == '/'))
                    {
                        this.currentPos = num2 + 1;
                        if (this.currentPos < num1)
                        {
                            base.State = 2;
                        }
                        return 4;
                    }
                    this.currentPos = num1 + 1;
                    return 3;
                }
                this.currentPos++;
                return this.LexCommentEnd();
            }
        Label_0099:
            return 5;
        }

        protected virtual int LexCommentEnd()
        {
            int num1 = this.source.Length;
            while (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                if (ch1 == '*')
                {
                    this.currentPos++;
                    if ((this.currentPos >= num1) || (this.source[this.currentPos] != '/'))
                    {
                        continue;
                    }
                    base.State = 0;
                    this.currentPos++;
                    return 3;
                }
                this.currentPos++;
            }
            base.State = 1;
            return 3;
        }

        protected virtual int LexDefine()
        {
            this.currentPos++;
            if (this.currentPos < this.source.Length)
            {
                char ch1 = this.source[this.currentPos];
                if ((((ch1 >= 'a') && (ch1 <= 'z')) || ((ch1 >= 'A') && (ch1 <= 'Z'))) || (ch1 == '_'))
                {
                    this.LexIdent();
                }
            }
            return 8;
        }

        protected virtual int LexHexNumber()
        {
            int num1 = this.source.Length;
            while (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                if ((((ch1 < '0') || (ch1 > '9')) && ((ch1 < 'a') || (ch1 > 'f'))) && ((ch1 < 'A') || (ch1 > 'F')))
                {
                    break;
                }
                this.currentPos++;
            }
            return 1;
        }

        protected override int LexNumber()
        {
            int num2;
            char ch1 = this.source[this.currentPos];
            int num1 = this.source.Length;
            if ((ch1 == '+') || (ch1 == '-'))
            {
                num2 = this.currentPos + 1;
                if (((num2 >= num1) || (this.source[num2] < '0')) || (this.source[num2] > '9'))
                {
                    this.currentPos++;
                    return 5;
                }
            }
            if (ch1 == '0')
            {
                num2 = this.currentPos + 1;
                if ((num2 < num1) && ((this.source[num2] == 'x') || (this.source[num2] == 'X')))
                {
                    this.currentPos = num2 + 1;
                    return this.LexHexNumber();
                }
            }
            base.LexNumber();
            if (this.currentPos < num1)
            {
                ch1 = this.source[this.currentPos];
                if ((ch1 == '.') && (this.currentPos < (num1 - 1)))
                {
                    ch1 = this.source[this.currentPos + 1];
                    if ((ch1 >= '0') && (ch1 <= '9'))
                    {
                        this.currentPos++;
                        base.LexNumber();
                    }
                }
            }
            if (this.currentPos < num1)
            {
                ch1 = this.source[this.currentPos];
                if ((ch1 == 'E') || (ch1 == 'e'))
                {
                    num2 = this.currentPos + 1;
                    if (num2 < num1)
                    {
                        ch1 = this.source[num2];
                        if ((ch1 == '+') || (ch1 == '-'))
                        {
                            num2++;
                        }
                    }
                    if (num2 < num1)
                    {
                        ch1 = this.source[num2];
                        if ((ch1 >= '0') && (ch1 <= '9'))
                        {
                            this.currentPos = num2;
                            base.LexNumber();
                        }
                    }
                }
            }
            return 1;
        }

        protected virtual int LexString()
        {
            char ch1 = this.source[this.currentPos];
            this.currentPos++;
            if (ch1 != '@')
            {
                return this.LexString(true, ch1);
            }
            int num1 = this.source.Length;
            if (this.currentPos < num1)
            {
                ch1 = this.source[this.currentPos];
                if (ch1 == '"')
                {
                    this.currentPos++;
                    return this.LexString(false, ch1);
                }
            }
            return 5;
        }

        protected virtual int LexString(bool Escapes, char StartChar)
        {
            int num1 = this.source.Length;
            bool flag1 = false;
            while (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                if ((ch1 == StartChar) && !flag1)
                {
                    this.currentPos++;
                    break;
                }
                if (Escapes)
                {
                    if (ch1 == '\\')
                    {
                        flag1 = !flag1;
                    }
                    else
                    {
                        flag1 = false;
                    }
                }
                this.currentPos++;
            }
            return 7;
        }

        protected virtual int LexXmlComment()
        {
            int num1 = this.source.Length;
            this.currentPos++;
            while (this.currentPos < num1)
            {
                if (this.source[this.currentPos] == '<')
                {
                    break;
                }
                this.currentPos++;
            }
            if (this.currentPos == num1)
            {
                base.State = 0;
            }
            return 4;
        }

        protected virtual int LexXmlCommentTag()
        {
            this.currentPos++;
            int num1 = this.source.Length;
            while (this.currentPos < num1)
            {
                if (this.source[this.currentPos] == '>')
                {
                    this.currentPos++;
                    break;
                }
                this.currentPos++;
            }
            if (this.currentPos == num1)
            {
                base.State = 0;
            }
            return 3;
        }

        public override int NextValidToken()
        {
            string text1 = (base.Token == 5) ? base.TokenString : string.Empty;
            int num1 = base.NextValidToken();
            if (((base.PrevLine != base.LineIndex) && (text1 != "{")) && ((text1 != "}") && (text1 != ";")))
            {
                base.LineBreaks.Add(base.LineIndex, true);
            }
            return num1;
        }

        protected void ParseAccessor(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            this.ParseStatements(Info, ((IAccessorInfo) Info).Statements, Level);
        }

        protected void ParseBlock(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            ISyntaxInfo info1 = this.currentBlock;
            this.currentBlock = Info;
            Point point1 = new Point(-1, -1);
            string text1 = string.Empty;
            if (base.TokenString == "[")
            {
                point1 = new Point(this.tokenPos, base.LineIndex);
                while (base.TokenString == "[")
                {
                    base.SkipBrackets("]", ref text1);
                }
            }
            Point point2 = new Point(this.tokenPos, base.LineIndex);
            string text2 = base.TokenString;
            if (base.Token == 8)
            {
                if (text2 == "#region")
                {
                    this.ParseRegion(Info, Infos, Level, this.parseBlockProc);
                }
                else
                {
                    this.NextValidToken();
                }
            }
            else if (base.Token != 2)
            {
                if (base.Token == 0)
                {
                    this.ParseDeclaration(CsScope.None, CsModifier.None, text2, Info, point2, point1, text1, Level);
                }
                else
                {
                    this.NextValidToken();
                    this.currentBlock = info1;
                }
            }
            else
            {
                switch (((SyntaxToken) base.GetReswordToken(text2)))
                {
                    case SyntaxToken.DataType:
                    case SyntaxToken.Declaration:
                    {
                        this.ParseDeclaration(CsScope.None, CsModifier.None, text2, Info, point2, point1, text1, Level);
                        return;
                    }
                    case SyntaxToken.Scope:
                    case SyntaxToken.Modifier:
                    {
                        CsScope scope1 = CsScope.None;
                        CsModifier modifier1 = CsModifier.None;
                        this.GetScope(out scope1, out modifier1);
                        if (base.IsReswordOrIdentifier(base.Token))
                        {
                            this.ParseDeclaration(scope1, modifier1, base.TokenString, Info, point2, point1, text1, Level);
                        }
                        return;
                    }
                }
                this.NextValidToken();
            }
        }

        private void ParseCaseStatement(IStatementInfo Info, ISyntaxInfos Infos, int Level)
        {
            if (this.SkipToSymbol(":"))
            {
                if (this.IsBlockStart())
                {
                    base.ParseBlock(Infos, Info, Level, null, this.parseStatementsProc);
                }
                else
                {
                    Info.DeclarationSize = new Size(this.currentPos - Info.Position.X, base.LineIndex - Info.Position.Y);
                    Info.StartPoint = new Point(base.PrevPos, base.PrevLine);
                    int num1 = 1;
                    while (!base.Eof && !this.IsBlockEnd(ref num1))
                    {
                        if ((base.Token == 2) && ((base.TokenString == "case") || (base.TokenString == "default")))
                        {
                            break;
                        }
                        this.ParseStatements(Info, Infos, Level + 1);
                    }
                    Info.EndPoint = new Point(base.PrevPos, base.PrevLine);
                    Infos.Add(Info);
                }
            }
        }

        private void ParseDeclaration(CsScope Scope, CsModifier Modifiers, string InfoType, ISyntaxInfo Info, Point Position, Point AttrPt, string Attributes, int Level)
        {
            if (base.GetReswordToken(InfoType) == 11)
            {
                this.ProcessDeclaration(Scope, Modifiers, InfoType, Info, Position, AttrPt, Attributes, Level);
            }
            else
            {
                string text1 = string.Empty;
                string text2 = InfoType;
                string text3 = string.Empty;
                switch (this.GetTypeAndName(ref text1, ref text2))
                {
                    case CsParserType.Field:
                    {
                        CsFieldInfo info1 = new CsFieldInfo(text1, text2, Position, Scope, AttrPt, Attributes);
                        if (Info is IClassInfo)
                        {
                            ((IClassInfo) Info).Fields.Add(info1);
                        }
                        return;
                    }
                    case CsParserType.Method:
                    {
                        if (!base.IsInterface(Info))
                        {
                            base.ParseBlock((Info is IInterfaceInfo) ? ((IInterfaceInfo) Info).Methods : null, new CsMethodInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes), Level, this.parseDelegateProc, this.parseMethodProc);
                            return;
                        }
                        base.ParseBlock(((IInterfaceInfo) Info).Methods, new CsDelegateInfo(text1, text2, Position, Scope, AttrPt, Attributes), Level, this.parseDelegateProc, null);
                        return;
                    }
                    case CsParserType.Property:
                    {
                        ICsPropInfo info2 = new CsPropInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes);
                        base.ParseBlock((Info is IInterfaceInfo) ? ((IInterfaceInfo) Info).Properties : null, info2, Level, null, this.parsePropertyProc);
                        if (base.IsInterface(Info))
                        {
                            if (info2.PropertyGet != null)
                            {
                                info2.PropertyGet.Visible = false;
                            }
                            if (info2.PropertySet != null)
                            {
                                info2.PropertySet.Visible = false;
                            }
                        }
                        return;
                    }
                }
            }
        }

        protected void ParseDelegate(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            this.ParseParams((IDelegateInfo) Info);
            this.NextValidToken();
        }

        protected void ParseEnum(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            if (base.Token == 0)
            {
                IEnumInfo info1 = (IEnumInfo) Info;
                info1.Fields.Add(new CsFieldInfo(base.TokenString, info1.DataType, new Point(this.tokenPos, base.LineIndex), CsScope.Public));
                while (!base.Eof)
                {
                    string text1;
                    if (base.NextValidToken(out text1) == 5)
                    {
                        if (text1 == ",")
                        {
                            return;
                        }
                        if (text1 == "}")
                        {
                            return;
                        }
                    }
                    this.NextValidToken();
                }
            }
            else
            {
                this.NextValidToken();
            }
        }

        protected void ParseEvent(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            if (base.Token != 0)
            {
                this.NextValidToken();
            }
            else
            {
                string text2;
                ICsEventInfo info1 = (ICsEventInfo) Info;
                string text1 = base.TokenString;
                if ((text2 = text1) != null)
                {
                    text2 = string.IsInterned(text2);
                    if (text2 != "add")
                    {
                        if (text2 == "remove")
                        {
                            info1.EventRemove = new AccessorInfo(text1, new Point(this.tokenPos, base.LineIndex), Level);
                            this.NextValidToken();
                            base.ParseBlock(null, info1.EventRemove, Level, null, this.parseAccessorProc);
                            return;
                        }
                    }
                    else
                    {
                        info1.EventAdd = new AccessorInfo(text1, new Point(this.tokenPos, base.LineIndex), Level);
                        this.NextValidToken();
                        base.ParseBlock(null, info1.EventAdd, Level, null, this.parseAccessorProc);
                        return;
                    }
                }
                this.NextValidToken();
            }
        }

        private void ParseIfElseStatement(IStatementInfo Info, ISyntaxInfos Infos, int Level)
        {
            if ((base.Token == 2) && (base.TokenString == "else"))
            {
                this.ParseStatement(base.TokenString, Infos, Level);
                ((CsIfStatementInfo) Info).IfEndPoint = Info.EndPoint;
                Info.EndPoint = new Point(base.PrevPos, base.PrevLine);
            }
        }

        private void ParseLocalVars(string InfoType, ISyntaxInfo Info, Point Position)
        {
            string text1 = string.Empty;
            string text2 = InfoType;
            if ((this.GetTypeAndName(ref text1, ref text2) == CsParserType.Field) && (Info is IHasLocalVars))
            {
                LocalVarInfo info1 = new LocalVarInfo(text1, text2, Position);
                ((IHasLocalVars) Info).LocalVars.Add(info1);
            }
        }

        protected void ParseMethod(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            this.ParseStatements(Info, ((IMethodInfo) Info).Statements, Level);
        }

        private void ParseParams(IDelegateInfo Info)
        {
            string text3;
            string text1 = string.Empty;
            string text2 = string.Empty;
            Point point1 = new Point(this.tokenPos, base.LineIndex);
            while (this.GetParamTypeAndName(ref text1, ref text2, ref point1, out text3))
            {
                Info.Params.Add(new ParamInfo(text1, text2, point1, text3));
                if (base.TokenString == ")")
                {
                    return;
                }
                text1 = string.Empty;
                text2 = string.Empty;
            }
        }

        protected void ParseProperty(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            if (base.Token != 2)
            {
                this.NextValidToken();
            }
            else
            {
                string text2;
                ICsPropInfo info1 = (ICsPropInfo) Info;
                string text1 = base.TokenString;
                if ((text2 = text1) != null)
                {
                    text2 = string.IsInterned(text2);
                    if (text2 != "get")
                    {
                        if (text2 == "set")
                        {
                            info1.PropertySet = new AccessorInfo(text1, new Point(this.tokenPos, base.LineIndex), Level);
                            this.NextValidToken();
                            base.ParseBlock(null, info1.PropertySet, Level, null, this.parseAccessorProc);
                            return;
                        }
                    }
                    else
                    {
                        info1.PropertyGet = new AccessorInfo(text1, new Point(this.tokenPos, base.LineIndex), Level);
                        this.NextValidToken();
                        base.ParseBlock(null, info1.PropertyGet, Level, null, this.parseAccessorProc);
                        return;
                    }
                }
                this.NextValidToken();
            }
        }

        protected void ParseRegion(ISyntaxInfo Info, ISyntaxInfos Infos, int Level, SourceParser.ParseProc Proc)
        {
            string text1 = base.Strings[base.LineIndex].Substring(this.currentPos);
            RegionInfo info1 = new RegionInfo(text1, new Point(this.tokenPos, base.LineIndex), Level);
            base.SkipToNextLine();
            if (Info is IRangeInfo)
            {
                ((IRangeInfo) Info).Regions.Add(info1);
            }
            while (!base.Eof && !this.IsRegionEnd())
            {
                Proc(Info, Infos, Level);
            }
            info1.EndPoint = this.ValidatePosition((this.source != null) ? this.source.Length : 0, base.LineIndex);
            base.SkipToNextLine();
        }

        private IStatementInfo ParseStatement(string Name, ISyntaxInfos Infos, int Level)
        {
            string text1;
            IStatementInfo info1 = null;
            if ((text1 = Name) != null)
            {
                text1 = string.IsInterned(text1);
                if (text1 != "if")
                {
                    if ((text1 == "case") || (text1 == "default"))
                    {
                        info1 = new CsStatementInfo(Name, new Point(this.tokenPos, base.LineIndex), Level);
                        this.ParseCaseStatement(info1, Infos, Level);
                        return info1;
                    }
                }
                else
                {
                    info1 = new CsIfStatementInfo(Name, new Point(this.tokenPos, base.LineIndex), Level);
                    goto Label_0091;
                }
            }
            info1 = new CsStatementInfo(Name, new Point(this.tokenPos, base.LineIndex), Level);
        Label_0091:
            this.NextValidToken();
            if (base.IsSymbolToken("("))
            {
                base.SkipBrackets(")");
            }
            if ((base.Token == 2) && (base.GetReswordToken(base.TokenString) == 2))
            {
                info1.StartPoint = new Point(base.PrevPos, base.PrevLine);
                info1.DeclarationSize = new Size(this.tokenPos - info1.Position.X, base.LineIndex - info1.Position.Y);
                if ((Name == "else") && (base.TokenString == "if"))
                {
                    IStatementInfo info2 = this.ParseStatement(base.TokenString, Infos, Level);
                    ((CsIfStatementInfo) info2).ElseIfPoint = new Point(info1.Position.X + info1.DeclarationSize.Width, info1.Position.Y + info1.DeclarationSize.Height);
                }
                else
                {
                    this.ParseStatement(base.TokenString, Infos, Level + 1);
                }
                info1.EndPoint = new Point(base.PrevPos, base.PrevLine);
                Infos.Add(info1);
            }
            else
            {
                base.ParseBlock(Infos, info1, Level, null, this.parseStatementsProc);
            }
            if (info1 is CsIfStatementInfo)
            {
                this.ParseIfElseStatement(info1, Infos, Level);
            }
            return info1;
        }

        protected void ParseStatements(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            string text1 = base.TokenString;
            if (base.Token == 2)
            {
                int num1 = base.GetReswordToken(text1);
                if (num1 == 2)
                {
                    this.ParseStatement(text1, Infos, Level);
                }
                else if (num1 == 3)
                {
                    this.ParseLocalVars(text1, Info, new Point(this.tokenPos, base.LineIndex));
                }
                else
                {
                    this.NextValidToken();
                }
            }
            else if ((base.Token == 8) && (base.TokenString == "#region"))
            {
                this.ParseRegion(Info, Infos, Level, this.parseStatementsProc);
            }
            else if (base.Token == 0)
            {
                this.ParseLocalVars(text1, Info, new Point(this.tokenPos, base.LineIndex));
            }
            else
            {
                this.NextValidToken();
            }
        }

        protected virtual void ProcessDeclaration(CsScope Scope, CsModifier Modifiers, string InfoType, ISyntaxInfo Info, Point Position, Point AttrPt, string Attributes, int Level)
        {
            string text3;
            string text1 = string.Empty;
            string text2 = string.Empty;
            if ((text3 = InfoType) != null)
            {
                text3 = string.IsInterned(text3);
                if (text3 != "using")
                {
                    char[] chArray1;
                    if (text3 == "namespace")
                    {
                        if (this.GetName(ref text1, "."))
                        {
                            base.ParseBlock((Info is ICsUnitInfo) ? ((ICsUnitInfo) Info).Namespaces : null, new CsNamespaceInfo(text1, Position, Level), Level, null, this.parseBlockProc);
                        }
                        return;
                    }
                    if (text3 == "interface")
                    {
                        if (this.GetNameAndType(out text1, out text2))
                        {
                            chArray1 = new char[1] { ',' } ;
                            base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Interfaces : null, new CsInterfaceInfo(text1, text2.Split(chArray1), Position, Level, Scope, AttrPt, Attributes), Level, null, this.parseBlockProc);
                        }
                        return;
                    }
                    if (text3 == "class")
                    {
                        if (this.GetNameAndType(out text1, out text2))
                        {
                            chArray1 = new char[1] { ',' } ;
                            base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Classes : null, new CsClassInfo(text1, text2.Split(chArray1), Position, Level, Scope, Modifiers, AttrPt, Attributes), Level, null, this.parseBlockProc);
                        }
                        return;
                    }
                    if (text3 == "struct")
                    {
                        if (this.GetNameAndType(out text1, out text2))
                        {
                            chArray1 = new char[1] { ',' } ;
                            base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Structures : null, new CsStructInfo(text1, text2.Split(chArray1), Position, Level, Scope, Modifiers, AttrPt, Attributes), Level, null, this.parseBlockProc);
                        }
                        return;
                    }
                    if (text3 == "delegate")
                    {
                        if (this.GetTypeAndName(ref text1, ref text2) == CsParserType.Method)
                        {
                            base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Delegates : null, new CsDelegateInfo(text1, text2, Position, Scope, AttrPt, Attributes), Level, this.parseDelegateProc, null);
                        }
                        return;
                    }
                    if (text3 == "enum")
                    {
                        if (this.GetNameAndType(out text1, out text2))
                        {
                            base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Enums : null, new CsEnumInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes), Level, null, this.parseEnumProc);
                        }
                        return;
                    }
                    if (text3 == "event")
                    {
                        CsParserType type1 = this.GetTypeAndName(ref text1, ref text2);
                        ICsEventInfo info1 = new CsEventInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes);
                        if (type1 == CsParserType.Property)
                        {
                            base.ParseBlock((Info is IInterfaceInfo) ? ((IInterfaceInfo) Info).Events : null, info1, Level, null, this.parseEventProc);
                            if (base.IsInterface(Info))
                            {
                                if (info1.EventAdd != null)
                                {
                                    info1.EventAdd.Visible = false;
                                }
                                if (info1.EventRemove == null)
                                {
                                    return;
                                }
                                info1.EventRemove.Visible = false;
                            }
                            return;
                        }
                        if ((type1 == CsParserType.Field) && (Info is IInterfaceInfo))
                        {
                            ((InterfaceInfo) Info).Events.Add(info1);
                        }
                        return;
                    }
                }
                else
                {
                    this.ProcessUsing(Info, Position, Level);
                    return;
                }
            }
            this.NextValidToken();
        }

        protected virtual void ProcessUsing(ISyntaxInfo Info, Point Position, int Level)
        {
            Point point1 = new Point(this.source.Length, base.LineIndex);
            Point point2 = new Point(this.currentPos, base.LineIndex);
            string text1 = string.Empty;
            if (this.GetName(ref text1, "."))
            {
                IUsesInfo info1 = null;
                if (Info is ICsUnitInfo)
                {
                    info1 = ((ICsUnitInfo) Info).Uses;
                }
                else if (Info is ICsNamespaceInfo)
                {
                    info1 = ((ICsNamespaceInfo) Info).Uses;
                }
                if (info1 != null)
                {
                    if (info1.Uses.Count == 0)
                    {
                        info1.StartPoint = point2;
                        info1.Position = Position;
                    }
                    info1.Uses.Add(new UsesInfo(text1, Position, Level));
                    info1.EndPoint = point1;
                }
            }
        }

        public override void ReparseText()
        {
            this.Reset();
            this.NextValidToken();
            while (!base.Eof)
            {
                this.ParseBlock(base.UnitInfo, null, 0);
            }
            base.ReparseText();
        }

        private bool SkipToSymbol(string symbol)
        {
            while (!base.Eof)
            {
                if ((base.Token == 5) || (base.Token == 2))
                {
                    string text1 = base.TokenString;
                    if (text1 == symbol)
                    {
                        this.NextValidToken();
                        return true;
                    }
                    if (this.IsBlockStart(text1) || this.IsBlockEnd(text1))
                    {
                        return false;
                    }
                }
                this.NextValidToken();
            }
            return false;
        }


        // Fields
        protected LexerProc lexCommentEndProc;
        protected LexerProc lexCommentProc;
        protected LexerProc lexDefineProc;
        protected LexerProc lexStringProc;
        protected LexerProc lexXmlCommentProc;
        protected LexerProc lexXmlCommentTagProc;
        protected SourceParser.ParseProc parseAccessorProc;
        protected SourceParser.ParseProc parseBlockProc;
        protected SourceParser.ParseProc parseDelegateProc;
        protected SourceParser.ParseProc parseEnumProc;
        protected SourceParser.ParseProc parseEventProc;
        protected SourceParser.ParseProc parseMethodProc;
        protected SourceParser.ParseProc parsePropertyProc;
        protected SourceParser.ParseProc parseStatementsProc;

        // Nested Types
        protected enum CsParserType
        {
            // Fields
            Field = 1,
            Method = 2,
            None = 0,
            Param = 4,
            Property = 3
        }
    }
}

