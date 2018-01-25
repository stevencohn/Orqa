namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class DlgSyntaxSettings : Form
    {
        // Methods
        public DlgSyntaxSettings()
        {
            this.InitializeComponent();
            this.syntaxSettings = new River.Orqa.Editor.Dialogs.SyntaxSettings();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            this.SettingsFromControl();
        }

        private void cbBackColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curBkColor = this.cbBackColor.SelectedColor;
                this.StyleFromControl();
            }
        }

        private void cbForeColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curForeColor = this.cbForeColor.SelectedColor;
                this.StyleFromControl();
            }
        }

        private void ControlsFromSettings()
        {
            this.FillStyles();
            this.cbFontName.Items.AddRange(this.GetFonts());
            this.chbDragAndDrop.Checked = (this.syntaxSettings.SelectionOptions & SelectionOptions.DisableDragging) == SelectionOptions.None;
            switch (this.syntaxSettings.ScrollBars)
            {
                case RichTextBoxScrollBars.None:
                {
                    this.chbVertScrollBar.Checked = false;
                    this.chbHorzScrollBar.Checked = false;
                    break;
                }
                case RichTextBoxScrollBars.Horizontal:
                case RichTextBoxScrollBars.ForcedHorizontal:
                {
                    this.chbVertScrollBar.Checked = false;
                    this.chbHorzScrollBar.Checked = true;
                    break;
                }
                case RichTextBoxScrollBars.Vertical:
                case RichTextBoxScrollBars.ForcedVertical:
                {
                    this.chbVertScrollBar.Checked = true;
                    this.chbHorzScrollBar.Checked = false;
                    break;
                }
                case RichTextBoxScrollBars.Both:
                case RichTextBoxScrollBars.ForcedBoth:
                {
                    this.chbVertScrollBar.Checked = true;
                    this.chbHorzScrollBar.Checked = true;
                    break;
                }
            }
            this.chbShowMargin.Checked = this.syntaxSettings.ShowMargin;
            this.chbWordWrap.Checked = this.syntaxSettings.WordWrap;
            this.chbLineNumbers.Checked = (this.syntaxSettings.GutterOptions & GutterOptions.PaintLineNumbers) != GutterOptions.None;
            this.chbLineNumbersOnGutter.Checked = (this.syntaxSettings.GutterOptions & GutterOptions.PaintLinesOnGutter) != GutterOptions.None;
            this.chbShowGutter.Checked = this.syntaxSettings.ShowGutter;
            this.tbGutterWidth.Text = this.syntaxSettings.GutterWidth.ToString();
            this.tbMarginPosition.Text = this.syntaxSettings.MarginPos.ToString();
            this.chbBeyondEol.Checked = (this.syntaxSettings.NavigateOptions & NavigateOptions.BeyondEol) != NavigateOptions.None;
            this.chbBeyondEof.Checked = (this.syntaxSettings.NavigateOptions & NavigateOptions.BeyondEof) != NavigateOptions.None;
            this.chbMoveOnRightButton.Checked = (this.syntaxSettings.NavigateOptions & NavigateOptions.MoveOnRightButton) != NavigateOptions.None;
            this.chbHighlightUrls.Checked = this.syntaxSettings.HighlightUrls;
            this.chbAllowOutlining.Checked = this.syntaxSettings.AllowOutlining;
            this.chbShowHints.Checked = (this.syntaxSettings.OutlineOptions & OutlineOptions.ShowHints) != OutlineOptions.None;
            this.rbInsertSpaces.Checked = this.syntaxSettings.UseSpaces;
            this.rbKeepTabs.Checked = !this.syntaxSettings.UseSpaces;
            string[] textArray1 = new string[this.syntaxSettings.TabStops.Length];
            for (int num1 = 0; num1 < this.syntaxSettings.TabStops.Length; num1++)
            {
                textArray1[num1] = this.syntaxSettings.TabStops[num1].ToString();
            }
            this.tbTabStops.Text = string.Join(",", textArray1);
            this.cbFontName.SelectedIndex = this.cbFontName.Items.IndexOf(this.syntaxSettings.Font.Name);
            this.tbFontSize.Text = this.syntaxSettings.Font.Size.ToString();
        }

        private void DescriptionChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curDesc = this.tbDescription.Text;
                this.StyleFromControl();
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

        private void DlgSyntaxSettings_Activated(object sender, EventArgs e)
        {
            this.tvProperties.Focus();
        }

        private void DlgSyntaxSettings_Load(object sender, EventArgs e)
        {
            this.ControlsFromSettings();
            this.lbStyles.SelectedIndexChanged += new EventHandler(this.OnStyleSelected);
            this.lbStyles.SelectedIndex = 0;
            this.chbBold.CheckedChanged += new EventHandler(this.FontStyleChange);
            this.chbItalic.CheckedChanged += new EventHandler(this.FontStyleChange);
            this.chbUnderline.CheckedChanged += new EventHandler(this.FontStyleChange);
            this.tbDescription.TextChanged += new EventHandler(this.DescriptionChanged);
            this.cbFontName.SelectedIndexChanged += new EventHandler(this.FontNameChanged);
            this.tbFontSize.TextChanged += new EventHandler(this.FontSizeChanged);
            this.pnManage.Controls.Add(this.pnGeneral);
            this.pnManage.Controls.Add(this.pnFontsColors);
            this.pnManage.Controls.Add(this.pnAdditional);
            this.tvProperties.Nodes[0].ImageIndex = 0;
            this.tvProperties.Nodes[0].Expand();
        }

        private void FillStyles()
        {
            for (int num1 = 0; num1 < this.syntaxSettings.LexStyles.Length; num1++)
            {
                this.lbStyles.Items.Add(this.syntaxSettings.LexStyles[num1].Name);
            }
        }

        private void FontNameChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.laSampleText.Font = new Font(this.cbFontName.SelectedItem.ToString(), this.laSampleText.Font.Size, this.curFontStyle);
                this.WriteSampleText();
            }
        }

        private void FontSizeChanged(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.laSampleText.Font = new Font(this.laSampleText.Font.Name, (float) this.GetInt(this.tbFontSize.Text, 10), this.curFontStyle);
                this.WriteSampleText();
            }
        }

        private void FontStyleChange(object sender, EventArgs e)
        {
            if (!this.isControlUpdating)
            {
                this.curFontStyle = FontStyle.Regular;
                if (this.chbBold.Checked)
                {
                    this.curFontStyle |= FontStyle.Bold;
                }
                else
                {
                    this.curFontStyle &= ((FontStyle) (-2));
                }
                if (this.chbItalic.Checked)
                {
                    this.curFontStyle |= FontStyle.Italic;
                }
                else
                {
                    this.curFontStyle &= ((FontStyle) (-3));
                }
                if (this.chbUnderline.Checked)
                {
                    this.curFontStyle |= FontStyle.Underline;
                }
                else
                {
                    this.curFontStyle &= ((FontStyle) (-5));
                }
                this.StyleFromControl();
            }
        }

        private Panel GetCurrentPanel()
        {
            Panel panel1 = null;
            TreeNode node1 = this.tvProperties.SelectedNode;
            if (node1 != null)
            {
                switch (this.GetNodeLevel(node1))
                {
                    case 0:
                    {
                        panel1 = this.pnGeneral;
                        break;
                    }
                    case 1:
                    {
                        switch (node1.Index)
                        {
                            case 0:
                            {
                                panel1 = this.pnGeneral;
                                break;
                            }
                            case 1:
                            {
                                panel1 = this.pnFontsColors;
                                break;
                            }
                            case 2:
                            {
                                panel1 = this.pnAdditional;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (panel1 != null)
            {
                panel1.Location = new Point(0, 0);
                panel1.Size = this.pnManage.Size;
                panel1.Dock = DockStyle.Fill;
            }
            return panel1;
        }

        private string[] GetFonts()
        {
            FontFamily[] familyArray1 = FontFamily.Families;
            string[] textArray1 = new string[familyArray1.Length];
            for (int num1 = 0; num1 < familyArray1.Length; num1++)
            {
                textArray1[num1] = familyArray1[num1].Name;
            }
            return textArray1;
        }

        private int GetInt(string s, int DefaultValue)
        {
            int num1;
            try
            {
                num1 = int.Parse(s);
            }
            catch
            {
                num1 = DefaultValue;
            }
            return num1;
        }

        private int GetNodeLevel(TreeNode node)
        {
            int num1 = 0;
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                    num1++;
                }
            }
            return num1;
        }

        private ILexStyle GetSelectedStyle()
        {
            if (this.lbStyles.SelectedItem != null)
            {
                return this.syntaxSettings.LexStyles[this.lbStyles.SelectedIndex];
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager1 = new ResourceManager(typeof(DlgSyntaxSettings));
            this.pnButtons = new Panel();
            this.btCancel = new Button();
            this.btOK = new Button();
            this.pnMain = new Panel();
            this.pnManage = new Panel();
            this.tcMain = new TabControl();
            this.tpGeneral = new TabPage();
            this.pnGeneral = new Panel();
            this.gbLineNumbers = new GroupBox();
            this.chbLineNumbers = new CheckBox();
            this.chbLineNumbersOnGutter = new CheckBox();
            this.gbGutterMargin = new GroupBox();
            this.chbShowMargin = new CheckBox();
            this.laGutterWidth = new Label();
            this.chbShowGutter = new CheckBox();
            this.laMarginPosition = new Label();
            this.gbDocument = new GroupBox();
            this.chbDragAndDrop = new CheckBox();
            this.chbHorzScrollBar = new CheckBox();
            this.chbVertScrollBar = new CheckBox();
            this.chbWordWrap = new CheckBox();
            this.chbHighlightUrls = new CheckBox();
            this.tpFontsAndColors = new TabPage();
            this.pnFontsColors = new Panel();
            this.cbBackColor = new ColorBox(this.components);
            this.cbForeColor = new ColorBox(this.components);
            this.laDisplayItems = new Label();
            this.pnSampleText = new Panel();
            this.laSampleText = new Label();
            this.laSample = new Label();
            this.laSize = new Label();
            this.laFont = new Label();
            this.cbFontName = new ComboBox();
            this.laDescription = new Label();
            this.tbDescription = new TextBox();
            this.laBackColor = new Label();
            this.laForeColor = new Label();
            this.lbStyles = new ListBox();
            this.gbFontAttributes = new GroupBox();
            this.chbUnderline = new CheckBox();
            this.chbItalic = new CheckBox();
            this.chbBold = new CheckBox();
            this.tpAdditional = new TabPage();
            this.pnAdditional = new Panel();
            this.gbTabOptions = new GroupBox();
            this.tbTabStops = new TextBox();
            this.rbKeepTabs = new RadioButton();
            this.rbInsertSpaces = new RadioButton();
            this.laTabSizes = new Label();
            this.gbOutlineOptions = new GroupBox();
            this.chbAllowOutlining = new CheckBox();
            this.chbShowHints = new CheckBox();
            this.gbNavigateOptions = new GroupBox();
            this.chbMoveOnRightButton = new CheckBox();
            this.chbBeyondEof = new CheckBox();
            this.chbBeyondEol = new CheckBox();
            this.pnTree = new Panel();
            this.tvProperties = new TreeView();
            this.imageList1 = new ImageList(this.components);
            this.colorDialog1 = new ColorDialog();
            this.tbGutterWidth = new TextBox();
            this.tbMarginPosition = new TextBox();
            this.tbFontSize = new TextBox();
            this.pnButtons.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.pnManage.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.pnGeneral.SuspendLayout();
            this.gbLineNumbers.SuspendLayout();
            this.gbGutterMargin.SuspendLayout();
            this.gbDocument.SuspendLayout();
            this.tpFontsAndColors.SuspendLayout();
            this.pnFontsColors.SuspendLayout();
            this.pnSampleText.SuspendLayout();
            this.gbFontAttributes.SuspendLayout();
            this.tpAdditional.SuspendLayout();
            this.pnAdditional.SuspendLayout();
            this.gbTabOptions.SuspendLayout();
            this.gbOutlineOptions.SuspendLayout();
            this.gbNavigateOptions.SuspendLayout();
            this.pnTree.SuspendLayout();
            base.SuspendLayout();
            this.pnButtons.Anchor = AnchorStyles.Right | (AnchorStyles.Left | AnchorStyles.Bottom);
            this.pnButtons.BorderStyle = BorderStyle.Fixed3D;
            this.pnButtons.Controls.Add(this.btCancel);
            this.pnButtons.Controls.Add(this.btOK);
            this.pnButtons.Location = new Point(0, 290);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new Size(0x204, 40);
            this.pnButtons.TabIndex = 6;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.FlatStyle = FlatStyle.System;
            this.btCancel.Location = new Point(0x1a8, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btOK.DialogResult = DialogResult.OK;
            this.btOK.FlatStyle = FlatStyle.System;
            this.btOK.Location = new Point(0x158, 8);
            this.btOK.Name = "btOK";
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.Click += new EventHandler(this.btOK_Click);
            this.pnMain.Anchor = AnchorStyles.Right | (AnchorStyles.Left | (AnchorStyles.Bottom | AnchorStyles.Top));
            this.pnMain.Controls.Add(this.pnManage);
            this.pnMain.Controls.Add(this.pnTree);
            this.pnMain.Location = new Point(0, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new Size(0x200, 290);
            this.pnMain.TabIndex = 7;
            this.pnManage.Anchor = AnchorStyles.Right | (AnchorStyles.Left | (AnchorStyles.Bottom | AnchorStyles.Top));
            this.pnManage.Controls.Add(this.tcMain);
            this.pnManage.Location = new Point(0x88, 0);
            this.pnManage.Name = "pnManage";
            this.pnManage.Size = new Size(0x178, 290);
            this.pnManage.TabIndex = 1;
            this.tcMain.Controls.Add(this.tpGeneral);
            this.tcMain.Controls.Add(this.tpFontsAndColors);
            this.tcMain.Controls.Add(this.tpAdditional);
            this.tcMain.Dock = DockStyle.Fill;
            this.tcMain.Location = new Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new Size(0x178, 290);
            this.tcMain.TabIndex = 0;
            this.tcMain.Visible = false;
            this.tpGeneral.Controls.Add(this.pnGeneral);
            this.tpGeneral.Location = new Point(4, 0x16);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Size = new Size(0x170, 0x108);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.pnGeneral.Anchor = AnchorStyles.Right | (AnchorStyles.Left | (AnchorStyles.Bottom | AnchorStyles.Top));
            this.pnGeneral.BackColor = SystemColors.Control;
            this.pnGeneral.Controls.Add(this.gbLineNumbers);
            this.pnGeneral.Controls.Add(this.gbGutterMargin);
            this.pnGeneral.Controls.Add(this.gbDocument);
            this.pnGeneral.Location = new Point(0, 0);
            this.pnGeneral.Name = "pnGeneral";
            this.pnGeneral.Size = new Size(0x180, 0x12a);
            this.pnGeneral.TabIndex = 1;
            this.pnGeneral.Visible = false;
            this.gbLineNumbers.Controls.Add(this.chbLineNumbers);
            this.gbLineNumbers.Controls.Add(this.chbLineNumbersOnGutter);
            this.gbLineNumbers.FlatStyle = FlatStyle.System;
            this.gbLineNumbers.Location = new Point(8, 200);
            this.gbLineNumbers.Name = "gbLineNumbers";
            this.gbLineNumbers.Size = new Size(360, 0x48);
            this.gbLineNumbers.TabIndex = 0x27;
            this.gbLineNumbers.TabStop = false;
            this.gbLineNumbers.Text = "Line Numbers";
            this.chbLineNumbers.FlatStyle = FlatStyle.System;
            this.chbLineNumbers.Location = new Point(8, 0x10);
            this.chbLineNumbers.Name = "chbLineNumbers";
            this.chbLineNumbers.Size = new Size(0x80, 0x18);
            this.chbLineNumbers.TabIndex = 0;
            this.chbLineNumbers.Text = "Show Line Numbers";
            this.chbLineNumbersOnGutter.FlatStyle = FlatStyle.System;
            this.chbLineNumbersOnGutter.Location = new Point(8, 40);
            this.chbLineNumbersOnGutter.Name = "chbLineNumbersOnGutter";
            this.chbLineNumbersOnGutter.Size = new Size(0x70, 0x18);
            this.chbLineNumbersOnGutter.TabIndex = 1;
            this.chbLineNumbersOnGutter.Text = "Display on Gutter";
            this.gbGutterMargin.Controls.Add(this.tbMarginPosition);
            this.gbGutterMargin.Controls.Add(this.tbGutterWidth);
            this.gbGutterMargin.Controls.Add(this.chbShowMargin);
            this.gbGutterMargin.Controls.Add(this.laGutterWidth);
            this.gbGutterMargin.Controls.Add(this.chbShowGutter);
            this.gbGutterMargin.Controls.Add(this.laMarginPosition);
            this.gbGutterMargin.FlatStyle = FlatStyle.System;
            this.gbGutterMargin.Location = new Point(8, 120);
            this.gbGutterMargin.Name = "gbGutterMargin";
            this.gbGutterMargin.Size = new Size(360, 0x48);
            this.gbGutterMargin.TabIndex = 0x26;
            this.gbGutterMargin.TabStop = false;
            this.gbGutterMargin.Text = "Gutter&&Margin";
            this.chbShowMargin.FlatStyle = FlatStyle.System;
            this.chbShowMargin.Location = new Point(8, 0x26);
            this.chbShowMargin.Name = "chbShowMargin";
            this.chbShowMargin.TabIndex = 1;
            this.chbShowMargin.Text = "Show Margin";
            this.laGutterWidth.AutoSize = true;
            this.laGutterWidth.FlatStyle = FlatStyle.System;
            this.laGutterWidth.Location = new Point(120, 0x13);
            this.laGutterWidth.Name = "laGutterWidth";
            this.laGutterWidth.Size = new Size(0x44, 0x10);
            this.laGutterWidth.TabIndex = 3;
            this.laGutterWidth.Text = "Gutter width:";
            this.chbShowGutter.FlatStyle = FlatStyle.System;
            this.chbShowGutter.Location = new Point(8, 0x10);
            this.chbShowGutter.Name = "chbShowGutter";
            this.chbShowGutter.Size = new Size(0x68, 0x17);
            this.chbShowGutter.TabIndex = 0;
            this.chbShowGutter.Text = "Show Gutter";
            this.laMarginPosition.AutoSize = true;
            this.laMarginPosition.FlatStyle = FlatStyle.System;
            this.laMarginPosition.Location = new Point(120, 0x2a);
            this.laMarginPosition.Name = "laMarginPosition";
            this.laMarginPosition.Size = new Size(0x54, 0x10);
            this.laMarginPosition.TabIndex = 4;
            this.laMarginPosition.Text = "Margin position:";
            this.gbDocument.Controls.Add(this.chbDragAndDrop);
            this.gbDocument.Controls.Add(this.chbHorzScrollBar);
            this.gbDocument.Controls.Add(this.chbVertScrollBar);
            this.gbDocument.Controls.Add(this.chbWordWrap);
            this.gbDocument.Controls.Add(this.chbHighlightUrls);
            this.gbDocument.FlatStyle = FlatStyle.System;
            this.gbDocument.Location = new Point(8, 8);
            this.gbDocument.Name = "gbDocument";
            this.gbDocument.Size = new Size(360, 0x60);
            this.gbDocument.TabIndex = 11;
            this.gbDocument.TabStop = false;
            this.gbDocument.Text = "Document";
            this.chbDragAndDrop.FlatStyle = FlatStyle.System;
            this.chbDragAndDrop.Location = new Point(8, 60);
            this.chbDragAndDrop.Name = "chbDragAndDrop";
            this.chbDragAndDrop.Size = new Size(0x80, 0x18);
            this.chbDragAndDrop.TabIndex = 2;
            this.chbDragAndDrop.Text = "&Drag and drop text";
            this.chbHorzScrollBar.FlatStyle = FlatStyle.System;
            this.chbHorzScrollBar.Location = new Point(0xd8, 0x26);
            this.chbHorzScrollBar.Name = "chbHorzScrollBar";
            this.chbHorzScrollBar.Size = new Size(0x88, 0x18);
            this.chbHorzScrollBar.TabIndex = 4;
            this.chbHorzScrollBar.Text = "&Horizontal scroll bar";
            this.chbVertScrollBar.FlatStyle = FlatStyle.System;
            this.chbVertScrollBar.Location = new Point(0xd8, 0x10);
            this.chbVertScrollBar.Name = "chbVertScrollBar";
            this.chbVertScrollBar.Size = new Size(0x88, 0x18);
            this.chbVertScrollBar.TabIndex = 3;
            this.chbVertScrollBar.Text = "&Vertical scroll bar";
            this.chbWordWrap.FlatStyle = FlatStyle.System;
            this.chbWordWrap.Location = new Point(8, 0x10);
            this.chbWordWrap.Name = "chbWordWrap";
            this.chbWordWrap.Size = new Size(0x80, 0x17);
            this.chbWordWrap.TabIndex = 0;
            this.chbWordWrap.Text = "Word Wrap";
            this.chbHighlightUrls.FlatStyle = FlatStyle.System;
            this.chbHighlightUrls.Location = new Point(8, 0x26);
            this.chbHighlightUrls.Name = "chbHighlightUrls";
            this.chbHighlightUrls.Size = new Size(0x80, 0x18);
            this.chbHighlightUrls.TabIndex = 1;
            this.chbHighlightUrls.Text = "Highlight Urls";
            this.tpFontsAndColors.Controls.Add(this.pnFontsColors);
            this.tpFontsAndColors.Location = new Point(4, 0x16);
            this.tpFontsAndColors.Name = "tpFontsAndColors";
            this.tpFontsAndColors.Size = new Size(0x170, 0x108);
            this.tpFontsAndColors.TabIndex = 1;
            this.tpFontsAndColors.Text = "Fonts&&Colors";
            this.pnFontsColors.Controls.Add(this.tbFontSize);
            this.pnFontsColors.Controls.Add(this.cbBackColor);
            this.pnFontsColors.Controls.Add(this.cbForeColor);
            this.pnFontsColors.Controls.Add(this.laDisplayItems);
            this.pnFontsColors.Controls.Add(this.pnSampleText);
            this.pnFontsColors.Controls.Add(this.laSample);
            this.pnFontsColors.Controls.Add(this.laSize);
            this.pnFontsColors.Controls.Add(this.laFont);
            this.pnFontsColors.Controls.Add(this.cbFontName);
            this.pnFontsColors.Controls.Add(this.laDescription);
            this.pnFontsColors.Controls.Add(this.tbDescription);
            this.pnFontsColors.Controls.Add(this.laBackColor);
            this.pnFontsColors.Controls.Add(this.laForeColor);
            this.pnFontsColors.Controls.Add(this.lbStyles);
            this.pnFontsColors.Controls.Add(this.gbFontAttributes);
            this.pnFontsColors.Location = new Point(0, 0);
            this.pnFontsColors.Name = "pnFontsColors";
            this.pnFontsColors.Size = new Size(0x180, 0x128);
            this.pnFontsColors.TabIndex = 7;
            this.pnFontsColors.Visible = false;
            this.cbBackColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbBackColor.Location = new Point(0x98, 0xb6);
            this.cbBackColor.Name = "cbBackColor";
            this.cbBackColor.SelectedColor = Color.Empty;
            this.cbBackColor.Size = new Size(120, 0x15);
            this.cbBackColor.TabIndex = 0x12;
            this.cbBackColor.SelectedIndexChanged += new EventHandler(this.cbBackColor_SelectedIndexChanged);
            this.cbForeColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbForeColor.Location = new Point(0x98, 0x86);
            this.cbForeColor.Name = "cbForeColor";
            this.cbForeColor.SelectedColor = Color.Empty;
            this.cbForeColor.Size = new Size(120, 0x15);
            this.cbForeColor.TabIndex = 0x11;
            this.cbForeColor.SelectedIndexChanged += new EventHandler(this.cbForeColor_SelectedIndexChanged);
            this.laDisplayItems.AutoSize = true;
            this.laDisplayItems.FlatStyle = FlatStyle.System;
            this.laDisplayItems.Location = new Point(8, 0x40);
            this.laDisplayItems.Name = "laDisplayItems";
            this.laDisplayItems.Size = new Size(0x4b, 0x10);
            this.laDisplayItems.TabIndex = 4;
            this.laDisplayItems.Text = "&Display items:";
            this.pnSampleText.BorderStyle = BorderStyle.FixedSingle;
            this.pnSampleText.Controls.Add(this.laSampleText);
            this.pnSampleText.Location = new Point(8, 0xe8);
            this.pnSampleText.Name = "pnSampleText";
            this.pnSampleText.Size = new Size(360, 0x30);
            this.pnSampleText.TabIndex = 14;
            this.laSampleText.AutoSize = true;
            this.laSampleText.Location = new Point(160, 0x10);
            this.laSampleText.Name = "laSampleText";
            this.laSampleText.Size = new Size(0x3a, 0x10);
            this.laSampleText.TabIndex = 0;
            this.laSampleText.Text = "AaBbYyZz";
            this.laSample.AutoSize = true;
            this.laSample.FlatStyle = FlatStyle.System;
            this.laSample.Location = new Point(8, 0xd8);
            this.laSample.Name = "laSample";
            this.laSample.Size = new Size(0x2e, 0x10);
            this.laSample.TabIndex = 13;
            this.laSample.Text = "Sample:";
            this.laSize.AutoSize = true;
            this.laSize.FlatStyle = FlatStyle.System;
            this.laSize.Location = new Point(0x98, 0x10);
            this.laSize.Name = "laSize";
            this.laSize.Size = new Size(0x1d, 0x10);
            this.laSize.TabIndex = 2;
            this.laSize.Text = "&Size:";
            this.laFont.AutoSize = true;
            this.laFont.FlatStyle = FlatStyle.System;
            this.laFont.Location = new Point(8, 13);
            this.laFont.Name = "laFont";
            this.laFont.Size = new Size(30, 0x10);
            this.laFont.TabIndex = 0;
            this.laFont.Text = "Font:";
            this.cbFontName.Location = new Point(8, 0x20);
            this.cbFontName.Name = "cbFontName";
            this.cbFontName.Size = new Size(0x79, 0x15);
            this.cbFontName.TabIndex = 1;
            this.laDescription.AutoSize = true;
            this.laDescription.FlatStyle = FlatStyle.System;
            this.laDescription.Location = new Point(0x98, 0x40);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new Size(0x40, 0x10);
            this.laDescription.TabIndex = 6;
            this.laDescription.Text = "Description:";
            this.tbDescription.Location = new Point(0x98, 80);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new Size(0xd0, 20);
            this.tbDescription.TabIndex = 7;
			this.tbDescription.Text = String.Empty;
            this.laBackColor.AutoSize = true;
            this.laBackColor.FlatStyle = FlatStyle.System;
            this.laBackColor.Location = new Point(0x98, 160);
            this.laBackColor.Name = "laBackColor";
            this.laBackColor.Size = new Size(0x3f, 0x10);
            this.laBackColor.TabIndex = 10;
            this.laBackColor.Text = "Back Color:";
            this.laForeColor.AutoSize = true;
            this.laForeColor.FlatStyle = FlatStyle.System;
            this.laForeColor.Location = new Point(0x98, 0x70);
            this.laForeColor.Name = "laForeColor";
            this.laForeColor.Size = new Size(0x3d, 0x10);
            this.laForeColor.TabIndex = 8;
            this.laForeColor.Text = "Fore Color:";
            this.lbStyles.Location = new Point(8, 0x53);
            this.lbStyles.Name = "lbStyles";
            this.lbStyles.Size = new Size(120, 0x79);
            this.lbStyles.TabIndex = 5;
            this.gbFontAttributes.Controls.Add(this.chbUnderline);
            this.gbFontAttributes.Controls.Add(this.chbItalic);
            this.gbFontAttributes.Controls.Add(this.chbBold);
            this.gbFontAttributes.FlatStyle = FlatStyle.System;
            this.gbFontAttributes.Location = new Point(280, 0x70);
            this.gbFontAttributes.Name = "gbFontAttributes";
            this.gbFontAttributes.Size = new Size(0x58, 0x5c);
            this.gbFontAttributes.TabIndex = 12;
            this.gbFontAttributes.TabStop = false;
            this.gbFontAttributes.Text = "Attributes:";
            this.chbUnderline.FlatStyle = FlatStyle.System;
            this.chbUnderline.Location = new Point(8, 0x40);
            this.chbUnderline.Name = "chbUnderline";
            this.chbUnderline.Size = new Size(0x48, 0x18);
            this.chbUnderline.TabIndex = 2;
            this.chbUnderline.Text = "Underline";
            this.chbItalic.FlatStyle = FlatStyle.System;
            this.chbItalic.Location = new Point(8, 40);
            this.chbItalic.Name = "chbItalic";
            this.chbItalic.Size = new Size(0x48, 0x18);
            this.chbItalic.TabIndex = 1;
            this.chbItalic.Text = "Italic";
            this.chbBold.FlatStyle = FlatStyle.System;
            this.chbBold.Location = new Point(8, 0x10);
            this.chbBold.Name = "chbBold";
            this.chbBold.Size = new Size(0x48, 0x18);
            this.chbBold.TabIndex = 0;
            this.chbBold.Text = "Bold";
            this.tpAdditional.Controls.Add(this.pnAdditional);
            this.tpAdditional.Location = new Point(4, 0x16);
            this.tpAdditional.Name = "tpAdditional";
            this.tpAdditional.Size = new Size(0x170, 0x108);
            this.tpAdditional.TabIndex = 2;
            this.tpAdditional.Text = "Additional";
            this.pnAdditional.Controls.Add(this.gbTabOptions);
            this.pnAdditional.Controls.Add(this.gbOutlineOptions);
            this.pnAdditional.Controls.Add(this.gbNavigateOptions);
            this.pnAdditional.Location = new Point(0, 0);
            this.pnAdditional.Name = "pnAdditional";
            this.pnAdditional.Size = new Size(0x180, 0x128);
            this.pnAdditional.TabIndex = 8;
            this.pnAdditional.Visible = false;
            this.gbTabOptions.Controls.Add(this.tbTabStops);
            this.gbTabOptions.Controls.Add(this.rbKeepTabs);
            this.gbTabOptions.Controls.Add(this.rbInsertSpaces);
            this.gbTabOptions.Controls.Add(this.laTabSizes);
            this.gbTabOptions.FlatStyle = FlatStyle.System;
            this.gbTabOptions.Location = new Point(8, 200);
            this.gbTabOptions.Name = "gbTabOptions";
            this.gbTabOptions.Size = new Size(360, 0x48);
            this.gbTabOptions.TabIndex = 40;
            this.gbTabOptions.TabStop = false;
            this.gbTabOptions.Text = "Tab Options";
            this.tbTabStops.Location = new Point(0x48, 0x10);
            this.tbTabStops.Name = "tbTabStops";
            this.tbTabStops.Size = new Size(120, 20);
            this.tbTabStops.TabIndex = 5;
			this.tbTabStops.Text = String.Empty;
            this.rbKeepTabs.FlatStyle = FlatStyle.System;
            this.rbKeepTabs.Location = new Point(0xd8, 40);
            this.rbKeepTabs.Name = "rbKeepTabs";
            this.rbKeepTabs.TabIndex = 4;
            this.rbKeepTabs.Text = "&Keep tabs";
            this.rbInsertSpaces.FlatStyle = FlatStyle.System;
            this.rbInsertSpaces.Location = new Point(0xd8, 0x10);
            this.rbInsertSpaces.Name = "rbInsertSpaces";
            this.rbInsertSpaces.TabIndex = 3;
            this.rbInsertSpaces.Text = "Insert s&paces";
            this.laTabSizes.AutoSize = true;
            this.laTabSizes.FlatStyle = FlatStyle.System;
            this.laTabSizes.Location = new Point(8, 0x13);
            this.laTabSizes.Name = "laTabSizes";
            this.laTabSizes.Size = new Size(0x3a, 0x10);
            this.laTabSizes.TabIndex = 0;
            this.laTabSizes.Text = "Tab Sizes:";
            this.gbOutlineOptions.Controls.Add(this.chbAllowOutlining);
            this.gbOutlineOptions.Controls.Add(this.chbShowHints);
            this.gbOutlineOptions.FlatStyle = FlatStyle.System;
            this.gbOutlineOptions.Location = new Point(8, 120);
            this.gbOutlineOptions.Name = "gbOutlineOptions";
            this.gbOutlineOptions.Size = new Size(360, 0x48);
            this.gbOutlineOptions.TabIndex = 0x27;
            this.gbOutlineOptions.TabStop = false;
            this.gbOutlineOptions.Text = "Outline Options";
            this.chbAllowOutlining.FlatStyle = FlatStyle.System;
            this.chbAllowOutlining.Location = new Point(8, 0x10);
            this.chbAllowOutlining.Name = "chbAllowOutlining";
            this.chbAllowOutlining.TabIndex = 0;
            this.chbAllowOutlining.Text = "Allow outlining";
            this.chbShowHints.FlatStyle = FlatStyle.System;
            this.chbShowHints.Location = new Point(8, 0x26);
            this.chbShowHints.Name = "chbShowHints";
            this.chbShowHints.TabIndex = 1;
            this.chbShowHints.Text = "Show Hints";
            this.gbNavigateOptions.Controls.Add(this.chbMoveOnRightButton);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEof);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEol);
            this.gbNavigateOptions.FlatStyle = FlatStyle.System;
            this.gbNavigateOptions.Location = new Point(8, 0x10);
            this.gbNavigateOptions.Name = "gbNavigateOptions";
            this.gbNavigateOptions.Size = new Size(360, 0x60);
            this.gbNavigateOptions.TabIndex = 0x26;
            this.gbNavigateOptions.TabStop = false;
            this.gbNavigateOptions.Text = "Navigate Options";
            this.chbMoveOnRightButton.FlatStyle = FlatStyle.System;
            this.chbMoveOnRightButton.Location = new Point(8, 60);
            this.chbMoveOnRightButton.Name = "chbMoveOnRightButton";
            this.chbMoveOnRightButton.Size = new Size(0x88, 0x17);
            this.chbMoveOnRightButton.TabIndex = 14;
            this.chbMoveOnRightButton.Text = "Move on Right Button";
            this.chbBeyondEof.FlatStyle = FlatStyle.System;
            this.chbBeyondEof.Location = new Point(8, 0x26);
            this.chbBeyondEof.Name = "chbBeyondEof";
            this.chbBeyondEof.Size = new Size(0x88, 0x17);
            this.chbBeyondEof.TabIndex = 13;
            this.chbBeyondEof.Text = "Beyond Eof";
            this.chbBeyondEol.FlatStyle = FlatStyle.System;
            this.chbBeyondEol.Location = new Point(8, 0x10);
            this.chbBeyondEol.Name = "chbBeyondEol";
            this.chbBeyondEol.Size = new Size(0x88, 0x17);
            this.chbBeyondEol.TabIndex = 12;
            this.chbBeyondEol.Text = "Beyond Eol";
            this.pnTree.Anchor = AnchorStyles.Left | (AnchorStyles.Bottom | AnchorStyles.Top);
            this.pnTree.Controls.Add(this.tvProperties);
            this.pnTree.Location = new Point(0, 0);
            this.pnTree.Name = "pnTree";
            this.pnTree.Size = new Size(0x88, 290);
            this.pnTree.TabIndex = 0;
            this.tvProperties.Anchor = AnchorStyles.Right | (AnchorStyles.Left | (AnchorStyles.Bottom | AnchorStyles.Top));
            this.tvProperties.ImageIndex = 3;
            this.tvProperties.ImageList = this.imageList1;
            this.tvProperties.Location = new Point(0, 0);
            this.tvProperties.Name = "tvProperties";
            TreeNode[] nodeArray1 = new TreeNode[1];
            TreeNode[] nodeArray2 = new TreeNode[3] { new TreeNode("General"), new TreeNode("Fonts and Colors"), new TreeNode("Additional") } ;
            nodeArray1[0] = new TreeNode("Options", 1, 0, nodeArray2);
            this.tvProperties.Nodes.AddRange(nodeArray1);
            this.tvProperties.Scrollable = false;
            this.tvProperties.SelectedImageIndex = 2;
            this.tvProperties.ShowLines = false;
            this.tvProperties.ShowPlusMinus = false;
            this.tvProperties.ShowRootLines = false;
            this.tvProperties.Size = new Size(0x88, 290);
            this.tvProperties.TabIndex = 0;
            this.tvProperties.AfterSelect += new TreeViewEventHandler(this.tvProperties_AfterSelect);
            this.imageList1.ImageSize = new Size(0x10, 0x10);
            this.imageList1.ImageStream = (ImageListStreamer) manager1.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Red;
            this.tbGutterWidth.Location = new Point(0xd8, 0x10);
            this.tbGutterWidth.Name = "tbGutterWidth";
            this.tbGutterWidth.Size = new Size(40, 20);
            this.tbGutterWidth.TabIndex = 7;
            this.tbGutterWidth.Text = "0";
            this.tbMarginPosition.Location = new Point(0xd8, 40);
            this.tbMarginPosition.Name = "tbMarginPosition";
            this.tbMarginPosition.Size = new Size(40, 20);
            this.tbMarginPosition.TabIndex = 8;
            this.tbMarginPosition.Text = "0";
            this.tbFontSize.Location = new Point(0x98, 0x20);
            this.tbFontSize.Name = "tbFontSize";
            this.tbFontSize.Size = new Size(0x20, 20);
            this.tbFontSize.TabIndex = 0x13;
            this.tbFontSize.Text = "1";
            base.AcceptButton = this.btOK;
			base.AutoScaleMode = AutoScaleMode.None;
            this.AutoScaleDimensions = new Size(5, 13);
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(0x200, 0x148);
            base.Controls.Add(this.pnMain);
            base.Controls.Add(this.pnButtons);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "DlgSyntaxSettings";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Syntax Settings";
            base.Load += new EventHandler(this.DlgSyntaxSettings_Load);
            base.Activated += new EventHandler(this.DlgSyntaxSettings_Activated);
            this.pnButtons.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.pnManage.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.pnGeneral.ResumeLayout(false);
            this.gbLineNumbers.ResumeLayout(false);
            this.gbGutterMargin.ResumeLayout(false);
            this.gbDocument.ResumeLayout(false);
            this.tpFontsAndColors.ResumeLayout(false);
            this.pnFontsColors.ResumeLayout(false);
            this.pnSampleText.ResumeLayout(false);
            this.gbFontAttributes.ResumeLayout(false);
            this.tpAdditional.ResumeLayout(false);
            this.pnAdditional.ResumeLayout(false);
            this.gbTabOptions.ResumeLayout(false);
            this.gbOutlineOptions.ResumeLayout(false);
            this.gbNavigateOptions.ResumeLayout(false);
            this.pnTree.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnStyleSelected(object sender, EventArgs e)
        {
            this.tbDescription.Enabled = this.syntaxSettings.IsDescriptionEnabled(this.lbStyles.SelectedIndex);
            this.laDescription.Enabled = this.syntaxSettings.IsDescriptionEnabled(this.lbStyles.SelectedIndex);
            this.gbFontAttributes.Enabled = this.syntaxSettings.IsFontStyleEnabled(this.lbStyles.SelectedIndex);
            this.laBackColor.Enabled = this.syntaxSettings.IsBackColorEnabled(this.lbStyles.SelectedIndex);
            this.cbBackColor.Enabled = this.syntaxSettings.IsBackColorEnabled(this.lbStyles.SelectedIndex);
            ILexStyle style1 = this.GetSelectedStyle();
            if (style1 != null)
            {
                this.curForeColor = style1.ForeColor;
                this.curBkColor = style1.BackColor;
                this.curFontStyle = style1.FontStyle;
                this.curDesc = style1.Desc;
            }
            this.UpdateStyleControls();
        }

        private void SettingsFromControl()
        {
            bool flag1 = this.chbVertScrollBar.Checked;
            if (this.chbHorzScrollBar.Checked)
            {
                if (flag1)
                {
                    this.syntaxSettings.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
                }
                else
                {
                    this.syntaxSettings.ScrollBars = RichTextBoxScrollBars.ForcedHorizontal;
                }
            }
            else if (flag1)
            {
                this.syntaxSettings.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            }
            else
            {
                this.syntaxSettings.ScrollBars = RichTextBoxScrollBars.None;
            }
            if (this.chbDragAndDrop.Checked)
            {
                this.syntaxSettings.SelectionOptions &= ((SelectionOptions) (-3));
            }
            else
            {
                this.syntaxSettings.SelectionOptions |= SelectionOptions.DisableDragging;
            }
            this.syntaxSettings.ShowMargin = this.chbShowMargin.Checked;
            this.syntaxSettings.WordWrap = this.chbWordWrap.Checked;
            this.syntaxSettings.ShowGutter = this.chbShowGutter.Checked;
            this.syntaxSettings.GutterWidth = this.GetInt(this.tbGutterWidth.Text, EditConsts.DefaultGutterWidth);
            this.syntaxSettings.MarginPos = this.GetInt(this.tbMarginPosition.Text, EditConsts.DefaultMarginPosition);
            if (this.chbLineNumbers.Checked)
            {
                this.syntaxSettings.GutterOptions |= GutterOptions.PaintLineNumbers;
            }
            else
            {
                this.syntaxSettings.GutterOptions &= ((GutterOptions) (-2));
            }
            if (this.chbLineNumbersOnGutter.Checked)
            {
                this.syntaxSettings.GutterOptions |= GutterOptions.PaintLinesOnGutter;
            }
            else
            {
                this.syntaxSettings.GutterOptions &= ((GutterOptions) (-3));
            }
            if (this.chbBeyondEol.Checked)
            {
                this.syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEol;
            }
            else
            {
                this.syntaxSettings.NavigateOptions &= ((NavigateOptions) (-2));
            }
            if (this.chbBeyondEof.Checked)
            {
                this.syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEof;
            }
            else
            {
                this.syntaxSettings.NavigateOptions &= ((NavigateOptions) (-3));
            }
            if (this.chbMoveOnRightButton.Checked)
            {
                this.syntaxSettings.NavigateOptions |= NavigateOptions.MoveOnRightButton;
            }
            else
            {
                this.syntaxSettings.NavigateOptions &= ((NavigateOptions) (-17));
            }
            if (this.chbShowHints.Checked)
            {
                this.syntaxSettings.OutlineOptions |= OutlineOptions.ShowHints;
            }
            else
            {
                this.syntaxSettings.OutlineOptions &= ((OutlineOptions) (-9));
            }
            this.syntaxSettings.HighlightUrls = this.chbHighlightUrls.Checked;
            this.syntaxSettings.AllowOutlining = this.chbAllowOutlining.Checked;
            this.syntaxSettings.UseSpaces = this.rbInsertSpaces.Checked;
            char[] chArray1 = new char[1] { ',' } ;
            string[] textArray1 = this.tbTabStops.Text.Split(chArray1);
            int[] numArray1 = new int[textArray1.Length];
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                numArray1[num1] = this.GetInt(textArray1[num1], EditConsts.DefaultTabStop);
            }
            this.syntaxSettings.TabStops = numArray1;
            this.syntaxSettings.Font = new Font(this.cbFontName.SelectedItem.ToString(), (float) this.GetInt(this.tbFontSize.Text, 10));
        }

        private void StyleFromControl()
        {
            if (this.lbStyles.SelectedItem != null)
            {
                ILexStyle style1 = this.GetSelectedStyle();
                style1.ForeColor = this.curForeColor;
                style1.BackColor = this.curBkColor;
                style1.FontStyle = this.curFontStyle;
                style1.Desc = this.curDesc;
                this.UpdateStyleControls();
            }
        }

        private void tvProperties_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Panel panel1 = this.GetCurrentPanel();
            panel1.Visible = true;
            panel1.BringToFront();
        }

        private void UpdateStyleControls()
        {
            this.isControlUpdating = true;
            try
            {
                this.chbBold.Checked = (this.curFontStyle & FontStyle.Bold) != FontStyle.Regular;
                this.chbItalic.Checked = (this.curFontStyle & FontStyle.Italic) != FontStyle.Regular;
                this.chbUnderline.Checked = (this.curFontStyle & FontStyle.Underline) != FontStyle.Regular;
                this.tbDescription.Text = this.curDesc;
                this.laSampleText.Font = new Font(this.laSampleText.Font.Name, (float) ((int) this.laSampleText.Font.Size), this.curFontStyle);
                this.cbForeColor.SelectedColor = this.curForeColor;
                this.cbBackColor.SelectedColor = this.curBkColor;
                this.laSampleText.ForeColor = this.curForeColor;
                this.pnSampleText.BackColor = this.curBkColor;
                this.WriteSampleText();
            }
            finally
            {
                this.isControlUpdating = false;
            }
        }

        private void WriteSampleText()
        {
            this.laSampleText.Location = new Point((this.pnSampleText.Width - this.laSampleText.Width) / 2, (this.pnSampleText.Height - this.laSampleText.Height) / 2);
        }


        // Properties
        public ISyntaxSettings SyntaxSettings
        {
            get
            {
                return this.syntaxSettings;
            }
            set
            {
                this.syntaxSettings.Assign(value);
            }
        }


        // Fields
        public Button btCancel;
        public Button btOK;
        public ColorBox cbBackColor;
        public ComboBox cbFontName;
        public ColorBox cbForeColor;
        public CheckBox chbAllowOutlining;
        public CheckBox chbBeyondEof;
        public CheckBox chbBeyondEol;
        public CheckBox chbBold;
        public CheckBox chbDragAndDrop;
        public CheckBox chbHighlightUrls;
        public CheckBox chbHorzScrollBar;
        public CheckBox chbItalic;
        public CheckBox chbLineNumbers;
        public CheckBox chbLineNumbersOnGutter;
        public CheckBox chbMoveOnRightButton;
        public CheckBox chbShowGutter;
        public CheckBox chbShowHints;
        public CheckBox chbShowMargin;
        public CheckBox chbUnderline;
        public CheckBox chbVertScrollBar;
        public CheckBox chbWordWrap;
        private const int closeFolderImage = 1;
        public ColorDialog colorDialog1;
        public IContainer components;
        private Color curBkColor;
        private string curDesc;
        private FontStyle curFontStyle;
        private Color curForeColor;
        public GroupBox gbDocument;
        public GroupBox gbFontAttributes;
        public GroupBox gbGutterMargin;
        public GroupBox gbLineNumbers;
        public GroupBox gbNavigateOptions;
        public GroupBox gbOutlineOptions;
        public GroupBox gbTabOptions;
        public ImageList imageList1;
        private bool isControlUpdating;
        public Label laBackColor;
        public Label laDescription;
        public Label laDisplayItems;
        public Label laFont;
        public Label laForeColor;
        public Label laGutterWidth;
        public Label laMarginPosition;
        public Label laSample;
        public Label laSampleText;
        public Label laSize;
        public Label laTabSizes;
        public ListBox lbStyles;
        private const int openFolderImage = 0;
        public Panel pnAdditional;
        public Panel pnButtons;
        public Panel pnFontsColors;
        public Panel pnGeneral;
        public Panel pnMain;
        public Panel pnManage;
        public Panel pnSampleText;
        public Panel pnTree;
        public RadioButton rbInsertSpaces;
        public RadioButton rbKeepTabs;
        private const int selectedImage = 2;
        private ISyntaxSettings syntaxSettings;
        public TextBox tbDescription;
        public TextBox tbFontSize;
        public TextBox tbGutterWidth;
        public TextBox tbMarginPosition;
        public TextBox tbTabStops;
        public TabControl tcMain;
        public TabPage tpAdditional;
        public TabPage tpFontsAndColors;
        public TabPage tpGeneral;
        public TreeView tvProperties;
        private const int unSelectedImage = 3;
    }
}

