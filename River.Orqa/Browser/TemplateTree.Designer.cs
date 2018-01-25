namespace River.Orqa.Browser
{
	partial class TemplateTree
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateTree));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.rootCreateItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rootRefreshItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rootOptionsItem = new System.Windows.Forms.ToolStripMenuItem();
			this.treeIcons = new System.Windows.Forms.ImageList(this.components);
			this.templateContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.itemReportItem = new System.Windows.Forms.ToolStripMenuItem();
			this.itemEditItem = new System.Windows.Forms.ToolStripMenuItem();
			this.itemCopyItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameItemMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.folderContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addScriptTtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.folderCreateNewItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.renameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteItemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.folderRefreshItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tree = new River.Orqa.Controls.RiverTreeView();
			this.contextMenu.SuspendLayout();
			this.templateContextMenu.SuspendLayout();
			this.folderContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rootCreateItem,
            this.rootRefreshItem,
            this.rootOptionsItem});
			this.contextMenu.Name = "contextMenu";
			resources.ApplyResources(this.contextMenu, "contextMenu");
			// 
			// rootCreateItem
			// 
			resources.ApplyResources(this.rootCreateItem, "rootCreateItem");
			this.rootCreateItem.Name = "rootCreateItem";
			this.rootCreateItem.Click += new System.EventHandler(this.DoAddNewFolder);
			// 
			// rootRefreshItem
			// 
			resources.ApplyResources(this.rootRefreshItem, "rootRefreshItem");
			this.rootRefreshItem.Name = "rootRefreshItem";
			this.rootRefreshItem.Click += new System.EventHandler(this.DoRefreshAll);
			// 
			// rootOptionsItem
			// 
			resources.ApplyResources(this.rootOptionsItem, "rootOptionsItem");
			this.rootOptionsItem.Name = "rootOptionsItem";
			this.rootOptionsItem.Click += new System.EventHandler(this.DoOptions);
			// 
			// treeIcons
			// 
			this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
			this.treeIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.treeIcons.Images.SetKeyName(0, "FolderOpen.gif");
			this.treeIcons.Images.SetKeyName(1, "FolderClose.gif");
			this.treeIcons.Images.SetKeyName(2, "Template.ico");
			this.treeIcons.Images.SetKeyName(3, "Print.ico");
			this.treeIcons.Images.SetKeyName(4, "Script.ico");
			// 
			// templateContextMenu
			// 
			this.templateContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemReportItem,
            this.itemEditItem,
            this.itemCopyItem,
            this.renameItemMenuItem2,
            this.deleteToolStripMenuItem});
			this.templateContextMenu.Name = "contextMenu";
			resources.ApplyResources(this.templateContextMenu, "templateContextMenu");
			this.templateContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.DoOpeningContextMenu);
			// 
			// itemReportItem
			// 
			resources.ApplyResources(this.itemReportItem, "itemReportItem");
			this.itemReportItem.Name = "itemReportItem";
			this.itemReportItem.Click += new System.EventHandler(this.OpenTemplate);
			// 
			// itemEditItem
			// 
			resources.ApplyResources(this.itemEditItem, "itemEditItem");
			this.itemEditItem.Name = "itemEditItem";
			this.itemEditItem.Click += new System.EventHandler(this.EditTemplate);
			// 
			// itemCopyItem
			// 
			resources.ApplyResources(this.itemCopyItem, "itemCopyItem");
			this.itemCopyItem.Name = "itemCopyItem";
			this.itemCopyItem.Click += new System.EventHandler(this.CopyTemplate);
			// 
			// renameItemMenuItem2
			// 
			resources.ApplyResources(this.renameItemMenuItem2, "renameItemMenuItem2");
			this.renameItemMenuItem2.Name = "renameItemMenuItem2";
			this.renameItemMenuItem2.Click += new System.EventHandler(this.DoRenameItem);
			// 
			// deleteToolStripMenuItem
			// 
			resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DoDeleteItem);
			// 
			// folderContextMenu
			// 
			this.folderContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addScriptTtoolStripMenuItem,
            this.folderCreateNewItem,
            this.toolStripSeparator1,
            this.renameMenuItem,
            this.deleteItemMenuItem,
            this.toolStripSeparator2,
            this.folderRefreshItem});
			this.folderContextMenu.Name = "folderContextMenu";
			resources.ApplyResources(this.folderContextMenu, "folderContextMenu");
			// 
			// addScriptTtoolStripMenuItem
			// 
			resources.ApplyResources(this.addScriptTtoolStripMenuItem, "addScriptTtoolStripMenuItem");
			this.addScriptTtoolStripMenuItem.Name = "addScriptTtoolStripMenuItem";
			this.addScriptTtoolStripMenuItem.Click += new System.EventHandler(this.DoAddNewItem);
			// 
			// folderCreateNewItem
			// 
			resources.ApplyResources(this.folderCreateNewItem, "folderCreateNewItem");
			this.folderCreateNewItem.Name = "folderCreateNewItem";
			this.folderCreateNewItem.Click += new System.EventHandler(this.DoAddNewFolder);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// renameMenuItem
			// 
			resources.ApplyResources(this.renameMenuItem, "renameMenuItem");
			this.renameMenuItem.Name = "renameMenuItem";
			this.renameMenuItem.Click += new System.EventHandler(this.DoRenameItem);
			// 
			// deleteItemMenuItem
			// 
			resources.ApplyResources(this.deleteItemMenuItem, "deleteItemMenuItem");
			this.deleteItemMenuItem.Name = "deleteItemMenuItem";
			this.deleteItemMenuItem.Click += new System.EventHandler(this.DoDeleteItem);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// folderRefreshItem
			// 
			resources.ApplyResources(this.folderRefreshItem, "folderRefreshItem");
			this.folderRefreshItem.Name = "folderRefreshItem";
			this.folderRefreshItem.Click += new System.EventHandler(this.DoRefreshTemplateFolder);
			// 
			// tree
			// 
			this.tree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.tree.ContextMenuStrip = this.contextMenu;
			resources.ApplyResources(this.tree, "tree");
			this.tree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(188)))));
			this.tree.HideSelection = false;
			this.tree.ImageList = this.treeIcons;
			this.tree.LabelEdit = true;
			this.tree.MultiSelect = false;
			this.tree.Name = "tree";
			this.tree.ShowLines = false;
			this.tree.ShowNodeToolTips = true;
			this.tree.ShowRootLines = false;
			this.tree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tree_AfterLabelEdit);
			this.tree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.DoBeforeCollapse);
			this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.DoBeforeExpand);
			this.tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DoMouseDown);
			this.tree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DoMouseUp);
			// 
			// TemplateTree
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tree);
			this.Name = "TemplateTree";
			this.contextMenu.ResumeLayout(false);
			this.templateContextMenu.ResumeLayout(false);
			this.folderContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private River.Orqa.Controls.RiverTreeView tree;
		private System.Windows.Forms.ContextMenuStrip templateContextMenu;
		private System.Windows.Forms.ToolStripMenuItem itemReportItem;
		private System.Windows.Forms.ToolStripMenuItem itemCopyItem;
		private System.Windows.Forms.ImageList treeIcons;
		private System.Windows.Forms.ContextMenuStrip folderContextMenu;
		private System.Windows.Forms.ToolStripMenuItem folderRefreshItem;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem rootOptionsItem;
		private System.Windows.Forms.ToolStripMenuItem rootRefreshItem;
		private System.Windows.Forms.ToolStripMenuItem itemEditItem;
		private System.Windows.Forms.ToolStripMenuItem rootCreateItem;
		private System.Windows.Forms.ToolStripMenuItem folderCreateNewItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteItemMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameItemMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem addScriptTtoolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}
