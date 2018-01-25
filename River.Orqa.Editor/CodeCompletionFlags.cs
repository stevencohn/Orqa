namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum CodeCompletionFlags
    {
        // Fields
        AcceptOnClick = 0x10,
        AcceptOnDblClick = 0x20,
        AcceptOnDelimiter = 0x100,
        AcceptOnEnter = 2,
        CloseOnEscape = 1,
        CloseOnLostFocus = 8,
        CloseOnMouseLeave = 4,
        FeetToScreen = 0x40,
        KeepActive = 0x80,
        None = 0
    }
}

