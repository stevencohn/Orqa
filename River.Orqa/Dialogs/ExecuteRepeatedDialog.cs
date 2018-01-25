//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Presents a dialog box prompting for the number of times to repeat the current query.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class ExecuteRepeatedDialog
	//********************************************************************************************

	/// <summary>
	/// Presents a dialog box prompting for the number of times to repeat the current query.
	/// </summary>

	internal partial class ExecuteRepeatedDialog : Form
	{
		private string oorTitle;			// localized format string
		private string oorMessage;			// localized format string


		//========================================================================================
		// Constructor
		//========================================================================================
		
		public ExecuteRepeatedDialog ()
		{
			InitializeComponent();

			oorTitle = null;
			oorMessage = null;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gest the repeat count requested by the user.
		/// </summary>

		public int RepeatCount
		{
			get
			{
				string repeat = repeatBox.Text.Trim();
				if (repeat.Length == 0)
					return 0;
				else
				{
					try
					{
						return Int32.Parse(repeat);
					}
					catch
					{
						return 0;
					}
				}
			}

			set
			{
				repeatBox.Text = value.ToString();
			}
		}


		//========================================================================================
		// DoValidation()
		//		FormClosing event handler
		//========================================================================================
		
		private void DoValidation (object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.None)
			{
				int repeat = this.RepeatCount;
				if (repeat < 0)
				{
					if (oorTitle == null)
					{
						Translator translator = new Translator("Dialogs.ExecuteRepeatedDialog");
						oorTitle = translator.GetString("OutOfRangeTitle");
						oorMessage = translator.GetString("OutOfRangeMessage");
						translator = null;
					}

					MessageBox.Show(
						oorMessage, oorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

					e.Cancel = true;
				}
			}
		}
	}
}