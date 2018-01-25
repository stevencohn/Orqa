//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// User preferences and program options.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0, extensibility added via SheetBase and XML attributes
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.Collections;
	using System.Drawing;
	using System.IO;
	using System.Reflection;
	using System.Runtime.Remoting;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml.Linq;
	using System.Xml.XPath;
	using River.Orqa;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class OptionsDialog
	//********************************************************************************************

	internal partial class OptionsDialog : Form
	{
		private Hashtable sheets;
		private Hashtable sheetIndex;
		private Translator translator;
		private string activeSheetIndex;
		private string path;

		internal static ArrayList FontList;			// cached/shared list of system fonts


		//========================================================================================
		// Constructors
		//========================================================================================

		static OptionsDialog ()
		{
			FontList = null;
		}


		public OptionsDialog ()
		{
			InitializeComponent();

			sheets = new Hashtable();
			sheetIndex = new Hashtable();
			activeSheetIndex = null;

			path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			if (!path.EndsWith("\\"))
				path += "\\";
		}


		//========================================================================================
		// OnLoad()
		//		Populates treeview.  Must be done here instead of constructor to avoid
		//		Forms bug that always displays the horizontal scroll bar.
		//========================================================================================

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad(e);

			translator = new Translator("Options");

			XPathNavigator nav = UserOptions.Defaults.CreateNavigator();
			nav.MoveToFirstChild();
			BuildNavigationTree(nav.Select("*[@sheet|@ref]"), tree.Nodes);

			tree.ExpandAll();

			if (activeSheetIndex != null)
			{
				TreeNode node = (TreeNode)sheetIndex[activeSheetIndex];
				if (node != null)
				{
					tree.SelectedNode = node;
				}
			}
		}


		//========================================================================================
		// ActiveSheetIndex
		//========================================================================================

		/// <summary>
		/// Used to set the initial sheet selected when the options dialog is displayed.
		/// The value must specify the full XPath of the XML element including "/OrqaOptions"
		/// </summary>

		public string ActiveSheet
		{
			set { activeSheetIndex = value; }
		}


		//========================================================================================
		// BuildNavigationTree()
		//		Generate the tree by recursing the DefaultOptions XML.
		//========================================================================================

		private void BuildNavigationTree (XPathNodeIterator nodes, TreeNodeCollection tree)
		{
			TreeNode treeNode;
			string name;

			while (nodes.MoveNext())
			{
				name = translator.GetString(nodes.Current.GetAttribute("resx", nodes.Current.NamespaceURI));

				treeNode = new TreeNode(name);
				treeNode.ImageIndex = int.Parse(nodes.Current.GetAttribute("icon", nodes.Current.NamespaceURI));
				treeNode.SelectedImageIndex = treeNode.ImageIndex;

				treeNode.Tag = nodes.Current.GetAttribute("sheet", nodes.Current.NamespaceURI);
				if ((string)treeNode.Tag == String.Empty)
				{
					// some parent nodes may reference other nodes.  These parents do not have
					// the sheet attribute but instead have a ref attribute.  The ref attribute
					// specifies the XPath of the referenced node hosting the sheet to use.

					string refName = nodes.Current.GetAttribute("ref", nodes.Current.NamespaceURI);

					XPathNavigator refnode = nodes.Current.CreateNavigator();
					refnode.MoveToRoot();
					refnode.MoveToFirstChild();

					refnode = refnode.SelectSingleNode(refName);

					if (refnode != null)
					{
						treeNode.Tag = refnode.GetAttribute("sheet", nodes.Current.NamespaceURI);
					}
				}

				tree.Add(treeNode);

				sheetIndex.Add(FullPathOf(nodes.Current.CreateNavigator()), treeNode);

				XPathNodeIterator children = nodes.Current.Select("*[@sheet|ref]");

				if ((children != null) && (children.Count > 0))
				{
					BuildNavigationTree(children, treeNode.Nodes);
				}
			}
		}


		private string FullPathOf (XPathNavigator node)
		{
			StringBuilder path = new StringBuilder();
			path.Append(node.Name);

			while (node.MoveToParent())
			{
				path.Insert(0, node.Name + "/");
			}

			return path.ToString();
		}


		//========================================================================================
		// ActivateSheet()
		//========================================================================================

		private void ActivateSheet (object sender, TreeViewEventArgs e)
		{
			SheetBase sheet = null;

			if (sheets.Contains(e.Node.Tag))
			{
				sheet = (SheetBase)sheets[e.Node.Tag];
			}
			else
			{
				StringBuilder title = new StringBuilder();
				string typeName = (string)e.Node.Tag;

				// The open/save dialog boxes reset the current working directory.  Make sure
				// the working directory is preserved after an open/save dialog is invoked.

				try
				{
					if (typeName.IndexOf("ResultsFont") > -1)
					{
						sheet = new ResultsFontSheet();
					}
					else
					{
						string assemblyFile = Assembly.GetExecutingAssembly().CodeBase;

						ObjectHandle handle = Activator.CreateInstanceFrom(assemblyFile, typeName);
						sheet = (SheetBase)handle.Unwrap();
					}
				}
				catch (Exception exc)
				{
					Dialogs.ExceptionDialog.ShowException(exc);
				}

				// update the options custom title bar text
				TreeNode parent = e.Node;
				while (parent != null)
				{
					if (title.Length > 0)
					{
						title.Insert(0, " -> ");
					}

					title.Insert(0, parent.Text);
					parent = parent.Parent;
				}

				sheet.Dock = DockStyle.Fill;
				sheet.BackColor = Color.Transparent;
				sheet.Title = title.ToString();
				sheets.Add(e.Node.Tag, sheet);
			}

			headerLabel.Text = sheet.Title;

			contentPanel.Controls.Add(sheet);

			if (contentPanel.Controls.Count > 1)
				contentPanel.Controls.RemoveAt(0);
		}


		//========================================================================================
		// DoExport()
		//		The open/save dialog boxes reset the current working directory.  We need
		//		to preserve the working directory prior to invoking these dialogs and 
		//		restore it afterwards to ensure that System.Activator keeps working.
		//========================================================================================

		private void DoExport (object sender, EventArgs e)
		{
			string curdir = Directory.GetCurrentDirectory();

			DialogResult result = saveDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				XDocument doc = UserOptions.OptionsDoc;
				InternalSave();

				try
				{
					UserOptions.Save(saveDialog.FileName);
				}
				catch (Exception)
				{
					MessageBox.Show(
						translator.GetString("ExportError"),
						translator.GetString("ExportErrorTitle"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
						);
				}
				finally
				{
					UserOptions.OptionsDoc = doc;
				}
			}

			Directory.SetCurrentDirectory(curdir);
		}


		//========================================================================================
		// DoImport()
		//		The open/save dialog boxes reset the current working directory.  We need
		//		to preserve the working directory prior to invoking these dialogs and 
		//		restore it afterwards to ensure that System.Activator keeps working.
		//========================================================================================

		private void DoImport (object sender, EventArgs e)
		{
			string curdir = Directory.GetCurrentDirectory();

			DialogResult result = openDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				try
				{
					UserOptions.Load(openDialog.FileName);
					InternalReset();
				}
				catch (Exception exc)
				{
					Logger.WriteLine("ERROR [" + exc.Message + "]", Color.Red);

					MessageBox.Show(
						translator.GetString("ImportError"),
						translator.GetString("ImportErrorTitle"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
						);
				}
			}

			Directory.SetCurrentDirectory(curdir);
		}


		//========================================================================================
		// DoReset()
		//========================================================================================

		private void DoReset (object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(
				translator.GetString("ResetPrompt"),
				translator.GetString("ResetPromptTitle"),
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);

			if (result == DialogResult.Yes)
			{
				InternalReset();
			}
		}


		private void InternalReset ()
		{
			UserOptions.Reset();

			IDictionaryEnumerator en = sheets.GetEnumerator();
			SheetBase sheet;

			while (en.MoveNext())
			{
				sheet = (SheetBase)en.Value;
				sheet.Reset();
			}
		}


		//========================================================================================
		// DoSave()
		//========================================================================================

		private void DoSave (object sender, EventArgs e)
		{
			InternalSave();
			this.Close();
		}


		private void InternalSave ()
		{
			IDictionaryEnumerator en = sheets.GetEnumerator();
			SheetBase sheet;

			while (en.MoveNext())
			{
				sheet = (SheetBase)en.Value;
				sheet.SaveOptions();
			}

			UserOptions.Save();
		}


		public static void BuildFontList (Graphics g)
		{
			System.Drawing.Font font;
			System.Drawing.FontStyle style;
			DisplayFont displayFont;

			FontList = new ArrayList();

			FontFamily[] families = FontFamily.Families;
			foreach (FontFamily family in families)
			{
				if (family.IsStyleAvailable(FontStyle.Regular))
					style = FontStyle.Regular;
				else if (family.IsStyleAvailable(FontStyle.Bold))
					style = FontStyle.Bold;
				else if (family.IsStyleAvailable(FontStyle.Italic))
					style = FontStyle.Italic;
				else
					style = FontStyle.Strikeout;

				if (style != FontStyle.Strikeout)
				{
					font = new System.Drawing.Font(family, 12.0F, style);

					displayFont = new DisplayFont(family, style);

					displayFont.IsMonospace
						= (Math.Truncate(g.MeasureString("WWW", font).Width)
						== Math.Truncate(g.MeasureString("iii", font).Width));

					FontList.Add(displayFont);
				}
			}
		}
	}
}