namespace QWhale.Design.Dialogs
{
    using QWhale.Design;
    using River.Orqa.Editor;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class DlgFlagEnumeration : Form
    {
        // Methods
        public DlgFlagEnumeration()
        {
            this.components = null;
            this.mainEditor = null;
            this.editValue = null;
            this.originalValue = null;
            this.InitializeComponent();
            this.lockCheckUpdate = 0;
            base.TopLevel = false;
            this.Font = Control.DefaultFont;
            this.listBox = new FlagEnumerationListBox();
            this.listBox.Font = Control.DefaultFont;
            this.listBox.BorderStyle = BorderStyle.None;
            Rectangle rectangle1 = base.ClientRectangle;
            this.listBox.SetBounds(rectangle1.Left, rectangle1.Top, rectangle1.Width, rectangle1.Height);
            this.listBox.ItemCheck += new ItemCheckEventHandler(this.listBox_ItemCheckEventHandler);
            this.listBox.Visible = true;
            base.ClientSize = new Size(0, this.listBox.ItemHeight * 8);
            base.Controls.Add(this.listBox);
        }

        public DlgFlagEnumeration(FlagEnumerationEditor editor) : this()
        {
            this.mainEditor = editor;
        }

        protected void BeginUpdate()
        {
            this.lockCheckUpdate++;
        }

        protected void ClearAll()
        {
            foreach (string text1 in this.listBox.Items)
            {
                if (text1 != EditConsts.EmptyOption)
                {
                    this.DisableOption(text1);
                }
            }
        }

        protected void DisableOption(string optionName)
        {
            if (optionName == EditConsts.EmptyOption)
            {
                this.SelectAll();
            }
            else
            {
                this.FromInt(((int) this.editValue) & ~this.GetOptionValue(optionName));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void EnableOption(string optionName)
        {
            if (optionName == EditConsts.EmptyOption)
            {
                this.ClearAll();
            }
            else
            {
                this.FromInt(((int) this.editValue) | this.GetOptionValue(optionName));
            }
        }

        protected void EndUpdate()
        {
            this.lockCheckUpdate--;
        }

        protected void FromInt(int value)
        {
            System.Type type1 = this.EditValue.GetType();
            this.editValue = Activator.CreateInstance(type1);
            type1.GetFields()[0].SetValue(this.editValue, value);
        }

        protected ArrayList GetFields(System.Type type)
        {
            ArrayList list1 = new ArrayList();
            FieldInfo[] infoArray1 = type.GetFields();
            FieldInfo[] infoArray2 = infoArray1;
            for (int num1 = 0; num1 < infoArray2.Length; num1++)
            {
                FieldInfo info1 = infoArray2[num1];
                if (!info1.IsSpecialName)
                {
                    list1.Add(info1);
                }
            }
            list1.Sort(new FieldsComparer());
            return list1;
        }

        protected int GetOptionValue(string optionName)
        {
            System.Type type1 = this.EditValue.GetType();
            return (int) type1.GetField(optionName).GetValue(this.EditValue);
        }

        private void InitializeComponent()
        {
            this.AutoScaleDimensions = new Size(5, 13);
            base.ClientSize = new Size(0x124, 0x10a);
            base.FormBorderStyle = FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DlgFlagEnumeration";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.WindowsDefaultBounds;
            this.Text = "DlgFlagEnumeration";
            base.TopMost = true;
        }

        protected bool IsOptionEnabled(string optionName)
        {
            int num1 = (int) this.editValue;
            if (optionName == EditConsts.EmptyOption)
            {
                return (num1 == this.GetOptionValue(optionName));
            }
            return ((num1 & this.GetOptionValue(optionName)) == this.GetOptionValue(optionName));
        }

        protected void listBox_ItemCheckEventHandler(object sender, ItemCheckEventArgs e)
        {
            if (this.lockCheckUpdate == 0)
            {
                this.BeginUpdate();
                try
                {
                    string text1 = this.listBox.Items[e.Index].ToString();
                    if (e.NewValue == CheckState.Checked)
                    {
                        this.EnableOption(text1);
                    }
                    else
                    {
                        this.DisableOption(text1);
                    }
                    this.UpdateListBox();
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Return)
            {
                if (this.mainEditor != null)
                {
                    this.mainEditor.Service.CloseDropDown();
                }
                return true;
            }
            if (keyData != Keys.Escape)
            {
                return base.ProcessDialogKey(keyData);
            }
            this.editValue = this.originalValue;
            if (this.mainEditor != null)
            {
                this.mainEditor.Service.CloseDropDown();
            }
            return true;
        }

        protected void SelectAll()
        {
            foreach (string text1 in this.listBox.Items)
            {
                if (text1 != EditConsts.EmptyOption)
                {
                    this.EnableOption(text1);
                }
            }
        }

        protected void UpdateListBox()
        {
            this.listBox.ListBoxBeginUpdate();
            try
            {
                for (int num1 = 0; num1 < this.listBox.Items.Count; num1++)
                {
                    string text1 = this.listBox.Items[num1].ToString();
                    this.listBox.SetItemChecked(num1, this.IsOptionEnabled(text1));
                }
            }
            finally
            {
                this.listBox.ListBoxEndUpdate();
            }
        }


        // Properties
        public object EditValue
        {
            get
            {
                return this.editValue;
            }
            set
            {
                if (this.editValue != value)
                {
                    this.editValue = value;
                    this.originalValue = value;
                    this.listBox.Items.Clear();
                    ArrayList list1 = this.GetFields(this.editValue.GetType());
                    this.BeginUpdate();
                    try
                    {
                        foreach (FieldInfo info1 in list1)
                        {
                            this.listBox.Items.Add(info1.Name, this.IsOptionEnabled(info1.Name) ? CheckState.Checked : CheckState.Unchecked);
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                    int num1 = Math.Min(this.listBox.Items.Count, 15);
                    this.listBox.ClientSize = new Size(base.Size.Width, this.listBox.ItemHeight * num1);
                    base.ClientSize = new Size(base.Size.Width, this.listBox.ItemHeight * num1);
                }
            }
        }

        public CheckedListBox ListBox
        {
            get
            {
                return this.listBox;
            }
        }


        // Fields
        private Container components;
        private object editValue;
        private FlagEnumerationListBox listBox;
        private int lockCheckUpdate;
        private FlagEnumerationEditor mainEditor;
        private object originalValue;

        // Nested Types
        private class FieldsComparer : IComparer
        {
            // Methods
            public FieldsComparer()
            {
            }

            int IComparer.Compare(object x, object y)
            {
                FieldInfo info1 = (FieldInfo) x;
                FieldInfo info2 = (FieldInfo) y;
                if ((info1 == null) || (info2 == null))
                {
                    return Comparer.Default.Compare(x, y);
                }
                if (info1 == info2)
                {
                    return 0;
                }
                if (info1.Name == EditConsts.EmptyOption)
                {
                    return -1;
                }
                if (info2.Name == EditConsts.EmptyOption)
                {
                    return 1;
                }
                return Comparer.Default.Compare(info1.Name, info2.Name);
            }

        }
    }
}

