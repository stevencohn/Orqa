namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class DlgSearch : Form
    {
        // Methods
        public DlgSearch()
        {
            this.firstSearch = true;
            this.saveText = string.Empty;
            this.InitializeComponent();
            this.ReplaceChanged();

			this.chbSearchHiddenText.Checked = true;
        }

        private void AddToHistory()
        {
            this.AddToHistory(this.cbFindWhat.Items, this.cbFindWhat.Text);
            if (this.isReplace)
            {
                this.AddToHistory(this.cbReplaceWith.Items, this.cbReplaceWith.Text);
            }
        }

        private void AddToHistory(IList List, string s)
        {
            if (s != string.Empty)
            {
                for (int num1 = List.IndexOf(s); num1 >= 0; num1 = List.IndexOf(s))
                {
                    List.RemoveAt(num1);
                }
                List.Insert(0, s);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btFindNext_Click(object sender, EventArgs e)
        {
            this.AddToHistory();
            if (this.search != null)
            {
                bool flag1;
                SearchOptions options1 = this.Options;
                if ((!this.search.FirstSearch && !this.firstSearch) && ((this.cbFindWhat.Text == this.saveText) && (options1 == this.saveOptions)))
                {
                    if ((options1 & SearchOptions.BackwardSearch) != SearchOptions.None)
                    {
                        flag1 = this.search.FindPrevious();
                    }
                    else
                    {
                        flag1 = this.search.FindNext();
                    }
                }
                else
                {
                    flag1 = this.search.Find(this.cbFindWhat.Text, options1, this.GetExpression());
                }
                if (flag1)
                {
                    this.TextFound();
                }
                else
                {
                    this.ShowNotFound(this.cbFindWhat.Text);
                }
            }
        }

        private void btMarkAll_Click(object sender, EventArgs e)
        {
            if (this.search != null)
            {
                this.search.MarkAll(this.cbFindWhat.Text, this.Options, this.GetExpression());
            }
        }

        private void btPopup_Click(object sender, EventArgs e)
        {
            this.cmFind.Show((Control) sender, new Point(0, 0));
        }

        private void btReplace_Click(object sender, EventArgs e)
        {
            if (!this.isReplace)
            {
                this.IsReplace = true;
            }
            else
            {
                this.AddToHistory();
                if ((this.search != null) && !this.search.Replace(this.cbFindWhat.Text, this.cbReplaceWith.Text, this.Options, this.GetExpression()))
                {
                    this.ShowNotFound(this.cbFindWhat.Text);
                }
            }
        }

        private void btReplaceAll_Click(object sender, EventArgs e)
        {
            this.AddToHistory();
            if ((this.search != null) && (this.search.ReplaceAll(this.cbFindWhat.Text, this.cbReplaceWith.Text, this.Options, this.GetExpression()) == 0))
            {
                this.ShowNotFound(this.cbFindWhat.Text);
            }
        }

        private void chbUseRegularExpressions_Click(object sender, EventArgs e)
        {
            this.btPopup.Enabled = this.chbUseRegularExpressions.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Regex GetExpression()
        {
            Regex regex1 = null;
            if (this.chbUseRegularExpressions.Checked)
            {
                try
                {
                    regex1 = new Regex(this.cbFindWhat.Text, this.GetRegexOptions());
                }
                catch (Exception exception1)
                {
                    regex1 = null;
                    ErrorHandler.Error(exception1);
                }
            }
            return regex1;
        }

        private RegexOptions GetRegexOptions()
        {
            return ((this.chbSearchUp.Checked ? RegexOptions.RightToLeft : RegexOptions.None) | (!this.chbMatchCase.Checked ? RegexOptions.IgnoreCase : RegexOptions.None));
        }

        public void Init()
        {
            this.firstSearch = true;
            this.saveOptions = SearchOptions.None;
            this.saveText = string.Empty;
        }

        private void InitializeComponent()
        {
			this.laFindWhat = new System.Windows.Forms.Label();
			this.cbFindWhat = new System.Windows.Forms.ComboBox();
			this.btPopup = new System.Windows.Forms.Button();
			this.btFindNext = new System.Windows.Forms.Button();
			this.btReplace = new System.Windows.Forms.Button();
			this.btMarkAll = new System.Windows.Forms.Button();
			this.btClose = new System.Windows.Forms.Button();
			this.btHelp = new System.Windows.Forms.Button();
			this.cbReplaceWith = new System.Windows.Forms.ComboBox();
			this.laReplaceWith = new System.Windows.Forms.Label();
			this.btReplaceAll = new System.Windows.Forms.Button();
			this.cmFind = new System.Windows.Forms.ContextMenu();
			this.miSingleChar = new System.Windows.Forms.MenuItem();
			this.miZeroOrMore = new System.Windows.Forms.MenuItem();
			this.miOneOrMore = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.miBeginLine = new System.Windows.Forms.MenuItem();
			this.miEndLine = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.miOneCharInSet = new System.Windows.Forms.MenuItem();
			this.miOneCharNotInSet = new System.Windows.Forms.MenuItem();
			this.miOr = new System.Windows.Forms.MenuItem();
			this.miEscape = new System.Windows.Forms.MenuItem();
			this.miTag = new System.Windows.Forms.MenuItem();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.chbPromptOnReplace = new System.Windows.Forms.CheckBox();
			this.gbSearch = new System.Windows.Forms.GroupBox();
			this.chbSelectionOnly = new System.Windows.Forms.CheckBox();
			this.rbEntireScope = new System.Windows.Forms.RadioButton();
			this.rbFromCursor = new System.Windows.Forms.RadioButton();
			this.chbSearchUp = new System.Windows.Forms.CheckBox();
			this.chbSearchHiddenText = new System.Windows.Forms.CheckBox();
			this.chbMatchWholeWord = new System.Windows.Forms.CheckBox();
			this.chbMatchCase = new System.Windows.Forms.CheckBox();
			this.chbUseRegularExpressions = new System.Windows.Forms.CheckBox();
			this.pnlSearch.SuspendLayout();
			this.gbSearch.SuspendLayout();
			this.SuspendLayout();
			// 
			// laFindWhat
			// 
			this.laFindWhat.AutoSize = true;
			this.laFindWhat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.laFindWhat.Location = new System.Drawing.Point(8, 12);
			this.laFindWhat.Name = "laFindWhat";
			this.laFindWhat.Size = new System.Drawing.Size(52, 13);
			this.laFindWhat.TabIndex = 0;
			this.laFindWhat.Text = "Fi&nd what:";
			// 
			// cbFindWhat
			// 
			this.cbFindWhat.FormattingEnabled = true;
			this.cbFindWhat.Location = new System.Drawing.Point(95, 8);
			this.cbFindWhat.Name = "cbFindWhat";
			this.cbFindWhat.Size = new System.Drawing.Size(282, 21);
			this.cbFindWhat.TabIndex = 1;
			// 
			// btPopup
			// 
			this.btPopup.BackColor = System.Drawing.SystemColors.Control;
			this.btPopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btPopup.ImageIndex = 0;
			this.btPopup.Location = new System.Drawing.Point(381, 8);
			this.btPopup.Name = "btPopup";
			this.btPopup.Size = new System.Drawing.Size(17, 20);
			this.btPopup.TabIndex = 2;
			this.btPopup.Text = ">";
			this.btPopup.UseVisualStyleBackColor = false;
			this.btPopup.Click += new System.EventHandler(this.btPopup_Click);
			// 
			// btFindNext
			// 
			this.btFindNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btFindNext.Location = new System.Drawing.Point(407, 8);
			this.btFindNext.Name = "btFindNext";
			this.btFindNext.Size = new System.Drawing.Size(78, 23);
			this.btFindNext.TabIndex = 12;
			this.btFindNext.Text = "&Find Next";
			this.btFindNext.Click += new System.EventHandler(this.btFindNext_Click);
			// 
			// btReplace
			// 
			this.btReplace.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btReplace.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btReplace.Location = new System.Drawing.Point(407, 34);
			this.btReplace.Name = "btReplace";
			this.btReplace.Size = new System.Drawing.Size(78, 23);
			this.btReplace.TabIndex = 13;
			this.btReplace.Text = "&Replace";
			this.btReplace.Click += new System.EventHandler(this.btReplace_Click);
			// 
			// btMarkAll
			// 
			this.btMarkAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btMarkAll.Location = new System.Drawing.Point(407, 86);
			this.btMarkAll.Name = "btMarkAll";
			this.btMarkAll.Size = new System.Drawing.Size(78, 23);
			this.btMarkAll.TabIndex = 15;
			this.btMarkAll.Text = "&Mark All";
			this.btMarkAll.Click += new System.EventHandler(this.btMarkAll_Click);
			// 
			// btClose
			// 
			this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btClose.Location = new System.Drawing.Point(407, 112);
			this.btClose.Name = "btClose";
			this.btClose.Size = new System.Drawing.Size(78, 23);
			this.btClose.TabIndex = 16;
			this.btClose.Text = "Close";
			this.btClose.Click += new System.EventHandler(this.btClose_Click);
			// 
			// btHelp
			// 
			this.btHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btHelp.Location = new System.Drawing.Point(407, 138);
			this.btHelp.Name = "btHelp";
			this.btHelp.Size = new System.Drawing.Size(78, 23);
			this.btHelp.TabIndex = 17;
			this.btHelp.Text = "Help";
			// 
			// cbReplaceWith
			// 
			this.cbReplaceWith.FormattingEnabled = true;
			this.cbReplaceWith.Location = new System.Drawing.Point(95, 32);
			this.cbReplaceWith.Name = "cbReplaceWith";
			this.cbReplaceWith.Size = new System.Drawing.Size(282, 21);
			this.cbReplaceWith.TabIndex = 3;
			// 
			// laReplaceWith
			// 
			this.laReplaceWith.AutoSize = true;
			this.laReplaceWith.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.laReplaceWith.Location = new System.Drawing.Point(8, 35);
			this.laReplaceWith.Name = "laReplaceWith";
			this.laReplaceWith.Size = new System.Drawing.Size(68, 13);
			this.laReplaceWith.TabIndex = 2;
			this.laReplaceWith.Text = "Re&place with:";
			// 
			// btReplaceAll
			// 
			this.btReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btReplaceAll.Location = new System.Drawing.Point(407, 60);
			this.btReplaceAll.Name = "btReplaceAll";
			this.btReplaceAll.Size = new System.Drawing.Size(78, 23);
			this.btReplaceAll.TabIndex = 14;
			this.btReplaceAll.Text = "Replace &All";
			this.btReplaceAll.Click += new System.EventHandler(this.btReplaceAll_Click);
			// 
			// cmFind
			// 
			this.cmFind.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miSingleChar,
            this.miZeroOrMore,
            this.miOneOrMore,
            this.menuItem4,
            this.miBeginLine,
            this.miEndLine,
            this.menuItem10,
            this.miOneCharInSet,
            this.miOneCharNotInSet,
            this.miOr,
            this.miEscape,
            this.miTag});
			// 
			// miSingleChar
			// 
			this.miSingleChar.Index = 0;
			this.miSingleChar.Text = ". Any single character";
			this.miSingleChar.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miZeroOrMore
			// 
			this.miZeroOrMore.Index = 1;
			this.miZeroOrMore.Text = "* Zero or more";
			this.miZeroOrMore.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miOneOrMore
			// 
			this.miOneOrMore.Index = 2;
			this.miOneOrMore.Text = "+ One or more";
			this.miOneOrMore.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "-";
			this.menuItem4.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miBeginLine
			// 
			this.miBeginLine.Index = 4;
			this.miBeginLine.Text = "^ Beginning of line";
			this.miBeginLine.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miEndLine
			// 
			this.miEndLine.Index = 5;
			this.miEndLine.Text = "$ End of line";
			this.miEndLine.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 6;
			this.menuItem10.Text = "-";
			this.menuItem10.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miOneCharInSet
			// 
			this.miOneCharInSet.Index = 7;
			this.miOneCharInSet.Text = "[] Any one character in the set";
			this.miOneCharInSet.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miOneCharNotInSet
			// 
			this.miOneCharNotInSet.Index = 8;
			this.miOneCharNotInSet.Text = "[^] Any one character not in the set";
			this.miOneCharNotInSet.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miOr
			// 
			this.miOr.Index = 9;
			this.miOr.Text = "| Or";
			this.miOr.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miEscape
			// 
			this.miEscape.Index = 10;
			this.miEscape.Text = "\\ Escape Special Character";
			this.miEscape.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// miTag
			// 
			this.miTag.Index = 11;
			this.miTag.Text = "{} Tag expression";
			this.miTag.Click += new System.EventHandler(this.miBeginWord_Click);
			// 
			// pnlSearch
			// 
			this.pnlSearch.Controls.Add(this.chbPromptOnReplace);
			this.pnlSearch.Controls.Add(this.gbSearch);
			this.pnlSearch.Controls.Add(this.chbSearchUp);
			this.pnlSearch.Controls.Add(this.chbSearchHiddenText);
			this.pnlSearch.Controls.Add(this.chbMatchWholeWord);
			this.pnlSearch.Controls.Add(this.chbMatchCase);
			this.pnlSearch.Controls.Add(this.chbUseRegularExpressions);
			this.pnlSearch.Location = new System.Drawing.Point(8, 56);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(392, 122);
			this.pnlSearch.TabIndex = 18;
			// 
			// chbPromptOnReplace
			// 
			this.chbPromptOnReplace.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbPromptOnReplace.Location = new System.Drawing.Point(12, 88);
			this.chbPromptOnReplace.Name = "chbPromptOnReplace";
			this.chbPromptOnReplace.Size = new System.Drawing.Size(120, 24);
			this.chbPromptOnReplace.TabIndex = 19;
			this.chbPromptOnReplace.Text = "Prompt on replace";
			// 
			// gbSearch
			// 
			this.gbSearch.Controls.Add(this.chbSelectionOnly);
			this.gbSearch.Controls.Add(this.rbEntireScope);
			this.gbSearch.Controls.Add(this.rbFromCursor);
			this.gbSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbSearch.Location = new System.Drawing.Point(208, 6);
			this.gbSearch.Name = "gbSearch";
			this.gbSearch.Size = new System.Drawing.Size(176, 103);
			this.gbSearch.TabIndex = 17;
			this.gbSearch.TabStop = false;
			this.gbSearch.Text = "Search";
			// 
			// chbSelectionOnly
			// 
			this.chbSelectionOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbSelectionOnly.Location = new System.Drawing.Point(8, 58);
			this.chbSelectionOnly.Name = "chbSelectionOnly";
			this.chbSelectionOnly.Size = new System.Drawing.Size(104, 24);
			this.chbSelectionOnly.TabIndex = 2;
			this.chbSelectionOnly.Text = "Selection &Only";
			// 
			// rbEntireScope
			// 
			this.rbEntireScope.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbEntireScope.ImageIndex = 0;
			this.rbEntireScope.Location = new System.Drawing.Point(8, 38);
			this.rbEntireScope.Name = "rbEntireScope";
			this.rbEntireScope.Size = new System.Drawing.Size(160, 18);
			this.rbEntireScope.TabIndex = 1;
			this.rbEntireScope.Text = "&Entire scope";
			// 
			// rbFromCursor
			// 
			this.rbFromCursor.Checked = true;
			this.rbFromCursor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFromCursor.Location = new System.Drawing.Point(8, 16);
			this.rbFromCursor.Name = "rbFromCursor";
			this.rbFromCursor.Size = new System.Drawing.Size(160, 18);
			this.rbFromCursor.TabIndex = 0;
			this.rbFromCursor.Text = "From cursor";
			// 
			// chbSearchUp
			// 
			this.chbSearchUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbSearchUp.Location = new System.Drawing.Point(12, 44);
			this.chbSearchUp.Name = "chbSearchUp";
			this.chbSearchUp.Size = new System.Drawing.Size(156, 24);
			this.chbSearchUp.TabIndex = 14;
			this.chbSearchUp.Text = "Search &up";
			// 
			// chbSearchHiddenText
			// 
			this.chbSearchHiddenText.Checked = true;
			this.chbSearchHiddenText.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbSearchHiddenText.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbSearchHiddenText.Location = new System.Drawing.Point(138, 88);
			this.chbSearchHiddenText.Name = "chbSearchHiddenText";
			this.chbSearchHiddenText.Size = new System.Drawing.Size(156, 24);
			this.chbSearchHiddenText.TabIndex = 13;
			this.chbSearchHiddenText.Text = "Search &hidden text";
			this.chbSearchHiddenText.Visible = false;
			// 
			// chbMatchWholeWord
			// 
			this.chbMatchWholeWord.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbMatchWholeWord.Location = new System.Drawing.Point(12, 24);
			this.chbMatchWholeWord.Name = "chbMatchWholeWord";
			this.chbMatchWholeWord.Size = new System.Drawing.Size(156, 24);
			this.chbMatchWholeWord.TabIndex = 12;
			this.chbMatchWholeWord.Text = "Match &whole word";
			// 
			// chbMatchCase
			// 
			this.chbMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbMatchCase.Location = new System.Drawing.Point(12, 2);
			this.chbMatchCase.Name = "chbMatchCase";
			this.chbMatchCase.Size = new System.Drawing.Size(156, 22);
			this.chbMatchCase.TabIndex = 11;
			this.chbMatchCase.Text = "Match &case";
			// 
			// chbUseRegularExpressions
			// 
			this.chbUseRegularExpressions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chbUseRegularExpressions.Location = new System.Drawing.Point(12, 66);
			this.chbUseRegularExpressions.Name = "chbUseRegularExpressions";
			this.chbUseRegularExpressions.Size = new System.Drawing.Size(164, 24);
			this.chbUseRegularExpressions.TabIndex = 15;
			this.chbUseRegularExpressions.Text = "Use &regular expressions";
			this.chbUseRegularExpressions.Click += new System.EventHandler(this.chbUseRegularExpressions_Click);
			// 
			// DlgSearch
			// 
			this.AcceptButton = this.btFindNext;
			this.CancelButton = this.btClose;
			this.ClientSize = new System.Drawing.Size(494, 187);
			this.Controls.Add(this.pnlSearch);
			this.Controls.Add(this.btReplaceAll);
			this.Controls.Add(this.cbReplaceWith);
			this.Controls.Add(this.laReplaceWith);
			this.Controls.Add(this.laFindWhat);
			this.Controls.Add(this.btHelp);
			this.Controls.Add(this.btClose);
			this.Controls.Add(this.btMarkAll);
			this.Controls.Add(this.btReplace);
			this.Controls.Add(this.btFindNext);
			this.Controls.Add(this.btPopup);
			this.Controls.Add(this.cbFindWhat);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "DlgSearch";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Find";
			this.TopMost = true;
			this.pnlSearch.ResumeLayout(false);
			this.gbSearch.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        private void miBeginWord_Click(object sender, EventArgs e)
        {
            string text1 = ((MenuItem) sender).Text.Trim();
            int num1 = text1.IndexOf(' ');
            if (num1 > 0)
            {
                text1 = text1.Substring(0, num1);
            }
            this.cbFindWhat.Text = this.cbFindWhat.Text + text1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if ((this.cbFindWhat.Text == string.Empty) && (this.cbFindWhat.Items.Count > 0))
            {
                this.cbFindWhat.SelectedIndex = 0;
            }
        }

        protected void ReplaceChanged()
        {
            int num1 = this.isReplace ? (this.cbReplaceWith.Height + 4) : -(this.cbReplaceWith.Height + 4);
            this.Text = this.isReplace ? EditConsts.ReplaceDlgCaption : EditConsts.SearchDlgCaption;
            this.laReplaceWith.Visible = this.isReplace;
            this.cbReplaceWith.Visible = this.isReplace;
            this.btReplaceAll.Visible = this.isReplace;
            this.btReplace.Text = this.isReplace ? "&Replace" : "&Replace  \x00bb";
            this.btPopup.Enabled = this.chbUseRegularExpressions.Checked;
            this.pnlSearch.Top += num1;
            this.btMarkAll.Top += num1;
            this.btClose.Top += num1;
            this.btHelp.Top += num1;
            base.Height += num1;
        }

        protected void ShowNotFound(string s)
        {
            MessageBox.Show(string.Format(EditConsts.StringNotFound, s));
            this.cbFindWhat.Focus();
        }

        private void TextFound()
        {
            this.firstSearch = false;
            this.saveOptions = this.Options;
            this.saveText = this.cbFindWhat.Text;
        }


        // Properties
        public bool IsReplace
        {
            get
            {
                return this.isReplace;
            }
            set
            {
                if (this.isReplace != value)
                {
                    this.isReplace = value;
                    this.ReplaceChanged();
                }
            }
        }

        public SearchOptions Options
        {
            get
            {
                return
					(this.chbMatchCase.Checked ? SearchOptions.CaseSensitive : SearchOptions.None) |
					(this.chbMatchWholeWord.Checked ? SearchOptions.WholeWordsOnly : SearchOptions.None) |
					SearchOptions.SearchHiddenText | //(this.chbSearchHiddenText.Checked ? SearchOptions.SearchHiddenText : SearchOptions.None) |
					(this.chbSearchUp.Checked ? SearchOptions.BackwardSearch : SearchOptions.None) |
					(this.chbUseRegularExpressions.Checked ? SearchOptions.RegularExpressions : SearchOptions.None) |
					(this.rbEntireScope.Checked ? SearchOptions.EntireScope : SearchOptions.None) |
					(this.chbSelectionOnly.Checked ? SearchOptions.SelectionOnly : SearchOptions.None) |
					(this.findTextAtCursor ? SearchOptions.FindTextAtCursor : SearchOptions.None) |
					(this.chbPromptOnReplace.Checked ? SearchOptions.PromptOnReplace : SearchOptions.None)
					;
            }
            set
            {
                this.chbMatchCase.Checked = (value & SearchOptions.CaseSensitive) != SearchOptions.None;
                this.chbMatchWholeWord.Checked = (value & SearchOptions.WholeWordsOnly) != SearchOptions.None;
                this.chbSearchHiddenText.Checked = (value & SearchOptions.SearchHiddenText) != SearchOptions.None;
                this.chbSearchUp.Checked = (value & SearchOptions.BackwardSearch) != SearchOptions.None;
                this.chbUseRegularExpressions.Checked = (value & SearchOptions.RegularExpressions) != SearchOptions.None;
                this.rbEntireScope.Checked = (value & SearchOptions.EntireScope) != SearchOptions.None;
                this.chbSelectionOnly.Checked = (value & SearchOptions.SelectionOnly) != SearchOptions.None;
                this.btPopup.Enabled = this.chbUseRegularExpressions.Checked;
                this.findTextAtCursor = (value & SearchOptions.FindTextAtCursor) != SearchOptions.None;
                this.chbPromptOnReplace.Checked = (value & SearchOptions.PromptOnReplace) != SearchOptions.None;
            }
        }

        public ISearch Search
        {
            get
            {
                return this.search;
            }
            set
            {
                this.search = value;
            }
        }


        // Fields
        public Button btClose;
        public Button btFindNext;
        public Button btHelp;
        public Button btMarkAll;
        public Button btPopup;
        public Button btReplace;
        public Button btReplaceAll;
        public ComboBox cbFindWhat;
        public ComboBox cbReplaceWith;
        public CheckBox chbMatchCase;
        public CheckBox chbMatchWholeWord;
        public CheckBox chbPromptOnReplace;
        public CheckBox chbSearchHiddenText;
        public CheckBox chbSearchUp;
        public CheckBox chbSelectionOnly;
        public CheckBox chbUseRegularExpressions;
        public ContextMenu cmFind;
        public IContainer components;
        private bool findTextAtCursor;
        private bool firstSearch;
        public GroupBox gbSearch;
        private bool isReplace;
        public Label laFindWhat;
        public Label laReplaceWith;
        public MenuItem menuItem10;
        public MenuItem menuItem4;
        public MenuItem miBeginLine;
        public MenuItem miEndLine;
        public MenuItem miEscape;
        public MenuItem miOneCharInSet;
        public MenuItem miOneCharNotInSet;
        public MenuItem miOneOrMore;
        public MenuItem miOr;
        public MenuItem miSingleChar;
        public MenuItem miTag;
        public MenuItem miZeroOrMore;
        public Panel pnlSearch;
        public RadioButton rbEntireScope;
        public RadioButton rbFromCursor;
        private SearchOptions saveOptions;
        private string saveText;
        private ISearch search;
    }
}

