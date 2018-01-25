namespace River.Orqa.Query
{
	partial class ResultsView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsView));
			River.Orqa.Editor.Dialogs.SearchDialog searchDialog3 = new River.Orqa.Editor.Dialogs.SearchDialog();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			River.Orqa.Editor.Dialogs.SearchDialog searchDialog4 = new River.Orqa.Editor.Dialogs.SearchDialog();
			this.tabset = new River.Orqa.Controls.Controls.TabControl();
			this.resultTargetIcons = new System.Windows.Forms.ImageList(this.components);
			this.textPage = new River.Orqa.Controls.Controls.TabPage();
			this.textpad = new River.Orqa.Query.Notepad();
			this.gridPage = new River.Orqa.Controls.Controls.TabPage();
			this.grid = new System.Windows.Forms.DataGridView();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.toggleResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.messagePage = new River.Orqa.Controls.Controls.TabPage();
			this.msgpad = new River.Orqa.Query.Notepad();
			this.planPage = new River.Orqa.Controls.Controls.TabPage();
			this.planView = new River.Orqa.Query.PlanView();
			this.statsPage = new River.Orqa.Controls.Controls.TabPage();
			this.statisticsView = new River.Orqa.Query.StatisticsView();
			this.textPage.SuspendLayout();
			this.gridPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.contextMenu.SuspendLayout();
			this.messagePage.SuspendLayout();
			this.planPage.SuspendLayout();
			this.statsPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabset
			// 
			resources.ApplyResources(this.tabset, "tabset");
			this.tabset.HideTabsMode = River.Orqa.Controls.Controls.TabControl.HideTabsModes.ShowAlways;
			this.tabset.ImageList = this.resultTargetIcons;
			this.tabset.Name = "tabset";
			this.tabset.SelectedIndex = 0;
			this.tabset.SelectedTab = this.textPage;
			this.tabset.ShowClose = true;
			this.tabset.TabPages.AddRange(new River.Orqa.Controls.Controls.TabPage[] {
            this.textPage,
            this.gridPage,
            this.messagePage,
            this.planPage,
            this.statsPage});
			this.tabset.ClosePressed += new System.EventHandler(this.DoToggleResults);
			this.tabset.TabIndexChanged += new System.EventHandler(this.DoTabIndexChanged);
			// 
			// resultTargetIcons
			// 
			this.resultTargetIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("resultTargetIcons.ImageStream")));
			this.resultTargetIcons.TransparentColor = System.Drawing.Color.Transparent;
			this.resultTargetIcons.Images.SetKeyName(0, "ResultsText.gif");
			this.resultTargetIcons.Images.SetKeyName(1, "ResultsGrid.gif");
			this.resultTargetIcons.Images.SetKeyName(2, "ResultsXml.gif");
			// 
			// textPage
			// 
			this.textPage.Controls.Add(this.textpad);
			this.textPage.Icon = ((System.Drawing.Icon)(resources.GetObject("textPage.Icon")));
			resources.ApplyResources(this.textPage, "textPage");
			this.textPage.Name = "textPage";
			// 
			// textpad
			// 
			resources.ApplyResources(this.textpad, "textpad");
			this.textpad.FirstSearch = true;
			this.textpad.IsSaved = false;
			this.textpad.Name = "textpad";
			this.textpad.ScrollPosition = new System.Drawing.Point(0, 0);
			searchDialog3.Visible = false;
			this.textpad.SearchDialog = searchDialog3;
			this.textpad.OpeningOptions += new River.Orqa.Options.OpeningOptionsEventHandler(this.DoOpeningOptions);
			// 
			// gridPage
			// 
			this.gridPage.Controls.Add(this.grid);
			this.gridPage.Icon = ((System.Drawing.Icon)(resources.GetObject("gridPage.Icon")));
			resources.ApplyResources(this.gridPage, "gridPage");
			this.gridPage.Name = "gridPage";
			this.gridPage.Selected = false;
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.grid.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.ContextMenuStrip = this.contextMenu;
			resources.ApplyResources(this.grid, "grid");
			this.grid.MultiSelect = false;
			this.grid.Name = "grid";
			this.grid.ReadOnly = true;
			this.grid.RowHeadersVisible = false;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
			this.grid.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toggleResultsToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.contextMenu.Name = "contextMenu";
			resources.ApplyResources(this.contextMenu, "contextMenu");
			// 
			// saveAsToolStripMenuItem
			// 
			resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.DoSaving);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			// 
			// toggleResultsToolStripMenuItem
			// 
			resources.ApplyResources(this.toggleResultsToolStripMenuItem, "toggleResultsToolStripMenuItem");
			this.toggleResultsToolStripMenuItem.Name = "toggleResultsToolStripMenuItem";
			this.toggleResultsToolStripMenuItem.Click += new System.EventHandler(this.DoToggleResults);
			// 
			// optionsToolStripMenuItem
			// 
			resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.DoOptions);
			// 
			// messagePage
			// 
			this.messagePage.Controls.Add(this.msgpad);
			this.messagePage.Icon = ((System.Drawing.Icon)(resources.GetObject("messagePage.Icon")));
			resources.ApplyResources(this.messagePage, "messagePage");
			this.messagePage.Name = "messagePage";
			this.messagePage.Selected = false;
			// 
			// msgpad
			// 
			resources.ApplyResources(this.msgpad, "msgpad");
			this.msgpad.FirstSearch = true;
			this.msgpad.IsSaved = false;
			this.msgpad.Name = "msgpad";
			this.msgpad.ScrollPosition = new System.Drawing.Point(0, 0);
			searchDialog4.Visible = false;
			this.msgpad.SearchDialog = searchDialog4;
			this.msgpad.OpeningOptions += new River.Orqa.Options.OpeningOptionsEventHandler(this.DoOpeningOptions);
			// 
			// planPage
			// 
			this.planPage.Controls.Add(this.planView);
			this.planPage.Icon = ((System.Drawing.Icon)(resources.GetObject("planPage.Icon")));
			resources.ApplyResources(this.planPage, "planPage");
			this.planPage.Name = "planPage";
			this.planPage.Selected = false;
			// 
			// planView
			// 
			resources.ApplyResources(this.planView, "planView");
			this.planView.Name = "planView";
			// 
			// statsPage
			// 
			this.statsPage.Controls.Add(this.statisticsView);
			this.statsPage.Icon = ((System.Drawing.Icon)(resources.GetObject("statsPage.Icon")));
			resources.ApplyResources(this.statsPage, "statsPage");
			this.statsPage.Name = "statsPage";
			this.statsPage.Selected = false;
			// 
			// statisticsView
			// 
			resources.ApplyResources(this.statisticsView, "statisticsView");
			this.statisticsView.Name = "statisticsView";
			// 
			// ResultsView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabset);
			this.Name = "ResultsView";
			this.textPage.ResumeLayout(false);
			this.gridPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.contextMenu.ResumeLayout(false);
			this.messagePage.ResumeLayout(false);
			this.planPage.ResumeLayout(false);
			this.statsPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private River.Orqa.Controls.Controls.TabControl tabset;
		private River.Orqa.Controls.Controls.TabPage textPage;
		private River.Orqa.Controls.Controls.TabPage gridPage;
		private River.Orqa.Controls.Controls.TabPage planPage;
		private River.Orqa.Controls.Controls.TabPage messagePage;
		private River.Orqa.Controls.Controls.TabPage statsPage;
		private Notepad textpad;
		private PlanView planView;
		private StatisticsView statisticsView;
		private System.Windows.Forms.ImageList resultTargetIcons;
		private Notepad msgpad;
		private System.Windows.Forms.DataGridView grid;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toggleResultsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
	}
}
