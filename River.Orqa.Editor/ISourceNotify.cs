namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public interface ISourceNotify : INotifyEx, INotify
    {
        // Methods
        int BeginUpdate(UpdateReason Reason);

        void LinesChanged(int First, int Last);

        void LinesChanged(int First, int Last, bool Modified);


        // Properties
        NotifyState State { get; set; }

    }
}

