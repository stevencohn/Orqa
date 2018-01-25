namespace River.Orqa.Editor
{
    using System;
    using System.Collections;

    public class HyperText
    {
        // Methods
        public HyperText()
        {
        }

        private static void InitIdentsTable()
        {
            if (HyperText.identsTable == null)
            {
                HyperText.identsTable = new Hashtable();
                for (char ch1 = 'A'; ch1 <= 'Z'; ch1++)
                {
                    HyperText.identsTable.Add(ch1, ch1);
                }
                for (char ch2 = 'a'; ch2 <= 'z'; ch2++)
                {
                    HyperText.identsTable.Add(ch2, ch2);
                }
                for (char ch3 = '0'; ch3 <= '9'; ch3++)
                {
                    HyperText.identsTable.Add(ch3, ch3);
                }
                HyperText.identsTable.Add('_', '_');
            }
        }

        public static bool IsEmailString(string Text)
        {
            bool flag1 = string.Compare(Text, 0, EditConsts.MailTo, 0, EditConsts.MailTo.Length, true) == 0;
            if (!flag1)
            {
                int num1 = Text.Length;
                int num2 = Text.IndexOf("@");
                int num3 = Text.LastIndexOf("@");
                int num4 = Text.LastIndexOf(".");
                flag1 = (((num2 >= 0) && (num2 == num3)) && (num4 > num2)) && (num2 != (num1 - 1));
                if (flag1)
                {
                    HyperText.InitIdentsTable();
                    flag1 = HyperText.identsTable.ContainsKey(Text[num2]);
                }
            }
            return flag1;
        }

        public static bool IsFileString(string Text)
        {
            return (string.Compare(Text, 0, EditConsts.FileProtocol, 0, EditConsts.FileProtocol.Length, true) == 0);
        }

        public static bool IsFtpString(string Text)
        {
            return (string.Compare(Text, 0, EditConsts.FTPProtocol, 0, EditConsts.FTPProtocol.Length, true) == 0);
        }

        public static bool IsGopherString(string Text)
        {
            return (string.Compare(Text, 0, EditConsts.GopherProtocol, 0, EditConsts.GopherProtocol.Length, true) == 0);
        }

        public static bool IsHttpString(string Text)
        {
            return (string.Compare(Text, 0, EditConsts.HTTPProtocol, 0, EditConsts.HTTPProtocol.Length, true) == 0);
        }

        public static bool IsHyperText(string Text)
        {
            if (((!HyperText.IsEmailString(Text) && !HyperText.IsWWWString(Text)) && (!HyperText.IsHttpString(Text) && !HyperText.IsFtpString(Text))) && !HyperText.IsGopherString(Text))
            {
                return HyperText.IsFileString(Text);
            }
            return true;
        }

        public static bool IsWWWString(string Text)
        {
            return (string.Compare(Text, 0, EditConsts.WWW, 0, EditConsts.WWW.Length, true) == 0);
        }


        // Fields
        private static Hashtable identsTable;
    }
}

