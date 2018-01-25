//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Represents a single TNS entry along with information indicating origin.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Jul-2005      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;


	//********************************************************************************************
	// class TnsName
	//********************************************************************************************

	/// <summary>
	/// Represents a single TNS entry along with information regarding origin.
	/// </summary>

	internal class TnsName
	{
		private string name;
		private Tns.Location location;
		private string info;


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new TNS Name entry.
		/// </summary>
		/// <param name="name">The unique SID name of this entry.</param>
		/// <param name="location">The location or path of the data source.</param>
		/// <param name="info">Extra information regarding this entry.</param>

		public TnsName (string name, Tns.Location location, string info)
		{
			this.name = name;
			this.location = location;
			this.info = info;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets any extra information regarding this entry, such as host address of
		/// a TNS entry discovered in the tnsnames.ora file.
		/// </summary>
		/// <value>
		/// Returns an empty string if no extra information is available.
		/// </value>

		public string Info
		{
			get { return info == null ? String.Empty : info; }
		}


		/// <summary>
		/// The location or path of the data source.  For example this may indicate
		/// the registry key or tnsnames.ora file path.
		/// </summary>

		public Tns.Location Location
		{
			get { return location; }
		}


		/// <summary>
		/// The unique SID name of this entry.
		/// </summary>

		public string Name
		{
			get { return name; }
		}


		//========================================================================================
		// ToString()
		//========================================================================================

		/// <summary>
		/// Specific override used to sort a collection of TnsName entries.
		/// </summary>
		/// <returns>A string value specifying the TNS name of this entry.</returns>

		public override string ToString ()
		{
			return name;
		}
	}
}
