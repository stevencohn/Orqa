namespace River.Orqa.Editor
{
    using System;

    [Flags]
    public enum SelectionOptions
    {
        // Fields
        DeselectOnCopy = 0x40,
        DisableDragging = 2,
        DisableSelection = 1,
        HideSelection = 0x10,
        None = 0,
        OverwriteBlocks = 0x100,
        PersistentBlocks = 0x80,
        SelectBeyondEol = 4,
        SelectLineOnDblClick = 0x20,
        SmartFormat = 0x200,
        UseColors = 8
    }
}

