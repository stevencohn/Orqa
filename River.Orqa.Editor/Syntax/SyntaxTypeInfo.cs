namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class SyntaxTypeInfo : SyntaxInfo, ISyntaxTypeInfo, ISyntaxInfo
    {
        // Methods
        public SyntaxTypeInfo()
        {
            this.dataType = string.Empty;
        }

        public SyntaxTypeInfo(string Name, string DataType, Point Position) : base(Name, Position)
        {
            this.dataType = string.Empty;
            this.dataType = DataType;
        }


        // Properties
        public string DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                this.dataType = value;
            }
        }


        // Fields
        private string dataType;
    }
}

