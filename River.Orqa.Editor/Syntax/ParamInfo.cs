namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class ParamInfo : SyntaxTypeInfo, IParamInfo, ISyntaxTypeInfo, ISyntaxInfo
    {
        // Methods
        public ParamInfo()
        {
        }

        public ParamInfo(string Name, string DataType, Point Position, string Qualifier) : base(Name, DataType, Position)
        {
            this.qualifier = Qualifier;
        }

        protected override void Init()
        {
            base.Init();
            base.ImageIndex = UnitInfo.ParamImage;
        }


        // Properties
        public string ParamText
        {
            get
            {
                return ((this.Qualifier + ' ' + base.DataType).Trim() + ' ' + base.Name).Trim();
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
        private string qualifier;
    }
}

