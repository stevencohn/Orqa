namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class CodeCompletionHint : CodeCompletionWindow, ICodeCompletionHint, ICodeCompletionWindow, IControlProps
    {
        // Methods
        public CodeCompletionHint(SyntaxEdit Owner) : base(Owner)
        {
            base.SizeAble = false;
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.curPos = new Point(0, 0);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | (ControlStyles.StandardDoubleClick | (ControlStyles.StandardClick | (ControlStyles.Opaque | ControlStyles.UserPaint))), true);
            CompletionHint hint1 = this.Hint;
            hint1.UpdateSize = (EventHandler) Delegate.Combine(hint1.UpdateSize, new EventHandler(this.DoUpdateSize));
            base.CompletionFlags &= ((CodeCompletionFlags) (-291));
        }

        protected override Control CreatePopupControl()
        {
            return new CompletionHint();
        }

        protected override void DoHide()
        {
            base.DoHide();
            if (this.hideTimer != null)
            {
                this.hideTimer.Enabled = false;
            }
        }

        protected override void DoShow(Point Position)
        {
            base.DoShow(Position);
            this.MakeBold();
            base.Update();
            if (this.autoHide)
            {
                this.HideTimer.Enabled = true;
            }
        }

        protected void DoUpdateSize(object Sender, EventArgs e)
        {
            base.Size = this.Hint.HintSize;
        }

        ~CodeCompletionHint()
        {
            if (this.hideTimer != null)
            {
                this.hideTimer.Dispose();
            }
        }

        protected override int GetSelectedIndex()
        {
            return this.Hint.SelectedIndex;
        }

        private void MakeBold()
        {
            SyntaxEdit edit1 = base.GetSyntaxEdit();
            if (((edit1 != null) && (base.StartPos.X >= 0)) && ((base.StartPos.Y >= 0) && !this.curPos.Equals(edit1.Position)))
            {
                this.curPos = edit1.Position;
                if (base.StartPos.Y == this.curPos.Y)
                {
                    this.Hint.MakeBold(edit1.RemovePlainText(this.curPos.Y), base.StartPos.X, this.curPos.X);
                }
            }
        }

        protected void OnHideHint(object Sender, EventArgs e)
        {
            this.Close(false);
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            SyntaxEdit edit1;
            if ((m.Msg == 0x100) || (m.Msg == 260))
            {
                switch ((((Keys) m.WParam.ToInt32()) & Keys.KeyCode))
                {
                    case Keys.Prior:
                    case Keys.Next:
                    case Keys.End:
                    case Keys.Home:
                    {
                        this.Close(false);
                        goto Label_00B0;
                    }
                    case Keys.Left:
                    case Keys.Right:
                    {
                        goto Label_00B0;
                    }
                    case Keys.Up:
                    {
                        if (!this.Hint.NeedArrows())
                        {
                            this.Close(false);
                            goto Label_0083;
                        }
                        this.Hint.ChangeSelection(false);
                        goto Label_0083;
                    }
                    case Keys.Down:
                    {
                        if (!this.Hint.NeedArrows())
                        {
                            this.Close(false);
                            goto Label_00A7;
                        }
                        this.Hint.ChangeSelection(true);
                        goto Label_00A7;
                    }
                }
            }
            goto Label_00B0;
        Label_0083:
            return true;
        Label_00A7:
            return true;
        Label_00B0:
            edit1 = base.GetSyntaxEdit();
            if ((m.Msg == 0x102) && (edit1 != null))
            {
                char ch1 = (char) ((ushort) m.WParam.ToInt32());
                if (ch1 == ')')
                {
                    this.Close(false);
                }
            }
            this.MakeBold();
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


        public virtual void ResetAutoHide()
        {
            this.AutoHide = false;
        }

        public virtual void ResetAutoHidePause()
        {
            this.AutoHidePause = EditConsts.DefaultHideHintDelay;
        }

        public override void ResetContent()
        {
            this.Hint.ResetContent();
        }

        protected override void SetProvider(ICodeCompletionProvider Provider)
        {
            this.Hint.Provider = Provider;
        }


        // Properties
        public bool AutoHide
        {
            get
            {
                return this.autoHide;
            }
            set
            {
                if (this.autoHide != value)
                {
                    this.autoHide = value;
                    if (value && base.Visible)
                    {
                        this.HideTimer.Enabled = true;
                    }
                }
            }
        }

        public int AutoHidePause
        {
            get
            {
                if (this.hideTimer == null)
                {
                    return EditConsts.DefaultHideHintDelay;
                }
                return this.hideTimer.Interval;
            }
            set
            {
                this.HideTimer.Interval = value;
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                Win32.InitCommonControls(8);
                System.Windows.Forms.CreateParams params1 = base.CreateParams;
                params1.Parent = IntPtr.Zero;
                params1.ClassName = "tooltips_class32";
                params1.Style |= 1;
                params1.ExStyle = 0;
                return params1;
            }
        }

        protected internal Timer HideTimer
        {
            get
            {
                if (this.hideTimer == null)
                {
                    this.hideTimer = new Timer();
                    this.hideTimer.Enabled = false;
                    this.hideTimer.Interval = EditConsts.DefaultHideHintDelay;
                    this.hideTimer.Tick += new EventHandler(this.OnHideHint);
                }
                return this.hideTimer;
            }
        }

        protected internal CompletionHint Hint
        {
            get
            {
                return (CompletionHint) base.PopupControl;
            }
        }


        // Fields
        private bool autoHide;
        private Point curPos;
        private Timer hideTimer;
    }
}

