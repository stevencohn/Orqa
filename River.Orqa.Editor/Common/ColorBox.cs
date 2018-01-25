namespace River.Orqa.Editor.Common
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ColorBox), "Images.ColorBox.bmp")]
    public class ColorBox : ComboBox
    {
        // Methods
        public ColorBox()
        {
            this.components = null;
            this.InternalCreate();
            this.InitializeComponent();
        }

        public ColorBox(IContainer container)
        {
            this.components = null;
            container.Add(this);
            this.InternalCreate();
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

        private Color GetColor(int index)
        {
            if ((index > 0) && (index < this.Items.Count))
            {
                return Color.FromName(this.Items[index].ToString());
            }
            return Color.Empty;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private void InternalCreate()
        {
            this.fontBrush = new SolidBrush(this.ForeColor);
            base.DropDownStyle = ComboBoxStyle.DropDown;
            base.DrawMode = DrawMode.OwnerDrawFixed;
            this.UpdateColors();
            this.SelectedIndex = 0;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.Index >= 0) && (e.Index < this.Items.Count))
            {
                e.DrawBackground();
                bool flag1 = (e.State & DrawItemState.Disabled) != DrawItemState.None;
                bool flag2 = ((e.State & DrawItemState.Focus) != DrawItemState.None) && ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None);
                bool flag3 = (e.State & DrawItemState.Selected) != DrawItemState.None;
                if (flag2)
                {
                    e.DrawFocusRectangle();
                }
                Brush brush1 = new SolidBrush(this.GetColor(e.Index));
                try
                {
                    string text1 = (e.Index == 0) ? Consts.EmptyColor : this.GetColor(e.Index).Name;
                    Rectangle rectangle1 = new Rectangle(e.Bounds.Left, e.Bounds.Top, Math.Min(e.Bounds.Width, 0x1c), e.Bounds.Height);
                    rectangle1.Inflate(-2, -2);
                    Rectangle rectangle2 = new Rectangle(rectangle1.Right + 1, e.Bounds.Top, (e.Bounds.Width - rectangle1.Right) - 1, e.Bounds.Height);
                    if (e.State == DrawItemState.HotLight)
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    }
                    e.Graphics.FillRectangle(brush1, rectangle1);
                    e.Graphics.DrawRectangle(Pens.Black, rectangle1);
                    Brush brush2 = this.fontBrush;
                    if (flag1)
                    {
                        brush2 = Brushes.Gray;
                    }
                    else if (flag3)
                    {
                        brush2 = SystemBrushes.HighlightText;
                    }
                    e.Graphics.DrawString(text1, this.Font, brush2, (RectangleF) rectangle2);
                }
                finally
                {
                    brush1.Dispose();
                }
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.fontBrush = new SolidBrush(this.ForeColor);
        }

        private void UpdateColors()
        {
            this.Items.Add(Consts.EmptyColor);
            System.Type type1 = typeof(Color);
            PropertyInfo[] infoArray1 = type1.GetProperties();
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                if (infoArray1[num1].PropertyType == typeof(Color))
                {
                    this.Items.Add(infoArray1[num1].Name);
                }
            }
            type1 = typeof(SystemColors);
            infoArray1 = type1.GetProperties();
            for (int num2 = 0; num2 < infoArray1.Length; num2++)
            {
                if (infoArray1[num2].PropertyType == typeof(Color))
                {
                    this.Items.Add(infoArray1[num2].Name);
                }
            }
        }


        // Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		// SMC: added new
		public new ComboBox.ObjectCollection Items
        {
            get
            {
                return base.Items;
            }
        }

        [Category("Appearence")]
        public Color SelectedColor
        {
            get
            {
                if (base.SelectedItem == null)
                {
                    return Color.Empty;
                }
                return this.GetColor(this.SelectedIndex);
            }
            set
            {
                if (value == Color.Empty)
                {
                    this.SelectedIndex = 0;
                }
                else
                {
                    for (int num1 = 0; num1 < this.Items.Count; num1++)
                    {
                        if (string.Compare(value.Name, this.Items[num1].ToString(), true) == 0)
                        {
                            this.SelectedIndex = num1;
                            return;
                        }
                    }
                }
            }
        }

		// SMC: added new
        public new string Text
        {
            get
            {
                return base.Text;
            }
        }


        // Fields
        private const int colorWidth = 0x1c;
        private Container components;
        private Brush fontBrush;
    }
}

