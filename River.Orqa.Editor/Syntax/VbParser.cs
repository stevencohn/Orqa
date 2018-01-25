namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(VbParser), "Images.VbParser.bmp"), ToolboxItem(true)]
    public class VbParser : SourceParser
    {
        // Methods
        public VbParser()
        {
        }

        protected override void AfterParseBlock()
        {
            if (this.IsBlockEnd(base.TokenString))
            {
                this.NextValidToken();
            }
        }

        protected override bool BeforeParseBlock(ref Point BlockPt)
        {
            BlockPt = new Point(base.PrevPos, base.PrevLine);
            return true;
        }

        protected override IUnitInfo CreateUnitInfo()
        {
            return new VbUnitInfo();
        }

        protected bool GetClassNameAndType(out string AName, out string AType)
        {
            AType = string.Empty;
            if (base.NextValidToken(out AName) != 0)
            {
                return false;
            }
            return this.GetType(out AType);
        }

        protected bool GetName(ref string Name, string Delimiter)
        {
            Name = string.Empty;
            int num1 = base.LineIndex;
            while (!base.Eof)
            {
                string text1;
                int num2 = base.NextValidToken(out text1);
                if (((num2 == 0) || base.IsSymbolToken(num2, text1, Delimiter)) || (base.TokenString.ToLower() == "new"))
                {
                    Name = Name + text1;
                    continue;
                }
                if (((num2 == 2) || base.IsSymbolToken(num2, text1, "(")) || (num1 != base.LineIndex))
                {
                    break;
                }
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
            return this.GetSimpleType(out AType);
        }

        protected bool GetParamNameAndType(ref string AName, ref string AType, ref Point Position, out string Qualifier)
        {
            Qualifier = string.Empty;
            base.NextValidToken(out AName);
            Position = new Point(this.tokenPos, base.LineIndex);
            if (AName == "<")
            {
                if (!base.SkipBrackets(">"))
                {
                    return false;
                }
                AName = base.TokenString;
            }
            if (base.GetReswordToken(AName) == 8)
            {
                Qualifier = AName;
                base.NextValidToken(out AName);
            }
            if ((base.Token != 0) || !this.GetSimpleType(out AType))
            {
                return false;
            }
            string text1 = base.TokenString;
            if (base.Token != 5)
            {
                return false;
            }
            if (text1 != ",")
            {
                return (text1 == ")");
            }
            return true;
        }

        protected void GetScope(out VbScope Scope, out VbModifier Modifiers)
        {
            Scope = VbScope.None;
            Modifiers = VbModifier.None;
            while (base.Token == 2)
            {
                string text1 = base.TokenString.ToLower();
                int num1 = base.GetReswordToken(text1);
                if (num1 == 6)
                {
                    Scope |= ((VbScope) Enum.Parse(typeof(VbScope), text1, true));
                }
                else
                {
                    if (num1 != 7)
                    {
                        return;
                    }
                    Modifiers |= ((VbModifier) Enum.Parse(typeof(VbModifier), text1, true));
                }
                this.NextValidToken();
            }
        }

        private bool GetSimpleType(out string AType)
        {
            string text1;
            AType = string.Empty;
            base.NextValidToken(out text1);
            text1 = text1.ToLower();
            if ((base.Token == 2) && (text1 == "as"))
            {
                while (!base.Eof)
                {
                    base.NextValidToken(out text1);
                    if (base.IsReswordOrIdentifier(base.Token))
                    {
                        AType = AType + text1;
                        base.NextValidToken(out text1);
                        if (!base.IsSymbolToken("."))
                        {
                            break;
                        }
                        AType = AType + text1;
                    }
                }
            }
            return (AType != string.Empty);
        }

        private bool GetType(out string AType)
        {
            AType = string.Empty;
            string text1 = base.TokenString.ToLower();
            if ((base.Token == 2) && (((text1 == "as") || (text1 == "implements")) || (text1 == "inherits")))
            {
                int num1 = base.LineIndex;
                while (!base.Eof)
                {
                    switch (base.NextValidToken(out text1))
                    {
                        case 0:
                        {
                            if (num1 != base.LineIndex)
                            {
                                return true;
                            }
                            AType = AType + text1;
                            continue;
                        }
                        case 1:
                        {
                            goto Label_00FC;
                        }
                        case 2:
                        {
                            if (text1 != "implements")
                            {
                                goto Label_009D;
                            }
                            num1 = base.LineIndex;
                            AType = AType + ",";
                            continue;
                        }
                        case 5:
                        {
                            goto Label_00CB;
                        }
                        default:
                        {
                            goto Label_00FC;
                        }
                    }
                Label_009D:
                    if (num1 == base.LineIndex)
                    {
                        AType = AType + text1;
                        continue;
                    }
                    return true;
                Label_00CB:
                    if ((num1 != base.LineIndex) || ((text1 != ",") && (text1 != ".")))
                    {
                        return true;
                    }
                    AType = AType + text1;
                    continue;
                Label_00FC:
                    if (num1 != base.LineIndex)
                    {
                        return true;
                    }
                }
            }
            else if (text1 == "mybase")
            {
                base.SkipToNextLine();
            }
            return true;
        }

        protected override void InitReswords()
        {
            base.InitReswords();
            Hashtable hashtable1 = base.Reswords;
            hashtable1.Add("as", SyntaxToken.Operator);
            hashtable1.Add("and", SyntaxToken.Operator);
            hashtable1.Add("andalso", SyntaxToken.Operator);
            hashtable1.Add("is", SyntaxToken.Operator);
            hashtable1.Add("like", SyntaxToken.Operator);
            hashtable1.Add("typeof", SyntaxToken.Operator);
            hashtable1.Add("gettype", SyntaxToken.Operator);
            hashtable1.Add("mod", SyntaxToken.Operator);
            hashtable1.Add("not", SyntaxToken.Operator);
            hashtable1.Add("or", SyntaxToken.Operator);
            hashtable1.Add("orelse", SyntaxToken.Operator);
            hashtable1.Add("xor", SyntaxToken.Operator);
            hashtable1.Add("if", SyntaxToken.Statement);
            hashtable1.Add("else", SyntaxToken.Statement);
            hashtable1.Add("elseif", SyntaxToken.Statement);
            hashtable1.Add("select", SyntaxToken.Statement);
            hashtable1.Add("case", SyntaxToken.Statement);
            hashtable1.Add("do", SyntaxToken.Statement);
            hashtable1.Add("for", SyntaxToken.Statement);
            hashtable1.Add("each", SyntaxToken.Statement);
            hashtable1.Add("while", SyntaxToken.Statement);
            hashtable1.Add("default", SyntaxToken.Statement);
            hashtable1.Add("goto", SyntaxToken.Statement);
            hashtable1.Add("throw", SyntaxToken.Statement);
            hashtable1.Add("try", SyntaxToken.Statement);
            hashtable1.Add("catch", SyntaxToken.Statement);
            hashtable1.Add("finally", SyntaxToken.Statement);
            hashtable1.Add("loop", SyntaxToken.Statement);
            hashtable1.Add("synclock", SyntaxToken.Statement);
            hashtable1.Add("end", SyntaxToken.Statement);
            hashtable1.Add("error", SyntaxToken.Statement);
            hashtable1.Add("exit", SyntaxToken.Statement);
            hashtable1.Add("next", SyntaxToken.Statement);
            hashtable1.Add("on", SyntaxToken.Statement);
            hashtable1.Add("option", SyntaxToken.Statement);
            hashtable1.Add("raiseevent", SyntaxToken.Statement);
            hashtable1.Add("redim", SyntaxToken.Statement);
            hashtable1.Add("removehandler", SyntaxToken.Statement);
            hashtable1.Add("resume", SyntaxToken.Statement);
            hashtable1.Add("step", SyntaxToken.Statement);
            hashtable1.Add("stop", SyntaxToken.Statement);
            hashtable1.Add("until", SyntaxToken.Statement);
            hashtable1.Add("when", SyntaxToken.Statement);
            hashtable1.Add("with", SyntaxToken.Statement);
            hashtable1.Add("boolean", SyntaxToken.DataType);
            hashtable1.Add("byte", SyntaxToken.DataType);
            hashtable1.Add("char", SyntaxToken.DataType);
            hashtable1.Add("date", SyntaxToken.DataType);
            hashtable1.Add("decimal", SyntaxToken.DataType);
            hashtable1.Add("double", SyntaxToken.DataType);
            hashtable1.Add("integer", SyntaxToken.DataType);
            hashtable1.Add("long", SyntaxToken.DataType);
            hashtable1.Add("object", SyntaxToken.DataType);
            hashtable1.Add("short", SyntaxToken.DataType);
            hashtable1.Add("single", SyntaxToken.DataType);
            hashtable1.Add("string", SyntaxToken.DataType);
            hashtable1.Add("variant", SyntaxToken.DataType);
            hashtable1.Add("class", SyntaxToken.Declaration);
            hashtable1.Add("const", SyntaxToken.Declaration);
            hashtable1.Add("delegate", SyntaxToken.Declaration);
            hashtable1.Add("dim", SyntaxToken.Declaration);
            hashtable1.Add("enum", SyntaxToken.Declaration);
            hashtable1.Add("event", SyntaxToken.Declaration);
            hashtable1.Add("function", SyntaxToken.Declaration);
            hashtable1.Add("imports", SyntaxToken.Declaration);
            hashtable1.Add("interface", SyntaxToken.Declaration);
            hashtable1.Add("module", SyntaxToken.Declaration);
            hashtable1.Add("namespace", SyntaxToken.Declaration);
            hashtable1.Add("property", SyntaxToken.Declaration);
            hashtable1.Add("structure", SyntaxToken.Declaration);
            hashtable1.Add("sub", SyntaxToken.Declaration);
            hashtable1.Add("mybase", SyntaxToken.Resword);
            hashtable1.Add("myclass", SyntaxToken.Resword);
            hashtable1.Add("me", SyntaxToken.Resword);
            hashtable1.Add("addhandler", SyntaxToken.Resword);
            hashtable1.Add("addressof", SyntaxToken.Resword);
            hashtable1.Add("ansi", SyntaxToken.Resword);
            hashtable1.Add("assembly", SyntaxToken.Resword);
            hashtable1.Add("auto", SyntaxToken.Resword);
            hashtable1.Add("aliac", SyntaxToken.Resword);
            hashtable1.Add("call", SyntaxToken.Resword);
            hashtable1.Add("cbool", SyntaxToken.Resword);
            hashtable1.Add("cbyte", SyntaxToken.Resword);
            hashtable1.Add("cchar", SyntaxToken.Resword);
            hashtable1.Add("cdate", SyntaxToken.Resword);
            hashtable1.Add("cdec", SyntaxToken.Resword);
            hashtable1.Add("cdbl", SyntaxToken.Resword);
            hashtable1.Add("cint", SyntaxToken.Resword);
            hashtable1.Add("clng", SyntaxToken.Resword);
            hashtable1.Add("cobj", SyntaxToken.Resword);
            hashtable1.Add("cshort", SyntaxToken.Resword);
            hashtable1.Add("csng", SyntaxToken.Resword);
            hashtable1.Add("cstr", SyntaxToken.Resword);
            hashtable1.Add("ctype", SyntaxToken.Resword);
            hashtable1.Add("declare", SyntaxToken.Resword);
            hashtable1.Add("directcast", SyntaxToken.Resword);
            hashtable1.Add("erase", SyntaxToken.Resword);
            hashtable1.Add("gosub", SyntaxToken.Resword);
            hashtable1.Add("handles", SyntaxToken.Resword);
            hashtable1.Add("implements", SyntaxToken.Resword);
            hashtable1.Add("inherits", SyntaxToken.Resword);
            hashtable1.Add("let", SyntaxToken.Resword);
            hashtable1.Add("lib", SyntaxToken.Resword);
            hashtable1.Add("new", SyntaxToken.Resword);
            hashtable1.Add("preserve", SyntaxToken.Resword);
            hashtable1.Add("return", SyntaxToken.Resword);
            hashtable1.Add("then", SyntaxToken.Resword);
            hashtable1.Add("to", SyntaxToken.Resword);
            hashtable1.Add("unicode", SyntaxToken.Resword);
            hashtable1.Add("withevents", SyntaxToken.Resword);
            hashtable1.Add("nothing", SyntaxToken.Value);
            hashtable1.Add("false", SyntaxToken.Value);
            hashtable1.Add("true", SyntaxToken.Value);
            hashtable1.Add("private", SyntaxToken.Scope);
            hashtable1.Add("protected", SyntaxToken.Scope);
            hashtable1.Add("friend", SyntaxToken.Scope);
            hashtable1.Add("public", SyntaxToken.Scope);
            hashtable1.Add("static", SyntaxToken.Scope);
            hashtable1.Add("mustoverride", SyntaxToken.Modifier);
            hashtable1.Add("mustinherit", SyntaxToken.Modifier);
            hashtable1.Add("overrides", SyntaxToken.Modifier);
            hashtable1.Add("overloads", SyntaxToken.Modifier);
            hashtable1.Add("overridable", SyntaxToken.Modifier);
            hashtable1.Add("notoverridable", SyntaxToken.Modifier);
            hashtable1.Add("notinheritable", SyntaxToken.Modifier);
            hashtable1.Add("readonly", SyntaxToken.Modifier);
            hashtable1.Add("shadows", SyntaxToken.Modifier);
            hashtable1.Add("shared", SyntaxToken.Modifier);
            hashtable1.Add("writeonly", SyntaxToken.Modifier);
            hashtable1.Add("byval", SyntaxToken.Param);
            hashtable1.Add("byref", SyntaxToken.Param);
            hashtable1.Add("optional", SyntaxToken.Param);
            hashtable1.Add("paramarray", SyntaxToken.Param);
            hashtable1.Add("get", SyntaxToken.Accessor);
            hashtable1.Add("set", SyntaxToken.Accessor);
            this.blocks = new Hashtable();
            this.blocks.Add("if", SyntaxToken.BlockStart);
            this.blocks.Add("select", SyntaxToken.BlockStart);
            this.blocks.Add("do", SyntaxToken.BlockStart);
            this.blocks.Add("for", SyntaxToken.BlockStart);
            this.blocks.Add("try", SyntaxToken.BlockStart);
            this.blocks.Add("with", SyntaxToken.BlockStart);
            this.blocks.Add("class", SyntaxToken.BlockStart);
            this.blocks.Add("enum", SyntaxToken.BlockStart);
            this.blocks.Add("function", SyntaxToken.BlockStart);
            this.blocks.Add("interface", SyntaxToken.BlockStart);
            this.blocks.Add("module", SyntaxToken.BlockStart);
            this.blocks.Add("namespace", SyntaxToken.BlockStart);
            this.blocks.Add("structure", SyntaxToken.BlockStart);
            this.blocks.Add("property", SyntaxToken.BlockStart);
            this.blocks.Add("sub", SyntaxToken.BlockStart);
            this.blocks.Add("end", SyntaxToken.BlockEnd);
            this.blocks.Add("next", SyntaxToken.BlockEnd);
            this.blocks.Add("loop", SyntaxToken.BlockEnd);
        }

        protected override void InitRules()
        {
            base.InitRules();
            char[] chArray1 = new char[1] { '"' } ;
            base.RegisterLexerProc(0, chArray1, this.lexStringProc);
            base.RegisterLexerProc(0, '\'', this.lexCommentProc);
            base.RegisterLexerProc(0, '#', this.lexDefineProc);
            chArray1 = new char[2] { '+', '-' } ;
            base.RegisterLexerProc(0, chArray1, this.lexNumberProc);
        }

        protected override void InitStyles()
        {
            base.InitStyles();
            base.Scheme.Name = "Visual Basic.NET";
            base.Scheme.Desc = "Syntax Scheme for Visual Basic Language";
        }

        protected override void InitSyntax()
        {
            this.parseBlockProc = new SourceParser.ParseProc(this.ParseBlock);
            this.parsePropertyProc = new SourceParser.ParseProc(this.ParseProperty);
            this.parseStatementsProc = new SourceParser.ParseProc(this.ParseStatements);
            this.parseEnumProc = new SourceParser.ParseProc(this.ParseEnum);
            this.parseAccessorProc = new SourceParser.ParseProc(this.ParseAccessor);
            this.parseDelegateProc = new SourceParser.ParseProc(this.ParseDelegate);
            this.parseMethodProc = new SourceParser.ParseProc(this.ParseMethod);
            this.lexStringProc = new LexerProc(this.LexString);
            this.lexCommentProc = new LexerProc(this.LexComment);
            this.lexDefineProc = new LexerProc(this.LexDefine);
            base.CaseSensitive = false;
            base.InitSyntax();
        }

        public override bool IsBlockEnd(string s)
        {
            if (base.Token == 2)
            {
                object obj1 = this.blocks[s.ToLower()];
                if (obj1 != null)
                {
                    return (((SyntaxToken) obj1) == SyntaxToken.BlockEnd);
                }
            }
            return false;
        }

        protected override bool IsBlockEnd(ref int BlockCount)
        {
            string text1 = base.TokenString;
            if (((base.Token == 5) || (base.Token == 2)) && this.IsBlockEnd(text1))
            {
                return true;
            }
            return false;
        }

        public override bool IsBlockStart(string s)
        {
            if (base.Token == 2)
            {
                object obj1 = this.blocks[base.TokenString.ToLower()];
                if (obj1 != null)
                {
                    return (((SyntaxToken) obj1) == SyntaxToken.BlockStart);
                }
            }
            return false;
        }

        protected bool IsRegionEnd()
        {
            string text1 = base.TokenString.ToLower();
            if ((base.Token == 8) && (base.TokenString.ToLower() == "#end"))
            {
                base.NextValidToken(out text1);
                if (text1.ToLower() == "region")
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual int LexComment()
        {
            this.currentPos = this.source.Length + 1;
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

        protected override int LexIdentifier()
        {
            int num1 = base.LexIdentifier();
            if ((num1 == 2) && (base.TokenString.ToLower() == "rem"))
            {
                num1 = this.LexComment();
            }
            return num1;
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
            int num1 = this.source.Length;
            return this.LexString(ch1);
        }

        protected virtual int LexString(char StartChar)
        {
            int num1 = this.source.Length;
            while (this.currentPos < num1)
            {
                char ch1 = this.source[this.currentPos];
                if (ch1 == StartChar)
                {
                    this.currentPos++;
                    break;
                }
                this.currentPos++;
            }
            return 7;
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
            if (base.TokenString == "<")
            {
                point1 = new Point(this.tokenPos, base.LineIndex);
                while (base.TokenString == "<")
                {
                    base.SkipBrackets(">", ref text1);
                }
            }
            Point point2 = new Point(this.tokenPos, base.LineIndex);
            string text2 = base.TokenString.ToLower();
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
                    this.ParseDeclaration(VbScope.None, VbModifier.None, text2, Info, point2, point1, text1, Level);
                }
                else
                {
                    this.NextValidToken();
                    this.currentBlock = info1;
                }
            }
            else
            {
                switch (base.GetReswordToken(text2))
                {
                    case 3:
                    case 11:
                    {
                        this.ParseDeclaration(VbScope.None, VbModifier.None, text2, Info, point2, point1, text1, Level);
                        return;
                    }
                    case 6:
                    case 7:
                    {
                        VbScope scope1 = VbScope.None;
                        VbModifier modifier1 = VbModifier.None;
                        this.GetScope(out scope1, out modifier1);
                        if (base.IsReswordOrIdentifier(base.Token))
                        {
                            this.ParseDeclaration(scope1, modifier1, base.TokenString.ToLower(), Info, point2, point1, text1, Level);
                        }
                        return;
                    }
                }
                this.NextValidToken();
            }
        }

        private void ParseCaseStatement(IStatementInfo Info, ISyntaxInfos Infos, int Level)
        {
            int num1 = 1;
            string text1 = string.Empty;
            base.SkipToNextLine();
            while (!base.Eof && !this.IsBlockEnd(ref num1))
            {
                if ((base.Token == 2) && (base.TokenString.ToLower() == "case"))
                {
                    ((VbCaseStatementInfo) Info).CasePoints.Add(new Point(this.tokenPos, base.LineIndex));
                }
                this.ParseStatements(Info, Infos, Level + 1);
            }
            if (this.IsBlockEnd(base.TokenString))
            {
                this.NextValidToken();
            }
            this.NextValidToken();
            Info.HasBlock = true;
            Info.EndPoint = new Point(base.PrevPos, base.PrevLine);
        }

        protected void ParseDeclaration(VbScope Scope, VbModifier Modifiers, string InfoType, ISyntaxInfo Info, Point Position, Point AttrPt, string Attributes, int Level)
        {
			//string text1 = string.Empty;
			//string text2 = string.Empty;
			//if (base.GetReswordToken(InfoType) != 11)
			//{
			//    this.NextValidToken();
			//}
			//else
			//{
			//    object obj1;
			//    if (((obj1 = InfoType) != null) && ((obj1 = <PrivateImplementationDetails>.$$method0x60004eb-1[obj1]) != null))
			//    {
			//        char[] chArray1;
			//        switch (((int) obj1))
			//        {
			//            case 0:
			//            {
			//                this.ProcessImports(Info, Position, Level);
			//                return;
			//            }
			//            case 1:
			//            case 2:
			//            {
			//                if (this.GetName(ref text1, "."))
			//                {
			//                    base.ParseBlock((Info is IVbUnitInfo) ? ((IVbUnitInfo) Info).Namespaces : null, new VbNamespaceInfo(text1, Position, Level), Level, null, this.parseBlockProc);
			//                }
			//                return;
			//            }
			//            case 3:
			//            {
			//                if (this.GetClassNameAndType(out text1, out text2))
			//                {
			//                    chArray1 = new char[1] { ',' } ;
			//                    base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Interfaces : null, new VbInterfaceInfo(text1, text2.Split(chArray1), Position, Level, Scope, AttrPt, Attributes), Level, null, this.parseBlockProc);
			//                }
			//                return;
			//            }
			//            case 4:
			//            {
			//                if (this.GetClassNameAndType(out text1, out text2))
			//                {
			//                    chArray1 = new char[1] { ',' } ;
			//                    base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Classes : null, new VbClassInfo(text1, text2.Split(chArray1), Position, Level, Scope, Modifiers, AttrPt, Attributes), Level, null, this.parseBlockProc);
			//                }
			//                return;
			//            }
			//            case 5:
			//            {
			//                if (this.GetClassNameAndType(out text1, out text2))
			//                {
			//                    chArray1 = new char[1] { ',' } ;
			//                    base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Structures : null, new VbStructInfo(text1, text2.Split(chArray1), Position, Level, Scope, Modifiers, AttrPt, Attributes), Level, null, this.parseBlockProc);
			//                }
			//                return;
			//            }
			//            case 6:
			//            {
			//                string text3;
			//                if (base.NextValidToken(out text3) == 2)
			//                {
			//                    text3 = text3.ToLower();
			//                    if (((text3 != "sub") && (text3 != "function")) || !this.GetName(ref text1, string.Empty))
			//                    {
			//                        return;
			//                    }
			//                    IDelegateInfo info1 = new VbDelegateInfo(text1, text2, Position, Scope, AttrPt, Attributes);
			//                    base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Delegates : null, info1, Level, this.parseDelegateProc, null);
			//                }
			//                return;
			//            }
			//            case 7:
			//            case 8:
			//            {
			//                if (this.GetName(ref text1, string.Empty))
			//                {
			//                    IMethodInfo info2 = new VbMethodInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes);
			//                    if (!base.IsInterface(Info))
			//                    {
			//                        base.ParseBlock((Info is IInterfaceInfo) ? ((IInterfaceInfo) Info).Methods : null, info2, Level, this.parseDelegateProc, this.parseMethodProc);
			//                        return;
			//                    }
			//                    base.ParseBlock(((IInterfaceInfo) Info).Methods, info2, Level, this.parseDelegateProc, null);
			//                }
			//                return;
			//            }
			//            case 9:
			//            case 10:
			//            {
			//                if (this.GetNameAndType(out text1, out text2))
			//                {
			//                    IFieldInfo info3 = new VbFieldInfo(text1, text2, Position, Scope, AttrPt, Attributes);
			//                    info3.DataType = text2;
			//                    if (!(Info is IClassInfo))
			//                    {
			//                        if (Info is IHasLocalVars)
			//                        {
			//                            ((IHasLocalVars) Info).LocalVars.Add(info3);
			//                        }
			//                        return;
			//                    }
			//                    ((IClassInfo) Info).Fields.Add(info3);
			//                }
			//                return;
			//            }
			//            case 11:
			//            {
			//                if (this.GetName(ref text1, string.Empty))
			//                {
			//                    IPropInfo info4 = new VbPropInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes);
			//                    base.ParseBlock((Info is IInterfaceInfo) ? ((IInterfaceInfo) Info).Properties : null, info4, Level, this.parseDelegateProc, this.parsePropertyProc);
			//                    if (!base.IsInterface(Info))
			//                    {
			//                        return;
			//                    }
			//                    if (info4.PropertyGet != null)
			//                    {
			//                        info4.PropertyGet.Visible = false;
			//                    }
			//                    if (info4.PropertySet == null)
			//                    {
			//                        return;
			//                    }
			//                    info4.PropertySet.Visible = false;
			//                }
			//                return;
			//            }
			//            case 12:
			//            {
			//                if (this.GetClassNameAndType(out text1, out text2))
			//                {
			//                    base.ParseBlock((Info is IClassInfo) ? ((IClassInfo) Info).Enums : null, new VbEnumInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes), Level, null, this.parseEnumProc);
			//                }
			//                return;
			//            }
			//            case 13:
			//            {
			//                if (this.GetNameAndType(out text1, out text2) && (Info is IInterfaceInfo))
			//                {
			//                    IEventInfo info5 = new VbEventInfo(text1, text2, Position, Level, Scope, Modifiers, AttrPt, Attributes);
			//                    ((InterfaceInfo) Info).Events.Add(info5);
			//                }
			//                return;
			//            }
			//        }
			//    }
			//    this.NextValidToken();
			//}
        }

        protected void ParseDelegate(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            string text1 = string.Empty;
            string text2 = string.Empty;
            Point point1 = new Point(this.tokenPos, base.LineIndex);
            if (base.IsSymbolToken("("))
            {
                string text4;
                while (this.GetParamNameAndType(ref text1, ref text2, ref point1, out text4))
                {
                    ((IHasParams) Info).Params.Add(new ParamInfo(text1, text2, point1, text4));
                    if (base.TokenString == ")")
                    {
                        break;
                    }
                    text1 = string.Empty;
                    text2 = string.Empty;
                }
            }
            if (this.GetSimpleType(out text2))
            {
                ((ISyntaxTypeInfo) Info).DataType = text2;
            }
        }

        protected void ParseEnum(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            if (base.Token == 0)
            {
                IEnumInfo info1 = (IEnumInfo) Info;
                info1.Fields.Add(new VbFieldInfo(base.TokenString, info1.DataType, new Point(this.tokenPos, base.LineIndex), VbScope.Public));
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

        private void ParseIfElseStatement(IStatementInfo Info, ISyntaxInfos Infos, int Level)
        {
            int num1 = 1;
            string text1 = string.Empty;
            base.SkipToNextLine();
            while (!base.Eof && !this.IsBlockEnd(ref num1))
            {
                if (base.Token == 2)
                {
                    string text2;
                    text1 = base.TokenString.ToLower();
                    if ((text2 = text1) != null)
                    {
                        text2 = string.IsInterned(text2);
                        if (text2 != "else")
                        {
                            if (text2 == "elseif")
                            {
                                goto Label_0070;
                            }
                        }
                        else
                        {
                            ((VbIfStatementInfo) Info).ElsePt = new Point(this.tokenPos, base.LineIndex);
                        }
                    }
                }
                goto Label_0097;
            Label_0070:
                ((VbIfStatementInfo) Info).ElseIfPoints.Add(new Point(this.tokenPos, base.LineIndex));
            Label_0097:
                this.ParseStatements(Info, Infos, Level + 1);
            }
            if (this.IsBlockEnd(base.TokenString))
            {
                this.NextValidToken();
            }
            this.NextValidToken();
            Info.HasBlock = true;
            Info.EndPoint = new Point(base.PrevPos, base.PrevLine);
            Infos.Add(Info);
        }

        private void ParseLocalVars(string InfoType, ISyntaxInfo Info)
        {
            string text1 = InfoType;
            string text2 = InfoType;
            Point point1 = Point.Empty;
            int num1 = base.LineIndex;
            while (!base.Eof && (num1 == base.LineIndex))
            {
                if (text1 == "dim")
                {
                    base.NextValidToken(out text1);
                }
                bool flag1 = base.Token == 0;
                if (flag1)
                {
                    point1 = new Point(this.tokenPos, base.LineIndex);
                    flag1 = this.GetSimpleType(out text2);
                }
                if (!flag1)
                {
                    return;
                }
                if (Info is IHasLocalVars)
                {
                    LocalVarInfo info1 = new LocalVarInfo(text1, text2, point1);
                    ((IHasLocalVars) Info).LocalVars.Add(info1);
                }
                this.SkipToSymbol(",");
                text1 = base.TokenString.ToLower();
            }
        }

        protected void ParseMethod(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            this.ParseStatements(Info, ((IMethodInfo) Info).Statements, Level);
        }

        protected void ParseProperty(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            int num1 = 1;
            IPropInfo info1 = (IPropInfo) Info;
            string text1 = string.Empty;
            while (!base.Eof && !this.IsBlockEnd(ref num1))
            {
                string text2;
                text1 = base.TokenString.ToLower();
                if (base.Token != 2)
                {
                    goto Label_00DA;
                }
                if ((text2 = text1) == null)
                {
                    goto Label_00D1;
                }
                text2 = string.IsInterned(text2);
                if (text2 != "get")
                {
                    if (text2 == "set")
                    {
                        goto Label_0095;
                    }
                    goto Label_00D1;
                }
                info1.PropertyGet = new AccessorInfo(text1, new Point(this.tokenPos, base.LineIndex), Level);
                this.NextValidToken();
                base.ParseBlock(null, info1.PropertyGet, Level, null, this.parseAccessorProc);
                continue;
            Label_0095:
                info1.PropertySet = new AccessorInfo(text1, new Point(this.tokenPos, base.LineIndex), Level);
                this.NextValidToken();
                base.ParseBlock(null, info1.PropertySet, Level, null, this.parseAccessorProc);
                continue;
            Label_00D1:
                this.NextValidToken();
                continue;
            Label_00DA:
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
                    if (text1 == "select")
                    {
                        info1 = new VbCaseStatementInfo(Name, new Point(this.tokenPos, base.LineIndex), Level);
                        this.ParseCaseStatement(info1, Infos, Level);
                        Infos.Add(info1);
                        return info1;
                    }
                }
                else
                {
                    info1 = new VbIfStatementInfo(Name, new Point(this.tokenPos, base.LineIndex), Level);
                    this.ParseIfElseStatement(info1, Infos, Level);
                    Infos.Add(info1);
                    return info1;
                }
            }
            info1 = new VbStatementInfo(Name, new Point(this.tokenPos, base.LineIndex), Level);
            base.SkipToNextLine();
            base.ParseBlock(Infos, info1, Level, null, this.parseStatementsProc);
            info1.EndPoint = new Point(base.PrevPos, base.PrevLine);
            return info1;
        }

        protected void ParseStatements(ISyntaxInfo Info, ISyntaxInfos Infos, int Level)
        {
            string text1 = base.TokenString.ToLower();
            if (base.Token == 2)
            {
                int num1 = base.GetReswordToken(text1);
                if ((num1 == 2) && this.IsBlockStart(text1))
                {
                    this.ParseStatement(text1, Infos, Level);
                }
                else if ((num1 == 11) && (base.TokenString.ToLower() == "dim"))
                {
                    this.ParseLocalVars(text1, Info);
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
            else
            {
                this.NextValidToken();
            }
        }

        protected virtual void ProcessImports(ISyntaxInfo Info, Point Position, int Level)
        {
            Point point1 = new Point(this.source.Length, base.LineIndex);
            Point point2 = new Point(this.currentPos, base.LineIndex);
            string text1 = string.Empty;
            if (this.GetName(ref text1, "."))
            {
                IUsesInfo info1 = null;
                if (Info is ICsUnitInfo)
                {
                    info1 = ((IVbUnitInfo) Info).Uses;
                }
                else if (Info is IVbNamespaceInfo)
                {
                    info1 = ((IVbNamespaceInfo) Info).Uses;
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
            this.UpdateIndents();
            base.ReparseText();
        }

        private void SkipParams()
        {
            if (base.TokenString == "(")
            {
                this.SkipToSymbol(")");
            }
        }

        private bool SkipToSymbol(string symbol)
        {
            int num1 = base.LineIndex;
            while (!base.Eof)
            {
                if ((base.Token == 5) && (base.TokenString == symbol))
                {
                    this.NextValidToken();
                    return true;
                }
                if (num1 != base.LineIndex)
                {
                    return false;
                }
                this.NextValidToken();
            }
            return false;
        }


        // Fields
        private Hashtable blocks;
        protected LexerProc lexCommentProc;
        protected LexerProc lexDefineProc;
        protected LexerProc lexStringProc;
        protected SourceParser.ParseProc parseAccessorProc;
        protected SourceParser.ParseProc parseBlockProc;
        protected SourceParser.ParseProc parseDelegateProc;
        protected SourceParser.ParseProc parseEnumProc;
        protected SourceParser.ParseProc parseMethodProc;
        protected SourceParser.ParseProc parsePropertyProc;
        protected SourceParser.ParseProc parseStatementsProc;

        // Nested Types
        protected enum VbParserType
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

