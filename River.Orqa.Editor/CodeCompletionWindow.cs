namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxItem(false), DesignTimeVisible(false)]
    public class CodeCompletionWindow : Form, ICodeCompletionWindow, IControlProps
    {
        // Events
        public event ClosePopupEvent ClosePopup;

        // Methods
        public CodeCompletionWindow(SyntaxEdit Owner)
        {
            this.completionFlags = EditConsts.DefaultCodeCompletionFlags;
            this.ownerControl = Owner;
            this.startPos = new Point(0, 0);
            this.endPos = new Point(0, 0);
            base.MinimumSize = new Size(this.Font.Height + 2, this.Font.Height + 2);
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Width = 200;
            base.Height = 200;
            base.ShowInTaskbar = false;
            base.ControlBox = false;
            this.BackColor = Consts.DefaultControlBackColor;
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.TopLevel = true;
            base.DoubleClick += new EventHandler(this.DoDoubleClick);
            base.Click += new EventHandler(this.DoClick);
            base.LostFocus += new EventHandler(this.DoLostFocus);
            this.popupControl = this.CreatePopupControl();
            this.popupControl.DoubleClick += new EventHandler(this.DoDoubleClick);
            this.popupControl.Click += new EventHandler(this.DoClick);
            this.popupControl.LostFocus += new EventHandler(this.DoLostFocus);
            base.Controls.Clear();
            base.Controls.Add(this.popupControl);
            this.popupControl.Dock = DockStyle.Fill;
            this.UpdateAutoSize();
        }

        public virtual void Close(bool Accept)
        {
            if (base.Visible)
            {
                PopupHookManager.PopupClosed(this);
                this.DoHide();
                CodeCompletionProvider provider1 = this.provider as CodeCompletionProvider;
                if (provider1 != null)
                {
                    provider1.SelIndex = this.GetSelectedIndex();
                }
                if ((this.ClosePopup != null) || ((provider1 != null) && provider1.IsCloseEventAssigned()))
                {
                    ClosingEventArgs args1 = new ClosingEventArgs(Accept, provider1);
                    if (((provider1 != null) && (provider1.ColumnCount != 0)) && ((provider1.SelIndex >= 0) && (provider1.SelIndex < provider1.Count)))
                    {
                        args1.Text = provider1.GetText(provider1.SelIndex);
                    }
                    else
                    {
                        args1.Text = string.Empty;
                    }
                    if ((provider1 != null) && provider1.IsCloseEventAssigned())
                    {
                        provider1.DoClosePopup(this, args1);
                    }
                    if (this.ClosePopup != null)
                    {
                        this.ClosePopup(this, args1);
                    }
                }
            }
        }

        public virtual void CloseDelayed(bool Accept)
        {
            if (base.IsHandleCreated)
            {
                Win32.PostMessage(base.Handle, 0x401, Accept ? new IntPtr(1) : IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                this.Close(Accept);
            }
        }

        protected virtual Control CreatePopupControl()
        {
            return null;
        }

        protected void DoClick(object sender, EventArgs e)
        {
            if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnClick) != CodeCompletionFlags.None)
            {
                this.Close(true);
            }
        }

        protected void DoDoubleClick(object sender, EventArgs e)
        {
            if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnDblClick) != CodeCompletionFlags.None)
            {
                this.Close(true);
            }
        }

        protected virtual void DoHide()
        {
            base.Visible = false;
        }

        protected void DoLostFocus(object sender, EventArgs e)
        {
            if (!this.showing && ((this.CompletionFlags & CodeCompletionFlags.CloseOnLostFocus) != CodeCompletionFlags.None))
            {
                this.Close(false);
            }
        }

        protected virtual void DoShow(Point Position)
        {
            base.Location = Position;
            Win32.SetWindowPos(base.Handle, (IntPtr) (-1), Position.X, Position.Y, base.Width, base.Height, 80);
            base.Visible = true;
            if (this.popupControl.CanFocus)
            {
                this.popupControl.Focus();
            }
            this.PerformSearch();
        }

        private void EnsureVisible(ref Point Position)
        {
            Rectangle rectangle1 = new Rectangle(Position, base.Bounds.Size);
            int num1 = rectangle1.Bottom - SystemInformation.WorkingArea.Height;
            if (num1 > 0)
            {
                Position.Y -= num1;
            }
            num1 = rectangle1.Right - SystemInformation.WorkingArea.Width;
            if (num1 > 0)
            {
                Position.X -= num1;
            }
        }

        ~CodeCompletionWindow()
        {
            this.popupControl.DoubleClick -= new EventHandler(this.DoDoubleClick);
            this.popupControl.Click -= new EventHandler(this.DoClick);
            this.popupControl.LostFocus -= new EventHandler(this.DoLostFocus);
        }

        protected virtual int GetSelectedIndex()
        {
            return -1;
        }

        protected SyntaxEdit GetSyntaxEdit()
        {
            if (!(this.ownerControl is SyntaxEdit))
            {
                return null;
            }
            return (SyntaxEdit) this.ownerControl;
        }

        public bool IsFocused()
        {
            if (!this.Focused)
            {
                return this.popupControl.Focused;
            }
            return true;
        }

        private bool IsPositionValid()
        {
            SyntaxEdit edit1 = this.GetSyntaxEdit();
            if (edit1 == null)
            {
                return true;
            }
            Point point1 = edit1.Position;
            if ((((this.startPos.Y < 0) || (point1.Y == this.startPos.Y)) && ((this.endPos.Y < 0) || (point1.Y == this.endPos.Y))) && ((this.startPos.X < 0) || (point1.X >= this.startPos.X)))
            {
                if (this.endPos.X >= 0)
                {
                    return (point1.X <= this.endPos.X);
                }
                return true;
            }
            return false;
        }

        protected virtual bool PerformSearch()
        {
            return false;
        }

        public void Popup()
        {
            this.PopupAt(Control.MousePosition);
        }

        public void PopupAt(Point Position)
        {
            this.showing = true;
            try
            {
                if ((this.completionFlags & CodeCompletionFlags.FeetToScreen) > CodeCompletionFlags.None)
                {
                    this.EnsureVisible(ref Position);
                }
                PopupHookManager.PopupClosed(this);
                PopupHookManager.PopupShowing(this);
                this.DoShow(Position);
            }
            finally
            {
                this.showing = false;
            }
        }

        public virtual void PopupAt(int X, int Y)
        {
            this.PopupAt(new Point(X, Y));
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            Keys keys1 = Keys.None;
            if ((m.Msg == 0x100) || (m.Msg == 260))
            {
                keys1 = ((Keys) m.WParam.ToInt32()) & Keys.KeyCode;
                Keys keys2 = keys1;
                if (keys2 != Keys.Return)
                {
                    if ((keys2 == Keys.Escape) && ((this.CompletionFlags & CodeCompletionFlags.CloseOnEscape) != CodeCompletionFlags.None))
                    {
                        this.Close(false);
                        return true;
                    }
                }
                else if ((this.CompletionFlags & CodeCompletionFlags.AcceptOnEnter) != CodeCompletionFlags.None)
                {
                    this.Close(true);
                    return true;
                }
            }
            SyntaxEdit edit1 = this.GetSyntaxEdit();
            if (edit1 != null)
            {
                edit1.DoProcessKeyMessage(ref m);
            }
            if (((this.CompletionFlags & CodeCompletionFlags.KeepActive) == CodeCompletionFlags.None) && !this.IsPositionValid())
            {
                this.Close(false);
            }
            if (((m.Msg == 0x102) || (keys1 == Keys.Back)) && this.PerformSearch())
            {
                return true;
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

        void IControlProps.Invalidate()
        {
            base.Invalidate();
        }

        void IControlProps.Invalidate(Rectangle rectangle1)
        {
            base.Invalidate(rectangle1);
        }

        Point IControlProps.PointToClient(Point point1)
        {
            return base.PointToClient(point1);
        }

        Point IControlProps.PointToScreen(Point point1)
        {
            return base.PointToScreen(point1);
        }


        void IControlProps.Update()
        {
            base.Update();
        }

        public virtual void ResetAutoSize()
        {
            this.AutoSize = false;
        }

        public virtual void ResetCodeCompletionFlags()
        {
            this.CompletionFlags = EditConsts.DefaultCodeCompletionFlags;
        }

        public virtual void ResetContent()
        {
        }

        public virtual void ResetSizeAble()
        {
            this.SizeAble = false;
        }

        protected virtual void SetProvider(ICodeCompletionProvider provider)
        {
        }

        protected virtual void UpdateAutoSize()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x401)
            {
                this.Close(m.WParam != IntPtr.Zero);
            }
            base.WndProc(ref m);
        }


        // Properties
		// SMC: added override
        public override bool AutoSize
        {
            get
            {
                return this.autoSize;
            }
            set
            {
                if (this.autoSize != value)
                {
                    this.autoSize = value;
                    if (value)
                    {
                        this.UpdateAutoSize();
                    }
                }
            }
        }

        public CodeCompletionFlags CompletionFlags
        {
            get
            {
                return this.completionFlags;
            }
            set
            {
                this.completionFlags = value;
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                uint num1 = 0x80000000;
                System.Windows.Forms.CreateParams params1 = base.CreateParams;
                params1.Style |= ((int) num1);
                return params1;
            }
        }

        public Point EndPos
        {
            get
            {
                return this.endPos;
            }
            set
            {
                this.endPos = value;
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
                this.images = value;
            }
        }

        public Control OwnerControl
        {
            get
            {
                return this.ownerControl;
            }
            set
            {
                this.ownerControl = value;
            }
        }

        public Control PopupControl
        {
            get
            {
                return this.popupControl;
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
                    this.SetProvider(value);
                }
            }
        }

        public bool SizeAble
        {
            get
            {
                return this.sizeAble;
            }
            set
            {
                if (this.sizeAble != value)
                {
                    this.sizeAble = value;
                    if (value)
                    {
                        base.FormBorderStyle = FormBorderStyle.Sizable;
                    }
                    else
                    {
                        base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    }
                }
            }
        }

        public Point StartPos
        {
            get
            {
                return this.startPos;
            }
            set
            {
                this.startPos = value;
            }
        }


        // Fields
        private bool autoSize;
        private CodeCompletionFlags completionFlags;
        private Point endPos;
        private ImageList images;
        private Control ownerControl;
        private Control popupControl;
        private ICodeCompletionProvider provider;
        private bool showing;
        private bool sizeAble;
        private Point startPos;
    }
}

