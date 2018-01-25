namespace River.Orqa.Editor
{
    using System;

    public interface ICodeCompletionHint : ICodeCompletionWindow, IControlProps
    {
        // Methods
        void ResetAutoHide();

        void ResetAutoHidePause();


        // Properties
        bool AutoHide { get; set; }

        int AutoHidePause { get; set; }

    }
}

