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
	using System.IO;
	using System.Windows.Forms;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class ProjectTree
	//********************************************************************************************

	/// <summary>
	/// Displays and manages a tree view used to present SQL templates.  These templates
	/// can be dragged/dropped onto a Query window or double-clicked to copy their contents
	/// into the current Query window.  They can also be dragged onto the desktop or similar
	/// targets.
	/// </summary>

	internal partial class ProjectTree : UserControl, IWorker
	{

		private ICommander commander;					// reference to main commander

		// Events

		public event ScriptSelectionHandler ScriptSelected;
		public event ScriptOpenHandler ScriptOpen;


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new template tree controle
		/// </summary>

		public ProjectTree ()
		{
			InitializeComponent();

			tree.ShowRootLines = false;
			tree.GotFocus += new EventHandler(DoUpdateCommander);
		}


		#region Commander

		public void UpdateCommander ()
		{
			DoUpdateCommander(this, new EventArgs());
		}


		private void DoUpdateCommander (object sender, EventArgs e)
		{
			commander.SetWorker(this);

			commander.AdvancedControls.IsEnabled = false;
			commander.ConnectControls.IsEnabled = true;
			commander.ExecuteControls.IsEnabled = false;
			commander.PasteControls.IsEnabled = CanPaste;
			commander.RedoControls.IsEnabled = false;
			commander.SaveControls.IsEnabled = !IsSaved;
			commander.SelectControls.IsEnabled = HasSelection;
			commander.TextControls.IsEnabled = false;
			commander.UndoControls.IsEnabled = false;
			commander.SetChecker("ToggleWhitespace", false);
			commander.SetChecker("ToggleResultsPane", false);
			commander.SqlControls.IsEnabled = false;
		}

		#endregion Commander


		//========================================================================================
		// Properties
		//========================================================================================

		public bool CanPaste
		{
			get { return false; }
		}


		public ICommander Commander
		{
			set { this.commander = value; }
		}


		public string Filename
		{
			get
			{
				TreeNode node = tree.SelectedNode;
				while (node.Parent != null)
					node = node.Parent;

				Project project = (Project)node.Tag;

				return project.Root.Path;
			}
		}


		public bool HasSelection
		{
			get { return (tree.Nodes.Count > 0); }
		}


		public bool IsSaved
		{
			get { return false; }
		}
		

		//========================================================================================
		// AddProject()
		//========================================================================================

		public void AddProject (string path)
		{
			#region Temporary Intro Panel management
			if (introPanel != null)
			{
				introPanel.Controls.Remove(introBox);
				this.Controls.Remove(introPanel);
				introBox = null;
				introPanel = null;
				tree.Dock = DockStyle.Fill;
			}
			#endregion Temporary Intro Panel management

			Project project = null;

			try
			{
				var manager = new ProjectManager();
				project = manager.LoadProjectFile(path);
			}
			catch (Exception exc)
			{
				Dialogs.ExceptionDialog.ShowException(exc);
			}

			TreeNode projectNode;

			if (tree.Nodes.ContainsKey(project.Name))
			{
				projectNode = tree.Nodes[project.Name];
			}
			else
			{
				int index = BestIndexOfProject(project.Name);

				if (index > tree.Nodes.Count)
					projectNode = tree.Nodes.Add(project.Name, project.Name, 0, 0);
				else
					projectNode = tree.Nodes.Insert(index, project.Name, project.Name, 0, 0);

				projectNode.Tag = project;
				projectNode.ContextMenuStrip = projectContextMenu;

				PopulateProject(project.Root.Items, projectNode.Nodes);
			}

			projectNode.Expand();
		}


		private int BestIndexOfProject (string name)
		{
			int i = 0;
			bool found = false;

			while ((i < tree.Nodes.Count) && !found)
			{
				if (!(found = (tree.Nodes[i].Text.CompareTo(name) > 0)))
					i++;
			}

			return i;
		}


		//========================================================================================
		// PopulateProject()
		//========================================================================================

		private void PopulateProject (ProjectItemCollection items, TreeNodeCollection nodes)
		{
			ProjectItemCollection.Enumerator e = items.GetEnumerator();
			while (e.MoveNext())
			{
				ProjectItem item = e.Current;

				TreeNode node = CreateNode(item);
				nodes.Add(node);

				if (item.IsDirectory)
				{
					PopulateProject(item.Items, node.Nodes);
				}
			}
		}


		private TreeNode CreateNode (ProjectItem item)
		{
			int image = 4; // missing icon

			if (item.IsDirectory)
			{
				image = 1;
			}
			else if (item.Exists)
			{
				if (item.Extension.Equals(".sql"))
					image = 3;
				else if (item.Extension.Equals(".bat"))
					image = 5;
				else
					image = 6; // general text icon
			}

			TreeNode node = new TreeNode(item.Name, image, image);
			node.Name = item.Name;
			node.Tag = item;

			if (item.IsDirectory)
				node.ContextMenuStrip = folderContextMenu;
			else
				node.ContextMenuStrip = scriptContextMenu;

			return node;
		}


		//========================================================================================
		// Add handlers
		//========================================================================================

		#region Add handlers

		private void DoAddNewFolder (object sender, EventArgs e)
		{
			var dialog = new Dialogs.NewFolderDialog();
			DialogResult result = dialog.ShowDialog();

			if ((result == DialogResult.OK) && (dialog.FolderName != null))
			{
				TreeNode parent = tree.SelectedNode;
				IProjectContainer container = (IProjectContainer)parent.Tag;

				if (!Directory.Exists(container.FolderPath))
				{
					Directory.CreateDirectory(container.FolderPath);
				}

				var item = new DirectoryItem(container.FolderPath + "\\" + dialog.FolderName);

				int index = container.AddChild(item);
				TreeNode node = CreateNode(item);

				if (index < tree.SelectedNode.Nodes.Count)
					parent.Nodes.Insert(index, node);
				else
					parent.Nodes.Add(node);

				parent.Expand();
			}
		}


		private void DoAddNewItem (object sender, EventArgs e)
		{
			var dialog = new Dialogs.NewItemDialog();

			var translator = new Translator("Dialogs.NewItems");
			dialog.SetTemplateConfiguration(translator.GetString("SqlProjectItems"));

			DialogResult result = dialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				TreeNode parent = tree.SelectedNode;
				IProjectContainer container = (IProjectContainer)parent.Tag;

				if (!Directory.Exists(container.FolderPath))
				{
					Directory.CreateDirectory(container.FolderPath);
				}

				string filnam = container.FolderPath + "\\" + dialog.FileName;

				using (var writer = new StreamWriter(filnam, false))
				{
					writer.WriteLine(dialog.TemplateContent);
					writer.Close();
				}

				var item = new FileItem(container.FolderPath + "\\" + dialog.FileName);

				int index = container.AddChild(item);
				TreeNode node = CreateNode(item);

				if (index < tree.SelectedNode.Nodes.Count)
					parent.Nodes.Insert(index, node);
				else
					parent.Nodes.Add(node);

				parent.Expand();
			}
		}

		#endregion Add handlers


		//========================================================================================
		// Remove handlers
		//========================================================================================

		#region Remove handlers

		private void DoCloseProject (object sender, EventArgs e)
		{
			if (tree.SelectedNode.Parent == null)
				tree.Nodes.Remove(tree.SelectedNode);
		}


		private void DoDeleteItem (object sender, EventArgs e)
		{
			ProjectItem item = (ProjectItem)tree.SelectedNode.Tag;

			Translator translator = new Translator("Browser");

			DialogResult result = MessageBox.Show(
				String.Format(translator.GetString("ConfirmDeleteMessage"), item.Name),
				translator.GetString("ConfirmDeleteTitle"),
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);

			if (result == DialogResult.Yes)
			{
				try
				{
					if (item.IsDirectory)
					{
						if (Directory.Exists(item.Path))
							Directory.Delete(item.Path, true);
					}
					else
					{
						if (File.Exists(item.Path))
							File.Delete(item.Path);
					}

					TreeNode parent = tree.SelectedNode.Parent;
					IProjectContainer container = (IProjectContainer)parent.Tag;
					container.DeleteChild(item);

					tree.SelectedNode.Parent.Nodes.Remove(tree.SelectedNode);
				}
				catch
				{
					// show a message?
				}
			}
		}

		#endregion Remove handlers


		//========================================================================================
		// OpenSaveRefresh stuff
		//========================================================================================

		#region OpenSaveRefresh stuff

		private void DoOpenScript (object sender, EventArgs e)
		{
			if (ScriptSelected != null)
			{
				ProjectItem item = (ProjectItem)tree.SelectedNode.Tag;
				ScriptSelected(item.Path);
			}
		}


		private void DoRefreshFolder (object sender, EventArgs e)
		{
			DoRefreshProject(sender, e);
		}


		private void DoRefreshProject (object sender, EventArgs e)
		{
			TreeNode node = tree.SelectedNode;
			while (node.Parent != null)
			{
				node = node.Parent;
			}

			Project project = (Project)node.Tag;
			tree.Nodes.Remove(node);
			AddProject(project.Root.Path);
		}


		private void DoSaveProject (object sender, EventArgs e)
		{
			TreeNode node = tree.SelectedNode;
			while (node.Parent != null)
			{
				node = node.Parent;
			}

			var manager = new ProjectManager();
			Project project = (Project)node.Tag;
			manager.SaveProjectFile(project);
		}


		private void DoScriptOpen (object sender, EventArgs e)
		{
			if (ScriptOpen != null)
				ScriptOpen();
		}


		public void SaveFile (string filename)
		{
			// TODO: SaveAs
			DoSaveProject(this, new EventArgs());
		}

		#endregion OpenSaveRefresh stuff


		//========================================================================================
		// Tree handlers
		//========================================================================================

		#region Tree handlers

		private void DoBeforeExpand (object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Parent != null)
			{
				e.Node.ImageIndex = e.Node.SelectedImageIndex = 2;
			}
		}


		private void DoBeforeCollapse (object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Parent == null)
			{
				e.Cancel = true;
				return;
			}

			e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
		}


		private void DoKeyUp (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (tree.SelectedNode == null)
					return;

				if (tree.SelectedNode.Parent == null)
				{
					tree.SelectedNode.Toggle();
				}
				else
				{
					ProjectItem item = (ProjectItem)tree.SelectedNode.Tag;

					if (item.IsDirectory)
						tree.SelectedNode.Toggle();
					else
						DoOpenScript(sender, e);
				}

				e.Handled = true;
			}
		}


		private void DoNodeMouseDoubleClick (object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Parent != null)
			{
				ProjectItem item = (ProjectItem)e.Node.Tag;
				if (!item.IsDirectory)
				{
					DoOpenScript(sender, e);
				}
			}
		}

		#endregion Tree handlers
	}
}
