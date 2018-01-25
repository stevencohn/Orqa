namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public interface ISyntaxSettings : IPersistentSettings
    {
        // Methods
        void ApplyToEdit(SyntaxEdit SyntaxEdit);

        bool IsBackColorEnabled(int Index);

        bool IsDescriptionEnabled(int Index);

        bool IsFontStyleEnabled(int Index);

        void LoadFromEdit(SyntaxEdit SyntaxEdit);


        // Properties
        bool AllowOutlining { get; set; }

        System.Drawing.Font Font { get; set; }

        River.Orqa.Editor.GutterOptions GutterOptions { get; set; }

        int GutterWidth { get; set; }

        bool HighlightUrls { get; set; }

        LexStyle[] LexStyles { get; set; }

        int MarginPos { get; set; }

        River.Orqa.Editor.NavigateOptions NavigateOptions { get; set; }

        River.Orqa.Editor.OutlineOptions OutlineOptions { get; set; }

        RichTextBoxScrollBars ScrollBars { get; set; }

        River.Orqa.Editor.SelectionOptions SelectionOptions { get; set; }

        bool ShowGutter { get; set; }

        bool ShowMargin { get; set; }

        int[] TabStops { get; set; }

        bool UseSpaces { get; set; }

        bool WordWrap { get; set; }

    }
}

