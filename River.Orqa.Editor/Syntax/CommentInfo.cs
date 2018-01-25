namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;

    public class CommentInfo : RangeInfo, ICommentInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        public CommentInfo()
        {
        }

        public CommentInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override int GetIndentLevel(int Index, Hashtable Breaks)
        {
            if (base.EndPoint.Y > base.Position.Y)
            {
                return -1;
            }
            return base.Level;
        }


        // Properties
        public override string DisplayText
        {
            get
            {
                return "/**//*";
            }
        }

    }
}

