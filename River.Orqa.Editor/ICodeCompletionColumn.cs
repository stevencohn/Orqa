namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface ICodeCompletionColumn
    {
        // Methods
        void ResetFontStyle();

        void ResetForeColor();

        void ResetVisible();


        // Properties
        System.Drawing.FontStyle FontStyle { get; set; }

        Color ForeColor { get; set; }

        string Name { get; set; }

        bool Visible { get; set; }

    }
}

