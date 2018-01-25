//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// 
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa
{
	using System;
	using System.Windows.Forms;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class Statusbar
	//********************************************************************************************

	internal static class Statusbar
	{
		private static StatusStrip strip;
		private static ICommander commander;
		private static ToolStripStatusLabel status;
		private static ToolStripStatusLabel connections;
		private static string connectionsDesc;


		//========================================================================================
		// Constructor
		//========================================================================================

		static Statusbar ()
		{
			Translator translator = new Translator("Orqa");
			connectionsDesc = translator.GetString("StatusConnections");
			translator = null;
		}


		//========================================================================================
		// Strip
		//========================================================================================

		/// <summary>
		/// Sets the status strip referenced by the Statusbar static class.
		/// This must be called by the main program to initialize the Statusbar.
		/// </summary>

		public static StatusStrip Strip
		{
			set
			{
				strip = value;
				status = (ToolStripStatusLabel)strip.Items["statusMessage"];
				connections = (ToolStripStatusLabel)strip.Items["statusConnections"];
			}
		}


		/// <summary>
		/// Sets the ICommand object used to update ConnectionControls when
		/// the number of connections changes.
		/// </summary>

		public static ICommander Commander
		{
			set { commander = value; }
		}


		//========================================================================================
		// Message
		//========================================================================================

		/// <summary>
		/// Sets the text displayed in the main message part of the status strip.
		/// </summary>

		public static string Message
		{
			set
			{
				status.Text = value;
				Application.DoEvents();
			}
		}


		//========================================================================================
		// Connections
		//========================================================================================

		/// <summary>
		/// Sets the connections label showing the number of active connections currently
		/// open by the application.
		/// </summary>

		public static int Connections
		{
			set
			{
				connections.Text = String.Format(connectionsDesc, value.ToString());

				if (value == 0)
				{
					commander.ConnectControls.IsEnabled = false;
					commander.ExecuteControls.IsEnabled = false;
					commander.PasteControls.IsEnabled = false;
					commander.SaveControls.IsEnabled = false;
					commander.SelectControls.IsEnabled = false;
					commander.TextControls.IsEnabled = false;
				}
			}
		}
	}
}