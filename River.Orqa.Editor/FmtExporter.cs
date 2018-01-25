namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.IO;

    public class FmtExporter
    {
        // Methods
        public FmtExporter()
        {
            this.foreColor = Consts.DefaultControlForeColor;
            this.backColor = Consts.DefaultControlBackColor;
            this.fontStyle = FontStyle.Regular;
            this.firstLine = true;
        }

        protected virtual void AddFontStyle(FontStyle Style)
        {
        }

        protected virtual void ApplyStyle(FontStyle Style, Color ForeColor, Color BackColor)
        {
            if ((ForeColor != this.foreColor) && (ForeColor != Color.Empty))
            {
                this.WriteForeColor(ForeColor);
                this.foreColor = ForeColor;
            }
            if (Style != this.fontStyle)
            {
                this.WriteFontStyle(Style);
                this.fontStyle = Style;
            }
        }

        protected virtual void EndDocument()
        {
        }

        protected virtual void EndLine()
        {
        }

        protected virtual void RemoveFontStyle(FontStyle Style)
        {
        }

        protected virtual void StartDocument(TextWriter Writer)
        {
            this.writer = Writer;
        }

        protected virtual void StartLine()
        {
        }

        public void Write(ISyntaxEdit Owner, TextWriter Writer)
        {
            this.owner = Owner;
            this.StartDocument(Writer);
            this.WriteContent();
            this.EndDocument();
        }

        protected virtual void WriteBackColor(Color BackColor)
        {
        }

        protected virtual void WriteContent()
        {
            bool flag1 = this.owner.Source.Lexer != null;
            if (flag1)
            {
                this.owner.Source.ParseToString(this.owner.Lines.Count - 1);
            }
            string text1 = string.Empty;
            short[] numArray1 = null;
            DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
            this.firstLine = true;
            for (int num5 = 0; num5 < strings1.Lines.Count; num5++)
            {
                strings1.GetString(num5, ref text1, ref numArray1, flag1, false);
                int num4 = text1.Length;
                this.StartLine();
                this.firstLine = false;
                if (flag1 && (text1 != string.Empty))
                {
                    int num1 = numArray1[0];
                    int num2 = num1;
                    int num3 = 0;
                    for (int num6 = 1; num6 < num4; num6++)
                    {
                        num1 = numArray1[num6];
                        if ((num1 != num2) && !((SyntaxEdit) this.owner).EqualStyles(num2, num1, true))
                        {
                            this.WriteLine(text1, num2, num3, num6 - 1);
                            num2 = num1;
                            num3 = num6;
                        }
                    }
                    if (num3 < num4)
                    {
                        this.WriteLine(text1, num2, num3, num4 - 1);
                    }
                }
                else
                {
                    this.WriteLine(text1, -1, 0, num4 - 1);
                }
                this.EndLine();
            }
        }

        protected virtual void WriteFontStyle(FontStyle Style)
        {
            FontStyle style1 = this.fontStyle & ~Style;
            FontStyle style2 = Style & ~this.fontStyle;
            if ((style1 & FontStyle.Strikeout) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Strikeout);
            }
            if ((style1 & FontStyle.Underline) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Underline);
            }
            if ((style1 & FontStyle.Italic) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Italic);
            }
            if ((style1 & FontStyle.Bold) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Bold);
            }
            if ((style2 & FontStyle.Bold) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Bold);
            }
            if ((style2 & FontStyle.Italic) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Italic);
            }
            if ((style2 & FontStyle.Underline) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Underline);
            }
            if ((style2 & FontStyle.Strikeout) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Strikeout);
            }
        }

        protected virtual void WriteForeColor(Color ForeColor)
        {
        }

        protected virtual void WriteLine(string Line, int Style, int Start, int End)
        {
            FontStyle style2;
            Color color1;
            Color color2;
            ColorFlags flags1 = ColorFlags.None;
            SyntaxEdit edit1 = this.owner as SyntaxEdit;
            ILexStyle style1 = edit1.GetLexStyle(Style, ref flags1);
            if (style1 != null)
            {
                style2 = edit1.GetFontStyle(style1.FontStyle, flags1);
                color1 = edit1.GetFontColor(style1.ForeColor, flags1);
                if (style1.BackColor != Color.Empty)
                {
                    color2 = style1.BackColor;
                }
                else
                {
                    color2 = this.owner.BackColor;
                }
            }
            else
            {
                style2 = edit1.GetFontStyle(this.owner.Font.Style, flags1);
                color1 = edit1.GetFontColor(this.owner.ForeColor, flags1);
                color2 = this.owner.BackColor;
            }
            this.ApplyStyle(style2, color1, color2);
            this.WriteText(Start, Line.Substring(Start, (End - Start) + 1));
        }

        protected virtual void WriteText(int Pos, string Text)
        {
        }


        // Fields
        protected Color backColor;
        protected bool firstLine;
        protected FontStyle fontStyle;
        protected Color foreColor;
        protected ISyntaxEdit owner;
        protected TextWriter writer;
    }
}

