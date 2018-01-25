namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface IEditRuler : IControlProps
    {
        // Events
        event EventHandler Change;

        // Methods
        void Assign(IEditRuler Source);

        void CancelDragging();

        void ResetIndentBackColor();

        void ResetOptions();

        void ResetUnits();


        // Properties
        Color IndentBackColor { get; set; }

        bool IsDragging { get; }

        int MarkWidth { get; set; }

        RulerOptions Options { get; set; }

        int PageStart { get; set; }

        int PageWidth { get; set; }

        int RulerStart { get; set; }

        int RulerWidth { get; set; }

        RulerUnits Units { get; set; }

        bool Vertical { get; set; }

    }
}

