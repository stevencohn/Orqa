namespace River.Orqa.Editor
{
    using System;

    public interface IWordWrap
    {
        // Methods
        int GetWrapMargin();

        void ResetWordWrap();

        void ResetWrapAtMargin();

        bool UpdateWordWrap();

        bool UpdateWordWrap(int First, int Last);


        // Properties
        bool WordWrap { get; set; }

        bool WrapAtMargin { get; set; }

        int WrapMargin { get; }

    }
}

