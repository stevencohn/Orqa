//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Presents a dialog box prompting for the preferred execution method
// when interpreting multiple statements in a query.
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
	using River.Orqa.Database;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class AlgorithmDialog
	//********************************************************************************************

	/// <summary>
	/// Presents a dialog box prompting for the preferred execution method
	/// when interpreting multiple statements in a query.
	/// </summary>

	internal partial class AlgorithmDialog : Form
	{
		private string title;					// localized format string


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Required for Visual Studio designer.  Use the next signature...
		/// </summary>

		public AlgorithmDialog ()
		{
			InitializeComponent();

			this.title = null;
		}


		/// <summary>
		/// Initializes a new dialog with the specified properties.
		/// </summary>
		/// <param name="count">The number of statements discovered in the query.</param>
		/// <param name="isParsing">True if parsing rather than executing.</param>

		public AlgorithmDialog (int count, bool isParsing)
			: this()
		{
			if (title == null)
			{
				Translator translator = new Translator("Dialogs.AlgorithmDialog");
				title = translator.GetString("Title");
				translator = null;
			}

			this.Text = String.Format(title, count);

			if (isParsing)
			{
				wrapRadio.Enabled = false;
				blockRadio.Enabled = false;
			}
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the currently selected preferred interpretation method.
		/// </summary>

		public ParseMode Mode
		{
			get
			{
				if (sequentialRadio.Checked)
					return ParseMode.Sequential;

				if (blockRadio.Checked)
					return ParseMode.Block;

				if (wrapRadio.Checked)
					return ParseMode.Wrapped;

				return ParseMode.Prompt;   // this case shouldn't occur
			}
		}


		/// <summary>
		/// Gets a value indicating whether the current selection should be
		/// saved as the preferred default selection.
		/// </summary>

		public bool StoreSelection
		{
			get { return storeBox.Checked; }
		}
	}
}