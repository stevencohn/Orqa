namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface IHyperTextEx : IHyperText
    {
        // Events
        event UrlJumpEvent JumpToUrl;

        // Methods
        void Assign(IHyperTextEx Source);

        void ResetShowHints();

        void ResetUrlColor();

        void ResetUrlStyle();

        void UrlJump(string Text);


        // Properties
        bool ShowHints { get; set; }

        Color UrlColor { get; set; }

        FontStyle UrlStyle { get; set; }

    }
}

