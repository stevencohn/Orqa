//************************************************************************************************
// Copyright © 2002-2005 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.Orqa.UnitTests
{
	using System;
	using System.Text;
	using System.Collections.Generic;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using River.Orqa.Resources;


	[TestClass]
	public class ResourcesTests
	{
		[TestMethod]
		public void Translate ()
		{
			Translator translator = new Translator("Options");

			string s = translator.GetInvariantString("DefaultOptions");
			Assert.IsNotNull(s);
			Assert.IsTrue(s.Length > 0);

			Console.WriteLine("Options:\n" + s + "\n");
		}
	}
}
