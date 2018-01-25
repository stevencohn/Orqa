//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Represents an Oracle database server connection.  All instances are exposed
// through this one connection.  Multiple Orqa query windows may reuse this one
// connection managed by its reference count.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa
{
	using System;
	using System.Collections.Generic;
	using System.Security;
	using System.Windows.Forms;
	using River.Orqa.Database;
	using River.Orqa.Dialogs;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class Program
	//********************************************************************************************

	static class Program
	{
		
		//========================================================================================
		// Main()
		//========================================================================================

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		[STAThread]
		static void Main (string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			try
			{
				if (IsValidEnvironment())
				{
					Dialogs.ConnectDialog dialog = new Dialogs.ConnectDialog();
					DialogResult result = dialog.ShowDialog();

					if (result == DialogResult.OK)
					{
						if ((args.Length > 0) && System.IO.File.Exists(args[0]))
						{
							Application.Run(new MainWindow(dialog.Connection, args[0]));
						}
						else
						{
							Application.Run(new MainWindow(dialog.Connection));
						}
					}
				}
			}
			catch (SecurityException)
			{
				Translator translator = new Translator("Orqa");

				MessageBox.Show(
					translator.GetString("SecurityExceptionMsg"),
					translator.GetString("SecurityExceptionTitle"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
					);
			}
			catch (Exception exc)
			{
				ExceptionDialog dialog = new ExceptionDialog(exc);
				dialog.ShowDialog();
			}
		}

	
		//========================================================================================
		// IsValidEnvironment()
		//========================================================================================

		private static bool IsValidEnvironment ()
		{
			if (!DatabaseSetup.IsOracleInstalled)
			{
				//Translator translator = new Translator("Orqa");

				//MessageBox.Show(
				//	translator.GetString("OracleNotInstalledMsg"),
				//	translator.GetString("OracleNotInstalledTitle"),
				//	MessageBoxButtons.OK,
				//	MessageBoxIcon.Error
				//	);

				//return false;
			}

			if (!DatabaseSetup.IsOdpInstalled)
			{
				//Translator translator = new Translator("Orqa");

				//MessageBox.Show(
				//	translator.GetString("OdpNotInstalledMsg"),
				//	translator.GetString("OdpNotInstalledTitle"),
				//	MessageBoxButtons.OK,
				//	MessageBoxIcon.Error
				//	);

				//return false;
			}

			return true;
		}
	}
}