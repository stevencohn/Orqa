namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface IBraceMatchingEx : IBraceMatching
    {
        // Methods
        void Assign(IBraceMatchingEx Source);

        void ResetBracesColor();

        void ResetBracesStyle();

        void ResetUseRoundRect();


        // Properties
        Color BracesColor { get; set; }

        FontStyle BracesStyle { get; set; }

        bool UseRoundRect { get; set; }

    }
}

