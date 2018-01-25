
namespace River.Orqa.Dialogs
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using River.Orqa.Browser;
	using River.Orqa.Controls;


	//********************************************************************************************
	// class ChangeDbDialog
	//********************************************************************************************

	internal partial class ChangeDbDialog : Form
	{

		//========================================================================================
		// Constructor
		//========================================================================================

		public ChangeDbDialog ()
		{
			InitializeComponent();
		}


		public ChangeDbDialog (object[] schemas)
			: this()
		{
			string active = ((SchemataSchema)schemas[0]).Database.DefaultSchema;

			ListViewItem item;
			foreach (SchemataSchema schema in schemas)
			{
				item = new ListViewItem(new string[]
					{
						schema.SchemaName,
						schema.Database.OraConnection.ServerVersion,
						schema.Database.OraConnection.ConnectionString
					});

				item.Tag = schema;

				schemaList.Items.Add(item);

				if (schema.SchemaName == active)
					item.Selected = true;
			}
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public River.Orqa.Browser.SchemataSchema Schema
		{
			get { return (SchemataSchema)schemaList.SelectedItems[0].Tag; }
		}


		public string SchemaName
		{
			get { return schemaList.SelectedItems[0].Text; }
		}


		//========================================================================================
		// DoResize()
		//========================================================================================
		
		private void DoResize (object sender, EventArgs e)
		{
			// simple solution - resize last column only
			// works for both "stack trace" and "other" views

			ColumnHeader col = schemaList.Columns[schemaList.Columns.Count - 1];

			int delta = 0;
			for (int i = 0; i < col.Index; i++)
			{
				delta += schemaList.Columns[i].Width;
			}

			col.Width = schemaList.ClientSize.Width - delta - 25;
		}


		//========================================================================================
		// DoOK()
		//========================================================================================
		
		private void DoOK (object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}