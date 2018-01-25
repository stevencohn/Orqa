//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Presents an entry dialog box used to set the values of parameters passed to
// and from a stored procedure.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using Oracle.ManagedDataAccess.Types;
	using River.Orqa.Browser;
	using River.Orqa.Database;


	//********************************************************************************************
	// class ParametersDialog
	//********************************************************************************************

	/// <summary>
	/// Presents an entry dialog box used to set the values of parameters passed to
	/// and from a stored procedure.
	/// </summary>

	internal partial class ParametersDialog : Form
	{
		private const int DirectionColumn = 0;
		private const int NameColumn = 1;
		private const int DataTypeColumn = 2;
		private const int ValueColumn = 3;
		private const int NullableColumn = 4;

		private SchemataProcedure node;
		private ParameterCollection parameters;
		//private EventHandler cellChangedHandler;
		bool accepted = false;						// true when OK is clicked


		//========================================================================================
		// Constructors
		//========================================================================================
		
		public ParametersDialog ()
		{
			InitializeComponent();

			parameterGrid.AllowUserToOrderColumns = false;
		}


		public ParametersDialog (SchemataProcedure node)
			: this()
		{
			this.node = node;

			if (node.Parent is SchemataPackage)
				titleNameLabel.Text = node.SchemaName + "." + node.Parent.Text + "." + node.Text;
			else
				titleNameLabel.Text = node.SchemaName + "." + node.Text;

			parameterGrid.Tag = node;
		}


		protected override void  OnLoad (EventArgs e)
		{
			parameterGrid.DataSource = bindingSource;

			parameters = new ParameterCollection();
			foreach (SchemataParameter parameter in node.Nodes)
			{
				parameters.Add(new Parameter(parameter));
			}

			bindingSource.DataSource = parameters;

			int firstEditableRow = -1;

			// set the datatype for each row input field

			foreach (DataGridViewRow row in parameterGrid.Rows)
			{
				Parameter parameter = parameters[row.Index];

				if ((parameter.Direction & ParameterDirection.Input) == ParameterDirection.Input)
				{
					row.Cells[ValueColumn].ValueType = parameter.Type;

					if (firstEditableRow < 0)
						firstEditableRow = row.Index;
				}
				else
				{
					row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
					row.Cells[ValueColumn].Value = "<" + parameter.Direction.ToString() + ">";
					row.ReadOnly = true;
				}
			}

			okButton.Enabled = false;

			parameterGrid.CellValueChanged += new DataGridViewCellEventHandler(DoCellValueChanged);

			if (firstEditableRow < 0)
			{
				firstEditableRow = 0;
				okButton.Enabled = true;
			}

			parameterGrid.Focus();

			if (parameterGrid.Rows.Count > 0)
			{
				parameterGrid.CurrentCell = parameterGrid.Rows[firstEditableRow].Cells[ValueColumn];

				//cellChangedHandler = new EventHandler(DoCurrentCellChanged);
				//parameterGrid.CurrentCellChanged += cellChangedHandler;

				parameterGrid.BeginEdit(false);
			}
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public ParameterCollection Parameters
		{
			get { return (ParameterCollection)bindingSource.DataSource; }
		}


		public string ProcedureName
		{
			get { return titleNameLabel.Text; }
		}


		//========================================================================================
		// DoCellFormatting()
		//========================================================================================

		private void DoCellFormatting (object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == DirectionColumn)
			{
				ParameterDirection direction = (ParameterDirection)e.Value;

				switch (direction)
				{
					case ParameterDirection.Input:
					case ParameterDirection.InputOutput:
						e.Value = directionImages.Images[0];
						break;

					case ParameterDirection.Output:
						e.Value = directionImages.Images[2];
						break;

					case ParameterDirection.ReturnValue:
						e.Value = directionImages.Images[3];
						break;
				}

				e.FormattingApplied = true;
			}
		}


		//========================================================================================
		// DoCellValueChanged()
		//========================================================================================

		private void DoCellValueChanged (object sender, DataGridViewCellEventArgs e)
		{
			if (errorPanel.Visible)
				errorPanel.Visible = false;

			DataGridViewRow row = parameterGrid.Rows[e.RowIndex];
			bool updated = false;

			if (e.ColumnIndex == ValueColumn)
			{
				string value = row.Cells[ValueColumn].FormattedValue.ToString().Trim();
				if ((value.Length == 0) && ((bool)row.Cells[NullableColumn].FormattedValue == false))
				{
					okButton.Enabled = false;
					updated = true;
				}
			}
			else if (e.ColumnIndex == NullableColumn)
			{
				bool @checked = (bool)row.Cells[NullableColumn].FormattedValue;
				if (@checked)
				{
					row.Cells[ValueColumn].Value = null;
				}
				else
				{
					string value = ((string)row.Cells[ValueColumn].FormattedValue).ToString().Trim();
					if (value.Length == 0)
					{
						okButton.Enabled = false;
						updated = true;
					}
				}
			}

			if (!updated)
				UpdateReadyState();
		}


		private void UpdateReadyState ()
		{
			Parameter parameter;
			DataGridViewRow row;
			bool foundEmpty = false;					// true when found empty row!
			int i = 0;

			while ((i < parameterGrid.Rows.Count) && !foundEmpty)
			{
				parameter = parameters[i];
				if ((parameter.Direction == ParameterDirection.Input) ||
					(parameter.Direction == ParameterDirection.InputOutput))
				{
					row = parameterGrid.Rows[i];
					if ((bool)((DataGridViewCheckBoxCell)row.Cells[NullableColumn]).FormattedValue == false)
					{
						string value = row.Cells[ValueColumn].FormattedValue.ToString().Trim();
						foundEmpty = (value.Length == 0);
					}
				}

				i++;
			}

			okButton.Enabled = !foundEmpty;
		}


		//========================================================================================
		// DoAccept()
		//========================================================================================
		
		private void DoAccept (object sender, EventArgs e)
		{
			accepted = true;
			this.Close();
		}

		private void DoCancel (object sender, EventArgs e)
		{
			accepted = false;
			parameters = null;
			this.Close();
		}

		private void DoFormClosing (object sender, FormClosingEventArgs e)
		{
			// TODO: check e.CloseReason...

			if (!accepted)
				return;

			bool empty = false;
			int i = 0;

			parameters = (ParameterCollection)bindingSource.DataSource;
			Parameter parameter = null;

			while ((i < parameters.Count) && !empty)
			{
				parameter = parameters[i];

				if ((parameter.Direction & ParameterDirection.Input) == ParameterDirection.Input)
				{
					if (!(empty = (parameter.Value == null)))
						i++;
				}
				else
					i++;
			}

			if (empty)
			{
				MessageBox.Show(
					"Missing value for parameter " + parameter.Name,
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
					);

				parameterGrid.Rows[i].Selected = true;

				e.Cancel = true;
			}
		}


		private void DoDataError (object sender, DataGridViewDataErrorEventArgs e)
		{
			errorPanel.Visible = true;
			e.Cancel = true;
		}


		//private void DoCurrentCellChanged (object sender, EventArgs e)
		//{
		//    DataGridViewCell cell = parameterGrid.CurrentCell;
		//    if ((cell.ColumnIndex != ValueColumn) && (cell.ColumnIndex != NullableColumn))
		//    {
		//        parameterGrid.CurrentCellChanged -= cellChangedHandler;
		//        parameterGrid.CurrentCell = parameterGrid.Rows[cell.RowIndex].Cells[ValueColumn];
		//        parameterGrid.CurrentCellChanged += cellChangedHandler;
		//    }
		//}
	}
}