namespace River.Orqa.Dialogs
{
	partial class ConnectDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectDialog));
			this.headerPanel = new System.Windows.Forms.Panel();
			this.versionLabel = new System.Windows.Forms.LinkLabel();
			this.queryAnalyzerLabel = new System.Windows.Forms.Label();
			this.logoBox = new System.Windows.Forms.PictureBox();
			this.orcaBox = new System.Windows.Forms.PictureBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.servicesButton = new System.Windows.Forms.Button();
			this.userIDLabel = new System.Windows.Forms.Label();
			this.userIDBox = new System.Windows.Forms.TextBox();
			this.passwordBox = new System.Windows.Forms.TextBox();
			this.modeBox = new System.Windows.Forms.ComboBox();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.modeLabel = new System.Windows.Forms.Label();
			this.methodGroupBox = new System.Windows.Forms.GroupBox();
			this.tnsLinkLabel = new System.Windows.Forms.LinkLabel();
			this.serviceLabel = new System.Windows.Forms.Label();
			this.portLabel = new System.Windows.Forms.Label();
			this.hostLabel = new System.Windows.Forms.Label();
			this.tnsLabel = new System.Windows.Forms.Label();
			this.serviceBox = new System.Windows.Forms.TextBox();
			this.portBox = new System.Windows.Forms.TextBox();
			this.hostBox = new System.Windows.Forms.TextBox();
			this.remoteButton = new System.Windows.Forms.RadioButton();
			this.tnsBox = new System.Windows.Forms.ComboBox();
			this.tnsButton = new System.Windows.Forms.RadioButton();
			this.localButton = new System.Windows.Forms.RadioButton();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.headerPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.orcaBox)).BeginInit();
			this.methodGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// headerPanel
			// 
			this.headerPanel.BackColor = System.Drawing.Color.White;
			this.headerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.headerPanel.Controls.Add(this.versionLabel);
			this.headerPanel.Controls.Add(this.queryAnalyzerLabel);
			this.headerPanel.Controls.Add(this.logoBox);
			this.headerPanel.Controls.Add(this.orcaBox);
			resources.ApplyResources(this.headerPanel, "headerPanel");
			this.headerPanel.Name = "headerPanel";
			// 
			// versionLabel
			// 
			resources.ApplyResources(this.versionLabel, "versionLabel");
			this.helpProvider.SetHelpString(this.versionLabel, resources.GetString("versionLabel.HelpString"));
			this.versionLabel.LinkColor = System.Drawing.Color.RoyalBlue;
			this.versionLabel.Name = "versionLabel";
			this.helpProvider.SetShowHelp(this.versionLabel, ((bool)(resources.GetObject("versionLabel.ShowHelp"))));
			this.versionLabel.TabStop = true;
			this.versionLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DoAboutHelp);
			// 
			// queryAnalyzerLabel
			// 
			resources.ApplyResources(this.queryAnalyzerLabel, "queryAnalyzerLabel");
			this.queryAnalyzerLabel.BackColor = System.Drawing.Color.Transparent;
			this.queryAnalyzerLabel.Name = "queryAnalyzerLabel";
			// 
			// logoBox
			// 
			resources.ApplyResources(this.logoBox, "logoBox");
			this.logoBox.Name = "logoBox";
			this.logoBox.TabStop = false;
			// 
			// orcaBox
			// 
			resources.ApplyResources(this.orcaBox, "orcaBox");
			this.orcaBox.Name = "orcaBox";
			this.orcaBox.TabStop = false;
			this.orcaBox.Click += new System.EventHandler(this.DoOpenWithoutConnection);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.helpProvider.SetHelpString(this.cancelButton, resources.GetString("cancelButton.HelpString"));
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.helpProvider.SetShowHelp(this.cancelButton, ((bool)(resources.GetObject("cancelButton.ShowHelp"))));
			this.cancelButton.Click += new System.EventHandler(this.Cancel);
			// 
			// okButton
			// 
			this.helpProvider.SetHelpString(this.okButton, resources.GetString("okButton.HelpString"));
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.helpProvider.SetShowHelp(this.okButton, ((bool)(resources.GetObject("okButton.ShowHelp"))));
			this.okButton.Click += new System.EventHandler(this.OK);
			// 
			// servicesButton
			// 
			this.helpProvider.SetHelpString(this.servicesButton, resources.GetString("servicesButton.HelpString"));
			resources.ApplyResources(this.servicesButton, "servicesButton");
			this.servicesButton.Name = "servicesButton";
			this.helpProvider.SetShowHelp(this.servicesButton, ((bool)(resources.GetObject("servicesButton.ShowHelp"))));
			this.servicesButton.Click += new System.EventHandler(this.OpenServiceController);
			// 
			// userIDLabel
			// 
			resources.ApplyResources(this.userIDLabel, "userIDLabel");
			this.userIDLabel.BackColor = System.Drawing.Color.Transparent;
			this.userIDLabel.Name = "userIDLabel";
			// 
			// userIDBox
			// 
			this.helpProvider.SetHelpString(this.userIDBox, resources.GetString("userIDBox.HelpString"));
			resources.ApplyResources(this.userIDBox, "userIDBox");
			this.userIDBox.Name = "userIDBox";
			this.helpProvider.SetShowHelp(this.userIDBox, ((bool)(resources.GetObject("userIDBox.ShowHelp"))));
			// 
			// passwordBox
			// 
			this.helpProvider.SetHelpString(this.passwordBox, resources.GetString("passwordBox.HelpString"));
			resources.ApplyResources(this.passwordBox, "passwordBox");
			this.passwordBox.Name = "passwordBox";
			this.helpProvider.SetShowHelp(this.passwordBox, ((bool)(resources.GetObject("passwordBox.ShowHelp"))));
			// 
			// modeBox
			// 
			this.modeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.modeBox.FormattingEnabled = true;
			this.helpProvider.SetHelpString(this.modeBox, resources.GetString("modeBox.HelpString"));
			this.modeBox.Items.AddRange(new object[] {
            resources.GetString("modeBox.Items"),
            resources.GetString("modeBox.Items1"),
            resources.GetString("modeBox.Items2")});
			resources.ApplyResources(this.modeBox, "modeBox");
			this.modeBox.Name = "modeBox";
			this.helpProvider.SetShowHelp(this.modeBox, ((bool)(resources.GetObject("modeBox.ShowHelp"))));
			// 
			// passwordLabel
			// 
			resources.ApplyResources(this.passwordLabel, "passwordLabel");
			this.passwordLabel.BackColor = System.Drawing.Color.Transparent;
			this.passwordLabel.Name = "passwordLabel";
			// 
			// modeLabel
			// 
			resources.ApplyResources(this.modeLabel, "modeLabel");
			this.modeLabel.BackColor = System.Drawing.Color.Transparent;
			this.modeLabel.Name = "modeLabel";
			// 
			// methodGroupBox
			// 
			this.methodGroupBox.BackColor = System.Drawing.Color.Transparent;
			this.methodGroupBox.Controls.Add(this.tnsLinkLabel);
			this.methodGroupBox.Controls.Add(this.serviceLabel);
			this.methodGroupBox.Controls.Add(this.portLabel);
			this.methodGroupBox.Controls.Add(this.hostLabel);
			this.methodGroupBox.Controls.Add(this.tnsLabel);
			this.methodGroupBox.Controls.Add(this.serviceBox);
			this.methodGroupBox.Controls.Add(this.portBox);
			this.methodGroupBox.Controls.Add(this.hostBox);
			this.methodGroupBox.Controls.Add(this.remoteButton);
			this.methodGroupBox.Controls.Add(this.tnsBox);
			this.methodGroupBox.Controls.Add(this.tnsButton);
			this.methodGroupBox.Controls.Add(this.localButton);
			resources.ApplyResources(this.methodGroupBox, "methodGroupBox");
			this.methodGroupBox.Name = "methodGroupBox";
			this.methodGroupBox.TabStop = false;
			// 
			// tnsLinkLabel
			// 
			resources.ApplyResources(this.tnsLinkLabel, "tnsLinkLabel");
			this.helpProvider.SetHelpString(this.tnsLinkLabel, resources.GetString("tnsLinkLabel.HelpString"));
			this.tnsLinkLabel.LinkColor = System.Drawing.Color.RoyalBlue;
			this.tnsLinkLabel.Name = "tnsLinkLabel";
			this.helpProvider.SetShowHelp(this.tnsLinkLabel, ((bool)(resources.GetObject("tnsLinkLabel.ShowHelp"))));
			this.tnsLinkLabel.TabStop = true;
			this.tnsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DoEditTNS);
			// 
			// serviceLabel
			// 
			resources.ApplyResources(this.serviceLabel, "serviceLabel");
			this.serviceLabel.Name = "serviceLabel";
			// 
			// portLabel
			// 
			resources.ApplyResources(this.portLabel, "portLabel");
			this.portLabel.Name = "portLabel";
			// 
			// hostLabel
			// 
			resources.ApplyResources(this.hostLabel, "hostLabel");
			this.hostLabel.Name = "hostLabel";
			// 
			// tnsLabel
			// 
			resources.ApplyResources(this.tnsLabel, "tnsLabel");
			this.tnsLabel.Name = "tnsLabel";
			// 
			// serviceBox
			// 
			resources.ApplyResources(this.serviceBox, "serviceBox");
			this.helpProvider.SetHelpString(this.serviceBox, resources.GetString("serviceBox.HelpString"));
			this.serviceBox.Name = "serviceBox";
			this.helpProvider.SetShowHelp(this.serviceBox, ((bool)(resources.GetObject("serviceBox.ShowHelp"))));
			// 
			// portBox
			// 
			resources.ApplyResources(this.portBox, "portBox");
			this.helpProvider.SetHelpString(this.portBox, resources.GetString("portBox.HelpString"));
			this.portBox.Name = "portBox";
			this.helpProvider.SetShowHelp(this.portBox, ((bool)(resources.GetObject("portBox.ShowHelp"))));
			// 
			// hostBox
			// 
			resources.ApplyResources(this.hostBox, "hostBox");
			this.helpProvider.SetHelpString(this.hostBox, resources.GetString("hostBox.HelpString"));
			this.hostBox.Name = "hostBox";
			this.helpProvider.SetShowHelp(this.hostBox, ((bool)(resources.GetObject("hostBox.ShowHelp"))));
			// 
			// remoteButton
			// 
			resources.ApplyResources(this.remoteButton, "remoteButton");
			this.helpProvider.SetHelpString(this.remoteButton, resources.GetString("remoteButton.HelpString"));
			this.remoteButton.Name = "remoteButton";
			this.helpProvider.SetShowHelp(this.remoteButton, ((bool)(resources.GetObject("remoteButton.ShowHelp"))));
			this.remoteButton.CheckedChanged += new System.EventHandler(this.SelectMethod);
			// 
			// tnsBox
			// 
			resources.ApplyResources(this.tnsBox, "tnsBox");
			this.tnsBox.FormattingEnabled = true;
			this.helpProvider.SetHelpString(this.tnsBox, resources.GetString("tnsBox.HelpString"));
			this.tnsBox.Name = "tnsBox";
			this.helpProvider.SetShowHelp(this.tnsBox, ((bool)(resources.GetObject("tnsBox.ShowHelp"))));
			// 
			// tnsButton
			// 
			resources.ApplyResources(this.tnsButton, "tnsButton");
			this.helpProvider.SetHelpString(this.tnsButton, resources.GetString("tnsButton.HelpString"));
			this.tnsButton.Name = "tnsButton";
			this.helpProvider.SetShowHelp(this.tnsButton, ((bool)(resources.GetObject("tnsButton.ShowHelp"))));
			this.tnsButton.CheckedChanged += new System.EventHandler(this.SelectMethod);
			// 
			// localButton
			// 
			resources.ApplyResources(this.localButton, "localButton");
			this.localButton.Checked = true;
			this.helpProvider.SetHelpString(this.localButton, resources.GetString("localButton.HelpString"));
			this.localButton.Name = "localButton";
			this.helpProvider.SetShowHelp(this.localButton, ((bool)(resources.GetObject("localButton.ShowHelp"))));
			this.localButton.TabStop = true;
			this.localButton.CheckedChanged += new System.EventHandler(this.SelectMethod);
			// 
			// ConnectDialog
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.methodGroupBox);
			this.Controls.Add(this.modeLabel);
			this.Controls.Add(this.passwordLabel);
			this.Controls.Add(this.modeBox);
			this.Controls.Add(this.passwordBox);
			this.Controls.Add(this.userIDBox);
			this.Controls.Add(this.userIDLabel);
			this.Controls.Add(this.servicesButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.headerPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectDialog";
			this.ShowIcon = false;
			this.headerPanel.ResumeLayout(false);
			this.headerPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.orcaBox)).EndInit();
			this.methodGroupBox.ResumeLayout(false);
			this.methodGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel headerPanel;
		private System.Windows.Forms.PictureBox orcaBox;
		private System.Windows.Forms.PictureBox logoBox;
        private System.Windows.Forms.Label queryAnalyzerLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button servicesButton;
		private System.Windows.Forms.Label userIDLabel;
		private System.Windows.Forms.TextBox userIDBox;
		private System.Windows.Forms.TextBox passwordBox;
		private System.Windows.Forms.ComboBox modeBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.Label modeLabel;
		private System.Windows.Forms.GroupBox methodGroupBox;
		private System.Windows.Forms.RadioButton localButton;
		private System.Windows.Forms.RadioButton tnsButton;
		private System.Windows.Forms.ComboBox tnsBox;
		private System.Windows.Forms.RadioButton remoteButton;
		private System.Windows.Forms.TextBox hostBox;
		private System.Windows.Forms.TextBox serviceBox;
		private System.Windows.Forms.TextBox portBox;
		private System.Windows.Forms.Label tnsLabel;
		private System.Windows.Forms.Label hostLabel;
		private System.Windows.Forms.Label serviceLabel;
		private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.LinkLabel versionLabel;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.LinkLabel tnsLinkLabel;
	}
}