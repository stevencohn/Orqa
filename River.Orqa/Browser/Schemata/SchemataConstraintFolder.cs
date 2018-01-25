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
	using System.Text;
	using System.Windows.Forms;
	using Oracle.ManagedDataAccess.Client;
	using River.Orqa.Database;


	//********************************************************************************************
	// class SchemataConstraintFolder
	//********************************************************************************************

	internal class SchemataConstraintFolder : SchemataNode
	{
		//========================================================================================
		// Constructor
		//		Public type constructor.  The default base constructor will be called
		//		implicitly, which, in turn, will call our Configure factory method.
		//========================================================================================

		internal SchemataConstraintFolder (DatabaseConnection dbase, SchemataNode srvnode)
		{
			this.dbase      = dbase;
			this.srvnode    = srvnode;
			this.schemaName = srvnode.SchemaName;

			this.canRefresh   = true;
			this.hasDiscovery = true;

			Text = "Constraints";
			ImageIndex = SelectedImageIndex	= SchemaIcons.FolderClose;

			Nodes.Add(new TreeNode("Discovering..."));

			this.AddProperty(translator.GetString("PConstraintFolderName"), Text);
			this.AddProperty(translator.GetString("PConstraintFolderType"), "Constraint Folder");
		}


		//========================================================================================
		// Methods
		//========================================================================================


		//========================================================================================
		// Discover()
		//		If an inheritor allows discoveries (HasDiscovery == true), then the inheritor
		//		should override this method to propulate its Nodes collections.
		//========================================================================================

		internal override void Discover ()
		{
			Statusbar.Message = "Discover " + Parent.Text + " constraints...";

			string sql =
				"SELECT owner, constraint_name, constraint_type, table_name, search_condition,"
				+ " r_owner, r_constraint_name, delete_rule, status, deferrable, deferred,"
				+ " validated, generated, bad, rely, last_change, index_owner, index_name,"
				+ " invalid, view_related"
				+ " FROM All_Constraints"
				+ " WHERE owner = '" + schemaName
				+ "' AND table_name ='" + Parent.Text
				+ "' AND constraint_type IN ('C','P','R','U')"
				+  " AND generated='USER NAME'"
				+ " ORDER BY constraint_name"
				;

			if (Logger.IsEnabled)
				Logger.WriteLine(sql);

			Nodes.Clear();

			OracleCommand cmd = new OracleCommand(sql,dbase.OraConnection);
			OracleDataAdapter da = new OracleDataAdapter();
			da.SelectCommand = cmd;
			DataSet ds = new DataSet();

			try
			{
				int count = da.Fill(ds);

				if (Logger.IsEnabled)
				{
					Logger.WriteSection(Parent.Parent.Text.ToUpper() + " CONSTRAINTS");
					for (int r=0; r < ds.Tables[0].Rows.Count; r++)
					{
						Logger.WriteRowData(ds.Tables[0].Rows[r]);
					}
				}

				if (count == 0)
				{
					// TODO: ((SchemaTree)this.TreeView).UnlockWindow();
				}
				else
				{
					StringBuilder text;
					SchemataConstraint constraint;
					string constraintName;
					string constraintType;
					string deleteRule;

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						constraintName = row["constraint_name"].ToString();
						constraintType = row["constraint_type"].ToString();
						deleteRule = row["delete_rule"].ToString();

						text = new StringBuilder(constraintName);
						text.Append(" (");

						if (constraintType.Equals("C"))
						{
							text.Append("...");
						}
						else
						{
							text.Append(row["r_constraint_name"].ToString().ToLower());

							if (deleteRule.Equals("CASCADE"))
								text.Append(", cascade");
						}

						text.Append(")");

						constraint = new SchemataConstraint(
							dbase,
							srvnode,
							text.ToString(),
							SchemataTypes.Constraint
							);

						switch (constraintType)
						{
							case "P":
								constraintType = "Primary key";
								constraint.ImageIndex = constraint.SelectedImageIndex = SchemaIcons.PrimaryKey;
								break;

							case "U":
								constraintType = "Unique key";
								constraint.ImageIndex = constraint.SelectedImageIndex = SchemaIcons.UniqueKey;
								break;

							case "R":
								constraintType = "Foreign key";
								constraint.ImageIndex = constraint.SelectedImageIndex = SchemaIcons.ForeignKey;
								break;

							default:
								constraintType = "Check Constraint";
								break;
						}

						constraint.AddProperty("Name", constraintName);
						constraint.AddProperty("Owner", row["OWNER"].ToString());
						constraint.AddProperty("Constraint Type", constraintType);
						constraint.AddProperty("Table Name", row["TABLE_NAME"].ToString());
						constraint.AddProperty("Search Condition", row["SEARCH_CONDITION"].ToString());
						constraint.AddProperty("Foreign Owner", row["R_OWNER"].ToString());
						constraint.AddProperty("Foreign Constraint", row["R_CONSTRAINT_NAME"].ToString());
						constraint.AddProperty("Delete Rule", deleteRule);
						constraint.AddProperty("Status", row["STATUS"].ToString());
						constraint.AddProperty("Deferrable", row["DEFERRABLE"].ToString());
						constraint.AddProperty("Deferred", row["DEFERRED"].ToString());
						constraint.AddProperty("Validated", row["VALIDATED"].ToString());
						constraint.AddProperty("Generated", row["GENERATED"].ToString());
						constraint.AddProperty("Bad", row["BAD"].ToString());
						constraint.AddProperty("Rely", row["RELY"].ToString());
						constraint.AddProperty("Last Change", row["LAST_CHANGE"].ToString());
						constraint.AddProperty("Index Owner", row["INDEX_OWNER"].ToString());
						constraint.AddProperty("Index Name", row["INDEX_NAME"].ToString());
						constraint.AddProperty("Invalid", row["INVALID"].ToString());
						constraint.AddProperty("View Related", row["VIEW_RELATED"].ToString());

						Nodes.Add(constraint);
					}
				}

				ds = null;

				Statusbar.Message = String.Empty;
			}
			catch (Exception exc)
			{
				River.Orqa.Dialogs.ExceptionDialog.ShowException(exc);
			}

			cmd.Dispose();
			cmd = null;

			isDiscovered = true;
		}


		//========================================================================================
		// DoDefaultAction()
		//========================================================================================

		internal override void DoDefaultAction ()
		{
			this.Toggle();
		}
	}
}
