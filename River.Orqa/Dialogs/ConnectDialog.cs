//************************************************************************************************
// Copyright � 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Presents a dialog box allowing the user to specify database connection properties.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.Configuration;
	using System.Diagnostics;
	using System.ServiceProcess;
	using System.Windows.Forms;
	using River.Orqa.Database;
	using River.Orqa.Options;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class ConnectDialog
	//********************************************************************************************

	internal partial class ConnectDialog : Form
	{
		private static readonly char pwdChar = (char)(0x25CF);
		private DatabaseConnection connection = null;
		private StatusStrip status;
		private Translator translator;


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new connection dialog box.
		/// </summary>

		public ConnectDialog ()
		{
			InitializeComponent();

			translator = new Translator("Dialogs.ConnectDialog");

			passwordBox.PasswordChar = pwdChar;
			modeBox.SelectedIndex = 0;

			var pwd = ConfigurationManager.AppSettings["everest_password"];
			if (!string.IsNullOrEmpty(pwd))
			{
				userIDBox.Text = "everest";
				passwordBox.Text = pwd;
			}

			ToolTip tip = new ToolTip();
			tip.SetToolTip(servicesButton, translator.GetString("servicesButton_toolTip"));

			Version version = this.GetType().Assembly.GetName().Version;

			versionLabel.Text = String.Format(
				translator.GetString("VersionLabel"), version.Major, version.Minor);

			PopulateTnsSelection();

			string lastConnectMethod = UserOptions.GetString("general/lastConnectMethod");
			if (lastConnectMethod == null)
			{
				if (HasLocalService)
					localButton.Checked = true;
				else
					tnsButton.Checked = true;
			}
			else
			{
				if (lastConnectMethod.Equals("remote"))
				{
					remoteButton.Checked = true;
				}
				else if (lastConnectMethod.Equals("local") && HasLocalService)
				{
					localButton.Checked = true;
				}
				else
				{
					tnsButton.Checked = true;
				}
			}

			if ((tnsBox.Text.Length == 0) && tnsButton.Checked)
			{
				tnsBox.Select();
				tnsBox.Focus();
			}
			else if (userIDBox.Text.Length == 0)
			{
				userIDBox.Select();
				userIDBox.Focus();
			}
			else
			{
				passwordBox.Select();
				passwordBox.Focus();
			}

			status = null;
		}


		//========================================================================================
		// HasLocalService
		//========================================================================================

		private bool HasLocalService
		{
			get
			{
				ServiceController[] allServices = ServiceController.GetServices();
				ServiceController service;
				System.Collections.IEnumerator e = allServices.GetEnumerator();
				bool foundService = false;

				while (e.MoveNext() && !foundService)
				{
					service = (ServiceController)e.Current;
					if (foundService = service.ServiceName.StartsWith("OracleService"))
					{
						foundService = (service.Status == ServiceControllerStatus.Running);
					}
					else if (service.ServiceName.CompareTo("OracleService") > 0)
					{
						// Unfortunately, the GetServices() method doesn't appear to return
						// services in alphabetical order, so this break may be premature...

						//break;
					}
				}

				e = null;
				service = null;
				allServices = null;

				return foundService;
			}
		}


		//========================================================================================
		// PopulateTnsSelection()
		//========================================================================================

		private void PopulateTnsSelection ()
		{
			TnsName[] names = Tns.Names;

			if (names.Length == 0)
			{
				//MessageBox.Show(
				//	"No TNS names were found\n"
				//	+ "RegPath: " + Tns.RegistryPath
				//	+ "TnsPath: " + Tns.TnsNameOraPath,
				//	"No TNS names",
				//	MessageBoxButtons.OK, MessageBoxIcon.Warning); 

				return;
			}

			tnsBox.Items.Clear();

			int ixEmpower = -1;
			int ixEverest = -1;
			int ixUNIFI = -1;

			foreach (TnsName name in names)
			{
				tnsBox.Items.Add(name.Name);

				userIDBox.Text = userIDBox.Text.Trim();
				if (userIDBox.Text.Length == 0)
				{
					if (name.Name.StartsWith("unifi") || name.Name.StartsWith("everest"))
					{
						userIDBox.Text = "everest";

						var pwd = ConfigurationManager.AppSettings["everest_password"];
						if (!string.IsNullOrEmpty(pwd))
						{
							passwordBox.Text = pwd;
						}
					}
					else if (name.Name.StartsWith("empow"))
					{
						userIDBox.Text = "system";
						var pwd = ConfigurationManager.AppSettings["system_password"];
						if (!string.IsNullOrEmpty(pwd))
						{
							passwordBox.Text = pwd;
						}
					}
				}

				// default selection if present

				if (name.Name.StartsWith("unifi"))
					ixUNIFI = tnsBox.Items.Count - 1;

				if (name.Name.Equals("everest"))
					ixEverest = tnsBox.Items.Count - 1;

				if (name.Name.StartsWith("empow"))
					ixEmpower = tnsBox.Items.Count - 1;
			}

			if (ixUNIFI >= 0)
			{
				tnsBox.SelectedIndex = ixUNIFI;
			}
			else if (ixEverest >= 0)
			{
				tnsBox.SelectedIndex = ixEverest;
			}
			else if (ixEmpower >= 0)
			{
				tnsBox.SelectedIndex = ixEmpower;
			}
			else
			{
				tnsBox.SelectedIndex = 0;
			}

			// if no selection made then default to first in list
			if (tnsBox.SelectedIndex < 0)
				if (tnsBox.Items.Count > 0)
					tnsBox.SelectedIndex = 0;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the database connection specified by the dialog entry information
		/// and accepted by the user.
		/// </summary>

		public DatabaseConnection Connection
		{
			get { return connection; }
		}


		//========================================================================================
		// SelectMethod()
		//========================================================================================

		private void SelectMethod (object sender, System.EventArgs e)
		{
			if (localButton.Checked)
			{
				tnsBox.Enabled = false;
				hostBox.Enabled = false;
				portBox.Enabled = false;
				serviceBox.Enabled = false;
			}
			else if (tnsButton.Checked)
			{
				tnsBox.Enabled = true;
				hostBox.Enabled = false;
				portBox.Enabled = false;
				serviceBox.Enabled = false;
			}
			else
			{
				tnsBox.Enabled = false;
				hostBox.Enabled = true;
				portBox.Enabled = true;
				serviceBox.Enabled = true;

				if (hostBox.Text.Length == 0)
					hostBox.Text = "127.0.0.1";

				if (portBox.Text.Length == 0)
					portBox.Text = "1521";

				if (serviceBox.Text.Length == 0)
					serviceBox.Text = tnsBox.Text;
			}
		}


		//========================================================================================
		// OK()
		//========================================================================================

		private void OK (object sender, System.EventArgs e)
		{
			if (Connect())
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}


		//========================================================================================
		// Connect()
		//========================================================================================

		private bool Connect ()
		{
			this.Cursor = Cursors.WaitCursor;
			this.Capture = true;

			EnableControls(false);

			string userID = userIDBox.Text.Trim();
			string password = passwordBox.Text.Trim();

			if (modeBox.SelectedIndex > 0)
			{
				userID += ";DBA Privilege=" + modeBox.Text;
				//password += " as " + modeBox.Text;
			}

			if (localButton.Checked)
			{
				if (status != null)
				{
					status.Text = String.Format(
						translator.GetString("ConnectingToLocalMsg"),
						userID, password
						);
				}

				connection = new DatabaseConnection(userID, password);
			}
			else if (tnsButton.Checked)
			{
				string tns = tnsBox.Text.Trim().ToUpper();

				if (status != null)
				{
					status.Text = String.Format(
						translator.GetString("ConnectingToTNSMsg"),
						tns
						);
				}

				connection = new DatabaseConnection(userID, password, tns);
			}
			else
			{
				string host = hostBox.Text.Trim().ToUpper();
				int port = int.Parse(portBox.Text.Trim());
				string service = serviceBox.Text.Trim().ToUpper();

				if (status != null)
				{
					status.Text = String.Format(
						translator.GetString("ConnectingToRemoteMsg"),
						host, port, service
						);
				}

				connection = new DatabaseConnection(userID, password, host, port, service);
			}

			if (status != null)
				status.Text = String.Empty;

			if (connection != null)
			{
				try
				{
					object con = connection.OraConnection;
				}
				catch (Exception exc)
				{
					ExceptionDialog.ShowException(exc);
					connection = null;
				}
			}

			this.Capture = false;
			this.Cursor = Cursors.Default;

			if (connection != null)
			{
				string lastConnectMethod = String.Empty;
				if (remoteButton.Checked)
					lastConnectMethod = "remote";
				else if (localButton.Checked)
					lastConnectMethod = "local";
				else
					lastConnectMethod = "tns";

				UserOptions.SetValue("general/lastConnectMethod", lastConnectMethod);

				return true;
			}

			EnableControls(true);
			return false;
		}


		//========================================================================================
		// DoOpenWithoutConnection()
		//		Open Orqa without connecting to the database.
		//========================================================================================

		private void DoOpenWithoutConnection (object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

	
		//========================================================================================
		// EnableControls()
		//========================================================================================

		private void EnableControls (bool enabled)
		{
			okButton.Enabled = enabled;
			servicesButton.Enabled = enabled;
		}


		//========================================================================================
		// OpenServiceController()
		//========================================================================================

		private void OpenServiceController (object sender, System.EventArgs e)
		{
			ServiceControllerDialog dialog = new ServiceControllerDialog();
			dialog.ShowDialog(this);
		}


		//========================================================================================
		// Cancel()
		//========================================================================================

		private void Cancel (object sender, System.EventArgs e)
		{
			userIDBox.Text = String.Empty;
			passwordBox.Text = String.Empty;
			tnsBox.Text = String.Empty;
			hostBox.Text = String.Empty;
			portBox.Text = String.Empty;
			serviceBox.Text = String.Empty;

			connection = null;

			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}


		//========================================================================================
		// DoAboutHelp()
		//      Activated via versionLinkLabel.
		//========================================================================================
		
		private void DoAboutHelp (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Dialogs.AboutDialog about = new Dialogs.AboutDialog();
			about.DisableConnections();
			about.ShowDialog(this);
			about = null;
		}


		//========================================================================================
		// DoEditTNS()
		//========================================================================================
		
		private void DoEditTNS (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo("notepad", Tns.TnsNameOraPath);
			process.EnableRaisingEvents = true;
			process.Exited += new EventHandler(EditTNS_Exited);

			tnsLinkLabel.Enabled = false;

			process.Start();
		}

		private void EditTNS_Exited (object sender, EventArgs e)
		{
			Tns.Refresh();
			PopulateTnsSelection();
			tnsLinkLabel.Enabled = true;
		}
	}
}