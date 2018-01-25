//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Declares an event delegate, handler and argument used to indicate when a user
// would like to open the Options dialog from a contained UserControl.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Jul-2005      New
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;


	//********************************************************************************************
	// Delegate
	//********************************************************************************************

	internal delegate void OpeningOptionsEventHandler (object sender, OpeningOptionsEventArgs e);


	//********************************************************************************************
	// class OpeningOptionsEventArgs
	//********************************************************************************************

	internal class OpeningOptionsEventArgs : System.EventArgs
	{
		private string sheetPath;

		public OpeningOptionsEventArgs  (string sheetPath)
		{
			this.sheetPath = sheetPath;
		}

		public string SheetPath
		{
			get { return sheetPath; }
		}
	}
}
