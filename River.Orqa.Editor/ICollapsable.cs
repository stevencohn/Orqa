namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.Drawing;

    public interface ICollapsable
    {
        // Methods
        void Collapse(int Index);

        void EnsureExpanded(Point Point);

        void EnsureExpanded(int Index);

        void Expand(int Index);

        string GetOutlineHint(IOutlineRange Range);

        IOutlineRange GetOutlineRange(Point Point);

        IOutlineRange GetOutlineRange(int Index);

        int GetOutlineRanges(IList Ranges);

        int GetOutlineRanges(IList Ranges, Point Point);

        int GetOutlineRanges(IList Ranges, int Index);

        int GetOutlineRanges(IList Ranges, Point StartPoint, Point EndPoint);

        bool IsCollapsed(int Index);

        bool IsExpanded(int Index);

        bool IsVisible(Point Point);

        bool IsVisible(int Index);

        IOutlineRange Outline(Point StartPoint, Point EndPoint);

        IOutlineRange Outline(int First, int Last);

        IOutlineRange Outline(Point StartPoint, Point EndPoint, int Level);

        IOutlineRange Outline(Point StartPoint, Point EndPoint, string OutlineText);

        IOutlineRange Outline(int First, int Last, int Level);

        IOutlineRange Outline(int First, int Last, string OutlineText);

        IOutlineRange Outline(Point StartPoint, Point EndPoint, int Level, string OutlineText);

        IOutlineRange Outline(int First, int Last, int Level, string OutlineText);

        void ResetAllowOutlining();

        void ResetOutlineOptions();

        void SetOutlineRanges(IList Ranges);

        void SetOutlineRanges(IList Ranges, bool PreserveVisible);

        void ToggleOutlining();

        void UnOutline();

        void UnOutline(Point Point);

        void UnOutline(int Index);


        // Properties
        bool AllowOutlining { get; set; }

        River.Orqa.Editor.OutlineOptions OutlineOptions { get; set; }

    }
}

