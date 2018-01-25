//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Schemata tree Table  node.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 27-Feb-2003		Separated from Schemata.cs
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.Data;
	using System.Windows.Forms;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataParameter
	//********************************************************************************************

	internal class SchemataParameter : SchemataNode
	{
		private string name;
		private string typeName;
		private ParameterDirection direction;


		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		public SchemataParameter (
			DatabaseConnection dbase, SchemataNode srvnode, string text,
			string argumentName, string typeName, ParameterDirection direction)
		{
			this.dbase = dbase;
			this.srvnode = srvnode;
			this.schemaName = srvnode.SchemaName;
			this.canDrag = true;

			this.name = argumentName;
			this.typeName = typeName;
			this.direction = direction;

			Text = text;
			ImageIndex = SelectedImageIndex = SchemaIcons.ParameterIn;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public ParameterDirection Direction
		{
			get { return direction; }
		}


		public new string Name
		{
			get { return name; }
		}


		public string TypeName
		{
			get { return typeName; }
		}
	}
}
