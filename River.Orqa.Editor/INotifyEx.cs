namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;

    public interface INotifyEx : INotify
    {
        // Properties
        int FirstChanged { get; }

        int LastChanged { get; }

    }
}

