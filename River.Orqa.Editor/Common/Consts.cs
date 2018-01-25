namespace River.Orqa.Editor.Common
{
    using System;
    using System.Drawing;

    public class Consts
    {
        // Methods
        static Consts()
        {
            Consts.EmptyColor = "None";
            Consts.DefaultControlBackColor = SystemColors.Window;
            Consts.DefaultControlForeColor = SystemColors.WindowText;
            Consts.ErrorCaption = "Error";
            Consts.CRLF = "\r\n";
            Consts.TabStr = "\t";
            char[] chArray1 = new char[2] { '\r', '\n' } ;
            Consts.crlfArray = chArray1;
            Consts.DefaultHintDelay = 500;
        }

        public Consts()
        {
        }


        // Fields
        public const char AveChar = 'x';
        public const char CR = '\r';
        public static string CRLF;
        public static char[] crlfArray;
        public static Color DefaultControlBackColor;
        public static Color DefaultControlForeColor;
        public static int DefaultHintDelay;
        public static string EmptyColor;
        public static string ErrorCaption;
        public const char LF = '\n';
        public const char Space = ' ';
        public const char Tab = '\t';
        public static string TabStr;
    }
}

