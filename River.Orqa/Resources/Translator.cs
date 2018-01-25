//************************************************************************************************
// Copyright © 2005 Steven M. Cohn. All Rights Reserved.
//
// Provides access to resources through a set of convenience methods. Optimizes
// ResourceManagers across all threads of the current AppDomain making this making this
// supportive of a multi-client threaded server environment such as Microsoft's
// HTTP.SYS and IIS.
//
//************************************************************************************************

namespace River.Orqa.Resources
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Resources;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Xml.XPath;


	//********************************************************************************************
	// class Translator
	//********************************************************************************************

	/// <summary>
	/// Provides access to resources through a set of convenience methods.
	/// Optimizes ResourceManagers across all threads of the current AppDomain
	/// making this supportive of a multi-client threaded server environment
	/// such as Microsoft's HTTP.SYS and IIS.
	/// </summary>

	internal class Translator
	{
		private class ResourceManagers : Dictionary<string, ResourceManager> { }

		private static ResourceManagers managers;	// App-wide cached resource managers
		private static string rootPath;				// default root executable path

		private string baseName;					// base name of active satellite assembly
		private string resourceNs;					// internal namespace within satellite assembly

		private ResourceManager rm;					// active resource manager based on culture


		//========================================================================================
		// Constructors
		//========================================================================================

		/// <summary>
		/// Static constructor.  For internal use only.
		/// </summary>

		static Translator ()
		{
			managers = new ResourceManagers();

			// This rootPath logic is required to satisfy both local installations
			// and Click-Once deployments

			string path = System.Windows.Forms.Application.ExecutablePath;
			string name = Path.GetFileName(path);
			string testName = name.ToLower();

			if (testName.IndexOf("nunit") >= 0)
			{
				// special case for NUnit execution!

				rootPath = Path.GetDirectoryName(
					new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			}
			else
			{
				rootPath = path.Replace(name, String.Empty);
			}
		}


		/// <summary>
		/// Initializes a new Translator, providing resources for the specified assembly.
		/// </summary>
		/// <param name="resourceNamespace">
		/// The namespace within the assembly from which resources are to be retrieved.
		/// </param>

		public Translator (string resourceNamespace)
		{
			try
			{
				this.baseName = Assembly.GetExecutingAssembly().GetName().Name;

				// use the ResourceNamespace property to do the common work
				if (!resourceNamespace.StartsWith("Resources"))
				{
					resourceNamespace = "Resources." + resourceNamespace;
				}

				ResourceNamespace = resourceNamespace;
			}
			catch
			{
				// VS likes to screw us!  So swallow missing file exception.
				rm = null;
			}
		}


		//========================================================================================
		// LoadResourceManager()
		//		Assumes that the resource manager for the specified baseName has not yet been
		//		loaded.  Loads the resource manager and seeds it with the resource set associated
		//		with the current thread culture.
		//========================================================================================

		private ResourceManager LoadResourceManager (string baseName, string resourceNs)
		{
			string resourceBaseName;
			if ((resourceNs == null) || (resourceNs.Length == 0))
				resourceBaseName = baseName;
			else
				resourceBaseName = baseName + "." + resourceNs;

			if (managers.ContainsKey(resourceBaseName))
			{
				return managers[resourceBaseName];
			}

			ResourceManager rm;
			string filnam = rootPath + "\\" + baseName + ".dll";
			if (!File.Exists(filnam))
			{
				filnam = rootPath + "\\" + baseName + ".exe";
			}

			try
			{
				Assembly assembly = Assembly.LoadFrom(filnam);
				rm = new ResourceManager(resourceBaseName, assembly);
				managers.Add(resourceBaseName, rm);
			}
			catch (Exception exc)
			{
				throw new Exception("Error loading resource assembly "
					+ filnam + ".  Exception: [" + exc.Message + "]");
			}

			return rm;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets or sets the namespace from which resources are to be retrieved.  This
		/// supercedes the namespace specified when the Translated was instantiated.
		/// </summary>
		/// <value>
		/// A string specifying the namespace within the current satellite
		/// assembly from which resources are to be retrieved.
		/// </value>

		public string ResourceNamespace
		{
			get
			{
				return resourceNs;
			}

			set
			{
				this.resourceNs = value;
				this.rm = LoadResourceManager(this.baseName, this.resourceNs);
			}
		}


		//========================================================================================
		// GetBitmap()
		//========================================================================================

		/// <summary>
		/// Retrieves the Bitmap referenced by the specified resource identifier.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing a Bitmap.
		/// </param>
		/// <returns>
		/// A Bitmap referenced by the resource value.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a Bitmap.
		/// </exception>

		#region GetBitmap

		public Bitmap GetBitmap (string resID)
		{
			return GetBitmap(resID, Thread.CurrentThread.CurrentCulture);
		}


		private Bitmap GetBitmap (string resID, CultureInfo culture)
		{
			if (resID.IndexOf(',') > 0)
			{
				string[] parts = resID.Split(',');
				string namePart = parts[0].Trim();
				string basePart = parts[1].Trim();
				string nsPart = (parts.Length > 2 ? parts[2].Trim() : null);

				return GetBitmap(namePart, basePart, nsPart, culture);
			}

			return GetBitmap(resID, this.baseName, this.resourceNs, culture);
		}


		/// <summary>
		/// Retrieves the Bitmap referenced by the specified resource identifier from a
		/// specific satellite assembly.  The default resource manager is not
		/// changed; a transient reference is made to the baseName resource manager and is
		/// immediately disposed when completed.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing a Bitmap.
		/// </param>
		/// <param name="baseName">
		/// The base name of the secondary satellite assembly.
		/// </param>
		/// <param name="resourceNs">
		/// The namespace within the satellite assembly from which resources are to be retrieved.
		/// </param>
		/// <returns>
		/// A Bitmap referenced by the resource identifier.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a Bitmap.
		/// </exception>

		public Bitmap GetBitmap (string resID, string baseName, string resourceNs)
		{
			return GetBitmap(resID, baseName, resourceNs, Thread.CurrentThread.CurrentCulture);
		}


		private Bitmap GetBitmap (string resID, string baseName, string resourceNs, CultureInfo culture)
		{
			if (rm == null)
				return null;

			ResourceManager tmpRm;

			if (baseName.Equals(this.baseName) && resourceNs.Equals(this.resourceNs))
				tmpRm = this.rm;
			else
				tmpRm = LoadResourceManager(baseName, resourceNs);

			Bitmap bitmap;
			try
			{
				bitmap = (Bitmap)tmpRm.GetObject(resID, culture);
			}
			catch (MissingManifestResourceException)
			{
				bitmap = null;
			}

			tmpRm = null;

			return bitmap;
		}

		#endregion // GetBitmap

		#region GetInvariantBitmap()

		/// <summary>
		/// Retrieves the Bitmap referenced by the specified invariant resource identifier.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing a Bitmap.
		/// </param>
		/// <returns>
		/// A Bitmap specifying the invariant resource value.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a Bitmap.
		/// </exception>

		public Bitmap GetInvariantBitmap (string resID)
		{
			return GetBitmap(resID, CultureInfo.InvariantCulture);
		}


		/// <summary>
		/// Retrieves the Bitmap reference by the specified resource identifier from a
		/// specific invariant satellite assembly.  The default resource manager is not
		/// changed; a transient reference is made to the baseName resource manager and is
		/// immediately disposed when completed.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing an invariant Bitmap.
		/// </param>
		/// <param name="baseName">
		/// The base name of the secondary invariant satellite assembly.
		/// </param>
		/// <param name="resourceNs">
		/// The namespace within the invariant satellite assembly from which resources
		/// are to be retrieved.
		/// </param>
		/// <returns>
		/// A Bitmap specifying the invariant resource value.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a Bitmap.
		/// </exception>

		public Bitmap GetInvariantBitmap (string resID, string baseName, string resourceNs)
		{
			return GetBitmap(resID, baseName, resourceNs, CultureInfo.InvariantCulture);
		}

		#endregion // GetInvariantBitmap()


		//========================================================================================
		// GetIcon()
		//========================================================================================

		/// <summary>
		/// Retrieves the Icon referenced by the specified resource identifier.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing an Icon.
		/// </param>
		/// <returns>
		/// An Icon referenced by the resource value.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not an Icon.
		/// </exception>

		#region GetIcon

		public Icon GetIcon (string resID)
		{
			return GetIcon(resID, Thread.CurrentThread.CurrentCulture);
		}


		private Icon GetIcon (string resID, CultureInfo culture)
		{
			if (resID.IndexOf(',') > 0)
			{
				string[] parts = resID.Split(',');
				string namePart = parts[0].Trim();
				string basePart = parts[1].Trim();
				string nsPart = (parts.Length > 2 ? parts[2].Trim() : null);

				return GetIcon(namePart, basePart, nsPart, culture);
			}

			return GetIcon(resID, this.baseName, this.resourceNs, culture);
		}


		/// <summary>
		/// Retrieves the Icon referenced by the specified resource identifier from a
		/// specific satellite assembly.  The default resource manager is not
		/// changed; a transient reference is made to the baseName resource manager and is
		/// immediately disposed when completed.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing an Icon.
		/// </param>
		/// <param name="baseName">
		/// The base name of the secondary satellite assembly.
		/// </param>
		/// <param name="resourceNs">
		/// The namespace within the satellite assembly from which resources are to be retrieved.
		/// </param>
		/// <returns>
		/// An Icon referenced by the resource identifier.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not an Icon.
		/// </exception>

		public Icon GetIcon (string resID, string baseName, string resourceNs)
		{
			return GetIcon(resID, baseName, resourceNs, Thread.CurrentThread.CurrentCulture);
		}


		private Icon GetIcon (string resID, string baseName, string resourceNs, CultureInfo culture)
		{
			if (rm == null)
				return null;

			ResourceManager tmpRm;

			if (baseName.Equals(this.baseName) && resourceNs.Equals(this.resourceNs))
				tmpRm = this.rm;
			else
				tmpRm = LoadResourceManager(baseName, resourceNs);

			Icon icon;
			try
			{
				icon = (Icon)tmpRm.GetObject(resID, culture);
			}
			catch (MissingManifestResourceException)
			{
				icon = null;
			}

			tmpRm = null;

			return icon;
		}

		#endregion // GetIcon

		#region GetInvariantIcon()

		/// <summary>
		/// Retrieves the Icon referenced by the specified invariant resource identifier.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing an Icon.
		/// </param>
		/// <returns>
		/// An Icon specifying the invariant resource value.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not an Icon.
		/// </exception>

		public Icon GetInvariantIcon (string resID)
		{
			return GetIcon(resID, CultureInfo.InvariantCulture);
		}


		/// <summary>
		/// Retrieves the Icon reference by the specified resource identifier from a
		/// specific invariant satellite assembly.  The default resource manager is not
		/// changed; a transient reference is made to the baseName resource manager and is
		/// immediately disposed when completed.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing an invariant Icon.
		/// </param>
		/// <param name="baseName">
		/// The base name of the secondary invariant satellite assembly.
		/// </param>
		/// <param name="resourceNs">
		/// The namespace within the invariant satellite assembly from which resources
		/// are to be retrieved.
		/// </param>
		/// <returns>
		/// An Icon specifying the invariant resource value.
		/// If resID is not found then <b>null</b> is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not an Icon.
		/// </exception>

		public Icon GetInvariantIcon (string resID, string baseName, string resourceNs)
		{
			return GetIcon(resID, baseName, resourceNs, CultureInfo.InvariantCulture);
		}

		#endregion // GetInvariantIcon()


		//========================================================================================
		// GetString()
		//========================================================================================

		/// <summary>
		/// Retrieves the string value of the specified resource identifier.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing a string value.
		/// </param>
		/// <returns>
		/// A string value specifying the resource value.
		/// If resID is not found then String.Empty is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a string.
		/// </exception>

		public string GetString (string resID)
		{
			return GetString(resID, Thread.CurrentThread.CurrentCulture);
		}


		private string GetString (string resID, CultureInfo culture)
		{
			if (resID.IndexOf(',') > 0)
			{
				string[] parts = resID.Split(',');
				string namePart = parts[0].Trim();
				string basePart = parts[1].Trim();
				string nsPart = (parts.Length > 2 ? parts[2].Trim() : null);

				return GetString(namePart, basePart, nsPart, culture);
			}

			return GetString(resID, this.baseName, this.resourceNs, culture);
		}


		/// <summary>
		/// Retrieves the string value of the specified resource identifier from a
		/// specific satellite assembly.  The default resource manager is not
		/// changed; a transient reference is made to the baseName resource manager and is
		/// immediately disposed when completed.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing a string value.
		/// </param>
		/// <param name="baseName">
		/// The base name of the secondary satellite assembly.
		/// </param>
		/// <param name="resourceNs">
		/// The namespace within the satellite assembly from which resources are to be retrieved.
		/// </param>
		/// <returns>
		/// A string specifying the resource value.
		/// If resID is not found then String.Empty is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a string.
		/// </exception>

		public string GetString (string resID, string baseName, string resourceNs)
		{
			return GetString(resID, baseName, resourceNs, Thread.CurrentThread.CurrentCulture);
		}


		private string GetString (string resID, string baseName, string resourceNs, CultureInfo culture)
		{
			if (rm == null)
				return "{MISSING}";

			ResourceManager tmpRm;

			if (baseName.Equals(this.baseName) && resourceNs.Equals(this.resourceNs))
				tmpRm = this.rm;
			else
				tmpRm = LoadResourceManager(baseName, resourceNs);

			string s;
			try
			{
				s = tmpRm.GetString(resID, culture);
			}
			catch (MissingManifestResourceException)
			{
				s = String.Empty;
			}

			tmpRm = null;

			return s;
		}

		#region GetInvariantString()

		/// <summary>
		/// Retrieves the string value of the specified invariant resource identifier.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing a string value.
		/// </param>
		/// <returns>
		/// A string value specifying the invariant resource value.
		/// If resID is not found then String.Empty is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a string.
		/// </exception>

		public string GetInvariantString (string resID)
		{
			return GetString(resID, CultureInfo.InvariantCulture);
		}


		/// <summary>
		/// Retrieves the string value of the specified resource identifier from a
		/// specific invariant satellite assembly.  The default resource manager is not
		/// changed; a transient reference is made to the baseName resource manager and is
		/// immediately disposed when completed.
		/// </summary>
		/// <param name="resID">
		/// A resource identifier referencing an invariant string value.
		/// </param>
		/// <param name="baseName">
		/// The base name of the secondary invariant satellite assembly.
		/// </param>
		/// <param name="resourceNs">
		/// The namespace within the invariant satellite assembly from which resources
		/// are to be retrieved.
		/// </param>
		/// <returns>
		/// A string specifying the invariant resource value.
		/// If resID is not found then String.Empty is returned.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// The value of the specified resource is not a string.
		/// </exception>

		public string GetInvariantString (string resID, string baseName, string resourceNs)
		{
			return GetString(resID, baseName, resourceNs, CultureInfo.InvariantCulture);
		}

		#endregion // GetInvariantString()


		//========================================================================================
		// SetCulture()
		//		Sets the running thread culture.  The difference between CurrentCulture and
		//		CurrentUICulture is that CurrentCulture sets the culture for formatting of dates
		//		and numbers, while CurrentUICulture sets the culture for loading resources. 
		//========================================================================================

		/// <summary>
		/// Sets the active culture for resources of the associated assembly.
		/// </summary>
		/// <param name="cultureName">The ISO code of the culture to use.</param>
		/// <exception cref="System.ArgumentException">
		/// <i>cultureName</i> is not a valid culture name. 
		/// </exception>

		public static void SetCulture (string cultureName)
		{
			CultureInfo info = CultureInfo.CreateSpecificCulture(cultureName);

			Thread.CurrentThread.CurrentCulture = info;
			Thread.CurrentThread.CurrentUICulture = info;
		}


		//========================================================================================
		// Translate(xml-display-string)
		//========================================================================================

		/// <summary>
		/// Replaces delimited substrings in the given format string with values
		/// specified by the given variables XML parameter wher ethe XML has been
		/// serialized as a string.
		/// </summary>
		/// <param name="format">
		/// A string containing bracket-delimeted element names corresponding to
		/// elements of the given variables XML.
		/// </param>
		/// <param name="variables">
		/// A string describing an XML structure with a root node and one or more
		/// child elements specifying values for input into the format string.
		/// </param>

		public string Translate (string format, string variables)
		{
			XPathDocument doc = new XPathDocument(new StringReader(variables));
			XPathNavigator navigator = doc.CreateNavigator();
			navigator.MoveToFirstChild();
			return Translate(format, navigator);
		}


		/// <summary>
		/// Replaces delimited substrings in the given format string with values
		/// specified by the given variables XML parameter.
		/// </summary>
		/// <param name="format">
		/// A string containing bracket-delimeted element names corresponding to
		/// elements of the given variables XML.
		/// </param>
		/// <param name="variables">
		/// An XPathNavigator pointing to a root node and one or more
		/// child elements specifying values for input into the format string.
		/// </param>

		public string Translate (string format, XPathNavigator variables)
		{
			Match match = Regex.Match(format, @"\{(.*?)\}");
			StringBuilder result = new StringBuilder(format);

			while (match.Success)
			{
				if (variables.MoveToChild(match.Groups[1].Value, String.Empty))
				{
					result.Replace(match.Value, variables.Value);
					variables.MoveToParent();
				}

				match = match.NextMatch();
			}

			return result.ToString();
		}
	}
}