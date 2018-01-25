namespace River.Orqa.Editor
{
    using System;

    public interface ICodeCompletionBox : ICodeCompletionWindow, IControlProps
    {
        // Methods
        ICodeCompletionColumn AddColumn();

        void ClearColumns();

        ICodeCompletionColumn InsertColumn(int Index);

        void RemoveColumnAt(int Index);

        void ResetDropDownCount();


        // Properties
        ICodeCompletionColumn[] Columns { get; }

        int DropDownCount { get; set; }

        string Filter { get; set; }

        bool Filtered { get; set; }

    }
}

