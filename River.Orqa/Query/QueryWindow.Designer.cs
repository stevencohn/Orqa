namespace River.Orqa.Query
{
	partial class QueryWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryWindow));
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.messageStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.userStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.formatStatus = new System.Windows.Forms.ToolStripDropDownButton();
			this.textMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gridMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xmlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timerStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.rowsStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.caretStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.marginPanel = new System.Windows.Forms.Panel();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.editorView = new River.Orqa.Query.EditorView();
			this.resultsView = new River.Orqa.Query.ResultsView();
			this.statusbar.SuspendLayout();
			this.marginPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusbar
			// 
			this.statusbar.AllowMerge = false;
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageStatus,
            this.userStatus,
            this.formatStatus,
            this.timerStatus,
            this.rowsStatus,
            this.caretStatus});
			this.statusbar.Location = new System.Drawing.Point(0, 344);
			this.statusbar.Name = "statusbar";
			this.statusbar.ShowItemToolTips = true;
			this.statusbar.Size = new System.Drawing.Size(492, 27);
			this.statusbar.TabIndex = 0;
			// 
			// messageStatus
			// 
			this.messageStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.messageStatus.Name = "messageStatus";
			this.messageStatus.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.messageStatus.Size = new System.Drawing.Size(166, 22);
			this.messageStatus.Spring = true;
			this.messageStatus.Text = "Ready";
			this.messageStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// userStatus
			// 
			this.userStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
			this.userStatus.Name = "userStatus";
			this.userStatus.Padding = new System.Windows.Forms.Padding(3, 0, 6, 0);
			this.userStatus.Size = new System.Drawing.Size(50, 22);
			this.userStatus.Text = "(user)";
			this.userStatus.ToolTipText = "Current user";
			// 
			// formatStatus
			// 
			this.formatStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.formatStatus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textMenuItem,
            this.gridMenuItem,
            this.xmlMenuItem});
			this.formatStatus.Image = ((System.Drawing.Image)(resources.GetObject("formatStatus.Image")));
			this.formatStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.formatStatus.Name = "formatStatus";
			this.formatStatus.Size = new System.Drawing.Size(29, 25);
			this.formatStatus.ToolTipText = "Results in text";
			// 
			// textMenuItem
			// 
			this.textMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("textMenuItem.Image")));
			this.textMenuItem.Name = "textMenuItem";
			this.textMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.textMenuItem.Size = new System.Drawing.Size(191, 22);
			this.textMenuItem.Text = "Results in Text";
			this.textMenuItem.Click += new System.EventHandler(this.DoChangeResultsTarget);
			// 
			// gridMenuItem
			// 
			this.gridMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("gridMenuItem.Image")));
			this.gridMenuItem.Name = "gridMenuItem";
			this.gridMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.gridMenuItem.Size = new System.Drawing.Size(191, 22);
			this.gridMenuItem.Text = "Results in Grid";
			this.gridMenuItem.Click += new System.EventHandler(this.DoChangeResultsTarget);
			// 
			// xmlMenuItem
			// 
			this.xmlMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("xmlMenuItem.Image")));
			this.xmlMenuItem.Name = "xmlMenuItem";
			this.xmlMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.xmlMenuItem.Size = new System.Drawing.Size(191, 22);
			this.xmlMenuItem.Text = "Results in XML";
			this.xmlMenuItem.Click += new System.EventHandler(this.DoChangeResultsTarget);
			// 
			// timerStatus
			// 
			this.timerStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.timerStatus.Name = "timerStatus";
			this.timerStatus.Padding = new System.Windows.Forms.Padding(3, 0, 6, 0);
			this.timerStatus.Size = new System.Drawing.Size(62, 22);
			this.timerStatus.Text = "00:00.00";
			this.timerStatus.ToolTipText = "Query execution time";
			// 
			// rowsStatus
			// 
			this.rowsStatus.AutoSize = false;
			this.rowsStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.rowsStatus.Name = "rowsStatus";
			this.rowsStatus.Padding = new System.Windows.Forms.Padding(3, 0, 6, 0);
			this.rowsStatus.Size = new System.Drawing.Size(80, 22);
			this.rowsStatus.Text = "0 rows";
			this.rowsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rowsStatus.ToolTipText = "Affected rows";
			// 
			// caretStatus
			// 
			this.caretStatus.AutoSize = false;
			this.caretStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.caretStatus.Name = "caretStatus";
			this.caretStatus.Padding = new System.Windows.Forms.Padding(3, 0, 6, 0);
			this.caretStatus.Size = new System.Drawing.Size(90, 22);
			this.caretStatus.Text = "Ln 1, Col 1";
			this.caretStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.caretStatus.ToolTipText = "Editing location";
			// 
			// marginPanel
			// 
			this.marginPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.marginPanel.Controls.Add(this.splitContainer);
			this.marginPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.marginPanel.Location = new System.Drawing.Point(0, 0);
			this.marginPanel.Name = "marginPanel";
			this.marginPanel.Padding = new System.Windows.Forms.Padding(3);
			this.marginPanel.Size = new System.Drawing.Size(492, 344);
			this.marginPanel.TabIndex = 2;
			// 
			// splitContainer
			// 
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(3, 3);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.editorView);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.resultsView);
			this.splitContainer.Size = new System.Drawing.Size(484, 336);
			this.splitContainer.SplitterDistance = 168;
			this.splitContainer.TabIndex = 0;
			this.splitContainer.Text = "splitContainer1";
			// 
			// editorView
			// 
			this.editorView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorView.IsSaved = true;
			this.editorView.Location = new System.Drawing.Point(0, 0);
			this.editorView.Name = "editorView";
			this.editorView.Size = new System.Drawing.Size(480, 164);
			this.editorView.TabIndex = 0;
			this.editorView.CaretChanged += new River.Orqa.Query.CaretChangedEventHandler(this.DoCaretChanged);
			this.editorView.Edited += new River.Orqa.Query.EditorEventHandler(this.DoEdited);
			this.editorView.ExecuteQuery += new System.EventHandler(this.DoExecuteQuery);
			this.editorView.OpeningOptions += new River.Orqa.Options.OpeningOptionsEventHandler(this.DoOpeningOptions);
			this.editorView.ParseQuery += new System.EventHandler(this.DoParseQuery);
			this.editorView.SelectionChanged += new System.EventHandler(this.DoSelectionChanged);
			this.editorView.ToggleResults += new System.EventHandler(this.DoToggleResults);
			// 
			// resultsView
			// 
			this.resultsView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultsView.IsSaved = false;
			this.resultsView.Location = new System.Drawing.Point(0, 0);
			this.resultsView.Name = "resultsView";
			this.resultsView.Size = new System.Drawing.Size(480, 160);
			this.resultsView.TabIndex = 0;
			this.resultsView.OpeningOptions += new River.Orqa.Options.OpeningOptionsEventHandler(this.DoOpeningOptions);
			this.resultsView.Saving += new System.EventHandler(this.DoSaving);
			this.resultsView.ToggleResults += new System.EventHandler(this.DoToggleResults);
			// 
			// QueryWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(492, 371);
			this.Controls.Add(this.marginPanel);
			this.Controls.Add(this.statusbar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "QueryWindow";
			this.ShowInTaskbar = false;
			this.Text = "QueryWindow";
			this.Activated += new System.EventHandler(this.DoWindowActivated);
			this.Load += new System.EventHandler(this.DoWindowLoad);
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.marginPanel.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.Panel marginPanel;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.ToolStripStatusLabel messageStatus;
		private System.Windows.Forms.ToolStripStatusLabel userStatus;
		private System.Windows.Forms.ToolStripStatusLabel timerStatus;
		private System.Windows.Forms.ToolStripStatusLabel rowsStatus;
		private System.Windows.Forms.ToolStripStatusLabel caretStatus;
		private System.Windows.Forms.ToolStripDropDownButton formatStatus;
		private System.Windows.Forms.ToolStripMenuItem textMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gridMenuItem;
		private System.Windows.Forms.ToolStripMenuItem xmlMenuItem;
		private River.Orqa.Query.ResultsView resultsView;
		private River.Orqa.Query.EditorView editorView;
	}
}