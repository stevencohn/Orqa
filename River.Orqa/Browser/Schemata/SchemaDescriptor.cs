//************************************************************************************************
//				Copyright © 2002-2003 Steven M. Cohn. All Rights Reserved.
//
// Title:		SchemaDescriptor.cs
//
// Facility:	Microsoft Development Environment 2003	Version 7.1.3088
// Environment:	Microsoft .NET Framework 1.1			Version 1.1.4322
//
// Description:
//
// Revision History:
// -Who------------------- -When---------- -What--------------------------------------------------
// Steven M. Cohn			27-May-2002		New
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Text;
	using System.Windows.Forms;


	//********************************************************************************************
	// class SchemaDescriptor
	//********************************************************************************************

	public class SchemaDescriptor
	{
		public TreeNode srvnode;					// this node's top-most server node
		public TreeNode node;						// this node
		public OleDbConnection con;					// connection
		public string schemaName;					// schema name
		public OraObjects type;						// type of node
		public OraObjects subtype;					// subtype of node
		public object[] restrictions;				// guid-specific restrictions
		public bool discovered;						// true if completed discovery

		
		//========================================================================================
		// Constructor
		//========================================================================================

		public SchemaDescriptor (
			TreeNode srvnode,
			TreeNode node,
			OleDbConnection con,
			string schemaName,
			OraObjects type,
			OraObjects subtype,
			object[] restrictions)
		{
			this.srvnode		= srvnode;
			this.node			= node;
			this.con			= con;
			this.schemaName		= schemaName;
			this.type			= type;
			this.subtype		= subtype;
			this.restrictions	= restrictions;
			this.discovered		= false;
		}


		//========================================================================================
		// Dump()
		//========================================================================================

		public void Dump (DebugWindow debugger)
		{
			Logger.WriteLine("SchemaName: " + schemaName);
			Logger.WriteLine("Type/SubTp: " + type.ToString() + "/" + subtype.ToString());

			if (restrictions == null)
				Logger.WriteLine("Restrictns: <null>");
			else
			{
				StringBuilder rs = new StringBuilder("Restrictns: ");

				for (int i=0; i < restrictions.Length; i++)
				{
					if (i > 0) rs.Append("/");
					if (restrictions[i] == null)
						rs.Append("<null>");
					else
						rs.Append(restrictions[i].ToString());
				}

				Logger.WriteLine(rs.ToString());
			}
		}
	}
}
