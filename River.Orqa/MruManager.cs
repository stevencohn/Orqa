namespace River.Orqa
{
	using System;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using River.Orqa.Options;


	//********************************************************************************************
	// Delegate
	//********************************************************************************************

	internal delegate void MruCallback (string filename);


	//********************************************************************************************
	// class MruManager
	//********************************************************************************************

	internal class MruManager
	{
		ToolStripMenuItem fileMenu;					// the menu containing the MRU
		ToolStripMenuItem menu;						// the MRU sub menu
		MruCallback callback;						// MainWindow.OpenRecentFile
		int maxItems;								// the user preferred max item count
		string path;								// path to the User Options folder


		//========================================================================================
		// Constructor
		//========================================================================================

		public MruManager (ToolStripMenuItem fileMenu, MruCallback callback)
		{
			this.fileMenu = fileMenu;
			this.maxItems = UserOptions.GetInt("general/maxMru");

			this.path = Environment.GetFolderPath(
				Environment.SpecialFolder.LocalApplicationData) + '\\' +
				System.Windows.Forms.Application.CompanyName + "\\" +
				System.Windows.Forms.Application.ProductName;

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			path += "\\Mru.orq";

			this.callback = callback;
		}


		//========================================================================================
		// MaxLength
		//========================================================================================

		internal int MaxLength
		{
			set
			{
				maxItems = value;

				while (menu.DropDownItems.Count > maxItems)
				{
					menu.DropDownItems.RemoveAt(menu.DropDownItems.Count - 1);
				}

				if (maxItems == 0)
				{
					fileMenu.DropDownItems.Remove(menu);
					fileMenu.DropDownItems.RemoveAt(fileMenu.DropDownItems.Count - 2);
					menu = null;
				}

				Save();
			}
		}


		//========================================================================================
		// AddFile()
		//========================================================================================

		internal void Add (string filename)
		{
			AddItem(filename);
			Save();
		}


		private void AddItem (string filename)
		{
			if (menu == null)
			{
				menu = new ToolStripMenuItem("Recent &File List");
				fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.Count - 1, menu);
				fileMenu.DropDownItems.Insert(fileMenu.DropDownItems.Count - 1, new ToolStripSeparator());
			}

			ToolStripMenuItem item = FindRecentItem(filename);

			if (item == null)
			{
				// add new recent file
				item = new ToolStripMenuItem(filename);
				item.Tag = filename; // needed to the for loop right below...
				item.Click += new EventHandler(DoSelectRecentFile);
			}
			else
			{
				// promote to 'most' recent file
				menu.DropDownItems.Remove(item);
			}

			// now adjust all items

			menu.DropDownItems.Insert(0, item);

			for (int i = 0; i < menu.DropDownItems.Count; i++)
			{
				menu.DropDownItems[i].Text = "&" + (i + 1) + "  " + menu.DropDownItems[i].Tag;
			}

			if (menu.DropDownItems.Count > maxItems)
			{
				menu.DropDownItems.RemoveAt(menu.DropDownItems.Count - 1);
			}
		}


		public ToolStripMenuItem FindRecentItem (string filename)
		{
			int i = 0;
			bool found = false;
			while ((i < menu.DropDownItems.Count) && !found)
			{
				if (! (found = (((string)menu.DropDownItems[i].Tag).Equals(filename))))
					i++;
			}

			return (found ? (ToolStripMenuItem)menu.DropDownItems[i] : null);
		}


		//========================================================================================
		// DoSelectRecentFile()
		//========================================================================================

		private void DoSelectRecentFile (object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			callback((string)item.Tag);
		}


		//========================================================================================
		// ClearFiles()
		//========================================================================================

		internal void ClearFiles ()
		{
			menu.DropDownItems.Clear();
			File.Delete(path);
		}


		//========================================================================================
		// Load()
		//========================================================================================

		internal void Load ()
		{
			try
			{
				StreamReader reader = new StreamReader(path);

				string filnam;
				int count = (menu == null ? 0 : menu.DropDownItems.Count);

				while (((filnam = reader.ReadLine()) != null) && (count < maxItems))
				{
					AddItem(filnam);
					count++;
				}

				reader.Close();
				reader = null;
			}
			catch (FileNotFoundException)
			{
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}
		}


		//========================================================================================
		// Save()
		//========================================================================================

		private void Save ()
		{
			if (menu != null)
			{
				StreamWriter writer = new StreamWriter(path, false);

				for (int i = 0; i < menu.DropDownItems.Count; i++)
				{
					writer.WriteLine((string)menu.DropDownItems[i].Tag);
				}

				writer.Close();
				writer = null;
			}
		}
	}
}
