//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Displays and manages a tree view used to present SQL templates.  These templates
// can be dragged/dropped onto a Query window or double-clicked to copy their contents
// into the current Query window.  They can also be dragged onto the desktop or similar
// targets.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.IO;
	using System.Windows.Forms;
	using River.Orqa.Dialogs;
	using River.Orqa.Options;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class TemplateTree
	//********************************************************************************************

	/// <summary>
	/// Displays and manages a tree view used to present SQL templates.  These templates
	/// can be dragged/dropped onto a Query window or double-clicked to copy their contents
	/// into the current Query window.  They can also be dragged onto the desktop or similar
	/// targets.
	/// </summary>

	internal partial class TemplateTree : UserControl
	{
		private const int OpenFolderIcon = 0;
		private const int ClosedFolderIcon = 1;
		private const int TemplateIcon = 2;
		private const int ReportIcon = 3;
		private const int QueryIcon = 4;

		private bool isMouseOverNode = false;
		private string templatePath;
		private string templateExt;
		private string reportExt;
		private string queryExt;


		// Events

		public event TemplateSelectionHandler TemplateSelected;


		//========================================================================================
		// Lifecycle
		//========================================================================================

		/// <summary>
		/// Initializes a new template tree controle
		/// </summary>

		public TemplateTree ()
		{
			InitializeComponent();
		}


		/// <summary>
		/// Prepopulates the template tree when the control is loaded.  Doing it here
		/// prevents the horizontal scrollbar problem.
		/// </summary>
		/// <param name="e">Event arguments</param>

		protected override void OnLoad (EventArgs e)
		{
			if (this.DesignMode)
				return;

			templateExt = UserOptions.GetString("general/templateExtension");
			queryExt = UserOptions.GetString("general/queryExtension");
			reportExt = UserOptions.GetString("general/reportExtension");
			templatePath = UserOptions.GetString("general/templatePath");

			var translator = new Translator("Browser");
			string rootName = translator.GetString("TemplateRoot");
			var root = new TreeNode(rootName, OpenFolderIcon, OpenFolderIcon);
			root.ContextMenuStrip = contextMenu;
			root.Tag = templatePath;
			root.Expand();
			tree.Nodes.Insert(0, root);

			RefreshTemplates();
		}


		#region Setup

		internal void RefreshTemplates ()
		{
			var root = tree.Nodes[0];
			root.Nodes.Clear();

			PopulateTemplates(root.Nodes, templatePath);

			root.Expand();
			for (int i = 0; i < root.Nodes.Count; i++)
			{
				root.Nodes[i].Expand();
			}
		}


		private void PopulateTemplates (TreeNodeCollection nodes, string path)
		{
			TreeNode node;
			string[] dirnams;

			try
			{
				dirnams = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
			}
			catch (Exception)
			{
				return;
			}

			foreach (string dirnam in dirnams)
			{
				string name = Path.GetFileNameWithoutExtension(dirnam);
				if (!name.ToLower().Equals("hidden"))
				{
					node = new TreeNode(
						Path.GetFileNameWithoutExtension(dirnam),
						ClosedFolderIcon, ClosedFolderIcon);

					node.ContextMenuStrip = folderContextMenu;
					node.Tag = dirnam;
					nodes.Add(node);

					PopulateTemplates(node.Nodes, dirnam);
				}
			}

			string[] filnams = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
			var extensions = new StringCollection();

			// template=2, report=3, query=4
			extensions.Add(templateExt);
			extensions.Add(reportExt);
			extensions.Add(queryExt);

			foreach (string filnam in filnams)
			{
				string ext = Path.GetExtension(filnam).Substring(1);
				if (extensions.Contains(ext))
				{
					int iconIndex;

					if (ext.Equals(reportExt)) iconIndex = ReportIcon;
					else if (ext.Equals(queryExt)) iconIndex = QueryIcon;
					else iconIndex = TemplateIcon;

					node = new TreeNode(
						Path.GetFileNameWithoutExtension(filnam),
						iconIndex, iconIndex);

					node.ContextMenuStrip = templateContextMenu;
					node.Tag = filnam;
					node.ToolTipText = filnam;
					nodes.Add(node);
				}
			}
		}


		/// <summary>
		/// Updates the control to reflect any recent changes to the User Options.
		/// </summary>

		public void SetOptions ()
		{
			queryExt = UserOptions.GetString("general/queryExtension");
			reportExt = UserOptions.GetString("general/reportExtension");
			templateExt = UserOptions.GetString("general/templateExtension");
			templatePath = UserOptions.GetString("general/templatePath");

			// may be zero if OnLoad has not yet run (tree hasn't yet been displayed)
			if (tree.Nodes.Count > 0)
			{
				tree.Nodes[0].Tag = templatePath;
				RefreshTemplates();
			}
		}

		#endregion Setup


		//========================================================================================
		// Node actions, drag/drop, double-click
		//========================================================================================

		#region Node actions

		private void DoMouseDown (object sender, MouseEventArgs e)
		{
			TreeNode node = tree.GetNodeAt(e.X, e.Y);
			isMouseOverNode = ((node != null) && (node.ImageIndex > ClosedFolderIcon));
			if (!isMouseOverNode)
				return;

			if (e.Button == MouseButtons.Left)
			{
				tree.SelectedNode = node;

				if (e.Clicks == 1)
				{
					tree.DoDragDrop(LoadTemplate((string)node.Tag), DragDropEffects.Copy);
				}
				else
				{
					// double-click!

					string ext = Path.GetExtension((string)node.Tag).Substring(1);
					if (ext.Equals(reportExt))
					{
						OpenTemplate(node, e);
					}
					else
					{
						CopyTemplate(node, e);
					}
				}
			}
		}


		private void DoMouseUp (object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode node = tree.GetNodeAt(e.X, e.Y);
				if ((node != null) && (node.Tag != null))
				{
					tree.SelectedNode = node;
				}
			}
		}


		private void CopyTemplate (object sender, EventArgs e)
		{
			if (TemplateSelected != null)
			{
				TemplateSelected((string)tree.SelectedNode.Tag, false, false);
				//TemplateSelected(LoadTemplate((string)tree.SelectedNode.Tag), false, false);
			}
		}


		private void EditTemplate (object sender, EventArgs e)
		{
			if (TemplateSelected != null)
			{
				TemplateSelected((string)tree.SelectedNode.Tag, true, false);
				//TemplateSelected(LoadTemplate((string)tree.SelectedNode.Tag), true, false);
			}
		}


		private string LoadTemplate (string filnam)
		{
			string text = null;
			using (var reader = new StreamReader(filnam))
			{
				text = reader.ReadToEnd();

				// fix \n\r line endings
				text = text.Replace("\r", String.Empty);
				reader.Close();
			}

			return text;
		}


		private void OpenTemplate (object sender, EventArgs e)
		{
			if (TemplateSelected != null)
			{
				TemplateSelected((string)tree.SelectedNode.Tag, true, true);
				//TemplateSelected(LoadTemplate((string)tree.SelectedNode.Tag), true, true);
			}
		}


		private void DoBeforeExpand (object sender, TreeViewCancelEventArgs e)
		{
			e.Node.ImageIndex = e.Node.SelectedImageIndex = OpenFolderIcon;
		}


		private void DoBeforeCollapse (object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Parent == null)
			{
				e.Cancel = true;
				return;
			}

			e.Node.ImageIndex = e.Node.SelectedImageIndex = ClosedFolderIcon;
		}

		#endregion Node actions


		//========================================================================================
		// Menu handlers
		//========================================================================================

		private void DoOpeningContextMenu (object sender, CancelEventArgs e)
		{
			if (!isMouseOverNode)
			{
				e.Cancel = true;
				return;
			}

			string ext = Path.GetExtension(tree.SelectedNode.Tag as String);
			ext = ext.Substring(1);

			// .tpl files are not executable
			itemReportItem.Enabled =
				!ext.Equals(templateExt, StringComparison.InvariantCultureIgnoreCase);
		}


		private void DoAddNewItem (object sender, EventArgs e)
		{
			var dialog = new Dialogs.NewItemDialog();

			var translator = new Translator("Dialogs.NewItems");
			dialog.SetTemplateConfiguration(translator.GetString("SqlTemplateItems"));

			DialogResult result = dialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				TreeNode parent = tree.SelectedNode;
				string path = (string)parent.Tag;

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				string filnam = Path.Combine(path, dialog.FileName);

				using (var writer = new StreamWriter(filnam, false))
				{
					writer.WriteLine(dialog.TemplateContent);
					writer.Close();
				}

				// template=2, report=3, query=4
				string ext = Path.GetExtension(filnam).Substring(1);
				int iconIndex = TemplateIcon;
				if (ext.Equals(reportExt))
				{
					iconIndex = ReportIcon;
				}
				else if (ext.Equals(queryExt))
				{
					iconIndex = QueryIcon;
				}

				var node = new TreeNode(
					Path.GetFileNameWithoutExtension(filnam),
					iconIndex, iconIndex);

				node.Tag = filnam;

				// TODO: alphabetize
				parent.Nodes.Add(node);
				parent.Expand();
			}
		}

	
		private void DoAddNewFolder (object sender, EventArgs e)
		{
			using (var dialog = new Dialogs.NewFolderDialog())
			{
				DialogResult result = dialog.ShowDialog();

				if ((result == DialogResult.OK) && (dialog.FolderName != null))
				{
					TreeNode parent = tree.SelectedNode;
					string path = (string)parent.Tag;

					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					string dirpath = Path.Combine(path, dialog.FolderName);

					var node = new TreeNode(
						Path.GetFileNameWithoutExtension(dirpath),
						ClosedFolderIcon, ClosedFolderIcon);

					node.ContextMenuStrip = folderContextMenu;
					node.Tag = dirpath;

					// TODO: alphabetize
					parent.Nodes.Add(node);

					parent.Expand();
				}
			}
		}


		private void DoDeleteItem (object sender, EventArgs e)
		{
			var translator = new Translator("Browser");
			TreeNode node = tree.SelectedNode;

			DialogResult result = MessageBox.Show(
				String.Format(translator.GetString("ConfirmDeleteMessage"), node.Text),
				translator.GetString("ConfirmDeleteTitle"),
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.Yes)
			{
				string path = (string)node.Tag;
				if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
				{
					if (Directory.Exists(path))
					{
						try
						{
							Directory.Delete(path, true);
							node.Remove();
						}
						catch (Exception exc)
						{
							ExceptionDialog.ShowException(exc);
						}
					}
				}
				else
				{
					if (File.Exists(path))
					{
						try
						{
							File.Delete(path);
							node.Remove();
						}
						catch (Exception exc)
						{
							ExceptionDialog.ShowException(exc);
						}
					}
				}
			}
		}


		private void DoOptions (object sender, EventArgs e)
		{
			var dialog = new OptionsDialog();
			dialog.ActiveSheet = "/OrqaOptions/general";

			DialogResult result = dialog.ShowDialog(this);

			if (result == DialogResult.OK)
			{
				SetOptions();
			}
		}


		private void DoRefreshAll (object sender, EventArgs e)
		{
			RefreshTemplates();
		}


		private void DoRefreshTemplateFolder (object sender, EventArgs e)
		{
			TreeNode node = tree.SelectedNode;

			RefreshFolder(node);
			node.Expand();

			// expand second level only for root node
			if (node.Parent == null)
			{
				for (int i = 0; i < node.Nodes.Count; i++)
				{
					node.Nodes[i].Expand();
				}
			}
		}


		private void RefreshFolder (TreeNode node)
		{
			bool expanded = node.IsExpanded;
			node.Nodes.Clear();
			PopulateTemplates(node.Nodes, (string)node.Tag);

			if (expanded)
			{
				node.Expand();
			}
		}


		private void DoRenameItem (object sender, EventArgs e)
		{
			TreeNode node = tree.SelectedNode;
			node.BeginEdit();
		}


		private void tree_AfterLabelEdit (object sender, NodeLabelEditEventArgs e)
		{
			if (e.Label == null)
			{
				e.CancelEdit = true;
				return;
			}

			string label = e.Label.Trim();
			if (String.IsNullOrEmpty(label))
			{
				e.CancelEdit = true;
				return;
			}

			TreeNode node = e.Node;
			string path = node.Tag as String;

			if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
			{
				if (!Path.GetFileNameWithoutExtension(path).Equals(
					label, StringComparison.InvariantCultureIgnoreCase))
				{
					string newpath = Path.Combine(Path.GetDirectoryName(path), label);

					try
					{
						Directory.Move(path, newpath);
						node.Tag = newpath;

						RefreshFolder(node);
						e.CancelEdit = true;
					}
					catch (Exception exc)
					{
						ExceptionDialog.ShowException(exc);
						e.CancelEdit = true;
					}
				}
			}
			else
			{
				if (!String.IsNullOrEmpty(label) &&
					!Path.GetFileNameWithoutExtension(path).Equals(
					label, StringComparison.InvariantCultureIgnoreCase))
				{
					string newpath = Path.Combine(
						Path.GetDirectoryName(path), label + Path.GetExtension(path));

					try
					{
						File.Move(path, newpath);
						node.Tag = newpath;
					}
					catch (Exception exc)
					{
						ExceptionDialog.ShowException(exc);
						e.CancelEdit = true;
					}
				}
			}
		}
	}
}
