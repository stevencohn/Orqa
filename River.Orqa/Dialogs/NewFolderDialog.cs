//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Presents a dialog allowing the user to select the type and name of a new item
// to add to a project folder.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 08-Nov-2005      New
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;


	//********************************************************************************************
	// class NewFolderDialog
	//********************************************************************************************

	public partial class NewFolderDialog : Form
	{

		//========================================================================================
		// Constructor
		//========================================================================================
		
		public NewFolderDialog ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the name of the folder to create or <b>null</b> if no name was specified.
		/// </summary>

		public string FolderName
		{
			get
			{
				string name = nameBox.Text.Trim();
				return (name.Length == 0 ? null : name);
			}
		}


		//========================================================================================
		// Handlers
		//========================================================================================

		private void DoAccept (object sender, EventArgs e)
		{
			this.Close();
		}


		private void DoCancel (object sender, EventArgs e)
		{
			nameBox.Text = String.Empty;
			this.Close();
		}
	}
}