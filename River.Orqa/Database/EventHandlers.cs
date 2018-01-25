//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Event delegate signature and handler arguments for statement parsing and
// query execution notifications.
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
	// delegates
	//********************************************************************************************

	/// <summary>
	/// Declares the signature of a QueryCompleted event handler invoked when
	/// each query in a QueryCollection is executed.
	/// </summary>
	/// <param name="e">The handler argument specifying the completed query.</param>

	internal delegate void QueryCompletedEventHandler (QueryCompletedEventArgs e);


	/// <summary>
	/// Declares the signature of a QueriesCompleted event handler invoked afer
	/// all queries in a QueryCollection are executed.  This handler reports the
	/// summary information for all queries.
	/// </summary>
	/// <param name="e">
	/// The handler argument specifying the completed query collection
	/// containing summary information.
	/// </param>

	internal delegate void QueriesCompletedEventHandler (QueriesCompletedEventArgs e);


	/// <summary>
	/// Declares the signature of a Notification event handler that must be implemented
	/// by consumers of Notification events.
	/// </summary>
	/// <param name="e">The arguments passed to the consumer.</param>

	internal delegate void NotificationEventHandler (NotificationEventArgs e);


	//********************************************************************************************
	// class QueryCompletedEventArgs
	//********************************************************************************************

	/// <summary>
	/// Represents the argument passed to a QueriesCompleted event handler.
	/// This argument contains the query collection with summary information.
	/// </summary>

	internal class QueryCompletedEventArgs
	{
		private Query query;						// executed query


		/// <summary>
		/// Initializes a new instance of the specified type with the given query.
		/// </summary>
		/// <param name="queries">The query completed.</param>

		public QueryCompletedEventArgs (Query query)
		{
			this.query = query;
		}


		/// <summary>
		/// Gets the completed query.
		/// </summary>

		public Query Query
		{
			get { return query; }
		}
	}


	//********************************************************************************************
	// class QueriesCompletedEventArgs
	//********************************************************************************************

	/// <summary>
	/// Represents the argument passed to a QueriesCompleted event handler.
	/// This argument contains the query collection with summary information.
	/// </summary>

	internal class QueriesCompletedEventArgs
	{
		private QueryCollection queries;			// all executed queries


		/// <summary>
		/// Initializes a new instance of the specified type with the given query.
		/// </summary>
		/// <param name="queries">The query collection with summary information.</param>

		public QueriesCompletedEventArgs (QueryCollection queries)
		{
			this.queries = queries;
		}


		/// <summary>
		/// Gets the query collection.
		/// </summary>

		public QueryCollection Queries
		{
			get { return queries; }
		}
	}


	//********************************************************************************************
	// class NotificationEventArgs
	//********************************************************************************************
	
	/// <summary>
	/// Represents the arguments passed to the consumer of a NotificationEventHandler.
	/// This argument contains the notification message.
	/// </summary>

	internal class NotificationEventArgs
	{
		private Message message;					// notification message


		/// <summary>
		/// Initializes a new instance of the specified type with the given text.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>

		public NotificationEventArgs (Message message)
		{
			this.message = message;
		}


		/// <summary>
		/// Gets the message.
		/// </summary>

		public Message Message
		{
			get { return message; }
		}
	}
}
