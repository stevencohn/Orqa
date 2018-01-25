//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Manages a Visual Studio Databse Project file commonly having the extension ".dbp"
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 10-Nov-2005		New
// 01-Sep-2013		VS 2012
//************************************************************************************************

namespace River.Orqa.Browser
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml.Linq;


	//********************************************************************************************
	// class ProjectManager
	//********************************************************************************************

	/// <summary>
	/// Manages a Visual Studio Databse Project file commonly having the extension ".dbp"
	/// </summary>

	internal class ProjectManager
	{
		private string Separator = String.Empty + Path.DirectorySeparatorChar;


		public Project LoadProjectFile (string filename)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException();
			}

			filename = Path.GetFullPath(filename);
			var project = ReadProjectFile(filename);
			return project;
		}


		private Project ReadProjectFile (string filename)
		{
			var project = new Project(filename);

			var root = XElement.Load(filename);
			var ns = root.GetDefaultNamespace();

			var paths =
				from e in root
					.Elements(ns + "ItemGroup")
					.Elements(ns + "None")
				select e.Attribute("Include").Value;

			foreach (string path in paths)
			{
				BuildProjectTree(project.Root, path.Split(Separator[0]), 0);
			}

			return project;
		}

		private void BuildProjectTree (ProjectItem parent, string[] parts, int index)
		{
			var item = parent.Items.Where(i => i.Name == parts[index]).FirstOrDefault();
			if (item == null)
			{
				if (index < parts.Length - 1)
				{
					var dir = new DirectoryItem(Path.Combine(parent.FolderPath, parts[index]));
					parent.AddChild(dir);
					BuildProjectTree(dir, parts, index + 1);
				}
				else
				{
					var file = new FileItem(Path.Combine(parent.FolderPath, parts[index]));
					parent.AddChild(file);
				}
			}
			else
			{
				BuildProjectTree(item, parts, index + 1);
			}
		}


		private void ReadDBReferences (StreamReader reader, StringBuilder buffer, int level)
		{
			bool foundEnd = false;
			string line;
			string trimmed;
			string indent = new string(' ', (level + 1) * 2);

			while (!(reader.EndOfStream || foundEnd))
			{
				line = reader.ReadLine();

				trimmed = line.Trim();

				if (trimmed.StartsWith("Begin"))
				{
					buffer.Append(indent + "  " + trimmed + Environment.NewLine);
					ReadDBReferences(reader, buffer, level + 1);
				}
				else if (trimmed.Equals("End"))
				{
					foundEnd = true;

					if (level > 0)
						buffer.Append(indent + trimmed + Environment.NewLine);
				}
				else
				{
					buffer.Append(indent + "  " + trimmed + Environment.NewLine);
				}
			}
		}


		public void SaveProjectFile (Project project)
		{
			if (File.Exists(project.Root.Path))
			{
				FileAttributes atts = File.GetAttributes(project.Root.Path);
				if ((atts & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					var dialog = new Dialogs.OverwriteDialog();
					dialog.Filename = Path.GetFileName(project.Root.Path);
					DialogResult result = dialog.ShowDialog();

					if (result == DialogResult.OK)
					{
						atts ^= FileAttributes.ReadOnly;
						File.SetAttributes(project.Root.Path, atts);
					}
					else
					{
						return;
					}
				}

				File.Delete(project.Root.Path);
			}

			project.Root.Path = Path.GetDirectoryName(project.Root.Path)
				+ "\\" + project.Name + Path.GetExtension(project.Root.Path);

			StreamWriter writer = File.CreateText(project.Root.Path);
			writer.Write(project.GenerateProjectFile());
			writer.Close();
			writer = null;
		}
	}
}
