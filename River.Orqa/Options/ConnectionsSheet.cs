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
	// class ConnectionsSheet
	//********************************************************************************************

	internal partial class ConnectionsSheet : SheetBase
	{
		private Translator translator;


		//========================================================================================
		// Constructor
		//========================================================================================

		public ConnectionsSheet ()
			: base()
		{
			InitializeComponent();

			translator = new Translator("Options");

			// Parse Modes

			XPathNavigator nav = new XPathDocument(
				new StringReader(translator.GetString("ParseModes"))).CreateNavigator();

			XPathNodeIterator nodes = nav.Select("ParseModes/mode");
			while (nodes.MoveNext())
			{
				parseModeBox.Items.Add(nodes.Current.Value);
			}

			Reset();
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		public override void Reset ()
		{
			loginTimeoutBox.Value = UserOptions.GetInt("connections/loginTimeout");
			queryTimeoutBox.Value = UserOptions.GetInt("connections/queryTimeout");

			parseModeBox.SelectedIndex = (int)Enum.Parse(
				typeof(Database.ParseMode),
				UserOptions.GetString("connections/parseMode"),
				false);

			planTableBox.Text = UserOptions.GetString("connections/planTable");
			utilProcedureBox.Text = UserOptions.GetString("connections/utilProcedure");
		}


		//========================================================================================
		// SaveOptions()
		//========================================================================================

		public override void SaveOptions ()
		{
			UserOptions.SetValue("connections/loginTimeout", (int)loginTimeoutBox.Value);
			UserOptions.SetValue("connections/queryTimeout", (int)queryTimeoutBox.Value);

			UserOptions.SetValue(
				"connections/parseMode",
				((ParseMode)parseModeBox.SelectedIndex).ToString());

			UserOptions.SetValue("connections/planTable", planTableBox.Text);
			UserOptions.SetValue("connections/utilProcedure", utilProcedureBox.Text);
		}
	}
}
