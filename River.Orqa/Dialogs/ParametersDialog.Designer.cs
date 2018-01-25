namespace River.Orqa.Dialogs
{
	partial class ParametersDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParametersDialog));
			this.titlePanel = new System.Windows.Forms.Panel();
			this.titleNameLabel = new System.Windows.Forms.Label();
			this.titleLabel = new System.Windows.Forms.Label();
			this.instructionLabel = new System.Windows.Forms.Label();
			this.directionImages = new System.Windows.Forms.ImageList(this.components);
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.parameterGrid = new River.Orqa.Controls.RiverDataGridView();
			this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.errorPicture = new System.Windows.Forms.PictureBox();
			this.errorLabel = new System.Windows.Forms.Label();
			this.errorPanel = new System.Windows.Forms.Panel();
			this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.titlePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.parameterGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorPicture)).BeginInit();
			this.errorPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// titlePanel
			// 
			this.titlePanel.BackColor = System.Drawing.Color.White;
			this.titlePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.titlePanel.Controls.Add(this.titleNameLabel);
			this.titlePanel.Controls.Add(this.titleLabel);
			resources.ApplyResources(this.titlePanel, "titlePanel");
			this.titlePanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.titlePanel.Name = "titlePanel";
			this.titlePanel.Padding = new System.Windows.Forms.Padding(10);
			// 
			// titleNameLabel
			// 
			resources.ApplyResources(this.titleNameLabel, "titleNameLabel");
			this.titleNameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.titleNameLabel.Name = "titleNameLabel";
			// 
			// titleLabel
			// 
			resources.ApplyResources(this.titleLabel, "titleLabel");
			this.titleLabel.Name = "titleLabel";
			// 
			// instructionLabel
			// 
			resources.ApplyResources(this.instructionLabel, "instructionLabel");
			this.instructionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.instructionLabel.Name = "instructionLabel";
			// 
			// directionImages
			// 
			this.directionImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("directionImages.ImageStream")));
			this.directionImages.Images.SetKeyName(0, "ParameterIn.gif");
			this.directionImages.Images.SetKeyName(1, "ParameterIn.gif");
			this.directionImages.Images.SetKeyName(2, "ParameterOut.gif");
			this.directionImages.Images.SetKeyName(3, "ReturnValue.gif");
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Click += new System.EventHandler(this.DoCancel);
			// 
			// okButton
			// 
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Name = "okButton";
			this.okButton.Click += new System.EventHandler(this.DoAccept);
			// 
			// parameterGrid
			// 
			this.parameterGrid.AllowUserToAddRows = false;
			this.parameterGrid.AllowUserToDeleteRows = false;
			this.parameterGrid.AllowUserToOrderColumns = true;
			this.parameterGrid.AllowUserToResizeRows = false;
			resources.ApplyResources(this.parameterGrid, "parameterGrid");
			this.parameterGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.parameterGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.parameterGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.parameterGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.parameterGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.parameterGrid.Columns.Add(this.dataGridViewImageColumn1);
			this.parameterGrid.Columns.Add(this.dataGridViewTextBoxColumn1);
			this.parameterGrid.Columns.Add(this.dataGridViewTextBoxColumn2);
			this.parameterGrid.Columns.Add(this.dataGridViewTextBoxColumn3);
			this.parameterGrid.Columns.Add(this.dataGridViewCheckBoxColumn1);
			this.parameterGrid.DefaultSelectedColumnIndex = 3;
			this.parameterGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.parameterGrid.GridColor = System.Drawing.SystemColors.ControlLight;
			this.parameterGrid.MultiSelect = false;
			this.parameterGrid.Name = "parameterGrid";
			this.parameterGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.parameterGrid.RowHeadersVisible = false;
			this.parameterGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.parameterGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DoCellFormatting);
			this.parameterGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DoDataError);
			// 
			// dataGridViewImageColumn1
			// 
			this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewImageColumn1.DataPropertyName = "Direction";
			this.dataGridViewImageColumn1.FillWeight = 1F;
			this.dataGridViewImageColumn1.Frozen = true;
			resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
			this.dataGridViewImageColumn1.Name = "directionCol";
			this.dataGridViewImageColumn1.ReadOnly = true;
			this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn1.FillWeight = 32F;
			resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "nameCol";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "DataType";
			this.dataGridViewTextBoxColumn2.FillWeight = 24F;
			resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataTypeCol";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "Value";
			this.dataGridViewTextBoxColumn3.FillWeight = 50F;
			resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "valueCol";
			// 
			// dataGridViewCheckBoxColumn1
			// 
			this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewCheckBoxColumn1.FillWeight = 22.25434F;
			resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "nullCol";
			this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// errorPicture
			// 
			resources.ApplyResources(this.errorPicture, "errorPicture");
			this.errorPicture.Name = "errorPicture";
			this.errorPicture.TabStop = false;
			// 
			// errorLabel
			// 
			resources.ApplyResources(this.errorLabel, "errorLabel");
			this.errorLabel.Name = "errorLabel";
			// 
			// errorPanel
			// 
			resources.ApplyResources(this.errorPanel, "errorPanel");
			this.errorPanel.Controls.Add(this.errorLabel);
			this.errorPanel.Controls.Add(this.errorPicture);
			this.errorPanel.Name = "errorPanel";
			// 
			// ParametersDialog
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.errorPanel);
			this.Controls.Add(this.instructionLabel);
			this.Controls.Add(this.parameterGrid);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.titlePanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ParametersDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DoFormClosing);
			this.titlePanel.ResumeLayout(false);
			this.titlePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.parameterGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorPicture)).EndInit();
			this.errorPanel.ResumeLayout(false);
			this.errorPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel titlePanel;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label titleNameLabel;
		private System.Windows.Forms.Label instructionLabel;
		private System.Windows.Forms.ImageList directionImages;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private River.Orqa.Controls.RiverDataGridView parameterGrid;
		private System.Windows.Forms.PictureBox errorPicture;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.Panel errorPanel;
		private System.Windows.Forms.BindingSource bindingSource;
		private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
	}
}