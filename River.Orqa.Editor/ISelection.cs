namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface ISelection
    {
        // Events
        event EventHandler SelectionChanged;

        // Methods
        void Assign(ISelection Source);

        int BeginUpdate();

        bool CanCopy();

        bool CanCut();

        bool CanDrag(Point Position);

        bool CanPaste();

        void Capitalize();

        void ChangeBlock(StringEvent Action);

        void ChangeBlock(StringEvent Action, bool ChangeIfEmpty, bool ExtendFirstLine);

        void CharTransponse();

        void Clear();

        void Copy();

        void Cut();

        void CutLine();

        void Delete();

        void DeleteLeft();

        void DeleteLine();

        void DeleteRight();

        void DeleteWhiteSpace();

        void DeleteWordLeft();

        void DeleteWordRight();

        void DragTo(Point Position, bool DeleteOrigin);

        int EndUpdate();

        bool GetSelectionForLine(int Index, out int Left, out int Right);

        void Indent();

        void InsertString(string String);

        bool IsEmpty();

        bool IsPosInSelection(Point Position);

        bool IsPosInSelection(int X, int Y);

        void LineTransponse();

        void LowerCase();

        bool Move(Point Position, bool DeleteOrigin);

        void NewLine();

        void NewLineAbove();

        void NewLineBelow();

        void Paste();

        void ProcessEscape();

        void ProcessShiftTab();

        void ProcessTab();

        void ResetAllowedSelectionMode();

        void ResetBackColor();

        void ResetForeColor();

        void ResetInActiveBackColor();

        void ResetInActiveForeColor();

        void ResetOptions();

        void SelectAll();

        void SelectCharLeft();

        void SelectCharLeft(River.Orqa.Editor.SelectionType SelectionType);

        void SelectCharRight();

        void SelectCharRight(River.Orqa.Editor.SelectionType SelectionType);

        int SelectedCount();

        string SelectedString(int Index);

        void SelectFileBegin();

        void SelectFileBegin(River.Orqa.Editor.SelectionType SelectionType);

        void SelectFileEnd();

        void SelectFileEnd(River.Orqa.Editor.SelectionType SelectionType);

		Point SelectionToTextPoint (Point Position);

        void SelectLine();

        void SelectLineBegin();

        void SelectLineBegin(River.Orqa.Editor.SelectionType SelectionType);

        void SelectLineDown();

        void SelectLineDown(River.Orqa.Editor.SelectionType SelectionType);

        void SelectLineEnd();

        void SelectLineEnd(River.Orqa.Editor.SelectionType SelectionType);

        void SelectLineUp();

        void SelectLineUp(River.Orqa.Editor.SelectionType SelectionType);

        void SelectPageDown();

        void SelectPageDown(River.Orqa.Editor.SelectionType SelectionType);

        void SelectPageUp();

        void SelectPageUp(River.Orqa.Editor.SelectionType SelectionType);

        void SelectScreenBottom();

        void SelectScreenBottom(River.Orqa.Editor.SelectionType SelectionType);

        void SelectScreenTop();

        void SelectScreenTop(River.Orqa.Editor.SelectionType SelectionType);

        void SelectToCloseBrace();

        void SelectToOpenBrace();

        void SelectWord();

        void SelectWordLeft();

        void SelectWordLeft(River.Orqa.Editor.SelectionType SelectionType);

        void SelectWordRight();

        void SelectWordRight(River.Orqa.Editor.SelectionType SelectionType);

        void SetSelection(River.Orqa.Editor.SelectionType SelectionType, Rectangle SelectionRect);

        void SetSelection(River.Orqa.Editor.SelectionType SelectionType, Point SelectionStart, Point SelectionEnd);

        void SmartFormat();

        void SmartFormatBlock();

        void SmartFormatDocument();

        void SwapAnchor();

        void Tabify();

        Point TextToSelectionPoint(Point Position);

        void ToggleOutlining();

        void ToggleOverWrite();

        void UnIndent();

        void UnTabify();

        void UpperCase();

        void WordTransponse();


        // Properties
        River.Orqa.Editor.AllowedSelectionMode AllowedSelectionMode { get; set; }

        Color BackColor { get; set; }

        Color ForeColor { get; set; }

        Color InActiveBackColor { get; set; }

        Color InActiveForeColor { get; set; }

        SelectionOptions Options { get; set; }

        string SelectedText { get; set; }

        int SelectionLength { get; set; }

        Rectangle SelectionRect { get; set; }

		int SelectionStart { get; set; }

		SelectionState SelectionState { get; set; }

		River.Orqa.Editor.SelectionType SelectionType { get; set; }

        int UpdateCount { get; }

    }
}

