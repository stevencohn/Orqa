
namespace River.Native.Test
{
	using System;
	using System.Text;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Microsoft.Win32;
	using River.Native;


	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class WowRegistryTest
	{

		[TestMethod]
		public void OpenTest ()
		{
			WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(@"SOFTWARE\Oracle");
			Assert.IsNotNull(key);
		}


		[TestMethod]
		public void OpenSubKeyTest ()
		{
			WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(@"SOFTWARE\Oracle");
			Assert.IsNotNull(key);

			WowRegistryKey subkey = key.OpenSubKey("KEY_OraDb11g_home1");
			Assert.IsNotNull(subkey);
		}


		[TestMethod]
		public void GetSubKeyCountTest ()
		{
			WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(@"SOFTWARE\Oracle");
			int count = key.GetSubKeyCount();
			Console.WriteLine(String.Format("GetSubKeyCountTest count=[{0}]", count));
			Assert.IsTrue(count > 0);

			RegistryKey nativeHKLM = Registry.LocalMachine;
			if (nativeHKLM != null)
			{
				RegistryKey nativeKey = nativeHKLM.OpenSubKey(@"SOFTWARE\Oracle");
				if (nativeKey != null)
				{
					string[] names = nativeKey.GetSubKeyNames();
				}
			}
		}


		[TestMethod]
		public void GetSubKeyNamesTest ()
		{
			WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(@"SOFTWARE\Oracle");
			string[] names = key.GetSubKeyNames();
			Assert.IsNotNull(names);
			Assert.IsTrue(names.Length > 0);

			foreach (string name in names)
				Console.WriteLine(String.Format("Subkey name=[{0}]", name));
		}

		[TestMethod]
		public void GetValueTest ()
		{
			WowRegistryKey key =
				WowRegistry.LocalMachine.OpenSubKey(@"SOFTWARE\ORACLE\KEY_OraDb11g_home1");

			Assert.IsNotNull(key);

			string value = (string)key.GetValue("ORACLE_HOME");
			Console.WriteLine(String.Format("GetValueTest value=[{0}]", value));
			Assert.IsNotNull(value);
		}

		[TestMethod]
		public void GetUnknownValueTest ()
		{
			WowRegistryKey key = WowRegistry.LocalMachine.OpenSubKey(@"Does_Not_Exist_Key");
			Assert.IsNull(key);
		}
	}
}
