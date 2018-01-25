//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Collects statistics for a single session over one or more queries.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections.Specialized;
	using System.Data;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Options;


	//********************************************************************************************
	// class Statistic
	//********************************************************************************************

	/// <summary>
	/// Statistics accumulation record.  This should remain a class rather than a struct
	/// so it can be updated "by reference".
	/// </summary>

	internal class Statistic
	{
		public int ClassID;
		public string ClassName;
		public int StatID;
		public string Name;
		public int Initial;
		public int Value;
		public int Total;
	}


	//********************************************************************************************
	// class Statistics
	//********************************************************************************************

	/// <summary>
	/// Collects statistics for a single session over one or more queries.
	/// One Statistics instance is maintained by each QueryDriver.
	/// </summary>

	internal class Statistics : OrderedDictionary
	{
		private DatabaseConnection con;
		private int classes;


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new Statistics processing instance.  One Statistics instance
		/// is maintained by each QueryDriver.
		/// </summary>
		/// <param name="con">The database connection from which to fetch statistics.</param>

		public Statistics (DatabaseConnection con)
		{
			this.con = con;
		}


		//========================================================================================
		// Initialize()
		//========================================================================================

		public void Initialize ()
		{
			// allow classes to be changed dynamically in run-time
			this.classes = UserOptions.GetInt("statistics/classifications");

			// clear out the stats collection
			this.Clear();

			// get an initial snapshot so we can calculate a delta with Summarize
			DataRowCollection rows = GetSnapshot();

			Statistic stat;
			int classID;

			// build the statistics collection from the current snapshot
			foreach (DataRow row in rows)
			{
				classID = (int)(decimal)row["classID"];
				if ((classes & classID) == classID)
				{
					stat = new Statistic();
					stat.ClassID = classID;

					if (row["className"] == System.DBNull.Value)
						stat.ClassName = String.Empty;
					else
						stat.ClassName = (string)row["className"];

					stat.StatID = (int)(decimal)row["statID"];
					stat.Name = (string)row["name"];
					stat.Initial = (int)(decimal)row["value"];
					stat.Value = stat.Initial;
					stat.Total = stat.Initial;

					this.Add(stat.Name, stat);
				}
			}
		}


		//========================================================================================
		// Summarize()
		//========================================================================================

		/// <summary>
		/// Updates the current statistics collection, capturing new quantifying values
		/// from the data source.  These values are compared to the values captured
		/// during initialization to calculate delta values.
		/// </summary>

		public void Summarize ()
		{
			DataRowCollection rows = GetSnapshot();
			Statistic stat;
			int classID;

			foreach (DataRow row in rows)
			{
				classID = (int)(decimal)row["classID"];
				if ((classes & classID) == classID)
				{
					stat = (Statistic)this[(string)row["name"]];
					stat.Value = (int)(decimal)row["value"];
				}
			}
		}


		//========================================================================================
		// GetSnapshot()
		//========================================================================================

		private DataRowCollection GetSnapshot ()
		{
			string sql =
@"SELECT E.class AS classID,
  CASE E.class
    WHEN 1 THEN 'User'
    WHEN 2 THEN 'Redo'
    WHEN 4 THEN 'Enqueue'
    WHEN 8 THEN 'Cache'
    WHEN 16 THEN 'OS'
    WHEN 32 THEN 'Parallel Server'
    WHEN 64 THEN 'SQL'
    WHEN 128 THEN 'Debug'
    ELSE ''
  END AS className,
       S.statistic# AS statID, E.name, S.value
  FROM v$sesstat S
  JOIN V$session N
    ON N.sid = S.sid
   AND N.audsid = userenv('SESSIONID')
  JOIN v$statname E
    ON E.statistic# = S.statistic#
 ORDER BY E.class, E.name";

				// this test is exceedingly slow, so we filter out classes
				// above in Initialize and Summarize:

				// x-BITAND(x,y)+y = y
				//+    " AND (E.class-BITAND(E.class," + classes + ")+" + classes + ") = " + classes

			var dataset = new DataSet();
			using (var cmd = new OracleCommand(sql, con.OraConnection))
			{
				using (var adapter = new OracleDataAdapter(cmd))
				{
					adapter.Fill(dataset);
				}
			}

			return dataset.Tables[0].Rows;
		}
	}
}
