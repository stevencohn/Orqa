namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public class PositionChangedEventArgs : EventArgs
    {
        // Methods
        public PositionChangedEventArgs(UpdateReason AReason, int ADeltaX, int ADeltaY)
        {
            this.Reason = AReason;
            this.DeltaX = ADeltaX;
            this.DeltaY = ADeltaY;
        }


        // Fields
        public int DeltaX;
        public int DeltaY;
        public UpdateReason Reason;
    }
}

