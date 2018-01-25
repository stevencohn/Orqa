namespace River.Orqa.Options
{
	partial class ConnectionsSheet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionsSheet));
			this.parseModeLabel = new System.Windows.Forms.Label();
			this.parseModeBox = new System.Windows.Forms.ComboBox();
			this.utilProcedureBox = new System.Windows.Forms.TextBox();
			this.planTableBox = new System.Windows.Forms.TextBox();
			this.planTableLabel = new System.Windows.Forms.Label();
			this.utilProcedureLabel = new System.Windows.Forms.Label();
			this.loginTimeoutLabel = new System.Windows.Forms.Label();
			this.queryTimeoutLabel = new System.Windows.Forms.Label();
			this.loginTimeoutBox = new System.Windows.Forms.NumericUpDown();
			this.queryTimeoutBox = new System.Windows.Forms.NumericUpDown();
			this.planGroup = new System.Windows.Forms.GroupBox();
			this.planInstructions = new System.Windows.Forms.Label();
			this.wrapperGroup = new System.Windows.Forms.GroupBox();
			this.wrapperInstructions = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.loginTimeoutBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.queryTimeoutBox)).BeginInit();
			this.planGroup.SuspendLayout();
			this.wrapperGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// parseModeLabel
			// 
			resources.ApplyResources(this.parseModeLabel, "parseModeLabel");
			this.parseModeLabel.Name = "parseModeLabel";
			// 
			// parseModeBox
			// 
			this.parseModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.parseModeBox.FormattingEnabled = true;
			resources.ApplyResources(this.parseModeBox, "parseModeBox");
			this.parseModeBox.Name = "parseModeBox";
			// 
			// utilProcedureBox
			// 
			resources.ApplyResources(this.utilProcedureBox, "utilProcedureBox");
			this.utilProcedureBox.Name = "utilProcedureBox";
			// 
			// planTableBox
			// 
			resources.ApplyResources(this.planTableBox, "planTableBox");
			this.planTableBox.Name = "planTableBox";
			// 
			// planTableLabel
			// 
			resources.ApplyResources(this.planTableLabel, "planTableLabel");
			this.planTableLabel.Name = "planTableLabel";
			// 
			// utilProcedureLabel
			// 
			resources.ApplyResources(this.utilProcedureLabel, "utilProcedureLabel");
			this.utilProcedureLabel.Name = "utilProcedureLabel";
			// 
			// loginTimeoutLabel
			// 
			resources.ApplyResources(this.loginTimeoutLabel, "loginTimeoutLabel");
			this.loginTimeoutLabel.Name = "loginTimeoutLabel";
			// 
			// queryTimeoutLabel
			// 
			resources.ApplyResources(this.queryTimeoutLabel, "queryTimeoutLabel");
			this.queryTimeoutLabel.Name = "queryTimeoutLabel";
			// 
			// loginTimeoutBox
			// 
			resources.ApplyResources(this.loginTimeoutBox, "loginTimeoutBox");
			this.loginTimeoutBox.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.loginTimeoutBox.Name = "loginTimeoutBox";
			this.loginTimeoutBox.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			// 
			// queryTimeoutBox
			// 
			resources.ApplyResources(this.queryTimeoutBox, "queryTimeoutBox");
			this.queryTimeoutBox.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.queryTimeoutBox.Name = "queryTimeoutBox";
			this.queryTimeoutBox.Value = new decimal(new int[] {
            240,
            0,
            0,
            0});
			// 
			// planGroup
			// 
			resources.ApplyResources(this.planGroup, "planGroup");
			this.planGroup.Controls.Add(this.planInstructions);
			this.planGroup.Controls.Add(this.planTableLabel);
			this.planGroup.Controls.Add(this.planTableBox);
			this.planGroup.Name = "planGroup";
			this.planGroup.Padding = new System.Windows.Forms.Padding(10, 5, 10, 3);
			this.planGroup.TabStop = false;
			// 
			// planInstructions
			// 
			this.planInstructions.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.planInstructions, "planInstructions");
			this.planInstructions.Name = "planInstructions";
			// 
			// wrapperGroup
			// 
			resources.ApplyResources(this.wrapperGroup, "wrapperGroup");
			this.wrapperGroup.Controls.Add(this.wrapperInstructions);
			this.wrapperGroup.Controls.Add(this.utilProcedureLabel);
			this.wrapperGroup.Controls.Add(this.utilProcedureBox);
			this.wrapperGroup.Name = "wrapperGroup";
			this.wrapperGroup.Padding = new System.Windows.Forms.Padding(10, 5, 10, 3);
			this.wrapperGroup.TabStop = false;
			// 
			// wrapperInstructions
			// 
			this.wrapperInstructions.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.wrapperInstructions, "wrapperInstructions");
			this.wrapperInstructions.Name = "wrapperInstructions";
			// 
			// ConnectionsSheet
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.Controls.Add(this.wrapperGroup);
			this.Controls.Add(this.planGroup);
			this.Controls.Add(this.queryTimeoutLabel);
			this.Controls.Add(this.queryTimeoutBox);
			this.Controls.Add(this.loginTimeoutLabel);
			this.Controls.Add(this.loginTimeoutBox);
			this.Controls.Add(this.parseModeBox);
			this.Controls.Add(this.parseModeLabel);
			this.Name = "ConnectionsSheet";
			((System.ComponentModel.ISupportInitialize)(this.loginTimeoutBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.queryTimeoutBox)).EndInit();
			this.planGroup.ResumeLayout(false);
			this.planGroup.PerformLayout();
			this.wrapperGroup.ResumeLayout(false);
			this.wrapperGroup.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label parseModeLabel;
		private System.Windows.Forms.ComboBox parseModeBox;
		private System.Windows.Forms.TextBox utilProcedureBox;
		private System.Windows.Forms.TextBox planTableBox;
		private System.Windows.Forms.Label planTableLabel;
		private System.Windows.Forms.Label utilProcedureLabel;
		private System.Windows.Forms.Label loginTimeoutLabel;
		private System.Windows.Forms.Label queryTimeoutLabel;
		private System.Windows.Forms.NumericUpDown loginTimeoutBox;
		private System.Windows.Forms.NumericUpDown queryTimeoutBox;
		private System.Windows.Forms.GroupBox planGroup;
		private System.Windows.Forms.GroupBox wrapperGroup;
		private System.Windows.Forms.Label planInstructions;
		private System.Windows.Forms.Label wrapperInstructions;
	}
}
