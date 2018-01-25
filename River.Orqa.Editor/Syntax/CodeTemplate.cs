namespace River.Orqa.Editor.Syntax
{
    using System;

    public class CodeTemplate : ICodeTemplate
    {
        // Methods
        public CodeTemplate()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.code = string.Empty;
        }


        // Properties
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

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

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                this.imageIndex = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }


        // Fields
        private string code;
        private object customData;
        private string description;
        private int imageIndex;
        private string name;
    }
}

