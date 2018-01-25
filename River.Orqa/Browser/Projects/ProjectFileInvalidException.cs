//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// This exception is thrown when the specified project file is invalid.
// A project file may be invalid because its version may be too old.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 10-Nov-2005		New
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Runtime.Serialization;


	//********************************************************************************************
	// class ProjectFileInvalidException
	//********************************************************************************************

	/// <summary>
	/// This exception is thrown when the specified project file is invalid.
	/// A project file may be invalid because its version may be too old.
	/// </summary>
	/// <seealso cref="ApplicationException"/>

	[Serializable]
	internal class ProjectFileInvalidException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>

		public ProjectFileInvalidException () : base() { }


		/// <summary>
		/// Initializes a new instance of the class with a specified error message. 
		/// </summary>
		/// <param name="message">A message that describes the error.</param>

		public ProjectFileInvalidException (string message) : base(message) { }


		/// <summary>
		/// Initializes a new instance of the class with serialized data. 
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <seealso cref="SerializationInfo"/>
		/// <seealso cref="StreamingContext"/>

		public ProjectFileInvalidException (SerializationInfo info, StreamingContext context) : base(info, context) { }


		/// <summary>
		/// Initializes a new instance of the class with a specified error message and
		/// a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.
		/// If the innerException parameter is not a null reference, the current exception is
		/// raised in a catch block that handles the inner exception.</param>

		public ProjectFileInvalidException (string message, Exception innerException) : base(message, innerException) { }
	}
}
