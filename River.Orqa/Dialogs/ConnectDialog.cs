//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
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
    using System.Xml.Linq;
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


		private class Settings
		{
			public string UserID;
			public string Password;
			public string Mode;
			public string Method;
			public string Tns;
			public string Host;
			public string Port;
			public string Service;
		}


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
			var settings = GetSettings();

			passwordBox.PasswordChar = pwdChar;
			modeBox.SelectedIndex = 0;

			userIDBox.Text = settings.UserID ?? "sys";

			var pwd = ConfigurationManager.AppSettings[userIDBox.Text + "_password"];
			if (!string.IsNullOrEmpty(pwd))
			{
				passwordBox.Text = pwd;
			}

			if (!string.IsNullOrEmpty(settings.Mode))
			{
				modeBox.SelectedItem = settings.Mode.ToUpper();
			}

			ToolTip tip = new ToolTip();
			tip.SetToolTip(servicesButton, translator.GetString("servicesButton_toolTip"));

			Version version = this.GetType().Assembly.GetName().Version;

			versionLabel.Text = String.Format(
				translator.GetString("VersionLabel"), version.Major, version.Minor);

			PopulateTnsSelection();

			switch (settings.Method)
			{
				case "tns":
					tnsButton.Checked = true;
					tnsBox.Text = settings.Tns ?? string.Empty;
					break;

				case "remote":
					remoteButton.Checked = true;
					hostBox.Text = settings.Host ?? "localhost";
					portBox.Text = settings.Port ?? "1521";
					serviceBox.Text = settings.Service ?? string.Empty;
					break;

				default:
					if (HasLocalService)
						localButton.Checked = true;
					else
						tnsButton.Checked = true;
					break;
			}

			if (userIDBox.Text.Length == 0)
			{
				userIDBox.Select();
				userIDBox.Focus();
			}
			else if (passwordBox.Text.Length == 0)
			{
				passwordBox.Select();
				passwordBox.Focus();
			}
			else if ((tnsBox.Text.Length == 0) && tnsButton.Checked)
			{
				tnsBox.Select();
				tnsBox.Focus();
			}
			else
			{
				hostBox.Select();
				hostBox.Focus();
			}

			status = null;
		}


		private Settings GetSettings ()
		{
			var element = UserOptions.GetElement("general/lastConnection");

			XElement e;

			if (element != null)
			{
				return new Settings
				{
					UserID = element.Element("userID")?.Value,
					Mode = element.Element("mode")?.Value,
					Method = element.Element("method")?.Value,
					Tns = element.Element("tns")?.Value,
					Host = element.Element("host")?.Value,
					Port = element.Element("port")?.Value,
					Service = element.Element("service")?.Value
				};
			}

			return new Settings
			{
				UserID = "sys",
				Mode = "Normal",
				Method = "local",
				Tns = string.Empty,
				Host = "localhost",
				Port = "1521",
				Service = string.Empty
			};
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
					if (name.Name.Equals("unifi", StringComparison.InvariantCultureIgnoreCase) ||
						name.Name.Equals("everest", StringComparison.InvariantCultureIgnoreCase))
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

				if ((ixUNIFI < 0) && name.Name.StartsWith("unifi"))
					ixUNIFI = tnsBox.Items.Count - 1;

				if ((ixEverest < 0) && name.Name.StartsWith("everest"))
					ixEverest = tnsBox.Items.Count - 1;

				if ((ixEmpower < 0) && name.Name.StartsWith("empow"))
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

			var settings = new XElement("lastConnection");

			string userID = userIDBox.Text.Trim();
			string password = passwordBox.Text.Trim();

			settings.Add(new XElement("userID", userID));
			settings.Add(new XElement("mode", modeBox.Text));

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

				settings.Add(new XElement("method", "local"));
			}
			else if (tnsButton.Checked)
			{
				string tns = tnsBox.Text.Trim();

				if (status != null)
				{
					status.Text = String.Format(
						translator.GetString("ConnectingToTNSMsg"),
						tns
						);
				}

				connection = new DatabaseConnection(userID, password, tns);

				settings.Add(new XElement("method", "tns"));
				settings.Add(new XElement("tns", tns));
			}
			else
			{
				string host = hostBox.Text.Trim();
				int port = int.Parse(portBox.Text.Trim());
				string service = serviceBox.Text.Trim();

				if (status != null)
				{
					status.Text = String.Format(
						translator.GetString("ConnectingToRemoteMsg"),
						host, port, service
						);
				}

				connection = new DatabaseConnection(userID, password, host, port, service);

				settings.Add(new XElement("method", "remote"));
				settings.Add(new XElement("host", host));
				settings.Add(new XElement("port", port));
				settings.Add(new XElement("service", service));
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
				UserOptions.SetValue("general/lastConnection", settings);
				UserOptions.Save();

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