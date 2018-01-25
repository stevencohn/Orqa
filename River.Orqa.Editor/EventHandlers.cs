namespace River.Orqa.Editor
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class EventHandlers
    {
        // Methods
        public EventHandlers(ISyntaxEdit Owner)
        {
            this.owner = Owner;
            ISelection selection1 = this.owner.Selection;
            this.tabifyEvent = new KeyEvent(selection1.Tabify);
            this.unTabifyEvent = new KeyEvent(selection1.UnTabify);
            this.lowerCaseEvent = new KeyEvent(selection1.LowerCase);
            this.upperCaseEvent = new KeyEvent(selection1.UpperCase);
            this.capitalizeEvent = new KeyEvent(selection1.Capitalize);
            this.deleteWhiteSpaceEvent = new KeyEvent(selection1.DeleteWhiteSpace);
            this.moveCharLeftEvent = new KeyEvent(this.owner.MoveCharLeft);
            this.moveCharRightEvent = new KeyEvent(this.owner.MoveCharRight);
            this.moveLineUpEvent = new KeyEvent(this.owner.MoveLineUp);
            this.moveLineDownEvent = new KeyEvent(this.owner.MoveLineDown);
            this.moveWordLeftEvent = new KeyEvent(this.owner.MoveWordLeft);
            this.moveWordRightEvent = new KeyEvent(this.owner.MoveWordRight);
            this.movePageUpEvent = new KeyEvent(this.owner.MovePageUp);
            this.movePageDownEvent = new KeyEvent(this.owner.MovePageDown);
            this.moveScreenTopEvent = new KeyEvent(this.owner.MoveScreenTop);
            this.moveScreenBottomEvent = new KeyEvent(this.owner.MoveScreenBottom);
            this.moveLineBeginEvent = new KeyEvent(this.MoveLineBegin);
            this.moveLineEndEvent = new KeyEvent(this.owner.MoveLineEnd);
            this.moveFileBeginEvent = new KeyEvent(this.owner.MoveFileBegin);
            this.moveFileEndEvent = new KeyEvent(this.owner.MoveFileEnd);
            this.selectCharLeftEvent = new KeyEventEx(this.SelectCharLeft);
            this.selectCharRightEvent = new KeyEventEx(this.SelectCharRight);
            this.selectLineUpEvent = new KeyEventEx(this.SelectLineUp);
            this.selectLineDownEvent = new KeyEventEx(this.SelectLineDown);
            this.selectWordLeftEvent = new KeyEventEx(this.SelectWordLeft);
            this.selectWordRightEvent = new KeyEventEx(this.SelectWordRight);
            this.selectPageUpEvent = new KeyEventEx(this.SelectPageUp);
            this.selectPageDownEvent = new KeyEventEx(this.SelectPageDown);
            this.selectScreenTopEvent = new KeyEventEx(this.SelectScreenTop);
            this.selectScreenBottomEvent = new KeyEventEx(this.SelectScreenBottom);
            this.selectLineBeginEvent = new KeyEventEx(this.SelectLineBegin);
            this.selectLineEndEvent = new KeyEventEx(this.SelectLineEnd);
            this.selectFileBeginEvent = new KeyEventEx(this.SelectFileBegin);
            this.selectFileEndEvent = new KeyEventEx(this.SelectFileEnd);
            this.selectAllEvent = new KeyEvent(selection1.SelectAll);
            this.toggleBookMarkEvent = new KeyEvent(this.ToggleBookMark);
            this.nextBookMarkEvent = new KeyEvent(this.GotoNextBookMark);
            this.prevBookMarkEvent = new KeyEvent(this.GotoPrevBookMark);
            this.clearBookMarkEvent = new KeyEvent(this.ClearAllBookMarks);
            this.toggleBookMarkEventEx = new KeyEventEx(this.ToggleBookMark);
            this.moveBookMarkEvent = new KeyEventEx(this.GotoBookMark);
            this.deleteLeftEvent = new KeyEvent(this.DeleteLeft);
            this.deleteWordLeftEvent = new KeyEvent(selection1.DeleteWordLeft);
            this.deleteWordRightEvent = new KeyEvent(selection1.DeleteWordRight);
            this.deleteRightEvent = new KeyEvent(selection1.DeleteRight);
            this.newLineEvent = new KeyEvent(selection1.NewLine);
            this.processEscapeEvent = new KeyEvent(this.ProcessEscape);
            this.processTabEvent = new KeyEvent(selection1.ProcessTab);
            this.processShiftTabEvent = new KeyEvent(selection1.ProcessShiftTab);
            this.toggleOverwriteEvent = new KeyEvent(selection1.ToggleOverWrite);
            IOutlining outlining1 = this.owner.Outlining;
            this.toggleOutliningEvent = new KeyEvent(outlining1.ToggleOutlining);
            this.toggleOutliningSelectionEvent = new KeyEvent(selection1.ToggleOutlining);
            this.swapAnchorEvent = new KeyEvent(selection1.SwapAnchor);
            this.undoEvent = new KeyEvent(this.Undo);
            this.redoEvent = new KeyEvent(this.Redo);
            this.findEvent = new KeyEvent(this.Find);
            this.findNextEvent = new KeyEvent(this.FindNext);
            this.findPreviousEvent = new KeyEvent(this.FindPrevious);
            this.findNextSelectedEvent = new KeyEvent(this.FindNextSelected);
            this.findPreviousSelectedEvent = new KeyEvent(this.FindPreviousSelected);
            this.replaceEvent = new KeyEvent(this.Replace);
            this.gotolineEvent = new KeyEvent(this.GotoLine);
            this.initIncrementalSearchEvent = new KeyEvent(this.owner.StartIncrementalSearch);
            this.initReverseIncrementalSearchEvent = new KeyEvent(this.StartReverseIncrementalSearch);
            this.cutEvent = new KeyEvent(selection1.Cut);
            this.copyEvent = new KeyEvent(selection1.Copy);
            this.pasteEvent = new KeyEvent(selection1.Paste);
            this.completeWordEvent = new KeyEvent(this.owner.CompleteWord);
            this.listMembersEvent = new KeyEvent(this.owner.ListMembers);
            this.quickInfoEvent = new KeyEvent(this.owner.QuickInfo);
            this.parameterInfoEvent = new KeyEvent(this.owner.ParameterInfo);
            this.formatSelectionEvent = new KeyEvent(selection1.SmartFormat);
            this.formatDocumentEvent = new KeyEvent(selection1.SmartFormatDocument);
            this.charTransposeEvent = new KeyEvent(selection1.CharTransponse);
            this.wordTransposeEvent = new KeyEvent(selection1.WordTransponse);
            this.lineTransposeEvent = new KeyEvent(selection1.LineTransponse);
            this.toggleHiddenTextEvent = new KeyEvent(this.ToggleHiddenText);
            this.toggleMatchCaseEvent = new KeyEvent(this.ToggleMatchCase);
            this.toggleRegularExpressionsEvent = new KeyEvent(this.ToggleRegularExpressions);
            this.toggleSearchUpEvent = new KeyEvent(this.ToggleSearchUp);
            this.toggleWholeWordEvent = new KeyEvent(this.ToggleWholeWord);
            this.scrollLineUpEvent = new KeyEvent(this.owner.ScrollLineUp);
            this.scrollLineDownEvent = new KeyEvent(this.owner.ScrollLineDown);
            this.moveToOpenBraceEvent = new KeyEvent(this.owner.MoveToOpenBrace);
            this.moveToOpenBraceExtendEvent = new KeyEvent(selection1.SelectToOpenBrace);
            this.moveToCloseBraceEvent = new KeyEvent(this.owner.MoveToCloseBrace);
            this.moveToCloseBraceExtendEvent = new KeyEvent(selection1.SelectToCloseBrace);
            this.newLineAboveEvent = new KeyEvent(selection1.NewLineAbove);
            this.newLineBelowEvent = new KeyEvent(selection1.NewLineBelow);
            this.selectWordEvent = new KeyEvent(selection1.SelectWord);
            this.lineCutEvent = new KeyEvent(selection1.CutLine);
            this.lineDeleteEvent = new KeyEvent(selection1.DeleteLine);
        }

        public void ClearAllBookMarks()
        {
            this.owner.Source.BookMarks.ClearAllBookMarks();
        }

        public void DeleteLeft()
        {
            if (this.owner.InIncrementalSearch && (this.owner.IncrementalSearchString != string.Empty))
            {
                this.owner.IncrementalSearch(string.Empty, true);
            }
            else
            {
                this.owner.Selection.DeleteLeft();
            }
        }

        public void Find()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.Execute(this.owner, false, false);
            }
        }

        public void FindNext()
        {
            this.owner.FindNext();
        }

        public void FindNextSelected()
        {
            this.owner.FindNextSelected();
        }

        public void FindPrevious()
        {
            this.owner.FindPrevious();
        }

        public void FindPreviousSelected()
        {
            this.owner.FindPreviousSelected();
        }

        public void GotoBookMark(object Param)
        {
            this.owner.Source.BookMarks.GotoBookMark((int) Param);
        }

        public void GotoLine()
        {
            int num1 = this.owner.Position.Y;
            if ((this.owner.GotoLineDialog != null) && (this.owner.GotoLineDialog.Execute(this.owner, this.owner.Lines.Count, ref num1) == DialogResult.OK))
            {
                this.owner.MoveToLine(num1);
            }
        }

        public void GotoNextBookMark()
        {
            this.owner.Source.BookMarks.GotoNextBookMark();
        }

        public void GotoPrevBookMark()
        {
            this.owner.Source.BookMarks.GotoPrevBookMark();
        }

        public void MoveLineBegin()
        {
            Point point1 = this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position);
            string text1 = this.owner.DisplayLines[point1.Y];
            int num1 = text1.Length - text1.TrimStart(new char[0]).Length;
            point1.X = (point1.X == num1) ? 0 : num1;
            this.owner.MoveTo(this.owner.DisplayLines.DisplayPointToPoint(point1));
        }

        public void ProcessEscape()
        {
            if (this.owner.InIncrementalSearch)
            {
                this.owner.FinishIncrementalSearch();
            }
            else
            {
                this.owner.Selection.ProcessEscape();
            }
        }

        public void Redo()
        {
            this.owner.Source.Redo();
        }

        public void Replace()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.Execute(this.owner, false, true);
            }
        }

        public void SelectCharLeft(object SelectionType)
        {
            this.owner.Selection.SelectCharLeft((SelectionType) SelectionType);
        }

        public void SelectCharRight(object SelectionType)
        {
            this.owner.Selection.SelectCharRight((SelectionType) SelectionType);
        }

        public void SelectFileBegin(object SelectionType)
        {
            this.owner.Selection.SelectFileBegin((SelectionType) SelectionType);
        }

        public void SelectFileEnd(object SelectionType)
        {
            this.owner.Selection.SelectFileEnd((SelectionType) SelectionType);
        }

        public void SelectLineBegin(object SelectionType)
        {
            this.owner.Selection.SelectLineBegin((SelectionType) SelectionType);
        }

        public void SelectLineDown(object SelectionType)
        {
            this.owner.Selection.SelectLineDown((SelectionType) SelectionType);
        }

        public void SelectLineEnd(object SelectionType)
        {
            this.owner.Selection.SelectLineEnd((SelectionType) SelectionType);
        }

        public void SelectLineUp(object SelectionType)
        {
            this.owner.Selection.SelectLineUp((SelectionType) SelectionType);
        }

        public void SelectPageDown(object SelectionType)
        {
            this.owner.Selection.SelectPageDown((SelectionType) SelectionType);
        }

        public void SelectPageUp(object SelectionType)
        {
            this.owner.Selection.SelectPageUp((SelectionType) SelectionType);
        }

        public void SelectScreenBottom(object SelectionType)
        {
            this.owner.Selection.SelectScreenBottom((SelectionType) SelectionType);
        }

        public void SelectScreenTop(object SelectionType)
        {
            this.owner.Selection.SelectScreenTop((SelectionType) SelectionType);
        }

        public void SelectWordLeft(object SelectionType)
        {
            this.owner.Selection.SelectWordLeft((SelectionType) SelectionType);
        }

        public void SelectWordRight(object SelectionType)
        {
            this.owner.Selection.SelectWordRight((SelectionType) SelectionType);
        }

        public void StartReverseIncrementalSearch()
        {
            this.owner.StartIncrementalSearch(true);
        }

        public void ToggleBookMark()
        {
            this.owner.Source.BookMarks.ToggleBookMark();
        }

        public void ToggleBookMark(object Param)
        {
            this.owner.Source.BookMarks.ToggleBookMark((int) Param);
        }

        public void ToggleHiddenText()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleHiddenText();
            }
        }

        public void ToggleMatchCase()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleMatchCase();
            }
        }

        public void ToggleRegularExpressions()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleRegularExpressions();
            }
        }

        public void ToggleSearchUp()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleSearchUp();
            }
        }

        public void ToggleWholeWord()
        {
            if (this.owner.SearchDialog != null)
            {
                this.owner.SearchDialog.ToggleWholeWord();
            }
        }

        public void Undo()
        {
            this.owner.Source.Undo();
        }


        // Fields
        public KeyEvent capitalizeEvent;
        public KeyEvent charTransposeEvent;
        public KeyEvent clearBookMarkEvent;
        public KeyEvent completeWordEvent;
        public KeyEvent copyEvent;
        public KeyEvent cutEvent;
        public KeyEvent deleteLeftEvent;
        public KeyEvent deleteRightEvent;
        public KeyEvent deleteWhiteSpaceEvent;
        public KeyEvent deleteWordLeftEvent;
        public KeyEvent deleteWordRightEvent;
        public KeyEvent findEvent;
        public KeyEvent findNextEvent;
        public KeyEvent findNextSelectedEvent;
        public KeyEvent findPreviousEvent;
        public KeyEvent findPreviousSelectedEvent;
        public KeyEvent formatDocumentEvent;
        public KeyEvent formatSelectionEvent;
        public KeyEvent gotolineEvent;
        public KeyEvent initIncrementalSearchEvent;
        public KeyEvent initReverseIncrementalSearchEvent;
        public KeyEvent lineCutEvent;
        public KeyEvent lineDeleteEvent;
        public KeyEvent lineTransposeEvent;
        public KeyEvent listMembersEvent;
        public KeyEvent lowerCaseEvent;
        public KeyEventEx moveBookMarkEvent;
        public KeyEvent moveCharLeftEvent;
        public KeyEvent moveCharRightEvent;
        public KeyEvent moveFileBeginEvent;
        public KeyEvent moveFileEndEvent;
        public KeyEvent moveLineBeginEvent;
        public KeyEvent moveLineDownEvent;
        public KeyEvent moveLineEndEvent;
        public KeyEvent moveLineUpEvent;
        public KeyEvent movePageDownEvent;
        public KeyEvent movePageUpEvent;
        public KeyEvent moveScreenBottomEvent;
        public KeyEvent moveScreenTopEvent;
        public KeyEvent moveToCloseBraceEvent;
        public KeyEvent moveToCloseBraceExtendEvent;
        public KeyEvent moveToOpenBraceEvent;
        public KeyEvent moveToOpenBraceExtendEvent;
        public KeyEvent moveWordLeftEvent;
        public KeyEvent moveWordRightEvent;
        public KeyEvent newLineAboveEvent;
        public KeyEvent newLineBelowEvent;
        public KeyEvent newLineEvent;
        public KeyEvent nextBookMarkEvent;
        private ISyntaxEdit owner;
        public KeyEvent parameterInfoEvent;
        public KeyEvent pasteEvent;
        public KeyEvent prevBookMarkEvent;
        public KeyEvent processEscapeEvent;
        public KeyEvent processShiftTabEvent;
        public KeyEvent processTabEvent;
        public KeyEvent quickInfoEvent;
        public KeyEvent redoEvent;
        public KeyEvent replaceEvent;
        public KeyEvent scrollLineDownEvent;
        public KeyEvent scrollLineUpEvent;
        public KeyEvent selectAllEvent;
        public KeyEventEx selectCharLeftEvent;
        public KeyEventEx selectCharRightEvent;
        public KeyEventEx selectFileBeginEvent;
        public KeyEventEx selectFileEndEvent;
        public KeyEventEx selectLineBeginEvent;
        public KeyEventEx selectLineDownEvent;
        public KeyEventEx selectLineEndEvent;
        public KeyEventEx selectLineUpEvent;
        public KeyEventEx selectPageDownEvent;
        public KeyEventEx selectPageUpEvent;
        public KeyEventEx selectScreenBottomEvent;
        public KeyEventEx selectScreenTopEvent;
        public KeyEvent selectWordEvent;
        public KeyEventEx selectWordLeftEvent;
        public KeyEventEx selectWordRightEvent;
        public KeyEvent swapAnchorEvent;
        public KeyEvent tabifyEvent;
        public KeyEvent toggleBookMarkEvent;
        public KeyEventEx toggleBookMarkEventEx;
        public KeyEvent toggleHiddenTextEvent;
        public KeyEvent toggleMatchCaseEvent;
        public KeyEvent toggleOutliningEvent;
        public KeyEvent toggleOutliningSelectionEvent;
        public KeyEvent toggleOverwriteEvent;
        public KeyEvent toggleRegularExpressionsEvent;
        public KeyEvent toggleSearchUpEvent;
        public KeyEvent toggleWholeWordEvent;
        public KeyEvent undoEvent;
        public KeyEvent unTabifyEvent;
        public KeyEvent upperCaseEvent;
        public KeyEvent wordTransposeEvent;
    }
}

