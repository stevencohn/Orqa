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
	// class EditorAdvancedSheet
	//********************************************************************************************

	internal partial class EditorTabsSheet : SheetBase
	{
		private Translator translator;


		//========================================================================================
		// Constructor
		//========================================================================================

		public EditorTabsSheet ()
			: base()
		{
			InitializeComponent();

			translator = new Translator("Options");

			Reset();
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		public override void Reset ()
		{
			sizeBox.Value = UserOptions.GetInt("editor/editorTabs/size");

			if (UserOptions.GetBoolean("editor/editorTabs/keepTabs"))
				keepTabsRadio.Checked = true;
			else
				insertSpacesRadio.Checked = true;
		}


		//========================================================================================
		// SaveOptions()
		//========================================================================================

		public override void SaveOptions ()
		{
			UserOptions.SetValue("editor/editorTabs/size", (int)sizeBox.Value);
			UserOptions.SetValue("editor/editorTabs/keepTabs", keepTabsRadio.Checked);
		}
	}
}
