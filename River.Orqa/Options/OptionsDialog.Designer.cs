namespace River.Orqa.Options
{
	partial class OptionsDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
			this.panel = new System.Windows.Forms.Panel();
			this.contentPanel = new System.Windows.Forms.Panel();
			this.headerLabel = new River.Orqa.Options.FadingLabel();
			this.tree = new System.Windows.Forms.TreeView();
			this.treeIcons = new System.Windows.Forms.ImageList(this.components);
			this.buttonPanel = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.toolbar = new System.Windows.Forms.ToolStrip();
			this.importToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.exportToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.resetToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.openDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveDialog = new System.Windows.Forms.SaveFileDialog();
			this.panel.SuspendLayout();
			this.buttonPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			resources.ApplyResources(this.panel, "panel");
			this.panel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Controls.Add(this.contentPanel);
			this.panel.Controls.Add(this.headerLabel);
			this.panel.Name = "panel";
			this.panel.Padding = new System.Windows.Forms.Padding(1);
			// 
			// contentPanel
			// 
			resources.ApplyResources(this.contentPanel, "contentPanel");
			this.contentPanel.Name = "contentPanel";
			this.contentPanel.Padding = new System.Windows.Forms.Padding(10);
			// 
			// headerLabel
			// 
			resources.ApplyResources(this.headerLabel, "headerLabel");
			this.headerLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.headerLabel.Name = "headerLabel";
			this.headerLabel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			// 
			// tree
			// 
			resources.ApplyResources(this.tree, "tree");
			this.tree.HideSelection = false;
			this.tree.ImageList = this.treeIcons;
			this.tree.Margin = new System.Windows.Forms.Padding(3, 0, 10, 3);
			this.tree.Name = "tree";
			this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ActivateSheet);
			// 
			// treeIcons
			// 
			this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
			this.treeIcons.Images.SetKeyName(0, "Options.gif");
			this.treeIcons.Images.SetKeyName(1, "Editor.gif");
			this.treeIcons.Images.SetKeyName(2, "Connections.gif");
			this.treeIcons.Images.SetKeyName(3, "Results.gif");
			this.treeIcons.Images.SetKeyName(4, "Statistics.gif");
			this.treeIcons.Images.SetKeyName(5, "EditorFonts.gif");
			this.treeIcons.Images.SetKeyName(6, "EditorTabs.gif");
			// 
			// buttonPanel
			// 
			this.buttonPanel.Controls.Add(this.panel1);
			this.buttonPanel.Controls.Add(this.okButton);
			this.buttonPanel.Controls.Add(this.cancelButton);
			resources.ApplyResources(this.buttonPanel, "buttonPanel");
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.toolbar);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// toolbar
			// 
			resources.ApplyResources(this.toolbar, "toolbar");
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripButton,
            this.exportToolStripButton,
            this.resetToolStripButton});
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// importToolStripButton
			// 
			this.importToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.importToolStripButton, "importToolStripButton");
			this.importToolStripButton.Name = "importToolStripButton";
			this.importToolStripButton.Click += new System.EventHandler(this.DoImport);
			// 
			// exportToolStripButton
			// 
			this.exportToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.exportToolStripButton, "exportToolStripButton");
			this.exportToolStripButton.Name = "exportToolStripButton";
			this.exportToolStripButton.Click += new System.EventHandler(this.DoExport);
			// 
			// resetToolStripButton
			// 
			this.resetToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.resetToolStripButton, "resetToolStripButton");
			this.resetToolStripButton.Name = "resetToolStripButton";
			this.resetToolStripButton.Click += new System.EventHandler(this.DoReset);
			// 
			// okButton
			// 
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Name = "okButton";
			this.okButton.Click += new System.EventHandler(this.DoSave);
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			// 
			// openDialog
			// 
			this.openDialog.DefaultExt = "xml";
			resources.ApplyResources(this.openDialog, "openDialog");
			// 
			// saveDialog
			// 
			this.saveDialog.DefaultExt = "xml";
			resources.ApplyResources(this.saveDialog, "saveDialog");
			// 
			// OptionsDialog
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.panel);
			this.Controls.Add(this.tree);
			this.Controls.Add(this.buttonPanel);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.panel.ResumeLayout(false);
			this.buttonPanel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.toolbar.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Panel contentPanel;
		private System.Windows.Forms.TreeView tree;
		private System.Windows.Forms.Panel buttonPanel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStrip toolbar;
		private System.Windows.Forms.ToolStripButton importToolStripButton;
		private System.Windows.Forms.ToolStripButton exportToolStripButton;
		private System.Windows.Forms.ToolStripButton resetToolStripButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ImageList treeIcons;
		private System.Windows.Forms.OpenFileDialog openDialog;
		private System.Windows.Forms.SaveFileDialog saveDialog;
		private FadingLabel headerLabel;
	}
}