//************************************************************************************************
// Copyright © 2002-2005 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.Orqa.UnitTests
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Xml.Serialization;
	using System.Xml.XPath;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using River.Orqa.Options;


	[TestClass]
	public class UserOptionsTests
	{
		[TestMethod]
		public void FileAssocTest ()
		{
			FileAssociation assoc = new FileAssociation("sqlfoo");
			assoc.DisplayName = "SQL Script";
			assoc.ProperName = "sqlfooscript";
			assoc.AddCommand("open", "C:\\River\\Orqa2\\Orqa\\bin\\debug\\orqa.exe %1");
			assoc.Create();
		}


		[TestMethod]
		public void Font ()
		{
			System.Drawing.Font systemFont = new System.Drawing.Font("Tahoma", 8.0f, FontStyle.Regular);
			Options.OptionFont font = new Options.OptionFont(systemFont);

			XmlSerializer serializer = new XmlSerializer(typeof(Options.OptionFont));
			StringBuilder buffer = new StringBuilder();

			serializer.Serialize(new StringWriter(buffer), font);

			Console.WriteLine("-");
			Console.WriteLine(buffer.ToString());
			Console.WriteLine("-");

			Options.OptionFont tnof
				= (Options.OptionFont)serializer.Deserialize(new StringReader(buffer.ToString()));

			Assert.AreEqual(font.ToString(), tnof.ToString());

			XPathDocument doc = new XPathDocument(new StringReader(buffer.ToString()));
			XPathNavigator nav = doc.CreateNavigator();
			nav.MoveToRoot();
			nav.MoveToNext(XPathNodeType.Element);
			Options.OptionFont xfont = new Options.OptionFont(nav);

			Assert.AreEqual(font.ToString(), xfont.ToString());
		}


		[TestMethod]
		public void OptionsTest ()
		{
			Console.WriteLine("OptionsTest ------------------------------");
			Console.WriteLine(UserOptions.Defaults.ToString(System.Xml.Linq.SaveOptions.None));
		}


		[TestMethod]
		public void ScreenPosition ()
		{
			Console.WriteLine("ScreenPositionTest ----------------------");
			Rectangle original = new Rectangle(10, 10, 100, 100);
			UserOptions.SaveScreenPosition(original);
			Rectangle position = UserOptions.LoadScreenPosition();

			Assert.IsTrue(position.Equals(original));
			Console.WriteLine(UserOptions.OptionsDoc.ToString(System.Xml.Linq.SaveOptions.None));
		}
	}
}
