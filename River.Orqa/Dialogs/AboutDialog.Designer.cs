namespace River.Orqa.Dialogs
{
    partial class AboutDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
			this.headerPanel = new System.Windows.Forms.Panel();
			this.copyrightLabel = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			this.queryAnalyzerLabel = new System.Windows.Forms.Label();
			this.logoBox = new System.Windows.Forms.PictureBox();
			this.orcaBox = new System.Windows.Forms.PictureBox();
			this.tabset = new System.Windows.Forms.TabControl();
			this.conPage = new System.Windows.Forms.TabPage();
			this.paramView = new System.Windows.Forms.ListView();
			this.paramCol = new System.Windows.Forms.ColumnHeader();
			this.paramValueCol = new System.Windows.Forms.ColumnHeader();
			this.conView = new System.Windows.Forms.ListView();
			this.instanceCol = new System.Windows.Forms.ColumnHeader();
			this.hostCol = new System.Windows.Forms.ColumnHeader();
			this.versionCol = new System.Windows.Forms.ColumnHeader();
			this.startedCol = new System.Windows.Forms.ColumnHeader();
			this.okButton1 = new System.Windows.Forms.Button();
			this.locPage = new System.Windows.Forms.TabPage();
			this.configView = new System.Windows.Forms.ListView();
			this.keyCol = new System.Windows.Forms.ColumnHeader();
			this.valueCol = new System.Windows.Forms.ColumnHeader();
			this.okButton2 = new System.Windows.Forms.Button();
			this.historyPage = new System.Windows.Forms.TabPage();
			this.historyBox = new System.Windows.Forms.RichTextBox();
			this.okButtonHistory = new System.Windows.Forms.Button();
			this.detailPanel = new System.Windows.Forms.Panel();
			this.headerPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.orcaBox)).BeginInit();
			this.tabset.SuspendLayout();
			this.conPage.SuspendLayout();
			this.locPage.SuspendLayout();
			this.historyPage.SuspendLayout();
			this.detailPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// headerPanel
			// 
			this.headerPanel.BackColor = System.Drawing.Color.White;
			this.headerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.headerPanel.Controls.Add(this.copyrightLabel);
			this.headerPanel.Controls.Add(this.versionLabel);
			this.headerPanel.Controls.Add(this.queryAnalyzerLabel);
			this.headerPanel.Controls.Add(this.logoBox);
			this.headerPanel.Controls.Add(this.orcaBox);
			resources.ApplyResources(this.headerPanel, "headerPanel");
			this.headerPanel.Name = "headerPanel";
			// 
			// copyrightLabel
			// 
			resources.ApplyResources(this.copyrightLabel, "copyrightLabel");
			this.copyrightLabel.Name = "copyrightLabel";
			// 
			// versionLabel
			// 
			resources.ApplyResources(this.versionLabel, "versionLabel");
			this.versionLabel.Name = "versionLabel";
			// 
			// queryAnalyzerLabel
			// 
			resources.ApplyResources(this.queryAnalyzerLabel, "queryAnalyzerLabel");
			this.queryAnalyzerLabel.BackColor = System.Drawing.Color.Transparent;
			this.queryAnalyzerLabel.Name = "queryAnalyzerLabel";
			// 
			// logoBox
			// 
			resources.ApplyResources(this.logoBox, "logoBox");
			this.logoBox.Name = "logoBox";
			this.logoBox.TabStop = false;
			// 
			// orcaBox
			// 
			resources.ApplyResources(this.orcaBox, "orcaBox");
			this.orcaBox.Name = "orcaBox";
			this.orcaBox.TabStop = false;
			// 
			// tabset
			// 
			this.tabset.Controls.Add(this.conPage);
			this.tabset.Controls.Add(this.locPage);
			this.tabset.Controls.Add(this.historyPage);
			resources.ApplyResources(this.tabset, "tabset");
			this.tabset.Name = "tabset";
			this.tabset.SelectedIndex = 0;
			this.tabset.SelectedIndexChanged += new System.EventHandler(this.DoSelectTab);
			// 
			// conPage
			// 
			this.conPage.BackColor = System.Drawing.Color.Transparent;
			this.conPage.Controls.Add(this.paramView);
			this.conPage.Controls.Add(this.conView);
			this.conPage.Controls.Add(this.okButton1);
			resources.ApplyResources(this.conPage, "conPage");
			this.conPage.Name = "conPage";
			// 
			// paramView
			// 
			resources.ApplyResources(this.paramView, "paramView");
			this.paramView.BackColor = System.Drawing.Color.GhostWhite;
			this.paramView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.paramCol,
            this.paramValueCol});
			this.paramView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.paramView.MultiSelect = false;
			this.paramView.Name = "paramView";
			this.paramView.UseCompatibleStateImageBehavior = false;
			this.paramView.View = System.Windows.Forms.View.Details;
			// 
			// paramCol
			// 
			resources.ApplyResources(this.paramCol, "paramCol");
			// 
			// paramValueCol
			// 
			resources.ApplyResources(this.paramValueCol, "paramValueCol");
			// 
			// conView
			// 
			resources.ApplyResources(this.conView, "conView");
			this.conView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.instanceCol,
            this.hostCol,
            this.versionCol,
            this.startedCol});
			this.conView.GridLines = true;
			this.conView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.conView.HideSelection = false;
			this.conView.MultiSelect = false;
			this.conView.Name = "conView";
			this.conView.UseCompatibleStateImageBehavior = false;
			this.conView.View = System.Windows.Forms.View.Details;
			this.conView.SelectedIndexChanged += new System.EventHandler(this.DoSelectConnection);
			// 
			// instanceCol
			// 
			resources.ApplyResources(this.instanceCol, "instanceCol");
			// 
			// hostCol
			// 
			resources.ApplyResources(this.hostCol, "hostCol");
			// 
			// versionCol
			// 
			resources.ApplyResources(this.versionCol, "versionCol");
			// 
			// startedCol
			// 
			resources.ApplyResources(this.startedCol, "startedCol");
			// 
			// okButton1
			// 
			resources.ApplyResources(this.okButton1, "okButton1");
			this.okButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton1.Name = "okButton1";
			this.okButton1.Click += new System.EventHandler(this.DoClose);
			// 
			// locPage
			// 
			this.locPage.BackColor = System.Drawing.Color.Transparent;
			this.locPage.Controls.Add(this.configView);
			this.locPage.Controls.Add(this.okButton2);
			resources.ApplyResources(this.locPage, "locPage");
			this.locPage.Name = "locPage";
			// 
			// configView
			// 
			resources.ApplyResources(this.configView, "configView");
			this.configView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.keyCol,
            this.valueCol});
			this.configView.GridLines = true;
			this.configView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.configView.Name = "configView";
			this.configView.UseCompatibleStateImageBehavior = false;
			this.configView.View = System.Windows.Forms.View.Details;
			// 
			// keyCol
			// 
			resources.ApplyResources(this.keyCol, "keyCol");
			// 
			// valueCol
			// 
			resources.ApplyResources(this.valueCol, "valueCol");
			// 
			// okButton2
			// 
			resources.ApplyResources(this.okButton2, "okButton2");
			this.okButton2.Name = "okButton2";
			this.okButton2.Click += new System.EventHandler(this.DoClose);
			// 
			// historyPage
			// 
			this.historyPage.BackColor = System.Drawing.Color.Transparent;
			this.historyPage.Controls.Add(this.historyBox);
			this.historyPage.Controls.Add(this.okButtonHistory);
			resources.ApplyResources(this.historyPage, "historyPage");
			this.historyPage.Name = "historyPage";
			// 
			// historyBox
			// 
			resources.ApplyResources(this.historyBox, "historyBox");
			this.historyBox.BackColor = System.Drawing.SystemColors.Window;
			this.historyBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.historyBox.Name = "historyBox";
			this.historyBox.ReadOnly = true;
			// 
			// okButtonHistory
			// 
			resources.ApplyResources(this.okButtonHistory, "okButtonHistory");
			this.okButtonHistory.Name = "okButtonHistory";
			this.okButtonHistory.Click += new System.EventHandler(this.DoClose);
			// 
			// detailPanel
			// 
			this.detailPanel.BackColor = System.Drawing.Color.Transparent;
			this.detailPanel.Controls.Add(this.tabset);
			resources.ApplyResources(this.detailPanel, "detailPanel");
			this.detailPanel.Name = "detailPanel";
			// 
			// AboutDialog
			// 
			this.AcceptButton = this.okButton1;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.okButton1;
			this.Controls.Add(this.detailPanel);
			this.Controls.Add(this.headerPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.headerPanel.ResumeLayout(false);
			this.headerPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.orcaBox)).EndInit();
			this.tabset.ResumeLayout(false);
			this.conPage.ResumeLayout(false);
			this.locPage.ResumeLayout(false);
			this.historyPage.ResumeLayout(false);
			this.detailPanel.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label queryAnalyzerLabel;
        private System.Windows.Forms.PictureBox logoBox;
        private System.Windows.Forms.PictureBox orcaBox;
        private System.Windows.Forms.TabControl tabset;
        private System.Windows.Forms.TabPage conPage;
        private System.Windows.Forms.TabPage locPage;
        private System.Windows.Forms.Panel detailPanel;
        private System.Windows.Forms.Button okButton1;
        private System.Windows.Forms.ListView conView;
        private System.Windows.Forms.ColumnHeader instanceCol;
        private System.Windows.Forms.ColumnHeader hostCol;
        private System.Windows.Forms.ColumnHeader versionCol;
        private System.Windows.Forms.ColumnHeader startedCol;
        private System.Windows.Forms.Button okButton2;
        private System.Windows.Forms.ListView configView;
        private System.Windows.Forms.ColumnHeader keyCol;
        private System.Windows.Forms.ColumnHeader valueCol;
		private System.Windows.Forms.TabPage historyPage;
		private System.Windows.Forms.Button okButtonHistory;
		private System.Windows.Forms.RichTextBox historyBox;
		private System.Windows.Forms.ListView paramView;
		private System.Windows.Forms.ColumnHeader paramCol;
		private System.Windows.Forms.ColumnHeader paramValueCol;
    }
}