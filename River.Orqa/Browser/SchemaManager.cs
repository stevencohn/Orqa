//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// blah.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Data;
	using System.Text;
	using System.Windows.Forms;
	using River.Orqa.Database;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class SchemaManager
	//********************************************************************************************

	internal partial class SchemaManager : UserControl, IBrowser
	{
		#region SchemaItem
		private class SchemaItem
		{
			private string text;
			private SchemataNode node;

			public SchemaItem (SchemataSchema node)
			{
				this.node = node;
				this.text = node.Parent.Text + "." + node.Text;
			}

			public SchemataNode Node
			{
				get { return node; }
			}

			public override string ToString ()
			{
				return text;
			}
		}
		#endregion SchemaItem

		private Translator translator;				// resources
		private bool toggling;						// true if expanding/collapsing (no rename)


		public SchemaManager ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// Events
		//========================================================================================

		public event BrowserSelectorEventHandler SchemaSelected;

		public event BrowserTreeEventHandler SchemataSelected;
		public event BrowserTreeEventHandler Compiling;
		public event BrowserTreeEventHandler Editing;
		public event BrowserTreeEventHandler Opening;
		public event BrowserTreeEventHandler ShowingProperties;


		//========================================================================================
		// OnLoad()
		//========================================================================================

		/// <summary>
		/// Instantiate translator here to to make VS designer happy.
		/// Otherwise, VS removes this component from its parent!
		/// </summary>
		/// <param name="e">The event argument.</param>

		protected override void OnLoad (EventArgs e)
		{
			if (this.DesignMode)
				return;

			translator = new Translator("Browser");
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the number of current connections.
		/// </summary>

		public int Count
		{
			get
			{
				return tree.Nodes.Count;
			}
		}


		/// <summary>
		/// Gets or sets the currently selected schema in the schema selector.
		/// </summary>

		public SchemataSchema Schema
		{
			get
			{
				return (SchemataSchema)((SchemaItem)(schemaBox.SelectedItem)).Node;
			}

			set
			{
				bool found = false;
				int i = 0;
				SchemaItem item;
				while ((i < schemaBox.Items.Count) && !found)
				{
					item = (SchemaItem)schemaBox.Items[i];
					if (!(found = (item.Node == value)))
						i++;
				}

				schemaBox.SelectedIndex = i;
			}
		}


		/// <summary>
		/// Gets an array of database connections representing each server attached to.
		/// This is used by the About box to show status of all current connections.
		/// </summary>

		public DatabaseConnection[] Servers
		{
			get
			{
				// servers are represented by top-level root nodes in tree
				DatabaseConnection[] servers = new DatabaseConnection[tree.Nodes.Count];

				for (int i = 0; i < tree.Nodes.Count; i++)
				{
					servers[i] = ((SchemataServer)(tree.Nodes[i])).DatabaseConnection;
				}

				return servers;
			}
		}


		//========================================================================================
		// AddConnection()
		//========================================================================================

		/// <summary>
		/// Adds a connection and its representation to both the schema selector
		/// and the object tree.
		/// </summary>
		/// <param name="con">The database connection to add.</param>

		public void AddConnection (DatabaseConnection con)
		{
			// find an existing node for this connection
			bool found = false;
			int i = 0;
			DatabaseConnection dc;

			while ((i < tree.Nodes.Count) && !found)
			{
				dc = ((SchemataServer)tree.Nodes[i]).DatabaseConnection;

				if ((dc.HostName == con.HostName) && (dc.ServiceName == con.ServiceName))
				{
					found = true;
				}
				else
				{
					i++;
				}
			}

			if (!found)
			{
				// create a new node for the connection
				SchemataServer srvnode = new SchemataServer(con);
				srvnode.Expand();
				tree.Nodes.Insert(0, srvnode);

				// after the node is expanded, it is auto discovered so we can grab schemas
				foreach (TreeNode node in srvnode.Nodes)
				{
					schemaBox.Items.Add(new SchemaItem((SchemataSchema)node));
				}

				int index = schemaBox.FindString(srvnode.Text + "." + con.DefaultSchema);
				schemaBox.SelectedIndex = (index < 0 ? 0 : index);
			}
		}


		/// <summary>
		/// Builds a collection of SchemataSchema items representing the selectable
		/// schemas for the specified database connection.
		/// </summary>
		/// <param name="con">The database connection to examine</param>
		/// <returns></returns>

		public SchemataSchema[] GetDatabaseSchemas (DatabaseConnection con)
		{
			// find the node for this connection
			bool found = false;
			int i = 0;
			while ((i < tree.Nodes.Count) && !found)
			{
				if (!(found = ((SchemataServer)tree.Nodes[i]).DatabaseConnection.Equals(con)))
					i++;
			}

			// we should fine it!  if not, we've got a big problem!

			TreeNode server = tree.Nodes[i];
			SchemataSchema[] schemas = new SchemataSchema[server.Nodes.Count];

			for (int s = 0; s < server.Nodes.Count; s++)
			{
				schemas[s] = (SchemataSchema)server.Nodes[s];
			}

			return schemas;
		}


		//========================================================================================
		// DoSchemaSelected()
		//		This should bubble up to the main window to inform the current query window
		//		to change the default schema.
		//========================================================================================

		private void DoSchemaSelected (object sender, EventArgs e)
		{
			if (SchemaSelected != null)
			{
				SchemaSelected(((SchemaItem)(schemaBox.SelectedItem)).Node.Text);
			}
		}


		//========================================================================================
		// FindProcParameters()
		//		Returns an array of parameters for the specified procedure.
		//
		//		  1 Part:
		//			Procedure
		//				search current server.schema for procedure
		//
		//		  2 Parts:
		//			Schema.Procedure
		//			Package.Procedure
		//				search current server for schema.procedure
		//				then search current server.schema for package.procedure
		//
		//		  3 Parts:
		//			Server.Schema.Procedure
		//			Schema.Package.Procedure
		//				Search for server.schema.procedure
		//				then search current server for schema.package.procedure
		//
		//		  4 Parts:
		//			Server.Schema.Package.Procedure
		//				fully qualified!
		//
		//========================================================================================

		public SchemataParameter[] FindProcParameters (string procName)
		{
			SchemataParameter[] pars = null;

			// selector.Text looks something like: "svcnam @host.schema"
			// "everest @local.anonymous"
			// ". @cohn-laptop.EVEREST"

			string[] serviceParts = schemaBox.Text.Split('@');
			string[] hostParts = serviceParts[1].Split('.');

			string service = serviceParts[0].Trim();
			string server = hostParts[0].Trim().ToLower();
			string schema = hostParts[1].ToLower();

			string[] parts = procName.Split('.');
			switch (parts.Length)
			{
				case 1:
					pars = FindProcParameters(server, schema, parts[0]);
					break;

				case 2:
					pars = FindProcParameters(server, parts[0], parts[1]);

					if (pars.Length == 0)
						pars = FindProcParameters(server, schema, parts[0], parts[1]);
					break;

				case 3:
					pars = FindProcParameters(parts[0], parts[1], parts[2]);

					if (pars.Length == 0)
						pars = FindProcParameters(server, parts[0], parts[1], parts[2]);
					break;

				case 4:
					pars = FindProcParameters(parts[0], parts[1], parts[2], parts[3]);
					break;
			}

			return pars;
		}


		private SchemataParameter[] FindProcParameters (
			string serverName, string schemaName, string procName)
		{
			SchemataProcedure procedure = null;

			SchemataServer server = FindServer(serverName);
			if (server != null)
			{
				SchemataSchema schema = server.FindSchema(schemaName);
				if (schema != null)
				{
					procedure = schema.FindProcedure(procName);
				}
			}

			return (procedure == null ? new SchemataParameter[0] : procedure.Parameters);
		}


		private SchemataParameter[] FindProcParameters (
			string serverName, string schemaName, string packageName, string procName)
		{
			SchemataProcedure procedure = null;

			SchemataServer server = FindServer(serverName);
			if (server != null)
			{
				SchemataSchema schema = server.FindSchema(schemaName);
				if (schema != null)
				{
					SchemataPackage package = schema.FindPackage(packageName);
					if (package != null)
					{
						procedure = package.FindProcedure(procName);
					}
				}
			}

			return (procedure == null ? null : procedure.Parameters);
		}


		private SchemataServer FindServer (string serverName)
		{
			int i = 0;
			bool found = false;
			SchemataServer server = null;

			while ((i < tree.Nodes.Count) && !found)
			{
				server = (SchemataServer)tree.Nodes[i];
				if (!(found = server.DatabaseConnection.HostName.ToLower().Equals(serverName)))
					i++;
			}

			return (found ? server : null);
		}


		//========================================================================================
		// TreeView event handlers
		//========================================================================================

		#region TreeView Event Handlers

		private void DoAfterCollapse (object sender, TreeViewEventArgs e)
		{
			if (e.Node.ImageIndex == SchemaIcons.FolderOpen)
				e.Node.ImageIndex = e.Node.SelectedImageIndex = SchemaIcons.FolderClose;

			toggling = false;
		}


		private void DoAfterExpand (object sender, TreeViewEventArgs e)
		{
			if (e.Node.ImageIndex == SchemaIcons.FolderClose)
				e.Node.ImageIndex = e.Node.SelectedImageIndex = SchemaIcons.FolderOpen;

			toggling = false;
		}


		private void DoAfterLabelEdit (object sender, NodeLabelEditEventArgs e)
		{
			if (e.Label != null)
			{
				SchemataNode node = (SchemataNode)tree.SelectedNode;
				node.Rename(e.Label);
				node.Text = e.Label.ToUpper();
				tree.LabelEdit = false;
			}
		}


		private void DoAfterSelect (object sender, TreeViewEventArgs e)
		{
			if (e.Node is SchemataNode)
			{
				if (SchemataSelected != null)
					SchemataSelected(new BrowserTreeEventArgs(BrowserTreeAction.Select, (SchemataNode)e.Node));
			}
		}


		private void DoBeforeCollapse (object sender, TreeViewCancelEventArgs e)
		{
			toggling = true;
		}


		private void DoBeforeExpand (object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node is SchemataNode)
			{
				// TODO: LockWindow();

				SchemataNode node = (SchemataNode)e.Node;
				if (node.HasDiscovery && !node.IsDiscovered)
					node.Discover();

				toggling = true;
			}
		}


		private void DoNodeMouseClick (object sender, TreeNodeMouseClickEventArgs e)
		{
			if (toggling)
				return;

			if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
			{
				if (e.Node == tree.SelectedNode)
				{
					DoRename(sender, e);
				}
			}
		}


		private void DoMouseDown (object sender, MouseEventArgs e)
		{
			// Node might not be a SchemataNode if the "Discovering..." node is
			// displayed and the user tried to click it before discovery completes

			TreeNode rawnode = tree.GetNodeAt(e.X, e.Y);
			if ((rawnode == null) || !(rawnode is SchemataNode))
				return;

			SchemataNode node = (SchemataNode)rawnode;

			if (e.Button == MouseButtons.Left)
			{
				// for some reason, tree doesn't assume focus when you return to the
				// control with a left click.  So let's force it here.
				tree.Focus();

				tree.SelectedNode = node;

				if (node.CanDrag)
				{
					int i = node.Text.IndexOf(" ");
					string text = (i < 0 ? node.Text : node.Text.Substring(0, i));

					tree.DoDragDrop(text, DragDropEffects.Copy);
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				tree.SelectedNode = node;

				//if (node.CanOpen || node.CanEdit || node.CanDelete
				//    || node.CanRefresh || node.CanCompile || node.CanDescribe)
				{
					if (node.CanDescribe)
					{
						treeMenu.Items[1].Image = menuIcons.Images[1];
						treeMenu.Items[1].Text = translator.GetString("ScriptContextMenu");
					}
					else
					{
						treeMenu.Items[1].Image = menuIcons.Images[0];
						treeMenu.Items[1].Text = translator.GetString("EditContextMenu");
					}

					treeMenu.Items[0].Enabled = node.CanOpen;
					treeMenu.Items[1].Enabled = node.CanEdit || node.CanDescribe;
					treeMenu.Items[2].Enabled = node.CanCompile;
					treeMenu.Items[3].Enabled = node.CanDelete;
					treeMenu.Items[4].Enabled = node.CanRename;
					treeMenu.Items[7].Enabled = node.CanRefresh;
				}
			}
		}


		private void DoKeyUp (object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Delete:
					// DeleteObject()
					e.Handled = true;
					e.SuppressKeyPress = true;
					break;

				case Keys.Enter:
					if (tree.SelectedNode != null)
					{
						SchemataNode node = (SchemataNode)tree.SelectedNode;
						node.DoDefaultAction();
						e.Handled = true;
						e.SuppressKeyPress = true;
					}
					break;

				default:
					e.Handled = false;
					break;
			}
		}

		#endregion TreeView Event Handlers


		#region Context Menu Event Handlers

		private void DoCompile (object sender, EventArgs e)
		{
			if (Compiling != null)
			{
				SchemataNode node = (SchemataNode)tree.SelectedNode;
				if (node.IsWrapped)
				{
					MessageBox.Show(
						"Unable to compile.  Contents are wrapped.",
						"Wrapped Content",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
						);

					return;
				}

				Compiling(new BrowserTreeEventArgs(
					BrowserTreeAction.Compile, (SchemataNode)tree.SelectedNode));
			}
		}


		private void DoDelete (object sender, EventArgs e)
		{
			SchemataNode node = (SchemataNode)tree.SelectedNode;

			DialogResult result = MessageBox.Show(
				String.Format(translator.GetString("ConfirmDeleteMessage"), node.Text),
				translator.GetString("ConfirmDeleteTitle"),
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning
				);

			if (result == DialogResult.Yes)
			{
				SchemataNode parent = (SchemataNode)node.Parent;

				TreeNode next = null;
				if (tree.SelectedNode.NextNode != null)
					next = tree.SelectedNode.NextNode;
				else if (tree.SelectedNode.PrevNode != null)
					next = tree.SelectedNode.PrevNode;
				else
					next = parent;

				if (node.Delete())
				{
					if (next != null)
						tree.SelectedNode = next;
				}
			}
		}


		private void DoEdit (object sender, EventArgs e)
		{
			if (Editing != null)
			{
				SchemataNode node = (SchemataNode)tree.SelectedNode;
				if (node.IsWrapped)
				{
					MessageBox.Show(
						"Unable to edit.  Contents are wrapped.",
						"Wrapped Content",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
						);

					return;
				}

				Editing(new BrowserTreeEventArgs(
					BrowserTreeAction.Edit, (SchemataNode)tree.SelectedNode));
			}
		}


		private void DoOpen (object sender, EventArgs e)
		{
			if (Opening != null)
				Opening(new BrowserTreeEventArgs(
					BrowserTreeAction.Open, (SchemataNode)tree.SelectedNode));
		}


		private void DoProperties (object sender, EventArgs e)
		{
			if (ShowingProperties != null)
				ShowingProperties(new BrowserTreeEventArgs(
					BrowserTreeAction.Properties, (SchemataNode)tree.SelectedNode));
		}


		private void DoRefresh (object sender, EventArgs e)
		{
			((SchemataNode)tree.SelectedNode).Refresh();
		}


		private void DoRename (object sender, EventArgs e)
		{
			SchemataNode node = (SchemataNode)tree.SelectedNode;
			if (node.CanRename)
			{
				tree.LabelEdit = true;
				tree.SelectedNode.BeginEdit();
			}
		}

		#endregion Context Menu Event Handlers
	}
}
