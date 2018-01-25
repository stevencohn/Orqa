//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Base class for nodes types in the main tree view.  Inheritors encapsulate
// all available functionality for a given object type.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 27-Feb-2003		Separated from Schemata.cs
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Data;
	using System.Threading;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class SchemataNode
	//********************************************************************************************

	internal class SchemataNode : TreeNode, ICustomTypeDescriptor
	{
		protected DatabaseConnection dbase;		// database information
		protected string schemaName;			// this node's parent schema name
		protected string[] restrictions;		// guid-specific restrictions
		protected SchemataNode srvnode;			// this node's top-most server node

		// instrinsics
		protected bool canCompile;				// true if object can be compiled
		protected bool canDelete;				// true if object can be deleted
		protected bool canDescribe;				// true if object can be described (DDL)
		protected bool canEdit;					// true if object can be edited
		protected bool canOpen;					// true if datagrid can be opened
		protected bool canRename;				// true if can rename object
		protected bool canRefresh;				// true if node can be refreshed
		protected bool canDrag;					// true if can drag name to text editor
		protected bool hasDetails;				// true if object has displayable details
		protected bool hasDiscovery;			// true if node has discovery feature
		protected bool isDiscovered;			// true if node previously discovered
		protected bool isWrapped;				// true if source is wrapped (obfuscated)

		private HybridDictionary properties;	// collected informational properties
		private PropertyDescriptorCollection propertyDescriptions;
		protected static Translator translator;
		private static string identityCategory;
		private static string folderTooltip;

		protected BackgroundWorker worker;
		protected List<TreeNode> discoveries;


		//========================================================================================
		// Constructor
		//========================================================================================

		static SchemataNode ()
		{
			Translator translator = new Translator("Browser");
			identityCategory = translator.GetString("IdentityCategory");
			folderTooltip = translator.GetString("FolderTooltip");
			translator = null;
		}


		protected SchemataNode ()
		{
			this.dbase = null;
			this.schemaName = null;
			this.restrictions = null;
			this.srvnode = null;
			this.properties = new HybridDictionary();
			this.propertyDescriptions = null;

			this.canCompile = false;
			this.canDelete = false;
			this.canDescribe = false;
			this.canEdit = false;
			this.canOpen = false;
			this.canRefresh = false;
			this.canRename = false;
			this.canDrag = false;
			this.hasDetails = false;
			this.hasDiscovery = false;
			this.isDiscovered = false;
			this.isWrapped = false;

			translator = new Translator("Browser");
		}


		internal virtual bool CanCompile { get { return canCompile; } }
		internal virtual bool CanDelete { get { return canDelete; } }
		internal virtual bool CanDescribe { get { return canDescribe; } }
		internal virtual bool CanEdit { get { return canEdit; } }
		internal virtual bool CanOpen { get { return canOpen; } }
		internal virtual bool CanRefresh { get { return canRefresh; } }
		internal virtual bool CanRename { get { return canRename; } }
		internal virtual bool CanDrag { get { return canDrag; } }

		internal virtual bool HasDetails { get { return hasDetails; } }
		internal virtual bool HasDiscovery { get { return hasDiscovery; } }

		internal DatabaseConnection DatabaseConnection
		{
			get { return dbase; }
		}

		internal bool IsDiscovered
		{
			get { return isDiscovered; }
			set { isDiscovered = value; }
		}

		internal bool IsWrapped
		{
			get { return isWrapped; }
			set { isWrapped = value; }
		}

		internal object[] Restrictions
		{
			get { return restrictions; }
		}

		internal string SchemaName
		{
			get { return schemaName; }
		}

		internal SchemataNode ServerNode
		{
			get { return srvnode; }
		}


		//========================================================================================
		// Methods
		//========================================================================================

		internal void AddProperty (string name, string value)
		{
			properties.Add(name, value);
		}


		//========================================================================================
		// CollectProperties()
		//========================================================================================

		internal virtual void CollectProperties (DataColumnCollection cols, DataRow row)
		{
			foreach (DataColumn col in cols)
			{
				if (row[col.ColumnName] != DBNull.Value)
				{
					if (!properties.Contains(col.ColumnName))
					{
						properties.Add(col.ColumnName, row[col.ColumnName].ToString());
					}
				}
			}
		}


		//========================================================================================
		// Compile()
		//		If an inheritor allows compilation (CanCompile == true), then the inheritor
		//		should override this method to compile itself.
		//========================================================================================

		internal virtual void Compile (River.Orqa.Query.QueryWindow window)
		{
		}


		//========================================================================================
		// Delete()
		//		If an inheritor allows deletion (CanDelete == true), then the inheritor
		//		should override this method to delete itself.
		//========================================================================================

		internal virtual bool Delete ()
		{
			return true;
		}


		//========================================================================================
		// Describe()
		//========================================================================================

		protected void Describe (
			string type, string name, string owner, River.Orqa.Query.QueryWindow window)
		{
			window.SetStatusMessage("Generating DDL...");

			worker = new BackgroundWorker();
			worker.DoWork += new DoWorkEventHandler(DoDescribeWork);
			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoDescribeCompleted);
			worker.RunWorkerAsync(new object[] { type, name, owner, window });
		}

		private void DoDescribeWork (object sender, DoWorkEventArgs e)
		{
			object[] args = (object[])e.Argument;
			string type = (string)args[0];
			string name = (string)args[1];
			string owner = (string)args[2];
			River.Orqa.Query.QueryWindow window = (River.Orqa.Query.QueryWindow)args[3];

			string sql =
				"SELECT dbms_metadata.get_ddl('" + type
				+ "','" + name.ToUpper() + "','" + owner + "') AS ddl"
				+ " FROM dual";

			Logger.WriteSection("Describe");
			Logger.WriteLine(sql);

			var cmd = new OracleCommand(sql);

			try
			{
				cmd.Connection = dbase.OraConnection;

				OracleDataReader reader = cmd.ExecuteReader();

				if (reader.FieldCount > 0)
				{
					if (reader.Read())
					{
						string ddl = reader.GetString(0).Trim();
						e.Result = new object[] { ddl, owner, name, window };
					}
				}

				reader.Close();
				reader.Dispose();
				reader = null;
			}
			catch (Exception exc)
			{
				DialogResult result = MessageBox.Show(null,
					"To fix, try invoking $ORACLE_HOME/rdbms/admin/initmeta.sql.\n\n" +
					"Would you like to view Exception information now?", "initmeta",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
				{
					Dialogs.ExceptionDialog.ShowException(exc);
				}
			}

			cmd.Dispose();
			cmd = null;
		}


		private void DoDescribeCompleted (object sender, RunWorkerCompletedEventArgs e)
		{
			object[] args = (object[])e.Result;
			string ddl = (string)args[0];
			string owner = (string)args[1];
			string name = (string)args[2];
			River.Orqa.Query.QueryWindow window = (River.Orqa.Query.QueryWindow)args[3];

			window.InsertText(ddl);
			window.IsSaved = true;
			window.SetTitle(owner + "." + name);
			window.SetStatusMessage("Ready");
		}


		//========================================================================================
		// Discover()
		//		If an inheritor allows discoveries (HasDiscovery == true), then the inheritor
		//		should override this method to propulate its Nodes collections.
		//========================================================================================

		/// <summary>
		/// Asynchronously discovers the contents of the current node.
		/// </summary>

		internal virtual void Discover ()
		{
			// inheritors should set status line and then call base.Discover()

			if (!isDiscovered)
			{
				discoveries = new List<TreeNode>();

				worker = new BackgroundWorker();
				worker.DoWork += new DoWorkEventHandler(DoDiscoverWork);
				worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoDiscoverCompleted);
				worker.RunWorkerAsync(this);
			}
		}


		/// <summary>
		/// Synchronously discovers the contents of the current node.
		/// </summary>

		public virtual void DiscoverNow ()
		{
			if (!isDiscovered)
			{
				discoveries = new List<TreeNode>();
				DoDiscoverWork(null, null);
				DoDiscoverCompleted(null, new RunWorkerCompletedEventArgs(null, null, false));
			}
		}


		protected virtual void DoDiscoverWork (object sender, DoWorkEventArgs args)
		{
			// must implement
		}


		protected virtual void DoDiscoverCompleted (object sender, RunWorkerCompletedEventArgs e)
		{
			if ((e.Error == null) && !e.Cancelled)
			{
				if (!isDiscovered)
				{
					this.Nodes.Clear();

					if (discoveries.Count > 0)
						this.Nodes.AddRange(discoveries.ToArray());
					else
						this.ImageIndex = this.SelectedImageIndex = SchemaIcons.FolderClose;

					if (folderTooltip != null)
						this.ToolTipText = String.Format(folderTooltip, this.Nodes.Count);

					isDiscovered = true;
				}

				discoveries = null;

				if (worker != null) // if DiscoverNow...
				{
					worker.Dispose();
					worker = null;
				}

				this.Expand();
				Statusbar.Message = String.Empty;
			}
		}


		//========================================================================================
		// DoDefaultAction()
		//		Invokes the default action for this schemata node item.
		//========================================================================================

		internal virtual void DoDefaultAction ()
		{
		}


		//========================================================================================
		// Edit()
		//		If an inheritor allowed editing (CanEdit == true), then the inheritor
		//		should override this method to edit its content.
		//========================================================================================

		internal virtual void Edit (River.Orqa.Query.QueryWindow window)
		{
		}


		//========================================================================================
		// Refresh()
		//		This is a handler that is called by the BeforeExpand event of the TreeView.
		//		Inheritors should ip
		//========================================================================================

		internal void Refresh ()
		{
			if (hasDiscovery)
			{
				isDiscovered = false;
				Discover();
			}
		}


		//========================================================================================
		// Rename()
		//		If an inheritor allows renaming (CanRename == true), then the inheritor
		//		should override this method to rename its content.
		//========================================================================================

		internal virtual void Rename (string name)
		{
			string sql = "RENAME " + this.Text + " TO " + name;

			try
			{
				OracleCommand cmd = new OracleCommand(sql);
				cmd.Connection = dbase.OraConnection;

				cmd.ExecuteNonQuery();

				this.Text = name.ToUpper();
			}
			catch (Exception exc)
			{
				Dialogs.ExceptionDialog.ShowException(exc);
			}
		}


		//========================================================================================
		// ShowDetail()
		//		If an inheritor allows detail view (HasDetail == true), then the inheritor
		//		should override this method to present a detail window.
		//========================================================================================

		internal virtual void ShowDetail ()
		{
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
