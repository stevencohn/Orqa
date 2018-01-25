namespace River.Orqa
{
	partial class Logger
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logger));
			this.notepad = new System.Windows.Forms.RichTextBox();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// notepad
			// 
			this.notepad.BackColor = System.Drawing.Color.Linen;
			this.notepad.ContextMenuStrip = this.contextMenu;
			this.notepad.Dock = System.Windows.Forms.DockStyle.Fill;
			this.notepad.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.notepad.Location = new System.Drawing.Point(0, 0);
			this.notepad.Name = "notepad";
			this.notepad.ReadOnly = true;
			this.notepad.Size = new System.Drawing.Size(200, 150);
			this.notepad.TabIndex = 0;
			this.notepad.Text = "";
			// 
			// contextMenu
			// 
			this.contextMenu.Enabled = true;
			this.contextMenu.GripMargin = new System.Windows.Forms.Padding(2);
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.selectallToolStripMenuItem,
            this.clearallToolStripMenuItem});
			this.contextMenu.Location = new System.Drawing.Point(21, 36);
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.contextMenu.Size = new System.Drawing.Size(160, 92);
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
			// selectallToolStripMenuItem
			// 
			this.selectallToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("selectallToolStripMenuItem.Image")));
			this.selectallToolStripMenuItem.Name = "selectallToolStripMenuItem";
			this.selectallToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectallToolStripMenuItem.Text = "Select all";
			this.selectallToolStripMenuItem.Click += new System.EventHandler(this.DoSelectAll);
			// 
			// clearallToolStripMenuItem
			// 
			this.clearallToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clearallToolStripMenuItem.Image")));
			this.clearallToolStripMenuItem.Name = "clearallToolStripMenuItem";
			this.clearallToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
			this.clearallToolStripMenuItem.Text = "Clear all";
			this.clearallToolStripMenuItem.Click += new System.EventHandler(this.DoClear);
			// 
			// Logger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.notepad);
			this.Name = "Logger";
			this.Size = new System.Drawing.Size(200, 150);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox notepad;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectallToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearallToolStripMenuItem;
	}
}
