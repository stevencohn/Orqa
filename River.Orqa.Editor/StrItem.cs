namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.IO;

    public class StrItem
    {
        // Methods
        public StrItem(string s)
        {
            this.String = s;
        }

        public static void SetColorFlag(ref short[] ColorData, int Start, int Len, byte Flag, bool Set)
        {
            for (int num3 = Start; num3 < (Start + Len); num3++)
            {
                short num1 = ColorData[num3];
                byte num2 = Set ? ((byte) ((num1 >> 8) | Flag)) : ((byte) ((num1 >> 8) & ~Flag));
                ColorData[num3] = (short) (((byte) num1) | (num2 << 8));
            }
        }

        public static string[] Split(string Text)
        {
            string text1;
            if ((Text == null) || (Text == string.Empty))
            {
                return new string[0];
            }
            ArrayList list1 = new ArrayList();
            StringReader reader1 = new StringReader(Text);
            while ((text1 = reader1.ReadLine()) != null)
            {
                list1.Add(text1);
            }
            char ch1 = Text[Text.Length - 1];
            if ((ch1 == '\r') || (ch1 == '\n'))
            {
                list1.Add(string.Empty);
            }
            string[] textArray1 = new string[list1.Count];
            list1.CopyTo(textArray1);
            return textArray1;
        }


        // Properties
        public int LexState
        {
            get
            {
                return this.lexState;
            }
            set
            {
                this.lexState = value;
            }
        }

        public int PrevLexState
        {
            get
            {
                return this.prevLexState;
            }
            set
            {
                this.prevLexState = value;
            }
        }

        public StrItemState State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        public virtual string String
        {
            get
            {
                return this.str;
            }
            set
            {
                if (this.str != value)
                {
                    this.str = value;
                    this.ColorData = new short[this.str.Length];
                }
            }
        }


        // Fields
        public short[] ColorData;
        private int lexState;
        private int prevLexState;
        private StrItemState state;
        private string str;
    }
}

