namespace River.Orqa.Editor.Syntax
{
    using System;

    public interface ICodeTemplate
    {
        // Properties
        string Code { get; set; }

        object CustomData { get; set; }

        string Description { get; set; }

        int ImageIndex { get; set; }

        string Name { get; set; }

    }
}

