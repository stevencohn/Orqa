//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Event delegate signature and handler arguments for statement parsing and
// query execution notifications.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2004      New
// 01-Aug-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;


	//********************************************************************************************
	// delegates
	//********************************************************************************************

	internal delegate void BrowserSelectorEventHandler (string schemaName);
	internal delegate void BrowserTreeEventHandler (BrowserTreeEventArgs e);
	internal delegate void ScriptOpenHandler ();
	internal delegate void ScriptSelectionHandler (string path);
	internal delegate void TemplateSelectionHandler (string content, bool inNewWindow, bool runReport);


	internal enum BrowserTreeAction
	{
		Compile,
		Delete,
		Edit,
		Open,
		Properties,
		Select
	}


	//********************************************************************************************
	// class BrowserTreeEventArgs
	//********************************************************************************************

	internal class BrowserTreeEventArgs : System.EventArgs
	{
		private BrowserTreeAction action;
		private SchemataNode node;

		public BrowserTreeEventArgs (BrowserTreeAction action, SchemataNode node)
		{
			this.action = action;
			this.node = node;
		}

		public BrowserTreeAction Action
		{
			get { return action; }
		}


		public SchemataNode Node
		{
			get { return node; }
		}
	}
}
