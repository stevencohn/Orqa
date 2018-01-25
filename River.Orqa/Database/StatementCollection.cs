//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Given a block of text, build a collection of SQL statements that are ready
// to be executed against a data source provider.
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
	using System.Text;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Options;


	//********************************************************************************************
	// class StatementCollection
	//********************************************************************************************

	/// <summary>
	/// Given a block of text, build a collection SQL statements that are ready
	/// to be executed against a data source provider.
	/// </summary>

	internal class StatementCollection : StringCollection
	{
		private bool isCombined;				// true if combined (wrapped in anon BEGIN/END)
		private bool isWrapped;					// true if block is wrapped in a sproc
		private ParameterCollection parameters;	// parameters for Wrapped content


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initialize a new empty statement collection.
		/// </summary>

		public StatementCollection ()
		{
			this.isCombined = false;
			this.isWrapped = false;
			this.parameters = null;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the value of the maintained isCombined field.
		/// </summary>

		public bool IsCombined
		{
			get { return isCombined; }
		}


		/// <summary>
		/// Gets or sets a value indicating if the statements should be wrapped in a
		/// temporary stored procedure for execution.  This is invoked from the client;
		/// we only maintain it here for convenience.
		/// </summary>

		public bool IsWrapped
		{
			get { return isWrapped; }
			set { isWrapped = value; }
		}


		/// <summary>
		/// Gets a collection of output parameters returned from a Wrapped block
		/// of SELECT statements.
		/// </summary>

		public ParameterCollection Parameters
		{
			get { return parameters; }
		}


		//========================================================================================
		// Combine()
		//========================================================================================

		/// <summary>
		/// Joins all statements within the collection into a single string ready
		/// to be executed.  Each of the combined statements are guaranteeed to be
		/// separated from the next with a semi-colon and a space.
		/// </summary>
		/// <returns>
		/// A string specifying the combined content.
		/// </returns>

		public string Combine ()
		{
			if (this.Count == 0)
				return null;

			var text = new StringBuilder(this[0]);

			while (this.Count > 1)
			{
				if (text[text.Length - 1] != ';')
					text.Append(';');

				text.Append(' ');
				text.Append(this[1]);

				this.RemoveAt(1);
			}

			isCombined = true;

			return this[0] = text.ToString();
		}


		//========================================================================================
		// Wrap()
		//========================================================================================

		/// <summary>
		/// Combines the statements of this collection into a temporary stored procedure.
		/// The procedure will declare zero or more output parameters returning sys_refcursors
		/// for each query statement.
		/// </summary>
		/// <remarks>
		/// After wrapping, the Parameters property will contain zero or more Parameter
		/// instances, each describing an output parameter for a SELECT statement in the
		/// statement collection.
		/// <para>
		/// The name of the stored procedure to generate is specified by the 
		/// <i>connections/utilProcedure</i> option.
		/// </para>
		/// </remarks>
		/// <returns>
		/// A string specifying the wrapped content.
		/// </returns>

		public string Wrap (string schemaName)
		{
			parameters = new ParameterCollection();
			Parameter parameter = null;

			var args = new StringBuilder();
			var body = new StringBuilder();
			string stmt;

			// 1. Generate CREATE PROCEDURE statement

			body.Append("BEGIN");

			for (int i = 0; i < this.Count; i++)
			{
				stmt = (string)this[i];

				if (stmt.ToUpper().StartsWith("SELECT"))
				{
					parameter = new Parameter(
						"recset" + i, ParameterDirection.Output, OracleDbType.RefCursor, null);

					parameters.Add(parameter);

					if (args.Length > 0)
						args.Append(", ");

					args.Append("recset" + i + " OUT sys_refcursor"); //Orqa_Util.Recordset");
					body.Append(" OPEN recset" + i + " FOR " + stmt + ";");
				}
				else
				{
					body.Append(" " + stmt);

					if (stmt[stmt.Length - 1] != ';')
						body.Append(';');
				}
			}

			body.Append(" END;");

			string procedureName = schemaName + "." + UserOptions.GetString("connections/utilProcedure");
			var text = new StringBuilder("CREATE OR REPLACE PROCEDURE " + procedureName);

			if (args.Length > 0)
				text.Append(" (" + args.ToString() + ")");

			text.Append(" AS ");
			text.Append(body.ToString());

			this.Clear();
			this.Add(text.ToString());

			// 2. Generate EXEC statement

			text.Length = 0;
			text.Append("EXEC " + procedureName);

			for (int i = 0; i < parameters.Count; i++)
			{
				if (i == 0)
					text.Append(" @@" + parameters[i].Name);
				else
					text.Append(", @@" + parameters[i].Name);
			}

			text.Append(";");

			this.Add(text.ToString());

			isWrapped = true;

			return this[0];
		}
	}
}
