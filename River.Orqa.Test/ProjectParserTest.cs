//************************************************************************************************
// Copyright © 2002-2005 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.Orqa.UnitTests
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using River.Orqa.Browser;

	[TestClass]
	public class ProjectParserTest
	{
		[TestMethod]
		public void Parse ()
		{
			//Project project = ProjectManager.LoadProjectFile(@"C:\Everest\DBGroup\DB_Security\DB_Security.dbp");
			var manager = new ProjectManager();
			Project project = manager.LoadProjectFile(@"..\..\ProjectParser.dbp");

			Console.WriteLine("Project: " + project.Name + " (" + project.FolderPath + ")");

			DumpProject(project.Root, 1);

			Console.WriteLine();
			Console.WriteLine(project.GenerateProjectFile());

			manager.SaveProjectFile(project);
		}


		private void DumpProject (ProjectItem root, int count)
		{
			string indent = new string('.', count * 2);
			Console.WriteLine(root.Name + " - " + root.Path);

			ProjectItemCollection.Enumerator e = root.Items.GetEnumerator();
			while (e.MoveNext())
			{
				ProjectItem item = e.Current;

				if (item.IsDirectory)
				{
					DumpProject(item, count + 1);
				}
				else
				{
					Console.WriteLine(indent + item.Name + " - " + item.Path);
				}
			}
		}
	}
}
