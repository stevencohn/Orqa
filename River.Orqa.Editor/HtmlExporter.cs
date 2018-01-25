namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public class HtmlExporter : FmtExporter
    {
        // Methods
        public HtmlExporter()
        {
            this.colorChanged = false;
            this.rBuilder = new StringBuilder();
        }

        protected override void AddFontStyle(FontStyle Style)
        {
            switch (Style)
            {
                case FontStyle.Bold:
                {
                    this.WriteTag("b");
                    return;
                }
                case FontStyle.Italic:
                {
                    this.WriteTag("i");
                    return;
                }
                case (FontStyle.Italic | FontStyle.Bold):
                {
                    return;
                }
                case FontStyle.Underline:
                {
                    this.WriteTag("u");
                    return;
                }
                case FontStyle.Strikeout:
                {
                    this.WriteTag("s");
                    return;
                }
            }
        }

        private string ColorToHtml(Color Color)
        {
            return ("#" + Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2"));
        }

        protected override void EndDocument()
        {
            if (this.colorChanged)
            {
                this.WriteEndTag("font");
            }
            this.WriteFontStyle(FontStyle.Regular);
            this.WriteEndTag("font");
            this.writer.WriteLine();
            this.WriteEndTag("body");
            this.writer.WriteLine();
            this.WriteEndTag("html");
        }

        protected override void RemoveFontStyle(FontStyle Style)
        {
            switch (Style)
            {
                case FontStyle.Bold:
                {
                    this.WriteEndTag("b");
                    return;
                }
                case FontStyle.Italic:
                {
                    this.WriteEndTag("i");
                    return;
                }
                case (FontStyle.Italic | FontStyle.Bold):
                {
                    return;
                }
                case FontStyle.Underline:
                {
                    this.WriteEndTag("u");
                    return;
                }
                case FontStyle.Strikeout:
                {
                    this.WriteEndTag("s");
                    return;
                }
            }
        }

        protected override void StartDocument(TextWriter Writer)
        {
            base.StartDocument(Writer);
            this.WriteTag("html");
            this.writer.WriteLine();
            this.WriteTag("head");
            this.writer.Write(this.owner.Source.FileName);
            this.WriteEndTag("head");
            this.writer.WriteLine();
            this.WriteTag("body");
            this.writer.WriteLine();
            this.WriteTag("font", "name", this.owner.Font.Name);
            this.writer.WriteLine();
        }

        protected override void StartLine()
        {
            if (!this.firstLine)
            {
                this.writer.WriteLine();
                this.WriteTag("br");
            }
        }

        private string StringToHtml(int Pos, string String)
        {
            if ((String == string.Empty) || (String == null))
            {
                return String;
            }
            this.rBuilder.Length = 0;
            bool flag1 = true;
            string text1 = String;
            for (int num3 = 0; num3 < text1.Length; num3++)
            {
                int num1;
                char ch1 = text1[num3];
                char ch2 = ch1;
                if (ch2 <= '"')
                {
                    if (ch2 == '\t')
                    {
                        goto Label_00AE;
                    }
                    switch (ch2)
                    {
                        case ' ':
                        {
                            if (!flag1)
                            {
                                goto Label_009C;
                            }
                            this.rBuilder.Append("&nbsp;");
                            goto Label_0156;
                        }
                        case '!':
                        {
                            goto Label_0149;
                        }
                        case '"':
                        {
                            this.rBuilder.Append("&quot;");
                            goto Label_0156;
                        }
                    }
                    goto Label_0149;
                }
                if (ch2 == '&')
                {
                    goto Label_0123;
                }
                switch (ch2)
                {
                    case '<':
                    {
                        this.rBuilder.Append("&lt;");
                        goto Label_0156;
                    }
                    case '=':
                    {
                        goto Label_0149;
                    }
                    case '>':
                    {
                        this.rBuilder.Append("&gt;");
                        goto Label_0156;
                    }
                    default:
                    {
                        goto Label_0149;
                    }
                }
            Label_009C:
                this.rBuilder.Append(ch1);
                goto Label_0156;
            Label_00AE:
                num1 = this.owner.Lines.GetTabStop(Pos);
                if (flag1)
                {
                    for (int num2 = 0; num2 < num1; num2++)
                    {
                        this.rBuilder.Append("&nbsp;");
                    }
                }
                else
                {
                    this.rBuilder.Append(new string(' ', num1));
                }
                Pos += num1;
                goto Label_0156;
            Label_0123:
                this.rBuilder.Append("&amp;");
                goto Label_0156;
            Label_0149:
                this.rBuilder.Append(ch1);
            Label_0156:
                if ((ch1 != ' ') && (ch1 != '\t'))
                {
                    flag1 = false;
                }
                if (ch1 != '\t')
                {
                    Pos++;
                }
            }
            return this.rBuilder.ToString();
        }

        protected override void WriteBackColor(Color BackColor)
        {
        }

        private void WriteEndTag(string Tag)
        {
            this.writer.Write("</" + Tag + ">");
        }

        protected override void WriteForeColor(Color ForeColor)
        {
            if (this.colorChanged)
            {
                this.WriteEndTag("font");
            }
            this.WriteTag("font", "color", this.ColorToHtml(ForeColor));
            this.colorChanged = true;
        }

        private void WriteTag(string Tag)
        {
            this.writer.Write("<" + Tag + ">");
        }

        private void WriteTag(string Tag, string Name, string Value)
        {
            object[] objArray1 = new object[7] { "<", Tag, ' ', Name, "=", Value, ">" } ;
            this.writer.Write(string.Concat(objArray1));
        }

        protected override void WriteText(int Pos, string Text)
        {
            this.writer.Write(this.StringToHtml(Pos, Text));
        }


        // Fields
        private bool colorChanged;
        private const string htmlAmp = "&amp;";
        private const string htmlB = "b";
        private const string htmlBody = "body";
        private const string htmlBr = "br";
        private const string htmlColor = "color";
        private const string htmlFont = "font";
        private const string htmlHead = "head";
        private const string htmlHtml = "html";
        private const string htmlI = "i";
        private const string htmlLt = "&lt;";
        private const string htmlName = "name";
        private const string htmlNbsp = "&nbsp;";
        private const string htmlQt = "&gt;";
        private const string htmlQuot = "&quot;";
        private const string htmlS = "s";
        private const string htmlU = "u";
        private StringBuilder rBuilder;
    }
}

