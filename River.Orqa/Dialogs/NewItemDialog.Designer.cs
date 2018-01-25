namespace River.Orqa.Dialogs
{
	partial class NewItemDialog
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Item Templates", System.Windows.Forms.HorizontalAlignment.Left);
			this.templatesView = new System.Windows.Forms.ListView();
			this.fixedColumn = new System.Windows.Forms.ColumnHeader();
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.descriptionBox = new System.Windows.Forms.TextBox();
			this.nameLabel = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// templatesView
			// 
			this.templatesView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.templatesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fixedColumn});
			listViewGroup2.Header = "Item Templates";
			listViewGroup2.Name = "ItemTemplates";
			this.templatesView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
			this.templatesView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.templatesView.HideSelection = false;
			this.templatesView.Location = new System.Drawing.Point(13, 14);
			this.templatesView.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
			this.templatesView.MultiSelect = false;
			this.templatesView.Name = "templatesView";
			this.templatesView.Size = new System.Drawing.Size(418, 143);
			this.templatesView.SmallImageList = this.images;
			this.templatesView.TabIndex = 1;
			this.templatesView.View = System.Windows.Forms.View.SmallIcon;
			this.templatesView.SelectedIndexChanged += new System.EventHandler(this.DoSelectedTemplate);
			// 
			// fixedColumn
			// 
			this.fixedColumn.Text = "Template";
			// 
			// images
			// 
			this.images.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.images.ImageSize = new System.Drawing.Size(16, 16);
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// descriptionBox
			// 
			this.descriptionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionBox.Location = new System.Drawing.Point(13, 160);
			this.descriptionBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 6);
			this.descriptionBox.Name = "descriptionBox";
			this.descriptionBox.ReadOnly = true;
			this.descriptionBox.Size = new System.Drawing.Size(418, 20);
			this.descriptionBox.TabIndex = 2;
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(13, 192);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(34, 13);
			this.nameLabel.TabIndex = 3;
			this.nameLabel.Text = "&Name:";
			// 
			// nameBox
			// 
			this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.nameBox.Location = new System.Drawing.Point(54, 189);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(377, 20);
			this.nameBox.TabIndex = 4;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(356, 217);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.DoCancel);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(275, 217);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.DoAccept);
			// 
			// NewItemDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(444, 253);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.nameBox);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.descriptionBox);
			this.Controls.Add(this.templatesView);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(400, 280);
			this.Name = "NewItemDialog";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add New Item";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView templatesView;
		private System.Windows.Forms.TextBox descriptionBox;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ImageList images;
		private System.Windows.Forms.ColumnHeader fixedColumn;
	}
}