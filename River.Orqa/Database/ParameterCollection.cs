//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Maintains a collection of Parameter instances.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Aug-2005      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections.Generic;


	//********************************************************************************************
	// class ParameterCollection
	//********************************************************************************************

	/// <summary>
	/// Maintains a collection of Parameter instances.
	/// </summary>

	internal class ParameterCollection : List<Parameter>
	{

		//========================================================================================
		// GetEnumerator()
		//========================================================================================

		/// <summary>
		/// Returns a typed enumerator for this collection.
		/// </summary>
		/// <returns>A ParameterCollection.Enumerator</returns>

		public new ParameterCollection.Enumerator GetEnumerator ()
		{
			return (ParameterCollection.Enumerator)base.GetEnumerator();
		}
	}
}
