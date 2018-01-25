namespace River.Orqa.Browser
{
	partial class SchemaManager
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaManager));
			this.schemaBox = new System.Windows.Forms.ComboBox();
			this.treeIcons = new System.Windows.Forms.ImageList(this.components);
			this.tree = new River.Orqa.Controls.RiverTreeView();
			this.treeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propsTtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuIcons = new System.Windows.Forms.ImageList(this.components);
			this.treeMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// schemaBox
			// 
			this.schemaBox.BackColor = System.Drawing.SystemColors.Info;
			resources.ApplyResources(this.schemaBox, "schemaBox");
			this.schemaBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.schemaBox.FormattingEnabled = true;
			this.schemaBox.Name = "schemaBox";
			this.schemaBox.Sorted = true;
			this.schemaBox.SelectedIndexChanged += new System.EventHandler(this.DoSchemaSelected);
			// 
			// treeIcons
			// 
			this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
			this.treeIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.treeIcons.Images.SetKeyName(0, "");
			this.treeIcons.Images.SetKeyName(1, "");
			this.treeIcons.Images.SetKeyName(2, "");
			this.treeIcons.Images.SetKeyName(3, "");
			this.treeIcons.Images.SetKeyName(4, "");
			this.treeIcons.Images.SetKeyName(5, "");
			this.treeIcons.Images.SetKeyName(6, "");
			this.treeIcons.Images.SetKeyName(7, "");
			this.treeIcons.Images.SetKeyName(8, "");
			this.treeIcons.Images.SetKeyName(9, "");
			this.treeIcons.Images.SetKeyName(10, "");
			this.treeIcons.Images.SetKeyName(11, "");
			this.treeIcons.Images.SetKeyName(12, "");
			this.treeIcons.Images.SetKeyName(13, "");
			this.treeIcons.Images.SetKeyName(14, "");
			this.treeIcons.Images.SetKeyName(15, "");
			this.treeIcons.Images.SetKeyName(16, "");
			this.treeIcons.Images.SetKeyName(17, "Sequence.ico");
			this.treeIcons.Images.SetKeyName(18, "Synonym.gif");
			this.treeIcons.Images.SetKeyName(19, "");
			this.treeIcons.Images.SetKeyName(20, "");
			this.treeIcons.Images.SetKeyName(21, "");
			this.treeIcons.Images.SetKeyName(22, "DatabaseLocked.ico");
			this.treeIcons.Images.SetKeyName(23, "Type.ico");
			// 
			// tree
			// 
			this.tree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
			this.tree.ContextMenuStrip = this.treeMenu;
			resources.ApplyResources(this.tree, "tree");
			this.tree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(188)))));
			this.tree.HideSelection = false;
			this.tree.ImageList = this.treeIcons;
			this.tree.MultiSelect = false;
			this.tree.Name = "tree";
			this.tree.ShowNodeToolTips = true;
			this.tree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.DoAfterLabelEdit);
			this.tree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.DoBeforeCollapse);
			this.tree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.DoAfterCollapse);
			this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.DoBeforeExpand);
			this.tree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.DoAfterExpand);
			this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DoAfterSelect);
			this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.DoNodeMouseClick);
			this.tree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DoKeyUp);
			this.tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DoMouseDown);
			// 
			// treeMenu
			// 
			this.treeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.editToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.propsTtoolStripMenuItem,
            this.toolStripSeparator1,
            this.refreshToolStripMenuItem});
			this.treeMenu.Name = "treeMenu";
			resources.ApplyResources(this.treeMenu, "treeMenu");
			// 
			// openToolStripMenuItem
			// 
			resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.DoOpen);
			// 
			// editToolStripMenuItem
			// 
			resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Click += new System.EventHandler(this.DoEdit);
			// 
			// compileToolStripMenuItem
			// 
			resources.ApplyResources(this.compileToolStripMenuItem, "compileToolStripMenuItem");
			this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
			this.compileToolStripMenuItem.Click += new System.EventHandler(this.DoCompile);
			// 
			// deleteToolStripMenuItem
			// 
			resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DoDelete);
			// 
			// renameToolStripMenuItem
			// 
			resources.ApplyResources(this.renameToolStripMenuItem, "renameToolStripMenuItem");
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.DoRename);
			// 
			// propsTtoolStripMenuItem
			// 
			resources.ApplyResources(this.propsTtoolStripMenuItem, "propsTtoolStripMenuItem");
			this.propsTtoolStripMenuItem.Name = "propsTtoolStripMenuItem";
			this.propsTtoolStripMenuItem.Click += new System.EventHandler(this.DoProperties);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// refreshToolStripMenuItem
			// 
			resources.ApplyResources(this.refreshToolStripMenuItem, "refreshToolStripMenuItem");
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.DoRefresh);
			// 
			// menuIcons
			// 
			this.menuIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("menuIcons.ImageStream")));
			this.menuIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.menuIcons.Images.SetKeyName(0, "Edit.gif");
			this.menuIcons.Images.SetKeyName(1, "Script.gif");
			// 
			// SchemaManager
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tree);
			this.Controls.Add(this.schemaBox);
			this.Name = "SchemaManager";
			this.treeMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox schemaBox;
		private System.Windows.Forms.ImageList treeIcons;
		private River.Orqa.Controls.RiverTreeView tree;
		private System.Windows.Forms.ContextMenuStrip treeMenu;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ImageList menuIcons;
		private System.Windows.Forms.ToolStripMenuItem propsTtoolStripMenuItem;
	}
}
