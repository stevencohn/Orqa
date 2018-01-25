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
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.IO;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class ProjectItem
	//********************************************************************************************

	#region class DirectoryItem

	internal class DirectoryItem : ProjectItem
	{
		public DirectoryItem (string path)
			: base(path)
		{
			try
			{
				this.info = new DirectoryInfo(path);
			}
			catch (FileNotFoundException) { }

			PopulateProperties();
		}


		public override bool IsDirectory
		{
			get { return true; }
		}
	}

	#endregion class DirectoryItem

	#region class FileItem

	internal class FileItem : ProjectItem
	{
		public FileItem (string path)
			: base(path)
		{
			try
			{
				this.info = new FileInfo(path);
			}
			catch (FileNotFoundException) { }

			PopulateProperties();
		}

		public override bool IsDirectory
		{
			get { return false; }
		}
	}

	#endregion class FileItem


	internal abstract class ProjectItem : IProjectContainer, IComparable, ICustomTypeDescriptor
	{
		private static string identityCategory;

		protected string path;						// full path
		protected string name;						// display name
		protected FileSystemInfo info;				// file info
		protected ProjectItemCollection items;		// children

		private HybridDictionary properties;		// collected informational properties
		private PropertyDescriptorCollection propertyDescriptions;


		//========================================================================================
		// Constructor
		//========================================================================================

		static ProjectItem ()
		{
			var translator = new Translator("Browser");
			identityCategory = translator.GetString("IdentityCategory");
			translator = null;
		}


		public ProjectItem (string path)
		{
			this.path = path;
			this.name = System.IO.Path.GetFileName(path);
			this.info = null;
			this.items = new ProjectItemCollection();

			this.properties = new HybridDictionary();
			this.propertyDescriptions = null;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public bool Exists
		{
			get { return info.Exists; }
		}


		public string Extension
		{
			get { return info.Extension; }
		}


		public string FolderPath
		{
			get
			{
				if (IsDirectory)
					return info.FullName;
				else
					return System.IO.Path.GetDirectoryName(info.FullName);
			}
		}


		public abstract bool IsDirectory { get; }


		public ProjectItemCollection Items
		{
			get { return items; }
		}


		public string Name
		{
			get
			{
				if (name == null)
					name = info.Name;

				return name;
			}

			set
			{
				name = value;
			}
		}


		public string Path
		{
			get { return path; }
			set { path = value; }
		}


		//========================================================================================
		// Methods
		//========================================================================================

		public int AddChild (ProjectItem item)
		{
			int i = 0;
			bool found = false;

			while ((i < items.Count) && !found)
			{
				if (!(found = (items[i].CompareTo(item) > 0)))
					i++;
			}

			if (i < items.Count)
				items.Insert(i, item);
			else
				items.Add(item);

			return i;
		}


		public void AddProperty (string name, string value)
		{
			properties.Add(name, value);
		}


		public int CompareTo (object other)
		{
			if (!(other is ProjectItem))
				return 0;

			ProjectItem otherItem = (ProjectItem)other;

			if (this.IsDirectory && !otherItem.IsDirectory)
			{
				return -1;
			}
			else if ((!this.IsDirectory) && otherItem.IsDirectory)
			{
				return 1;
			}
			else
			{
				return this.Name.CompareTo(otherItem.Name);
			}
		}


		public void DeleteChild (ProjectItem item)
		{
			items.Remove(item);
		}


		protected void PopulateProperties ()
		{
			AddProperty("Name", name);
			AddProperty("Path", path);
			AddProperty("IsDirectory", IsDirectory.ToString());

			if ((info == null) || !info.Exists)
			{
				AddProperty("Created", String.Empty);
				AddProperty("Exists", "False");
				AddProperty("Last Access", String.Empty);
				AddProperty("Modified", String.Empty);
			}
			else
			{
				AddProperty("Created", info.CreationTime.ToString());
				AddProperty("Exists", info.Exists.ToString());
				AddProperty("Last Access", info.LastAccessTime.ToString());
				AddProperty("Modified", info.LastWriteTime.ToString());
			}
		}


		//========================================================================================
		// ICustomTypeDescriptor implementation
		//========================================================================================

		#region ICustomTypeDescriptor implementation

		public String GetClassName ()
		{
			return null;
		}

		public AttributeCollection GetAttributes ()
		{
			return null;
		}

		public String GetComponentName ()
		{
			return null;
		}

		public TypeConverter GetConverter ()
		{
			return null;
		}

		public EventDescriptor GetDefaultEvent ()
		{
			return null;
		}

		public PropertyDescriptor GetDefaultProperty ()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		public object GetEditor (Type editorBaseType)
		{
			return null;
		}

		public EventDescriptorCollection GetEvents (Attribute[] attributes)
		{
			return null;
		}

		public EventDescriptorCollection GetEvents ()
		{
			return null;
		}

		public object GetPropertyOwner (PropertyDescriptor pd)
		{
			return this;
		}

		public PropertyDescriptorCollection GetProperties (Attribute[] attributes)
		{
			return GetProperties();
		}

		public PropertyDescriptorCollection GetProperties ()
		{
			if (propertyDescriptions == null)
			{
				propertyDescriptions = new PropertyDescriptorCollection(null);

				IDictionaryEnumerator e = properties.GetEnumerator();
				string key;

				while (e.MoveNext())
				{
					key = ((string)e.Key).Trim().ToLower();

					SchemaProperty property;

					if (key.Equals("name"))
						property = new SchemaProperty((string)e.Key, identityCategory);
					else
						property = new SchemaProperty((string)e.Key);

					property.SetValue(null, e.Value);

					propertyDescriptions.Add(property);
				}
			}

			return propertyDescriptions;
		}

		#endregion ICustomTypeDescriptor implementations
	}
}
