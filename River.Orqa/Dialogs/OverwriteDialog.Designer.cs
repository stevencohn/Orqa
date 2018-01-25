namespace River.Orqa.Dialogs
{
	partial class OverwriteDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverwriteDialog));
			this.warningMsg = new System.Windows.Forms.Label();
			this.warningIcon = new System.Windows.Forms.PictureBox();
			this.questionLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.warningIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// warningMsg
			// 
			this.warningMsg.AutoSize = true;
			this.warningMsg.Location = new System.Drawing.Point(70, 13);
			this.warningMsg.Name = "warningMsg";
			this.warningMsg.Size = new System.Drawing.Size(276, 13);
			this.warningMsg.TabIndex = 0;
			this.warningMsg.Text = "The file {0} cannot be saved because it is write-protected.";
			// 
			// warningIcon
			// 
			this.warningIcon.Image = ((System.Drawing.Image)(resources.GetObject("warningIcon.Image")));
			this.warningIcon.Location = new System.Drawing.Point(13, 13);
			this.warningIcon.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
			this.warningIcon.Name = "warningIcon";
			this.warningIcon.Size = new System.Drawing.Size(34, 34);
			this.warningIcon.TabIndex = 1;
			this.warningIcon.TabStop = false;
			// 
			// questionLabel
			// 
			this.questionLabel.AutoSize = true;
			this.questionLabel.Location = new System.Drawing.Point(70, 34);
			this.questionLabel.Name = "questionLabel";
			this.questionLabel.Size = new System.Drawing.Size(172, 13);
			this.questionLabel.TabIndex = 2;
			this.questionLabel.Text = "Would you like to overwrite this file?";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(425, 66);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "&Overwrite";
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(506, 66);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			// 
			// OverwriteDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(594, 102);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.questionLabel);
			this.Controls.Add(this.warningIcon);
			this.Controls.Add(this.warningMsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OverwriteDialog";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Save Read-Only File";
			((System.ComponentModel.ISupportInitialize)(this.warningIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label warningMsg;
		private System.Windows.Forms.PictureBox warningIcon;
		private System.Windows.Forms.Label questionLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
	}
}