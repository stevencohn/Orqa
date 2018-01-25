namespace QWhale.Design.Dialogs
{
    using System;
    using System.Windows.Forms;

    internal class FlagEnumerationListBox : CheckedListBox
    {
        // Methods
        public FlagEnumerationListBox()
        {
            this.updateCount = 0;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        public void ListBoxBeginUpdate()
        {
            base.BeginUpdate();
            this.updateCount++;
        }

        public void ListBoxEndUpdate()
        {
            base.EndUpdate();
            this.updateCount--;
            if (this.updateCount == 0)
            {
                base.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.updateCount == 0)
            {
                base.OnPaint(e);
            }
        }


        // Fields
        private int updateCount;
    }
}

