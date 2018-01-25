namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;

    public class ListMember : MethodInfo, IListMember, IMethodInfo, IDelegateInfo, ISyntaxTypeInfo, IHasParams, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public ListMember()
        {
            this.qualifier = string.Empty;
            this.paramText = string.Empty;
            this.customData = null;
        }


        // Properties
        public object CustomData
        {
            get
            {
                return this.customData;
            }
            set
            {
                this.customData = value;
            }
        }

        public virtual string ParamText
        {
            get
            {
                if (this.paramText != string.Empty)
                {
                    return this.paramText;
                }
                string text1 = string.Empty;
                foreach (IParamInfo info1 in base.Params)
                {
                    string[] textArray1;
                    if (text1 != string.Empty)
                    {
                        textArray1 = new string[5] { info1.Qualifier, " ", info1.DataType, " ", info1.Name } ;
                        text1 = text1 + "," + string.Concat(textArray1).Trim();
                        continue;
                    }
                    textArray1 = new string[5] { info1.Qualifier, " ", info1.DataType, " ", info1.Name } ;
                    text1 = text1 + string.Concat(textArray1).Trim();
                }
                return ("(" + text1 + ")");
            }
            set
            {
                this.paramText = value;
            }
        }

        public string Qualifier
        {
            get
            {
                return this.qualifier;
            }
            set
            {
                this.qualifier = value;
            }
        }


        // Fields
        private object customData;
        private string paramText;
        private string qualifier;
    }
}

