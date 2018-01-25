//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// The Orqa About box.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.Collections;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class AboutDialog
	//********************************************************************************************

	/// <summary>
	/// 
	/// </summary>

	internal partial class AboutDialog : Form
	{
		private Translator translator;

		#region struct Version
		class Version
		{
			public string instance;
			public string host;
			public string version;
			public DateTime startup;
			public SortedList parameters;

			public Version ()
			{
				parameters = new SortedList();
			}
		}
		#endregion struct Version


		/// <summary>
		/// 
		/// </summary>

		public AboutDialog ()
		{
			InitializeComponent();

			translator = new Translator("Dialogs.AboutDialog");

			System.Version version = this.GetType().Assembly.GetName().Version;

			versionLabel.Text = String.Format(
				translator.GetString("VersionLabel"),
				version.Major,
				version.Minor,
				new DateTime(2000, 1, 1).AddDays(version.Build).ToShortDateString());

			copyrightLabel.Text = String.Format(
				translator.GetString("Copyright"), DateTime.Now.Year);

			DisplayLocalConfiguration();

			historyBox.Rtf = translator.GetString("History", "River.Orqa", "Resources.Orqa");
		}


		public void DisableConnections ()
		{
			ListViewItem item = new ListViewItem(translator.GetString("Unavailable"));
			item.Font = new Font(item.Font, FontStyle.Italic);
			item.ForeColor = Color.Maroon;
			conView.Items.Add(item);

			tabset.SelectedTab = locPage;
		}


		public DatabaseConnection[] Servers
		{
			set
			{
				SortedList list = new SortedList();

				foreach (DatabaseConnection con in value)
				{
					using (var cmd = new OracleCommand(
						"SELECT instance_name, version, startup_time FROM V$Instance",
						con.OraConnection
						))
					{
						using (OracleDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								var v = new Version();
								v.instance = (string)reader["instance_name"];
								v.host = (con.HostName == null ? "local" : con.HostName.ToLower());
								v.version = (string)reader["version"];
								v.startup = (DateTime)reader["startup_time"];

								string name = v.instance + "@" + v.host;
								list.Add(name, v);

								cmd.CommandText
									= "SELECT LOWER(COALESCE(N.parameter, P.name)) AS name,"
									+ "       COALESCE(COALESCE(N.value, P.value), '') AS value"
									+ "  FROM SYS.V_$PARAMETER P"
									+ "  LEFT OUTER JOIN SYS.V_$NLS_PARAMETERS N"
									+ "    ON LOWER(N.parameter) = LOWER(P.name)"
									+ " ORDER BY 1";

								using (OracleDataReader preader = cmd.ExecuteReader())
								{
									while (preader.Read())
									{
										v.parameters.Add(
											preader["name"].ToString(),
											preader["value"].ToString());
									}

									preader.Close();
								}
							}

							reader.Close();
						}
					}
				}

				foreach (Version v in list.Values)
				{
					ListViewItem item = new ListViewItem(v.instance);
					item.SubItems.Add(v.host);
					item.SubItems.Add(v.version);
					item.SubItems.Add(v.startup.ToString("dd-MMM-yyyy hh:mm"));
					item.Tag = v;

					conView.Items.Add(item);
				}

				if (conView.Items.Count > 0)
				{
					conView.Items[0].Selected = true;
				}
			}
		}


		private void DisplayLocalConfiguration ()
		{
			ListViewItem item;

			foreach (TnsName name in Tns.AllNames)
			{
				StringBuilder info = new StringBuilder();
				info.Append(name.Location.ToString());
				if ((name.Info != null) && (name.Info != String.Empty))
					info.Append(" (" + name.Info + ")");

				item = new ListViewItem(
					new string[] { name.Name, info.ToString() });

				item.ForeColor = Color.ForestGreen;

				configView.Items.Add(item);
			}

			configView.Items.Add(new ListViewItem(
				new string[] { translator.GetString("OracleHome"), DatabaseSetup.OracleHome }));

			configView.Items.Add(new ListViewItem(
				new string[] { translator.GetString("TnsNames"), Tns.TnsNameOraPath } ));

			configView.Items.Add(new ListViewItem(
				new string[] { translator.GetString("RegistryKey"), DatabaseSetup.OracleHomeKey }));

			configView.Items.Add(new ListViewItem(
				new string[] { translator.GetString("RegistryHome"), DatabaseSetup.OracleHomeName } ));

			configView.Items.Add(new ListViewItem(
				new string[] { translator.GetString("RegistrySID"), DatabaseSetup.OracleSid } ));
		}


		private void DoSelectTab (object sender, EventArgs e)
		{
			switch (tabset.SelectedIndex)
			{
				case 0:
					conView.Focus();
					conView.Select();
					this.AcceptButton = okButton1;
					this.CancelButton = okButton1;
					break;

				case 1:
					configView.Focus();
					configView.Select();
					this.AcceptButton = okButton2;
					this.CancelButton = okButton2;
					break;

				case 2:
					this.AcceptButton = okButtonHistory;
					this.CancelButton = okButtonHistory;
					break;
			}
		}

		private void DoSelectConnection (object sender, EventArgs e)
		{
			if (conView.SelectedItems.Count > 0)
			{
				paramView.Items.Clear();

				Version v = (Version)conView.SelectedItems[0].Tag;

				if (v != null)
				{
					IDictionaryEnumerator en = v.parameters.GetEnumerator();
					while (en.MoveNext())
					{
						ListViewItem item = new ListViewItem((string)en.Key);
						item.SubItems.Add((string)en.Value);

						paramView.Items.Add(item);
					}
				}
			}
		}


		private void DoClose (object sender, EventArgs e)
		{
			this.Close();
		}
	}
}