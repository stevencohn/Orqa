namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Gutter : IGutter
    {
        // Events
        [Browsable(false)]
        public event EventHandler Click;
        [Browsable(false)]
        public event EventHandler DoubleClick;

        // Methods
        public Gutter()
        {
            this.visible = true;
            this.width = EditConsts.DefaultGutterWidth;
            this.lineNumbersAlignment = StringAlignment.Near;
            this.lineNumbersStart = EditConsts.DefaultLineNumbersStart;
            this.lineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;
            this.lineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;
            this.bookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;
            this.wrapImageIndex = EditConsts.DefaultWrapImageIndex;
			this.lineBookmarksColor = EditConsts.DefaultLineBookmarksColor;
            this.drawInfo = new DrawInfo();
            this.brush = new SolidBrush(EditConsts.DefaultGutterBackColor);
            this.pen = new System.Drawing.Pen(EditConsts.DefaultGutterForeColor, 1f);
            this.internalImages = new ImageList();
            this.internalImages.ImageSize = new Size(15, 15);
            this.ranges = new ArrayList();
            this.options = EditConsts.DefaultGutterOptions;
            this.lineNumbersForeColor = EditConsts.DefaultLineNumbersForeColor;
            this.lineNumbersBackColor = EditConsts.DefaultLineNumbersBackColor;
            try
            {
                ResourceManager manager1 = new ResourceManager(typeof(Resources));
                this.internalImages.ImageStream = (ImageListStreamer) manager1.GetObject("SyntaxEdit.Gutter.Images.ImageStream");
            }
            catch
            {
            }
            this.internalImages.TransparentColor = EditConsts.DefaultTransparentColor;
        }

        public Gutter(ISyntaxEdit Owner) : this()
        {
            this.owner = Owner;
        }

        private bool AllowOutlining()
        {
            if (this.owner != null)
            {
                return this.owner.Outlining.AllowOutlining;
            }
            return false;
        }

        public void Assign(IGutter Source)
        {
            this.BeginUpdate();
            try
            {
                this.width = Source.Width;
                if (Source.Brush is SolidBrush)
                {
                    ((SolidBrush) this.brush).Color = ((SolidBrush) Source.Brush).Color;
                }
                this.Pen.Color = Source.Pen.Color;
                this.Pen.Width = Source.Pen.Width;
                this.Visible = Source.Visible;
                this.LineNumbersStart = Source.LineNumbersStart;
                this.LineNumbersLeftIndent = Source.LineNumbersLeftIndent;
                this.LineNumbersRightIndent = Source.LineNumbersRightIndent;
                this.LineNumbersForeColor = Source.LineNumbersForeColor;
                this.LineNumbersBackColor = Source.LineNumbersBackColor;
                this.LineNumbersAlignment = Source.LineNumbersAlignment;
                this.Options = Source.Options;
                this.BookMarkImageIndex = Source.BookMarkImageIndex;
                this.WrapImageIndex = Source.WrapImageIndex;
                if (this.images == null)
                {
                    this.Images.ImageStream = Source.Images.ImageStream;
                }
                else
                {
                    this.Images = Source.Images;
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected void BeginUpdate()
        {
            this.updateCount++;
        }

        protected internal bool CanDrawImage(int Index, int Left, int Right)
        {
            if ((Index >= 0) && (Index < this.Images.Images.Count))
            {
                return ((Left + this.Images.ImageSize.Width) <= Right);
            }
            return false;
        }

        private void CenterOutlineRect(ref int L, ref int T, int W)
        {
            int num1 = EditConsts.DefaultCollasedImageWidth;
            int num2 = (W - num1) >> 1;
            if (num2 > 0)
            {
                L += num2;
                T += num2;
            }
        }

        private int CompareRange(Point Pt, Point sPt, Point ePt)
        {
            if ((Pt.Y < sPt.Y) || ((Pt.Y == sPt.Y) && (Pt.X < sPt.X)))
            {
                return 1;
            }
            if ((Pt.Y <= ePt.Y) && (((Pt.Y != ePt.Y) || (Pt.X < ePt.X)) || (ePt.X == 0x7fffffff)))
            {
                return 0;
            }
            return -1;
        }

        private IntPtr CreateDC(IntPtr RefHandle, Rectangle R, out IntPtr Bitmap, out IntPtr OldBitmap)
        {
            IntPtr ptr1 = Win32.CreateCompatibleDC(RefHandle);
            Bitmap = Win32.CreateCompatibleBitmap(RefHandle, this.Rect.Width, this.Rect.Height);
            OldBitmap = Win32.SelectObject(ptr1, Bitmap);
            return ptr1;
        }

        private void DrawGutter(ITextPainter Painter, Rectangle Rect, int StartLine)
        {
            int num1 = -1;
            this.DrawGutter(Painter, Rect, false, StartLine, 0, ref num1);
        }

        private void DrawGutter(ITextPainter Painter, Rectangle Rect, bool CalcIndex, int Line, int ImageX, ref int ImageIndex)
        {
            int num1 = Rect.Top;
            int num2 = this.owner.Painter.FontHeight;
            bool flag1 = (this.owner.Outlining.OutlineOptions & OutlineOptions.DrawLines) != OutlineOptions.None;
            bool flag2 = this.owner.Outlining.AllowOutlining;
            bool flag3 = (this.options & GutterOptions.PaintLinesBeyondEof) != GutterOptions.None;
            bool flag4 = this.NeedLineNumbers();
            bool flag5 = (this.options & GutterOptions.PaintBookMarks) != GutterOptions.None;
            bool flag6 = flag4 && (this.visible & ((this.options & GutterOptions.PaintLinesOnGutter) != GutterOptions.None));
            int num3 = this.GetLineNumbersLeft(true);
            int num4 = this.GetLineNumbersRight(true);
            int num5 = this.GetOutlineLeft();
            int num6 = Rect.Left + this.GetPaintWidth(false);
            int num7 = this.owner.DisplayLines.GetCount();
            ArrayList list1 = new ArrayList();
            while ((num1 < Rect.Bottom) || CalcIndex)
            {
                Point point1 = this.owner.DisplayLines.DisplayPointToPoint(0, Line);
                if ((Line >= num7) && (!flag4 || !flag3))
                {
                    break;
                }
                if (flag4 && !CalcIndex)
                {
                    if (point1.X == 0)
                    {
                        this.DrawLineNumber(Painter, point1.Y, num3, num1, flag6);
                    }
                    else if (!this.Transparent)
                    {
                        Color color1 = Painter.BkColor;
                        try
                        {
                            Painter.BkColor = flag6 ? this.BrushColor : this.lineNumbersBackColor;
                            Painter.FillRectangle(num3, num1, num4 - num3, num2);
                        }
                        finally
                        {
                            Painter.BkColor = color1;
                        }
                    }
                }
                if (Line < num7)
                {
                    int num8 = 0;
                    if (this.Visible)
                    {
                        if ((this.owner.WordWrap && (point1.X != 0)) && ((this.wrapImageIndex >= 0) && this.CanDrawImage(this.wrapImageIndex, num8, num6)))
                        {
                            if ((CalcIndex && (ImageX >= num8)) && (ImageX < (num8 + this.Images.ImageSize.Width)))
                            {
                                ImageIndex = this.wrapImageIndex;
                                return;
                            }
                            num8 = this.DrawImage(Painter, this.wrapImageIndex, point1.Y, num8, num1, num6, !CalcIndex, this.Transparent || ((num8 >= num3) && ((num8 + this.Images.ImageSize.Width) <= num4)));
                        }
                        if (num1 >= (Rect.Top - this.Images.ImageSize.Height))
                        {
                            if (point1.X == 0)
                            {
                                int num9 = ((LineStyles) this.owner.Source.LineStyles).GetLineStyle(point1.Y);
                                if (((num9 >= 0) && (num9 < this.owner.LineStyles.Count)) && (!this.owner.Outlining.AllowOutlining || this.owner.Outlining.IsVisible(point1.Y)))
                                {
                                    num9 = ((LineStylesEx) this.owner.LineStyles).GetStyle(num9).ImageIndex;
                                    if (this.CanDrawImage(num9, num8, num6))
                                    {
                                        if ((CalcIndex && (ImageX >= num8)) && (ImageX < (num8 + this.Images.ImageSize.Width)))
                                        {
                                            ImageIndex = num9;
                                            return;
                                        }
                                        num8 += this.DrawImage(Painter, num9, point1.Y, num8, num1, num6, !CalcIndex, this.Transparent || ((num8 >= num3) && ((num8 + this.Images.ImageSize.Width) <= num4)));
                                    }
                                }
                            }
                            if (flag5)
                            {
                                Point point2 = ((DisplayStrings) this.owner.DisplayLines).DisplayPointToPoint(0x7fffffff, Line, true, false, false);
                                this.owner.Source.BookMarks.GetBookMarks(point1, point2, list1);
                                foreach (IBookMark mark1 in list1)
                                {
                                    if (this.owner.Outlining.AllowOutlining && !this.owner.Outlining.IsVisible(new Point(mark1.Char, mark1.Line)))
                                    {
                                        continue;
                                    }
                                    int num10 = mark1.Index;
                                    if (num10 == 0x7fffffff)
                                    {
                                        num10 = this.bookMarkImageIndex;
                                    }
                                    if (this.CanDrawImage(num10, num8, num6))
                                    {
                                        if ((CalcIndex && (ImageX >= num8)) && (ImageX < (num8 + this.Images.ImageSize.Width)))
                                        {
                                            ImageIndex = num10;
                                            return;
                                        }
                                        this.DrawImage(Painter, num10, point1.Y, num8, num1, num6, !CalcIndex, this.Transparent || ((num8 >= num3) && ((num8 + this.Images.ImageSize.Width) <= num4)));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (flag2 && !CalcIndex)
                    {
                        this.owner.Outlining.GetOutlineRanges(this.ranges, point1.Y);
                        DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
                        Point point3 = strings1.DisplayPointToPoint(0x7fffffff, Line, true, false, false);
                        bool flag7 = false;
                        bool flag8 = true;
                        foreach (IOutlineRange range1 in this.ranges)
                        {
                            if (this.CompareRange(range1.StartPoint, point1, point3) == 0)
                            {
                                flag8 &= range1.Visible;
                                flag7 = true;
                            }
                        }
                        if (flag7)
                        {
                            IRange range2 = (IRange) this.ranges[0];
                            this.DrawOutlineButton(Painter, point1.Y, num5, num1, num2, flag1, flag8, this.CompareRange(range2.StartPoint, point1, point3) > 0, this.CompareRange(range2.EndPoint, point1, point3) < 0);
                        }
                        if (!flag7 && flag1)
                        {
                            bool flag9 = false;
                            bool flag10 = false;
                            foreach (IOutlineRange range3 in this.ranges)
                            {
                                int num11 = this.CompareRange(range3.StartPoint, point1, point3);
                                int num12 = this.CompareRange(range3.EndPoint, point1, point3);
                                flag9 |= ((num11 > 0) && (num12 <= 0));
                                flag10 |= (num12 == 0);
                            }
                            if (flag9)
                            {
                                this.DrawOutline(Painter, point1.Y, num5, num1, num2, flag10);
                            }
                        }
                    }
                }
                if (CalcIndex)
                {
                    break;
                }
                num1 += num2;
                Line++;
            }
            if ((flag4 && !this.Transparent) && ((num1 > Rect.Top) && (num4 > num3)))
            {
                Painter.ExcludeClipRect(num3, Rect.Top, num4 - num3, num1 - Rect.Top);
            }
        }

        private int DrawImage(ITextPainter Painter, int ImageIndex, int Line, int Left, int Top, int Right, bool NeedDraw, bool Transparent)
        {
            if (!this.CanDrawImage(ImageIndex, Left, Right))
            {
                return 0;
            }
            if (NeedDraw)
            {
                this.drawInfo.Init();
                this.drawInfo.Line = Line;
                this.drawInfo.GutterImage = ImageIndex;
                Rectangle rectangle1 = new Rectangle(Left, Top, this.Images.ImageSize.Width, this.Images.ImageSize.Height);
                if (!this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.Before, DrawState.GutterImage | DrawState.Gutter, this.drawInfo))
                {
                    if (!Transparent)
                    {
                        Painter.FillRectangle(rectangle1);
                    }
                    Painter.DrawImage(this.Images, ImageIndex, rectangle1);
                    if (!Transparent)
                    {
                        Painter.ExcludeClipRect(rectangle1.Left, rectangle1.Top, rectangle1.Width, rectangle1.Height);
                    }
                }
                this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.After, DrawState.GutterImage | DrawState.Gutter, this.drawInfo);
            }
            return this.Images.ImageSize.Width;
        }

        private void DrawLine(ITextPainter Painter, int x1, int y1, int x2, int y2)
        {
            Painter.DrawLine(x1, y1, x2, y2);
            if (!this.Transparent)
            {
                if (x1 == x2)
                {
                    Painter.ExcludeClipRect(x1, y1, 1, y2 - y1);
                }
                else
                {
                    Painter.ExcludeClipRect(x1, y1, x2 - x1, 1);
                }
            }
        }

        private void DrawLineNumber(ITextPainter Painter, int Line, int Left, int Top, bool DrawOnGutter)
        {
            if (this.owner != null)
            {
                Rectangle rectangle1 = new Rectangle(Left, Top, this.lineNumberWidth - (this.lineNumbersLeftIndent + this.lineNumbersRightIndent), this.owner.Painter.FontHeight);
                this.drawInfo.Init();
                this.drawInfo.Line = Line;
                Color color1 = Painter.Color;
                Color color2 = Painter.BkColor;
                bool flag1 = this.Transparent;
                try
                {
                    string text1;
                    int num1;
                    int num3;
                    Painter.Color = this.lineNumbersForeColor;
                    Painter.BkColor = DrawOnGutter ? this.BrushColor : this.lineNumbersBackColor;
                    if (flag1)
                    {
                        Painter.BkMode = 1;
                    }
                    if (!this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.Before, DrawState.LineNumber | DrawState.Gutter, this.drawInfo))
                    {
                        int num4 = Line + this.lineNumbersStart;
                        text1 = num4.ToString();
                        num1 = flag1 ? 0 : 2;
                        switch (this.lineNumbersAlignment)
                        {
                            case StringAlignment.Near:
                            {
                                Painter.TextOut(text1, -1, rectangle1, num1);
                                goto Label_015A;
                            }
                            case StringAlignment.Center:
                            {
                                goto Label_0129;
                            }
                            case StringAlignment.Far:
                            {
                                int num2 = Painter.StringWidth(text1);
                                Painter.TextOut(text1, -1, rectangle1, (rectangle1.Left + rectangle1.Width) - num2, rectangle1.Top, num1);
                                goto Label_015A;
                            }
                        }
                    }
                    goto Label_015A;
                Label_0129:
                    num3 = Painter.StringWidth(text1);
                    Painter.TextOut(text1, -1, rectangle1, rectangle1.Left + ((rectangle1.Width - num3) / 2), rectangle1.Top, num1);
                Label_015A:
                    this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.After, DrawState.LineNumber | DrawState.Gutter, this.drawInfo);
                }
                finally
                {
                    if (flag1)
                    {
                        Painter.BkMode = 2;
                    }
                    Painter.Color = color1;
                    Painter.BkColor = color2;
                }
            }
        }

        private void DrawOutline(ITextPainter Painter, int Line, int L, int T, int W, bool AEnd)
        {
            int num1 = EditConsts.DefaultCollasedImageWidth;
            int num2 = T;
            this.CenterOutlineRect(ref L, ref T, W);
            Rectangle rectangle1 = new Rectangle(L, T, num1, W);
            this.drawInfo.Init();
            this.drawInfo.Line = Line;
            Color color1 = Painter.PenColor;
            try
            {
                Painter.PenColor = this.owner.Outlining.OutlineColor;
                if (!this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.Before, DrawState.OutlineArea | DrawState.Gutter, this.drawInfo))
                {
                    this.DrawLine(Painter, L + (num1 >> 1), num2, L + (num1 >> 1), num2 + W);
                    if (AEnd)
                    {
                        this.DrawLine(Painter, L + (num1 >> 1), (num2 + W) - 1, (L + num1) + 1, (num2 + W) - 1);
                    }
                }
            }
            finally
            {
                Painter.PenColor = color1;
            }
            this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.After, DrawState.OutlineArea | DrawState.Gutter, this.drawInfo);
        }

        private void DrawOutlineButton(ITextPainter Painter, int Line, int L, int T, int W, bool DrawLines, bool AVisible, bool ABefore, bool AAfter)
        {
            int num1 = EditConsts.DefaultCollasedImageWidth;
            int num2 = T;
            this.CenterOutlineRect(ref L, ref T, W);
            Rectangle rectangle1 = new Rectangle(L, T, num1, num1);
            this.drawInfo.Init();
            this.drawInfo.Line = Line;
            IOutlining outlining1 = this.owner.Outlining;
            Color color1 = Painter.BkColor;
            Color color2 = Painter.PenColor;
            try
            {
                Painter.BkColor = outlining1.OutlineColor;
                Painter.PenColor = outlining1.OutlineColor;
                if (!this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.Before, DrawState.OutlineButton | DrawState.Gutter, this.drawInfo))
                {
                    this.DrawRectangle(Painter, rectangle1, !AVisible);
                    if (DrawLines)
                    {
                        if (ABefore)
                        {
                            this.DrawLine(Painter, L + (num1 >> 1), num2, L + (num1 >> 1), T + 1);
                        }
                        if (AAfter)
                        {
                            this.DrawLine(Painter, L + (num1 >> 1), T + num1, L + (num1 >> 1), (num2 + W) + 1);
                        }
                    }
                }
                this.owner.OnCustomDraw(Painter, rectangle1, DrawStage.After, DrawState.OutlineButton | DrawState.Gutter, this.drawInfo);
            }
            finally
            {
                Painter.BkColor = color1;
                Painter.PenColor = color2;
            }
        }

        private void DrawRectangle(ITextPainter Painter, Rectangle Rect, bool DrawPlus)
        {
            Rectangle rectangle1 = new Rectangle(Rect.Left + 1, Rect.Top + 1, Rect.Width - 1, Rect.Height - 1);
            if (!this.Transparent)
            {
                Color color1 = Painter.BkColor;
                try
                {
                    Painter.BkColor = (this.visible && ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != OutlineOptions.None)) ? this.BrushColor : this.owner.BackColor;
                    Painter.FillRectangle(rectangle1);
                }
                finally
                {
                    Painter.BkColor = color1;
                }
            }
            int num1 = EditConsts.DefaultCollasedImageWidth;
            Painter.DrawRectangle(Rect.Left, Rect.Top, Rect.Width + 1, Rect.Height + 1);
            Painter.DrawLine(Rect.Left + 2, Rect.Top + (num1 >> 1), (Rect.Left + num1) - 1, Rect.Top + (num1 >> 1));
            if (DrawPlus)
            {
                Painter.DrawLine(Rect.Left + (num1 >> 1), Rect.Top + 2, Rect.Left + (num1 >> 1), (Rect.Top + num1) - 1);
            }
            if (!this.Transparent)
            {
                Painter.ExcludeClipRect(Rect.Left, Rect.Top, Rect.Width + 1, Rect.Height + 1);
            }
        }

        protected void EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
        }

        ~Gutter()
        {
            this.ranges.Clear();
            this.brush.Dispose();
            this.pen.Dispose();
            this.internalImages.ImageStream = null;
            this.internalImages.Dispose();
        }

        private int GetLineNumbersLeft(bool UseIndent)
        {
            if (!this.NeedLineNumbers())
            {
                return 0;
            }
            return (((this.visible && ((this.options & GutterOptions.PaintLinesOnGutter) == GutterOptions.None)) ? this.GetPaintWidth() : 0) + (UseIndent ? this.lineNumbersLeftIndent : 0));
        }

        private int GetLineNumbersRight(bool UseIndent)
        {
            if (!this.NeedLineNumbers())
            {
                return 0;
            }
            return ((this.GetLineNumbersLeft(UseIndent) + this.lineNumberWidth) - (UseIndent ? (this.lineNumbersLeftIndent + this.lineNumbersRightIndent) : 0));
        }

        private int GetLineNumbersWidth()
        {
            if ((this.options & GutterOptions.PaintLineNumbers) == GutterOptions.None)
            {
                return 0;
            }
            return this.lineNumberWidth;
        }

        private int GetOutlineLeft()
        {
            if ((this.owner != null) && ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != OutlineOptions.None))
            {
                return (this.GetPaintWidth() - this.GetOutlineWidth());
            }
            return (this.GetWidth() - this.GetOutlineWidth());
        }

        private int GetOutlineWidth()
        {
            if (!this.AllowOutlining())
            {
                return 0;
            }
            return this.owner.Painter.FontHeight;
        }

        private int GetPaintWidth()
        {
            return this.GetPaintWidth(true);
        }

        private int GetPaintWidth(bool CheckOutlining)
        {
            int num1 = this.visible ? this.Width : 0;
            if ((this.options & GutterOptions.PaintLinesOnGutter) != GutterOptions.None)
            {
                num1 = Math.Max(num1, this.GetLineNumbersWidth());
            }
            if ((CheckOutlining && (this.owner != null)) && ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != OutlineOptions.None))
            {
                num1 += this.GetOutlineWidth();
            }
            return num1;
        }

        protected internal int GetWidth()
        {
            int num1 = this.visible ? this.Width : 0;
            int num2 = this.GetLineNumbersWidth();
            if ((this.options & GutterOptions.PaintLinesOnGutter) == GutterOptions.None)
            {
                num1 += num2;
            }
            else
            {
                num1 = Math.Max(num1, num2);
            }
            return (num1 + this.GetOutlineWidth());
        }

        protected internal bool IsMouseOnGutter(int X, int Y)
        {
            Point point1 = this.ScreenToClient(X, Y);
            X = point1.X;
            Rectangle rectangle2 = this.Rect;
            if (X >= this.Rect.Left)
            {
                return (X <= (this.Rect.Left + this.GetWidth()));
            }
            return false;
        }

        protected internal bool IsMouseOnGutterImage(int X, int Y, out int ImageIndex)
        {
            ImageIndex = -1;
            if (this.Visible && this.IsMouseOnGutter(X, Y))
            {
                Point point2 = this.ScreenToClient(X, Y);
                X = point2.X;
                Point point1 = this.owner.ScreenToDisplay(X, Y);
                Rectangle rectangle1 = this.Rect;
                this.DrawGutter(null, new Rectangle(rectangle1.Left, rectangle1.Top, this.Width, 0), true, point1.Y, X, ref ImageIndex);
            }
            return (ImageIndex >= 0);
        }

        protected internal bool IsMouseOnOutlineArea(int X, int Y)
        {
            if (this.AllowOutlining())
            {
                Point point1 = this.ScreenToClient(X, Y);
                X = point1.X;
                int num1 = this.GetOutlineLeft() + this.Rect.Left;
                if (X >= num1)
                {
                    return (X <= (num1 + this.GetOutlineWidth()));
                }
            }
            return false;
        }

        protected internal bool IsMouseOnOutlineImage(int X, int Y)
        {
            if (this.AllowOutlining())
            {
                DisplayStrings strings1 = (DisplayStrings) this.owner.DisplayLines;
                Point point1 = this.owner.ScreenToDisplay(X, Y);
                int num1 = point1.Y;
                point1 = strings1.DisplayPointToPoint(point1);
                this.owner.Outlining.GetOutlineRanges(this.ranges, point1.Y);
                Point point2 = strings1.DisplayPointToPoint(0x7fffffff, num1, true, false, false);
                bool flag1 = false;
                foreach (IOutlineRange range1 in this.ranges)
                {
                    if (this.CompareRange(range1.StartPoint, point1, point2) == 0)
                    {
                        flag1 = true;
                        break;
                    }
                }
                if (flag1)
                {
                    int num2 = this.GetOutlineLeft() + this.Rect.Left;
                    int num3 = this.owner.Painter.FontHeight;
                    point1 = this.ScreenToClient(X, Y);
                    int num4 = this.Rect.Top;
                    int num5 = (num3 == 0) ? point1.Y : (num4 + (((point1.Y - num4) / num3) * num3));
                    int num6 = EditConsts.DefaultCollasedImageWidth;
                    this.CenterOutlineRect(ref num2, ref num5, num3);
                    if (((point1.X >= (num2 - 1)) && (point1.X <= ((num2 + num6) + 1))) && (point1.Y >= (num5 - 1)))
                    {
                        return (point1.Y <= ((num5 + num6) + 1));
                    }
                    return false;
                }
            }
            return false;
        }

        protected internal bool LineNumbersChanged()
        {
            bool flag1 = this.NeedLineNumbers();
            if (flag1)
            {
                int num1 = this.lineNumberWidth;
                this.UpdateLineNumberLength();
                this.UpdateLineNumberWidth();
                flag1 = num1 != this.lineNumberWidth;
                if (!flag1)
                {
                    return flag1;
                }
                if ((this.owner != null) && this.owner.WordWrap)
                {
                    this.owner.UpdateWordWrap();
                }
                this.owner.Invalidate();
            }
            return flag1;
        }

        private bool NeedLineNumbers()
        {
            return ((this.options & GutterOptions.PaintLineNumbers) != GutterOptions.None);
        }

        private bool NeedPaint(ref Rectangle Rect)
        {
            if ((this.owner == null) || Rect.IsEmpty)
            {
                return false;
            }
            int num1 = this.owner.Painter.FontHeight;
            if (num1 > 0)
            {
                int num2 = Rect.Bottom;
                Rect.Y = (Rect.Top / num1) * num1;
                Rect.Height = num2 - Rect.Y;
            }
            return true;
        }

        public virtual void OnClick(EventArgs args)
        {
            if (this.Click != null)
            {
                this.Click(this, args);
            }
        }

        public virtual void OnDoubleClick(EventArgs args)
        {
            if (this.DoubleClick != null)
            {
                this.DoubleClick(this, args);
            }
        }

        public void Paint(ITextPainter Painter, Rectangle Rect)
        {
            this.Paint(Painter, Rect, new Point(0, 0), (this.owner != null) ? this.owner.Scrolling.WindowOriginY : 0);
        }

        public void Paint(ITextPainter Painter, Rectangle Rect, Point Location, int StartLine)
        {
            if (this.NeedPaint(ref Rect))
            {
                Color color1 = Painter.BkColor;
                try
                {
                    if (Painter.FontHeight > 0)
                    {
                        StartLine += (Rect.Top / Painter.FontHeight);
                    }
                    Painter.BkColor = (this.brush is SolidBrush) ? ((SolidBrush) this.brush).Color : EditConsts.DefaultGutterBackColor;
                    this.drawInfo.Init();
                    if (this.owner.OnCustomDraw(Painter, Rect, DrawStage.Before, DrawState.Gutter, this.drawInfo))
                    {
                        return;
                    }
                    this.DrawGutter(Painter, new Rectangle(Rect.Left, Rect.Top, Rect.Width, Rect.Height), StartLine);
                    int num1 = this.GetPaintWidth();
                    if (this.Visible && (num1 != 0))
                    {
                        if (!this.Transparent)
                        {
                            Painter.FillRectangle(0, Rect.Top, num1 - 1, Rect.Height);
                        }
                        Painter.DrawLine(num1 - 1, 0, num1 - 1, Rect.Bottom, this.Pen.Color, (int) this.Pen.Width, this.Pen.DashStyle);
                    }
                    if (((this.options & GutterOptions.PaintLineNumbers) != GutterOptions.None) && (((this.options & GutterOptions.PaintLinesOnGutter) == GutterOptions.None) || !this.Visible))
                    {
                        int num2 = this.GetLineNumbersLeft(false);
                        int num3 = this.GetLineNumbersRight(false);
                        if (num2 < num3)
                        {
                            if (!this.Transparent)
                            {
                                Painter.BkColor = this.lineNumbersBackColor;
                                Painter.FillRectangle(num2, Rect.Top, num3 - num2, Rect.Height);
                            }
                            Painter.DrawDotLine(num3 - 1, 0, num3 - 1, Rect.Bottom, this.lineNumbersForeColor);
                        }
                    }
                    if (!this.Transparent && (((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) == OutlineOptions.None) || !this.Visible))
                    {
                        num1 = this.GetOutlineWidth();
                        if (num1 != 0)
                        {
                            Painter.BkColor = this.owner.BackColor;
                            Painter.FillRectangle(this.GetOutlineLeft(), Rect.Top, num1, Rect.Height);
                        }
                    }
                    this.owner.OnCustomDraw(Painter, Rect, DrawStage.After, DrawState.Gutter, this.drawInfo);
                }
                finally
                {
                    Painter.BkColor = color1;
                }
            }
        }

        public virtual void ResetBookMarkImageIndex()
        {
            this.BookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;
        }

        public virtual void ResetBrushColor()
        {
            this.BrushColor = EditConsts.DefaultGutterBackColor;
        }

        public virtual void ResetDrawLineBookmarks()
        {
            this.DrawLineBookmarks = false;
        }

        public virtual void ResetLineBookmarksColor()
        {
            this.LineBookmarksColor = EditConsts.DefaultLineBookmarksColor;
        }

        public virtual void ResetLineNumbersAlignment()
        {
            this.LineNumbersAlignment = StringAlignment.Near;
        }

        public virtual void ResetLineNumbersBackColor()
        {
            this.LineNumbersBackColor = EditConsts.DefaultLineNumbersBackColor;
        }

        public virtual void ResetLineNumbersForeColor()
        {
            this.LineNumbersForeColor = EditConsts.DefaultLineNumbersForeColor;
        }

        public virtual void ResetLineNumbersLeftIndent()
        {
            this.LineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;
        }

        public virtual void ResetLineNumbersRightIndent()
        {
            this.LineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;
        }

        public virtual void ResetLineNumbersStart()
        {
            this.LineNumbersStart = EditConsts.DefaultLineNumbersStart;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultGutterOptions;
        }

        public virtual void ResetPenColor()
        {
            this.PenColor = EditConsts.DefaultGutterForeColor;
        }

        public virtual void ResetVisible()
        {
            this.Visible = true;
        }

        public virtual void ResetWidth()
        {
            this.Width = EditConsts.DefaultGutterWidth;
        }

        public virtual void ResetWrapImageIndex()
        {
            this.WrapImageIndex = EditConsts.DefaultWrapImageIndex;
        }

        private void RestoreDC(IntPtr Dc, IntPtr Bitmap, IntPtr OldBitmap)
        {
            Win32.SelectObject(Dc, OldBitmap);
            Win32.DeleteObject(Bitmap);
            Win32.DeleteDC(Dc);
        }

        private Point ScreenToClient(int X, int Y)
        {
            if ((this.owner != null) && (this.owner.Pages.PageType == PageType.PageLayout))
            {
                Rectangle rectangle1 = this.owner.Pages.GetPageAtPoint(X, Y).ClientRect;
                X -= rectangle1.Left;
                Y -= rectangle1.Top;
            }
            return new Point(X, Y);
        }

        public bool ShouldSerializeBookMarkImageIndex()
        {
            return (this.bookMarkImageIndex != EditConsts.DefaultBookMarkImageIndex);
        }

        public bool ShouldSerializeBrushColor()
        {
            return (this.BrushColor != EditConsts.DefaultGutterBackColor);
        }

        public bool ShouldSerializeLineBookmarksColor()
        {
            return (this.lineBookmarksColor != EditConsts.DefaultLineBookmarksColor);
        }

        public bool ShouldSerializeLineNumbersBackColor()
        {
            return (this.lineNumbersBackColor != EditConsts.DefaultLineNumbersBackColor);
        }

        public bool ShouldSerializeLineNumbersForeColor()
        {
            return (this.lineNumbersForeColor != EditConsts.DefaultLineNumbersForeColor);
        }

        public bool ShouldSerializeLineNumbersLeftIndent()
        {
            return (this.lineNumbersLeftIndent != EditConsts.DefaultLineNumbersIndent);
        }

        public bool ShouldSerializeLineNumbersRightIndent()
        {
            return (this.lineNumbersRightIndent != EditConsts.DefaultLineNumbersIndent);
        }

        public bool ShouldSerializeLineNumbersStart()
        {
            return (this.lineNumbersStart != EditConsts.DefaultLineNumbersStart);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultGutterOptions);
        }

        public bool ShouldSerializePenColor()
        {
            return (this.PenColor != EditConsts.DefaultGutterForeColor);
        }

        public bool ShouldSerializeWidth()
        {
            return (this.width != EditConsts.DefaultGutterWidth);
        }

        public bool ShouldSerializeWrapImageIndex()
        {
            return (this.wrapImageIndex != EditConsts.DefaultWrapImageIndex);
        }

        protected void Update()
        {
            this.Update(true);
        }

        protected void Update(bool NeedChange)
        {
            if (((this.updateCount == 0) && (this.owner != null)) && NeedChange)
            {
                this.owner.Invalidate();
            }
        }

        private void UpdateLineNumberLength()
        {
            if (this.owner != null)
            {
                int num1 = Math.Max((int) ((Math.Max(this.owner.DisplayLines.GetCount(), this.owner.Lines.Count) + this.lineNumbersStart) - 1), 0);
                if ((this.options & GutterOptions.PaintLinesBeyondEof) != GutterOptions.None)
                {
                    int num2;
                    if (this.owner.Pages.PageType == PageType.PageLayout)
                    {
                        IEditPage page1 = (this.owner.Pages.Count > 0) ? this.owner.Pages[this.owner.Pages.Count - 1] : this.owner.Pages.DefaultPage;
                        num2 = page1.EndLine;
                    }
                    else
                    {
                        num2 = this.owner.Scrolling.WindowOriginY + this.owner.LinesInHeight();
                    }
                    Point point1 = this.owner.DisplayLines.DisplayPointToPoint(0, num2);
                    num2 = point1.Y;
                    num1 = Math.Max(num1, num2);
                }
                this.lineNumberLength = num1.ToString().Length;
            }
        }

        private void UpdateLineNumberWidth()
        {
            if (this.owner != null)
            {
                this.lineNumberWidth = ((this.lineNumberLength * this.owner.Painter.FontWidth) + this.lineNumbersLeftIndent) + this.lineNumbersRightIndent;
            }
        }


        // Properties
        public int BookMarkImageIndex
        {
            get
            {
                return this.bookMarkImageIndex;
            }
            set
            {
                if (this.bookMarkImageIndex != value)
                {
                    this.bookMarkImageIndex = value;
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public System.Drawing.Brush Brush
        {
            get
            {
                return this.brush;
            }
            set
            {
                if (this.brush != value)
                {
                    this.brush = value;
                    this.Update();
                }
            }
        }

        public Color BrushColor
        {
            get
            {
                if (!(this.brush is SolidBrush))
                {
                    return EditConsts.DefaultGutterBackColor;
                }
                return ((SolidBrush) this.brush).Color;
            }
            set
            {
                if ((this.brush is SolidBrush) && (((SolidBrush) this.brush).Color != value))
                {
                    ((SolidBrush) this.brush).Color = value;
                    this.Update();
                }
            }
        }

        [DefaultValue(false)]
        public bool DrawLineBookmarks
        {
            get
            {
                return this.drawLineBookmarks;
            }
            set
            {
                if (this.drawLineBookmarks != value)
                {
                    this.drawLineBookmarks = value;
                    if (this.owner != null)
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        public ImageList Images
        {
            get
            {
                if (this.images == null)
                {
                    return this.internalImages;
                }
                return this.images;
            }
            set
            {
                if (this.images != value)
                {
                    this.images = value;
                    this.Update();
                }
            }
        }

        public Color LineBookmarksColor
        {
            get
            {
                return this.lineBookmarksColor;
            }
            set
            {
                if (this.lineBookmarksColor != value)
                {
                    this.lineBookmarksColor = value;
                    if (this.drawLineBookmarks && (this.owner != null))
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        [DefaultValue(0)]
        public StringAlignment LineNumbersAlignment
        {
            get
            {
                return this.lineNumbersAlignment;
            }
            set
            {
                if (this.lineNumbersAlignment != value)
                {
                    this.lineNumbersAlignment = value;
                    this.Update(this.NeedLineNumbers());
                }
            }
        }

        public Color LineNumbersBackColor
        {
            get
            {
                return this.lineNumbersBackColor;
            }
            set
            {
                if (this.lineNumbersBackColor != value)
                {
                    this.lineNumbersBackColor = value;
                    this.Update(this.NeedLineNumbers());
                }
            }
        }

        public Color LineNumbersForeColor
        {
            get
            {
                return this.lineNumbersForeColor;
            }
            set
            {
                if (this.lineNumbersForeColor != value)
                {
                    this.lineNumbersForeColor = value;
                    this.Update(this.NeedLineNumbers());
                }
            }
        }

        public int LineNumbersLeftIndent
        {
            get
            {
                return this.lineNumbersLeftIndent;
            }
            set
            {
                if (this.lineNumbersLeftIndent != value)
                {
                    this.lineNumbersLeftIndent = value;
                    this.LineNumbersChanged();
                }
            }
        }

        public int LineNumbersRightIndent
        {
            get
            {
                return this.lineNumbersRightIndent;
            }
            set
            {
                if (this.lineNumbersRightIndent != value)
                {
                    this.lineNumbersRightIndent = value;
                    this.LineNumbersChanged();
                }
            }
        }

        public int LineNumbersStart
        {
            get
            {
                return this.lineNumbersStart;
            }
            set
            {
                if (this.lineNumbersStart != value)
                {
                    this.lineNumbersStart = value;
                    this.LineNumbersChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor))]
        public GutterOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.LineNumbersChanged();
                    this.Update();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Pen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                if (this.pen != value)
                {
                    this.pen = value;
                    this.Update();
                }
            }
        }

        public Color PenColor
        {
            get
            {
                return this.pen.Color;
            }
            set
            {
                if (this.pen.Color != value)
                {
                    this.pen.Color = value;
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Rectangle Rect
        {
            get
            {
                Rectangle rectangle1 = (this.owner != null) ? ((SyntaxEdit) this.owner).GetClientRect(true) : new Rectangle(0, 0, 0, 0);
                rectangle1.Width = this.GetWidth();
                return rectangle1;
            }
        }

        protected internal bool Transparent
        {
            get
            {
                if (this.owner != null)
                {
                    return this.owner.Transparent;
                }
                return false;
            }
        }

        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    if ((this.owner != null) && this.owner.WordWrap)
                    {
                        this.owner.UpdateWordWrap();
                    }
                    this.Update();
                }
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (this.width != value)
                {
                    this.width = value;
                    if (this.visible)
                    {
                        this.Update();
                    }
                }
            }
        }

        public int WrapImageIndex
        {
            get
            {
                return this.wrapImageIndex;
            }
            set
            {
                if (this.wrapImageIndex != value)
                {
                    this.wrapImageIndex = value;
                    this.Update();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public object XmlInfo
        {
            get
            {
                return new XmlGutterInfo(this);
            }
            set
            {
                ((XmlGutterInfo) value).FixupReferences(this);
            }
        }


        // Fields
        private int bookMarkImageIndex;
        private System.Drawing.Brush brush;
        private DrawInfo drawInfo;
        private bool drawLineBookmarks;
        private ImageList images;
        private ImageList internalImages;
        private Color lineBookmarksColor;
        private int lineNumberLength;
        private StringAlignment lineNumbersAlignment;
        private Color lineNumbersBackColor;
        private Color lineNumbersForeColor;
        private int lineNumbersLeftIndent;
        private int lineNumbersRightIndent;
        private int lineNumbersStart;
        private int lineNumberWidth;
        private GutterOptions options;
        private ISyntaxEdit owner;
        private System.Drawing.Pen pen;
        private ArrayList ranges;
        private int updateCount;
        private bool visible;
        private int width;
        private int wrapImageIndex;
    }
}

