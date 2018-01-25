namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Drawing;

    public class CodeCompletionColumn : ICodeCompletionColumn
    {
        // Methods
        public CodeCompletionColumn()
        {
            this.foreColor = Consts.DefaultControlForeColor;
            this.visible = true;
        }

        public virtual void ResetFontStyle()
        {
            this.fontStyle = System.Drawing.FontStyle.Regular;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = Consts.DefaultControlForeColor;
        }

        public virtual void ResetVisible()
        {
            this.Visible = true;
        }


        // Properties
        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                this.fontStyle = value;
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                this.foreColor = value;
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

        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
            }
        }


        // Fields
        private System.Drawing.FontStyle fontStyle;
        private Color foreColor;
        private string name;
        private bool visible;
    }
}

