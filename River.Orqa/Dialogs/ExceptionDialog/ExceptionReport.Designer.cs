namespace River.Orqa.Dialogs
{
	partial class ExceptionReport
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionReport));
			this.abortButton = new System.Windows.Forms.Button();
			this.sendButton = new System.Windows.Forms.Button();
			this.msgBox = new System.Windows.Forms.TextBox();
			this.toBox = new System.Windows.Forms.TextBox();
			this.subjectBox = new System.Windows.Forms.TextBox();
			this.titlePanel = new System.Windows.Forms.Panel();
			this.titleBox = new System.Windows.Forms.TextBox();
			this.msgLabel = new System.Windows.Forms.Label();
			this.subjectLabel = new System.Windows.Forms.Label();
			this.toLabel = new System.Windows.Forms.Label();
			this.titlePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// abortButton
			// 
			this.abortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.abortButton.Location = new System.Drawing.Point(485, 289);
			this.abortButton.Name = "abortButton";
			this.abortButton.Size = new System.Drawing.Size(75, 23);
			this.abortButton.TabIndex = 9;
			this.abortButton.Text = "Abort";
			this.abortButton.Click += new System.EventHandler(this.SendAbort);
			// 
			// sendButton
			// 
			this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.sendButton.Location = new System.Drawing.Point(404, 289);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(75, 23);
			this.sendButton.TabIndex = 10;
			this.sendButton.Text = "Send";
			this.sendButton.Click += new System.EventHandler(this.SendReport);
			// 
			// msgBox
			// 
			this.msgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.msgBox.BackColor = System.Drawing.Color.White;
			this.msgBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.msgBox.Location = new System.Drawing.Point(78, 137);
			this.msgBox.Multiline = true;
			this.msgBox.Name = "msgBox";
			this.msgBox.ReadOnly = true;
			this.msgBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.msgBox.Size = new System.Drawing.Size(482, 146);
			this.msgBox.TabIndex = 11;
			this.msgBox.WordWrap = false;
			// 
			// toBox
			// 
			this.toBox.Location = new System.Drawing.Point(78, 85);
			this.toBox.Name = "toBox";
			this.toBox.ReadOnly = true;
			this.toBox.Size = new System.Drawing.Size(300, 20);
			this.toBox.TabIndex = 13;
			this.toBox.Text = "stevencohn@live.com";
			// 
			// subjectBox
			// 
			this.subjectBox.Location = new System.Drawing.Point(78, 110);
			this.subjectBox.Name = "subjectBox";
			this.subjectBox.ReadOnly = true;
			this.subjectBox.Size = new System.Drawing.Size(482, 20);
			this.subjectBox.TabIndex = 14;
			this.subjectBox.Text = "Orqa Exception";
			// 
			// titlePanel
			// 
			this.titlePanel.BackColor = System.Drawing.Color.White;
			this.titlePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.titlePanel.Controls.Add(this.titleBox);
			this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.titlePanel.Location = new System.Drawing.Point(0, 0);
			this.titlePanel.Name = "titlePanel";
			this.titlePanel.Size = new System.Drawing.Size(572, 79);
			this.titlePanel.TabIndex = 15;
			// 
			// titleBox
			// 
			this.titleBox.BackColor = System.Drawing.Color.White;
			this.titleBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.titleBox.Location = new System.Drawing.Point(13, 12);
			this.titleBox.Multiline = true;
			this.titleBox.Name = "titleBox";
			this.titleBox.ReadOnly = true;
			this.titleBox.Size = new System.Drawing.Size(546, 54);
			this.titleBox.TabIndex = 0;
			this.titleBox.Text = resources.GetString("titleBox.Text");
			// 
			// msgLabel
			// 
			this.msgLabel.AutoSize = true;
			this.msgLabel.Location = new System.Drawing.Point(13, 138);
			this.msgLabel.Name = "msgLabel";
			this.msgLabel.Size = new System.Drawing.Size(49, 13);
			this.msgLabel.TabIndex = 16;
			this.msgLabel.Text = "Message:";
			// 
			// subjectLabel
			// 
			this.subjectLabel.AutoSize = true;
			this.subjectLabel.Location = new System.Drawing.Point(13, 113);
			this.subjectLabel.Name = "subjectLabel";
			this.subjectLabel.Size = new System.Drawing.Size(42, 13);
			this.subjectLabel.TabIndex = 17;
			this.subjectLabel.Text = "Subject:";
			// 
			// toLabel
			// 
			this.toLabel.AutoSize = true;
			this.toLabel.Location = new System.Drawing.Point(13, 88);
			this.toLabel.Name = "toLabel";
			this.toLabel.Size = new System.Drawing.Size(19, 13);
			this.toLabel.TabIndex = 18;
			this.toLabel.Text = "To:";
			// 
			// ExceptionReport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.toLabel);
			this.Controls.Add(this.subjectLabel);
			this.Controls.Add(this.msgLabel);
			this.Controls.Add(this.titlePanel);
			this.Controls.Add(this.subjectBox);
			this.Controls.Add(this.toBox);
			this.Controls.Add(this.msgBox);
			this.Controls.Add(this.abortButton);
			this.Controls.Add(this.sendButton);
			this.Name = "ExceptionReport";
			this.Size = new System.Drawing.Size(572, 321);
			this.titlePanel.ResumeLayout(false);
			this.titlePanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button abortButton;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.TextBox msgBox;
		private System.Windows.Forms.TextBox toBox;
		private System.Windows.Forms.TextBox subjectBox;
		private System.Windows.Forms.Panel titlePanel;
		private System.Windows.Forms.Label msgLabel;
		private System.Windows.Forms.Label subjectLabel;
		private System.Windows.Forms.Label toLabel;
		private System.Windows.Forms.TextBox titleBox;
	}
}
