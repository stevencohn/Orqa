namespace River.Orqa.Query
{
	partial class PlanView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanView));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.browser = new System.Windows.Forms.WebBrowser();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Enabled = true;
			this.contextMenu.GripMargin = new System.Windows.Forms.Padding(2);
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
			this.contextMenu.Location = new System.Drawing.Point(21, 36);
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.contextMenu.Size = new System.Drawing.Size(108, 26);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Text = "Export...";
			this.exportToolStripMenuItem.Click += new System.EventHandler(this.DoExport);
			// 
			// browser
			// 
			this.browser.AllowNavigation = false;
			this.browser.AllowWebBrowserDrop = false;
			this.browser.ContextMenuStrip = this.contextMenu;
			this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.browser.IsWebBrowserContextMenuEnabled = false;
			this.browser.Location = new System.Drawing.Point(0, 0);
			this.browser.Name = "browser";
			this.browser.Size = new System.Drawing.Size(276, 242);
			this.browser.Url = new System.Uri("about:blank", System.UriKind.Absolute);
			// 
			// PlanView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.browser);
			this.Name = "PlanView";
			this.Size = new System.Drawing.Size(276, 242);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.WebBrowser browser;
	}
}
