//************************************************************************************************
// Copyright © 2002-2007 Steven M. Cohn. All Rights Reserved.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 05-Dec-2007      New
// 11-Aug-2009      Add Windows 7 support
//************************************************************************************************

namespace River.Native
{
	using System;
	using System.Management;


	//********************************************************************************************
	// class WowEnvironment
	//********************************************************************************************

	/// <summary>
	/// Provides convenient properties to determine the specifc operating system product
	/// name and bit architecture.
	/// </summary>
	/// <remarks>
	/// There are other attributes of the operating system that may be of interest but these
	/// are provided by the .NET System classes.  For example, the major and minor OS version
	/// and service pack versions are both available via the System.Environment.OSVersion.Version
	/// property; here the MajorRevision and MinorRevision properties describe the service pack
	/// version.
	/// </remarks>

	public static class WowEnvironment
	{

		/// <summary>
		/// Specifies the recognized Windows platforms.  Not all platforms may be fully
		/// supported by Everest.  The installer uses the WindowsPlatform to identify
		/// current Microsoft-supported OSes and compare that with Everest-supported OSes.
		/// </summary>

		public enum WindowsPlatform
		{
			/// <summary>
			/// An unrecognized or unsupported platform.
			/// </summary>

			Unknown,

			/// <summary>
			/// The Windows Vista platform including all minor versions and service packs.
			/// </summary>

			Vista,

			/// <summary>
			/// The Windows 7 platform including all minor versions and service packs.
			/// </summary>

			Windows7,

			/// <summary>
			/// The Windows 8 platform including all minor versions and service packs.
			/// </summary>

			Windows8,

			/// <summary>
			/// The Windows 95 platform including all minor versions and service packs.
			/// </summary>

			Windows95,

			/// <summary>
			/// The Windows 98 platform including all minor versions and service packs.
			/// </summary>

			Windows98,

			/// <summary>
			/// The Windows 2000 platform including all minor versions and service packs.
			/// </summary>

			Windows2000,

			/// <summary>
			/// The Windows 2003 platform including all minor versions and service packs.
			/// </summary>

			Windows2003,

			/// <summary>
			/// The Windows Me platform including all minor versions and service packs.
			/// </summary>

			WindowsMe,

			/// <summary>
			/// The Windows NT 3.51 platform including all minor versions and service packs.
			/// </summary>

			WindowsNT3,

			/// <summary>
			/// The Windows NT 4.0 platform including all minor versions and service packs.
			/// </summary>

			WindowsNT4,

			/// <summary>
			/// The Windows XP platform including all minor versions and service packs.
			/// </summary>

			WindowsXP
		}


		private static string fullOSName;
		private static string shortOSName;
		private static bool? is64BitMachine;
		private static bool? isWowProcess;
		private static WindowsPlatform? platform;


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets a Boolean value indicating whether the current machine is based on a 64-bit
		/// architecture.
		/// </summary>
		/// <value>
		/// <b>true</b> if this machine is running a 64-bit CPU; otherwise, <b>false</b>.
		/// </value>

		public static bool Is64BitMachine
		{
			get
			{
				if (is64BitMachine == null)
				{
					Initialize();
				}

				return (bool)is64BitMachine;
			}
		}


		public static bool IsWowProcess
		{
			get
			{
				if (isWowProcess == null)
				{
					Initialize();
				}

				return (bool)isWowProcess;
			}
		}


		/// <summary>
		/// Gets the full Microsoft-supplied product name of the current operating system.
		/// </summary>

		public static string FullOSName
		{
			get
			{
				if (fullOSName == null)
				{
					Initialize();
				}

				return fullOSName;
			}
		}


		/// <summary>
		/// Gets the WindowsPlatform specifying the current base operating system.
		/// </summary>

