//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Parameter information.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Data;
	using Oracle.ManagedDataAccess.Client;


	//********************************************************************************************
	// class Parameter
	//********************************************************************************************

	internal class Parameter
	{
		private string name;						// display name of oracle type
		private Type type;							// closest .NET system type equivalent
		private OracleDbType dtype;					// closest Oracle data type
		private ParameterDirection direction;		// in, out, in/out, return
		private object pvalue;						// value of this parameter


		//========================================================================================
		// Constructor
		//========================================================================================

		public Parameter (Browser.SchemataParameter node)
		{
			this.name = node.Name;
			this.direction = node.Direction;

			ParameterConverter.CustomType custom = ParameterConverter.Convert(node.TypeName);
			this.dtype = custom.OraType;
			this.type = custom.Type;

			this.pvalue = null;
		}


		public Parameter (
			string name, ParameterDirection direction, OracleDbType dtype, object pvalue)
		{
			this.name = name;
			this.direction = direction;
			this.dtype = dtype;
			this.type = typeof(string);		// TODO: should this be specific?
			this.pvalue = pvalue;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public string Name  // public for ListBox.DisplayName
		{
			get { return name; }
		}


		public OracleDbType DataType
		{
			get { return dtype; }
		}


		public ParameterDirection Direction
		{
			get { return direction; }
		}


		public object Value
		{
			get { return pvalue; }
			set { pvalue = value; }
		}


		public override string ToString ()
		{
			return name;
		}


		public Type Type
		{
			get { return type; }
		}
	}
}
