namespace River.Orqa.Editor
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IScrolling
    {
        // Events
        event EventHandler HorizontalScroll;
        event EventHandler VerticalScroll;

        // Methods
        void Assign(IScrolling Source);

        void ResetDefaultHorzScrollSize();

        void ResetScrollBars();

        void ResetShowScrollHint();

        void ResetSmoothScroll();


        // Properties
        int DefaultHorzScrollSize { get; set; }

        RichTextBoxScrollBars ScrollBars { get; set; }

        bool ShowScrollHint { get; set; }

        bool SmoothScroll { get; set; }

        int WindowOriginX { get; set; }

        int WindowOriginY { get; set; }

    }
}

