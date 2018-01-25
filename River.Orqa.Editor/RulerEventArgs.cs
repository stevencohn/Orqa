namespace River.Orqa.Editor
{
    using System;

    public class RulerEventArgs : EventArgs
    {
        // Methods
        public RulerEventArgs(object AObject)
        {
            this.Object = AObject;
        }


        // Fields
        public object Object;
    }
}

