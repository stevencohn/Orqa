namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class CodeCompletionProvider : ArrayList, ICodeCompletionProvider, IList, ICollection, IEnumerable
    {
        // Events
        public event ClosePopupEvent ClosePopup;

        // Methods
        public CodeCompletionProvider()
        {
            this.selIndex = -1;
        }

        public virtual bool ColumnVisible(int Column)
        {
            return false;
        }

        public void DoClosePopup(object Sender, ClosingEventArgs e)
        {
            if (this.ClosePopup != null)
            {
                this.ClosePopup(Sender, e);
            }
        }

        public virtual string GetColumnText(int Index, int Column)
        {
            return string.Empty;
        }

        public virtual string GetDescription(int Index)
        {
            return string.Empty;
        }

        public virtual int GetImageIndex(int Index)
        {
            return -1;
        }

        public virtual string GetText(int Index)
        {
            return string.Empty;
        }

        public virtual bool IsBold(string Source, string Text, ref int Start, ref int End)
        {
            Start = 0;
            End = 0;
            return false;
        }

        public bool IsCloseEventAssigned()
        {
            return (this.ClosePopup != null);
        }


        // Properties
        public virtual int ColumnCount
        {
            get
            {
                return 0;
            }
        }

        public string[] Descriptions
        {
            get
            {
                string[] textArray1 = new string[this.Count];
                for (int num1 = 0; num1 < this.Count; num1++)
                {
                    textArray1[num1] = this.GetDescription(num1);
                }
                return textArray1;
            }
        }

        public int[] ImageIndexes
        {
            get
            {
                int[] numArray1 = new int[this.Count];
                for (int num1 = 0; num1 < this.Count; num1++)
                {
                    numArray1[num1] = this.GetImageIndex(num1);
                }
                return numArray1;
            }
        }

        public ImageList Images
        {
            get
            {
                return this.images;
            }
            set
            {
                this.images = value;
            }
        }

        public int SelIndex
        {
            get
            {
                return this.selIndex;
            }
            set
            {
                this.selIndex = value;
            }
        }

        public bool ShowDescriptions
        {
            get
            {
                return this.showDescriptions;
            }
            set
            {
                this.showDescriptions = value;
            }
        }

        public string[] Strings
        {
            get
            {
                string[] textArray1 = new string[this.Count];
                for (int num1 = 0; num1 < this.Count; num1++)
                {
                    textArray1[num1] = this.GetText(num1);
                }
                return textArray1;
            }
        }


        // Fields
        private ImageList images;
        private int selIndex;
        private bool showDescriptions;
    }
}

