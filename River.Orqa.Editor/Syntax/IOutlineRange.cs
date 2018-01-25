namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;

    public interface IOutlineRange : IRange
    {
        // Properties
        string DisplayText { get; }

        int Level { get; set; }

        string Text { get; }

        bool Visible { get; set; }

    }
}

