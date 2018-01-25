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
	using System.Windows.Forms;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class GeneralSheet
	//********************************************************************************************

	internal partial class GeneralSheet : SheetBase
	{
		private Translator translator;


		//========================================================================================
		// Constructor
		//========================================================================================

		public GeneralSheet ()
			: base()
		{
			InitializeComponent();

			translator = new Translator("Options");

			Reset();
		}


		//========================================================================================
		// Directory Browsers
		//========================================================================================

		private void BrowseQueryDir (object sender, EventArgs e)
		{
			folderBrowserDialog.Description = translator.GetString("QueryFileFolder");
			folderBrowserDialog.SelectedPath = queryDirBox.Text;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				queryDirBox.Text = folderBrowserDialog.SelectedPath;
			}
		}


		private void BrowseResultsDir (object sender, EventArgs e)
		{
			folderBrowserDialog.Description = translator.GetString("QueryResultsFolder");
			folderBrowserDialog.SelectedPath = resultsDirBox.Text;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				resultsDirBox.Text = folderBrowserDialog.SelectedPath;
			}
		}


		private void BrowseTemplateDir (object sender, EventArgs e)
		{
			folderBrowserDialog.Description = translator.GetString("QueryTemplateFolder");
			folderBrowserDialog.SelectedPath = templateDirBox.Text;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				templateDirBox.Text = folderBrowserDialog.SelectedPath;
			}
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		public override void Reset ()
		{
			queryDirBox.Text = UserOptions.GetString("general/queryPath");
			resultsDirBox.Text = UserOptions.GetString("general/resultPath");
			templateDirBox.Text = UserOptions.GetString("general/templatePath");

			queryExtBox.Text = UserOptions.GetString("general/queryExtension");
			reportExtBox.Text = UserOptions.GetString("general/reportExtension");
			resultsExtBox.Text = UserOptions.GetString("general/resultExtension");
			templateExtBox.Text = UserOptions.GetString("general/templateExtension");

			maxMruBox.Value = UserOptions.GetInt("general/maxMru");

			saveModifiedCheck.Checked = UserOptions.GetBoolean("general/saveModified");
			savePositionCheck.Checked = UserOptions.GetBoolean("general/savePosition");
			defaultAppCheck.Checked = UserOptions.GetBoolean("general/defaultApp");
		}


		//========================================================================================
		// SaveOptions()
		//========================================================================================

		public override void SaveOptions ()
		{
			UserOptions.SetValue("general/queryPath", queryDirBox.Text);
			UserOptions.SetValue("general/resultPath", resultsDirBox.Text);
			UserOptions.SetValue("general/templatePath", templateDirBox.Text);

			UserOptions.SetValue("general/queryExtension", queryExtBox.Text);
			UserOptions.SetValue("general/reportExtension", reportExtBox.Text);
			UserOptions.SetValue("general/resultExtension", resultsExtBox.Text);
			UserOptions.SetValue("general/templateExtension", templateExtBox.Text);

			UserOptions.SetValue("general/maxMru", (int)maxMruBox.Value);

			UserOptions.SetValue("general/saveModified", saveModifiedCheck.Checked);
			UserOptions.SetValue("general/savePosition", savePositionCheck.Checked);
			UserOptions.SetValue("general/defaultApp", defaultAppCheck.Checked);
		}
	}
}
