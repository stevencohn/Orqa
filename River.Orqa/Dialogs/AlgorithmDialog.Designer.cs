namespace River.Orqa.Dialogs
{
	partial class AlgorithmDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlgorithmDialog));
			this.introLabel = new System.Windows.Forms.Label();
			this.methodGroup = new System.Windows.Forms.GroupBox();
			this.sequentialRadio = new System.Windows.Forms.RadioButton();
			this.wrapRadio = new System.Windows.Forms.RadioButton();
			this.blockRadio = new System.Windows.Forms.RadioButton();
			this.storeBox = new System.Windows.Forms.CheckBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.methodGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// introLabel
			// 
			this.introLabel.BackColor = System.Drawing.SystemColors.Window;
			this.introLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.introLabel, "introLabel");
			this.introLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
			this.introLabel.Name = "introLabel";
			this.introLabel.Padding = new System.Windows.Forms.Padding(10);
			// 
			// methodGroup
			// 
			resources.ApplyResources(this.methodGroup, "methodGroup");
			this.methodGroup.Controls.Add(this.blockRadio);
			this.methodGroup.Controls.Add(this.wrapRadio);
			this.methodGroup.Controls.Add(this.sequentialRadio);
			this.methodGroup.Name = "methodGroup";
			this.methodGroup.Padding = new System.Windows.Forms.Padding(20, 6, 15, 3);
			this.methodGroup.TabStop = false;
			// 
			// sequentialRadio
			// 
			resources.ApplyResources(this.sequentialRadio, "sequentialRadio");
			this.sequentialRadio.Checked = true;
			this.sequentialRadio.Name = "sequentialRadio";
			// 
			// wrapRadio
			// 
			resources.ApplyResources(this.wrapRadio, "wrapRadio");
			this.wrapRadio.Name = "wrapRadio";
			this.wrapRadio.TabStop = false;
			// 
			// blockRadio
			// 
			resources.ApplyResources(this.blockRadio, "blockRadio");
			this.blockRadio.Name = "blockRadio";
			this.blockRadio.TabStop = false;
			// 
			// storeBox
			// 
			resources.ApplyResources(this.storeBox, "storeBox");
			this.storeBox.Name = "storeBox";
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			// 
			// AlgorithmDialog
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.storeBox);
			this.Controls.Add(this.methodGroup);
			this.Controls.Add(this.introLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AlgorithmDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.methodGroup.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label introLabel;
		private System.Windows.Forms.GroupBox methodGroup;
		private System.Windows.Forms.RadioButton blockRadio;
		private System.Windows.Forms.RadioButton wrapRadio;
		private System.Windows.Forms.RadioButton sequentialRadio;
		private System.Windows.Forms.CheckBox storeBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
	}
}