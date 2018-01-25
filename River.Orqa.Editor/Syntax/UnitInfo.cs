namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    public class UnitInfo : RangeInfo, IUnitInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo
    {
        // Methods
        static UnitInfo()
        {
            UnitInfo.ClassImage = 0;
            UnitInfo.ConstImage = 1;
            UnitInfo.DelegateImage = 2;
            UnitInfo.EnumImage = 3;
            UnitInfo.EventImage = 4;
            UnitInfo.FieldImage = 5;
            UnitInfo.ParamImage = 5;
            UnitInfo.LocalVarImage = 5;
            UnitInfo.InterfaceImage = 6;
            UnitInfo.MethodImage = 7;
            UnitInfo.PropImage = 8;
            UnitInfo.StructImage = 9;
            UnitInfo.ImageDelta = 10;
        }

        public UnitInfo()
        {
        }

        public UnitInfo(string Name) : base(Name, new Point(0, 0), 0)
        {
        }

        public void BlockDeleting(Rectangle Rect)
        {
            this.sections.BlockDeleting(Rect);
        }

        public virtual ISyntaxInfo FindByName(string Name, Point Position)
        {
            return null;
        }

        public IRangeInfo FindRange(Point Position)
        {
            return (this.sections.FindRange(Position) as IRangeInfo);
        }

        public IRangeInfo FindRange(int Index)
        {
            return (this.sections.FindRange(Index) as IRangeInfo);
        }

        public virtual object GetClassByName(string Name)
        {
            return null;
        }

        public virtual int GetImageIndex(ISyntaxInfo Info)
        {
            return Info.ImageIndex;
        }

        public virtual int GetImageIndex(MemberInfo Info)
        {
            int num1 = -1;
            if (Info is MethodBase)
            {
                num1 = UnitInfo.MethodImage;
            }
            else if (Info is System.Reflection.EventInfo)
            {
                num1 = UnitInfo.EventImage;
            }
            else if (Info is System.Reflection.FieldInfo)
            {
                num1 = UnitInfo.FieldImage;
            }
            else if (Info is PropertyInfo)
            {
                num1 = UnitInfo.PropImage;
            }
            if (Info is System.Type)
            {
                System.Type type1 = (System.Type) Info;
                if (type1.IsEnum)
                {
                    num1 = UnitInfo.EnumImage;
                }
                else if (type1.IsClass)
                {
                    if (type1.IsValueType)
                    {
                        num1 = UnitInfo.StructImage;
                    }
                    else
                    {
                        num1 = UnitInfo.ClassImage;
                    }
                }
                else if (type1.IsInterface)
                {
                    num1 = UnitInfo.InterfaceImage;
                }
            }
            if (num1 >= 0)
            {
                num1 += (3 * UnitInfo.ImageDelta);
            }
            return num1;
        }

        public virtual IMethodInfo GetMethod(Point Position)
        {
            return null;
        }

        public virtual string GetMethodQualifier(IMethodInfo Method)
        {
            return string.Empty;
        }

        public virtual string GetMethodQualifier(System.Reflection.MethodInfo Method)
        {
            string text1 = string.Empty;
            if (Method.IsAbstract)
            {
                text1 = "abstract";
            }
            if (Method.IsVirtual)
            {
                text1 = (text1 + " virtual").Trim();
            }
            if (Method.IsFinal)
            {
                text1 = (text1 + " sealed").Trim();
            }
            if (Method.IsStatic)
            {
                text1 = (text1 + " static").Trim();
            }
            return text1;
        }

        public virtual int GetMethods(string Text, Point Position, IList Methods)
        {
            return 0;
        }

        public virtual object GetObjectClass(string Text, Point Position)
        {
            return null;
        }

        public virtual string GetParamText(System.Reflection.ParameterInfo[] Params)
        {
            string text1 = string.Empty;
            System.Reflection.ParameterInfo[] infoArray1 = Params;
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                System.Reflection.ParameterInfo info1 = infoArray1[num1];
                if (text1 != string.Empty)
                {
                    text1 = text1 + "," + info1.ParameterType.ToString() + info1.Name;
                }
                else
                {
                    text1 = text1 + info1.ParameterType.ToString() + info1.Name;
                }
            }
            return ("(" + text1 + ")");
        }

        public virtual string GetParamText(ISyntaxInfos Params)
        {
            string text1 = string.Empty;
            foreach (IParamInfo info1 in Params)
            {
                string[] textArray1;
                if (text1 != string.Empty)
                {
                    textArray1 = new string[5] { info1.Qualifier, " ", info1.DataType, " ", info1.Name } ;
                    text1 = text1 + "," + string.Concat(textArray1).Trim();
                    continue;
                }
                textArray1 = new string[5] { info1.Qualifier, " ", info1.DataType, " ", info1.Name } ;
                text1 = text1 + string.Concat(textArray1).Trim();
            }
            return ("(" + text1 + ")");
        }

        protected override void Init()
        {
            base.Init();
            this.sections = new RangeList();
            try
            {
                this.images = new ImageList();
                ResourceManager manager1 = new ResourceManager(typeof(Parser));
                this.images.ImageStream = (ImageListStreamer) manager1.GetObject("UnitInfo.Images.ImageStream");
            }
            catch
            {
            }
        }

        public void PositionChanged(int X, int Y, int DeltaX, int DeltaY, UpdateReason Reason)
        {
            this.sections.PositionChanged(X, Y, DeltaX, DeltaY);
        }

        protected virtual void ProcessSection(ISyntaxInfo Info)
        {
            if (Info is IRangeInfo)
            {
                IRangeInfo info1 = (IRangeInfo) Info;
                if (info1.StartPoint.Y >= 0)
                {
                    this.sections.Add(info1);
                }
                this.ProcessSections(info1.Regions);
                this.ProcessSections(info1.Comments);
            }
            if (Info is IInterfaceInfo)
            {
                IInterfaceInfo info2 = (InterfaceInfo) Info;
                this.ProcessSections(info2.Methods);
                this.ProcessSections(info2.Properties);
                this.ProcessSections(info2.Events);
            }
            if (Info is IClassInfo)
            {
                IClassInfo info3 = (IClassInfo) Info;
                this.ProcessSections(info3.Classes);
                this.ProcessSections(info3.Interfaces);
                this.ProcessSections(info3.Structures);
                this.ProcessSections(info3.Enums);
            }
            if (Info is IMethodInfo)
            {
                this.ProcessSections(((IMethodInfo) Info).Statements);
            }
            else if (Info is IPropInfo)
            {
                IPropInfo info4 = (ICsPropInfo) Info;
                if (info4.PropertyGet != null)
                {
                    this.ProcessSection(info4.PropertyGet);
                }
                if (info4.PropertySet != null)
                {
                    this.ProcessSection(info4.PropertySet);
                }
            }
            else if (Info is IEventInfo)
            {
                IEventInfo info5 = (ICsEventInfo) Info;
                if (info5.EventAdd != null)
                {
                    this.ProcessSection(info5.EventAdd);
                }
                if (info5.EventRemove != null)
                {
                    this.ProcessSection(info5.EventRemove);
                }
            }
            else if (Info is IAccessorInfo)
            {
                this.ProcessSections(((IAccessorInfo) Info).Statements);
            }
        }

        protected virtual void ProcessSections(ISyntaxInfos Infos)
        {
            this.sections.BeginUpdate();
            try
            {
                foreach (ISyntaxInfo info1 in Infos)
                {
                    this.ProcessSection(info1);
                }
            }
            finally
            {
                this.sections.EndUpdate();
            }
        }

        protected internal virtual void UpdateSections()
        {
        }


        // Properties
        public virtual bool CaseSensitive
        {
            get
            {
                return true;
            }
        }

        public ImageList Images
        {
            get
            {
                return this.images;
            }
            set
            {
                this.images = value;
            }
        }

        public RangeList Sections
        {
            get
            {
                return this.sections;
            }
        }

        public virtual string SelfName
        {
            get
            {
                return string.Empty;
            }
        }


        // Fields
        public static int ClassImage;
        public static int ConstImage;
        public static int DelegateImage;
        public static int EnumImage;
        public static int EventImage;
        public static int FieldImage;
        public static int ImageDelta;
        private ImageList images;
        public static int InterfaceImage;
        public static int LocalVarImage;
        public static int MethodImage;
        public static int ParamImage;
        public static int PropImage;
        private RangeList sections;
        public static int StructImage;
    }
}

