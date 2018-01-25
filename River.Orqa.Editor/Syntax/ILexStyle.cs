namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public interface ILexStyle
    {
        // Methods
        void Assign(ILexStyle Source);

        void ResetBackColor();

        void ResetFontStyle();

        void ResetForeColor();

        void ResetPlainText();


        // Properties
        Color BackColor { get; set; }

        string Desc { get; set; }

        System.Drawing.FontStyle FontStyle { get; set; }

        Color ForeColor { get; set; }

        int Index { get; }

        string Name { get; set; }

        bool PlainText { get; set; }

    }
}

