namespace River.Orqa.Browser
{
	partial class ProjectTree
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectTree));
			this.tree = new River.Orqa.Controls.RiverTreeView();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.scriptContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.renameTtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertiesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.folderContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.propertiesToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.refreshToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.introPanel = new System.Windows.Forms.Panel();
			this.introBox = new System.Windows.Forms.TextBox();
			this.contextMenu.SuspendLayout();
			this.scriptContextMenu.SuspendLayout();
			this.projectContextMenu.SuspendLayout();
			this.folderContextMenu.SuspendLayout();
			this.introPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tree
			// 
			this.tree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.tree.ContextMenuStrip = this.contextMenu;
			this.tree.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(188)))));
			this.tree.HideSelection = false;
			this.tree.ImageIndex = 0;
			this.tree.ImageList = this.images;
			this.tree.LabelEdit = true;
			this.tree.Location = new System.Drawing.Point(0, 379);
			this.tree.MultiSelect = false;
			this.tree.Name = "tree";
			this.tree.SelectedImageIndex = 0;
			this.tree.ShowLines = false;
			this.tree.Size = new System.Drawing.Size(300, 21);
			this.tree.TabIndex = 0;
			this.tree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.DoBeforeCollapse);
			this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.DoBeforeExpand);
			this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.DoNodeMouseDoubleClick);
			this.tree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DoKeyUp);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectToolStripMenuItem});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(153, 26);
			// 
			// openProjectToolStripMenuItem
			// 
			this.openProjectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openProjectToolStripMenuItem.Image")));
			this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
			this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openProjectToolStripMenuItem.Text = "Open Project...";
			this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.DoScriptOpen);
			// 
			// images
			// 
			this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			this.images.Images.SetKeyName(0, "DatabaseProject.png");
			this.images.Images.SetKeyName(1, "FolderClose.gif");
			this.images.Images.SetKeyName(2, "FolderOpen.gif");
			this.images.Images.SetKeyName(3, "SqlScript.png");
			this.images.Images.SetKeyName(4, "MissingFile.png");
			this.images.Images.SetKeyName(5, "BatchFile.png");
			this.images.Images.SetKeyName(6, "TextFile.png");
			// 
			// scriptContextMenu
			// 
			this.scriptContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.propertiesToolStripMenuItem});
			this.scriptContextMenu.Name = "scriptContextMenu";
			this.scriptContextMenu.Size = new System.Drawing.Size(147, 92);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.DoOpenScript);
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameToolStripMenuItem.Image")));
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.renameToolStripMenuItem.Text = "Rename";
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DoDeleteItem);
			// 
			// propertiesToolStripMenuItem
			// 
			this.propertiesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("propertiesToolStripMenuItem.Image")));
			this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
			this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.propertiesToolStripMenuItem.Text = "Properties";
			// 
			// projectContextMenu
			// 
			this.projectContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addScriptToolStripMenuItem,
            this.newFolderToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.renameTtoolStripMenuItem,
            this.closeToolStripMenuItem,
            this.propertiesToolStripMenuItem1,
            this.toolStripSeparator2,
            this.refreshToolStripMenuItem});
			this.projectContextMenu.Name = "projectContextMenu";
			this.projectContextMenu.Size = new System.Drawing.Size(139, 170);
			// 
			// addScriptToolStripMenuItem
			// 
			this.addScriptToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addScriptToolStripMenuItem.Image")));
			this.addScriptToolStripMenuItem.Name = "addScriptToolStripMenuItem";
			this.addScriptToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.addScriptToolStripMenuItem.Text = "Add Script...";
			this.addScriptToolStripMenuItem.Click += new System.EventHandler(this.DoAddNewItem);
			// 
			// newFolderToolStripMenuItem
			// 
			this.newFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newFolderToolStripMenuItem.Image")));
			this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
			this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.newFolderToolStripMenuItem.Text = "New Folder";
			this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.DoAddNewFolder);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.DoSaveProject);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
			// 
			// renameTtoolStripMenuItem
			// 
			this.renameTtoolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameTtoolStripMenuItem.Image")));
			this.renameTtoolStripMenuItem.Name = "renameTtoolStripMenuItem";
			this.renameTtoolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.renameTtoolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.renameTtoolStripMenuItem.Text = "Rename";
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem.Image")));
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.closeToolStripMenuItem.Text = "Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.DoCloseProject);
			// 
			// propertiesToolStripMenuItem1
			// 
			this.propertiesToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("propertiesToolStripMenuItem1.Image")));
			this.propertiesToolStripMenuItem1.Name = "propertiesToolStripMenuItem1";
			this.propertiesToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
			this.propertiesToolStripMenuItem1.Text = "Properties";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(135, 6);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("refreshToolStripMenuItem.Image")));
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.DoRefreshProject);
			// 
			// folderContextMenu
			// 
			this.folderContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator4,
            this.renameToolStripMenuItem1,
            this.deleteToolStripMenuItem1,
            this.propertiesToolStripMenuItem2,
            this.toolStripSeparator3,
            this.refreshToolStripMenuItem1});
			this.folderContextMenu.Name = "folderContextMenu";
			this.folderContextMenu.Size = new System.Drawing.Size(139, 148);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
			this.toolStripMenuItem1.Text = "Add Script...";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.DoAddNewItem);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(138, 22);
			this.toolStripMenuItem2.Text = "New Folder";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.DoAddNewFolder);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(135, 6);
			// 
			// renameToolStripMenuItem1
			// 
			this.renameToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("renameToolStripMenuItem1.Image")));
			this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
			this.renameToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.renameToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
			this.renameToolStripMenuItem1.Text = "Rename";
			// 
			// deleteToolStripMenuItem1
			// 
			this.deleteToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem1.Image")));
			this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
			this.deleteToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
			this.deleteToolStripMenuItem1.Text = "Delete";
			this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.DoDeleteItem);
			// 
			// propertiesToolStripMenuItem2
			// 
			this.propertiesToolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("propertiesToolStripMenuItem2.Image")));
			this.propertiesToolStripMenuItem2.Name = "propertiesToolStripMenuItem2";
			this.propertiesToolStripMenuItem2.Size = new System.Drawing.Size(138, 22);
			this.propertiesToolStripMenuItem2.Text = "Properties";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(135, 6);
			// 
			// refreshToolStripMenuItem1
			// 
			this.refreshToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("refreshToolStripMenuItem1.Image")));
			this.refreshToolStripMenuItem1.Name = "refreshToolStripMenuItem1";
			this.refreshToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.refreshToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
			this.refreshToolStripMenuItem1.Text = "Refresh";
			this.refreshToolStripMenuItem1.Click += new System.EventHandler(this.DoRefreshFolder);
			// 
			// introPanel
			// 
			this.introPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.introPanel.ContextMenuStrip = this.contextMenu;
			this.introPanel.Controls.Add(this.introBox);
			this.introPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.introPanel.Location = new System.Drawing.Point(0, 0);
			this.introPanel.Name = "introPanel";
			this.introPanel.Padding = new System.Windows.Forms.Padding(15);
			this.introPanel.Size = new System.Drawing.Size(300, 379);
			this.introPanel.TabIndex = 4;
			// 
			// introBox
			// 
			this.introBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.introBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.introBox.ContextMenuStrip = this.contextMenu;
			this.introBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.introBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.introBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(188)))));
			this.introBox.Location = new System.Drawing.Point(15, 15);
			this.introBox.Multiline = true;
			this.introBox.Name = "introBox";
			this.introBox.ReadOnly = true;
			this.introBox.Size = new System.Drawing.Size(270, 132);
			this.introBox.TabIndex = 1;
			this.introBox.Text = "This is a Beta release of the Orqa VS2012 Project Manager.  Use at your own risk!" +
    "\r\n\r\nRight-click to open a Visual Studio 2012 database project file (sqlproj).";
			// 
			// ProjectTree
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.introPanel);
			this.Controls.Add(this.tree);
			this.Name = "ProjectTree";
			this.Size = new System.Drawing.Size(300, 400);
			this.contextMenu.ResumeLayout(false);
			this.scriptContextMenu.ResumeLayout(false);
			this.projectContextMenu.ResumeLayout(false);
			this.folderContextMenu.ResumeLayout(false);
			this.introPanel.ResumeLayout(false);
			this.introPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private River.Orqa.Controls.RiverTreeView tree;
		private System.Windows.Forms.ImageList images;
		private System.Windows.Forms.ContextMenuStrip scriptContextMenu;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip projectContextMenu;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameTtoolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip folderContextMenu;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addScriptToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.Panel introPanel;
		private System.Windows.Forms.TextBox introBox;
	}
}
