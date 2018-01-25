namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public class PageHeader : IPageHeader
    {
        // Methods
        public PageHeader()
        {
            this.leftText = string.Empty;
            this.rightText = string.Empty;
            this.centerText = string.Empty;
            this.visible = true;
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
            this.fontColor = EditConsts.DefaultHeaderFontColor;
            this.regex = new Regex(@"\\\[[a-zA-Z_0-9]+\]", RegexOptions.Singleline);
            this.offset = new Point(0x18, 8);
        }

        public PageHeader(IEditPage Page) : this()
        {
            this.page = Page;
        }

        public void Assign(IPageHeader Source)
        {
            this.BeginUpdate();
            try
            {
                this.LeftText = Source.LeftText;
                this.RightText = Source.RightText;
                this.CenterText = Source.CenterText;
                this.Offset = Source.Offset;
                this.ReverseOnEvenPages = Source.ReverseOnEvenPages;
                this.Font = new System.Drawing.Font(Source.Font.FontFamily, Source.Font.Size, Source.Font.Style);
                this.FontColor = Source.FontColor;
                this.Visible = Source.Visible;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        ~PageHeader()
        {
            this.font.Dispose();
        }

        private EditPages GetPages()
        {
            if (this.page == null)
            {
                return null;
            }
            return ((EditPage) this.page).Pages;
        }

        public string GetTextToPaint(string Text, int PageIndex, int PageCount)
        {
            MatchCollection collection1 = this.regex.Matches(Text);
            for (int num1 = collection1.Count - 1; num1 >= 0; num1--)
            {
                Match match1 = collection1[num1];
                if (match1.Success)
                {
                    string text1 = Text.Substring(match1.Index, match1.Length);
                    if (string.Compare(text1, EditConsts.PageTag) == 0)
                    {
                        int num2 = PageIndex + 1;
                        text1 = num2.ToString();
                    }
                    if (string.Compare(text1, EditConsts.PagesTag) == 0)
                    {
                        text1 = PageCount.ToString();
                    }
                    if (string.Compare(text1, EditConsts.DateTag) == 0)
                    {
                        text1 = DateTime.Now.ToLongDateString();
                    }
                    if (string.Compare(text1, EditConsts.TimeTag) == 0)
                    {
                        text1 = DateTime.Now.ToLongTimeString();
                    }
                    if (string.Compare(text1, EditConsts.UserTag) == 0)
                    {
                        text1 = Environment.UserName;
                    }
                    else
                    {
                        EditPages pages1 = this.GetPages();
                        if (pages1 != null)
                        {
                            pages1.OnDrawHeader(ref text1);
                        }
                    }
                    Text = Text.Remove(match1.Index, match1.Length);
                    Text = Text.Insert(match1.Index, text1);
                }
            }
            return Text;
        }

        public void Paint(ITextPainter Painter, Rectangle Rect, int PageIndex, int PageCount, bool PageNumbers)
        {
            System.Drawing.Font font1 = Painter.Font;
            Color color1 = Painter.Color;
            Painter.BkMode = 1;
            try
            {
                DrawInfo info1;
                Painter.Font = this.Font;
                Painter.Color = this.fontColor;
                EditPages pages1 = this.GetPages();
                info1 = new DrawInfo();
                info1.Init();
                info1.Page = (this.page != null) ? this.page.Index : -1;
                SyntaxEdit edit1 = ((pages1 != null) && (pages1.Owner != null)) ? ((SyntaxEdit) pages1.Owner) : null;
                if ((edit1 == null) || !edit1.OnCustomDraw(Painter, Rect, DrawStage.Before, DrawState.PageHeader, info1))
                {
                    bool flag1 = ((pages1 != null) && this.reverseOnEvenPages) && (((PageIndex + 1) % 2) == 0);
                    string text2 = PageNumbers ? ((PageIndex + 1)).ToString() : this.RightText;
                    string text1 = flag1 ? text2 : this.LeftText;
                    if ((text1 != null) && (text1 != string.Empty))
                    {
                        Painter.TextOut(this.GetTextToPaint(text1, PageIndex, PageCount), -1, Rect);
                    }
                    text1 = this.CenterText;
                    if ((text1 != null) && (text1 != string.Empty))
                    {
                        text1 = this.GetTextToPaint(text1, PageIndex, PageCount);
                        int num1 = Painter.StringWidth(text1);
                        Painter.TextOut(text1, -1, (int) (((Rect.Left + Rect.Right) - num1) >> 1), Rect.Top);
                    }
                    text1 = flag1 ? this.LeftText : text2;
                    if ((text1 != null) && (text1 != string.Empty))
                    {
                        text1 = this.GetTextToPaint(text1, PageIndex, PageCount);
                        int num2 = Painter.StringWidth(text1);
                        Painter.TextOut(text1, -1, (int) (Rect.Right - num2), Rect.Top);
                    }
                }
                if (edit1 != null)
                {
                    edit1.OnCustomDraw(Painter, Rect, DrawStage.After, DrawState.PageHeader, info1);
                }
            }
            finally
            {
                Painter.BkMode = 2;
                Painter.Font = font1;
                Painter.Color = color1;
            }
        }

        public virtual void ResetFont()
        {
            this.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
        }

        public virtual void ResetFontColor()
        {
            this.FontColor = EditConsts.DefaultHeaderFontColor;
        }

        public virtual void ResetOffset()
        {
            this.Offset = new Point(8, 8);
        }

        public void ResetReverseOnEvenPages()
        {
            this.ReverseOnEvenPages = false;
        }

        public bool ShouldSerializeCenterText()
        {
            return (this.centerText != string.Empty);
        }

        public bool ShouldSerializeFontColor()
        {
            return (this.fontColor != EditConsts.DefaultHeaderFontColor);
        }

        public bool ShouldSerializeLeftText()
        {
            return (this.leftText != string.Empty);
        }

        public bool ShouldSerializeOffset()
        {
            if (this.offset.X == 0x18)
            {
                return (this.offset.Y != 8);
            }
            return true;
        }

        public bool ShouldSerializeRightText()
        {
            return (this.rightText != string.Empty);
        }

        public void Update()
        {
            if ((this.updateCount == 0) && (this.page != null))
            {
                this.page.Invalidate();
            }
        }


        // Properties
        public string CenterText
        {
            get
            {
                return this.centerText;
            }
            set
            {
                if (this.centerText != value)
                {
                    this.centerText = value;
                    this.Update();
                }
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                if (this.font != value)
                {
                    this.font = value;
                    this.Update();
                }
            }
        }

        public Color FontColor
        {
            get
            {
                return this.fontColor;
            }
            set
            {
                if (this.fontColor != value)
                {
                    this.fontColor = value;
                    this.Update();
                }
            }
        }

        public string LeftText
        {
            get
            {
                return this.leftText;
            }
            set
            {
                if (this.leftText != value)
                {
                    this.leftText = value;
                    this.Update();
                }
            }
        }

        public Point Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                if (this.offset != value)
                {
                    this.offset = value;
                    this.Update();
                }
            }
        }

        [DefaultValue(false)]
        public bool ReverseOnEvenPages
        {
            get
            {
                return this.reverseOnEvenPages;
            }
            set
            {
                if (this.reverseOnEvenPages != value)
                {
                    this.reverseOnEvenPages = value;
                    if (this.page != null)
                    {
                        this.page.Invalidate();
                    }
                }
            }
        }

        public string RightText
        {
            get
            {
                return this.rightText;
            }
            set
            {
                if (this.rightText != value)
                {
                    this.rightText = value;
                    this.Update();
                }
            }
        }

        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.Update();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object XmlInfo
        {
            get
            {
                return new XmlPageHeaderInfo(this);
            }
            set
            {
                ((XmlPageHeaderInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private string centerText;
        private System.Drawing.Font font;
        private Color fontColor;
        private string leftText;
        private Point offset;
        private IEditPage page;
        private Regex regex;
        private bool reverseOnEvenPages;
        private string rightText;
        private int updateCount;
        private bool visible;
    }
}

