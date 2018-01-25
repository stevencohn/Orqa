namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;

    public interface IWhiteSpace
    {
        // Methods
        void Assign(IWhiteSpace Source);

        void ResetEofSymbol();

        void ResetEolSymbol();

        void ResetSpaceSymbol();

        void ResetSymbolColor();

        void ResetTabSymbol();

        void ResetVisible();


        // Properties
        string EofString { get; }

        char EofSymbol { get; set; }

        string EolString { get; }

        char EolSymbol { get; set; }

        string SpaceString { get; }

        char SpaceSymbol { get; set; }

        Color SymbolColor { get; set; }

        string TabString { get; }

        char TabSymbol { get; set; }

        bool Visible { get; set; }

    }
}

