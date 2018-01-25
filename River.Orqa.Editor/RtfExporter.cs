namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public class RtfExporter : FmtExporter
    {
        // Methods
        public RtfExporter()
        {
            this.colorTable = new Hashtable();
            this.sBuilder = new StringBuilder();
            this.rBuilder = new StringBuilder();
        }

        private int AddColor(Color Color)
        {
            object obj1 = this.colorTable[Color];
            if (obj1 != null)
            {
                return (int) obj1;
            }
            int num1 = this.colorTable.Count;
            this.colorTable.Add(Color, num1);
            return num1;
        }

        protected override void AddFontStyle(FontStyle Style)
        {
            switch (Style)
            {
                case FontStyle.Bold:
                {
                    this.WriteTag(@"\b");
                    return;
                }
                case FontStyle.Italic:
                {
                    this.WriteTag(@"\i");
                    return;
                }
                case (FontStyle.Italic | FontStyle.Bold):
                {
                    return;
                }
                case FontStyle.Underline:
                {
                    this.WriteTag(@"\ul");
                    return;
                }
                case FontStyle.Strikeout:
                {
                    this.WriteTag(@"\strike");
                    return;
                }
            }
        }

        protected string ColorToRtf(Color Color)
        {
            string[] textArray1 = new string[6] { @"\red", Color.R.ToString(), @"\green", Color.G.ToString(), @"\blue", Color.B.ToString() } ;
            return string.Concat(textArray1);
        }

        protected override void EndDocument()
        {
            this.WriteHeader();
            this.WriteFontTable();
            this.WriteColorTable();
            this.writer.Write(@"\f1 ");
            int num1 = ((int) this.owner.Font.Size) * 2;
            this.writer.Write(@"\fs" + num1.ToString(), ' ');
            this.writer.Write(this.sBuilder.ToString());
            this.writer.Write("}");
        }

        protected override void RemoveFontStyle(FontStyle Style)
        {
            switch (Style)
            {
                case FontStyle.Bold:
                {
                    this.WriteTag(@"\b0");
                    return;
                }
                case FontStyle.Italic:
                {
                    this.WriteTag(@"\i0");
                    return;
                }
                case (FontStyle.Italic | FontStyle.Bold):
                {
                    return;
                }
                case FontStyle.Underline:
                {
                    this.WriteTag(@"\ulnone");
                    return;
                }
                case FontStyle.Strikeout:
                {
                    this.WriteTag(@"\strike0");
                    return;
                }
            }
        }

        protected override void StartDocument(TextWriter Writer)
        {
            base.StartDocument(Writer);
            this.AddColor(this.foreColor);
        }

        protected override void StartLine()
        {
            if (!this.firstLine)
            {
                this.WriteString(string.Empty, true);
                this.WriteTag(@"\line");
            }
        }

        private string StringToRtf(string String, ref bool FirstTab, ref bool LastTab)
        {
            if ((String == string.Empty) || (String == null))
            {
                return String;
            }
            this.rBuilder.Length = 0;
            FirstTab = String[0] == '\t';
            LastTab = false;
            string text1 = String;
            for (int num1 = 0; num1 < text1.Length; num1++)
            {
                char ch1 = text1[num1];
                char ch2 = ch1;
                if (ch2 != '\t')
                {
                    switch (ch2)
                    {
                        case '{':
                        case '}':
                        case '\\':
                        {
                            LastTab = false;
                            this.rBuilder.Append(@"\" + ch1);
                            goto Label_00BB;
                        }
                        case '|':
                        {
                            goto Label_0099;
                        }
                    }
                }
                else
                {
                    LastTab = true;
                    this.rBuilder.Append(@"\tab");
                    goto Label_00BB;
                }
            Label_0099:
                if (LastTab)
                {
                    this.rBuilder.Append(' ');
                }
                LastTab = false;
                this.rBuilder.Append(ch1);
            Label_00BB:;
            }
            return this.rBuilder.ToString();
        }

        protected override void WriteBackColor(Color BackColor)
        {
        }

        private void WriteColorTable()
        {
            if (this.colorTable.Count > 0)
            {
                ArrayList list1 = new ArrayList();
                this.writer.Write(@"{\colortbl ");
                IDictionaryEnumerator enumerator1 = this.colorTable.GetEnumerator();
                enumerator1.Reset();
                while (enumerator1.MoveNext())
                {
                    list1.Add(enumerator1.Entry);
                }
                list1.Sort(new River.Orqa.Editor.RtfExporter.ColorComparer());
                foreach (DictionaryEntry entry1 in list1)
                {
                    this.writer.Write(this.ColorToRtf((Color) entry1.Key) + ";");
                }
                this.writer.WriteLine("}");
            }
        }

        private void WriteFontTable()
        {
            this.writer.Write(@"{\fonttbl ");
            this.writer.Write(@"{\f1 " + this.owner.Font.Name + ";}");
            this.writer.WriteLine("}");
        }

        protected override void WriteForeColor(Color ForeColor)
        {
            this.WriteTag(@"\cf" + this.AddColor(ForeColor));
        }

        private void WriteHeader()
        {
            this.writer.WriteLine(@"{\rtf1\ansi ");
        }

        private void WriteString(string String)
        {
            if (String != string.Empty)
            {
                if (this.needSpace)
                {
                    this.sBuilder.Append(' ');
                }
                this.needSpace = false;
                this.sBuilder.Append(String);
            }
        }

        private void WriteString(string String, bool NewLine)
        {
            this.WriteString(String);
            if (NewLine)
            {
                this.sBuilder.Append(Consts.CRLF);
                this.needSpace = false;
            }
        }

        private void WriteTag(string Tag)
        {
            this.sBuilder.Append(Tag);
            this.needSpace = true;
        }

        protected override void WriteText(int Pos, string Text)
        {
            bool flag1 = false;
            bool flag2 = false;
            string text1 = this.StringToRtf(Text, ref flag1, ref flag2);
            if (flag1)
            {
                this.needSpace = false;
            }
            this.WriteString(text1);
            if (flag2)
            {
                this.needSpace = true;
            }
        }


        // Fields
        private Hashtable colorTable;
        private bool needSpace;
        private StringBuilder rBuilder;
        private const string rtfB = @"\b";
        private const string rtfB0 = @"\b0";
        private const string rtfBlue = @"\blue";
        private const string rtfCf = @"\cf";
        private const string rtfColorTbl = @"\colortbl ";
        private const string rtfF1 = @"\f1 ";
        private const string rtfFontTbl = @"\fonttbl ";
        private const string rtfGreen = @"\green";
        private const string rtfGroupBegin = "{";
        private const string rtfGroupEnd = "}";
        private const string rtfHeader = @"{\rtf1\ansi ";
        private const string rtfI = @"\i";
        private const string rtfI0 = @"\i0";
        private const string rtfLine = @"\line";
        private const string rtfRed = @"\red";
        private const string rtfSize = @"\fs";
        private const string rtfStrike = @"\strike";
        private const string rtfStrike0 = @"\strike0";
        private const string rtfTab = @"\tab";
        private const string rtfUl = @"\ul";
        private const string rtfUlnone = @"\ulnone";
        private StringBuilder sBuilder;

        // Nested Types
        private class ColorComparer : IComparer
        {
            // Methods
            public ColorComparer()
            {
            }

            public int Compare(object x, object y)
            {
                DictionaryEntry entry1 = (DictionaryEntry) x;
                entry1 = (DictionaryEntry) y;
                return (((int) entry1.Value) - ((int) entry1.Value));
            }

        }
    }
}

