//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Presents a dialog allowing the user to select the type and name of a new item
// to add to a project folder.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 08-Nov-2005      New
//************************************************************************************************

namespace River.Orqa.Dialogs
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using System.Xml.XPath;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class NewItemDialog
	//********************************************************************************************

	internal partial class NewItemDialog : Form
	{
		private Translator translator;
		private ItemType selectedItem;

		#region class ItemType

		private class ItemType
		{
			public string IconName;
			public string Name;
			public string Description;
			public string Filename;
			public string ResID;

			public ItemType (XPathNavigator nav)
			{
				nav.MoveToFirstChild();
				this.IconName = nav.Value;

				nav.MoveToNext();
				this.Name = nav.Value;

				nav.MoveToNext();
				this.Description = nav.Value;

				nav.MoveToNext();
				this.Filename = nav.Value;

				nav.MoveToNext();
				this.ResID = nav.Value;

				nav.MoveToParent();
			}
		}

		#endregion class ItemType


		//========================================================================================
		// Constructor
		//========================================================================================

		public NewItemDialog ()
		{
			InitializeComponent();

			templatesView.Columns[0].Width = (int)(templatesView.ClientSize.Width / 3.5);

			translator = null;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the temporary file name of the selected template or <b>null</b> if
		/// no template was selected.
		/// </summary>

		public string FileName
		{
			get
			{
				return (selectedItem == null ? null : selectedItem.Filename);
			}
		}


		/// <summary>
		/// Gets the content of the selected template or <b>null</b> if no template
		/// was selected.
		/// </summary>

		public string TemplateContent
		{
			get
			{
				if (selectedItem == null)
					return null;

				// translator must have been instantiated by now!
				translator = new Translator("Dialogs.NewItems");

				string content = translator.GetString(selectedItem.ResID);
				return content;
			}
		}


		//========================================================================================
		// SelectItem()
		//========================================================================================

		/// <summary>
		/// Select the item identified by the specified key.
		/// </summary>
		/// <param name="key">A string matching the key of the item to select.</param>

		public void SelectItem (string key)
		{
			templatesView.Items[key].Selected = true;
		}


		//========================================================================================
		// SetTemplateConfiguration()
		//========================================================================================

		/// <summary>
		/// Given a string specifying an XML description of available templates, populates
		/// the dialog with the template list.  This list will be presented to the user
		/// to select the appropriate template type and file name for a new item.
		/// </summary>
		/// <param name="config">A string specifying the XML description of available templates.</param>

		public void SetTemplateConfiguration (string config)
		{
			if (translator == null)
			{
				translator = new Translator("Dialogs.NewItems");
			}

			templatesView.Items.Clear();
			images.Images.Clear();

			var doc = new XPathDocument(new StringReader(config));
			XPathNavigator nav = doc.CreateNavigator();
			XPathNodeIterator iterator = nav.Select("templates/template");
			int i = 0;

			while (iterator.MoveNext())
			{
				var itemType = new ItemType(iterator.Current);

				var item = new ListViewItem(itemType.Name, i);
				item.Name = itemType.Name;
				item.Group = templatesView.Groups[0];

				Image image = translator.GetBitmap(itemType.IconName);
				images.Images.Add(image);

				item.Tag = itemType;

				templatesView.Items.Add(item);
				i++;
			}
		}


		//========================================================================================
		// Handlers
		//========================================================================================
		
		private void DoAccept (object sender, EventArgs e)
		{
			if (templatesView.SelectedItems.Count > 0)
			{
				selectedItem = (ItemType)templatesView.SelectedItems[0].Tag;
			}
			else
			{
				selectedItem = null;
			}

			this.Close();
		}


		private void DoCancel (object sender, EventArgs e)
		{
			selectedItem = null;
			this.Close();
		}


		private void DoSelectedTemplate (object sender, EventArgs e)
		{
			if (templatesView.SelectedItems.Count > 0)
			{
				ListViewItem item = templatesView.SelectedItems[0];
				ItemType itemtype = (ItemType)item.Tag;

				descriptionBox.Text = itemtype.Description;
				nameBox.Text = itemtype.Filename;
			}
		}
	}
}