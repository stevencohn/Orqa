namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface ILineStyle
    {
        // Methods
        void Assign(ILineStyle Source);

        void ResetBackColor();

        void ResetForeColor();

        void ResetImageIndex();

        void ResetOptions();


        // Properties
        Color BackColor { get; set; }

        Color ForeColor { get; set; }

        int ImageIndex { get; set; }

        string Name { get; set; }

        LineStyleOptions Options { get; set; }

    }
}

