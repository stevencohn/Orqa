namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;
	using System.Threading;
    using System.Windows.Forms;

    internal class PopupHookManager
    {
        // Methods
        static PopupHookManager()
        {
            PopupHookManager.inMouseHook = false;
            PopupHookManager.mouseHookHandle = IntPtr.Zero;
            PopupHookManager.Popups = new ArrayList();
        }

        public PopupHookManager()
        {
        }

        internal static void CheckMouse(Control control, Point mousePos)
        {
            for (int num1 = PopupHookManager.Popups.Count - 1; num1 >= 0; num1--)
            {
                PopupWnd wnd1 = (PopupWnd) PopupHookManager.Popups[num1];
                Control control1 = (Control) wnd1.Popup;
                ICodeCompletionWindow window1 = wnd1.Popup;
                if (((window1 != null) && control1.Created) && control1.Visible)
                {
                    Control control2 = control1.FindForm();
                    if ((((control2 != null) && !control2.Contains(control)) && ((control2 != control) && (control1 != control))) && (!control1.Contains(control) && !control1.Bounds.Contains(mousePos)))
                    {
                        window1.CloseDelayed(false);
                    }
                }
            }
        }

        internal static void FilterMouseMessage(int Msg, IntPtr HWnd, IntPtr WParam, IntPtr LParam)
        {
            if (((Msg >= 0x201) && (Msg <= 520)) || ((Msg >= 0xa1) && (Msg <= 0xa9)))
            {
                Control control1 = Control.FromHandle(HWnd);
                PopupHookManager.CheckMouse(control1, Control.MousePosition);
            }
        }

        internal static void InstallHook()
        {
            PopupHookManager.mouseHookProc = new HookHandler(PopupHookManager.MouseHook);

			PopupHookManager.mouseHookHandle = Win32.SetWindowsHookEx(
				7, PopupHookManager.
				mouseHookProc, 0, 
				Thread.CurrentThread.ManagedThreadId);
            
			Application.ApplicationExit += new EventHandler(PopupHookManager.OnApplicationExit);
        }

        private static int MouseHook(int ncode, IntPtr wParam, IntPtr lParam)
        {
            if (ncode == 0)
            {
                if (!PopupHookManager.inMouseHook && (lParam != IntPtr.Zero))
                {
                    River.Orqa.Editor.MOUSEHOOKSTRUCT mousehookstruct1 = (River.Orqa.Editor.MOUSEHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(River.Orqa.Editor.MOUSEHOOKSTRUCT));
                    try
                    {
                        PopupHookManager.inMouseHook = true;
                        PopupHookManager.FilterMouseMessage(wParam.ToInt32(), mousehookstruct1.hwnd, IntPtr.Zero, new IntPtr((mousehookstruct1.Pt.X << 0x10) | mousehookstruct1.Pt.Y));
                        goto Label_008B;
                    }
                    finally
                    {
                        PopupHookManager.inMouseHook = false;
                    }
                }
                return Win32.CallNextHookEx(PopupHookManager.mouseHookHandle, ncode, wParam, lParam);
            }
        Label_008B:
            return Win32.CallNextHookEx(PopupHookManager.mouseHookHandle, ncode, wParam, lParam);
        }

        internal static void OnApplicationExit(object sender, EventArgs e)
        {
            PopupHookManager.RemoveHook();
        }

        internal static void PopupClosed(ICodeCompletionWindow popup)
        {
            for (int num1 = PopupHookManager.Popups.Count - 1; num1 >= 0; num1--)
            {
                PopupWnd wnd1 = (PopupWnd) PopupHookManager.Popups[num1];
                if (wnd1.Popup == popup)
                {
                    wnd1.Release();
                    PopupHookManager.Popups.RemoveAt(num1);
                    break;
                }
            }
            if (PopupHookManager.Popups.Count == 0)
            {
                PopupHookManager.RemoveHook();
            }
        }

        internal static void PopupShowing(ICodeCompletionWindow popup)
        {
            PopupHookManager.Popups.Add(new PopupWnd(popup, ((CodeCompletionWindow) popup).OwnerControl.FindForm()));
            if (PopupHookManager.mouseHookHandle == IntPtr.Zero)
            {
                PopupHookManager.InstallHook();
            }
        }

        internal static void RemoveHook()
        {
            if (PopupHookManager.mouseHookHandle != IntPtr.Zero)
            {
                Application.ApplicationExit -= new EventHandler(PopupHookManager.OnApplicationExit);
                Win32.UnhookWindowsHookEx(PopupHookManager.mouseHookHandle);
                PopupHookManager.mouseHookHandle = IntPtr.Zero;
                PopupHookManager.mouseHookProc = null;
            }
        }


        // Fields
        private static bool inMouseHook;
        private static IntPtr mouseHookHandle;
        private static HookHandler mouseHookProc;
        private static ArrayList Popups;
    }
}

