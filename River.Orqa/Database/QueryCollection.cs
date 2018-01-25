//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Maintains a collection of Query instances along with summary information.
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
	// class QueryCollection
	//********************************************************************************************

	/// <summary>
	/// Maintains a collection of Query instances along with summary information.
	/// </summary>

	internal class QueryCollection : List<Query>
	{
		private int totalRecords;						// total records affected
		private int totalTicks;							// total execution time
		private Statistics statistics;					// summary statistics


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new empty collection.
		/// </summary>

		public QueryCollection ()
			: base()
		{
			this.totalRecords = 0;
			this.totalTicks = 0;
			this.statistics = null;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public bool HasPlans
		{
			get
			{
				bool found = false;
				int i = 0;
				while ((i < this.Count) && !found)
				{

					if (!(found = ((this[i].Plan != null) && (this[i].Plan.Tables.Count > 0))))
						i++;
				}

				return found;
			}
		}


			/// <summary>
			/// Gets or sets the summary statistics for all queries in the collection.
			/// </summary>

			public Statistics Statistics
		{
			get { return statistics; }
			set { statistics = value; }
		}


		/// <summary>
		/// Gets or sets the total number of records affected by all queries successfully
		/// executed in this collection.
		/// </summary>

		public int TotalRecords
		{
			get { return totalRecords; }
			set { totalRecords = value; }
		}


		/// <summary>
		/// Gets or sets the total tick count, in milliseconds, of all queries successfully
		/// executed in this collection.
		/// </summary>

		public int TotalTicks
		{
			get { return totalTicks; }
			set { totalTicks = value; }
		}


		/// <summary>
		/// Gets the total elapsed time for all executed queries in this collection.
		/// </summary>

		public TimeSpan TotalTime
		{
			get { return TimeSpan.FromMilliseconds(totalTicks); }
		}


		//========================================================================================
		// Clear()
		//========================================================================================

		/// <summary>
		/// Removes all queries from the collection and resets summary statistics.
		/// </summary>

		public new void Clear ()
		{
			this.totalRecords = 0;
			this.totalTicks = 0;
			this.statistics = null;

			base.Clear();
		}


		//========================================================================================
		// GetEnumerator()
		//========================================================================================

		/// <summary>
		/// Returns a typed enumerator for this collection.
		/// </summary>
		/// <returns>A QueryCollection.Enumerator</returns>

		public new QueryCollection.Enumerator GetEnumerator ()
		{
			return (QueryCollection.Enumerator)base.GetEnumerator();
		}
	}
}
