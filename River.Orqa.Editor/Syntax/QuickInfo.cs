namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;

    public class QuickInfo : CodeCompletionProvider, IQuickInfo, ICodeCompletionProvider, IList, ICollection, IEnumerable
    {
        // Methods
        public QuickInfo()
        {
            this.Add(string.Empty);
        }

        public override string GetText(int Index)
        {
            return this.Text;
        }

        public override bool IsBold(string Source, string Text, ref int Start, ref int End)
        {
            Start = this.boldStart;
            End = this.boldEnd;
            return (this.boldStart < this.boldEnd);
        }


        // Properties
        public int BoldEnd
        {
            get
            {
                return this.boldEnd;
            }
            set
            {
                this.boldEnd = value;
            }
        }

        public int BoldStart
        {
            get
            {
                return this.boldStart;
            }
            set
            {
                this.boldStart = value;
            }
        }

        public string Text
        {
            get
            {
                if (this.Count <= 0)
                {
                    return string.Empty;
                }
                return (string) this[0];
            }
            set
            {
                if (this.Count > 0)
                {
                    this[0] = value;
                }
                else
                {
                    this.Add(value);
                }
            }
        }


        // Fields
        private int boldEnd;
        private int boldStart;
    }
}

