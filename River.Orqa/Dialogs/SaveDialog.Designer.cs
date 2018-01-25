namespace River.Orqa.Dialogs
{
	partial class SaveDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveDialog));
			this.saveLabel = new System.Windows.Forms.Label();
			this.itemBox = new System.Windows.Forms.TextBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.noButton = new System.Windows.Forms.Button();
			this.yesButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// saveLabel
			// 
			resources.ApplyResources(this.saveLabel, "saveLabel");
			this.saveLabel.Name = "saveLabel";
			// 
			// itemBox
			// 
			resources.ApplyResources(this.itemBox, "itemBox");
			this.itemBox.BackColor = System.Drawing.SystemColors.Window;
			this.itemBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.itemBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.itemBox.Name = "itemBox";
			this.itemBox.ReadOnly = true;
			this.itemBox.TabStop = false;
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			// 
			// noButton
			// 
			resources.ApplyResources(this.noButton, "noButton");
			this.noButton.DialogResult = System.Windows.Forms.DialogResult.No;
			this.noButton.Name = "noButton";
			// 
			// yesButton
			// 
			resources.ApplyResources(this.yesButton, "yesButton");
			this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.yesButton.Name = "yesButton";
			// 
			// SaveDialog
			// 
			this.AcceptButton = this.yesButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.yesButton);
			this.Controls.Add(this.noButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.itemBox);
			this.Controls.Add(this.saveLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SaveDialog";
			this.ShowIcon = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DoFormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label saveLabel;
		private System.Windows.Forms.TextBox itemBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button noButton;
		private System.Windows.Forms.Button yesButton;
	}
}