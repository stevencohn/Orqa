//************************************************************************************************
// Copyright © 2005 River Corporation. All Rights Reserved.
//
// Enhances the standards Windows TreeView control, adding multi-node
// selection features, CheckBox utility features and eliminating flicker.
//
//************************************************************************************************

namespace River.Orqa.Controls
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;
	using River.Orqa.Options;


	//********************************************************************************************
	// class RiverTreeView
	//********************************************************************************************

	/// <summary>
	/// Enhances the standards Windows TreeView control, adding multi-node
	/// selection features, CheckBox utility features and eliminating flicker.
	/// </summary>
	/// <remarks>
	/// Run-time changes to TreeNode ForeColor or BackColor properties are retained by
	/// this class.  However, if the color is changed while the node is currently selected
	/// the color change may be lost.
	/// </remarks>

	public class RiverTreeView : TreeView
	{
		private bool multiSelect;						// true if allows multi-selections
		private TreeNode anchorNode;					// anchor of multi-select range
		private NodeCollection selectedNodes;			// list of selected nodes
		private ColoringCollection colorings;			// tracks custom node colors


		#region NodeCollection

		/// <summary>
		/// Maintains a collection of nodes.  This is used for both multiple selection
		/// mode and the list of all nodes that are currently checked.
		/// </summary>

		public class NodeCollection : List<TreeNode>
		{
			/// <summary>
			/// Returns an enumerator that can be used to iterate through the collection.
			/// </summary>
			/// <returns>A strongly-typed NodeCollection.Enumerator instance.</returns>

			public new NodeCollection.Enumerator GetEnumerator ()
			{
				return base.GetEnumerator();
			}
		}

		#endregion NodeCollection

		#region Coloring

		private class Coloring
		{
			public TreeNode Node;
			public Color ForeColor;
			public Color BackColor;
		}

		private class ColoringCollection : List<Coloring>
		{
			public void Add (TreeNode node)
			{
				Coloring coloring = new Coloring();
				coloring.Node = node;
				coloring.BackColor = node.BackColor;
				coloring.ForeColor = node.ForeColor;

				this.Add(coloring);
			}


			public bool Contains (TreeNode node)
			{
				int i = 0;
				bool found = false;
				while ((i < this.Count) && !found)
				{
					if (!(found = this[i].Node == node))
						i++;
				}

				return found;
			}


			public Coloring GetColoring (TreeNode node)
			{
				int i = 0;
				bool found = false;
				while ((i < this.Count) && !found)
				{
					if (!(found = this[i].Node.Equals(node)))
						i++;
				}

				return (found ? this[i] : null);
			}


			public new ColoringCollection.Enumerator GetEnumerator ()
			{
				return base.GetEnumerator();
			}
		}

		#endregion Coloring


		// Events

		#region Events

		/// <summary>
		/// This TreeViewEventHandler signals when a node is deselected in the tree
		/// view.  This is signaled when using either the mouse or keyboard.
		/// </summary>

		[Category("Waters"),
		Description("Occurs when a node is deselected.")]
		public event TreeViewEventHandler AfterDeselect;

		#endregion Events


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new empty tree view set to single-node selection mode.
		/// </summary>

		public RiverTreeView ()
		{
			// base fields
			this.HideSelection = false;

			// extended fields
			this.multiSelect = false;
			this.anchorNode = null;
			this.selectedNodes = new NodeCollection();
			this.colorings = new ColoringCollection();

			//this.DrawMode = TreeViewDrawMode.OwnerDrawAll;

			this.BackColor = Color.FromArgb(41, 41, 41);
			this.ForeColor = Color.FromArgb(222, 222, 188);

			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}


		//========================================================================================
		// Properties
		//========================================================================================

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating if checkboxes are dispayed in the tree.
		/// The default is <b>false</b>.
		/// </summary>
		/// <remarks>
		/// This property is mutually exclusive with the MultiSelect property; only one
		/// can be <b>true</b> at a time.  If MultiSelect is currently <b>true</b> and
		/// the CheckBoxes property is then set to <b>true</b>, MultiSelect is 
		/// automatically reset to <b>false</b>.
		/// </remarks>

		[Category("Waters"),
		Description("Gets or sets a value indicating if checkboxes are displayed in the tree.")]
		public new bool CheckBoxes
		{
			get
			{
				return base.CheckBoxes;
			}

			set
			{
				// When removing checkboxes, base.TreeView collapses all branches ensuring
				// visibility only of the currently select node.  So to avoid user confusion,
				// (when done at run-time) capture all expanded nodes and restore expansions
				// after checkboxes are removed.

				NodeCollection expanded = null;
				if (!value)
				{
					expanded = new NodeCollection();
					FindExpandedNodes(this.Nodes, expanded);
					UncheckAllNodes();
				}
				else if (multiSelect)
				{
					// CheckBoxes is mutually exclusive of MultiSelect
					DeselectAllNodes();
					multiSelect = false;
				}

				base.CheckBoxes = value;

				if (expanded != null)
				{
					while (expanded.Count > 0)
					{
						expanded[0].Expand();
						expanded.RemoveAt(0);
					}

					expanded.Clear();
					expanded = null;
				}
			}
		}


		private void FindExpandedNodes (TreeNodeCollection nodes, NodeCollection list)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].IsExpanded)
					list.Add(nodes[i]);

				if (nodes[i].Nodes.Count > 0)
					FindExpandedNodes(nodes[i].Nodes, list);
			}
		}


		/// <summary>
		/// Gets a collection of all checked nodes.  The collection will contain
		/// zero or more entires.
		/// </summary>
		/// <remarks>
		/// Use RiverTreeView.NodeCollection.Enumerator to declare an iterator for this
		/// collection.
		/// <para>
		/// This collection is separate from the collection of checked nodes internally
		/// managed by this tree view.  Changes to the collection of checked nodes within
		/// the tree view will not be reflected in a NodeCollection returned from this 
		/// property prior to those changes.  For example, nodes in this returned collection
		/// may have Checked properties set to <b>false</b> or the returned collection
		/// may be missing newly checked nodes.
		/// </para>
		/// </remarks>

		[Browsable(false)]
		public NodeCollection CheckedNodes
		{
			get { return GetCheckedNodes(); }
		}


		/// <summary>
		/// Gets or sets a value indicating if multiple selections are allowed.
		/// The default is <b>false</b>.
		/// </summary>
		/// <remarks>
		/// This property is mutually exclusive with the Checkboxes property; only one
		/// can be <b>true</b> at a time.  If Checkboxes is currently <b>true</b> and
		/// the MultiSelect property is then set to <b>true</b>, Checkboxes is 
		/// automatically reset to <b>false</b>.
		/// </remarks>

		[Category("Waters"),
		Description("Gets or sets a value indicating if multiple selections are allowed.")]
		public bool MultiSelect
		{
			get
			{
				return multiSelect;
			}

			set
			{
				if (multiSelect && !value)
				{
					// clean up all multi-select nodes
					DeselectAllNodes();
				}

				multiSelect = value;

				if (multiSelect)
				{
					// MultiSelect is mutual exclusive of CheckBoxes
					this.CheckBoxes = false;
				}
			}
		}


		/// <summary>
		/// Gets a collection of all selected nodes in the tree view.
		/// </summary>
		/// <remarks>
		/// If MultiSelect is <b>false</b> then the collection will contain zero or one
		/// entries; otherwise, the collection will contain zero or more entries.
		/// <para>
		/// Use RiverTreeView.NodeCollection.Enumerator to declare an iterator for this
		/// collection.
		/// </para>
		/// <para>
		/// This collection is separate from the collection of selected nodes internally
		/// managed by this tree view.  Changes to the collection of selected nodes within
		/// the tree view will not be reflected in a NodeCollection returned from this 
		/// property prior to those changes.
		/// </para>
		/// </remarks>

		[Browsable(false)]
		public NodeCollection SelectedNodes
		{
			get
			{
				// create a copy collection
				NodeCollection nodes = new NodeCollection();

				if (multiSelect)
				{
					// this test catches the "default selection" behavior of TreeView
					if ((selectedNodes.Count == 0) && (this.SelectedNode != null))
					{
						selectedNodes.Add(this.SelectedNode);
					}

					// copy the selection list
					foreach (TreeNode node in selectedNodes)
					{
						nodes.Add(node);
					}
				}

				// this.SelectedNode is not included in the selectedNodes collection
				// during multi-selects, so we can safely append it to the return list.

				if ((this.SelectedNode != null) && !nodes.Contains(this.SelectedNode))
				{
					// add the single selected node
					nodes.Add(this.SelectedNode);
				}

				return nodes;
			}
		}

		#endregion Properties


		//========================================================================================
		// Checked nodes methods
		//========================================================================================

		#region Checked nodes methods

		/// <summary>
		/// Checks all nodes in the tree.
		/// This is only effective if CheckBoxes is enabled.
		/// </summary>

		public void CheckAllNodes ()
		{
			if (base.CheckBoxes)
			{
				SetCheckedState(this.Nodes, true);
			}
		}


		/// <summary>
		/// Checks the specified node and all of its descendents.
		/// This is only effective if CheckBoxes is set to true.
		/// </summary>
		/// <param name="node">The starting node.</param>

		public void CheckAllNodes (TreeNode node)
		{
			if (base.CheckBoxes)
			{
				if (node == null)
					CheckAllNodes();

				node.Checked = true;
				SetCheckedState(node.Nodes, true);
			}
		}


		/// <summary>
		/// Unchecks all nodes in the tree.
		/// This is only effective if CheckBoxes is enabled.
		/// </summary>

		public void UncheckAllNodes ()
		{
			if (base.CheckBoxes)
			{
				SetCheckedState(this.Nodes, false);
			}
		}


		/// <summary>
		/// Unchecks the specified node and all of its descendents.
		/// This is only effective if CheckBoxes is set to true.
		/// </summary>
		/// <param name="node">The starting node.</param>

		public void UncheckAllNodes (TreeNode node)
		{
			if (base.CheckBoxes)
			{
				if (node == null)
					UncheckAllNodes();

				node.Checked = false;
				SetCheckedState(node.Nodes, false);
			}
		}


		// This is an internal helper for the CheckNodes/UnCheckNodes methods

		private void SetCheckedState (TreeNodeCollection nodes, bool isChecked)
		{
			foreach (TreeNode node in nodes)
			{
				node.Checked = isChecked;

				if (node.Nodes.Count > 0)
					SetCheckedState(node.Nodes, isChecked);
			}
		}


		// This is an internal helper for the CheckedNodes property.

		private NodeCollection GetCheckedNodes ()
		{
			NodeCollection checkedNodes = new NodeCollection();
			GetCheckedNodes(this.Nodes, checkedNodes);
			return checkedNodes;
		}


		private void GetCheckedNodes (TreeNodeCollection nodes, NodeCollection checkedNodes)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Checked)
					checkedNodes.Add(node);

				if (node.Nodes.Count > 0)
					GetCheckedNodes(node.Nodes, checkedNodes);
			}
		}

		#endregion Checked nodes methods


		//========================================================================================
		// Selected nodes methods
		//========================================================================================

		#region Selected nodes methods

		/// <summary>
		/// Deselects all nodes in the tree including the node that currently has focus.
		/// The SelectedNode property is also set to <b>null</b>.
		/// </summary>

		public void DeselectAllNodes ()
		{
			if (multiSelect)
			{
				DeselectAllNodes(this.Nodes);
			}

			this.SelectedNode = null;
		}


		/// <summary>
		/// Deselects the specified node and all of its descendents.
		/// This is only effective if MultiSelect is set to true.
		/// </summary>
		/// <param name="node">The starting node.</param>

		public void DeselectAllNodes (TreeNode node)
		{
			if (multiSelect)
			{
				if (node == null)
					DeselectAllNodes();

				DeselectNode(node);
				DeselectAllNodes(node.Nodes);

				selectedNodes.Clear();
			}
		}


		/// <summary>
		/// Deselect the specified node.
		/// </summary>
		/// <param name="node">The node in the tree to deselect.</param>

		public void DeselectNode (TreeNode node)
		{
			if (multiSelect)
			{
				if (selectedNodes.Contains(node))
				{
					selectedNodes.Remove(node);

					Coloring coloring = colorings.GetColoring(node);
					if (coloring == null && node.IsSelected)
					{
						node.BackColor = SystemColors.Window;
						node.ForeColor = SystemColors.WindowText;
					}
					else if (coloring != null)
					{
						node.BackColor = coloring.BackColor;
						node.ForeColor = coloring.ForeColor;
					}

					this.Invalidate(node.Bounds);
				}
			}
			else
			{
				if (this.SelectedNode == node)
				{
					this.SelectedNode = null;

					Coloring coloring = colorings.GetColoring(node);
					if (coloring != null)
					{
						node.BackColor = coloring.BackColor;
						node.ForeColor = coloring.ForeColor;
					}
				}
			}

			if (AfterDeselect != null)
				AfterDeselect(this, new TreeViewEventArgs(node));
		}


		/// <summary>
		/// Selects all nodes in the tree.
		/// This is only effective if MultiSelect is enabled.
		/// </summary>

		public void SelectAllNodes ()
		{
			if (multiSelect)
			{
				SelectAllNodes(this.Nodes);
			}
		}


		/// <summary>
		/// Selects the specified node and all of its descendents.
		/// This is only effective if MultiSelect is enabled.
		/// </summary>
		/// <param name="node">The starting node.</param>

		public void SelectAllNodes (TreeNode node)
		{
			if (multiSelect)
			{
				if (node == null)
					SelectAllNodes();

				selectedNodes.Clear();

				SelectNode(node);
				SelectAllNodes(node.Nodes);
			}
		}


		/// <summary>
		/// Select the specified node.
		/// </summary>
		/// <param name="node">The node in the tree to select.</param>
		/// <remarks>
		/// If single-node selection is enabled then any previously selected node is
		/// deselected prior to selecting this node.
		/// <para>
		/// If multi-node selection is enabled then this node is added to the collection
		/// of selected nodes for this tree view.
		/// </para>
		/// </remarks>

		public void SelectNode (TreeNode node)
		{
			Coloring coloring = colorings.GetColoring(node);
			if (coloring == null)
			{
				colorings.Add(node);
			}
			else
			{
				coloring.ForeColor = node.ForeColor;
				coloring.BackColor = node.BackColor;
			}

			if (multiSelect)
			{
				if (!selectedNodes.Contains(node))
					selectedNodes.Add(node);

				node.BackColor = FontsAndColors.SelectedTextBackground;
				node.ForeColor = SystemColors.HighlightText;

				this.Invalidate(node.Bounds);
			}
			else
			{
				this.SelectedNode = node;
			}
		}


		// This is a helper routine for node deselection

		private void DeselectAllNodes (TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				DeselectNode(node);

				if (node.Nodes.Count > 0)
					DeselectAllNodes(node.Nodes);
			}
		}


		// This is a helper routine for node selection

		private void SelectAllNodes (TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				SelectNode(node);

				if (node.Nodes.Count > 0)
					SelectAllNodes(node.Nodes);
			}
		}

		#endregion Selected nodes methods


		//========================================================================================
		// MultiSelect implementation
		//========================================================================================

		/// <summary>
		/// Gets a Boolean value indicating that the specified node is selected or unselected.
		/// </summary>
		/// <param name="node">The TreeNode to test.</param>
		/// <returns>Returns <b>true</b> if the specified node is selected; otherwise,
		/// returns <b>false</b>.</returns>

		public bool IsNodeSelected (TreeNode node)
		{
			return node.Equals(this.SelectedNode) || selectedNodes.Contains(node);
		}


		/// <summary>
		/// Prepares the multi-select state of the tree view; this state is finalized
		/// by the OnAfterSelect handler.
		/// <para>
		/// Inheritors that override this method must call the base version to ensure
		/// that event subscribers are notified accordingly.
		/// </para>
		/// </summary>
		/// <param name="e">The data for this event.</param>
		/// <remarks>
		/// There are generally three cases:
		/// <ul>
		/// <li>
		/// If the Ctrl key is pressed then simply add or remove the node
		/// from the selection list.
		/// </li>
		/// <li>
		/// If the Shift key is pressed then retain the existing anchor and clear
		/// the selection list.  The OnAfterSelect handler will re-populate this list
		/// from the anchor to the new this.SelectedNode.
		/// </li>
		/// <li>
		/// If neither Ctrl nor Shift is pressed then act like a single-node select
		/// tree.  This node is considered the new anchor point.
		/// </li>
		/// </ul>
		/// </remarks>

		protected override void OnBeforeSelect (TreeViewCancelEventArgs e)
		{
			if (multiSelect)
			{
				this.BeginUpdate();

				// recognize Cntrl regardless of other modifiers
				if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					if (selectedNodes.Contains(e.Node))
					{
						// a previously selected node needs to be deselected
						DeselectNode(e.Node);

						this.EndUpdate();
					}
					else
					{
						// add node to selected collection
						SelectNode(this.SelectedNode);
					}
				}
				// recognize Shift only if Cntrl is not also pressed
				else if ((Control.ModifierKeys & (Keys.Shift | Keys.Control)) == Keys.Shift)
				{
					if (e.Node == this.SelectedNode)
					{
						this.EndUpdate();
					}
					else
					{
						DeselectAllNodes();
					}
				}
				// if neither Cntrl nor Shift then act like single-node select
				else
				{
					if (selectedNodes.Count > 0)
					{
						// deselect any multi-select nodes before allowing new selection
						DeselectAllNodes();
					}

					// reset anchor for multi-node ranges
					anchorNode = e.Node;
				}
			}

			base.OnBeforeSelect(e);
		}


		/// <summary>
		/// Finalizes the multi-select state of the tree view as it was prepared by
		/// the OnBeforeSelect handler.
		/// <para>
		/// Inheritors that override this method must call the base version to ensure
		/// that event subscribers are notified accordingly.
		/// </para>
		/// </summary>
		/// <param name="e">The data for this event.</param>

		protected override void OnAfterSelect (TreeViewEventArgs e)
		{
			if (multiSelect)
			{
				// recognize Shift only if Cntrl is not also pressed
				if ((Control.ModifierKeys & (Keys.Shift | Keys.Control)) == Keys.Shift)
				{
					if (anchorNode.Bounds.Y < e.Node.Bounds.Y)
					{
						SelectRange(anchorNode, e.Node);
					}
					else
					{
						SelectRange(e.Node, anchorNode);
					}
				}

				this.EndUpdate();
			}

			base.OnAfterSelect(e);
		}


		// SHIFT-SELECT ALGORITHM:
		// - Start by deselecting any nodes in our selection list that fall outside 
		//   the known top/bottom coodindates.
		// - Then traverse the tree from the very first node until we find the
		//   bottomNode, selecing nodes within the coordinates along the way.

		private void SelectRange (TreeNode topNode, TreeNode bottomNode)
		{
			int topY = topNode.Bounds.Y;
			int bottomY = bottomNode.Bounds.Y;

			// deselect nodes beyond coordinates
			foreach (TreeNode node in selectedNodes)
			{
				if ((node.Bounds.Y < topY) || (node.Bounds.Y > bottomY))
				{
					DeselectNode(node);
				}
			}

			// select new nodes in range
			SelectRange(this.Nodes, topY, bottomY);
		}


		// Traverse the tree, selecting nodes within the vertical coordinates
		// until we reach the bottom node (last node before passing bottomY coordinate).

		private bool SelectRange (TreeNodeCollection nodes, int topY, int bottomY)
		{
			int nodeY;

			foreach (TreeNode node in nodes)
			{
				nodeY = node.Bounds.Y;

				if ((nodeY >= topY) && (nodeY <= bottomY))
				{
					// node is within range, so if it's not already selected, select it
					if (!selectedNodes.Contains(node))
					{
						SelectNode(node);
					}
				}
				else if (nodeY > bottomY)
				{
					// reached first node beyond bottomY coordinate so terminate scan
					return false;
				}

				// dive into expanded nodes, selecting only those visible
				if ((node.Nodes.Count > 0) && node.IsExpanded)
				{
					if (!SelectRange(node.Nodes, topY, bottomY))
					{
						return false;
					}
				}
			}

			return true;
		}


		//========================================================================================
		// OnAfterDeselect()
		//========================================================================================

		/// <summary>
		/// Invoked when a node is deselected.
		/// <para>
		/// Inheritors that override this method must call the base version to ensure
		/// that event subscribers are notified accordingly.
		/// </para>
		/// </summary>
		/// <param name="e">The data associated with this event.</param>

		protected virtual void OnAfterDeselect (TreeViewEventArgs e)
		{
			if (AfterDeselect != null)
			{
				AfterDeselect(this, new TreeViewEventArgs(e.Node));
			}
		}


		//========================================================================================
		// OnKeyDown()
		//========================================================================================

		/// <summary>
		/// Intercepts the KeyDown event to capture the Ctrl+A key sequence which
		/// selects all nodes in the tree view regardless of IsExpanded.
		/// <para>
		/// Inheritors that override this method must call the base version to ensure
		/// that event subscribers are notified accordingly.
		/// </para>
		/// </summary>
		/// <param name="e">The data associated with this event.</param>

		protected override void OnKeyDown (KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (multiSelect)
			{
				if (e.Control && (e.KeyCode == Keys.A))
				{
					DeselectAllNodes();
					SelectAllNodes();
				}
			}
		}


		//========================================================================================
		// OnMouseDown()
		//========================================================================================

		/// <summary>
		/// Extends the default mouse down handling to recognize right-clicks and
		/// force the selection of the node that was just clicked, since the native
		/// TreeView does not automatically do this.
		/// <para>
		/// Inheritors that override this method must call the base version to ensure
		/// that event subscribers are notified accordingly.
		/// </para>
		/// </summary>
		/// <param name="e">The data associated with the event.</param>

		protected override void OnMouseDown (MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// TreeView doesn't assume focus when you return to the control with a
				// left click but we want that functionality so force it here. 

				this.Focus();
			}
			else if (e.Button == MouseButtons.Right)
			{
				// TreeView doesn't change focus to a node that is right-clicked
				// but we want that functionality so force it here.

				TreeNode node = this.GetNodeAt(e.X, e.Y);
				if (node != null)
				{
					this.SelectedNode = node;
				}
			}

			base.OnMouseDown(e);
		}


		//========================================================================================
		// WndProc()
		//========================================================================================

		/// <summary>
		/// Intercepts Windows messages, filtering out the ERASE_BACKGROUND message
		/// to eliminate flicker in the tree view when resizing.
		/// <para>
		/// Inheritors that override this method must call the base version to ensure
		/// that event subscribers are notified accordingly.
		/// </para>
		/// </summary>
		/// <param name="messg">The windows message.</param>

		protected override void WndProc (ref Message messg)
		{
			if (messg.Msg == (int)0x0014)			// if ERASE_BACKGROUND
			{
				messg.Msg = (int)0x0000;			// reset message to null
			}

			base.WndProc(ref messg);
		}


		#region This future code attempts to deselect the this.SelectedNode when Cntrl-clicked.

		//protected override void OnNodeMouseClick (TreeNodeMouseClickEventArgs e)
		//{
		//    if (multiSelect)
		//    {
		//        Debug.WriteLine("Clicked [" + e.Node.FullPath + "]");

		//        // recognize Cntrl regardless of other modifiers
		//        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
		//        {
		//            if (e.Node == this.SelectedNode)
		//            {
		//                Debug.WriteLine("DrawNode BackColor=[" + e.Node.BackColor + "]");
		//                if (e.Node.BackColor == SystemColors.Highlight)
		//                {
		//                    e.Node.BackColor = SystemColors.Window;
		//                    this.Invalidate(e.Node.Bounds);
		//                }
		//                else
		//                {
		//                    e.Node.BackColor = SystemColors.Highlight;
		//                    this.Invalidate(e.Node.Bounds);
		//                }
		//            }
		//        }
		//        else if ((Control.ModifierKeys & Keys.Shift) == 0)
		//        {
		//            DeselectAllNodes();
		//        }
		//    }
		//}


		//protected override void OnDrawNode (DrawTreeNodeEventArgs e)
		//{
		//    e.DrawDefault = true;
		//    base.OnDrawNode(e);

		//    if ((e.State == TreeNodeStates.Selected) && (e.Node.BackColor == this.BackColor))
		//    {
		//        e.DrawDefault = false;
		//        Brush brush = (e.Node.IsSelected ? SystemBrushes.Highlight : Brushes.Red);// new SolidBrush(e.Node.BackColor));
		//        e.Graphics.FillRectangle(brush, e.Node.Bounds);

		//        Font font = (e.Node.NodeFont != null ? e.Node.NodeFont : this.Font);

		//        Color color = (e.Node.IsSelected && this.Focused
		//            ? SystemColors.HighlightText
		//            : (e.Node.ForeColor != Color.Empty ? e.Node.ForeColor : this.ForeColor));

		//        TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, color, TextFormatFlags.GlyphOverhangPadding);
		//    }
		//}

		#endregion
	}
}
