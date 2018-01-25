namespace River.Orqa.Options
{
	partial class EditorSheet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorSheet));
			this.gutterGroup = new System.Windows.Forms.GroupBox();
			this.lineNumbersCheck = new System.Windows.Forms.CheckBox();
			this.gutterWidthBox = new System.Windows.Forms.NumericUpDown();
			this.gutterWidthLabel = new System.Windows.Forms.Label();
			this.showGutterCheck = new System.Windows.Forms.CheckBox();
			this.showMarginCheck = new System.Windows.Forms.CheckBox();
			this.marginLabel = new System.Windows.Forms.Label();
			this.marginPositionBox = new System.Windows.Forms.NumericUpDown();
			this.wordWrapCheck = new System.Windows.Forms.CheckBox();
			this.wrapAtMarginCheck = new System.Windows.Forms.CheckBox();
			this.marginGroup = new System.Windows.Forms.GroupBox();
			this.navigationGroup = new System.Windows.Forms.GroupBox();
			this.hscrollCheck = new System.Windows.Forms.CheckBox();
			this.vscrollCheck = new System.Windows.Forms.CheckBox();
			this.beofCheck = new System.Windows.Forms.CheckBox();
			this.beolnCheck = new System.Windows.Forms.CheckBox();
			this.gutterGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gutterWidthBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.marginPositionBox)).BeginInit();
			this.marginGroup.SuspendLayout();
			this.navigationGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// gutterGroup
			// 
			resources.ApplyResources(this.gutterGroup, "gutterGroup");
			this.gutterGroup.Controls.Add(this.lineNumbersCheck);
			this.gutterGroup.Controls.Add(this.gutterWidthBox);
			this.gutterGroup.Controls.Add(this.gutterWidthLabel);
			this.gutterGroup.Controls.Add(this.showGutterCheck);
			this.gutterGroup.Name = "gutterGroup";
			this.gutterGroup.Padding = new System.Windows.Forms.Padding(25, 5, 10, 5);
			this.gutterGroup.TabStop = false;
			// 
			// lineNumbersCheck
			// 
			resources.ApplyResources(this.lineNumbersCheck, "lineNumbersCheck");
			this.lineNumbersCheck.Name = "lineNumbersCheck";
			// 
			// gutterWidthBox
			// 
			resources.ApplyResources(this.gutterWidthBox, "gutterWidthBox");
			this.gutterWidthBox.Name = "gutterWidthBox";
			// 
			// gutterWidthLabel
			// 
			resources.ApplyResources(this.gutterWidthLabel, "gutterWidthLabel");
			this.gutterWidthLabel.Name = "gutterWidthLabel";
			// 
			// showGutterCheck
			// 
			resources.ApplyResources(this.showGutterCheck, "showGutterCheck");
			this.showGutterCheck.Name = "showGutterCheck";
			this.showGutterCheck.CheckedChanged += new System.EventHandler(this.DoToggleGutter);
			// 
			// showMarginCheck
			// 
			resources.ApplyResources(this.showMarginCheck, "showMarginCheck");
			this.showMarginCheck.Name = "showMarginCheck";
			// 
			// marginLabel
			// 
			resources.ApplyResources(this.marginLabel, "marginLabel");
			this.marginLabel.Name = "marginLabel";
			// 
			// marginPositionBox
			// 
			resources.ApplyResources(this.marginPositionBox, "marginPositionBox");
			this.marginPositionBox.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.marginPositionBox.Name = "marginPositionBox";
			// 
			// wordWrapCheck
			// 
			resources.ApplyResources(this.wordWrapCheck, "wordWrapCheck");
			this.wordWrapCheck.Name = "wordWrapCheck";
			this.wordWrapCheck.CheckedChanged += new System.EventHandler(this.DoToggleWordWrap);
			// 
			// wrapAtMarginCheck
			// 
			resources.ApplyResources(this.wrapAtMarginCheck, "wrapAtMarginCheck");
			this.wrapAtMarginCheck.Name = "wrapAtMarginCheck";
			// 
			// marginGroup
			// 
			resources.ApplyResources(this.marginGroup, "marginGroup");
			this.marginGroup.Controls.Add(this.showMarginCheck);
			this.marginGroup.Controls.Add(this.wrapAtMarginCheck);
			this.marginGroup.Controls.Add(this.marginLabel);
			this.marginGroup.Controls.Add(this.wordWrapCheck);
			this.marginGroup.Controls.Add(this.marginPositionBox);
			this.marginGroup.Name = "marginGroup";
			this.marginGroup.Padding = new System.Windows.Forms.Padding(25, 5, 10, 10);
			this.marginGroup.TabStop = false;
			// 
			// navigationGroup
			// 
			resources.ApplyResources(this.navigationGroup, "navigationGroup");
			this.navigationGroup.Controls.Add(this.hscrollCheck);
			this.navigationGroup.Controls.Add(this.vscrollCheck);
			this.navigationGroup.Controls.Add(this.beofCheck);
			this.navigationGroup.Controls.Add(this.beolnCheck);
			this.navigationGroup.Name = "navigationGroup";
			this.navigationGroup.Padding = new System.Windows.Forms.Padding(25, 5, 10, 10);
			this.navigationGroup.TabStop = false;
			// 
			// hscrollCheck
			// 
			resources.ApplyResources(this.hscrollCheck, "hscrollCheck");
			this.hscrollCheck.Name = "hscrollCheck";
			// 
			// vscrollCheck
			// 
			resources.ApplyResources(this.vscrollCheck, "vscrollCheck");
			this.vscrollCheck.Name = "vscrollCheck";
			// 
			// beofCheck
			// 
			resources.ApplyResources(this.beofCheck, "beofCheck");
			this.beofCheck.Name = "beofCheck";
			// 
			// beolnCheck
			// 
			resources.ApplyResources(this.beolnCheck, "beolnCheck");
			this.beolnCheck.Name = "beolnCheck";
			// 
			// EditorSheet
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.Controls.Add(this.navigationGroup);
			this.Controls.Add(this.marginGroup);
			this.Controls.Add(this.gutterGroup);
			this.Name = "EditorSheet";
			this.gutterGroup.ResumeLayout(false);
			this.gutterGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gutterWidthBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.marginPositionBox)).EndInit();
			this.marginGroup.ResumeLayout(false);
			this.marginGroup.PerformLayout();
			this.navigationGroup.ResumeLayout(false);
			this.navigationGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gutterGroup;
		private System.Windows.Forms.CheckBox showGutterCheck;
		private System.Windows.Forms.Label gutterWidthLabel;
		private System.Windows.Forms.NumericUpDown gutterWidthBox;
		private System.Windows.Forms.CheckBox showMarginCheck;
		private System.Windows.Forms.Label marginLabel;
		private System.Windows.Forms.NumericUpDown marginPositionBox;
		private System.Windows.Forms.CheckBox wordWrapCheck;
		private System.Windows.Forms.CheckBox wrapAtMarginCheck;
		private System.Windows.Forms.CheckBox lineNumbersCheck;
		private System.Windows.Forms.GroupBox marginGroup;
		private System.Windows.Forms.GroupBox navigationGroup;
		private System.Windows.Forms.CheckBox beofCheck;
		private System.Windows.Forms.CheckBox beolnCheck;
		private System.Windows.Forms.CheckBox hscrollCheck;
		private System.Windows.Forms.CheckBox vscrollCheck;
	}
}
