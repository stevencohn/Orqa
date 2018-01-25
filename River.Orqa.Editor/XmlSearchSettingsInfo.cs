namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Dialogs;
    using System;

    public class XmlSearchSettingsInfo
    {
        // Methods
        public XmlSearchSettingsInfo()
        {
            this.searchOptions = EditConsts.DefaultSearchOptions;
            string[] textArray1 = new string[0];
            this.searchList = textArray1;
            textArray1 = new string[0];
            this.replaceList = textArray1;
        }

        public XmlSearchSettingsInfo(SearchSettings Owner) : this()
        {
            this.owner = Owner;
        }

        public void FixupReferences(SearchSettings Owner)
        {
            this.owner = Owner;
            this.SearchList = this.searchList;
            this.ReplaceList = this.replaceList;
            this.SearchOptions = this.searchOptions;
        }


        // Properties
        public string[] ReplaceList
        {
            get
            {
                if (this.owner == null)
                {
                    return this.replaceList;
                }
                string[] textArray1 = new string[this.owner.ReplaceList.Count];
                for (int num1 = 0; num1 < this.owner.ReplaceList.Count; num1++)
                {
                    textArray1[num1] = (string) this.owner.ReplaceList[num1];
                }
                return textArray1;
            }
            set
            {
                this.replaceList = value;
                if (this.owner != null)
                {
                    this.owner.ReplaceList.Clear();
                    string[] textArray1 = value;
                    for (int num1 = 0; num1 < textArray1.Length; num1++)
                    {
                        string text1 = textArray1[num1];
                        this.owner.ReplaceList.Add(text1);
                    }
                }
            }
        }

        public string[] SearchList
        {
            get
            {
                if (this.owner == null)
                {
                    return this.searchList;
                }
                string[] textArray1 = new string[this.owner.SearchList.Count];
                for (int num1 = 0; num1 < this.owner.SearchList.Count; num1++)
                {
                    textArray1[num1] = (string) this.owner.SearchList[num1];
                }
                return textArray1;
            }
            set
            {
                this.searchList = value;
                if (this.owner != null)
                {
                    this.owner.SearchList.Clear();
                    string[] textArray1 = value;
                    for (int num1 = 0; num1 < textArray1.Length; num1++)
                    {
                        string text1 = textArray1[num1];
                        this.owner.SearchList.Add(text1);
                    }
                }
            }
        }

        public River.Orqa.Editor.SearchOptions SearchOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.searchOptions;
                }
                return this.owner.SearchOptions;
            }
            set
            {
                this.searchOptions = value;
                if (this.owner != null)
                {
                    this.owner.SearchOptions = value;
                }
            }
        }


        // Fields
        private SearchSettings owner;
        private string[] replaceList;
        private string[] searchList;
        private River.Orqa.Editor.SearchOptions searchOptions;
    }
}

