namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CompletionListBox : ListBox
    {
        // Methods
        public CompletionListBox()
        {
            this.filtered = false;
			this.filter = String.Empty;
            this.itemWidth = 0;
            Keys[] keysArray1 = new Keys[4] { Keys.Up, Keys.Down, Keys.Prior, Keys.Next } ;
            this.navKeys = keysArray1;
            this.columns = new ArrayList();
            this.columnWidths = new ArrayList();
            this.painter = new TextPainter(null);
            this.painter.Font = this.Font;
            this.DrawMode = DrawMode.OwnerDrawVariable;
            base.BorderStyle = BorderStyle.None;
            base.HorizontalScrollbar = true;
        }

        public ICodeCompletionColumn AddColumn()
        {
            ICodeCompletionColumn column1 = new CodeCompletionColumn();
            this.columns.Add(column1);
            return column1;
        }

        public void ClearColumns()
        {
            this.columns.Clear();
        }

        private void DrawColumn(Graphics Graphics, Rectangle Rect, string Text, Color ForeColor, FontStyle Style)
        {
            if (Text != null)
            {
                this.painter.BeginPaint(Graphics);
                try
                {
                    this.painter.Color = ForeColor;
                    this.painter.BkMode = 1;
                    this.painter.FontStyle = Style;
                    this.painter.TextOut(Text, -1, Rect, 0);
                }
                finally
                {
                    this.painter.EndPaint(Graphics);
                }
            }
        }

        public ICodeCompletionColumn GetColumn(int Index)
        {
            return (ICodeCompletionColumn) this.columns[Index];
        }

        protected internal int GetIndex()
        {
            return this.GetIndex(this.SelectedIndex);
        }

        protected internal int GetIndex(int Index)
        {
            if ((this.filtered && (Index >= 0)) && (Index < base.Items.Count))
            {
                string text1 = (string) base.Items[Index];
                int num1 = text1.IndexOf("|");
                if (num1 >= 0)
                {
                    return int.Parse(text1.Substring(0, num1));
                }
            }
            return Index;
        }

        public ICodeCompletionColumn InsertColumn(int Index)
        {
            ICodeCompletionColumn column1 = new CodeCompletionColumn();
            this.columns.Insert(Index, column1);
            return column1;
        }

        protected virtual bool IsFiltered(string s)
        {
            if ((this.filtered && (this.filter != null)) && (this.filter != string.Empty))
            {
                return s.ToLower().StartsWith(this.filter.ToLower());
            }
            return true;
        }

        private int MeasureWidth(string Text, FontStyle Style)
        {
            if (Text != null)
            {
                this.painter.FontStyle = Style;
                return this.painter.StringWidth(Text);
            }
            return 0;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int num1 = this.GetIndex(e.Index);
            if (((this.provider == null) || (num1 < 0)) || (num1 >= this.provider.Count))
            {
                base.OnDrawItem(e);
            }
            else
            {
                bool flag1 = (e.State & DrawItemState.Disabled) != DrawItemState.None;
                bool flag2 = ((e.State & DrawItemState.Focus) != DrawItemState.None) && ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None);
                bool flag3 = (e.State & DrawItemState.Selected) != DrawItemState.None;
                Size size1 = (this.images != null) ? new Size(this.images.ImageSize.Width + EditConsts.DefaultColumnSeparator, this.images.ImageSize.Height) : new Size(0, 0);
                if (this.images != null)
                {
                    int num2 = ((CodeCompletionProvider) this.provider).GetImageIndex(num1);
                    if ((num2 >= 0) && (num2 < this.images.Images.Count))
                    {
                        this.images.Draw(e.Graphics, e.Bounds.Left, e.Bounds.Top + ((e.Bounds.Height - size1.Height) / 2), num2);
                    }
                    e.Graphics.TranslateTransform((float) size1.Width, 0f);
                }
                e.DrawBackground();
                if (flag2)
                {
                    e.DrawFocusRectangle();
                }
                int num3 = e.Bounds.Left + size1.Width;
                for (int num4 = 0; num4 < this.provider.ColumnCount; num4++)
                {
                    ICodeCompletionColumn column1 = (num4 < this.columns.Count) ? this.GetColumn(num4) : null;
                    if ((column1 == null) || column1.Visible)
                    {
                        Color color1 = (column1 != null) ? column1.ForeColor : this.ForeColor;
                        int num5 = (int) this.columnWidths[num4];
                        Rectangle rectangle1 = new Rectangle(num3, e.Bounds.Top, num5, e.Bounds.Height);
                        if (flag1)
                        {
                            color1 = EditConsts.DefaultDisabledForeColor;
                        }
                        else if (flag3)
                        {
                            color1 = EditConsts.DefaultHighlightForeColor;
                        }
                        this.DrawColumn(e.Graphics, rectangle1, this.provider.GetColumnText(num1, num4), color1, (column1 != null) ? column1.FontStyle : FontStyle.Regular);
                        num3 += num5;
                    }
                }
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.painter.Clear();
            this.painter.Font = this.Font;
            this.UpdateControlSize();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((Array.IndexOf(EditConsts.NavKeys, e.KeyCode) >= 0) && (Array.IndexOf(this.navKeys, e.KeyCode) < 0))
            {
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            if (this.images != null)
            {
                e.ItemHeight = Math.Max(e.ItemHeight, this.images.ImageSize.Height);
            }
        }

        protected virtual void ProviderChanged()
        {
            base.BeginUpdate();
            try
            {
                base.Items.Clear();
                this.columnWidths.Clear();
                this.itemWidth = 0;
                if (this.provider != null)
                {
                    for (int num1 = 0; num1 < this.provider.Count; num1++)
                    {
                        string text1 = this.provider.Strings[num1];
                        if (this.IsFiltered(text1))
                        {
                            base.Items.Add(num1.ToString() + "|" + text1);
                        }
                    }
                    for (int num3 = 0; num3 < this.provider.ColumnCount; num3++)
                    {
                        int num2 = 0;
                        ICodeCompletionColumn column1 = (num3 < this.columns.Count) ? this.GetColumn(num3) : null;
                        for (int num4 = 0; num4 < this.provider.Count; num4++)
                        {
                            if (this.IsFiltered(this.provider.Strings[num4]))
                            {
                                num2 = Math.Max(num2, this.MeasureWidth(this.provider.GetColumnText(num4, num3), (column1 != null) ? column1.FontStyle : FontStyle.Regular));
                            }
                        }
                        num2 += EditConsts.DefaultColumnSeparator;
                        this.itemWidth += num2;
                        this.columnWidths.Add(num2);
                    }
                }
                if (this.images != null)
                {
                    this.itemWidth += (this.images.ImageSize.Width + EditConsts.DefaultColumnSeparator);
                }
                this.UpdateControlSize();
            }
            finally
            {
                base.EndUpdate();
            }
        }

        public void RemoveColumnAt(int Index)
        {
            this.columns.RemoveAt(Index);
        }

        public void ResetContent()
        {
            this.ProviderChanged();
        }

        public void ResetContent(int Index)
        {
            base.BeginUpdate();
            try
            {
                this.ResetContent();
                if ((Index >= 0) && (Index < base.Items.Count))
                {
                    this.SelectedIndex = Index;
                }
            }
            finally
            {
                base.EndUpdate();
            }
        }

        protected internal void UpdateControlSize()
        {
            int num1 = this.painter.FontHeight + EditConsts.DefaultRowSeparator;
            if (this.images != null)
            {
                num1 = Math.Max(num1, this.images.ImageSize.Height);
            }
            this.ItemHeight = num1;
            base.HorizontalExtent = ((this.provider != null) && (this.provider.ColumnCount != 0)) ? this.itemWidth : 0;
            if (this.UpdateSize != null)
            {
                this.UpdateSize(this, EventArgs.Empty);
            }
        }


        // Properties
        public int ColumnCount
        {
            get
            {
                return this.columns.Count;
            }
        }

        protected internal ArrayList Columns
        {
            get
            {
                return this.columns;
            }
        }

        public string Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                if (this.filter != value)
                {
                    this.filter = value;
                    if (this.filtered)
                    {
                        this.ResetContent(0);
                    }
                }
            }
        }

        public bool Filtered
        {
            get
            {
                return this.filtered;
            }
            set
            {
                if (this.filtered != value)
                {
                    this.filtered = value;
                    this.ResetContent(0);
                }
            }
        }

        public ImageList Images
        {
            get
            {
                return this.images;
            }
            set
            {
                if (this.images != value)
                {
                    this.images = value;
                    this.UpdateControlSize();
                }
            }
        }

        public int ItemWidth
        {
            get
            {
                return this.itemWidth;
            }
        }

        public Keys[] NavKeys
        {
            get
            {
                return this.navKeys;
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
        private ArrayList columns;
        private ArrayList columnWidths;
        private string filter;
        private bool filtered;
        private ImageList images;
        private int itemWidth;
        private Keys[] navKeys;
        private ITextPainter painter;
        private ICodeCompletionProvider provider;
        private EventHandler updateSize;
    }
}

