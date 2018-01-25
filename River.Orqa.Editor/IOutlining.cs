namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface IOutlining : ICollapsable
    {
        // Methods
        void Assign(IOutlining Source);

        void OutlineText();

        void ResetOutlineColor();

        void UnOutlineText();


        // Properties
        Color OutlineColor { get; set; }

    }
}

