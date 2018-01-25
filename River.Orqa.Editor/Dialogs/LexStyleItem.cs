namespace River.Orqa.Editor.Dialogs
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct LexStyleItem
    {
        public string Name;
        public string InternalName;
        public Color ForeColor;
        public Color BackColor;
        public System.Drawing.FontStyle FontStyle;
        public bool PlainText;
        public LexStyleItem(string AName, string AInternalName, Color AForeColor, Color ABackColor, System.Drawing.FontStyle AFontStyle, bool APlainText)
        {
            this.Name = AName;
            this.InternalName = AInternalName;
            this.ForeColor = AForeColor;
            this.BackColor = ABackColor;
            this.FontStyle = AFontStyle;
            this.PlainText = APlainText;
        }
        public LexStyleItem(string AName, string AInternalName, Color AForeColor) : this(AName, AInternalName, AForeColor, Color.Empty, System.Drawing.FontStyle.Regular, false)
        {
        }
        public LexStyleItem(string AName, string AInternalName) : this(AName, AInternalName, EditConsts.DefaultLineStyleForeColor, Color.Empty, System.Drawing.FontStyle.Regular, false)
        {
        }
    }
}

