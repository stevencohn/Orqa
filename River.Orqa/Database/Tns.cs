//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Build collection of Oracle TNS names, including the default SID stored in
// either the registry or an environment variable.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Text;
	using River.Native;


	//********************************************************************************************
	// class Tns
	//********************************************************************************************

	/// <summary>
	/// Provides access to all known TNS entries defined on the local machine.
	/// Access to remote machines much be specified via these entries, specifically
	/// the tnsnames.ora file.
	/// </summary>

	internal class Tns
	{
		private static TnsName[] names;
		private static TnsName[] allNames;
		private static string registryPath;
		private static string tnsNamesOraPath;


		//========================================================================================
		// enum Location
		//========================================================================================

		/// <summary>
		/// Specifies the various locations from where TNS entries are discovered.
		/// </summary>

		public enum Location
		{
			/// <summary>
			/// A TNS entry defined by the ORACLE_SID environment variable.
			/// </summary>

			Environment,

			/// <summary>
			/// A TNS entry defined by the ORACLE_SID registry key.
			/// </summary>

			Registry,

			/// <summary>
			/// A TNS entry declared in the tnsnames.ora file.
			/// </summary>

			TnsNames
		}


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Static constructor; initializes a persistent table of TNS names.
		/// </summary>

		static Tns ()
		{
			Refresh();
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets an array of all TNS names defined everywhere represented as TnsName instances.
		/// </summary>

		public static TnsName[] AllNames
		{
			get { return allNames; }
		}


		/// <summary>
		/// Gets an array of unique TNS names represented as TnsName instances.
		/// </summary>

		public static TnsName[] Names
		{
			get { return names; }
		}


		/// <summary>
		/// Gets the path of the registry key from where the ORACLE_SID registry 
		/// entry was discovered.
		/// </summary>

		public static string RegistryPath
		{
			get { return registryPath == null ? String.Empty : registryPath; }
		}


		/// <summary>
		/// Gest the path of the tnsnames.ora file from which TNS entries were
		/// read.
		/// </summary>

		public static string TnsNameOraPath
		{
			get { return tnsNamesOraPath; }
		}


		//========================================================================================
		// Contains()
		//========================================================================================

		public bool Contains (string name)
		{
			int i = 0;
			bool found = false;
			name = name.ToLower();

			while ((i < names.Length) && !found)
			{
				found = name.Equals(names[i].Name);
				i++;
			}

			return found;
		}


		//========================================================================================
		// Refresh()
		//========================================================================================

		public static void Refresh ()
		{
			var list = new SortedList();
			var all = new SortedList();
			registryPath = null;

			ReadSidName(list, all);
			ReadTnsNames(list, all);

			names = new TnsName[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				names[i] = (TnsName)list.GetByIndex(i);
			}

			allNames = new TnsName[all.Count];
			for (int i = 0; i < all.Count; i++)
			{
				allNames[i] = (TnsName)all.GetByIndex(i);
			}

			list = null;
			all = null;
		}


		//========================================================================================
		// ReadSidName()
		//		Find the default TNS name specified in either the ORACLE_SID environment
		//		variable or the registry.  The environment variable takes precedent.
		//========================================================================================

		private static void ReadSidName (SortedList list, SortedList all)
		{
			string sid = null;
			TnsName name;

			sid = System.Environment.GetEnvironmentVariable("ORACLE_SID");
			if (sid != null)
			{
				sid = sid.ToLower();
				name = new TnsName(sid, Location.Environment, null);
				list.Add(sid, name);

				all.Add(sid + Location.Environment.ToString(), name);
			}

			sid = DatabaseSetup.OracleSid;
			if (sid != String.Empty)
			{
				sid = sid.ToLower();
				registryPath = DatabaseSetup.OracleHomeKey;
				name = new TnsName(sid, Location.Registry, registryPath);

				if (!list.Contains(sid))
				{
					list.Add(sid, name);
				}

				all.Add(sid + Location.Registry.ToString(), name);
			}
		}


		//========================================================================================
		// ReadTnsNames()
		//		Parse the tnsnames.ora file.  This routine essentially filters out all
		//		comments and anything between paranthesis, leaving only the TNS names.
		//========================================================================================

		private static void ReadTnsNames (SortedList list, SortedList all)
		{
			string home = String.Empty;

			WowRegistryKey key =
				WowRegistry.ClassesRoot.OpenSubKey("OracleDatabase.OracleDatabase\\CurVer");

			if (key == null)
			{
				home = DatabaseSetup.OracleHome;
			}
			else
			{
				key = WowRegistry.ClassesRoot.OpenSubKey((string)key.GetValue(null) + "\\CLSID");
				if (key != null)
				{
					string clsid = (string)key.GetValue(null);

					key = WowRegistry.ClassesRoot.OpenSubKey("CLSID\\" + clsid + "\\LocalServer32");
					if (key != null)
					{
						string oracon = (string)key.GetValue(null);
						if (!String.IsNullOrEmpty(oracon))
						{
							home = oracon.Substring(0, oracon.ToLower().IndexOf("\\oracon"));

							if (home.StartsWith("\""))
								home = home.Substring(1);
						}
					}
				}
			}

			var content = new StringBuilder();
			tnsNamesOraPath = Path.Combine(home, @"network\admin\tnsnames.ora");

			try
			{
				using (var reader = new StreamReader(
					(Stream)File.OpenRead(tnsNamesOraPath),
					System.Text.Encoding.ASCII))
				{
					char ch;
					int depth = 0;
					bool inComment = false;

					while (reader.Peek() >= 0)
					{
						ch = (char)reader.Read();

						if (inComment)
						{
							if (ch == 10)
								inComment = false;
						}
						else
						{
							if (ch == '#')
								inComment = true;
							else if (ch == '(')
								depth++;
							else if (ch == ')')
								depth--;
							else if ((depth == 0) && !Char.IsWhiteSpace(ch))
								content.Append(ch);
						}
					}

					reader.Close();
				}
			}
			catch (Exception)
			{
				return;
			}

			if (content.Length > 0)
			{
				string[] names = content.ToString().Split('=');
				string name;
				TnsName tname;

				for (int i = 0; i < names.Length; i++)
				{
					name = names[i].Trim().ToLower();

					if ((name.Length > 0) &&
						(name.IndexOf("http") < 0) &&
						(name.IndexOf("extproc") < 0))
					{
						tname = new TnsName(name, Location.TnsNames, null);

						if (!list.Contains(name))
						{
							list.Add(name, tname);
						}

						all.Add(name + Location.TnsNames.ToString(), tname);
					}
				}
			}
		}
	}
}
