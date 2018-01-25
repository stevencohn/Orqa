namespace River.Orqa.Editor
{
    using System;

    public interface IUndo
    {
        // Methods
        int BeginUndoUpdate();

        bool CanRedo();

        bool CanUndo();

        void ClearRedo();

        void ClearUndo();

        int DisableUndo();

        int EnableUndo();

        int EndUndoUpdate();

        void Redo();

        void ResetUndoLimit();

        void ResetUndoOptions();

        void Undo();


        // Properties
        int UndoLimit { get; set; }

        River.Orqa.Editor.UndoOptions UndoOptions { get; set; }

        int UndoUpdateCount { get; }

    }
}

