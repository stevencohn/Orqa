//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// This exception is thrown when attempting to connect locally and TNS error is caught.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 16-Feb-2006		New
//************************************************************************************************

using System;
using System.Runtime.Serialization;
using River.Orqa.Resources;

namespace River.Orqa.Database
{
	/// <summary>
	///  This exception is thrown when attempting to connect locally and a TNS error
	/// is caught.
	/// </summary>
	/// <seealso cref="ApplicationException"/>

	internal class MissingSidException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>

		public MissingSidException ()
			: base(new Translator("Orqa").GetString("MissingSidError").Replace("\\n","\n"))
		{
		}
	}
}

