//************************************************************************************************
// Copyright © 2002-2005 Steven M. Cohn. All Rights Reserved.
//
// Enhances the standards Windows DataGridView control, restricting when the
// context menu is displayed (only over content cells) and forces cell selection
// with right-mouse clicks.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Controls
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;


	//********************************************************************************************
	// class RiverDataGridView
	//********************************************************************************************

	/// <summary>
	/// Enhances the standards Windows DataGridView control, restricting when the
	/// context menu is displayed (only over content cells) and forces cell selection
	/// with right-mouse clicks.
	/// </summary>

	public class RiverDataGridView : DataGridView
	{
		private int defaultSelectedColumnIndex;			// preferred selection column


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initialize a new flicker-free ListView control.
		/// </summary>

		public RiverDataGridView ()
		{
			this.AllowUserToOrderColumns = true;
			this.AllowUserToResizeRows = false;
			this.AutoGenerateColumns = false;
			this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.ColumnHeadersHeight = 21;
			this.GridColor = System.Drawing.SystemColors.ControlLight;
			this.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.RowHeadersVisible = false;
			this.RowHeadersWidth = 21;
			this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;

			this.ContextMenuStripChanged += new EventHandler(DoContextMenuStripChanged);

			defaultSelectedColumnIndex = 0;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets or sets the index of the preferred column to select when a row is
		/// right-clicked.
		/// <para>
		/// If set to a value between zero and the last column index then the
		/// specified cell will be selected and activated (gain focus).
		/// </para>
		/// <para>
		/// If set to -1 then the entire row will be selected and focus is set to
		/// the clicked cell.
		/// </para>
		/// <para>
		/// The default value is zero which selects the cell in the first column.
		/// </para>
		/// </summary>

		public int DefaultSelectedColumnIndex
		{
			get
			{
				return defaultSelectedColumnIndex;
			}

			set
			{
				if ((value < -1) || (value > this.Columns.Count - 1))
				{
					throw new ArgumentOutOfRangeException();
				}

				defaultSelectedColumnIndex = value;
			}
		}


		//========================================================================================
		// Context menu control
		//========================================================================================

		/// <summary>
		/// Intercepts the ContextMenuStrip Changed event and binds a custom event
		/// handler to control when the context menu is displayed.
		/// </summary>
		/// <param name="sender">The source control sending the event.</param>
		/// <param name="e">The data associated with the event.</param>

		private void DoContextMenuStripChanged (object sender, EventArgs e)
		{
			if (this.ContextMenuStrip != null)
			{
				this.ContextMenuStrip.Opening += new CancelEventHandler(DoContextMenuStripOpening);
			}
		}


		/// <summary>
		/// Bound to the current ContextMenuStrip, this handler intercepts the Opening
		/// event and suppresses the context menu if the mouse is not currently over
		/// a content cell in the grid.
		/// </summary>
		/// <param name="sender">The source control sending the event.</param>
		/// <param name="e">The data associated with the event.</param>

		private void DoContextMenuStripOpening (object sender, CancelEventArgs e)
		{
			Point p = this.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
			DataGridView.HitTestInfo hit = this.HitTest(p.X, p.Y);

			if ((hit.ColumnIndex < 0) || (hit.RowIndex < 0))
			{
				// cancel the popup
				e.Cancel = true;
			}
		}


		//========================================================================================
		// Overrides
		//========================================================================================

		/// <summary>
		/// The default behavior of DataGridView does not change the row/cell selection
		/// upon right mouse clicks.  This handler intercepts the CellMouseDown event and
		/// forces the selection of the item pointed to by the mouse upon a right mouse click.
		/// </summary>
		/// <param name="e">The data associated with the event.</param>

		protected override void OnCellMouseDown (DataGridViewCellMouseEventArgs e)
		{
			if ((e.RowIndex < 0) || (e.ColumnIndex < 0))
			{
				// ignore if the mouse isn't over a content cell
				return;
			}

			if (e.Button == MouseButtons.Right)
			{
				if ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) == 0)
				{
					// clear selected rows
					IEnumerator rows = this.SelectedRows.GetEnumerator();
					while (rows.MoveNext())
					{
						((DataGridViewRow)rows.Current).Selected = false;
					}

					// clear selected columns
					IEnumerator cells = this.SelectedCells.GetEnumerator();
					while (cells.MoveNext())
					{
						((DataGridViewCell)cells.Current).Selected = false;
					}
				}

				// if current row already has focus, no need to change it
				if (this.CurrentCell.RowIndex != e.RowIndex)
				{
					// select and set focus to the target cell
					if (defaultSelectedColumnIndex < 0)
					{
						this.Rows[e.RowIndex].Selected = true;
						this.CurrentCell = this.Rows[e.RowIndex].Cells[e.ColumnIndex];
					}
					else
					{
						this.Rows[e.RowIndex].Cells[defaultSelectedColumnIndex].Selected = true;
						this.CurrentCell = this.Rows[e.RowIndex].Cells[defaultSelectedColumnIndex];
					}
				}
			}

			base.OnCellMouseDown(e);
		}
	}
}
