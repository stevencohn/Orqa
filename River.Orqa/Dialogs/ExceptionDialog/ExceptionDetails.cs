namespace River.Orqa.Dialogs
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Reflection;
	using System.Windows.Forms;


	internal partial class ExceptionDetails : UserControl
	{
		private Exception root;						// the root exception!

		private bool displayedGeneral = false;
		private bool displayedInner = false;
		private bool displayedStack = false;
		private bool displayedOther = false;


		//========================================================================================
		// Constructor
		//========================================================================================

		public ExceptionDetails ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// ExceptionInstance
		//========================================================================================

		public Exception ExceptionInstance
		{
			set { root = value; }
		}


		//========================================================================================
		// Events and handlers
		//========================================================================================

		/// <summary>
		/// </summary>

		public event EventHandler Abort;

		private void SendAbort (object sender, EventArgs e)
		{
			if (Abort != null)
				Abort(sender, e);
		}


		/// <summary>
		/// </summary>

		public event EventHandler Report;

		private void SendReport (object sender, EventArgs e)
		{
			if (Report != null)
				Report(sender, e);
		}


		#region Event Display

		//========================================================================================
		// DisplayGeneralInformation()
		//========================================================================================

		private void DisplayGeneral ()
		{
			if (!displayedGeneral)
			{
				msgBox.Text = root.Message;
				sourceBox.Text = root.Source;
				targetBox.Text = GetTargetMethodFormat(root);
				helpBox.Text = root.HelpLink;

				displayedGeneral = true;
			}
		}


		private string GetTargetMethodFormat (Exception exc)
		{
			if (exc.TargetSite == null)
				return String.Empty;

			return "["
				+ exc.TargetSite.DeclaringType.Assembly.GetName().Name + "]"
				+ exc.TargetSite.DeclaringType + "::"
				+ exc.TargetSite.Name + "()"
				;
		}


		//========================================================================================
		// DisplayInner()
		//========================================================================================

		private void DisplayInner ()
		{
			if (!displayedInner)
			{
				TreeNode parent = null;
				TreeNode child = null;
				Exception exc = root;

				innerTree.BeginUpdate();

				while (exc != null)
				{
					child = new TreeNode(exc.GetType().FullName);
					child.Nodes.Add(new TreeNode(exc.Message));
					child.Nodes.Add(new TreeNode(GetTargetMethodFormat(exc)));

					if (parent != null)
						parent.Nodes.Add(child);
					else
						innerTree.Nodes.Add(child);

					parent = child;
					exc = exc.InnerException;
				}

				innerTree.EndUpdate();
				innerTree.ExpandAll();

				displayedInner = true;
			}
		}


		//========================================================================================
		// DisplayStack()
		//========================================================================================

		private void DisplayStack ()
		{
			if (!displayedStack)
			{
				stackBox.Text = root.StackTrace;
				displayedStack = true;
			}
		}


		//========================================================================================
		// DisplayOther()
		//========================================================================================

		private void DisplayOther ()
		{
			if (!displayedOther)
			{
				IDictionaryEnumerator property = GetCustomExceptionInfo(root).GetEnumerator();

				otherList.Items.Clear();
				otherList.BeginUpdate();

				ListViewItem item;

				while (property.MoveNext())
				{
					item = new ListViewItem(property.Key.ToString());

					if (property.Value != null)
						item.SubItems.Add(property.Value.ToString());

					otherList.Items.Add(item);
				}

				otherList.EndUpdate();

				displayedOther = true;
			}
		}


		private Hashtable GetCustomExceptionInfo (Exception exc)
		{
			Hashtable info = new Hashtable();
			Type baseType = typeof(System.Exception);

			foreach (PropertyInfo property in exc.GetType().GetProperties())
			{
				if (baseType.GetProperty(property.Name) == null)
					info.Add(property.Name, property.GetValue(exc, null));
			}

			return info;
		}


		//========================================================================================
		// Event overrides and handlers
		//========================================================================================

		protected override void OnLoad (EventArgs e)
		{
			if (root != null)
			{
				titleBox.Text = root.GetType().FullName;
				DisplayGeneral();
			}
			else
			{
				tabset.Enabled = false;
				titleBox.Text = "Unknown";
			}

			base.OnLoad(e);
		}


		private void ChangeTab (object sender, System.EventArgs e)
		{
			switch (tabset.SelectedIndex)
			{
				case 1: DisplayStack(); break;
				case 2: DisplayInner(); break;
				case 3: DisplayOther(); break;
			}
		}


		private void TrackResize (object sender, EventArgs e)
		{
			ListView view = (ListView)sender;

			// simple solution - resize last column only
			// works for both "stack trace" and "other" views

			ColumnHeader col = view.Columns[view.Columns.Count - 1];

			int delta = 0;
			for (int i = 0; i < col.Index; i++)
			{
				delta += view.Columns[i].Width;
			}

			col.Width = view.ClientSize.Width - delta - 15;
		}

		#endregion Event Display
	}
}
