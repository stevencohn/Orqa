namespace River.Orqa.Editor
{
    using System;
    using System.Windows.Forms;

    internal class PopupWnd
    {
        // Methods
        public PopupWnd(ICodeCompletionWindow Popup, Control PopupParent)
        {
            this.popup = Popup;
            this.popupParent = PopupParent;
            if ((this.popupParent != null) && this.popupParent.IsHandleCreated)
            {
                this.nativeWindow = this.GetNativeWindow(this.popupParent.Handle);
                this.nativeWindow.Add(Popup);
            }
            if ((PopupParent is Form) && ((Form) PopupParent).IsMdiChild)
            {
                Form form1 = ((Form) PopupParent).MdiParent;
                if ((form1 != null) && form1.IsHandleCreated)
                {
                    this.mdiNativeWindow = this.GetNativeWindow(form1.Handle);
                    this.mdiNativeWindow.Add(Popup);
                }
            }
        }

        private PopupNativeWindow GetNativeWindow(IntPtr Handle)
        {
            NativeWindow window1 = NativeWindow.FromHandle(Handle);
            if (window1 is PopupNativeWindow)
            {
                return (PopupNativeWindow) window1;
            }
            return new PopupNativeWindow(Handle);
        }

        public void Release()
        {
            if (this.nativeWindow != null)
            {
                this.nativeWindow.Remove(this.Popup);
            }
            this.nativeWindow = null;
            if (this.mdiNativeWindow != null)
            {
                this.mdiNativeWindow.Remove(this.Popup);
            }
            this.mdiNativeWindow = null;
        }


        // Properties
        public ICodeCompletionWindow Popup
        {
            get
            {
                return this.popup;
            }
        }


        // Fields
        private PopupNativeWindow mdiNativeWindow;
        private PopupNativeWindow nativeWindow;
        private ICodeCompletionWindow popup;
        private Control popupParent;
    }
}

