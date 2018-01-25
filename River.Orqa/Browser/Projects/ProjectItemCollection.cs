//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Maintains a collection of project items within a project container.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 10-Nov-2005		New
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Collections.Generic;


	//********************************************************************************************
	// class ProjectItemCollection
	//********************************************************************************************

	/// <summary>
	/// Maintains a collection of project items within a project container.
	/// </summary>

	internal class ProjectItemCollection : List<ProjectItem>
	{
		public new ProjectItemCollection.Enumerator GetEnumerator ()
		{
			return (ProjectItemCollection.Enumerator)base.GetEnumerator();
		}
	}
}
