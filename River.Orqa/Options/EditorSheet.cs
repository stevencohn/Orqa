//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// User preferences and program options.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0, extensibility added via SheetBase and XML attributes
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class EditorSheet
	//********************************************************************************************

	internal partial class EditorSheet : SheetBase
	{
		private Translator translator;


		//========================================================================================
		// Constructor
		//========================================================================================

		public EditorSheet ()
			: base()
		{
			InitializeComponent();

			translator = new Translator("Options");

			Reset();
		}


		//========================================================================================
		// State management
		//========================================================================================

		private void DoToggleGutter (object sender, EventArgs e)
		{
			gutterWidthBox.Enabled = showGutterCheck.Checked;
			lineNumbersCheck.Enabled = showGutterCheck.Checked;
		}

		private void DoToggleWordWrap (object sender, EventArgs e)
		{
			wrapAtMarginCheck.Enabled = wordWrapCheck.Checked;
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		public override void Reset ()
		{
			showGutterCheck.Checked = UserOptions.GetBoolean("editor/general/showGutter");
			lineNumbersCheck.Checked = UserOptions.GetBoolean("editor/general/lineNumbers");
			gutterWidthBox.Value = UserOptions.GetInt("editor/general/gutterWidth");

			showMarginCheck.Checked = UserOptions.GetBoolean("editor/general/showMargin");
			marginPositionBox.Value = UserOptions.GetInt("editor/general/marginPosition");
			wordWrapCheck.Checked = UserOptions.GetBoolean("editor/general/wordWrap");
			wrapAtMarginCheck.Checked = UserOptions.GetBoolean("editor/general/wrapAtMargin");

			beolnCheck.Checked = UserOptions.GetBoolean("editor/general/beyondEoln");
			beofCheck.Checked = UserOptions.GetBoolean("editor/general/beyondEof");
			vscrollCheck.Checked = UserOptions.GetBoolean("editor/general/verticalScroll");
			hscrollCheck.Checked = UserOptions.GetBoolean("editor/general/horizontalScroll");

			DoToggleGutter(null, null);
			DoToggleWordWrap(null, null);
		}


		//========================================================================================
		// SaveOptions()
		//========================================================================================

		public override void SaveOptions ()
		{
			UserOptions.SetValue("editor/general/showGutter", showGutterCheck.Checked);
			UserOptions.SetValue("editor/general/lineNumbers", lineNumbersCheck.Checked);
			UserOptions.SetValue("editor/general/gutterWidth", (int)gutterWidthBox.Value);

			UserOptions.SetValue("editor/general/showMargin", showMarginCheck.Checked);
			UserOptions.SetValue("editor/general/marginPosition", (int)marginPositionBox.Value);
			UserOptions.SetValue("editor/general/wordWrap", wordWrapCheck.Checked);
			UserOptions.SetValue("editor/general/wrapAtMargin", wrapAtMarginCheck.Checked);

			UserOptions.SetValue("editor/general/beyondEoln", beolnCheck.Checked);
			UserOptions.SetValue("editor/general/beyondEof", beofCheck.Checked);
			UserOptions.SetValue("editor/general/verticalScroll", vscrollCheck.Checked);
			UserOptions.SetValue("editor/general/horizontalScroll", hscrollCheck.Checked);
		}
	}
}
