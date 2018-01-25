namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    public class CsUnitInfo : UnitInfo, ICsUnitInfo, IUnitInfo, INamespaceInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public CsUnitInfo()
        {
            this.attributes = string.Empty;
        }

        public CsUnitInfo(string Name) : base(Name)
        {
            this.attributes = string.Empty;
        }

        public override void Clear()
        {
            base.Clear();
            this.attributePt = new Point(-1, -1);
            this.attributes = string.Empty;
            this.fields.Clear();
            this.methods.Clear();
            this.events.Clear();
            this.methods.Clear();
            this.enums.Clear();
            this.delegates.Clear();
            this.classes.Clear();
            this.structures.Clear();
            this.interfaces.Clear();
            this.uses.Clear();
            this.namespaces.Clear();
        }

        public override ISyntaxInfo FindByName(string Name, Point Position)
        {
            int num1 = Name.LastIndexOf(".");
            object obj1 = null;
            if (num1 > 0)
            {
                obj1 = this.GetObjectClass(Name.Substring(0, num1), Position);
                Name = Name.Substring(num1 + 1);
            }
            else
            {
                obj1 = this.GetCurrentClass(Position);
            }
            if (obj1 is IInterfaceInfo)
            {
                foreach (IMethodInfo info1 in ((IInterfaceInfo) obj1).Methods)
                {
                    if ((Position.Y <= info1.StartPoint.Y) && ((Position.Y != info1.StartPoint.Y) || (Position.X <= info1.StartPoint.X)))
                    {
                        continue;
                    }
                    if ((Position.Y < info1.EndPoint.Y) || ((Position.Y == info1.EndPoint.Y) && (Position.X < info1.EndPoint.X)))
                    {
                        ISyntaxInfo info2 = info1.FindByName(Name, this.CaseSensitive);
                        if (info2 != null)
                        {
                            return info2;
                        }
                        break;
                    }
                }
            }
            object obj2 = this.GetInfoByName(obj1, Name, Position);
            if (!(obj2 is ISyntaxInfo))
            {
                return null;
            }
            return (ISyntaxInfo) obj2;
        }

        private object FindInfo(object Owner, string s)
        {
            object obj1 = null;
            if (Owner is Type)
            {
                return ((Type) Owner).GetMember(s);
            }
            if (Owner is IMethodInfo)
            {
                IMethodInfo info1 = (IMethodInfo) Owner;
                return info1.FindByName(s, this.CaseSensitive);
            }
            if (Owner is ISyntaxInfo)
            {
                obj1 = ((ISyntaxInfo) Owner).FindByName(s, this.CaseSensitive);
            }
            return obj1;
        }

        public override object GetClassByName(string s)
        {
            ISyntaxInfo info1;
            foreach (INamespaceInfo info2 in this.Namespaces)
            {
                info1 = this.GetClassByName(info2.Classes, s);
                if (info1 != null)
                {
                    return info1;
                }
            }
            info1 = this.GetClassByName(this.Classes, s);
            if (info1 == null)
            {
                return this.GetNativeClassByName(s);
            }
            return info1;
        }

        private ISyntaxInfo GetClassByName(ISyntaxInfos Infos, string s)
        {
            ISyntaxInfo info1 = Infos.FindByName(s, this.CaseSensitive);
            if (info1 == null)
            {
                foreach (IInterfaceInfo info2 in Infos)
                {
                    if (info2 is IClassInfo)
                    {
                        IClassInfo info3 = (IClassInfo) info2;
                        info1 = this.GetClassByName(info3.Classes, s);
                        if (info1 == null)
                        {
                            info1 = this.GetClassByName(info3.Interfaces, s);
                        }
                        if (info1 == null)
                        {
                            info1 = this.GetClassByName(info3.Structures, s);
                        }
                    }
                    if (info1 != null)
                    {
                        return info1;
                    }
                }
            }
            return info1;
        }

        private IInterfaceInfo GetCurrentClass(Point Position)
        {
            ArrayList list1 = new ArrayList();
            base.Sections.GetRanges(list1, Position);
            for (int num1 = list1.Count - 1; num1 >= 0; num1--)
            {
                if (list1[num1] is IInterfaceInfo)
                {
                    return (IInterfaceInfo) list1[num1];
                }
            }
            return null;
        }

        public override int GetImageIndex(ISyntaxInfo Info)
        {
            int num1 = Info.ImageIndex;
            if (Info is ICsScope)
            {
                CsScope scope1 = ((ICsScope) Info).Scope;
                if ((scope1 & CsScope.Private) != CsScope.None)
                {
                    return (num1 + UnitInfo.ImageDelta);
                }
                if ((scope1 & CsScope.Protected) != CsScope.None)
                {
                    return (num1 + (2 * UnitInfo.ImageDelta));
                }
                if ((scope1 & CsScope.Public) != CsScope.None)
                {
                    num1 += (3 * UnitInfo.ImageDelta);
                }
            }
            return num1;
        }

        private object GetInfoByName(object Owner, string s, Point Position)
        {
            object obj1 = null;
            if (Owner != null)
            {
                return this.FindInfo(Owner, s);
            }
            ArrayList list1 = new ArrayList();
            base.Sections.GetRanges(list1, Position);
            for (int num1 = list1.Count - 1; num1 >= 0; num1--)
            {
                IRangeInfo info1 = (IRangeInfo) list1[num1];
                obj1 = this.FindInfo(info1, s);
                if ((obj1 != null) || (info1 is IInterfaceInfo))
                {
                    break;
                }
            }
            return obj1;
        }

        public override IMethodInfo GetMethod(Point Position)
        {
            ArrayList list1 = new ArrayList();
            base.Sections.GetRanges(list1, Position);
            for (int num1 = list1.Count - 1; num1 >= 0; num1--)
            {
                if (list1[num1] is IMethodInfo)
                {
                    return (IMethodInfo) list1[num1];
                }
            }
            return null;
        }

        public override string GetMethodQualifier(IMethodInfo Method)
        {
            if (((CsMethodInfo) Method).Modifiers == CsModifier.None)
            {
                return string.Empty;
            }
            return ((CsMethodInfo) Method).Modifiers.ToString().Replace(',', ' ');
        }

        public override int GetMethods(string Text, Point Position, IList Methods)
        {
            Methods.Clear();
            int num1 = Text.LastIndexOf(".");
            object obj1 = null;
            if (num1 > 0)
            {
                string text1 = Text.Substring(0, num1);
                obj1 = this.GetObjectClass(text1, Position);
                if (obj1 == null)
                {
                    obj1 = this.GetClassByName(text1);
                }
                Text = Text.Substring(num1 + 1);
            }
            else
            {
                obj1 = this.GetCurrentClass(Position);
            }
            if (obj1 is IInterfaceInfo)
            {
                foreach (IMethodInfo info1 in ((IInterfaceInfo) obj1).Methods)
                {
                    if (string.Compare(info1.Name, Text, !this.CaseSensitive) == 0)
                    {
                        Methods.Add(info1);
                    }
                }
            }
            else if (obj1 is Type)
            {
                System.Reflection.MethodInfo[] infoArray1 = ((Type) obj1).GetMethods();
                for (int num2 = 0; num2 < infoArray1.Length; num2++)
                {
                    System.Reflection.MethodInfo info2 = infoArray1[num2];
                    if (string.Compare(info2.Name, Text, !this.CaseSensitive) == 0)
                    {
                        Methods.Add(info2);
                    }
                }
            }
            return Methods.Count;
        }

        protected internal Type GetNativeClassByName(string s)
        {
            Type type1 = this.GetNativeType(s);
            if ((type1 == null) && (s.IndexOf('.') < 0))
            {
                foreach (IUsesInfo info1 in this.Uses.Uses)
                {
                    type1 = this.GetNativeType(info1.Name + "." + s);
                    if (type1 != null)
                    {
                        return type1;
                    }
                }
            }
            return type1;
        }

        protected internal Type GetNativeType(string s)
        {
            object obj1 = this.nativeTypes[s];
            if (obj1 != null)
            {
                return (Type) obj1;
            }
            Type type1 = Type.GetType(s, false, !this.CaseSensitive);
            if (type1 == null)
            {
                Assembly[] assemblyArray1 = AppDomain.CurrentDomain.GetAssemblies();
                for (int num1 = 0; num1 < assemblyArray1.Length; num1++)
                {
                    Assembly assembly1 = assemblyArray1[num1];
                    type1 = assembly1.GetType(s, false, !this.CaseSensitive);
                    if (type1 != null)
                    {
                        break;
                    }
                }
            }
            return type1;
        }

        public override object GetObjectClass(string Name, Point Position)
        {
            object obj1 = null;
            char[] chArray1 = new char[1] { '.' } ;
            string[] textArray1 = Name.Split(chArray1);
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                string text1 = textArray1[num1];
                if (text1 != string.Empty)
                {
                    object obj2;
                    if (string.Compare(text1, this.SelfName, !this.CaseSensitive) == 0)
                    {
                        obj2 = this.GetCurrentClass(Position);
                    }
                    else
                    {
                        obj2 = this.GetInfoByName(obj1, text1, Position);
                    }
                    if (obj2 is ISyntaxTypeInfo)
                    {
                        obj1 = this.GetClassByName(((ISyntaxTypeInfo) obj2).DataType);
                    }
                    else if (obj2 is IInterfaceInfo)
                    {
                        obj1 = obj2;
                    }
                    else if (obj2 is MemberInfo)
                    {
                        obj1 = this.GetNativeClassByName(((MemberInfo) obj2).Name);
                    }
                    else
                    {
                        obj1 = null;
                    }
                    if (obj1 == null)
                    {
                        return null;
                    }
                }
            }
            return obj1;
        }

        protected override void Init()
        {
            base.Init();
            this.nativeTypes = new Hashtable();
            this.InitNativeTypes();
            this.attributePt = new Point(-1, -1);
            this.fields = new SyntaxInfos();
            this.events = new SyntaxInfos();
            this.properties = new SyntaxInfos();
            this.methods = new SyntaxInfos();
            this.enums = new SyntaxInfos();
            this.delegates = new SyntaxInfos();
            this.classes = new SyntaxInfos();
            this.structures = new SyntaxInfos();
            this.interfaces = new SyntaxInfos();
            this.uses = new UsesInfo();
            this.namespaces = new SyntaxInfos();
        }

        protected virtual void InitNativeTypes()
        {
            this.nativeTypes.Add("bool", typeof(bool));
            this.nativeTypes.Add("byte", typeof(byte));
            this.nativeTypes.Add("char", typeof(char));
            this.nativeTypes.Add("decimal", typeof(decimal));
            this.nativeTypes.Add("double", typeof(double));
            this.nativeTypes.Add("float", typeof(float));
            this.nativeTypes.Add("int", typeof(int));
            this.nativeTypes.Add("long", typeof(long));
            this.nativeTypes.Add("object", typeof(object));
            this.nativeTypes.Add("sbyte", typeof(sbyte));
            this.nativeTypes.Add("short", typeof(short));
            this.nativeTypes.Add("string", typeof(string));
            this.nativeTypes.Add("uint", typeof(uint));
            this.nativeTypes.Add("ulong", typeof(ulong));
            this.nativeTypes.Add("ushort", typeof(ushort));
        }

        protected override void ProcessSection(ISyntaxInfo Info)
        {
            base.ProcessSection(Info);
            if (Info is ICsNamespaceInfo)
            {
                this.ProcessSection(((ICsNamespaceInfo) Info).Uses);
            }
        }

        protected virtual void ProcessSections()
        {
            base.Sections.BeginUpdate();
            try
            {
                base.Sections.Clear();
                this.ProcessSection(this.Uses);
                this.ProcessSections(base.Comments);
                this.ProcessSections(base.Regions);
                this.ProcessSections(this.Enums);
                this.ProcessSections(this.Delegates);
                this.ProcessSections(this.Classes);
                this.ProcessSections(this.Structures);
                this.ProcessSections(this.Interfaces);
                this.ProcessSections(this.Namespaces);
            }
            finally
            {
                base.Sections.EndUpdate();
            }
        }

        protected override void ProcessSections(ISyntaxInfos Infos)
        {
            foreach (ISyntaxInfo info1 in Infos)
            {
                this.ProcessSection(info1);
            }
        }

        protected internal override void UpdateSections()
        {
            this.ProcessSections();
        }


        // Properties
        public Point AttributePt
        {
            get
            {
                return this.attributePt;
            }
            set
            {
                this.attributePt = value;
            }
        }

        public string Attributes
        {
            get
            {
                return this.attributes;
            }
            set
            {
                this.attributes = value;
            }
        }

        public string[] Base
        {
            get
            {
                return new string[0];
            }
            set
            {
            }
        }

        public ISyntaxInfos Classes
        {
            get
            {
                return this.classes;
            }
        }

        public ISyntaxInfos Delegates
        {
            get
            {
                return this.delegates;
            }
        }

        public ISyntaxInfos Enums
        {
            get
            {
                return this.enums;
            }
        }

        public ISyntaxInfos Events
        {
            get
            {
                return this.events;
            }
        }

        public ISyntaxInfos Fields
        {
            get
            {
                return this.fields;
            }
        }

        public ISyntaxInfos Interfaces
        {
            get
            {
                return this.interfaces;
            }
        }

        public ISyntaxInfos Methods
        {
            get
            {
                return this.methods;
            }
        }

        public ISyntaxInfos Namespaces
        {
            get
            {
                return this.namespaces;
            }
        }

        protected Hashtable NativeTypes
        {
            get
            {
                return this.nativeTypes;
            }
        }

        public ISyntaxInfos Properties
        {
            get
            {
                return this.properties;
            }
        }

        public override string SelfName
        {
            get
            {
                return "this";
            }
        }

        public ISyntaxInfos Structures
        {
            get
            {
                return this.structures;
            }
        }

        public IUsesInfo Uses
        {
            get
            {
                return this.uses;
            }
        }


        // Fields
        private Point attributePt;
        private string attributes;
        private ISyntaxInfos classes;
        private ISyntaxInfos delegates;
        private ISyntaxInfos enums;
        private ISyntaxInfos events;
        private ISyntaxInfos fields;
        private ISyntaxInfos interfaces;
        private ISyntaxInfos methods;
        private ISyntaxInfos namespaces;
        private Hashtable nativeTypes;
        private ISyntaxInfos properties;
        private ISyntaxInfos structures;
        private IUsesInfo uses;
    }
}

