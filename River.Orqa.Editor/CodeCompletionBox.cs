namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false), DesignTimeVisible(false)]
    public class CodeCompletionBox : CodeCompletionWindow, ICodeCompletionBox, ICodeCompletionWindow, IControlProps
    {
        // Methods
        public CodeCompletionBox(SyntaxEdit Owner) : base(Owner)
        {
            this.dropDownCount = EditConsts.DefaultDropDownCount;
            base.CompletionFlags &= ((CodeCompletionFlags) (-5));
            base.SizeAble = true;
            this.UpdateDropDown();
            CompletionListBox box1 = this.ListBox;
            box1.UpdateSize = (EventHandler) Delegate.Combine(box1.UpdateSize, new EventHandler(this.DoUpdateSize));
        }

        public ICodeCompletionColumn AddColumn()
        {
            return this.ListBox.AddColumn();
        }

        public void ClearColumns()
        {
            this.ListBox.ClearColumns();
        }

        protected override Control CreatePopupControl()
        {
            System.Windows.Forms.ListBox box1 = new CompletionListBox();
            box1.SelectedIndexChanged += new EventHandler(this.SelectionChanged);
            return box1;
        }

        protected override void DoHide()
        {
            base.DoHide();
            this.HideCodeHint();
        }

        protected override void DoShow(Point Position)
        {
            this.Filter = string.Empty;
            this.ListBox.Images = ((base.Provider != null) && (base.Provider.Images != null)) ? base.Provider.Images : base.Images;
            if ((base.Provider != null) && (base.Provider.SelIndex >= 0))
            {
                this.ListBox.SelectedIndex = base.Provider.SelIndex;
            }
            this.DoShowCodeHint();
            base.DoShow(Position);
        }

        protected virtual void DoShowCodeHint()
        {
            this.HideCodeHint();
            this.CodeHintTimer.Enabled = false;
            this.CodeHintTimer.Enabled = true;
        }

        protected void DoUpdateSize(object Sender, EventArgs e)
        {
            this.UpdateAutoSize();
            this.UpdateDropDown();
        }

        ~CodeCompletionBox()
        {
            if (this.codeHintTimer != null)
            {
                this.codeHintTimer.Dispose();
            }
        }

        protected override int GetSelectedIndex()
        {
            return this.ListBox.GetIndex();
        }

        protected virtual void HideCodeHint()
        {
            if (this.codeHint != null)
            {
                this.codeHint.Visible = false;
            }
        }

        private bool IndexMatches(string Text, int Index)
        {
            if ((Index >= 0) && (Index < base.Provider.Count))
            {
                string text1 = ((CodeCompletionProvider) base.Provider).GetText(Index);
                text1 = text1.Substring(0, Math.Min(text1.Length, Text.Length));
                return (string.Compare(Text, text1, true) == 0);
            }
            return false;
        }

        private int IndexOfString(string Text)
        {
            if (this.IndexMatches(Text, this.ListBox.SelectedIndex))
            {
                return this.ListBox.SelectedIndex;
            }
            for (int num1 = 0; num1 < base.Provider.Count; num1++)
            {
                if (this.IndexMatches(Text, num1))
                {
                    return num1;
                }
            }
            return -1;
        }

        public ICodeCompletionColumn InsertColumn(int Index)
        {
            return this.ListBox.InsertColumn(Index);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected virtual void OnShowCodeHint(object Sender, EventArgs e)
        {
            this.ShowCodeHint(this.ListBox.GetIndex());
            this.CodeHintTimer.Enabled = false;
        }

        protected override bool PerformSearch()
        {
            if (base.Provider == null)
            {
                return false;
            }
            SyntaxEdit edit1 = base.GetSyntaxEdit();
            if (edit1 != null)
            {
                Point point1 = edit1.Position;
                if ((point1.Y == base.StartPos.Y) && (point1.X > base.StartPos.X))
                {
                    string text1 = edit1.Lines[point1.Y];
                    if (base.StartPos.X >= text1.Length)
                    {
                        goto Label_0117;
                    }
                    text1 = (point1.X < text1.Length) ? text1.Substring(base.StartPos.X, point1.X - base.StartPos.X) : text1.Substring(base.StartPos.X);
                    if (this.Filtered)
                    {
                        this.Filter = text1.Trim();
                    }
                    else
                    {
                        int num1 = this.IndexOfString(text1);
                        if (num1 >= 0)
                        {
                            this.ListBox.SelectedIndex = num1;
                        }
                    }
                    return true;
                }
                if (this.Filtered)
                {
                    this.Filter = string.Empty;
                }
            }
        Label_0117:
            return false;
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if ((m.Msg == 0x100) || (m.Msg == 260))
            {
                Keys keys1 = ((Keys) m.WParam.ToInt32()) & Keys.KeyCode;
                if (Array.IndexOf(this.ListBox.NavKeys, keys1) >= 0)
                {
                    return false;
                }
            }
            SyntaxEdit edit1 = base.GetSyntaxEdit();
            if (((m.Msg == 0x102) && (edit1 != null)) && (Control.ModifierKeys == Keys.None))
            {
                char ch1 = (char) ((ushort) m.WParam.ToInt32());
                if ((((base.CompletionFlags & CodeCompletionFlags.AcceptOnDelimiter) != CodeCompletionFlags.None) && (ch1 >= ' ')) && edit1.Lines.IsDelimiter(ch1))
                {
                    this.Close(true);
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

		Rectangle IControlProps.Bounds
		{
			get { return base.Bounds; }
			set { base.Bounds = value; }
		}

		Rectangle IControlProps.ClientRectangle
		{
			get { return base.ClientRectangle; }
		}

		int IControlProps.Height
		{
			get { return base.Height; }
			set { base.Height = value; }
		}

		bool IControlProps.IsHandleCreated
		{
			get { return base.IsHandleCreated; }
		}

		int IControlProps.Left
		{
			get { return base.Left; }
			set { base.Left = value; }
		}

		Point IControlProps.Location
		{
			get { return base.Location; }
			set { base.Location = value; }
		}

		Control IControlProps.Parent
		{
			get { return base.Parent; }
			set { base.Parent = value; }
		}

		int IControlProps.Top
		{
			get { return base.Top; }
			set { base.Top = value; }
		}

		bool IControlProps.Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		int IControlProps.Width
		{
			get { return base.Width; }
			set { base.Width = value; }
		}


        public void RemoveColumnAt(int Index)
        {
            this.ListBox.RemoveColumnAt(Index);
        }

        public override void ResetContent()
        {
            this.ListBox.ResetContent();
        }

        public virtual void ResetDropDownCount()
        {
            this.DropDownCount = EditConsts.DefaultDropDownCount;
        }

        public override void ResetSizeAble()
        {
            base.SizeAble = true;
        }

        protected virtual void SelectionChanged(object Sender, EventArgs e)
        {
            this.DoShowCodeHint();
        }

        protected override void SetProvider(ICodeCompletionProvider Provider)
        {
            this.ListBox.Provider = Provider;
        }

        protected virtual void ShowCodeHint(int Index)
        {
            if (((base.Visible && (Index >= 0)) && ((base.Provider != null) && base.Provider.ShowDescriptions)) && (Index < base.Provider.Count))
            {
                string text1 = base.Provider.Descriptions[Index];
                if (text1 != string.Empty)
                {
                    CodeCompletionHint hint1 = this.CodeHint;
                    ((QuickInfo) hint1.Provider).Text = text1 + Index.ToString();
                    hint1.ResetContent();
                    hint1.PopupAt(base.PointToScreen(new Point(base.Width, (this.ListBox.SelectedIndex - this.ListBox.TopIndex) * this.ListBox.ItemHeight)));
                }
            }
            else
            {
                this.HideCodeHint();
            }
        }

        protected override void UpdateAutoSize()
        {
            if (base.AutoSize)
            {
                int num1 = (this.ListBox.ItemHeight * Math.Min(this.ListBox.Items.Count, EditConsts.MaxDropDownCount)) + EditConsts.DefaultRowSeparator;
                int num2 = this.ListBox.ItemWidth + (EditConsts.DefaultColumnSeparator * 4);
                num2 += Win32.GetSystemMetrics(0x15);
                base.ClientSize = new Size(Math.Max(num2, EditConsts.DefaultMinListBoxWidth), Math.Max(num1, EditConsts.DefaultMinListBoxHeight));
            }
        }

        private void UpdateDropDown()
        {
            if (!base.AutoSize)
            {
                base.ClientSize = new Size(base.ClientRectangle.Width, Math.Max((int) ((this.dropDownCount * this.ListBox.ItemHeight) + EditConsts.DefaultRowSeparator), EditConsts.DefaultMinListBoxHeight));
            }
        }


        // Properties
        protected internal virtual CodeCompletionHint CodeHint
        {
            get
            {
                if (this.codeHint == null)
                {
                    this.codeHint = new CodeCompletionHint(base.GetSyntaxEdit());
                    this.codeHint.Enabled = false;
                    this.codeHint.Provider = new QuickInfo();
                }
                return this.codeHint;
            }
        }

        protected internal Timer CodeHintTimer
        {
            get
            {
                if (this.codeHintTimer == null)
                {
                    this.codeHintTimer = new Timer();
                    this.codeHintTimer.Enabled = false;
                    this.codeHintTimer.Interval = Consts.DefaultHintDelay;
                    this.codeHintTimer.Tick += new EventHandler(this.OnShowCodeHint);
                }
                return this.codeHintTimer;
            }
        }

        public ICodeCompletionColumn[] Columns
        {
            get
            {
                ICodeCompletionColumn[] columnArray1 = new ICodeCompletionColumn[this.ListBox.ColumnCount];
                this.ListBox.Columns.CopyTo(columnArray1);
                return columnArray1;
            }
        }

        public int DropDownCount
        {
            get
            {
                return this.dropDownCount;
            }
            set
            {
                if (this.dropDownCount != value)
                {
                    this.dropDownCount = value;
                    this.UpdateDropDown();
                }
            }
        }

        public string Filter
        {
            get
            {
                return this.ListBox.Filter;
            }
            set
            {
                this.ListBox.Filter = value;
            }
        }

        public bool Filtered
        {
            get
            {
                return this.ListBox.Filtered;
            }
            set
            {
                this.ListBox.Filtered = value;
            }
        }

        protected internal CompletionListBox ListBox
        {
            get
            {
                return (CompletionListBox) base.PopupControl;
            }
        }


        // Fields
        private CodeCompletionHint codeHint;
        private Timer codeHintTimer;
        private int dropDownCount;
    }
}

