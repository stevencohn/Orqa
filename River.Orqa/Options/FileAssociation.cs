//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Manages file association registration.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.Collections;
	using System.Security;
	using Microsoft.Win32;


	//************************************************************************************************
	// class FileAssociation
	//************************************************************************************************

	/// <summary>Creates file associations for your programs.</summary>
	/// <example>The following example creates a file association for the type XYZ
	/// with a non-existent program.
	/// <br></br>
	/// <br>C# code</br>
	/// <code>
	/// FileAssociation FA = new FileAssociation("xyz");
	/// FA.ContentType = "application/myprogram";
	/// FA.FullName = "My XYZ Files!";
	/// FA.ProperName = "XYZ File";
	/// FA.AddCommand("open", "C:\\mydir\\myprog.exe %1");
	/// FA.Create();
	/// </code>
	/// </example>

	internal class FileAssociation
	{
		private string extension;					// file extension
		private string properName;					// internal proper name of type
		private string displayName;					// display name of the file type
		private string contentType;					// name of content type of file type
		private string perceivedType;				// name of perceived type of file type
		private string iconPath;					// path to resource file with icon
		private short iconIndex;					// icon index in resource file
		private ArrayList captions;					// names of commands
		private ArrayList commands;					// commands


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// Initializes a new file association manager for the given file type.
		/// </summary>
		/// <param name="extension">
		/// The file extension.  If this does not begin with a period, it will be added.
		/// </param>

		public FileAssociation (string extension)
		{
			this.extension = (extension[0] == '.' ? extension : "." + extension);

			this.contentType = "text/plain";
			this.perceivedType = "text";
			this.properName = null;
			this.displayName = null;
			this.iconPath = null;
			this.iconIndex = 0;

			this.captions = new ArrayList();
			this.commands = new ArrayList();
		}


		//========================================================================================
		// Properties
		//========================================================================================

		#region Properties

		/// <summary>
		/// Gets or sets the content type of the file type, for example "text/plain".
		/// </summary>

		public string ContentType
		{
			get { return contentType; }
			set { contentType = value; }
		}


		/// <summary>
		/// Gets the extension of the file type for this instance.
		/// </summary>
		/// <remarks>
		/// If the extension doesn't start with a dot ("."), a dot is automatically added.
		/// </remarks>

		public string Extension
		{
			get { return extension; }
		}


		/// <summary>
		/// Gets or sets the full display name of the type.
		/// </summary>

		public string DisplayName
		{
			get { return displayName; }
			set { displayName = value; }
		}


		/// <summary>
		/// Gets or sets the index of the icon of the file type.
		/// </summary>

		public short IconIndex
		{
			get { return iconIndex; }
			set { iconIndex = value; }
		}


		/// <summary>
		/// Gets or sets the path of the resource that contains the icon for the file type.
		/// </summary>
		/// <remarks>This resource can be an executable or a DLL.</remarks>

		public string IconPath
		{
			get { return iconPath; }
			set { iconPath = value; }
		}


		/// <summary>
		/// Gets or sets the proper name of the file type.  This is the cross-reference
		/// root type.
		/// </summary>

		public string ProperName
		{
			get { return properName; }
			set { properName = value; }
		}


		/// <summary>
		/// Gets or sets the perceived name of the file type.
		/// </summary>

		public string PerceivedType
		{
			get { return perceivedType; }
			set { perceivedType = value; }
		}

		#endregion Properties


		//========================================================================================
		// AddCommand()
		//========================================================================================

		/// <summary>
		/// Adds a new command to the command list.
		/// </summary>
		/// <param name="caption">The short name of the command, such as "open" or "print".</param>
		/// <param name="command">The full command to execute.</param>

		public void AddCommand (string caption, string command)
		{
			captions.Add(caption);
			commands.Add(command);
		}


		//========================================================================================
		// Create()
		//========================================================================================

		/// <summary>
		/// Creates the file association.
		/// </summary>

		public void Create ()
		{
			// clean up any existing definitions
			Remove();

			RegistryKey key;

			// create extension key

			key = Registry.ClassesRoot.CreateSubKey(extension);
			key.SetValue(String.Empty, properName);

			if (contentType != null)
				key.SetValue("Content Type", contentType);

			key.Close();

			// create properName key

			key = Registry.ClassesRoot.CreateSubKey(properName);
			key.SetValue(String.Empty, displayName);
			key.Close();

			if (iconPath != null)
			{
				key = Registry.ClassesRoot.CreateSubKey(properName + "\\" + "DefaultIcon");
				key.SetValue(String.Empty, iconPath + "," + iconIndex.ToString());
				key.Close();
			}

			for (int i = 0; i < captions.Count; i++)
			{
				key = Registry.ClassesRoot.CreateSubKey(
					properName + "\\" + "Shell" + "\\" + (String)captions[i]);

				key = key.CreateSubKey("Command");
				key.SetValue(String.Empty, commands[i]);
				key.Close();
			}
		}


		//========================================================================================
		// Remove()
		//========================================================================================

		/// <summary>
		/// Removes the appropriate Registry entries defining this file type.  Both the
		/// extension key and properName key are removed.
		/// </summary>

		public void Remove ()
		{
			try
			{
				Registry.ClassesRoot.DeleteSubKeyTree(extension);
			}
			catch (SecurityException exc)
			{
				throw exc;
			}
			catch { }

			try
			{
				Registry.ClassesRoot.DeleteSubKeyTree(properName);
			}
			catch (SecurityException exc)
			{
				throw exc;
			}
			catch { }
		}
	}
}
