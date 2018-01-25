//************************************************************************************************
// Copyright © 2002-2005 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.Orqa.UnitTests
{
	using System;
	using System.Drawing;
	using System.Drawing.Printing;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Soap;
	using System.Text;
	using System.Windows.Forms;
	using Microsoft.VisualStudio.TestTools.UnitTesting;


	[TestClass]
	public class PageSettingsTest
	{
		[TestMethod]
		public void PageSetupTest ()
		{
			PageSetupDialog dialog = new PageSetupDialog();
			dialog.PageSettings = new PageSettings();
			dialog.PrinterSettings = new PrinterSettings();

			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				MemoryStream stream = new MemoryStream();
				SoapFormatter formatter = new SoapFormatter();

				formatter.Serialize(stream, dialog.PageSettings);
				byte[] buffer = stream.GetBuffer();
				ASCIIEncoding encoder = new ASCIIEncoding();
				string s = encoder.GetString(buffer);

				Console.Out.WriteLine("PageSettings -----------------------------------------");
				Console.Out.WriteLine(s);

				Console.Out.WriteLine("PrinterSettings --------------------------------------");
			}
		}
	}
}
