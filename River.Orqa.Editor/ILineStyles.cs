namespace River.Orqa.Editor
{
    using System;
    using System.Collections;

    public interface ILineStyles : IList, ICollection, IEnumerable
    {
        // Methods
        void Assign(ILineStyles Source);

        int GetLineStyle(int Index);

        void RemoveLineStyle(int Line);

        void SetLineStyle(int Index, int Style);

        void ToggleLineStyle(int Line, int Style);

    }
}

