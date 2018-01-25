//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Declares event delegates, handlers and arguments used to indicate editor-based actions.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Query
{
	using System;
	using River.Orqa.Database;


	//********************************************************************************************
	// enum Editor.Action
	//********************************************************************************************

	internal enum EditorAction
	{
		HasNoContent,
		HasContent,
		Unsaved
	}


	//********************************************************************************************
	// Delegates
	//********************************************************************************************

	internal delegate void CaretChangedEventHandler (object sender, CaretChangedEventArgs e);
	internal delegate void EditorEventHandler (object sender, EditorEventArgs e);
	internal delegate void InsertModeChangedEventHandler (object sender, InsertModeChangedEventArgs e);
	internal delegate void ResultTargetChangedEventHandler (object sender, ResultTargetChangedEventArgs e);
	internal delegate void ToggleResultsHandler (object sender, ToggleResultsEventArgs e);


	//********************************************************************************************
	// class CaretChangedEventArgs
	//********************************************************************************************

	internal class CaretChangedEventArgs : System.EventArgs
	{
		private int col;
		private int line;

		public CaretChangedEventArgs (int line, int col)
		{
			this.col = col;
			this.line = line;
		}

		public int Column
		{
			get { return col; }
		}


		public int Line
		{
			get { return line; }
		}
	}


	//********************************************************************************************
	// class EditorEventArgs
	//********************************************************************************************

	internal class EditorEventArgs : EventArgs
	{
		private EditorAction action;			// specifies the general action type


		public EditorEventArgs (EditorAction action)
		{
			this.action = action;
		}


		public EditorAction Action
		{
			get { return action; }
		}
	}


	//********************************************************************************************
	// class InsertModeChangedEventArgs
	//********************************************************************************************

	internal class InsertModeChangedEventArgs : System.EventArgs
	{
		private bool isInsert;


		public InsertModeChangedEventArgs (bool isInsert)
		{
			this.isInsert = isInsert;
		}


		public bool IsInsert
		{
			get { return isInsert; }
		}
	}


	//********************************************************************************************
	// class ResultTargetChangedEventArgs
	//********************************************************************************************

	internal class ResultTargetChangedEventArgs : System.EventArgs
	{
		private ResultTarget resultTarget;


		public ResultTargetChangedEventArgs (ResultTarget resultTarget)
		{
			this.resultTarget = resultTarget;
		}


		public ResultTarget ResultTarget
		{
			get { return resultTarget; }
		}
	}


	//********************************************************************************************
	// class ToggleResultsEventArgs
	//********************************************************************************************

	internal class ToggleResultsEventArgs : System.EventArgs
	{
		private bool isCollapsed;


		public ToggleResultsEventArgs (bool isCollapsed)
		{
			this.isCollapsed = isCollapsed;
		}


		public bool IsCollapsed
		{
			get { return isCollapsed; }
		}
	}
}
