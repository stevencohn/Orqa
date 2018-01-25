namespace River.Orqa.Editor
{
    using System;

    public class NotifyEventArgs : EventArgs
    {
        // Methods
        public NotifyEventArgs()
        {
        }

        public NotifyEventArgs(NotifyState AState, int AFirst, int ALast, bool AUpdate)
        {
            this.State = AState;
            this.FirstChanged = AFirst;
            this.LastChanged = ALast;
            this.Update = AUpdate;
        }


        // Fields
        public int FirstChanged;
        public int LastChanged;
        public NotifyState State;
        public bool Update;
    }
}

