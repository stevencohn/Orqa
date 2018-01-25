namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class CodeTemplates : CodeCompletionProvider, ICodeTemplates, ICodeCompletionProvider, IList, ICollection, IEnumerable
    {
        // Methods
        public CodeTemplates()
        {
        }

        public ICodeTemplate AddTemplate()
        {
            ICodeTemplate template1 = new CodeTemplate();
            this.Add(template1);
            return template1;
        }

        public override bool ColumnVisible(int Column)
        {
            return true;
        }

        public override string GetColumnText(int Index, int Column)
        {
            switch (Column)
            {
                case 0:
                {
                    return this.GetTemplate(Index).Name;
                }
                case 1:
                {
                    return this.GetTemplate(Index).Description;
                }
            }
            return string.Empty;
        }

        public override int GetImageIndex(int Index)
        {
            return this.GetTemplate(Index).ImageIndex;
        }

        protected internal ICodeTemplate GetTemplate(int Index)
        {
            return (ICodeTemplate) base[Index];
        }

        public override string GetText(int Index)
        {
            return this.GetTemplate(Index).Code;
        }

        public ICodeTemplate InsertTemplate(int Index)
        {
            ICodeTemplate template1 = new CodeTemplate();
            this.Insert(Index, template1);
            return template1;
        }


        // Properties
        public override int ColumnCount
        {
            get
            {
                return 2;
            }
        }

		// SMC: added new
		public new ICodeTemplate this[int index]
        {
            get
            {
                return this.GetTemplate(index);
            }
        }

    }
}

