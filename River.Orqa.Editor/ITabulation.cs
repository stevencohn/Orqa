namespace River.Orqa.Editor
{
    using System;

    public interface ITabulation
    {
        // Methods
        void ResetTabStops();

        void ResetUseSpaces();


        // Properties
        int[] TabStops { get; set; }

        bool UseSpaces { get; set; }

    }
}

