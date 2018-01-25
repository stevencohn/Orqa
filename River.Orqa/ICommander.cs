//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Implements a pattern much like the Command/Dictator patterns; it handles both 
// actionable and enabled items.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa
{
	using System;
	using River.Orqa.Database;


	//********************************************************************************************
	// interface ICommander
	//********************************************************************************************

	internal interface ICommander
	{
		CommandGroup AdvancedControls { get; }
		CommandGroup ConnectControls { get; }
		CommandGroup ExecuteControls { get; }
		CommandGroup PasteControls { get; }
		CommandGroup RedoControls { get; }
		CommandGroup SaveControls { get; }
		CommandGroup SelectControls { get; }
		CommandGroup SqlControls { get; }
		CommandGroup TextControls { get; }
		CommandGroup UndoControls { get; }

		void SetChecker (string name, bool isChecked);
		void SetDefaults ();
		void SetResultTarget (ResultTarget target);
		void SetWorker (IWorker worker);
	}


	//********************************************************************************************
	// interface IWorker
	//********************************************************************************************

	internal interface IWorker
	{
		bool IsSaved { get; }
		string Filename { get; }

		void SaveFile (string filename);
		void UpdateCommander ();
	}
}
