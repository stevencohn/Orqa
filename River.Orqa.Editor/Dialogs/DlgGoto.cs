namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgGoto : Form, IGotoLineDialog
    {
        // Methods
        public DlgGoto()
        {
            this.components = null;
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public DialogResult Execute(object Sender, int Lines, ref int Line)
        {
            this.Lines = Lines;
            this.Line = Line;
            DialogResult result1 = base.ShowDialog();
            if (result1 == DialogResult.OK)
            {
                Line = this.Line;
            }
            return result1;
        }

        private void InitializeComponent()
        {
            this.laEnterNewLine = new Label();
            this.tbNewLineNumber = new TextBox();
            this.btCancel = new Button();
            this.btOK = new Button();
            this.panel1 = new Panel();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.laEnterNewLine.AutoSize = true;
            this.laEnterNewLine.FlatStyle = FlatStyle.System;
            this.laEnterNewLine.Location = new Point(8, 8);
            this.laEnterNewLine.Name = "laEnterNewLine";
            this.laEnterNewLine.Size = new Size(70, 0x10);
            this.laEnterNewLine.TabIndex = 0;
            this.laEnterNewLine.Text = "Line number:";
            this.tbNewLineNumber.Location = new Point(8, 0x20);
            this.tbNewLineNumber.Name = "tbNewLineNumber";
            this.tbNewLineNumber.Size = new Size(0xd8, 20);
            this.tbNewLineNumber.TabIndex = 1;
			this.tbNewLineNumber.Text = String.Empty;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.FlatStyle = FlatStyle.System;
            this.btCancel.Location = new Point(0x98, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btOK.DialogResult = DialogResult.OK;
            this.btOK.FlatStyle = FlatStyle.System;
            this.btOK.Location = new Point(0x48, 8);
            this.btOK.Name = "btOK";
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.panel1.Anchor = AnchorStyles.Right | (AnchorStyles.Left | AnchorStyles.Bottom);
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btOK);
            this.panel1.Location = new Point(0, 0x3a);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xf2, 40);
            this.panel1.TabIndex = 2;
            base.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new Size(5, 13);
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(0xf2, 0x60);
            base.Controls.Add(this.tbNewLineNumber);
            base.Controls.Add(this.laEnterNewLine);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MinimizeBox = false;
            base.Name = "DlgGoto";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Goto Line";
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }


        // Properties
        public int Line
        {
            get
            {
                return (int.Parse(this.tbNewLineNumber.Text) - 1);
            }
            set
            {
                int num1 = value + 1;
                this.tbNewLineNumber.Text = num1.ToString();
            }
        }

        public int Lines
        {
            set
            {
                this.laEnterNewLine.Text = string.Format(EditConsts.LineNumberCaption, 1, value);
            }
        }


        // Fields
        public Button btCancel;
        public Button btOK;
        public Container components;
        public Label laEnterNewLine;
        public Panel panel1;
        public TextBox tbNewLineNumber;
    }
}

