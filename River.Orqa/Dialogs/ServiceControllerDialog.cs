//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Presents a dialog box allowing the user start/stop/restart
// Oracle specific Windows services.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ServiceProcess;
	using System.Threading;
	using System.Windows.Forms;
	using River.Orqa.Controls;
	using River.Orqa.Resources;
	using River.Native;
	using System.Security.Permissions;
	using System.Security.Principal;


	//********************************************************************************************
	// class ServiceControllerDialog
	//********************************************************************************************

	internal partial class ServiceControllerDialog : Form
	{
		private delegate void RefreshHandler ();
		private delegate void StringInvoker (string s);

		private string localhost;
		private string hostName;
		private bool isAdministrator;
		private List<Thread> workers;
		private SortedList<string, ServiceItem> services;
		private ServiceManager serviceManager;
		private System.Threading.Timer refreshTimer;
		private Resources.Translator translator;

		#region class ServiceItem
		private class ServiceItem
		{
			private ServiceController service;
			private ServiceControllerStatus status;
			private ServiceStartMode startMode;

			public ServiceItem (ServiceController service)
			{
				this.service = service;
				this.status = service.Status;
				this.startMode = ServiceStartMode.Automatic;
			}

			public ServiceController Controller { get { return service; } }
			public string ServiceName { get { return service.ServiceName; } }
			public ServiceStartMode StartMode { get { return startMode; } set { startMode = value; } }
			public ServiceControllerStatus Status { get { return status; } set { status = value; } }
		}
		#endregion class ServiceItem

		#region class ThreadItem
		private class ThreadItem
		{
			public ServiceController Service { get; set; }
			public Thread Worker { get; set; }
		}
		#endregion class ThreadItem


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new service controller dialog box.
		/// </summary>

		public ServiceControllerDialog ()
		{
			InitializeComponent();

			translator = new Translator("Dialogs.ServiceControllerDialog");

			serviceManager = null;
			refreshTimer = null;

			try
			{ HostName = localhost = System.Net.Dns.GetHostName(); }
			catch
			{ HostName = String.Empty; }

			machineBox.SelectedIndex = 0;

			workers = new List<Thread>();
			services = new SortedList<string, ServiceItem>();

			WindowsIdentity identity = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(identity);
			isAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
			if (!isAdministrator &&
				(WowEnvironment.Platform == WowEnvironment.WindowsPlatform.Windows7))
			{
				statusBox.Text = "Must run as admin to control services!";
				startButton.Enabled = false;
				stopButton.Enabled = false;
				restartButton.Enabled = false;
			}

			refreshTimer = new System.Threading.Timer(
				new TimerCallback(DiscoverServices), null, 0, 1000);
		}


		//========================================================================================
		// Properties
		//========================================================================================

		private string HostName
		{
			set
			{
				if (value == null)
					return;

				string name = value.ToUpper().Trim();
				if (name.Length == 0)
					return;

				if (refreshTimer != null)
					refreshTimer.Dispose();

				hostName = name;

				if (!machineBox.Items.Contains(hostName))
				{
					machineBox.Items.Insert(0, hostName);
				}

				serviceView.Columns[0].Text = String.Format(
					translator.GetString("ServicesTitle"), hostName);

				serviceView.Items.Clear();

				if (refreshTimer != null)
				{
					refreshTimer = new System.Threading.Timer(
						new TimerCallback(DiscoverServices), null, 0, 1000);
				}
			}
		}


		//========================================================================================
		// DiscoverServices()
		//========================================================================================

		private void DiscoverServices ()
		{
			ServiceController[] allServices;

			try
			{
				if (hostName.Length == 0)
					allServices = ServiceController.GetServices();
				else
					allServices = ServiceController.GetServices(hostName);
			}
			catch
			{
				// hostName is probably unreachable
				return;
			}

			ServiceController controller;
			ServiceItem service;
			IEnumerator e = allServices.GetEnumerator();

			string servicePrefix = translator.GetString("ServicePrefix");

			while (e.MoveNext())
			{
				controller = (ServiceController)e.Current;
				if (controller.ServiceName.StartsWith(servicePrefix)) // Oracle
				{
					if (!services.TryGetValue(controller.ServiceName, out service))
					{
						service = new ServiceItem(controller);
						services.Add(service.ServiceName, service);
					}

					service.Status = controller.Status;

					if (serviceManager == null)
					{
						serviceManager = new ServiceManager(hostName);

						try
						{
							serviceManager.Open();
						}
						catch (System.Runtime.InteropServices.ExternalException)
						{
						}
					}

					try
					{
						service.StartMode = serviceManager.GetServiceStartMode(service.ServiceName);
					}
					catch (System.Runtime.InteropServices.ExternalException)
					{
					}
				}
			}

			RefreshServiceView();
		}


		private void RefreshServiceView ()
		{
			if (this.InvokeRequired)
			{
				if (!this.IsDisposed)
					this.Invoke(new RefreshHandler(RefreshServiceView));

				return;
			}

			ListViewItem item;
			string status;
			string startup;
			int selected = (serviceView.SelectedIndices.Count == 0 ? -1 : serviceView.SelectedIndices[0]);

			bool mustPopulate = (serviceView.Items.Count == 0);
			ServiceItem service;

			string dbServicePrefix = translator.GetString("DBServicePrefix");

			serviceView.BeginUpdate();

			for (int i = 0; i < services.Count; i++)
			{
				service = services.Values[i];

				if (isAdministrator)
				{
					if (service.StartMode == ServiceStartMode.Disabled)
						status = translator.GetString("DisabledMsg");
					else
						status = service.Status.ToString();

					startup = service.StartMode.ToString();
				}
				else
				{
					status = "-";
					startup = "-";
				}

				if (mustPopulate)
				{
					item = new ListViewItem(new string[] { service.ServiceName, status, startup });
					item.Tag = service;
					serviceView.Items.Add(item);
				}
				else
				{
					item = serviceView.Items[i];
					item.SubItems[1].Text = status;
					item.SubItems[2].Text = startup;
				}

				if (service.StartMode == ServiceStartMode.Disabled)
					item.ImageIndex = 2;
				else
					item.ImageIndex = (service.Status == ServiceControllerStatus.Running ? 0 : 1);

				if (service.ServiceName.StartsWith(dbServicePrefix)) // OracleService
				{
					item.ForeColor = Color.Blue;
				}
			}

			if (selected > -1)
				serviceView.Items[selected].Selected = true;

			serviceView.EndUpdate();
		}


		// used by the timercallback in the start/stop/restart methods

		private void DiscoverServices (object state)
		{
			if (refreshTimer != null)
			{
				DiscoverServices();
			}
		}


		// called by the Refresh button.

		private void RefreshServiceList (object sender, System.EventArgs e)
		{
			DiscoverServices();
		}


		//========================================================================================
		// DoSetMachine()
		//========================================================================================

		private void DoMachineKeyUp (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				string name = machineBox.Text.Trim();
				if (name.Length > 0)
					HostName = machineBox.Text;
				else
					machineBox.Text = hostName;
			}
		}


		private void DoSetMachine (object sender, EventArgs e)
		{
			HostName = machineBox.Text;
		}


		//========================================================================================
		// SelectService()
		//========================================================================================

		private void SelectService (object sender, System.EventArgs e)
		{
			if (serviceView.SelectedIndices.Count == 0)
			{
				SelectService(ServiceControllerStatus.ContinuePending);
			}
			else
			{
				ServiceItem service = services.Values[serviceView.SelectedIndices[0]];

				if (service.StartMode == ServiceStartMode.Disabled)
					SelectService(ServiceControllerStatus.ContinuePending);
				else
					SelectService(service.Status);
			}
		}


		private void SelectService (ServiceControllerStatus status)
		{
			if (isAdministrator)
			{
				startButton.Enabled = (status == ServiceControllerStatus.Stopped);
				stopButton.Enabled = (status == ServiceControllerStatus.Running);
				restartButton.Enabled = (status == ServiceControllerStatus.Running);
			}
		}


		//========================================================================================
		// Service Controllers
		//========================================================================================

		#region Service Controllers

		private void StartService (object sender, System.EventArgs e)
		{
			if (serviceView.SelectedIndices.Count > 0)
			{
				SelectService(ServiceControllerStatus.ContinuePending);

				Thread worker = new Thread(new ParameterizedThreadStart(StartServiceWorker));
				workers.Add(worker);
				worker.Start(worker);

				serviceView.SelectedItems[0].ImageIndex = 0;

				serviceView.Focus();
				SelectService(ServiceControllerStatus.Running);
			}
		}


		private void StartServiceWorker (object param)
		{
			Thread worker = param as Thread;
			if ((worker != null) && workers.Contains(worker))
			{
				workers.Remove(worker);
			}

			ServiceController service = services.Values[serviceView.SelectedIndices[0]].Controller;

			this.Invoke(new StringInvoker(delegate(string serviceName)
			{
				statusBox.Text = String.Format(
					translator.GetString("StartingMsg"),
					serviceName
					);
			}),
			service.ServiceName);

			service.Start();

			try
			{
				service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
			}
			catch (System.ServiceProcess.TimeoutException)
			{
				//MessageBox.Show(
				//    translator.GetString("StartTimeoutMsg"),
				//    translator.GetString("TimeoutTitle"),
				//    MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				ExceptionDialog.ShowException(exc);
			}

			if (refreshTimer != null)
			{
				DiscoverServices();
			}

			this.Invoke(new MethodInvoker(delegate
			{
				statusBox.Text = String.Empty;
			}));
		}


		private void StopService (object sender, System.EventArgs e)
		{
			if (serviceView.SelectedIndices.Count > 0)
			{
				SelectService(ServiceControllerStatus.ContinuePending);

				Thread worker = new Thread(new ParameterizedThreadStart(StopServiceWorker));
				workers.Add(worker);
				ThreadItem item = new ThreadItem();
				item.Service = services.Values[serviceView.SelectedIndices[0]].Controller;
				item.Worker = worker;
				worker.Start(item);

				serviceView.SelectedItems[0].ImageIndex = 1;

				serviceView.Focus();
				SelectService(ServiceControllerStatus.Stopped);
			}
		}


		private void StopServiceWorker (object param)
		{
			ThreadItem item = param as ThreadItem;

			if (workers.Contains(item.Worker))
			{
				workers.Remove(item.Worker);
			}

			ServiceController service = item.Service;

			this.Invoke(new StringInvoker(delegate(string serviceName)
			{
				statusBox.Text = String.Format(
					translator.GetString("StoppingMsg"),
					serviceName
					);
			}),
			service.ServiceName);

			service.Stop();

			try
			{
				service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 15));
			}
			catch (System.ServiceProcess.TimeoutException)
			{
				//MessageBox.Show(
				//    translator.GetString("StopTimeoutMsg"),
				//    translator.GetString("TimeoutTitle"),
				//    MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				ExceptionDialog.ShowException(exc);
			}

			if (refreshTimer != null)
			{
				DiscoverServices();
			}

			this.Invoke(new MethodInvoker(delegate
			{
				statusBox.Text = String.Empty;
			}));
		}


		private void RestartService (object sender, System.EventArgs e)
		{
			if (serviceView.SelectedIndices.Count > 0)
			{
				SelectService(ServiceControllerStatus.ContinuePending);

				Thread worker = new Thread(new ParameterizedThreadStart(RestartServiceWorker));
				workers.Add(worker);
				worker.Start(worker);

				serviceView.SelectedItems[0].ImageIndex = 0;

				serviceView.Focus();
				SelectService(ServiceControllerStatus.Running);
			}
		}


		private void RestartServiceWorker (object param)
		{
			Thread worker = param as Thread;
			if ((worker != null) && workers.Contains(worker))
			{
				workers.Remove(worker);
			}

			ServiceController service = services.Values[serviceView.SelectedIndices[0]].Controller;

			try
			{
				this.Invoke(new StringInvoker(delegate(string serviceName)
				{
					statusBox.Text = String.Format(
						translator.GetString("StoppingMsg"),
						serviceName
						);
				}),
				service.ServiceName);

				service.Stop();
				service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));

				this.Invoke(new StringInvoker(delegate(string serviceName)
				{
					statusBox.Text = String.Format(
						translator.GetString("StartingMsg"),
						serviceName
						);
				}),
				service.ServiceName);

				service.Start();
				service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));
			}
			catch (System.ServiceProcess.TimeoutException)
			{
				//MessageBox.Show(
				//    translator.GetString("RestartTimeoutMsg"),
				//    translator.GetString("TimeoutTitle"),
				//    MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				ExceptionDialog.ShowException(exc);
			}

			if (refreshTimer != null)
			{
				DiscoverServices();
			}

			this.Invoke(new MethodInvoker(delegate
			{
				statusBox.Text = String.Empty;
			}));
		}

		#endregion Service Controllers


		//========================================================================================
		// Context Menu handlers
		//========================================================================================

		private int x = 0;
		private int y = 0;

		private void DoMouseUp (object sender, MouseEventArgs e)
		{
			this.x = e.X;
			this.y = e.Y;
		}

		private void DoContextMenuOpening (object sender, CancelEventArgs e)
		{
			ListViewItem item = serviceView.GetItemAt(x, y);
			if (item == null || !isAdministrator)
			{
				// only allow context menu when mouse is over an item.
				e.Cancel = true;
				return;
			}

			ServiceItem service = (ServiceItem)item.Tag;

			bool enabled = (service.StartMode != ServiceStartMode.Disabled);
			startToolStripMenuItem.Enabled = enabled && (service.Status == ServiceControllerStatus.Stopped);
			stopToolStripMenuItem.Enabled = enabled && (service.Status == ServiceControllerStatus.Running);
			restartToolStripMenuItem.Enabled = enabled && (service.Status == ServiceControllerStatus.Running);

			switch (service.StartMode)
			{
				case ServiceStartMode.Automatic:
					automaticToolStripMenuItem.Checked = true;
					manualToolStripMenuItem.Checked = false;
					disabledToolStripMenuItem.Checked = false;
					break;

				case ServiceStartMode.Manual:
					automaticToolStripMenuItem.Checked = false;
					manualToolStripMenuItem.Checked = true;
					disabledToolStripMenuItem.Checked = false;
					break;

				case ServiceStartMode.Disabled:
					automaticToolStripMenuItem.Checked = false;
					manualToolStripMenuItem.Checked = false;
					disabledToolStripMenuItem.Checked = true;
					break;
			}
		}


		//========================================================================================
		// StartupMode context menu handlers
		//========================================================================================

		private void DoSetAutomaticMode (object sender, EventArgs e)
		{
			SetStartupMode(ServiceStartMode.Automatic);
		}


		private void DoSetManualMode (object sender, EventArgs e)
		{
			SetStartupMode(ServiceStartMode.Manual);
		}


		private void DoSetDisabledMode (object sender, EventArgs e)
		{
			ServiceItem item = (ServiceItem)serviceView.SelectedItems[0].Tag;
			if (item.Status == ServiceControllerStatus.Running)
			{
				StopService(sender, e);
			}

			SetStartupMode(ServiceStartMode.Disabled);
		}


		private void SetStartupMode (ServiceStartMode mode)
		{
			try
			{
				ServiceItem item = (ServiceItem)serviceView.SelectedItems[0].Tag;
				serviceManager.SetServiceStartMode(item.ServiceName, mode);
			}
			catch (System.Runtime.InteropServices.ExternalException)
			{
			}
		}


		//========================================================================================
		// DoFormClosing()
		//========================================================================================

		private void DoFormClosing (object sender, FormClosingEventArgs e)
		{
			if (refreshTimer != null)
			{
				refreshTimer.Dispose();
				refreshTimer = null;
			}

			while (workers.Count > 0)
			{
				Thread worker = workers[0];
				worker.Abort();
				workers.RemoveAt(0);
			}
		}
	}
}
