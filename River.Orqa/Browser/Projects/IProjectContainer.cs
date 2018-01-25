//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Represents a project member that can contain other members.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 10-Nov-2005		New
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;


	//********************************************************************************************
	// interface IProjectContainer
	//********************************************************************************************

	/// <summary>
	/// Represents a project member that can contain other members.
	/// </summary>

	internal interface IProjectContainer
	{
		/// <summary>
		/// Gets the directory path of the item.  If this is a Project instance, it 
		/// will return the directory part of the full path.
		/// </summary>

		string FolderPath { get; }


		/// <summary>
		/// Adds a new member to this container.
		/// </summary>
		/// <param name="item">A DirectoryItem or a FileItem to add to this container.</param>

		int AddChild (ProjectItem item);


		/// <summary>
		/// Removes the specified member from this container.
		/// </summary>
		/// <param name="item">A DirectoryItem or a FileItem to remove from this container.</param>

		void DeleteChild (ProjectItem item);
	}
}
