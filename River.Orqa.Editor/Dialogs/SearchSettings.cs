namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using System;
    using System.Collections;

    public class SearchSettings : PersistentSettings
    {
        // Methods
        public SearchSettings()
        {
            this.searchList = new ArrayList();
            this.replaceList = new ArrayList();
            this.searchOptions = EditConsts.DefaultSearchOptions;
        }

        public override void Assign(IPersistentSettings Source)
        {
            if (Source is SearchSettings)
            {
                SearchSettings settings1 = (SearchSettings) Source;
                this.CopyList(settings1.searchList, this.searchList);
                this.CopyList(settings1.replaceList, this.replaceList);
                this.searchOptions = settings1.searchOptions;
            }
        }

        private void CopyList(ArrayList FromList, ArrayList ToList)
        {
            ToList.Clear();
            foreach (object obj1 in FromList)
            {
                ToList.Add(obj1);
            }
        }

        public override Type GetXmlType()
        {
            return typeof(XmlSearchSettingsInfo);
        }


        // Properties
        public IList ReplaceList
        {
            get
            {
                return this.replaceList;
            }
        }

        public IList SearchList
        {
            get
            {
                return this.searchList;
            }
        }

        public River.Orqa.Editor.SearchOptions SearchOptions
        {
            get
            {
                return this.searchOptions;
            }
            set
            {
                this.searchOptions = value;
            }
        }

        public override object XmlInfo
        {
            get
            {
                return new XmlSearchSettingsInfo(this);
            }
            set
            {
                ((XmlSearchSettingsInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private ArrayList replaceList;
        private ArrayList searchList;
        private River.Orqa.Editor.SearchOptions searchOptions;
    }
}

