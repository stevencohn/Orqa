//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Maintains a collection of Message instances.
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
	// class MessageCollection
	//********************************************************************************************

	/// <summary>
	/// Maintains a collection of Message instances.
	/// </summary>

	internal class MessageCollection : List<Message>
	{

		//========================================================================================
		// GetEnumerator()
		//========================================================================================
		
		/// <summary>
		/// Returns a typed enumerator for this collection.
		/// </summary>
		/// <returns>A MessageCollection.Enumerator</returns>

		public new MessageCollection.Enumerator GetEnumerator ()
		{
			return (MessageCollection.Enumerator)base.GetEnumerator();
		}
	}
}
