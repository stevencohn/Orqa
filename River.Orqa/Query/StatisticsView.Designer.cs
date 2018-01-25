namespace River.Orqa.Query
{
	partial class StatisticsView
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsView));
			this.statsView = new System.Windows.Forms.ListView();
			this.nameCol = new System.Windows.Forms.ColumnHeader();
			this.deltaCol = new System.Windows.Forms.ColumnHeader();
			this.totalCol = new System.Windows.Forms.ColumnHeader();
			this.classCol = new System.Windows.Forms.ColumnHeader();
			this.statIcons = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// statsView
			// 
			this.statsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameCol,
            this.deltaCol,
            this.totalCol,
            this.classCol});
			this.statsView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.statsView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statsView.FullRowSelect = true;
			this.statsView.GridLines = true;
			this.statsView.Location = new System.Drawing.Point(0, 0);
			this.statsView.MultiSelect = false;
			this.statsView.Name = "statsView";
			this.statsView.Size = new System.Drawing.Size(500, 200);
			this.statsView.SmallImageList = this.statIcons;
			this.statsView.TabIndex = 0;
			this.statsView.View = System.Windows.Forms.View.Details;
			// 
			// nameCol
			// 
			this.nameCol.Text = "Counter";
			this.nameCol.Width = 220;
			// 
			// deltaCol
			// 
			this.deltaCol.Text = "Value";
			this.deltaCol.Width = 90;
			// 
			// totalCol
			// 
			this.totalCol.Text = "Total";
			this.totalCol.Width = 90;
			// 
			// classCol
			// 
			this.classCol.Text = "Class";
			this.classCol.Width = 90;
			// 
			// statIcons
			// 
			this.statIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("statIcons.ImageStream")));
			this.statIcons.Images.SetKeyName(0, "UserStats.gif");
			this.statIcons.Images.SetKeyName(1, "RedoStats.gif");
			this.statIcons.Images.SetKeyName(2, "EnqueueStats.gif");
			this.statIcons.Images.SetKeyName(3, "CacheStats.gif");
			this.statIcons.Images.SetKeyName(4, "ServerStats.ico");
			this.statIcons.Images.SetKeyName(5, "ParallelStats.gif");
			this.statIcons.Images.SetKeyName(6, "SqlStats.gif");
			this.statIcons.Images.SetKeyName(7, "DebugStats.gif");
			// 
			// StatisticsView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.statsView);
			this.Name = "StatisticsView";
			this.Size = new System.Drawing.Size(500, 200);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView statsView;
		private System.Windows.Forms.ColumnHeader nameCol;
		private System.Windows.Forms.ColumnHeader deltaCol;
		private System.Windows.Forms.ColumnHeader totalCol;
		private System.Windows.Forms.ColumnHeader classCol;
		private System.Windows.Forms.ImageList statIcons;
	}
}
