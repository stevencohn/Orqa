namespace River.Orqa.Query
{
	partial class EditorView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorView));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.parseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resultModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resultsinTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resultsinGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resultsinXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toggleResultsPaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Enabled = true;
			this.contextMenu.GripMargin = new System.Windows.Forms.Padding(2);
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.toolStripSeparator1,
            this.parseToolStripMenuItem,
            this.executeToolStripMenuItem,
            this.resultModeToolStripMenuItem,
            this.toolStripSeparator2,
            this.toggleResultsPaneToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.contextMenu.Location = new System.Drawing.Point(21, 36);
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.contextMenu.Size = new System.Drawing.Size(179, 255);
			this.contextMenu.Visible = true;
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.DoPrepareContextMenu);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.DoCut);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.DoCopy);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.DoPaste);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("selectAllToolStripMenuItem.Image")));
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.DoSelectAll);
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clearToolStripMenuItem.Image")));
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
			this.clearToolStripMenuItem.Text = "Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler(this.DoClear);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			// 
			// parseToolStripMenuItem
			// 
			this.parseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("parseToolStripMenuItem.Image")));
			this.parseToolStripMenuItem.Name = "parseToolStripMenuItem";
			this.parseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
			this.parseToolStripMenuItem.Text = "Parse";
			this.parseToolStripMenuItem.Click += new System.EventHandler(this.DoParse);
			// 
			// executeToolStripMenuItem
			// 
			this.executeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("executeToolStripMenuItem.Image")));
			this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
			this.executeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.executeToolStripMenuItem.Text = "Execute";
			this.executeToolStripMenuItem.Click += new System.EventHandler(this.DoExecute);
			// 
			// resultModeToolStripMenuItem
			// 
			this.resultModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resultsinTextToolStripMenuItem,
            this.resultsinGridToolStripMenuItem,
            this.resultsinXMLToolStripMenuItem});
			this.resultModeToolStripMenuItem.Name = "resultModeToolStripMenuItem";
			this.resultModeToolStripMenuItem.Text = "Result Mode";
			this.resultModeToolStripMenuItem.Click += new System.EventHandler(this.DoChangeResultTarget);
			// 
			// resultsinTextToolStripMenuItem
			// 
			this.resultsinTextToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resultsinTextToolStripMenuItem.Image")));
			this.resultsinTextToolStripMenuItem.Name = "resultsinTextToolStripMenuItem";
			this.resultsinTextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.resultsinTextToolStripMenuItem.Text = "Results in Text";
			this.resultsinTextToolStripMenuItem.Click += new System.EventHandler(this.DoChangeResultTarget);
			// 
			// resultsinGridToolStripMenuItem
			// 
			this.resultsinGridToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resultsinGridToolStripMenuItem.Image")));
			this.resultsinGridToolStripMenuItem.Name = "resultsinGridToolStripMenuItem";
			this.resultsinGridToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.resultsinGridToolStripMenuItem.Text = "Results in Grid";
			this.resultsinGridToolStripMenuItem.Click += new System.EventHandler(this.DoChangeResultTarget);
			// 
			// resultsinXMLToolStripMenuItem
			// 
			this.resultsinXMLToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resultsinXMLToolStripMenuItem.Image")));
			this.resultsinXMLToolStripMenuItem.Name = "resultsinXMLToolStripMenuItem";
			this.resultsinXMLToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.resultsinXMLToolStripMenuItem.Text = "Results in XML";
			this.resultsinXMLToolStripMenuItem.Click += new System.EventHandler(this.DoChangeResultTarget);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			// 
			// toggleResultsPaneToolStripMenuItem
			// 
			this.toggleResultsPaneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("toggleResultsPaneToolStripMenuItem.Image")));
			this.toggleResultsPaneToolStripMenuItem.Name = "toggleResultsPaneToolStripMenuItem";
			this.toggleResultsPaneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.toggleResultsPaneToolStripMenuItem.Text = "Toggle Results";
			this.toggleResultsPaneToolStripMenuItem.Click += new System.EventHandler(this.DoToggleResults);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Text = "Options...";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.DoOptions);
			// 
			// EditorView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "EditorView";
			this.Size = new System.Drawing.Size(359, 309);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem parseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resultModeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resultsinTextToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resultsinGridToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resultsinXMLToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toggleResultsPaneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
	}
}
