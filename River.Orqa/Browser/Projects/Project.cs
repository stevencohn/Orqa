//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Manages a Visual Studio Databse Project file commonly having the extension ".dbp"
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 10-Nov-2005		New
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Text;


	//********************************************************************************************
	// class Project
	//********************************************************************************************

	internal class Project : IProjectContainer
	{
		private static readonly string CR = System.Environment.NewLine;

		private string path;						// full path
		private string name;						// display name
		private string version;						// dbp file version
		private string dbref;						// DBRefFolder content
		private ProjectItem root;					// the project item


		//========================================================================================
		// Constructor
		//========================================================================================

		public Project (string path)
		{
			this.path = path;
			this.name = Path.GetFileName(path);
			this.version = null;
			this.dbref = null;
			this.root = new FileItem(path);
		}


		//========================================================================================
		// Properties
		//========================================================================================

		internal string DBRef
		{
			set { dbref = value; }
		}


		public string Name
		{
			get { return name; }
			set { name = value; }
		}


		public string FolderPath
		{
			get { return Path.GetDirectoryName(root.Path); }
		}


		public ProjectItem Root
		{
			get { return root; }
		}


		public string Version
		{
			get { return version; }
			set { version = value; }
		}


		//========================================================================================
		// Methods
		//========================================================================================

		public int AddChild (ProjectItem item)
		{
			return root.AddChild(item);
		}


		public void DeleteChild (ProjectItem item)
		{
			root.DeleteChild(item);
		}


		public string GenerateProjectFile ()
		{
			StringBuilder buffer = new StringBuilder();
			buffer.Append("# Microsoft Developer Studio Project File - Database Project" + CR);
			buffer.Append("Begin DataProject = \"" + System.IO.Path.GetFileNameWithoutExtension(name) + "\"" + CR);
			buffer.Append("  MSDTVersion = \"" + version + "\"" + CR);

			AppendContent(buffer, root.Items, 1);

			buffer.Append("  Begin DBRefFolder = \"Database References\"" + CR);

			if (dbref != null)
				buffer.Append(dbref);

			buffer.Append("  End" + CR);

			buffer.Append("End" + CR);

			return buffer.ToString();
		}


		private void AppendContent (StringBuilder buffer, ProjectItemCollection items, int count)
		{
			string indent = new string(' ', count * 2);

			ProjectItemCollection.Enumerator e = items.GetEnumerator();
			while (e.MoveNext())
			{
				ProjectItem item = e.Current;

				if (item.IsDirectory)
				{
					buffer.Append(indent + "Begin Folder = \"" + item.Name + "\"" + CR);
					AppendContent(buffer, item.Items, count + 1);
					buffer.Append(indent + "End" + CR);
				}
				else
				{
					if (Path.GetExtension(item.Name).Equals(".sql"))
						buffer.Append(indent + "Script = \"" + item.Name + "\"" + CR);
					else
						buffer.Append(indent + "Node = \"" + item.Name + "\"" + CR);
				}
			}
		}
	}
}
