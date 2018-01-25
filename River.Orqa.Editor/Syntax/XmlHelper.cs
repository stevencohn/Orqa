namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Drawing;

    public class XmlHelper
    {
        // Methods
        public XmlHelper()
        {
        }

        public static Color DeserializeColor(string color)
        {
            if (color != string.Empty)
            {
                char[] chArray1 = new char[1] { ':' } ;
                string[] textArray1 = color.Split(chArray1);
                if (textArray1.Length == 1)
                {
                    return Color.FromName(textArray1[0]);
                }
                if (textArray1.Length == 4)
                {
                    byte num1 = byte.Parse(textArray1[0]);
                    byte num2 = byte.Parse(textArray1[1]);
                    byte num3 = byte.Parse(textArray1[2]);
                    byte num4 = byte.Parse(textArray1[3]);
                    return Color.FromArgb(num1, num2, num3, num4);
                }
            }
            return Color.Empty;
        }

        public static string SerializeColor(Color color)
        {
            if (color.IsNamedColor)
            {
                return color.Name;
            }
            if (color == Color.Empty)
            {
                return string.Empty;
            }
            object[] objArray1 = new object[4] { color.A, color.R, color.G, color.B } ;
            return string.Format("{0}:{1}:{2}:{3}", objArray1);
        }

    }
}

