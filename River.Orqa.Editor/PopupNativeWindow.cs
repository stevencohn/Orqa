namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal class PopupNativeWindow : NativeWindow
    {
        // Methods
        public PopupNativeWindow(IntPtr Handle)
        {
            this.popups = new ArrayList();
            base.AssignHandle(Handle);
        }

        public void Add(ICodeCompletionWindow Popup)
        {
            this.popups.Add(Popup);
        }

        ~PopupNativeWindow()
        {
            this.popups.Clear();
        }

        public void Remove(ICodeCompletionWindow Popup)
        {
            this.popups.Remove(Popup);
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 0x86) && (m.WParam == IntPtr.Zero))
            {
                foreach (ICodeCompletionWindow window1 in this.popups)
                {
                    if (window1.Visible)
                    {
                        m.Result = (IntPtr) 1;
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }


        // Fields
        private ArrayList popups;
    }
}

