namespace River.Orqa.Options
{
	partial class EditorTabsSheet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorTabsSheet));
			this.tabGroup = new System.Windows.Forms.GroupBox();
			this.keepTabsRadio = new System.Windows.Forms.RadioButton();
			this.insertSpacesRadio = new System.Windows.Forms.RadioButton();
			this.sizeBox = new System.Windows.Forms.NumericUpDown();
			this.tabSizeLabel = new System.Windows.Forms.Label();
			this.tabGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sizeBox)).BeginInit();
			this.SuspendLayout();
			// 
			// tabGroup
			// 
			resources.ApplyResources(this.tabGroup, "tabGroup");
			this.tabGroup.Controls.Add(this.keepTabsRadio);
			this.tabGroup.Controls.Add(this.insertSpacesRadio);
			this.tabGroup.Controls.Add(this.sizeBox);
			this.tabGroup.Controls.Add(this.tabSizeLabel);
			this.tabGroup.Name = "tabGroup";
			this.tabGroup.Padding = new System.Windows.Forms.Padding(25, 5, 10, 10);
			this.tabGroup.TabStop = false;
			// 
			// keepTabsRadio
			// 
			resources.ApplyResources(this.keepTabsRadio, "keepTabsRadio");
			this.keepTabsRadio.Name = "keepTabsRadio";
			// 
			// insertSpacesRadio
			// 
			resources.ApplyResources(this.insertSpacesRadio, "insertSpacesRadio");
			this.insertSpacesRadio.Name = "insertSpacesRadio";
			// 
			// sizeBox
			// 
			resources.ApplyResources(this.sizeBox, "sizeBox");
			this.sizeBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.sizeBox.Name = "sizeBox";
			// 
			// tabSizeLabel
			// 
			resources.ApplyResources(this.tabSizeLabel, "tabSizeLabel");
			this.tabSizeLabel.Name = "tabSizeLabel";
			// 
			// EditorTabsSheet
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.Controls.Add(this.tabGroup);
			this.Name = "EditorTabsSheet";
			this.tabGroup.ResumeLayout(false);
			this.tabGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sizeBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox tabGroup;
		private System.Windows.Forms.Label tabSizeLabel;
		private System.Windows.Forms.RadioButton keepTabsRadio;
		private System.Windows.Forms.RadioButton insertSpacesRadio;
		private System.Windows.Forms.NumericUpDown sizeBox;
	}
}
