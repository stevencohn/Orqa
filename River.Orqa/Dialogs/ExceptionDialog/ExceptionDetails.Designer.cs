namespace River.Orqa.Dialogs
{
	partial class ExceptionDetails
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
			this.titleBox = new System.Windows.Forms.TextBox();
			this.titleLabel = new System.Windows.Forms.Label();
			this.abortButton = new System.Windows.Forms.Button();
			this.tabset = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.msgBox = new System.Windows.Forms.TextBox();
			this.sourceBox = new System.Windows.Forms.TextBox();
			this.targetBox = new System.Windows.Forms.TextBox();
			this.helpBox = new System.Windows.Forms.TextBox();
			this.helpLabel = new System.Windows.Forms.Label();
			this.targetLabel = new System.Windows.Forms.Label();
			this.sourceLabel = new System.Windows.Forms.Label();
			this.messageLabel = new System.Windows.Forms.Label();
			this.tabStack = new System.Windows.Forms.TabPage();
			this.stackBox = new System.Windows.Forms.TextBox();
			this.tabInner = new System.Windows.Forms.TabPage();
			this.innerTree = new System.Windows.Forms.TreeView();
			this.tabOther = new System.Windows.Forms.TabPage();
			this.otherList = new System.Windows.Forms.ListView();
			this.othNameCol = new System.Windows.Forms.ColumnHeader();
			this.othDescriptionCol = new System.Windows.Forms.ColumnHeader();
			this.reportButton = new System.Windows.Forms.Button();
			this.tabset.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.tabStack.SuspendLayout();
			this.tabInner.SuspendLayout();
			this.tabOther.SuspendLayout();
			this.SuspendLayout();
			// 
			// titleBox
			// 
			this.titleBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.titleBox.ForeColor = System.Drawing.Color.Red;
			this.titleBox.Location = new System.Drawing.Point(201, 9);
			this.titleBox.Name = "titleBox";
			this.titleBox.ReadOnly = true;
			this.titleBox.Size = new System.Drawing.Size(359, 20);
			this.titleBox.TabIndex = 6;
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.Location = new System.Drawing.Point(13, 12);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(183, 13);
			this.titleLabel.TabIndex = 5;
			this.titleLabel.Text = "The following exception has occurred:";
			// 
			// abortButton
			// 
			this.abortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.abortButton.Location = new System.Drawing.Point(485, 289);
			this.abortButton.Name = "abortButton";
			this.abortButton.Size = new System.Drawing.Size(75, 23);
			this.abortButton.TabIndex = 7;
			this.abortButton.Text = "Abort";
			this.abortButton.Click += new System.EventHandler(this.SendAbort);
			// 
			// tabset
			// 
			this.tabset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabset.Controls.Add(this.tabGeneral);
			this.tabset.Controls.Add(this.tabStack);
			this.tabset.Controls.Add(this.tabInner);
			this.tabset.Controls.Add(this.tabOther);
			this.tabset.Location = new System.Drawing.Point(14, 36);
			this.tabset.Name = "tabset";
			this.tabset.SelectedIndex = 0;
			this.tabset.Size = new System.Drawing.Size(546, 247);
			this.tabset.TabIndex = 4;
			this.tabset.SelectedIndexChanged += new System.EventHandler(this.ChangeTab);
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.msgBox);
			this.tabGeneral.Controls.Add(this.sourceBox);
			this.tabGeneral.Controls.Add(this.targetBox);
			this.tabGeneral.Controls.Add(this.helpBox);
			this.tabGeneral.Controls.Add(this.helpLabel);
			this.tabGeneral.Controls.Add(this.targetLabel);
			this.tabGeneral.Controls.Add(this.sourceLabel);
			this.tabGeneral.Controls.Add(this.messageLabel);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Padding = new System.Windows.Forms.Padding(8);
			this.tabGeneral.Size = new System.Drawing.Size(538, 221);
			this.tabGeneral.TabIndex = 0;
			this.tabGeneral.Text = "General Information";
			// 
			// msgBox
			// 
			this.msgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.msgBox.BackColor = System.Drawing.Color.White;
			this.msgBox.Location = new System.Drawing.Point(92, 11);
			this.msgBox.Multiline = true;
			this.msgBox.Name = "msgBox";
			this.msgBox.ReadOnly = true;
			this.msgBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.msgBox.Size = new System.Drawing.Size(435, 121);
			this.msgBox.TabIndex = 7;
			this.msgBox.WordWrap = false;
			// 
			// sourceBox
			// 
			this.sourceBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.sourceBox.Location = new System.Drawing.Point(92, 138);
			this.sourceBox.Name = "sourceBox";
			this.sourceBox.ReadOnly = true;
			this.sourceBox.Size = new System.Drawing.Size(435, 20);
			this.sourceBox.TabIndex = 6;
			// 
			// targetBox
			// 
			this.targetBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.targetBox.Location = new System.Drawing.Point(92, 164);
			this.targetBox.Name = "targetBox";
			this.targetBox.ReadOnly = true;
			this.targetBox.Size = new System.Drawing.Size(435, 20);
			this.targetBox.TabIndex = 5;
			// 
			// helpBox
			// 
			this.helpBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.helpBox.Location = new System.Drawing.Point(92, 190);
			this.helpBox.Name = "helpBox";
			this.helpBox.ReadOnly = true;
			this.helpBox.Size = new System.Drawing.Size(435, 20);
			this.helpBox.TabIndex = 4;
			// 
			// helpLabel
			// 
			this.helpLabel.AutoSize = true;
			this.helpLabel.Location = new System.Drawing.Point(39, 193);
			this.helpLabel.Name = "helpLabel";
			this.helpLabel.Size = new System.Drawing.Size(47, 13);
			this.helpLabel.TabIndex = 3;
			this.helpLabel.Text = "Help link:";
			// 
			// targetLabel
			// 
			this.targetLabel.AutoSize = true;
			this.targetLabel.Location = new System.Drawing.Point(11, 167);
			this.targetLabel.Name = "targetLabel";
			this.targetLabel.Size = new System.Drawing.Size(75, 13);
			this.targetLabel.TabIndex = 2;
			this.targetLabel.Text = "Target method:";
			// 
			// sourceLabel
			// 
			this.sourceLabel.AutoSize = true;
			this.sourceLabel.Location = new System.Drawing.Point(46, 141);
			this.sourceLabel.Name = "sourceLabel";
			this.sourceLabel.Size = new System.Drawing.Size(40, 13);
			this.sourceLabel.TabIndex = 1;
			this.sourceLabel.Text = "Source:";
			// 
			// messageLabel
			// 
			this.messageLabel.AutoSize = true;
			this.messageLabel.Location = new System.Drawing.Point(37, 14);
			this.messageLabel.Name = "messageLabel";
			this.messageLabel.Size = new System.Drawing.Size(49, 13);
			this.messageLabel.TabIndex = 0;
			this.messageLabel.Text = "Message:";
			// 
			// tabStack
			// 
			this.tabStack.Controls.Add(this.stackBox);
			this.tabStack.Location = new System.Drawing.Point(4, 22);
			this.tabStack.Name = "tabStack";
			this.tabStack.Padding = new System.Windows.Forms.Padding(8);
			this.tabStack.Size = new System.Drawing.Size(538, 221);
			this.tabStack.TabIndex = 1;
			this.tabStack.Text = "Stack Trace";
			// 
			// stackBox
			// 
			this.stackBox.BackColor = System.Drawing.SystemColors.Window;
			this.stackBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stackBox.Location = new System.Drawing.Point(8, 8);
			this.stackBox.Multiline = true;
			this.stackBox.Name = "stackBox";
			this.stackBox.ReadOnly = true;
			this.stackBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.stackBox.Size = new System.Drawing.Size(522, 205);
			this.stackBox.TabIndex = 0;
			this.stackBox.WordWrap = false;
			// 
			// tabInner
			// 
			this.tabInner.Controls.Add(this.innerTree);
			this.tabInner.Location = new System.Drawing.Point(4, 22);
			this.tabInner.Name = "tabInner";
			this.tabInner.Padding = new System.Windows.Forms.Padding(8);
			this.tabInner.Size = new System.Drawing.Size(538, 221);
			this.tabInner.TabIndex = 2;
			this.tabInner.Text = "Inner Exception";
			// 
			// innerTree
			// 
			this.innerTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.innerTree.Location = new System.Drawing.Point(8, 8);
			this.innerTree.Name = "innerTree";
			this.innerTree.Size = new System.Drawing.Size(522, 205);
			this.innerTree.TabIndex = 0;
			// 
			// tabOther
			// 
			this.tabOther.Controls.Add(this.otherList);
			this.tabOther.Location = new System.Drawing.Point(4, 22);
			this.tabOther.Name = "tabOther";
			this.tabOther.Padding = new System.Windows.Forms.Padding(8);
			this.tabOther.Size = new System.Drawing.Size(538, 221);
			this.tabOther.TabIndex = 3;
			this.tabOther.Text = "Other Information";
			// 
			// otherList
			// 
			this.otherList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.othNameCol,
            this.othDescriptionCol});
			this.otherList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.otherList.Location = new System.Drawing.Point(8, 8);
			this.otherList.Name = "otherList";
			this.otherList.Size = new System.Drawing.Size(522, 205);
			this.otherList.TabIndex = 0;
			this.otherList.View = System.Windows.Forms.View.Details;
			this.otherList.Resize += new System.EventHandler(this.TrackResize);
			// 
			// othNameCol
			// 
			this.othNameCol.Text = "Name";
			this.othNameCol.Width = 150;
			// 
			// othDescriptionCol
			// 
			this.othDescriptionCol.Text = "Description";
			this.othDescriptionCol.Width = 337;
			// 
			// reportButton
			// 
			this.reportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.reportButton.Location = new System.Drawing.Point(372, 289);
			this.reportButton.Name = "reportButton";
			this.reportButton.Size = new System.Drawing.Size(107, 23);
			this.reportButton.TabIndex = 8;
			this.reportButton.Text = "Send Report...";
			this.reportButton.Click += new System.EventHandler(this.SendReport);
			// 
			// ExceptionDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.titleBox);
			this.Controls.Add(this.titleLabel);
			this.Controls.Add(this.abortButton);
			this.Controls.Add(this.tabset);
			this.Controls.Add(this.reportButton);
			this.Name = "ExceptionDetails";
			this.Size = new System.Drawing.Size(572, 321);
			this.tabset.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.tabGeneral.PerformLayout();
			this.tabStack.ResumeLayout(false);
			this.tabStack.PerformLayout();
			this.tabInner.ResumeLayout(false);
			this.tabOther.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox titleBox;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Button abortButton;
		private System.Windows.Forms.TabControl tabset;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TextBox msgBox;
		private System.Windows.Forms.TextBox sourceBox;
		private System.Windows.Forms.TextBox targetBox;
		private System.Windows.Forms.TextBox helpBox;
		private System.Windows.Forms.Label helpLabel;
		private System.Windows.Forms.Label targetLabel;
		private System.Windows.Forms.Label sourceLabel;
		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.TabPage tabStack;
		private System.Windows.Forms.TabPage tabInner;
		private System.Windows.Forms.TreeView innerTree;
		private System.Windows.Forms.TabPage tabOther;
		private System.Windows.Forms.ListView otherList;
		private System.Windows.Forms.ColumnHeader othNameCol;
		private System.Windows.Forms.ColumnHeader othDescriptionCol;
		private System.Windows.Forms.Button reportButton;
		private System.Windows.Forms.TextBox stackBox;
	}
}
