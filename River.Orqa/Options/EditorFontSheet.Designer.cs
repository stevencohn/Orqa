namespace River.Orqa.Options
{
	partial class EditorFontSheet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorFontSheet));
			this.familyBox = new System.Windows.Forms.ComboBox();
			this.sizeBox = new System.Windows.Forms.NumericUpDown();
			this.familyLabel = new System.Windows.Forms.Label();
			this.sizeLabel = new System.Windows.Forms.Label();
			this.itemsBox = new System.Windows.Forms.ListBox();
			this.itemsLabel = new System.Windows.Forms.Label();
			this.foreLabel = new System.Windows.Forms.Label();
			this.backLabel = new System.Windows.Forms.Label();
			this.foreColorBox = new System.Windows.Forms.ComboBox();
			this.backColorBox = new System.Windows.Forms.ComboBox();
			this.boldCheck = new System.Windows.Forms.CheckBox();
			this.sample = new System.Windows.Forms.Label();
			this.sampleLabel = new System.Windows.Forms.Label();
			this.italicCheck = new System.Windows.Forms.CheckBox();
			this.customForeButton = new System.Windows.Forms.Button();
			this.customBackButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.sizeBox)).BeginInit();
			this.SuspendLayout();
			// 
			// familyBox
			// 
			this.familyBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.familyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.familyBox, "familyBox");
			this.familyBox.FormattingEnabled = true;
			this.familyBox.Name = "familyBox";
			this.familyBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawFontFamilyComboItem);
			// 
			// sizeBox
			// 
			resources.ApplyResources(this.sizeBox, "sizeBox");
			this.sizeBox.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
			this.sizeBox.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.sizeBox.Name = "sizeBox";
			this.sizeBox.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			// 
			// familyLabel
			// 
			resources.ApplyResources(this.familyLabel, "familyLabel");
			this.familyLabel.Name = "familyLabel";
			// 
			// sizeLabel
			// 
			resources.ApplyResources(this.sizeLabel, "sizeLabel");
			this.sizeLabel.Name = "sizeLabel";
			// 
			// itemsBox
			// 
			resources.ApplyResources(this.itemsBox, "itemsBox");
			this.itemsBox.Name = "itemsBox";
			this.itemsBox.SelectedIndexChanged += new System.EventHandler(this.DoChangeDisplayItem);
			// 
			// itemsLabel
			// 
			resources.ApplyResources(this.itemsLabel, "itemsLabel");
			this.itemsLabel.Name = "itemsLabel";
			// 
			// foreLabel
			// 
			resources.ApplyResources(this.foreLabel, "foreLabel");
			this.foreLabel.Name = "foreLabel";
			// 
			// backLabel
			// 
			resources.ApplyResources(this.backLabel, "backLabel");
			this.backLabel.Name = "backLabel";
			// 
			// foreColorBox
			// 
			this.foreColorBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.foreColorBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.foreColorBox.FormattingEnabled = true;
			resources.ApplyResources(this.foreColorBox, "foreColorBox");
			this.foreColorBox.Name = "foreColorBox";
			this.foreColorBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawColorComboItem);
			// 
			// backColorBox
			// 
			this.backColorBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.backColorBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.backColorBox.FormattingEnabled = true;
			resources.ApplyResources(this.backColorBox, "backColorBox");
			this.backColorBox.Name = "backColorBox";
			this.backColorBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawColorComboItem);
			// 
			// boldCheck
			// 
			resources.ApplyResources(this.boldCheck, "boldCheck");
			this.boldCheck.Name = "boldCheck";
			// 
			// sample
			// 
			this.sample.BackColor = System.Drawing.SystemColors.Window;
			this.sample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.sample, "sample");
			this.sample.Name = "sample";
			// 
			// sampleLabel
			// 
			resources.ApplyResources(this.sampleLabel, "sampleLabel");
			this.sampleLabel.Name = "sampleLabel";
			// 
			// italicCheck
			// 
			resources.ApplyResources(this.italicCheck, "italicCheck");
			this.italicCheck.Name = "italicCheck";
			// 
			// customForeButton
			// 
			resources.ApplyResources(this.customForeButton, "customForeButton");
			this.customForeButton.Name = "customForeButton";
			this.customForeButton.Click += new System.EventHandler(this.DoCustomColor);
			// 
			// customBackButton
			// 
			resources.ApplyResources(this.customBackButton, "customBackButton");
			this.customBackButton.Name = "customBackButton";
			this.customBackButton.Click += new System.EventHandler(this.DoCustomColor);
			// 
			// EditorFontSheet
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.Controls.Add(this.customBackButton);
			this.Controls.Add(this.customForeButton);
			this.Controls.Add(this.italicCheck);
			this.Controls.Add(this.sampleLabel);
			this.Controls.Add(this.sample);
			this.Controls.Add(this.boldCheck);
			this.Controls.Add(this.backColorBox);
			this.Controls.Add(this.foreColorBox);
			this.Controls.Add(this.backLabel);
			this.Controls.Add(this.foreLabel);
			this.Controls.Add(this.itemsLabel);
			this.Controls.Add(this.itemsBox);
			this.Controls.Add(this.sizeLabel);
			this.Controls.Add(this.familyLabel);
			this.Controls.Add(this.sizeBox);
			this.Controls.Add(this.familyBox);
			this.Name = "EditorFontSheet";
			((System.ComponentModel.ISupportInitialize)(this.sizeBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox familyBox;
		private System.Windows.Forms.NumericUpDown sizeBox;
		private System.Windows.Forms.Label familyLabel;
		private System.Windows.Forms.Label sizeLabel;
		private System.Windows.Forms.ListBox itemsBox;
		private System.Windows.Forms.Label itemsLabel;
		private System.Windows.Forms.Label foreLabel;
		private System.Windows.Forms.Label backLabel;
		private System.Windows.Forms.ComboBox foreColorBox;
		private System.Windows.Forms.ComboBox backColorBox;
		private System.Windows.Forms.CheckBox boldCheck;
		private System.Windows.Forms.Label sample;
		private System.Windows.Forms.Label sampleLabel;
		private System.Windows.Forms.CheckBox italicCheck;
		private System.Windows.Forms.Button customForeButton;
		private System.Windows.Forms.Button customBackButton;
	}
}
