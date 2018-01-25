namespace River.Orqa.Editor.Common
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class SortList : ArrayList
    {
        // Methods
        public SortList()
        {
        }

        public bool FindExact(object Obj, out int Index, IComparer comparer)
        {
            int num1 = 0;
            int num2 = this.Count - 1;
            int num3 = num1;
            bool flag1 = false;
            while (num1 <= num2)
            {
                num3 = (num1 + num2) >> 1;
                this.CompareIndex = num3;
                int num4 = comparer.Compare(this[num3], Obj);
                if (num4 < 0)
                {
                    num1 = num3 + 1;
                    continue;
                }
                num2 = num3 - 1;
                if (num4 == 0)
                {
                    flag1 = true;
                    num1 = num3;
                }
            }
            Index = num1;
            return flag1;
        }

        public bool FindFirst(object Obj, out int Index, IComparer comparer)
        {
            int num1 = 0;
            int num2 = this.Count - 1;
            int num3 = num1;
            bool flag1 = false;
            while (num1 <= num2)
            {
                num3 = (num1 + num2) >> 1;
                this.CompareIndex = num3;
                int num4 = comparer.Compare(this[num3], Obj);
                if (num4 < 0)
                {
                    num1 = num3 + 1;
                    continue;
                }
                num2 = num3 - 1;
                if (num4 == 0)
                {
                    flag1 = true;
                }
            }
            Index = num1;
            return flag1;
        }

        public bool FindLast(object Obj, out int Index, IComparer comparer)
        {
            int num1 = 0;
            int num2 = this.Count - 1;
            int num3 = num1;
            int num5 = 0;
            bool flag1 = false;
            while (num1 <= num2)
            {
                num3 = (num1 + num2) >> 1;
                this.CompareIndex = num3;
                int num4 = comparer.Compare(this[num3], Obj);
                if (num4 > 0)
                {
                    num2 = num3 - 1;
                    continue;
                }
                num1 = num3 + 1;
                if (num4 == 0)
                {
                    num5 = num3;
                    flag1 = true;
                }
            }
            Index = flag1 ? num5 : num1;
            return flag1;
        }

        public static bool InsideBlock(Point Pt, Rectangle Rect)
        {
            if ((Pt.Y > Rect.Bottom) || (Pt.Y < Rect.Top))
            {
                return false;
            }
            if (Pt.Y == Rect.Top)
            {
                if (Pt.Y != Rect.Bottom)
                {
                    return (Pt.X >= Rect.Left);
                }
                if (Pt.X >= Rect.Left)
                {
                    return (Pt.X < Rect.Right);
                }
                return false;
            }
            if (Pt.Y == Rect.Bottom)
            {
                return (Pt.X < Rect.Right);
            }
            return true;
        }

        public static bool UpdatePos(int X, int Y, int DeltaX, int DeltaY, ref Point Pt, bool EndPos)
        {
            int num1 = Pt.X;
            int num2 = Pt.Y;
            bool flag1 = SortList.UpdatePos(X, Y, DeltaX, DeltaY, ref num1, ref num2, EndPos);
            Pt.X = num1;
            Pt.Y = num2;
            return flag1;
        }

        public static bool UpdatePos(int X, int Y, int DeltaX, int DeltaY, ref int Char, ref int Line, bool EndPos)
        {
            int num1 = Char;
            int num2 = Line;
            if (DeltaY == 0)
            {
                bool flag1 = (DeltaX < 0) || EndPos;
                if ((Line == Y) && (((Char >= X) && !flag1) || ((Char > X) && flag1)))
                {
                    if (Char != 0x7fffffff)
                    {
                        Char += DeltaX;
                    }
                    Char = Math.Max(Char, 0);
                }
            }
            else
            {
                bool flag2 = EndPos;
                if ((Line > Y) || ((Line == Y) && (((Char >= X) && !flag2) || ((Char > X) && flag2))))
                {
                    if (((Line == Y) && (DeltaY >= 0)) || (((Line + DeltaY) == Y) && (DeltaY < 0)))
                    {
                        if (Char != 0x7fffffff)
                        {
                            Char += DeltaX;
                        }
                        Char = Math.Max(Char, 0);
                    }
                    if (Line != 0x7fffffff)
                    {
                        Line += DeltaY;
                    }
                    Line = Math.Max(Line, 0);
                }
            }
            if (Char == num1)
            {
                return (Line != num2);
            }
            return true;
        }


        // Fields
        public int CompareIndex;
    }
}

