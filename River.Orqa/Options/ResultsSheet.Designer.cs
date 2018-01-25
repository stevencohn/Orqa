namespace River.Orqa.Options
{
	partial class ResultsSheet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsSheet));
			this.targetLabel = new System.Windows.Forms.Label();
			this.formatLabel = new System.Windows.Forms.Label();
			this.delimeterLabel = new System.Windows.Forms.Label();
			this.maxLabel = new System.Windows.Forms.Label();
			this.targetBox = new System.Windows.Forms.ComboBox();
			this.formatBox = new System.Windows.Forms.ComboBox();
			this.maxCharBox = new System.Windows.Forms.NumericUpDown();
			this.delimeterBox = new System.Windows.Forms.TextBox();
			this.printHeadersCheck = new System.Windows.Forms.CheckBox();
			this.scrollResultsCheck = new System.Windows.Forms.CheckBox();
			this.rightAlignCheck = new System.Windows.Forms.CheckBox();
			this.outputQueryCheck = new System.Windows.Forms.CheckBox();
			this.dbmsOutputCheck = new System.Windows.Forms.CheckBox();
			this.optionsGroup = new System.Windows.Forms.GroupBox();
			this.cleanNewlinesCheck = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.maxCharBox)).BeginInit();
			this.optionsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// targetLabel
			// 
			resources.ApplyResources(this.targetLabel, "targetLabel");
			this.targetLabel.Name = "targetLabel";
			// 
			// formatLabel
			// 
			resources.ApplyResources(this.formatLabel, "formatLabel");
			this.formatLabel.Name = "formatLabel";
			// 
			// delimeterLabel
			// 
			resources.ApplyResources(this.delimeterLabel, "delimeterLabel");
			this.delimeterLabel.Name = "delimeterLabel";
			// 
			// maxLabel
			// 
			resources.ApplyResources(this.maxLabel, "maxLabel");
			this.maxLabel.Name = "maxLabel";
			// 
			// targetBox
			// 
			this.targetBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.targetBox.FormattingEnabled = true;
			resources.ApplyResources(this.targetBox, "targetBox");
			this.targetBox.Name = "targetBox";
			this.targetBox.SelectedIndexChanged += new System.EventHandler(this.ChangeTarget);
			// 
			// formatBox
			// 
			this.formatBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.formatBox.FormattingEnabled = true;
			resources.ApplyResources(this.formatBox, "formatBox");
			this.formatBox.Name = "formatBox";
			this.formatBox.SelectedIndexChanged += new System.EventHandler(this.ChangeFormat);
			// 
			// maxCharBox
			// 
			resources.ApplyResources(this.maxCharBox, "maxCharBox");
			this.maxCharBox.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.maxCharBox.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.maxCharBox.Name = "maxCharBox";
			this.maxCharBox.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
			// 
			// delimeterBox
			// 
			resources.ApplyResources(this.delimeterBox, "delimeterBox");
			this.delimeterBox.Name = "delimeterBox";
			// 
			// printHeadersCheck
			// 
			resources.ApplyResources(this.printHeadersCheck, "printHeadersCheck");
			this.printHeadersCheck.Name = "printHeadersCheck";
			// 
			// scrollResultsCheck
			// 
			resources.ApplyResources(this.scrollResultsCheck, "scrollResultsCheck");
			this.scrollResultsCheck.Name = "scrollResultsCheck";
			// 
			// rightAlignCheck
			// 
			resources.ApplyResources(this.rightAlignCheck, "rightAlignCheck");
			this.rightAlignCheck.Name = "rightAlignCheck";
			// 
			// outputQueryCheck
			// 
			resources.ApplyResources(this.outputQueryCheck, "outputQueryCheck");
			this.outputQueryCheck.Name = "outputQueryCheck";
			// 
			// dbmsOutputCheck
			// 
			resources.ApplyResources(this.dbmsOutputCheck, "dbmsOutputCheck");
			this.dbmsOutputCheck.Name = "dbmsOutputCheck";
			// 
			// optionsGroup
			// 
			resources.ApplyResources(this.optionsGroup, "optionsGroup");
			this.optionsGroup.Controls.Add(this.cleanNewlinesCheck);
			this.optionsGroup.Controls.Add(this.printHeadersCheck);
			this.optionsGroup.Controls.Add(this.dbmsOutputCheck);
			this.optionsGroup.Controls.Add(this.scrollResultsCheck);
			this.optionsGroup.Controls.Add(this.outputQueryCheck);
			this.optionsGroup.Controls.Add(this.rightAlignCheck);
			this.optionsGroup.Name = "optionsGroup";
			this.optionsGroup.Padding = new System.Windows.Forms.Padding(25, 5, 10, 10);
			this.optionsGroup.TabStop = false;
			// 
			// cleanNewlinesCheck
			// 
			resources.ApplyResources(this.cleanNewlinesCheck, "cleanNewlinesCheck");
			this.cleanNewlinesCheck.Name = "cleanNewlinesCheck";
			// 
			// ResultsSheet
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.Controls.Add(this.optionsGroup);
			this.Controls.Add(this.delimeterBox);
			this.Controls.Add(this.maxCharBox);
			this.Controls.Add(this.formatBox);
			this.Controls.Add(this.targetBox);
			this.Controls.Add(this.maxLabel);
			this.Controls.Add(this.delimeterLabel);
			this.Controls.Add(this.formatLabel);
			this.Controls.Add(this.targetLabel);
			this.Name = "ResultsSheet";
			((System.ComponentModel.ISupportInitialize)(this.maxCharBox)).EndInit();
			this.optionsGroup.ResumeLayout(false);
			this.optionsGroup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label targetLabel;
		private System.Windows.Forms.Label formatLabel;
		private System.Windows.Forms.Label delimeterLabel;
		private System.Windows.Forms.Label maxLabel;
		private System.Windows.Forms.ComboBox targetBox;
		private System.Windows.Forms.ComboBox formatBox;
		private System.Windows.Forms.NumericUpDown maxCharBox;
		private System.Windows.Forms.TextBox delimeterBox;
		private System.Windows.Forms.CheckBox printHeadersCheck;
		private System.Windows.Forms.CheckBox scrollResultsCheck;
		private System.Windows.Forms.CheckBox rightAlignCheck;
		private System.Windows.Forms.CheckBox outputQueryCheck;
		private System.Windows.Forms.CheckBox dbmsOutputCheck;
		private System.Windows.Forms.GroupBox optionsGroup;
		private System.Windows.Forms.CheckBox cleanNewlinesCheck;
	}
}
