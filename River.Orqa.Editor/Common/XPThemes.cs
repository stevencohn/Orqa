namespace River.Orqa.Editor.Common
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class XPThemes
    {
        // Methods
        static XPThemes()
        {
            XPThemes.currentTheme = XPThemeName.None;
            XPThemes.theme = null;
            XPThemes.theme = new XPTheme();
            XPThemes.InitTheme();
        }

        public XPThemes()
        {
        }

        private static void DetectCurrentTheme()
        {
            string text1 = XPThemes.theme.GetCurrentThemeName;
			if (text1 == String.Empty)
            {
                XPThemes.currentTheme = XPThemeName.Blue;
            }
            else if (text1 == "NormalColor")
            {
                XPThemes.currentTheme = XPThemeName.Blue;
            }
            else if (text1 == "HomeStead")
            {
                XPThemes.currentTheme = XPThemeName.HomeStead;
            }
            else if (text1 == "Metallic")
            {
                XPThemes.currentTheme = XPThemeName.Metallic;
            }
            else
            {
                XPThemes.currentTheme = XPThemeName.Custom;
            }
        }

        public static void DoneTheme()
        {
            XPThemes.theme.Close();
        }

        public static void DrawEditBorder(Graphics Graphics, Rectangle Rect)
        {
            XPThemes.theme.DrawEditBorder(Graphics, Rect);
        }

        public static void DrawEditBorder(IntPtr DC, Rectangle Rect)
        {
            XPThemes.theme.DrawEditBorder(DC, Rect);
        }

        public static void InitTheme()
        {
            XPThemes.theme.Open(string.Empty);
            if (XPThemes.theme.themesAvailable)
            {
                XPThemes.DetectCurrentTheme();
            }
            else
            {
                XPThemes.currentTheme = XPThemeName.None;
            }
        }


        // Properties
        public static XPThemeName CurrentTheme
        {
            get
            {
                return XPThemes.currentTheme;
            }
        }


        // Fields
        private static XPThemeName currentTheme;
        private static XPTheme theme;

        // Nested Types
        private class XPTheme
        {
            // Methods
            public XPTheme()
            {
                this.handle = 0;
                this.themesAvailable = false;
                this.themeNames = new Hashtable();
            }

            private void CheckThemes()
            {
                this.themesAvailable = (((OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != null) && Win32.IsAppThemed()) && Win32.IsThemeActive()) && ((Win32.GetThemeAppProperties() & 2) == 2);
            }

            public void Close()
            {
                this.themeNames.Clear();
                if (this.handle != 0)
                {
                    Win32.CloseThemeData(this.handle);
                }
            }

            public void DrawEditBorder(Graphics Graphics, Rectangle Rect)
            {
                if (this.themesAvailable)
                {
                    this.OpenTheme("edit");
                    this.DrawThemeBackground(Graphics, 1, 1, Rect);
                }
            }

            public void DrawEditBorder(IntPtr DC, Rectangle Rect)
            {
                if (this.themesAvailable)
                {
                    this.OpenTheme("edit");
                    this.DrawThemeBackground(DC, 1, 1, Rect);
                }
            }

            public void DrawThemeBackground(Graphics Graphics, int PartID, int StateID, Rectangle Rect)
            {
                IntPtr ptr1 = Graphics.GetHdc();
                try
                {
                    Win32.GdiRect rect1 = new Win32.GdiRect(Rect);
                    Win32.DrawThemeBackground(this.handle, ptr1, PartID, StateID, ref rect1, 0);
                }
                finally
                {
                    Graphics.ReleaseHdc(ptr1);
                }
            }

            public void DrawThemeBackground(IntPtr DC, int PartID, int StateID, Rectangle Rect)
            {
                Win32.GdiRect rect1 = new Win32.GdiRect(Rect);
                Win32.DrawThemeBackground(this.handle, DC, PartID, StateID, ref rect1, 0);
            }

            public void Open(string Name)
            {
                this.CheckThemes();
                if (this.themesAvailable && (Name != string.Empty))
                {
                    this.handle = Win32.OpenThemeData(0, Name);
                }
            }

            public void OpenTheme(string Name)
            {
                object obj1 = this.themeNames[Name];
                if (obj1 != null)
                {
                    this.handle = (int) obj1;
                }
                else
                {
                    this.handle = Win32.OpenThemeData(0, Name);
                    this.themeNames.Add(Name, this.handle);
                }
            }


            // Properties
            public string GetCurrentThemeName
            {
                get
                {
                    string text1;
                    int num1 = 260;
                    IntPtr ptr1 = Marshal.AllocHGlobal((int) (num1 + 1));
                    IntPtr ptr2 = Marshal.AllocHGlobal((int) (num1 + 1));
                    try
                    {
                        Win32.GetCurrentThemeName(ptr1, num1, ptr2, num1, 0, 0);
                        text1 = Marshal.PtrToStringAuto(ptr2);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(ptr1);
                        Marshal.FreeHGlobal(ptr2);
                    }
                    return text1;
                }
            }


            // Fields
            private const int EBP_BACKGROUND = 0;
            private const int EP_EDITTEXT = 1;
            private const int ETS_NORMAL = 1;
            private int handle;
            private Hashtable themeNames;
            internal bool themesAvailable;
        }
    }
}

