namespace River.Orqa.Editor.Syntax
{
    using System;

    public interface IParamInfo : ISyntaxTypeInfo, ISyntaxInfo
    {
        // Properties
        string Qualifier { get; set; }

    }
}

