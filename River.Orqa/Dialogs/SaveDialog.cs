//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Save collection dialog box.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using River.Orqa.Query;


	//********************************************************************************************
	// class SaveDialog
	//********************************************************************************************

	internal partial class SaveDialog : Form
	{
		private ArrayList items;
		Control focus;


		//========================================================================================
		// Constructor
		//========================================================================================

		public SaveDialog ()
		{
			InitializeComponent();

			focus = yesButton;
			itemBox.GotFocus += new EventHandler(DoItemBoxGotFocus);
			yesButton.GotFocus += new EventHandler(DoGotFocus);
			noButton.GotFocus += new EventHandler(DoGotFocus);
			cancelButton.GotFocus += new EventHandler(DoGotFocus);

			// remove default TextBox context menu
			itemBox.ContextMenu = new ContextMenu();

			items = new ArrayList();
		}


		//========================================================================================
		// Handlers
		//========================================================================================

		private void DoGotFocus (object sender, EventArgs e)
		{
			focus = (Control)sender;
		}


		private void DoItemBoxGotFocus (object sender, EventArgs e)
		{
			focus.Focus();
		}


		private void DoFormClosing (object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.None)
			{
				this.DialogResult = DialogResult.Cancel;
			}
		}


		//========================================================================================
		// Interface
		//========================================================================================

		public QueryWindow[] UnsavedItems
		{
			get
			{
				QueryWindow[] children = new QueryWindow[items.Count];
				for (int i = 0; i < items.Count; i++)
				{
					children[i] = (QueryWindow)items[i];
				}

				return children;
			}
		}


		public void AddItem (QueryWindow child)
		{
			items.Add(child);

			string[] lines = new string[items.Count];
			string line;
			QueryWindow item;

			for (int i = 0; i < items.Count; i++)
			{
				item = (QueryWindow)items[i];

				if (item.Filename == null)
				{
					line = item.Text;
					if (line[line.Length - 1] == '*')
						line = line.Substring(0, line.Length - 2);

					lines[i] = line;
				}
				else
				{
					lines[i] = item.Filename;
				}
			}

			itemBox.Lines = lines;
			itemBox.SelectionLength = 0;
		}


		public void AddUnsavedItems (Form[] forms)
		{
			QueryWindow child;
			foreach (Form form in forms)
			{
				child = (QueryWindow)form;
				if (!child.IsSaved)
				{
					items.Add(child);
				}
			}

			string line;
			string[] lines = new string[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				child = (QueryWindow)items[i];

				if (child.Filename == null)
				{
					line = child.Text;
					if (line[line.Length - 1] == '*')
						line = line.Substring(0, line.Length - 2);

					lines[i] = line;
				}
				else
				{
					lines[i] = child.Filename;
				}
			}

			itemBox.Lines = lines;
			itemBox.SelectionLength = 0;
		}
	}
}