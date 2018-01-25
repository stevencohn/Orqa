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
	using System.IO;
	using System.Windows.Forms;
	using System.Xml.XPath;
	using River.Orqa.Database;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class ResultsSheet
	//********************************************************************************************

	internal partial class ResultsSheet : SheetBase
	{
		private Translator translator;


		//========================================================================================
		// Constructor
		//========================================================================================

		public ResultsSheet ()
			: base()
		{
			InitializeComponent();

			translator = new Translator("Options");

			// Output Target

			XPathNavigator nav = new XPathDocument(
				new StringReader(translator.GetString("OutputTarget"))).CreateNavigator();

			XPathNodeIterator nodes = nav.Select("OutputTarget/target");
			while (nodes.MoveNext())
			{
				targetBox.Items.Add(nodes.Current.Value);
			}

			// Output Format

			nav = new XPathDocument(
				new StringReader(translator.GetString("OutputFormat"))).CreateNavigator();

			nodes = nav.Select("OutputFormat/format");
			while (nodes.MoveNext())
			{
				formatBox.Items.Add(nodes.Current.Value);
			}
			
			Reset();
		}


		//========================================================================================
		// ChangeFormat()
		//========================================================================================

		private void ChangeFormat (object sender, EventArgs e)
		{
			switch (formatBox.SelectedIndex)
			{
				case 0:
					rightAlignCheck.Enabled = true;
					delimeterLabel.Enabled = false;
					delimeterBox.Enabled = false;
					break;

				case 1:
				case 2:
				case 3:
					rightAlignCheck.Enabled = false;
					delimeterLabel.Enabled = false;
					delimeterBox.Enabled = false;
					break;

				case 4:
					rightAlignCheck.Enabled = false;
					delimeterLabel.Enabled = true;
					delimeterBox.Enabled = true;
					break;
			}
		}


		//========================================================================================
		// ChangeTarget()
		//========================================================================================

		private void ChangeTarget (object sender, EventArgs e)
		{
			switch (targetBox.SelectedIndex)
			{
				case 0: // text
					formatBox.Enabled = true;
					ChangeFormat(sender, e);
					maxCharBox.Enabled = true;
					printHeadersCheck.Enabled = true;
					rightAlignCheck.Enabled = true;
					outputQueryCheck.Enabled = true;
					cleanNewlinesCheck.Enabled = true;
					break;

				case 1: // grid
					formatBox.Enabled = false;
					ChangeFormat(sender, e);
					maxCharBox.Enabled = true;
					printHeadersCheck.Enabled = false;					
					rightAlignCheck.Enabled = true;
					outputQueryCheck.Enabled = false;
					cleanNewlinesCheck.Enabled = false;
					break;

				case 2: // xml
					formatBox.Enabled = false;
					ChangeFormat(sender, e);
					maxCharBox.Enabled = false;
					printHeadersCheck.Enabled = false;
					rightAlignCheck.Enabled = false;
					outputQueryCheck.Enabled = true;
					cleanNewlinesCheck.Enabled = false;
					break;
			}
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		public override void Reset ()
		{
			targetBox.SelectedIndex = (int)Enum.Parse(
			    typeof(Database.ResultTarget),
			    UserOptions.GetString("results/general/target"),
			    false);

			formatBox.SelectedIndex = (int)Enum.Parse(
			    typeof(Database.ResultFormat),
				UserOptions.GetString("results/general/format"),
			    false);

			delimeterBox.Text = UserOptions.GetString("results/general/delimeter");
			maxCharBox.Value = UserOptions.GetInt("results/general/maxChar");
			printHeadersCheck.Checked = UserOptions.GetBoolean("results/general/printHeader");
			scrollResultsCheck.Checked = UserOptions.GetBoolean("results/general/scrollResults");
			rightAlignCheck.Checked = UserOptions.GetBoolean("results/general/rightAlign");
			outputQueryCheck.Checked = UserOptions.GetBoolean("results/general/outputQuery");
			dbmsOutputCheck.Checked = UserOptions.GetBoolean("results/general/dbmsOutput");
			cleanNewlinesCheck.Checked = UserOptions.GetBoolean("results/general/cleanNewlines");
		}


		//========================================================================================
		// SaveOptions()
		//========================================================================================

		public override void SaveOptions ()
		{
			UserOptions.SetValue("results/general/target", (int)targetBox.SelectedIndex);
			UserOptions.SetValue("results/general/format", (int)formatBox.SelectedIndex);
			UserOptions.SetValue("results/general/delimeter", delimeterBox.Text);
			UserOptions.SetValue("results/general/maxChar", maxCharBox.Value.ToString());
			UserOptions.SetValue("results/general/printHeader", printHeadersCheck.Checked);
			UserOptions.SetValue("results/general/scrollResults", scrollResultsCheck.Checked);
			UserOptions.SetValue("results/general/rightAlign", rightAlignCheck.Checked);
			UserOptions.SetValue("results/general/outputQuery", outputQueryCheck.Checked);
			UserOptions.SetValue("results/general/dbmsOutput", dbmsOutputCheck.Checked);
			UserOptions.SetValue("results/general/cleanNewlines", cleanNewlinesCheck.Checked);
		}
	}
}
