namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ICodeCompletionWindow : IControlProps
    {
        // Events
        event ClosePopupEvent ClosePopup;

        // Methods
        void Close(bool Accept);

        void CloseDelayed(bool Accept);

        bool IsFocused();

        void Popup();

        void PopupAt(Point Position);

        void PopupAt(int X, int Y);

        void ResetAutoSize();

        void ResetCodeCompletionFlags();

        void ResetContent();

        void ResetSizeAble();


        // Properties
        bool AutoSize { get; set; }

        CodeCompletionFlags CompletionFlags { get; set; }

        Point EndPos { get; set; }

        ImageList Images { get; set; }

        Control OwnerControl { get; set; }

        ICodeCompletionProvider Provider { get; set; }

        bool SizeAble { get; set; }

        Point StartPos { get; set; }

    }
}

