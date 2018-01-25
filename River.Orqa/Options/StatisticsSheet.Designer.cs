namespace River.Orqa.Options
{
	partial class StatisticsSheet
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
			this.infoLabel = new System.Windows.Forms.Label();
			this.userCheck = new System.Windows.Forms.CheckBox();
			this.redoCheck = new System.Windows.Forms.CheckBox();
			this.enqueueCheck = new System.Windows.Forms.CheckBox();
			this.cacheCheck = new System.Windows.Forms.CheckBox();
			this.osCheck = new System.Windows.Forms.CheckBox();
			this.parallelCheck = new System.Windows.Forms.CheckBox();
			this.sqlCheck = new System.Windows.Forms.CheckBox();
			this.debugCheck = new System.Windows.Forms.CheckBox();
			this.classificationGroup = new System.Windows.Forms.GroupBox();
			this.debugLabel = new System.Windows.Forms.Label();
			this.sqlLabel = new System.Windows.Forms.Label();
			this.parallelLabel = new System.Windows.Forms.Label();
			this.osLabel = new System.Windows.Forms.Label();
			this.cacheLabel = new System.Windows.Forms.Label();
			this.enqueueLabel = new System.Windows.Forms.Label();
			this.redoLabel = new System.Windows.Forms.Label();
			this.userLabel = new System.Windows.Forms.Label();
			this.classificationGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// infoLabel
			// 
			this.infoLabel.AutoSize = true;
			this.infoLabel.Location = new System.Drawing.Point(0, 5);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(288, 13);
			this.infoLabel.TabIndex = 0;
			this.infoLabel.Text = "Select the classifications to include when collecting statistics";
			// 
			// userCheck
			// 
			this.userCheck.Location = new System.Drawing.Point(25, 25);
			this.userCheck.Name = "userCheck";
			this.userCheck.Size = new System.Drawing.Size(382, 17);
			this.userCheck.TabIndex = 0;
			this.userCheck.Text = "User";
			// 
			// redoCheck
			// 
			this.redoCheck.Location = new System.Drawing.Point(25, 48);
			this.redoCheck.Name = "redoCheck";
			this.redoCheck.Size = new System.Drawing.Size(382, 17);
			this.redoCheck.TabIndex = 2;
			this.redoCheck.Text = "Redo";
			// 
			// enqueueCheck
			// 
			this.enqueueCheck.Location = new System.Drawing.Point(25, 71);
			this.enqueueCheck.Name = "enqueueCheck";
			this.enqueueCheck.Size = new System.Drawing.Size(382, 17);
			this.enqueueCheck.TabIndex = 4;
			this.enqueueCheck.Text = "Enqueue";
			// 
			// cacheCheck
			// 
			this.cacheCheck.Location = new System.Drawing.Point(25, 94);
			this.cacheCheck.Name = "cacheCheck";
			this.cacheCheck.Size = new System.Drawing.Size(382, 17);
			this.cacheCheck.TabIndex = 6;
			this.cacheCheck.Text = "Cache";
			// 
			// osCheck
			// 
			this.osCheck.Location = new System.Drawing.Point(25, 117);
			this.osCheck.Name = "osCheck";
			this.osCheck.Size = new System.Drawing.Size(382, 17);
			this.osCheck.TabIndex = 8;
			this.osCheck.Text = "OS";
			// 
			// parallelCheck
			// 
			this.parallelCheck.Location = new System.Drawing.Point(25, 140);
			this.parallelCheck.Name = "parallelCheck";
			this.parallelCheck.Size = new System.Drawing.Size(382, 17);
			this.parallelCheck.TabIndex = 10;
			this.parallelCheck.Text = "Parallel Server";
			// 
			// sqlCheck
			// 
			this.sqlCheck.Location = new System.Drawing.Point(25, 163);
			this.sqlCheck.Name = "sqlCheck";
			this.sqlCheck.Size = new System.Drawing.Size(382, 17);
			this.sqlCheck.TabIndex = 12;
			this.sqlCheck.Text = "SQL";
			// 
			// debugCheck
			// 
			this.debugCheck.BackColor = System.Drawing.Color.Transparent;
			this.debugCheck.Location = new System.Drawing.Point(25, 186);
			this.debugCheck.Name = "debugCheck";
			this.debugCheck.Size = new System.Drawing.Size(382, 17);
			this.debugCheck.TabIndex = 14;
			this.debugCheck.Text = "Debug";
			this.debugCheck.UseVisualStyleBackColor = false;
			// 
			// classificationGroup
			// 
			this.classificationGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.classificationGroup.Controls.Add(this.debugLabel);
			this.classificationGroup.Controls.Add(this.sqlLabel);
			this.classificationGroup.Controls.Add(this.parallelLabel);
			this.classificationGroup.Controls.Add(this.osLabel);
			this.classificationGroup.Controls.Add(this.cacheLabel);
			this.classificationGroup.Controls.Add(this.enqueueLabel);
			this.classificationGroup.Controls.Add(this.redoLabel);
			this.classificationGroup.Controls.Add(this.userLabel);
			this.classificationGroup.Controls.Add(this.userCheck);
			this.classificationGroup.Controls.Add(this.redoCheck);
			this.classificationGroup.Controls.Add(this.sqlCheck);
			this.classificationGroup.Controls.Add(this.enqueueCheck);
			this.classificationGroup.Controls.Add(this.parallelCheck);
			this.classificationGroup.Controls.Add(this.cacheCheck);
			this.classificationGroup.Controls.Add(this.osCheck);
			this.classificationGroup.Controls.Add(this.debugCheck);
			this.classificationGroup.Location = new System.Drawing.Point(0, 36);
			this.classificationGroup.Name = "classificationGroup";
			this.classificationGroup.Size = new System.Drawing.Size(427, 219);
			this.classificationGroup.TabIndex = 1;
			this.classificationGroup.TabStop = false;
			this.classificationGroup.Text = "Classifications";
			// 
			// debugLabel
			// 
			this.debugLabel.AutoSize = true;
			this.debugLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.debugLabel.Location = new System.Drawing.Point(140, 187);
			this.debugLabel.Name = "debugLabel";
			this.debugLabel.Size = new System.Drawing.Size(247, 13);
			this.debugLabel.TabIndex = 15;
			this.debugLabel.Text = "Miscellaneous transaction details and internal states";
			// 
			// sqlLabel
			// 
			this.sqlLabel.AutoSize = true;
			this.sqlLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.sqlLabel.Location = new System.Drawing.Point(140, 164);
			this.sqlLabel.Name = "sqlLabel";
			this.sqlLabel.Size = new System.Drawing.Size(146, 13);
			this.sqlLabel.TabIndex = 13;
			this.sqlLabel.Text = "Query performance and tuning";
			// 
			// parallelLabel
			// 
			this.parallelLabel.AutoSize = true;
			this.parallelLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.parallelLabel.Location = new System.Drawing.Point(140, 141);
			this.parallelLabel.Name = "parallelLabel";
			this.parallelLabel.Size = new System.Drawing.Size(143, 13);
			this.parallelLabel.TabIndex = 11;
			this.parallelLabel.Text = "Parallel processing operations";
			// 
			// osLabel
			// 
			this.osLabel.AutoSize = true;
			this.osLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.osLabel.Location = new System.Drawing.Point(140, 118);
			this.osLabel.Name = "osLabel";
			this.osLabel.Size = new System.Drawing.Size(187, 13);
			this.osLabel.TabIndex = 9;
			this.osLabel.Text = "Low level operating system interactions";
			// 
			// cacheLabel
			// 
			this.cacheLabel.AutoSize = true;
			this.cacheLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.cacheLabel.Location = new System.Drawing.Point(140, 95);
			this.cacheLabel.Name = "cacheLabel";
			this.cacheLabel.Size = new System.Drawing.Size(137, 13);
			this.cacheLabel.TabIndex = 7;
			this.cacheLabel.Text = "Library caching performance";
			// 
			// enqueueLabel
			// 
			this.enqueueLabel.AutoSize = true;
			this.enqueueLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.enqueueLabel.Location = new System.Drawing.Point(140, 71);
			this.enqueueLabel.Name = "enqueueLabel";
			this.enqueueLabel.Size = new System.Drawing.Size(144, 13);
			this.enqueueLabel.TabIndex = 5;
			this.enqueueLabel.Text = "Enqueue locks and resources";
			// 
			// redoLabel
			// 
			this.redoLabel.AutoSize = true;
			this.redoLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.redoLabel.Location = new System.Drawing.Point(140, 49);
			this.redoLabel.Name = "redoLabel";
			this.redoLabel.Size = new System.Drawing.Size(179, 13);
			this.redoLabel.TabIndex = 3;
			this.redoLabel.Text = "Redo log transactions and operations";
			// 
			// userLabel
			// 
			this.userLabel.AutoSize = true;
			this.userLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.userLabel.Location = new System.Drawing.Point(140, 25);
			this.userLabel.Name = "userLabel";
			this.userLabel.Size = new System.Drawing.Size(155, 13);
			this.userLabel.TabIndex = 1;
			this.userLabel.Text = "General statistics for this session";
			// 
			// StatisticsSheet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.Controls.Add(this.classificationGroup);
			this.Controls.Add(this.infoLabel);
			this.Name = "StatisticsSheet";
			this.classificationGroup.ResumeLayout(false);
			this.classificationGroup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.CheckBox userCheck;
		private System.Windows.Forms.CheckBox redoCheck;
		private System.Windows.Forms.CheckBox enqueueCheck;
		private System.Windows.Forms.CheckBox cacheCheck;
		private System.Windows.Forms.CheckBox osCheck;
		private System.Windows.Forms.CheckBox parallelCheck;
		private System.Windows.Forms.CheckBox sqlCheck;
		private System.Windows.Forms.CheckBox debugCheck;
		private System.Windows.Forms.GroupBox classificationGroup;
		private System.Windows.Forms.Label debugLabel;
		private System.Windows.Forms.Label sqlLabel;
		private System.Windows.Forms.Label parallelLabel;
		private System.Windows.Forms.Label osLabel;
		private System.Windows.Forms.Label cacheLabel;
		private System.Windows.Forms.Label enqueueLabel;
		private System.Windows.Forms.Label redoLabel;
		private System.Windows.Forms.Label userLabel;
	}
}
