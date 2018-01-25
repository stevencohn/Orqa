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
	using System.Collections;
	using System.Windows.Forms;


	//********************************************************************************************
	// class CommandGroup
	//********************************************************************************************

	internal class CommandGroup
	{
		private Hashtable checkers;					// checkable things
		private ArrayList commands;					// members of group


		//========================================================================================
		// Constructor
		//========================================================================================

		internal CommandGroup ()
		{
			this.commands = new ArrayList();
			this.checkers = new Hashtable();
		}


		//========================================================================================
		// IsEnabled
		//========================================================================================

		internal bool IsEnabled
		{
			set
			{
				foreach (ToolStripItem command in commands)
				{
					command.Enabled = value;
				}
			}
		}


		//========================================================================================
		// AddChecker()
		//========================================================================================

		internal void AddChecker (string name, ToolStripItem item)
		{
			ArrayList commands;

			if (checkers.Contains(name))
			{
				commands = (ArrayList)checkers[name];
			}
			else
			{
				commands = new ArrayList();
				checkers.Add(name, commands);
			}

			commands.Add(item);
		}


		internal void SetChecker (string name, bool isChecked)
		{
			ArrayList commands = (ArrayList)checkers[name];

			foreach (object item in commands)
			{
				if (item is ToolStripButton)
				{
					((ToolStripButton)item).Checked = isChecked;
				}
				else
				{
					((ToolStripMenuItem)item).Checked = isChecked;
				}
			}
		}


		//========================================================================================
		// Join()
		//========================================================================================

		internal void Join (ToolStripItem item)
		{
			commands.Add(item);
		}


		internal void Join (ToolStripItem item1, ToolStripItem item2)
		{
			commands.Add(item1);
			commands.Add(item2);
		}
	}
}
