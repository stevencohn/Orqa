//************************************************************************************************
// Copyright © 2002-2007 Steven M. Cohn. All Rights Reserved.
//
// Provides a means of discovering basic Oracle setup information.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Jul-2005      New
// 12-Nov-2007      Added Vista 64-bit support
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using River.Native;


	//********************************************************************************************
	// class DatabaseSetup
	//********************************************************************************************

	/// <summary>
	/// Provides a means of discovering basic Oracle setup information.
	/// </summary>

	internal static class DatabaseSetup
	{
		private static bool uninformed = true;			// have not yet read setup info!
		private static bool redirected = false;			// true if 32bit image on 64bit CPU

		private static string registryPrefix;			// software\ or software\wow6432node

		private static string oracleHome = String.Empty;		// install directory of default home
		private static string oracleHomeKey = String.Empty;		// registry key for home
		private static string oracleHomeName = String.Empty;	// simple display name of home
		private static string oracleSid = String.Empty;			// default SID


		//========================================================================================
		// IsOracleInstalled
		//========================================================================================

		/// <summary>
		/// Gets a Boolean value indicating whether Oracle is installed and registered
		/// on the local system.  This simply looks for the HKLM/Software/Oracle key
		/// in the system registry, but does not validate beyond that.
		/// </summary>

		public static bool IsOracleInstalled
		{
			get
			{
				registryPrefix = @"SOFTWARE\ORACLE";
				WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(registryPrefix);

				if (key == null)
				{
					key = WowRegistry.Open(
						Microsoft.Win32.RegistryHive.LocalMachine, @"SOFTWARE\ORACLE", true);
				}

				return (key != null);
			}
		}


		//========================================================================================
		// IsOdpInstalled
		//========================================================================================

		/// <summary>
		/// Gets a Boolean value indicating whether ODP.NET is installed and registered
		/// on the local system.
		/// </summary>

		public static bool IsOdpInstalled
		{
			get
			{
				bool found = false;

				WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(registryPrefix);
				if (key != null)
				{
					found = HasOdpNetKey(key);
				}

				if (!found)
				{
					var subkey = WowRegistry.Open(
						Microsoft.Win32.RegistryHive.LocalMachine, @"SOFTWARE\ORACLE", true);

					if (subkey != null)
					{
						found = HasOdpNetKey(subkey);

						redirected = found;

						if (redirected)
						{
							WowRegistry.ForceRedirection = true;
						}
					}
				}

				return found;
			}
		}


		#region HasOdpNetKey

		// Different versions of Oracle place the ODP.NET key in different locations
		// under the HKLM\SOFTWARE\ORACLE key.  So recurse down the subtree until it
		// is found.

		private static bool HasOdpNetKey (WowRegistryKey key)
		{
			string[] keys = key.GetSubKeyNames();
			bool found = false;
			int i = 0;

			while ((i < keys.Length) && !found)
			{
				if (!(found = keys[i].Equals("ODP.NET")))
				{
					if (!(found = HasOdpNetKey(key.OpenSubKey(keys[i]))))
						i++;
				}
			}

			return found;
		}

		#endregion HasOdpNetKey


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the path of the install directory of the default Oracle home.
		/// </summary>

		public static string OracleHome
		{
			get
			{
				if (uninformed)
					GetSetupInformation();

				return oracleHome;
			}
		}


		/// <summary>
		/// Gets the path of the default Oracle home system Registry key.
		/// </summary>

		public static string OracleHomeKey
		{
			get
			{
				if (uninformed)
					GetSetupInformation();

				return oracleHomeKey;
			}
		}


		/// <summary>
		/// Gets the display name of the default Oracle home.
		/// </summary>

		public static string OracleHomeName
		{
			get
			{
				if (uninformed)
					GetSetupInformation();

				return oracleHomeName;
			}
		}


		/// <summary>
		/// Gets the default SID referred to by the current Oracle home.
		/// </summary>

		public static string OracleSid
		{
			get
			{
				if (uninformed)
				{
					GetSetupInformation();
				}

				return oracleSid;
			}
		}


		//========================================================================================
		// GetSetupInformation()
		//========================================================================================

		#region GetSetupInformation

		private static void GetSetupInformation ()
		{
			var subkey = WowRegistry.LocalMachine.OpenSubKey(registryPrefix);
			if (subkey != null)
			{
				oracleHomeKey = GetHomeKey(subkey);

				if (oracleHomeKey == null)
				{
					oracleHomeKey = GetFirstClientKey(
						WowRegistry.LocalMachine.OpenSubKey(registryPrefix));
				}

				if (oracleHomeKey != null)
				{
					WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(oracleHomeKey);
					if (key != null)
					{
						oracleHome = GetKeyValue(key, "ORACLE_HOME");
						oracleHomeName = GetKeyValue(key, "ORACLE_HOME_NAME");
						oracleSid = GetKeyValue(key, "ORACLE_SID");
					}
				}
			}

			uninformed = false;
		}


		private static string GetHomeKey (WowRegistryKey key)
		{
			string homeKey = (string)key.GetValue("ORACLE_HOME_KEY");

			if ((homeKey == null) && (key != null))
			{
				string[] keys = key.GetSubKeyNames();
				int i = 0;

				while ((i < keys.Length) && (homeKey == null))
				{
					var subkey = key.OpenSubKey(keys[i]);
					if ((homeKey = GetHomeKey(subkey)) == null)
						i++;
				}
			}

			return homeKey;
		}


		private static string GetFirstClientKey (WowRegistryKey key)
		{
			string homeKey = null;

			if (key != null)
			{
				string[] keys = key.GetSubKeyNames();
				int i = 0;

				while ((i < keys.Length) && (homeKey == null))
				{
					if (keys[i].StartsWith("KEY_Everest") ||
						keys[i].StartsWith("KEY_OraClient") ||
						keys[i].StartsWith("KEY_OraOdac"))
					{
						homeKey = registryPrefix + @"\" + keys[i];
					}
					else
					{
						i++;
					}
				}
			}

			return homeKey;
		}


		private static string GetKeyValue (WowRegistryKey key, string name)
		{
			string value = String.Empty;

			if (key != null)
			{
				try
				{
					if ((value = (string)key.GetValue(name)) == null)
					{
						value = String.Empty;
					}
				}
				catch { }
			}

			return value;
		}

		#endregion GetSetupInformation
	}
}
