namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public interface IUnitInfo : IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        void BlockDeleting(Rectangle Rect);

        ISyntaxInfo FindByName(string Name, Point Position);

        IRangeInfo FindRange(Point Position);

        IRangeInfo FindRange(int Index);

        object GetClassByName(string Name);

        int GetImageIndex(ISyntaxInfo Info);

        int GetImageIndex(MemberInfo Info);

        IMethodInfo GetMethod(Point Position);

        string GetMethodQualifier(IMethodInfo Info);

        string GetMethodQualifier(System.Reflection.MethodInfo Info);

        int GetMethods(string Text, Point Position, IList Methods);

        object GetObjectClass(string Name, Point Position);

        string GetParamText(ISyntaxInfos Params);

        string GetParamText(System.Reflection.ParameterInfo[] Params);

        void PositionChanged(int X, int Y, int DeltaX, int DeltaY, UpdateReason Reason);


        // Properties
        bool CaseSensitive { get; }

        ImageList Images { get; set; }

        RangeList Sections { get; }

        string SelfName { get; }

    }
}

