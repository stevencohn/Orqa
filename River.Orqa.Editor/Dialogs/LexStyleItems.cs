namespace River.Orqa.Editor.Dialogs
{
    using River.Orqa.Editor;
    using System;
    using System.Drawing;

    internal class LexStyleItems
    {
        // Methods
        static LexStyleItems()
        {
            LexStyleItem[] itemArray1 = new LexStyleItem[12] { 
				new LexStyleItem("Identifiers", "idents"), 
				new LexStyleItem("Numbers", "numbers"), 
				new LexStyleItem("ResWords", "reswords", EditConsts.DefaultReswordForeColor), 
				new LexStyleItem("Comments", "comments", EditConsts.DefaultCommentsForeColor, Color.Empty, EditConsts.DefaultCommentsFontStyle, true), 
				new LexStyleItem("Strings", "strings", EditConsts.DefaultStringsForeColor, Color.Empty, FontStyle.Regular, true), 
				new LexStyleItem("WhiteSpace", "whitespace"), 
				new LexStyleItem("Directives", "directives", EditConsts.DefaultDirectivesForeColor), 
				new LexStyleItem("XML Comments", "xmlcomments", EditConsts.DefaultXmlCommentsForeColor, Color.Empty, FontStyle.Regular, true), 
				new LexStyleItem("HTML tags", "htmltags", EditConsts.DefaultHtmlTagsForeColor), 
				// SMC: changed "line numbers" to "linenumbers"
				new LexStyleItem("Line Numbers", "linenumbers", EditConsts.DefaultLineNumbersForeColor), 
				new LexStyleItem("HyperText", "hypertext", EditConsts.DefaultHyperTextForeColor, Color.Empty, EditConsts.DefaultUrlFontStyle, false), 
				new LexStyleItem("Misspelled Words", "spelling", EditConsts.DefaultSpellForeColor) 
			};

			LexStyleItems.Items = itemArray1;
        }

        public LexStyleItems()
        {
        }


        // Fields
        public static LexStyleItem[] Items;
    }
}

