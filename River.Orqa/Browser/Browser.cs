//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Displays the hosted object and template browser tool window.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using River.Orqa.Options;
	using River.Orqa.Database;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class Browser
	//********************************************************************************************

	internal partial class Browser : UserControl, IBrowser
	{
		private ICommander commander;

		// Events

		public event BrowserTreeEventHandler BrowserActionRequested;
		public event BrowserSelectorEventHandler SchemaSelected;
		public event ScriptOpenHandler ScriptOpen;
		public event ScriptSelectionHandler ScriptSelected;
		public event TemplateSelectionHandler TemplateSelected;


		//========================================================================================
		// Constructor
		//========================================================================================

		public Browser ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public ICommander Commander
		{
			set { commander = projectTree.Commander = value; }
		}


		public SchemataSchema Schema
		{
			get { return schemaManager.Schema; }
			set { schemaManager.Schema = value; }
		}


		public DatabaseConnection[] Servers
		{
			get
			{
				return schemaManager.Servers;
			}
		}


		//========================================================================================
		// AddConnection()
		//========================================================================================

		public void AddConnection (DatabaseConnection dbase)
		{
			schemaManager.AddConnection(dbase);
			Statusbar.Connections = schemaManager.Count;
		}


		//========================================================================================
		// FindProcParameters()
		//========================================================================================

		public SchemataParameter[] FindProcParameters (string procName)
		{
			return schemaManager.FindProcParameters(procName);
		}


		//========================================================================================
		// GetDatabaseSchemas()
		//========================================================================================

		public SchemataSchema[] GetDatabaseSchemas (DatabaseConnection con)
		{
			return schemaManager.GetDatabaseSchemas(con);
		}


		//========================================================================================
		// OpenProjectFile()
		//========================================================================================

		public void OpenProjectFile (string path)
		{
			tabset.SelectedTab = projectPage;
			projectTree.AddProject(path);
		}


		//========================================================================================
		// SetOptions()
		//========================================================================================

		public void SetOptions ()
		{
			templateTree.SetOptions();
		}

		
		//========================================================================================
		// Events Dispatch
		//========================================================================================

		private void DoCompiling (BrowserTreeEventArgs e)
		{
			if (BrowserActionRequested != null)
				BrowserActionRequested(new BrowserTreeEventArgs(BrowserTreeAction.Compile, e.Node));
		}


		private void DoEditing (BrowserTreeEventArgs e)
		{
			if (BrowserActionRequested != null)
				BrowserActionRequested(new BrowserTreeEventArgs(BrowserTreeAction.Edit, e.Node));
		}


		private void DoOpening (BrowserTreeEventArgs e)
		{
			if (BrowserActionRequested != null)
				BrowserActionRequested(new BrowserTreeEventArgs(BrowserTreeAction.Open, e.Node));
		}


		private void DoSchemaSelected (string schemaName)
		{
			if (SchemaSelected != null)
				SchemaSelected(schemaName);
		}


		private void DoSchemataSelected (BrowserTreeEventArgs e)
		{
			if (BrowserActionRequested != null)
				BrowserActionRequested(new BrowserTreeEventArgs(BrowserTreeAction.Select, e.Node));
		}


		private void DoShowingProperties (BrowserTreeEventArgs e)
		{
			if (BrowserActionRequested != null)
				BrowserActionRequested(new BrowserTreeEventArgs(BrowserTreeAction.Properties, e.Node));
		}


		private void DoScriptOpen ()
		{
			if (ScriptOpen != null)
				ScriptOpen();
		}


		private void DoScriptSelected (string path)
		{
			if (ScriptSelected != null)
				ScriptSelected(path);
		}


		private void DoTabIndexChanged (object sender, EventArgs e)
		{
			if (tabset.SelectedTab == objectPage)
			{
				schemaManager.Focus();
				commander.SetDefaults();
			}
			else if (tabset.SelectedTab == projectPage)
			{
				projectTree.Focus();
			}
			else if (tabset.SelectedTab == templatePage)
			{
				templateTree.Focus();
				commander.SetDefaults();
			}
		}


		private void DoTemplateSelected (string content, bool inNewWindow, bool runReport)
		{
			if (TemplateSelected != null)
				TemplateSelected(content, inNewWindow, runReport);
		}
	}
}
