//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Displays a confirmation dialog box to overwrite a read-only file.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 10-Nov-2005		New
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;


	//********************************************************************************************
	// class OverwriteDialog
	//********************************************************************************************

	/// <summary>
	/// Displays a confirmation dialog box to overwrite a read-only file.
	/// </summary>

	internal partial class OverwriteDialog : Form
	{

		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new dialog.  Set the Filename property before displaying
		/// the dialog.
		/// </summary>

		public OverwriteDialog ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Sets the filename displayed as part of the warning message.  This
		/// should be set prior to displaying the dialog.
		/// </summary>

		public string Filename
		{
			set
			{
				warningMsg.Text = string.Format(warningMsg.Text, value);
				this.Width = warningMsg.Width + 100;
			}
		}
	}
}