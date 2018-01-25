//************************************************************************************************
// Copyright © 2002-2005 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.Orqa.UnitTests
{
	using System;
	using System.Text;
	using System.Windows.Forms;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using River.Orqa.Dialogs;
	using River.Orqa.Resources;


	[TestClass]
	public class DialogTests
	{
		[TestMethod]
		public void Except ()
		{
			Application.EnableVisualStyles();

			try
			{
				throw new Exception("Foo Exception",
					new Exception("InnerFoo Exception"));
			}
			catch (Exception exc)
			{
				Application.EnableVisualStyles();
				ExceptionDialog dialog = new ExceptionDialog(exc);
				dialog.ShowDialog();
			}
		}


		[TestMethod]
		public void NewItem ()
		{
			Application.EnableVisualStyles();

			NewItemDialog dialog = new NewItemDialog();

			Translator translator = new Translator("Dialogs.NewItems");
			dialog.SetTemplateConfiguration(translator.GetString("SqlProjectItems"));

			DialogResult result = dialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				Console.WriteLine("Filename: [" + dialog.FileName + "]");
				Console.WriteLine("Content: [" + dialog.TemplateContent + "]");
			}
			else
			{
				Console.WriteLine("Dialog cancelled.");
			}
		}
	}
}
