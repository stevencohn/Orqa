
namespace River.Orqa.Query
{
	using System;
	using System.Drawing;


	internal interface IEditor
	{
		bool CanPaste { get; }
		bool CanRedo { get; }
		bool CanUndo { get; }
		int Column { get; }
		Color ForeColor { get; set; }
		bool IsFocused { get; }
		bool IsSaved { get; set; }
		bool HasContent { get; }
		bool HasSelection { get; }
		int Line { get; }
		int TabStops { get; set; }
		string[] TextLines { get; }
		bool WhitespaceVisible { get; }

		void DoCut (object sender, EventArgs e);
		void DoCopy (object sender, EventArgs e);
		void DoClear (object sender, EventArgs e);
		void DoFind (object sender, EventArgs e);
		void DoGotoLine (object sender, EventArgs e);
		void DoMakeLowercase (object sender, EventArgs e);
		void DoMakeUppercase (object sender, EventArgs e);
		void DoPaste (object sender, EventArgs e);
		void DoRedo (object sender, EventArgs e);
		void DoSelectAll (object sender, EventArgs e);
		void DoToggleWhitespace (object sender, EventArgs e);
		void DoUndo (object sender, EventArgs e);

		void RevertColor ();
		void SaveFile (string filename);

		void Write (string text);
		void WriteLine ();
		void WriteLine (string text);
		void WriteNote (string text);
	}
}
