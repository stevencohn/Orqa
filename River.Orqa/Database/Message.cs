//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Represents a single informational message generated during statement parsing
// or query execution.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2004      New
// 01-Aug-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;


	//********************************************************************************************
	// class Message
	//********************************************************************************************

	/// <summary>
	/// Represents a single informational message generated during statement parsing
	/// or query execution.
	/// </summary>

	internal class Message
	{
		private MessageType type;			// informational level of this message
		private string text;				// text of this message

		public enum MessageType
		{
			Error,							// exceptions and errors
			Info,							// data source informationals
			User,							// USER_ERRORS
			State							// connection state changes
		}


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new instance of the specified type with the given text.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>

		public Message (MessageType type, string text)
		{
			this.type = type;
			this.text = text;
		}


		//========================================================================================
		// Type
		//========================================================================================

		/// <summary>
		/// Get the informational level of this message.
		/// </summary>

		public MessageType Type
		{
			get { return type; }
		}


		//========================================================================================
		// Message
		//========================================================================================

		/// <summary>
		/// Gets the text of this message.
		/// </summary>

		public string Text
		{
			get { return text; }
		}
	}
}
