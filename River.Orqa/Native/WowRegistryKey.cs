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
	using System.Management;
	using System.Runtime.InteropServices;
	using System.Text;
	using Microsoft.Win32;


	//********************************************************************************************
	// class WowRegistryKey
	//********************************************************************************************

	/// <summary>
	/// 
	/// </summary>

	public class WowRegistryKey
	{
		private const int REG_BINARY = 3;
		private const int REG_DWORD = 4;
		private const int REG_DWORD_BIG_ENDIAN = 5;
		private const int REG_DWORD_LITTLE_ENDIAN = 4;
		private const int REG_EXPAND_SZ = 2;
		private const int REG_LINK = 6;
		private const int REG_MULTI_SZ = 7;
		private const int REG_NONE = 0;
		private const int REG_QWORD = 11;
		private const int REG_QWORD_LITTLE_ENDIAN = 11;
		private const int REG_SZ = 1;

		private IntPtr intKey;
		private RegistryKey regKey;
		private RegistryHive hive;
		private string name;
		private bool redirected;

		#region Interop

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegEnumKeyEx (
			IntPtr hKey,
			int dwIndex,
			StringBuilder lpName,
			out int lpcbName,
			int[] lpReserved,
			StringBuilder lpClass,
			int[] lpcbClass,
			long[] lpftLastWriteTime);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegQueryInfoKey (
			IntPtr hKey,
			StringBuilder lpClass,
			int[] lpcbClass,
			IntPtr lpReserved_MustBeZero,
			ref int lpcSubKeys,
			int[] lpcbMaxSubKeyLen,
			int[] lpcbMaxClassLen,
			ref int lpcValues,
			int[] lpcbMaxValueNameLen,
			int[] lpcbMaxValueLen,
			int[] lpcbSecurityDescriptor,
			int[] lpftLastWriteTime);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegQueryValueEx (
			IntPtr hKey, string lpValueName,
			int[] lpReserved,
			ref int lpType,
			ref int lpData,
			ref int lpcbData);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegQueryValueEx (
			IntPtr hKey,
			string lpValueName,
			int[] lpReserved,
			ref int lpType,
			ref long lpData,
			ref int lpcbData);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegQueryValueEx (
			IntPtr hKey,
			string lpValueName,
			int[] lpReserved,
			ref int lpType,
			[Out] byte[] lpData,
			ref int lpcbData);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegQueryValueEx (
			IntPtr hKey,
			string lpValueName, 
			int[] lpReserved, 
			ref int lpType, 
			[Out] char[] lpData, 
			ref int lpcbData);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegQueryValueEx (
			IntPtr hKey, 
			string lpValueName, 
			int[] lpReserved, 
			ref int lpType, 
			StringBuilder lpData, 
			ref int lpcbData);

		#endregion Interop


		//========================================================================================
		// Constructor
		//========================================================================================

		internal WowRegistryKey (
			IntPtr intKey, RegistryKey regKey, RegistryHive hive, string name)
		{
			this.intKey = intKey;
			this.regKey = regKey;
			this.hive = hive;
			this.name = name;
			this.redirected = (WowEnvironment.Is64BitMachine && name.Contains("Wow6432Node"));
		}

	
		//========================================================================================
		// Properties
		//========================================================================================

		public IntPtr IntKey
		{
			get { return intKey; }
		}

	
		public RegistryKey RegKey
		{
			get { return regKey; }
		}


		public RegistryHive Hive
		{
			get { return hive; }
		}

	
		public string Name
		{
			get { return name; }
		}


		//========================================================================================
		// GetSubkeyCount()
		//========================================================================================

		/// <summary>
		/// Retrieves the number of subkeys beneath the current Registry key.
		/// </summary>
		/// <returns>The number of subkeys.</returns>

		public int GetSubKeyCount ()
		{
			int count = 0;

			if (regKey == null)
			{
				int values = 0;

				int errorCode = RegQueryInfoKey(
					intKey, null, null, IntPtr.Zero, ref count,
					null, null, ref values,
					null, null, null, null);

				if (errorCode != 0)
				{
					throw new Exception(
						String.Format("WowRegistryKey.GetSubKeyCount(), error={0}", errorCode));
				}
			}
			else
			{
				count = regKey.GetSubKeyNames().Length;
			}

			return count;
		}


		//========================================================================================
		// GetSubKeyNames()
		//========================================================================================

		/// <summary>
		/// Retrieves an array of strings that contains all the subkey names.
		/// </summary>
		/// <returns>An array of strings that contains the names of the subkeys for the current key.</returns>

		public string[] GetSubKeyNames ()
		{
			string[] names = null;

			if (regKey == null)
			{
				int count = GetSubKeyCount();
				names = new string[count];
				if (count > 0)
				{
					StringBuilder name = new StringBuilder(0x100);  // 256
					for (int i = 0; i < count; i++)
					{
						int capacity = name.Capacity;

						int errorCode = RegEnumKeyEx(
							intKey, i, name, out capacity,
							null, null, null, null);

						if (errorCode != 0)
						{
							throw new Exception(String.Format(
								"WowRegistryKey.GetSubKeyNames(), error={0}", errorCode));
						}

						names[i] = name.ToString();
					}
				}
			}
			else
			{
				names = regKey.GetSubKeyNames();
			}

			return names;
		}


		//========================================================================================
		// GetValue()
		//========================================================================================

		/// <summary>
		/// Retrieves the value associated with the specified name. Returns <b>null</b> if the
		/// name/value pair does not exist in the registry.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <returns>
		/// The value associated with <i>name</i> or <b>null</b> if name is not found.
		/// </returns>

		public object GetValue (string name)
		{
			return GetValueInternal(name, null, true);
		}


		/// <summary>
		/// Retrieves the value associated with the specified name. Returns <b>null</b> if the
		/// name/value pair does not exist in the registry.
		/// </summary>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="defaultVal">The default value to return if the name is not found.</param>
		/// <returns>
		/// The value associated with <i>name</i> or <b>null</b> if name is not found.
		/// </returns>

		public object GetValue (string name, object defaultVal)
		{
			return GetValueInternal(name, defaultVal, true);
		}


		/// <summary>
		/// Retrieves the value associated with the specified name. Returns <b>null</b> if the
		/// name/value pair does not exist in the registry.
		/// </summary>
		/// <param name="defaultVal">The default value to return if the name is not found.</param>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="expandVars">True to expand embedded environment variables.</param>
		/// <returns>
		/// The value associated with <i>name</i> or <b>null</b> if name is not found.
		/// </returns>

		public object GetValue (string name, object defaultVal, bool expandVars)
		{
			return GetValueInternal(name, defaultVal, expandVars);
		}


		private object GetValueInternal (string name, object defaultValue, bool expandVars)
		{
			object value = defaultValue;

			if (regKey == null)
			{
				int type = 0;
				int data = 0;
				int errorCode;

				errorCode = RegQueryValueEx(
					intKey, name, null, ref type, (byte[])null, ref data);

				if (errorCode == WowRegistry.ERR_KEY_NOT_FOUND)
				{
					return value;
				}

				if (errorCode != 0)
				{
					throw new Exception(String.Format(
						"WowRegistryKey.GetValue(), error={0}", errorCode));
				}

				switch (type)
				{
					case REG_SZ: // 1;
						{
							StringBuilder builder = new StringBuilder(data / 2);
							errorCode = RegQueryValueEx(intKey, name, null, ref type, builder, ref data);
							value = builder.ToString();
						}
						break;

					case REG_EXPAND_SZ: // 2;
						{
							StringBuilder builder = new StringBuilder(data / 2);
							errorCode = RegQueryValueEx(intKey, name, null, ref type, builder, ref data);
							if (!expandVars)
							{
								return Environment.ExpandEnvironmentVariables(builder.ToString());
							}
							value = builder.ToString();
						}
						break;

					case REG_BINARY: // 3;
					case REG_DWORD_BIG_ENDIAN: // 5;
						{
							byte[] binary = new byte[data];
							errorCode = RegQueryValueEx(intKey, name, null, ref type, binary, ref data);
							value = binary;
						}
						break;

					case REG_DWORD: // 4;
						{
							int dword = 0;
							errorCode = RegQueryValueEx(intKey, name, null, ref type, ref dword, ref data);
							value = dword;
						}
						break;

					/*
					case REG_MULTI_SZ: // 7;
						bool flag = _SystemDefaultCharSize != 1;
						list = new ArrayList();
						if (!flag)
						{
							byte[] buffer5 = new byte[lpcbData];
							num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref lpType, buffer5, ref lpcbData);
							int index = 0;
							int num12 = buffer5.Length;
							while ((num3 == 0) && (index < num12))
							{
								int num13 = index;
								while ((num13 < num12) && (buffer5[num13] != 0))
								{
									num13++;
								}
								if (num13 < num12)
								{
									if ((num13 - index) > 0)
									{
										list.Add(Encoding.Default.GetString(buffer5, index, num13 - index));
									}
									else if (num13 != (num12 - 1))
									{
										list.Add(string.Empty);
									}
								}
								else
								{
									list.Add(Encoding.Default.GetString(buffer5, index, num12 - index));
								}
								index = num13 + 1;
							}
							break;
						}
						char[] chArray = new char[lpcbData / 2];
						num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref lpType, chArray, ref lpcbData);
						int startIndex = 0;
						int length = chArray.Length;
						while ((num3 == 0) && (startIndex < length))
						{
							int num10 = startIndex;
							while ((num10 < length) && (chArray[num10] != '\0'))
							{
								num10++;
							}
							if (num10 < length)
							{
								if ((num10 - startIndex) > 0)
								{
									list.Add(new string(chArray, startIndex, num10 - startIndex));
								}
								else if (num10 != (length - 1))
								{
									list.Add(string.Empty);
								}
							}
							else
							{
								list.Add(new string(chArray, startIndex, length - startIndex));
							}
							startIndex = num10 + 1;
						}
						break;
					*/

					case REG_QWORD: // 11;
						{
							int qword = 0;
							errorCode = RegQueryValueEx(intKey, name, null, ref type, ref qword, ref data);
							value = qword;
						}
						break;
				}
			}
			else
			{
				value = regKey.GetValue(name);
			}

			return value;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>

		public WowRegistryKey OpenSubKey (string name)
		{
			WowRegistryKey key = null;
			IntPtr hKey;

			if (regKey == null)
			{
				int samFlags = WowRegistry.KEY_QUERY_VALUE | WowRegistry.KEY_ENUMERATE_SUB_KEYS;
				
				// [SMC 20090811] Since WowRegistry is taking care of opening the proper key
				// we shouldn't need to redirect again here...

				//if (redirected)
				//    samFlags |= WowRegistry.KEY_WOW64_64KEY;
				//else
				//    samFlags |= WowRegistry.KEY_WOW64_32KEY;

				if (WowRegistry.ForceRedirection)
				{
					samFlags |= WowRegistry.KEY_WOW64_32KEY;
				}

				int result = WowRegistry.RegOpenKeyEx(
					this.intKey,
					name,
					0,
					samFlags,
					out hKey);

				if (result != WowRegistry.ERR_KEY_NOT_FOUND)
				{
					if (result != 0)
					{
						throw new Exception(
							String.Format("WowRegistryKey.OpenSubKey(), error={0}", result));
					}

					key = new WowRegistryKey(hKey, null, this.hive, name);
				}
			}
			else
			{
				RegistryKey subKey = regKey.OpenSubKey(name);

				if (subKey != null)
				{
					string subName = this.name + "\\" + name;

					hKey = new IntPtr(subKey.GetHashCode());
					key = new WowRegistryKey(hKey, subKey, this.hive, subName);
				}
			}

			return key;
		}
	}
}
