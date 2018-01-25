//************************************************************************************************
// Copyright © 2002-2007 Steven M. Cohn. All Rights Reserved.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 05-Dec-2007      New
//************************************************************************************************

namespace River.Native
{
	using System;
	using System.Collections.Generic;
	using System.Management;
	using System.Runtime.InteropServices;
	using System.Text;
	using Microsoft.Win32;


	//********************************************************************************************
	// class WowRegistry
	//********************************************************************************************

	public class WowRegistry
	{
		internal static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(-2147483648);
		internal static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);
		internal static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);
		internal static readonly IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);
		internal static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);
		internal static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);
		internal static readonly IntPtr HKEY_USERS = new IntPtr(-2147483645);

		internal const int KEY_QUERY_VALUE = 0x1;
		internal const int KEY_SET_VALUE = 0x2;
		internal const int KEY_CREATE_SUB_KEY = 0x4;
		internal const int KEY_ENUMERATE_SUB_KEYS = 0x8;
		internal const int KEY_NOTIFY = 0x10;
		internal const int KEY_CREATE_LINK = 0x20;
		internal const int KEY_WOW64_32KEY = 0x200;
		internal const int KEY_WOW64_64KEY = 0x100;
		internal const int KEY_WOW64_RES = 0x300;

		internal const int ERR_KEY_NOT_FOUND = 2;

		private static WowRegistryKey classesRoot = null;
		private static WowRegistryKey currentUser = null;
		private static WowRegistryKey localMachine = null;
		private static WowRegistryKey users = null;
		private static WowRegistryKey performanceData = null;
		private static WowRegistryKey currentConfig = null;
		private static WowRegistryKey dynData = null;

		private static bool forceRedirection = false;


		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		internal static extern int RegOpenKeyEx (
			IntPtr hKey,
			string lpSubKey,
			int ulOptions,
			int samDesired,
			out IntPtr hkResult);


		//========================================================================================
		// Hives
		//========================================================================================

		/// <summary>
		/// Gets the Windows Registry base key HKEY_CLASSES_ROOT.
		/// </summary>

		public static WowRegistryKey ClassesRoot
		{
			get
			{
				if (classesRoot == null)
					classesRoot = Open(RegistryHive.ClassesRoot, String.Empty);

				return classesRoot;
			}
		}



		/// <summary>
		/// Gets the Windows Registry base key HKEY_CURRENT_CONFIG.
		/// </summary>

		public static WowRegistryKey CurrentConfig
		{
			get
			{
				if (currentConfig == null)
					currentConfig = Open(RegistryHive.CurrentConfig, String.Empty);

				return currentConfig;
			}
		}


		/// <summary>
		/// Gets the Windows Registry base key HKEY_CURRENT_USER.
		/// </summary>

		public static WowRegistryKey CurrentUser
		{
			get
			{
				if (currentUser == null)
					currentUser = Open(RegistryHive.CurrentUser, String.Empty);

				return currentUser;
			}
		}


		/// <summary>
		/// Gets the Windows Registry base key HKEY_DYN_DATA.
		/// </summary>

		public static WowRegistryKey DynData
		{
			get
			{
				if (dynData == null)
					dynData = Open(RegistryHive.DynData, String.Empty);

				return dynData;
			}
		}


		/// <summary>
		/// Gets the Windows Registry base key HKEY_LOCAL_MACHINE.
		/// </summary>

		public static WowRegistryKey LocalMachine
		{
			get
			{
				if (localMachine == null)
					localMachine = Open(RegistryHive.LocalMachine, String.Empty);

				return localMachine;
			}
		}


		/// <summary>
		/// Gets the Windows Registry base key HKEY_PERFORMANCE_DATA.
		/// </summary>

		public static WowRegistryKey PerformanceData
		{
			get
			{
				if (performanceData == null)
					performanceData = Open(RegistryHive.PerformanceData, String.Empty);

				return performanceData;
			}
		}


		/// <summary>
		/// Gets the Windows Registry base key HKEY_USERS.
		/// </summary>

		public static WowRegistryKey Users
		{
			get
			{
				if (users == null)
					users = Open(RegistryHive.Users, String.Empty);

				return users;
			}
		}


		/// <summary>
		/// 
		/// </summary>

		public static bool ForceRedirection
		{
			get { return forceRedirection; }
			set { forceRedirection = value; }
		}


		//========================================================================================
		// Open()
		//========================================================================================

		/// <summary>
		/// Initiates a WOW Registry session by opening a new WowRegistryKey.
		/// </summary>
		/// <param name="hive">The Registry hive containing the key.</param>
		/// <param name="keyName">The name of the key beneath the specified hive.</param>
		/// <returns>A WowRegistryKey used to access the data and subkeys of the key.</returns>

		public static WowRegistryKey Open (RegistryHive hive, string keyName)
		{
			return Open(hive, keyName, forceRedirection);
		}


		/// <summary>
		/// Initiates a WOW Registry session by opening a new WowRegistryKey, optionally
		/// redirecting from default (64-bit) hive to redirected (32-bit) hive.
		/// </summary>
		/// <param name="hive">The Registry hive containing the key.</param>
		/// <param name="keyName">The name of the key beneath the specified hive.</param>
		/// <param name="redirect"><b>true</b> to redirect to 32-bit hive.</param>
		/// <returns>A WowRegistryKey used to access the data and subkeys of the key.</returns>

		public static WowRegistryKey Open (RegistryHive hive, string keyName, bool redirect)
		{
			WowRegistryKey key = null;
			IntPtr hKey = IntPtr.Zero;

			if (WowEnvironment.Is64BitMachine)
			{
				IntPtr hiveId = IntPtr.Zero;

				switch (hive)
				{
					case RegistryHive.ClassesRoot: hiveId = HKEY_CLASSES_ROOT; break;
					case RegistryHive.CurrentConfig: hiveId = HKEY_CURRENT_CONFIG; break;
					case RegistryHive.CurrentUser: hiveId = HKEY_CURRENT_USER; break;
					case RegistryHive.DynData: hiveId = HKEY_DYN_DATA; break;
					case RegistryHive.LocalMachine: hiveId = HKEY_LOCAL_MACHINE; break;
					case RegistryHive.PerformanceData: hiveId = HKEY_PERFORMANCE_DATA; break;
					case RegistryHive.Users: hiveId = HKEY_USERS; break;
				}

				int samFlags = KEY_QUERY_VALUE | KEY_ENUMERATE_SUB_KEYS;

				if (redirect)
				{
					samFlags |= WowRegistry.KEY_WOW64_32KEY;
				}
				else if (WowEnvironment.IsWowProcess)
				{
					samFlags |= WowRegistry.KEY_WOW64_64KEY;
				}

				int result = RegOpenKeyEx(
					hiveId,
					keyName,
					0,
					samFlags,
					out hKey);

				if (result != ERR_KEY_NOT_FOUND)
				{
					if (result != 0)
					{
						throw new Exception(
							String.Format("WowRegistry.Open(), error={0}", result));
					}

					key = new WowRegistryKey(hKey, null, hive, keyName);
				}
			}
			else
			{
				RegistryKey regKey = null;

				if ((keyName == null) || keyName.Equals(String.Empty))
				{
					switch (hive)
					{
						case RegistryHive.ClassesRoot: regKey = Registry.ClassesRoot; break;
						case RegistryHive.CurrentConfig: regKey = Registry.CurrentConfig; break;
						case RegistryHive.CurrentUser: regKey = Registry.CurrentUser; break;
						case RegistryHive.LocalMachine: regKey = Registry.LocalMachine; break;
						case RegistryHive.PerformanceData: regKey = Registry.PerformanceData; break;
						case RegistryHive.Users: regKey = Registry.Users; break;
					}

					hKey = new IntPtr(regKey.GetHashCode());
					key = new WowRegistryKey(hKey, regKey, hive, keyName);
				}
				else
				{
					regKey = Registry.LocalMachine.OpenSubKey(keyName);
					if (regKey != null)
					{
						hKey = new IntPtr(regKey.GetHashCode());
						key = new WowRegistryKey(hKey, regKey, hive, keyName);
					}
				}
			}

			return key;
		}
	}
}
