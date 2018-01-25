namespace River.Orqa.Editor
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class HyperTextEx : IHyperTextEx, IHyperText
    {
        // Events
        [Browsable(false)]
        public event HyperTextEvent CheckHyperText
        {
            add
            {
                this.owner.Source.CheckHyperText += value;
            }
            remove
            {
                this.owner.Source.CheckHyperText -= value;
            }
        }
        [Browsable(false)]
        public event UrlJumpEvent JumpToUrl;

        // Methods
        public HyperTextEx()
        {
            this.urlColor = EditConsts.DefaultUrlForeColor;
            this.urlStyle = EditConsts.DefaultUrlFontStyle;
            this.showHints = true;
			this.urlJumpArgs = new UrlJumpEventArgs(String.Empty, false);
        }

        public HyperTextEx(ISyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        public void Assign(IHyperTextEx Source)
        {
            this.HighlightUrls = Source.HighlightUrls;
            this.UrlColor = Source.UrlColor;
            this.UrlStyle = Source.UrlStyle;
        }

        public bool IsHyperText(string Text)
        {
            return this.owner.Source.IsHyperText(Text);
        }

        public bool IsUrlAtPoint(int X, int Y)
        {
            string text1;
            return this.IsUrlAtPoint(X, Y, out text1, false);
        }

        protected internal bool IsUrlAtPoint(int X, int Y, out string Url, bool NeedUrl)
        {
            Url = string.Empty;
            if (this.HighlightUrls)
            {
                Point point1 = this.owner.ScreenToText(X, Y);
                return this.IsUrlAtTextPoint(point1.X, point1.Y, out Url, NeedUrl);
            }
            return false;
        }

        protected internal bool IsUrlAtTextPoint(int X, int Y, out string Url, bool NeedUrl)
        {
            bool flag1 = false;
            Url = string.Empty;
            if (this.HighlightUrls)
            {
                StrItem item1 = this.owner.Lines.GetItem(Y);
                if (item1 == null)
                {
                    return flag1;
                }
                short[] numArray1 = item1.ColorData;
                flag1 = ((X >= 0) && (X < numArray1.Length)) && ((((byte) (numArray1[X] >> 8)) & 0x10) != ((byte) 0));
                if (!flag1 || !NeedUrl)
                {
                    return flag1;
                }
                int num1 = X;
                int num2 = X;
                while ((num1 > 0) && ((((byte) (numArray1[num1 - 1] >> 8)) & 0x10) != ((byte) 0)))
                {
                    num1--;
                }
                while ((num2 < (numArray1.Length - 1)) && ((((byte) (numArray1[num2 + 1] >> 8)) & 0x10) != ((byte) 0)))
                {
                    num2++;
                }
                Url = item1.String.Substring(num1, (num2 - num1) + 1);
            }
            return flag1;
        }

        public virtual void ResetHighlightUrls()
        {
            this.HighlightUrls = false;
        }

        public virtual void ResetShowHints()
        {
            this.ShowHints = true;
        }

        public virtual void ResetUrlColor()
        {
            this.UrlColor = EditConsts.DefaultUrlForeColor;
        }

        public virtual void ResetUrlStyle()
        {
            this.UrlStyle = EditConsts.DefaultUrlFontStyle;
        }

        public bool ShouldSerializeUrlColor()
        {
            return (this.urlColor != EditConsts.DefaultUrlForeColor);
        }

        public bool ShouldSerializeUrlStyle()
        {
            return (this.urlStyle != EditConsts.DefaultUrlFontStyle);
        }

        public void UrlJump(string Text)
        {
            this.urlJumpArgs.Text = Text;
            this.urlJumpArgs.Handled = false;
            if (this.JumpToUrl != null)
            {
                this.JumpToUrl(this, this.urlJumpArgs);
            }
            if (!this.urlJumpArgs.Handled)
            {
                try
                {
                    Process.Start(Text);
                }
                catch
                {
                }
            }
        }


        // Properties
        [DefaultValue(false)]
        public bool HighlightUrls
        {
            get
            {
                return this.owner.Source.HighlightUrls;
            }
            set
            {
                this.owner.Source.HighlightUrls = value;
            }
        }

        [DefaultValue(true)]
        public bool ShowHints
        {
            get
            {
                return this.showHints;
            }
            set
            {
                this.showHints = value;
            }
        }

        public Color UrlColor
        {
            get
            {
                return this.urlColor;
            }
            set
            {
                if (this.urlColor != value)
                {
                    this.urlColor = value;
                    if (this.HighlightUrls)
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        public FontStyle UrlStyle
        {
            get
            {
                return this.urlStyle;
            }
            set
            {
                if (this.urlStyle != value)
                {
                    this.urlStyle = value;
                    if (this.HighlightUrls)
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public object XmlInfo
        {
            get
            {
                return new XmlHyperTextExInfo(this);
            }
            set
            {
                ((XmlHyperTextExInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private ISyntaxEdit owner;
        private bool showHints;
        private Color urlColor;
        private UrlJumpEventArgs urlJumpArgs;
        private FontStyle urlStyle;
    }
}

