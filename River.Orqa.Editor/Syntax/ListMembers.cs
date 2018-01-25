namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ListMembers : CodeCompletionProvider, IListMembers, ICodeCompletionProvider, IList, ICollection, IEnumerable
    {
        // Methods
        public ListMembers()
        {
            this.showHints = true;
            this.showQualifiers = true;
            this.showResults = true;
            this.showParams = false;
        }

        public IListMember AddMember()
        {
            IListMember member1 = new ListMember();
            this.Add(member1);
            return member1;
        }

        public override bool ColumnVisible(int Column)
        {
            switch (Column)
            {
                case 0:
                {
                    return this.showQualifiers;
                }
                case 1:
                {
                    return this.showResults;
                }
                case 2:
                {
                    return true;
                }
                case 3:
                {
                    return this.showParams;
                }
            }
            return false;
        }

        public override string GetColumnText(int Index, int Column)
        {
            switch (Column)
            {
                case 0:
                {
                    return this.GetListMember(Index).Qualifier;
                }
                case 1:
                {
                    return this.GetListMember(Index).DataType;
                }
                case 2:
                {
                    return this.GetListMember(Index).Name;
                }
                case 3:
                {
                    return this.GetListMember(Index).ParamText;
                }
                case 4:
                {
                    return this.GetListMember(Index).Description;
                }
            }
            return string.Empty;
        }

        public override string GetDescription(int Index)
        {
            if (this.showHints)
            {
                return this.GetListMember(Index).Description;
            }
            return string.Empty;
        }

        public override int GetImageIndex(int Index)
        {
            return this.GetListMember(Index).ImageIndex;
        }

        public IListMember GetListMember(int Index)
        {
            return (IListMember) base[Index];
        }

        public override string GetText(int Index)
        {
            return this.GetListMember(Index).Name;
        }

        public IListMember InsertMember(int Index)
        {
            IListMember member1 = new ListMember();
            this.Insert(Index, member1);
            return member1;
        }

        public override bool IsBold(string Source, string Text, ref int Start, ref int End)
        {
            int num1 = 0;
            for (int num2 = Start; num2 < End; num2++)
            {
                if ((num2 < Source.Length) && ((Source[num2] == ',') || (Source[num2] == '(')))
                {
                    num1++;
                }
            }
            Start = 0;
            End = 0;
            for (int num3 = 0; num3 < Text.Length; num3++)
            {
                if ((Text[num3] == ',') || (Text[num3] == '('))
                {
                    if (num1 == 0)
                    {
                        End = num3;
                        return true;
                    }
                    if (num1 == 1)
                    {
                        Start = num3;
                        End = Text.Length;
                    }
                    num1--;
                }
            }
            return false;
        }

        public virtual void ResetShowHints()
        {
            this.ShowHints = true;
        }

        public virtual void ResetShowParams()
        {
            this.ShowParams = false;
        }

        public virtual void ResetShowQualifiers()
        {
            this.ShowQualifiers = true;
        }

        public virtual void ResetShowResults()
        {
            this.ShowResults = true;
        }


        // Properties
        public override int ColumnCount
        {
            get
            {
                return 4;
            }
        }

		// SMC: added new
        public new IListMember this[int index]
        {
            get
            {
                return this.GetListMember(index);
            }
        }

        public bool ShowHints
        {
            get
            {
                return this.showHints;
            }
            set
            {
                this.showHints = value;
            }
        }

        public bool ShowParams
        {
            get
            {
                return this.showParams;
            }
            set
            {
                this.showParams = value;
            }
        }

        public bool ShowQualifiers
        {
            get
            {
                return this.showQualifiers;
            }
            set
            {
                this.showQualifiers = value;
            }
        }

        public bool ShowResults
        {
            get
            {
                return this.showResults;
            }
            set
            {
                this.showResults = value;
            }
        }


        // Fields
        private bool showHints;
        private bool showParams;
        private bool showQualifiers;
        private bool showResults;
    }
}

