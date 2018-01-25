namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxItem(true), ToolboxBitmap(typeof(JsParser), "Images.JsParser.bmp")]
    public class JsParser : CsParser
    {
        // Methods
        public JsParser()
        {
        }

        protected override CsParser.CsParserType GetTypeAndName(ref string AName, ref string AType)
        {
            CsParser.CsParserType type1 = base.GetTypeAndName(ref AName, ref AType);
            if ((type1 != CsParser.CsParserType.Property) && (AName != "this"))
            {
                return type1;
            }
            return CsParser.CsParserType.None;
        }

        protected override void InitReswords()
        {
            this.CommonInitReswords();
            Hashtable hashtable1 = base.Reswords;
            hashtable1.Add("instanceof", SyntaxToken.Operator);
            hashtable1.Add("if", SyntaxToken.Statement);
            hashtable1.Add("else", SyntaxToken.Statement);
            hashtable1.Add("switch", SyntaxToken.Statement);
            hashtable1.Add("case", SyntaxToken.Statement);
            hashtable1.Add("do", SyntaxToken.Statement);
            hashtable1.Add("for", SyntaxToken.Statement);
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
            hashtable1.Add("boolean", SyntaxToken.DataType);
            hashtable1.Add("byte", SyntaxToken.DataType);
            hashtable1.Add("char", SyntaxToken.DataType);
            hashtable1.Add("double", SyntaxToken.DataType);
            hashtable1.Add("float", SyntaxToken.DataType);
            hashtable1.Add("int", SyntaxToken.DataType);
            hashtable1.Add("long", SyntaxToken.DataType);
            hashtable1.Add("short", SyntaxToken.DataType);
            hashtable1.Add("void", SyntaxToken.DataType);
            hashtable1.Add("class", SyntaxToken.Declaration);
            hashtable1.Add("interface", SyntaxToken.Declaration);
            hashtable1.Add("package", SyntaxToken.Declaration);
            hashtable1.Add("import", SyntaxToken.Declaration);
            hashtable1.Add("super", SyntaxToken.Resword);
            hashtable1.Add("extends", SyntaxToken.Resword);
            hashtable1.Add("implements", SyntaxToken.Resword);
            hashtable1.Add("native", SyntaxToken.Resword);
            hashtable1.Add("transient", SyntaxToken.Resword);
            hashtable1.Add("throws", SyntaxToken.Resword);
            hashtable1.Add("new", SyntaxToken.Resword);
            hashtable1.Add("this", SyntaxToken.Resword);
            hashtable1.Add("null", SyntaxToken.Value);
            hashtable1.Add("false", SyntaxToken.Value);
            hashtable1.Add("true", SyntaxToken.Value);
            hashtable1.Add("private", SyntaxToken.Scope);
            hashtable1.Add("protected", SyntaxToken.Scope);
            hashtable1.Add("public", SyntaxToken.Scope);
            hashtable1.Add("abstract", SyntaxToken.Modifier);
            hashtable1.Add("const", SyntaxToken.Modifier);
            hashtable1.Add("final", SyntaxToken.Modifier);
            hashtable1.Add("static", SyntaxToken.Modifier);
            hashtable1.Add("volatile", SyntaxToken.Modifier);
            hashtable1.Add("synchronized", SyntaxToken.Modifier);
        }

        protected override void InitRules()
        {
            this.CommonInitRules();
            char[] chArray1 = new char[2] { '"', '\'' } ;
            base.RegisterLexerProc(0, chArray1, this.lexStringProc);
            base.RegisterLexerProc(0, '/', this.lexCommentProc);
            base.RegisterLexerProc(0, '#', this.lexDefineProc);
            chArray1 = new char[2] { '+', '-' } ;
            base.RegisterLexerProc(0, chArray1, this.lexNumberProc);
            base.RegisterLexerProc(1, this.lexCommentEndProc);
        }

        protected override void InitStyles()
        {
            base.InitStyles();
            base.Scheme.Name = "Java #";
            base.Scheme.Desc = "Syntax Scheme for J# Language";
        }

        protected override bool IsTypeSeparator()
        {
            if (base.Token != 2)
            {
                return false;
            }
            string text1 = base.TokenString;
            if (text1 != "extends")
            {
                return (text1 == "implements");
            }
            return true;
        }

        protected override int LexComment()
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
                        goto Label_0061;
                    }
                    this.currentPos = num1 + 1;
                    return 3;
                }
                this.currentPos++;
                return this.LexCommentEnd();
            }
        Label_0061:
            return 5;
        }

        protected override int LexString()
        {
            char ch1 = this.source[this.currentPos];
            this.currentPos++;
            return this.LexString(true, ch1);
        }

        protected override void ProcessDeclaration(CsScope Scope, CsModifier Modifiers, string InfoType, ISyntaxInfo Info, Point Position, Point AttrPt, string Attributes, int Level)
        {
            string text2;
            string text1 = string.Empty;
            if ((text2 = InfoType) != null)
            {
                text2 = string.IsInterned(text2);
                if (text2 != "import")
                {
                    if (text2 == "package")
                    {
                        if (base.GetName(ref text1, "."))
                        {
                            base.ParseBlock((Info is ICsUnitInfo) ? ((ICsUnitInfo) Info).Namespaces : null, new CsNamespaceInfo(text1, Position, Level), Level, null, this.parseBlockProc);
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
            base.ProcessDeclaration(Scope, Modifiers, InfoType, Info, Position, AttrPt, Attributes, Level);
        }

    }
}

