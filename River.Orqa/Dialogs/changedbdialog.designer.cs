namespace River.Orqa.Dialogs
{
	partial class ChangeDbDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeDbDialog));
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.schemaList = new River.Orqa.Controls.RiverListView();
			this.schemaHeader = new System.Windows.Forms.ColumnHeader();
			this.versionHeader = new System.Windows.Forms.ColumnHeader();
			this.dsnHeader = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// okButton
			// 
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.okButton.Click += new System.EventHandler(this.DoOK);
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			// 
			// schemaList
			// 
			resources.ApplyResources(this.schemaList, "schemaList");
			this.schemaList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.schemaHeader,
            this.versionHeader,
            this.dsnHeader});
			this.schemaList.FullRowSelect = true;
			this.schemaList.GridLines = true;
			this.schemaList.MultiSelect = false;
			this.schemaList.Name = "schemaList";
			this.schemaList.View = System.Windows.Forms.View.Details;
			// 
			// schemaHeader
			// 
			resources.ApplyResources(this.schemaHeader, "schemaHeader");
			// 
			// versionHeader
			// 
			resources.ApplyResources(this.versionHeader, "versionHeader");
			// 
			// dsnHeader
			// 
			resources.ApplyResources(this.dsnHeader, "dsnHeader");
			// 
			// ChangeDbDialog
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.schemaList);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangeDbDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Resize += new System.EventHandler(this.DoResize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private River.Orqa.Controls.RiverListView schemaList;
		private System.Windows.Forms.ColumnHeader schemaHeader;
		private System.Windows.Forms.ColumnHeader versionHeader;
		private System.Windows.Forms.ColumnHeader dsnHeader;
	}
}