		public static WindowsPlatform Platform
		{
			get
			{
				if (platform == null)
				{
					platform = WindowsPlatform.Unknown;

					// listed in order of most expected to least expected
					if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						switch (Environment.OSVersion.Version.Major)
						{
							case 6:
								if (Environment.OSVersion.Version.Minor >= 2)
								{
									platform = WindowsPlatform.Windows8;
								}
								else if (Environment.OSVersion.Version.Minor >= 1)
								{
									platform = WindowsPlatform.Windows7;
								}
								else
								{
									platform = WindowsPlatform.Vista;
								}
								break;

							case 5:
								if (Environment.OSVersion.Version.Minor == 0)
								{
									platform = WindowsPlatform.Windows2000;
								}
								else if (Environment.Version.MajorRevision == 2)
								{
									platform = WindowsPlatform.Windows2003;
								}
								else
								{
									platform = WindowsPlatform.WindowsXP;
								}
								break;

							case 4:
								platform = WindowsPlatform.WindowsNT4;
								break;

							case 3:
								platform = WindowsPlatform.WindowsNT3;
								break;
						}
					}
					else if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
					{
						switch (Environment.OSVersion.Version.Minor)
						{
							case 0:
								platform = WindowsPlatform.Windows95;
								break;

							case 10:
								platform = WindowsPlatform.Windows98;
								break;

							case 90:
								platform = WindowsPlatform.WindowsMe;
								break;
						}
					}
				}

				return (WindowsPlatform)platform;
			}
		}


		/// <summary>
		/// Gets a customized "short name" of the current operating system based on the
		/// <i>Platform</i> attribute.
		/// </summary>

		public static string ShortOSName
		{
			get
			{
				if (shortOSName == null)
				{
					switch (Platform)  // Use the Capital Property
					{
						// listed in order of most expected to least expected
						case WindowsPlatform.Vista: shortOSName = "Vista"; break;
						case WindowsPlatform.Windows7: shortOSName = "Windows 7"; break;
						case WindowsPlatform.Windows8: shortOSName = "Windows 8"; break;
						case WindowsPlatform.Windows2003: shortOSName = "Windows 2003"; break;
						case WindowsPlatform.Windows2000: shortOSName = "Windows 2000"; break;
						case WindowsPlatform.WindowsXP: shortOSName = "Windows XP"; break;
						case WindowsPlatform.WindowsNT4: shortOSName = "Windows NT 4.0"; break;
						case WindowsPlatform.WindowsNT3: shortOSName = "Windows NT 3.51"; break;
						case WindowsPlatform.Windows95: shortOSName = "Windows 95"; break;
						case WindowsPlatform.Windows98: shortOSName = "Windows 98"; break;
						case WindowsPlatform.WindowsMe: shortOSName = "Windows Me"; break;
						case WindowsPlatform.Unknown: shortOSName = "Unknown"; break;
					}
				}

				return shortOSName;
			}
		}


		//========================================================================================
		// Initialize()
		//========================================================================================

		static void Initialize ()
		{
			ManagementObject container;				// object containing properties
			ManagementObjectSearcher searcher;
			ManagementObjectCollection collection;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator;

			// get the address width

			searcher = new ManagementObjectSearcher("SELECT AddressWidth FROM Win32_Processor");
			collection = searcher.Get();
			enumerator = collection.GetEnumerator();

			if (enumerator.MoveNext())
			{
				container = (ManagementObject)enumerator.Current;
				is64BitMachine = (UInt16)(container["AddressWidth"]) == (UInt16)64;
			}

			// get the process memory length

			// IntPtr.Size should be 4 in a 32-bit process regardless of CPU
			// IntPtr.Size should be 8 in a 64-bit process
			isWowProcess = (bool)((bool)is64BitMachine && (IntPtr.Size == 4));

			// get the OS name

			searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_OperatingSystem");
			collection = searcher.Get();
			enumerator = collection.GetEnumerator();

			if (enumerator.MoveNext())
			{
				container = (ManagementObject)enumerator.Current;
				fullOSName = (string)container["Name"];
			}

			WindowsPlatform p = Platform;
			string s = ShortOSName;
		}
	}
}
