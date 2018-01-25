namespace River.Orqa.Options
{
	partial class GeneralSheet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralSheet));
			this.queryDirLabel = new System.Windows.Forms.Label();
			this.queryDirBox = new System.Windows.Forms.TextBox();
			this.queryDirButton = new System.Windows.Forms.Button();
			this.resultsDirBox = new System.Windows.Forms.TextBox();
			this.templateDirBox = new System.Windows.Forms.TextBox();
			this.resultsDirLabel = new System.Windows.Forms.Label();
			this.templateDirLabel = new System.Windows.Forms.Label();
			this.resultsDirButton = new System.Windows.Forms.Button();
			this.templateDirButton = new System.Windows.Forms.Button();
			this.queryExtLabel = new System.Windows.Forms.Label();
			this.resultsExtLabel = new System.Windows.Forms.Label();
			this.templateExtLabel = new System.Windows.Forms.Label();
			this.queryExtBox = new System.Windows.Forms.TextBox();
			this.resultsExtBox = new System.Windows.Forms.TextBox();
			this.templateExtBox = new System.Windows.Forms.TextBox();
			this.maxMruBox = new System.Windows.Forms.NumericUpDown();
			this.maxMruLabel = new System.Windows.Forms.Label();
			this.savePositionCheck = new System.Windows.Forms.CheckBox();
			this.defaultAppCheck = new System.Windows.Forms.CheckBox();
			this.saveModifiedCheck = new System.Windows.Forms.CheckBox();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.optionsGroup = new System.Windows.Forms.GroupBox();
			this.reportExtLabel = new System.Windows.Forms.Label();
			this.reportExtBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.maxMruBox)).BeginInit();
			this.optionsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// queryDirLabel
			// 
			resources.ApplyResources(this.queryDirLabel, "queryDirLabel");
			this.queryDirLabel.Name = "queryDirLabel";
			// 
			// queryDirBox
			// 
			resources.ApplyResources(this.queryDirBox, "queryDirBox");
			this.queryDirBox.Name = "queryDirBox";
			// 
			// queryDirButton
			// 
			resources.ApplyResources(this.queryDirButton, "queryDirButton");
			this.queryDirButton.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.queryDirButton.Name = "queryDirButton";
			this.queryDirButton.UseVisualStyleBackColor = false;
			this.queryDirButton.Click += new System.EventHandler(this.BrowseQueryDir);
			// 
			// resultsDirBox
			// 
			resources.ApplyResources(this.resultsDirBox, "resultsDirBox");
			this.resultsDirBox.Name = "resultsDirBox";
			// 
			// templateDirBox
			// 
			resources.ApplyResources(this.templateDirBox, "templateDirBox");
			this.templateDirBox.Name = "templateDirBox";
			// 
			// resultsDirLabel
			// 
			resources.ApplyResources(this.resultsDirLabel, "resultsDirLabel");
			this.resultsDirLabel.Name = "resultsDirLabel";
			// 
			// templateDirLabel
			// 
			resources.ApplyResources(this.templateDirLabel, "templateDirLabel");
			this.templateDirLabel.Name = "templateDirLabel";
			// 
			// resultsDirButton
			// 
			resources.ApplyResources(this.resultsDirButton, "resultsDirButton");
			this.resultsDirButton.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.resultsDirButton.Name = "resultsDirButton";
			this.resultsDirButton.UseVisualStyleBackColor = false;
			this.resultsDirButton.Click += new System.EventHandler(this.BrowseResultsDir);
			// 
			// templateDirButton
			// 
			resources.ApplyResources(this.templateDirButton, "templateDirButton");
			this.templateDirButton.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.templateDirButton.Name = "templateDirButton";
			this.templateDirButton.UseVisualStyleBackColor = false;
			this.templateDirButton.Click += new System.EventHandler(this.BrowseTemplateDir);
			// 
			// queryExtLabel
			// 
			resources.ApplyResources(this.queryExtLabel, "queryExtLabel");
			this.queryExtLabel.Name = "queryExtLabel";
			// 
			// resultsExtLabel
			// 
			resources.ApplyResources(this.resultsExtLabel, "resultsExtLabel");
			this.resultsExtLabel.Name = "resultsExtLabel";
			// 
			// templateExtLabel
			// 
			resources.ApplyResources(this.templateExtLabel, "templateExtLabel");
			this.templateExtLabel.Name = "templateExtLabel";
			// 
			// queryExtBox
			// 
			resources.ApplyResources(this.queryExtBox, "queryExtBox");
			this.queryExtBox.Name = "queryExtBox";
			// 
			// resultsExtBox
			// 
			resources.ApplyResources(this.resultsExtBox, "resultsExtBox");
			this.resultsExtBox.Name = "resultsExtBox";
			// 
			// templateExtBox
			// 
			resources.ApplyResources(this.templateExtBox, "templateExtBox");
			this.templateExtBox.Name = "templateExtBox";
			// 
			// maxMruBox
			// 
			resources.ApplyResources(this.maxMruBox, "maxMruBox");
			this.maxMruBox.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.maxMruBox.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.maxMruBox.Name = "maxMruBox";
			this.maxMruBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			// 
			// maxMruLabel
			// 
			resources.ApplyResources(this.maxMruLabel, "maxMruLabel");
			this.maxMruLabel.Name = "maxMruLabel";
			// 
			// savePositionCheck
			// 
			resources.ApplyResources(this.savePositionCheck, "savePositionCheck");
			this.savePositionCheck.Name = "savePositionCheck";
			// 
			// defaultAppCheck
			// 
			resources.ApplyResources(this.defaultAppCheck, "defaultAppCheck");
			this.defaultAppCheck.Name = "defaultAppCheck";
			// 
			// saveModifiedCheck
			// 
			resources.ApplyResources(this.saveModifiedCheck, "saveModifiedCheck");
			this.saveModifiedCheck.Name = "saveModifiedCheck";
			// 
			// optionsGroup
			// 
			resources.ApplyResources(this.optionsGroup, "optionsGroup");
			this.optionsGroup.Controls.Add(this.savePositionCheck);
			this.optionsGroup.Controls.Add(this.saveModifiedCheck);
			this.optionsGroup.Controls.Add(this.defaultAppCheck);
			this.optionsGroup.Name = "optionsGroup";
			this.optionsGroup.TabStop = false;
			// 
			// reportExtLabel
			// 
			resources.ApplyResources(this.reportExtLabel, "reportExtLabel");
			this.reportExtLabel.Name = "reportExtLabel";
			// 
			// reportExtBox
			// 
			resources.ApplyResources(this.reportExtBox, "reportExtBox");
			this.reportExtBox.Name = "reportExtBox";
			// 
			// GeneralSheet
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.Controls.Add(this.reportExtBox);
			this.Controls.Add(this.reportExtLabel);
			this.Controls.Add(this.optionsGroup);
			this.Controls.Add(this.maxMruLabel);
			this.Controls.Add(this.maxMruBox);
			this.Controls.Add(this.templateExtBox);
			this.Controls.Add(this.resultsExtBox);
			this.Controls.Add(this.queryExtBox);
			this.Controls.Add(this.templateExtLabel);
			this.Controls.Add(this.resultsExtLabel);
			this.Controls.Add(this.queryExtLabel);
			this.Controls.Add(this.templateDirButton);
			this.Controls.Add(this.resultsDirButton);
			this.Controls.Add(this.templateDirLabel);
			this.Controls.Add(this.resultsDirLabel);
			this.Controls.Add(this.templateDirBox);
			this.Controls.Add(this.resultsDirBox);
			this.Controls.Add(this.queryDirButton);
			this.Controls.Add(this.queryDirBox);
			this.Controls.Add(this.queryDirLabel);
			this.Name = "GeneralSheet";
			((System.ComponentModel.ISupportInitialize)(this.maxMruBox)).EndInit();
			this.optionsGroup.ResumeLayout(false);
			this.optionsGroup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label queryDirLabel;
		private System.Windows.Forms.TextBox queryDirBox;
		private System.Windows.Forms.Button queryDirButton;
		private System.Windows.Forms.TextBox resultsDirBox;
		private System.Windows.Forms.TextBox templateDirBox;
		private System.Windows.Forms.Label resultsDirLabel;
		private System.Windows.Forms.Label templateDirLabel;
		private System.Windows.Forms.Button resultsDirButton;
		private System.Windows.Forms.Button templateDirButton;
		private System.Windows.Forms.Label queryExtLabel;
		private System.Windows.Forms.Label resultsExtLabel;
		private System.Windows.Forms.Label templateExtLabel;
		private System.Windows.Forms.TextBox queryExtBox;
		private System.Windows.Forms.TextBox resultsExtBox;
		private System.Windows.Forms.TextBox templateExtBox;
		private System.Windows.Forms.NumericUpDown maxMruBox;
		private System.Windows.Forms.Label maxMruLabel;
		private System.Windows.Forms.CheckBox savePositionCheck;
		private System.Windows.Forms.CheckBox defaultAppCheck;
		private System.Windows.Forms.CheckBox saveModifiedCheck;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.GroupBox optionsGroup;
		private System.Windows.Forms.Label reportExtLabel;
		private System.Windows.Forms.TextBox reportExtBox;
	}
}
