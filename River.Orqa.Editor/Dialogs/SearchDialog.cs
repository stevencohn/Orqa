namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public class SearchDialog : ISearchDialog
    {
        // Methods
        static SearchDialog()
        {
            SearchDialog.searchDlg = null;
            SearchDialog.searchSettings = new SearchSettings();
        }

        public SearchDialog()
        {
        }

        private void CheckOption(SearchOptions value)
        {
            if ((SearchDialog.searchSettings.SearchOptions & value) == SearchOptions.None)
            {
                SearchDialog.searchSettings.SearchOptions |= value;
            }
            else
            {
                SearchDialog.searchSettings.SearchOptions &= ~value;
            }
        }

        private void CopyList(IList FromList, IList ToList)
        {
            ToList.Clear();
            foreach (object obj1 in FromList)
            {
                ToList.Add(obj1);
            }
        }

        private void DoClose(object Sender, EventArgs e)
        {
            if (SearchDialog.searchDlg != null)
            {
                SearchDialog.searchSettings.SearchOptions = SearchDialog.searchDlg.Options;
                this.CopyList(SearchDialog.searchDlg.cbFindWhat.Items, SearchDialog.searchSettings.SearchList);
                this.CopyList(SearchDialog.searchDlg.cbReplaceWith.Items, SearchDialog.searchSettings.ReplaceList);
            }
            SearchDialog.searchDlg = null;
        }

        public void DoneSearch(ISearch Search)
        {
            if ((SearchDialog.searchDlg != null) && (SearchDialog.searchDlg.Search == Search))
            {
                SearchDialog.searchDlg.Search = null;
            }
        }

        public void EnsureVisible(Rectangle Rect)
        {
            if (SearchDialog.searchDlg != null)
            {
                Rectangle rectangle1 = Rect;
                rectangle1.Intersect(SearchDialog.searchDlg.Bounds);
                if (!rectangle1.IsEmpty)
                {
                    if ((Rect.Bottom + SearchDialog.searchDlg.Height) < SystemInformation.WorkingArea.Height)
                    {
                        SearchDialog.searchDlg.Top = Rect.Bottom;
                    }
                    else
                    {
                        SearchDialog.searchDlg.Top = Rect.Top - SearchDialog.searchDlg.Height;
                    }
                    if ((Rect.Right + SearchDialog.searchDlg.Width) < SystemInformation.WorkingArea.Width)
                    {
                        SearchDialog.searchDlg.Left = Rect.Right;
                    }
                    else
                    {
                        SearchDialog.searchDlg.Left = Rect.Left - SearchDialog.searchDlg.Width;
                    }
                }
            }
        }

        public DialogResult Execute(ISearch Search, bool IsModal, bool IsReplace)
        {
            if (SearchDialog.searchDlg == null)
            {
                SearchDialog.searchDlg = new DlgSearch();
                SearchDialog.searchDlg.Options = SearchDialog.searchSettings.SearchOptions;
                this.CopyList(SearchDialog.searchSettings.SearchList, SearchDialog.searchDlg.cbFindWhat.Items);
                this.CopyList(SearchDialog.searchSettings.ReplaceList, SearchDialog.searchDlg.cbReplaceWith.Items);
                SearchDialog.searchDlg.Closed += new EventHandler(this.DoClose);
            }
            if ((SearchDialog.searchSettings.SearchOptions & SearchOptions.FindTextAtCursor) != SearchOptions.None)
            {
                string text1 = Search.GetTextAtCursor().Trim();
                if (text1 != string.Empty)
                {
                    SearchDialog.searchDlg.cbFindWhat.Text = text1;
                }
            }
            SearchDialog.searchDlg.Search = Search;
            SearchDialog.searchDlg.Init();
            SearchDialog.searchDlg.IsReplace = IsReplace;
            if (IsModal)
            {
                Form form1 = SearchDialog.searchDlg;
                DialogResult result1 = SearchDialog.searchDlg.ShowDialog();
                form1.Dispose();
                return result1;
            }
            SearchDialog.searchDlg.Show();
            return DialogResult.None;
        }

        public void ToggleHiddenText()
        {
            this.CheckOption(SearchOptions.SearchHiddenText);
            if (SearchDialog.searchDlg != null)
            {
                SearchDialog.searchDlg.chbSearchHiddenText.Checked = !SearchDialog.searchDlg.chbSearchHiddenText.Checked;
            }
        }

        public void ToggleMatchCase()
        {
            this.CheckOption(SearchOptions.CaseSensitive);
            if (SearchDialog.searchDlg != null)
            {
                SearchDialog.searchDlg.chbMatchCase.Checked = !SearchDialog.searchDlg.chbMatchCase.Checked;
            }
        }

        public void TogglePromptOnReplace()
        {
            this.CheckOption(SearchOptions.PromptOnReplace);
            if (SearchDialog.searchDlg != null)
            {
                SearchDialog.searchDlg.chbPromptOnReplace.Checked = !SearchDialog.searchDlg.chbPromptOnReplace.Checked;
            }
        }

        public void ToggleRegularExpressions()
        {
            this.CheckOption(SearchOptions.RegularExpressions);
            if (SearchDialog.searchDlg != null)
            {
                SearchDialog.searchDlg.chbUseRegularExpressions.Checked = !SearchDialog.searchDlg.chbUseRegularExpressions.Checked;
            }
        }

        public void ToggleSearchUp()
        {
            this.CheckOption(SearchOptions.BackwardSearch);
            if (SearchDialog.searchDlg != null)
            {
                SearchDialog.searchDlg.chbSearchUp.Checked = !SearchDialog.searchDlg.chbSearchUp.Checked;
            }
        }

        public void ToggleWholeWord()
        {
            this.CheckOption(SearchOptions.WholeWordsOnly);
            if (SearchDialog.searchDlg != null)
            {
                SearchDialog.searchDlg.chbMatchWholeWord.Checked = !SearchDialog.searchDlg.chbMatchWholeWord.Checked;
            }
        }


        // Properties
        public bool Visible
        {
            get
            {
                if (SearchDialog.searchDlg != null)
                {
                    return SearchDialog.searchDlg.Visible;
                }
                return false;
            }
            set
            {
                if (SearchDialog.searchDlg != null)
                {
                    SearchDialog.searchDlg.Visible = value;
                }
            }
        }


        // Fields
        private static DlgSearch searchDlg;
        private static SearchSettings searchSettings;
    }
}

