namespace River.Orqa.Editor.Syntax
{
    using River.Orqa.Editor.Common;

    public interface ICsMethodInfo : IMethodInfo, IDelegateInfo, ISyntaxTypeInfo, IHasParams, IRangeInfo, IOutlineRange, IRange, ISyntaxInfo, IAttributes, ICsScope, ICsModifiers, IHasLocalVars
    {
    }
}

