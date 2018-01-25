namespace River.Orqa.Editor.Syntax
{
    using System;

    public interface INotify
    {
        // Methods
        void AddNotifier(INotifier sender);

        int BeginUpdate();

        int EndUpdate();

        void Notify();

        void RemoveNotifier(INotifier sender);


        // Properties
        int UpdateCount { get; }

    }
}

