namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxItem(false), DesignTimeVisible(false)]
    public class CompletionHint : Control
    {
        // Methods
        public CompletionHint()
        {
            this.sourceText = string.Empty;
            this.selectedIndex = 1;
            this.drawTop = 3;
            this.drawLeft = 2;
            this.arrowWidth = 9;
            this.arrowHeight = 11;
            this.arrowTop = 3;
            this.arrowBottom = 6;
            this.strings = new ArrayList();
            this.painter = new TextPainter(null);
            this.painter.Font = this.Font;
            this.hintSize = base.Size;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | (ControlStyles.StandardDoubleClick | (ControlStyles.StandardClick | (ControlStyles.Opaque | ControlStyles.UserPaint))), true);
        }

        private void AddLine(string s, bool FirstLine)
        {
            if (s == string.Empty)
            {
                this.strings.Add(new HintItem(s));
            }
            else
            {
                int num1 = 0;
                int num2 = s.Length;
                int num4 = EditConsts.MaxHintWindowWidth;
                if (FirstLine)
                {
                    num4 -= this.MeasureArrows();
                }
                while (num1 < num2)
                {
                    int num3;
                    int num5;
                    int num6;
                    int num7 = this.MeasureLine(FirstLine, s, num1, num4, out num3, out num5, out num6);
                    if ((num1 + num3) < num2)
                    {
                        for (int num8 = num3; num8 > 0; num8--)
                        {
                            if (this.IsDelimiter(s, (num1 + num8) - 1))
                            {
                                num3 = num8;
                                break;
                            }
                        }
                    }
                    if (num3 <= 0)
                    {
                        num3 = 1;
                    }
                    if ((num1 + num3) < num2)
                    {
                        this.strings.Add(new HintItem(s.Substring(num1, num3), num7, num5, num6));
                    }
                    else
                    {
                        this.strings.Add(new HintItem(s.Substring(num1), num7, num5, num6));
                    }
                    num1 += num3;
                    num4 = EditConsts.MaxHintWindowWidth;
                }
            }
        }

        private void AddMultiLine(string s, bool FirstLine)
        {
            s = s.Replace(Consts.TabStr, new string(' ', EditConsts.DefaultSpacesInTab));
            int num1 = s.IndexOf('\n');
            int num2 = 0;
            while (num1 >= 0)
            {
                this.AddLine(s.Substring(num2, num1 - num2).Trim(Consts.crlfArray), FirstLine);
                num2 = num1;
                num1 = s.IndexOf('\n', (int) (num1 + 1));
                FirstLine = false;
            }
            this.AddLine(s.Substring(num2).Trim(Consts.crlfArray), FirstLine);
        }

        public void ChangeSelection(bool Inc)
        {
            if (this.provider != null)
            {
                int num1 = this.selectedIndex;
                if (Inc)
                {
                    num1++;
                    if (num1 >= this.provider.Count)
                    {
                        num1 = 0;
                    }
                }
                else
                {
                    num1--;
                    if (num1 < 0)
                    {
                        num1 = Math.Max((int) (this.provider.Count - 1), 0);
                    }
                }
                this.SelectedIndex = num1;
            }
        }

        private void DivideBold(string s, out string s1, out string s2, out string s3, int bStart, int bEnd)
        {
            if (bEnd >= bStart)
            {
                s1 = s.Substring(0, bStart);
                s2 = s.Substring(bStart, Math.Min(bEnd, s.Length) - bStart);
                s3 = (bEnd < s.Length) ? s.Substring(bEnd) : string.Empty;
            }
            else
            {
                s1 = s;
                s2 = string.Empty;
                s3 = string.Empty;
            }
        }

        private void DrawArrows()
        {
            string text1 = string.Format(EditConsts.NofMstr, this.selectedIndex + 1, this.provider.Count);
            int num1 = this.painter.StringWidth(text1);
            this.painter.BkColor = EditConsts.DefaultArrowBackColor;
            this.painter.FillRectangle(this.drawLeft, this.drawTop, this.arrowWidth, this.arrowHeight);
            int num2 = ((this.drawLeft * 3) + num1) + this.arrowWidth;
            this.painter.FillRectangle(num2, this.drawTop, this.arrowWidth, this.arrowHeight);
            Point point1 = new Point(this.drawLeft + 1, this.drawTop + this.arrowBottom);
            Point point2 = new Point(this.drawLeft + (this.arrowWidth / 2), this.drawTop + this.arrowTop);
            Point point3 = new Point((this.drawLeft + this.arrowWidth) - 2, this.drawTop + this.arrowBottom);
            Point[] pointArray3 = new Point[3] { point1, point2, point3 } ;
            Point[] pointArray1 = pointArray3;
            this.painter.Polygon(pointArray1, EditConsts.DefaultArrowForeColor);
            point1 = new Point(num2 + 1, (this.drawTop + this.arrowTop) + 1);
            point2 = new Point(num2 + (this.arrowWidth / 2), (this.drawTop + this.arrowBottom) + 1);
            point3 = new Point((num2 + this.arrowWidth) - 2, (this.drawTop + this.arrowTop) + 1);
            pointArray3 = new Point[3] { point1, point2, point3 } ;
            Point[] pointArray2 = pointArray3;
            this.painter.Polygon(pointArray2, EditConsts.DefaultArrowForeColor);
            this.painter.TextOut(text1, text1.Length, (int) ((this.drawLeft + this.arrowWidth) + 2), (int) (this.drawTop - 1));
        }

        private void DrawItem(HintItem Item, int Left, int Top)
        {
            string text1;
            string text2;
            string text3;
            this.DivideBold(Item.Text, out text1, out text2, out text3, Item.BoldStart, Item.BoldEnd);
            if (text1 != string.Empty)
            {
                this.painter.TextOut(text1, -1, Left, Top);
                Left += this.painter.StringWidth(text1);
            }
            if (text2 != string.Empty)
            {
                this.painter.FontStyle = FontStyle.Bold;
                this.painter.TextOut(text2, -1, Left, Top);
                Left += this.painter.StringWidth(text2);
            }
            this.painter.FontStyle = FontStyle.Regular;
            if (text3 != string.Empty)
            {
                this.painter.TextOut(text3, -1, Left, Top);
            }
        }

        private string GetTextStr()
        {
            string text1 = string.Empty;
            if (((this.provider != null) && (this.selectedIndex >= 0)) && (this.selectedIndex < this.provider.Count))
            {
                if (this.provider.ColumnCount == 0)
                {
                    return ((CodeCompletionProvider) this.provider).GetText(this.selectedIndex);
                }
                text1 = string.Empty;
                for (int num1 = 0; num1 < this.provider.ColumnCount; num1++)
                {
                    if (this.provider.ColumnVisible(num1))
                    {
                        string text2 = this.provider.GetColumnText(this.selectedIndex, num1);
                        if ((text2 != null) && (text2 != string.Empty))
                        {
                            if (text1 == string.Empty)
                            {
                                text1 = text2;
                            }
                            else
                            {
                                text1 = text1 + ' ' + text2;
                            }
                        }
                    }
                }
            }
            return text1;
        }

        private bool IsDelimiter(string s, int Pos)
        {
            char ch1 = s[Pos];
            if (ch1 > ' ')
            {
                return (EditConsts.DefaultDelimiters.IndexOf(ch1) >= 0);
            }
            return true;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys keys1 = keyData & Keys.KeyCode;
            if (Array.IndexOf(EditConsts.NavKeys, keys1) >= 0)
            {
                return true;
            }
            return base.IsInputKey(keyData);
        }

        protected internal bool MakeBold(string Source, int Start, int End)
        {
            if (this.UpdateBold(Source, Start, End))
            {
                this.UpdateControlSize();
                return true;
            }
            return false;
        }

        private int MeasureArrows()
        {
            if (this.NeedArrows())
            {
                string text1 = string.Format(EditConsts.NofMstr, this.selectedIndex + 1, this.provider.Count);
                return ((this.painter.StringWidth(text1) + (this.arrowWidth * 2)) + (this.drawLeft * 4));
            }
            return 0;
        }

        private int MeasureLine(bool FirstLine, string s, int P, int Margin, out int chars, out int bStart, out int bEnd)
        {
            string text1;
            string text2;
            string text3;
            int num1 = 0;
            bStart = 0;
            bEnd = 0;
            if (!FirstLine)
            {
                return this.painter.StringWidth(s, P, -1, Margin, out chars);
            }
            chars = 0;
            this.DivideBold(s, out text1, out text2, out text3, this.boldStart, this.boldEnd);
            if ((text1 != string.Empty) && (P < text1.Length))
            {
                num1 = this.painter.StringWidth(text1, P, -1, Margin, out chars);
                P += chars;
                bStart = chars;
                if (P < text1.Length)
                {
                    return num1;
                }
            }
            if (((Margin > num1) && (text2 != string.Empty)) && (P < (text1.Length + text2.Length)))
            {
                int num2;
                this.painter.FontStyle = FontStyle.Bold;
                num1 += this.painter.StringWidth(text2, P - text1.Length, -1, Margin - num1, out num2);
                chars += num2;
                bEnd = chars;
                P += num2;
                if (P < (text1.Length + text2.Length))
                {
                    return num1;
                }
            }
            this.painter.FontStyle = FontStyle.Regular;
            if ((Margin > num1) && (text3 != string.Empty))
            {
                int num3;
                num1 += this.painter.StringWidth(text3, (P - text1.Length) - text2.Length, -1, Margin - num1, out num3);
                chars += num3;
            }
            return num1;
        }

        private void MeasureText()
        {
            this.strings.Clear();
            if (((this.provider != null) && (this.selectedIndex >= 0)) && (this.selectedIndex < this.provider.Count))
            {
                this.AddMultiLine(this.GetTextStr(), true);
                string text1 = ((CodeCompletionProvider) this.provider).GetDescription(this.selectedIndex);
                if ((text1 != null) && (text1 != string.Empty))
                {
                    this.AddMultiLine(text1, false);
                }
            }
        }

        protected internal bool NeedArrows()
        {
            if (this.provider != null)
            {
                return (this.provider.Count > 1);
            }
            return false;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.painter.Clear();
            this.painter.Font = this.Font;
            this.UpdateControlSize();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            this.painter.BeginPaint(pe.Graphics);
            try
            {
                this.painter.BkColor = EditConsts.DefaultInfoBackColor;
                this.painter.FillRectangle(0, 0, base.Width, base.Height);
                if (this.provider == null)
                {
                    return;
                }
                this.painter.Color = this.ForeColor;
                this.painter.BkMode = 1;
                int num1 = 0;
                int num2 = this.drawTop;
                for (int num3 = 0; num3 < this.strings.Count; num3++)
                {
                    if ((num3 == 0) && (this.provider.Count > 1))
                    {
                        this.DrawArrows();
                        num1 = this.MeasureArrows() + this.drawLeft;
                    }
                    else
                    {
                        num1 = this.drawLeft;
                    }
                    this.DrawItem((HintItem) this.strings[num3], num1, num2);
                    num2 += (this.painter.FontHeight + this.drawTop);
                }
            }
            finally
            {
                this.painter.EndPaint(pe.Graphics);
            }
        }

        protected virtual void ProviderChanged()
        {
            this.selectedIndex = 0;
            this.StringChanged();
        }

        public void ResetContent()
        {
            this.ProviderChanged();
        }

        protected void StringChanged()
        {
            this.UpdateBold(this.sourceText, this.sourceStart, this.sourceEnd);
            this.UpdateControlSize();
        }

        private bool UpdateBold(string Source, int Start, int End)
        {
            this.sourceText = Source;
            this.sourceStart = Start;
            this.sourceEnd = End;
            int num1 = this.boldStart;
            int num2 = this.boldEnd;
            this.boldStart = Start;
            this.boldEnd = End;
            if (((this.provider != null) && ((CodeCompletionProvider) this.provider).IsBold(Source, this.GetTextStr(), ref this.boldStart, ref this.boldEnd)) && (num1 != this.boldStart))
            {
                return true;
            }
            return (num2 != this.boldEnd);
        }

        protected internal void UpdateControlSize()
        {
            this.MeasureText();
            int num1 = 0;
            for (int num2 = 0; num2 < this.strings.Count; num2++)
            {
                int num3 = ((HintItem) this.strings[num2]).Width;
                if (num2 == 0)
                {
                    num3 += this.MeasureArrows();
                }
                num1 = Math.Max(num1, num3);
            }
            this.hintSize = new Size(num1 + (this.drawLeft * 3), ((this.painter.FontHeight + this.drawTop) * Math.Max(this.strings.Count, 1)) + this.drawTop);
            if (this.UpdateSize != null)
            {
                this.UpdateSize(this, EventArgs.Empty);
            }
            base.Invalidate();
        }


        // Properties
        public Size HintSize
        {
            get
            {
                return this.hintSize;
            }
        }

        public ICodeCompletionProvider Provider
        {
            get
            {
                return this.provider;
            }
            set
            {
                if (this.provider != value)
                {
                    this.provider = value;
                    this.ProviderChanged();
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }
            set
            {
                if (this.selectedIndex != value)
                {
                    this.selectedIndex = value;
                    this.StringChanged();
                }
            }
        }

        public EventHandler UpdateSize
        {
            get
            {
                return this.updateSize;
            }
            set
            {
                this.updateSize = value;
            }
        }


        // Fields
        private int arrowBottom;
        private int arrowHeight;
        private int arrowTop;
        private int arrowWidth;
        private int boldEnd;
        private int boldStart;
        private int drawLeft;
        private int drawTop;
        private Size hintSize;
        private ITextPainter painter;
        private ICodeCompletionProvider provider;
        private int selectedIndex;
        private int sourceEnd;
        private int sourceStart;
        private string sourceText;
        private ArrayList strings;
        private EventHandler updateSize;

        // Nested Types
        private class HintItem
        {
            // Methods
            public HintItem(string s)
            {
                this.Text = s;
            }

            public HintItem(string s, int width, int bStart, int bEnd)
            {
                this.Text = s;
                this.Width = width;
                this.BoldStart = bStart;
                this.BoldEnd = bEnd;
            }


            // Fields
            public int BoldEnd;
            public int BoldStart;
            public string Text;
            public int Width;
        }
    }
}

