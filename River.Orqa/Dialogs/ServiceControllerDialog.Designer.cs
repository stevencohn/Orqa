namespace River.Orqa.Dialogs
{
	partial class ServiceControllerDialog
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceControllerDialog));
			this.titleLabel = new System.Windows.Forms.Label();
			this.machineBox = new System.Windows.Forms.ComboBox();
			this.refreshButton = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.stopButton = new System.Windows.Forms.Button();
			this.restartButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.serviceView = new River.Orqa.Controls.RiverListView();
			this.serviceCol = new System.Windows.Forms.ColumnHeader();
			this.statusCol = new System.Windows.Forms.ColumnHeader();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startuptypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.automaticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.icons = new System.Windows.Forms.ImageList(this.components);
			this.statusBox = new System.Windows.Forms.TextBox();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.startupCol = new System.Windows.Forms.ColumnHeader();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// titleLabel
			// 
			resources.ApplyResources(this.titleLabel, "titleLabel");
			this.titleLabel.Name = "titleLabel";
			// 
			// machineBox
			// 
			this.machineBox.FormattingEnabled = true;
			this.helpProvider.SetHelpString(this.machineBox, resources.GetString("machineBox.HelpString"));
			resources.ApplyResources(this.machineBox, "machineBox");
			this.machineBox.Name = "machineBox";
			this.helpProvider.SetShowHelp(this.machineBox, ((bool)(resources.GetObject("machineBox.ShowHelp"))));
			this.machineBox.SelectedIndexChanged += new System.EventHandler(this.DoSetMachine);
			this.machineBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DoMachineKeyUp);
			// 
			// refreshButton
			// 
			this.helpProvider.SetHelpString(this.refreshButton, resources.GetString("refreshButton.HelpString"));
			resources.ApplyResources(this.refreshButton, "refreshButton");
			this.refreshButton.Name = "refreshButton";
			this.helpProvider.SetShowHelp(this.refreshButton, ((bool)(resources.GetObject("refreshButton.ShowHelp"))));
			this.refreshButton.Click += new System.EventHandler(this.RefreshServiceList);
			// 
			// startButton
			// 
			resources.ApplyResources(this.startButton, "startButton");
			this.helpProvider.SetHelpString(this.startButton, resources.GetString("startButton.HelpString"));
			this.startButton.Name = "startButton";
			this.helpProvider.SetShowHelp(this.startButton, ((bool)(resources.GetObject("startButton.ShowHelp"))));
			this.startButton.Click += new System.EventHandler(this.StartService);
			// 
			// stopButton
			// 
			resources.ApplyResources(this.stopButton, "stopButton");
			this.helpProvider.SetHelpString(this.stopButton, resources.GetString("stopButton.HelpString"));
			this.stopButton.Name = "stopButton";
			this.helpProvider.SetShowHelp(this.stopButton, ((bool)(resources.GetObject("stopButton.ShowHelp"))));
			this.stopButton.Click += new System.EventHandler(this.StopService);
			// 
			// restartButton
			// 
			resources.ApplyResources(this.restartButton, "restartButton");
			this.helpProvider.SetHelpString(this.restartButton, resources.GetString("restartButton.HelpString"));
			this.restartButton.Name = "restartButton";
			this.helpProvider.SetShowHelp(this.restartButton, ((bool)(resources.GetObject("restartButton.ShowHelp"))));
			this.restartButton.Click += new System.EventHandler(this.RestartService);
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.helpProvider.SetHelpString(this.cancelButton, resources.GetString("cancelButton.HelpString"));
			this.cancelButton.Name = "cancelButton";
			this.helpProvider.SetShowHelp(this.cancelButton, ((bool)(resources.GetObject("cancelButton.ShowHelp"))));
			// 
			// serviceView
			// 
			resources.ApplyResources(this.serviceView, "serviceView");
			this.serviceView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.serviceCol,
            this.statusCol,
            this.startupCol});
			this.serviceView.ContextMenuStrip = this.contextMenuStrip;
			this.serviceView.FullRowSelect = true;
			this.serviceView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.helpProvider.SetHelpString(this.serviceView, resources.GetString("serviceView.HelpString"));
			this.serviceView.HideSelection = false;
			this.serviceView.MultiSelect = false;
			this.serviceView.Name = "serviceView";
			this.helpProvider.SetShowHelp(this.serviceView, ((bool)(resources.GetObject("serviceView.ShowHelp"))));
			this.serviceView.SmallImageList = this.icons;
			this.serviceView.UseCompatibleStateImageBehavior = false;
			this.serviceView.View = System.Windows.Forms.View.Details;
			this.serviceView.SelectedIndexChanged += new System.EventHandler(this.SelectService);
			this.serviceView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DoMouseUp);
			// 
			// serviceCol
			// 
			resources.ApplyResources(this.serviceCol, "serviceCol");
			// 
			// statusCol
			// 
			resources.ApplyResources(this.statusCol, "statusCol");
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.startuptypeToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
			this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.DoContextMenuOpening);
			// 
			// startToolStripMenuItem
			// 
			resources.ApplyResources(this.startToolStripMenuItem, "startToolStripMenuItem");
			this.startToolStripMenuItem.Name = "startToolStripMenuItem";
			this.startToolStripMenuItem.Click += new System.EventHandler(this.StartService);
			// 
			// stopToolStripMenuItem
			// 
			resources.ApplyResources(this.stopToolStripMenuItem, "stopToolStripMenuItem");
			this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
			this.stopToolStripMenuItem.Click += new System.EventHandler(this.StopService);
			// 
			// restartToolStripMenuItem
			// 
			resources.ApplyResources(this.restartToolStripMenuItem, "restartToolStripMenuItem");
			this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
			this.restartToolStripMenuItem.Click += new System.EventHandler(this.RestartService);
			// 
			// startuptypeToolStripMenuItem
			// 
			this.startuptypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.automaticToolStripMenuItem,
            this.manualToolStripMenuItem,
            this.disabledToolStripMenuItem});
			this.startuptypeToolStripMenuItem.Name = "startuptypeToolStripMenuItem";
			resources.ApplyResources(this.startuptypeToolStripMenuItem, "startuptypeToolStripMenuItem");
			// 
			// automaticToolStripMenuItem
			// 
			this.automaticToolStripMenuItem.Name = "automaticToolStripMenuItem";
			resources.ApplyResources(this.automaticToolStripMenuItem, "automaticToolStripMenuItem");
			this.automaticToolStripMenuItem.Click += new System.EventHandler(this.DoSetAutomaticMode);
			// 
			// manualToolStripMenuItem
			// 
			this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
			resources.ApplyResources(this.manualToolStripMenuItem, "manualToolStripMenuItem");
			this.manualToolStripMenuItem.Click += new System.EventHandler(this.DoSetManualMode);
			// 
			// disabledToolStripMenuItem
			// 
			this.disabledToolStripMenuItem.Name = "disabledToolStripMenuItem";
			resources.ApplyResources(this.disabledToolStripMenuItem, "disabledToolStripMenuItem");
			this.disabledToolStripMenuItem.Click += new System.EventHandler(this.DoSetDisabledMode);
			// 
			// icons
			// 
			this.icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("icons.ImageStream")));
			this.icons.TransparentColor = System.Drawing.Color.Transparent;
			this.icons.Images.SetKeyName(0, "SmallRunning.gif");
			this.icons.Images.SetKeyName(1, "SmallStopped.gif");
			this.icons.Images.SetKeyName(2, "SmallDisabled.png");
			// 
			// statusBox
			// 
			resources.ApplyResources(this.statusBox, "statusBox");
			this.statusBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.statusBox.Name = "statusBox";
			this.statusBox.ReadOnly = true;
			this.statusBox.TabStop = false;
			// 
			// startupCol
			// 
			resources.ApplyResources(this.startupCol, "startupCol");
			// 
			// ServiceControllerDialog
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.statusBox);
			this.Controls.Add(this.serviceView);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.restartButton);
			this.Controls.Add(this.stopButton);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.refreshButton);
			this.Controls.Add(this.machineBox);
			this.Controls.Add(this.titleLabel);
			this.DoubleBuffered = true;
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServiceControllerDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DoFormClosing);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.ComboBox machineBox;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.Button restartButton;
		private System.Windows.Forms.Button cancelButton;
		private River.Orqa.Controls.RiverListView serviceView;
		private System.Windows.Forms.ColumnHeader serviceCol;
		private System.Windows.Forms.ColumnHeader statusCol;
		private System.Windows.Forms.TextBox statusBox;
		private System.Windows.Forms.ImageList icons;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startuptypeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem automaticToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem disabledToolStripMenuItem;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.ColumnHeader startupCol;
	}
}