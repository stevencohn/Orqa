namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;

    public class RegionInfo : RangeInfo, IRegionInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        public RegionInfo()
        {
        }

        public RegionInfo(string Name, Point Position, int Level) : base(Name, Position, Level)
        {
        }

        public override int GetIndentLevel(int Index, Hashtable Breaks)
        {
            return base.Level;
        }


        // Properties
        public override string DisplayText
        {
            get
            {
                return base.Name;
            }
        }

    }
}

