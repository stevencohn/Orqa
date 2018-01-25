namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface IFormatText
    {
        // Events
        event EventHandler TextReparsed;

        // Methods
        void BlockDeleting(Rectangle Rect);

        void CodeCompletion(string Text, Point Position, CodeCompletionArgs e);

        IRange GetBlock(Point Position);

        int GetSmartIndent(int Line);

        bool IsBlockEnd(string Text);

        bool IsBlockStart(string Text);

        int Outline(IList Ranges);

        void PositionChanged(int X, int Y, int DeltaX, int DeltaY, UpdateReason Reason);

        void ReparseText();

        void ResetOptions();


        // Properties
        string[] Lines { get; set; }

        FormatTextOptions Options { get; set; }

        IStringList Strings { get; set; }

        IUnitInfo UnitInfo { get; }

    }
}

