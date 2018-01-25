namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;
    using System;
    using System.Reflection;

    public class VbUnitInfo : CsUnitInfo, IVbUnitInfo, ICsUnitInfo, IUnitInfo, INamespaceInfo, IClassInfo, IInterfaceInfo, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes
    {
        // Methods
        public VbUnitInfo()
        {
        }

        public VbUnitInfo(string Name) : base(Name)
        {
        }

        public override int GetImageIndex(ISyntaxInfo Info)
        {
            int num1 = Info.ImageIndex;
            if (Info is IVbScope)
            {
                VbScope scope1 = ((IVbScope) Info).Scope;
                if ((scope1 & VbScope.Private) != VbScope.None)
                {
                    return (num1 + UnitInfo.ImageDelta);
                }
                if ((scope1 & VbScope.Protected) != VbScope.None)
                {
                    return (num1 + (2 * UnitInfo.ImageDelta));
                }
                if ((scope1 & VbScope.Public) != VbScope.None)
                {
                    num1 += (3 * UnitInfo.ImageDelta);
                }
            }
            return num1;
        }

        public override string GetMethodQualifier(IMethodInfo Method)
        {
            if (((VbMethodInfo) Method).Modifiers == VbModifier.None)
            {
                return string.Empty;
            }
            return ((VbMethodInfo) Method).Modifiers.ToString().Replace(',', ' ');
        }

        public override string GetParamText(ISyntaxInfos Params)
        {
            string text1 = string.Empty;
            foreach (IParamInfo info1 in Params)
            {
                string[] textArray1;
                if (text1 != string.Empty)
                {
                    textArray1 = new string[5] { info1.Qualifier, " ", info1.Name, " As ", info1.DataType } ;
                    text1 = text1 + "," + string.Concat(textArray1).Trim();
                    continue;
                }
                textArray1 = new string[5] { info1.Qualifier, " ", info1.Name, " As ", info1.DataType } ;
                text1 = text1 + string.Concat(textArray1).Trim();
            }
            return ("(" + text1 + ")");
        }

        public override string GetParamText(System.Reflection.ParameterInfo[] Params)
        {
            string text1 = string.Empty;
            System.Reflection.ParameterInfo[] infoArray1 = Params;
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                System.Reflection.ParameterInfo info1 = infoArray1[num1];
                if (text1 != string.Empty)
                {
                    string[] textArray1 = new string[5] { text1, ",", info1.Name, " As ", info1.ParameterType.ToString() } ;
                    text1 = string.Concat(textArray1);
                }
                else
                {
                    text1 = text1 + info1.Name + " As " + info1.ParameterType.ToString();
                }
            }
            return ("(" + text1 + ")");
        }

        protected override void InitNativeTypes()
        {
            base.NativeTypes.Add("short", typeof(short));
            base.NativeTypes.Add("long", typeof(long));
            base.NativeTypes.Add("integer", typeof(int));
            base.NativeTypes.Add("variant", typeof(object));
        }


        // Properties
        public override bool CaseSensitive
        {
            get
            {
                return false;
            }
        }

        public override string SelfName
        {
            get
            {
                return "me";
            }
        }

    }
}

