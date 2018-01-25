//************************************************************************************************
// Copyright © 2002-2009 Steven M. Cohn. All Rights Reserved.
//
// User preferences and program options.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
// 15-May-2009		Use XDocument instead of XmlDocument
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Xml;
	using System.Xml.Linq;
	using System.Xml.XPath;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class Options
	//********************************************************************************************

	internal static class UserOptions
	{
		public static readonly string FileName = "Options.xml";
		public static readonly string Root = "OrqaOptions";

		private static readonly string path;			// user specific storage path

		private static XDocument options;				// custom user options
		private static XDocument defaults;				// system default options

		private static bool runExplainPlan;				// execute with explain plan
		private static bool runStatistics;				// execute with statistics


		//========================================================================================
		// Constructor
		//========================================================================================

		static UserOptions ()
		{
			// build path to options directory and ensure its existence

			path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				+ "\\" + System.Windows.Forms.Application.CompanyName
				+ "\\" + System.Windows.Forms.Application.ProductName;

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			// load the default options instance

			Translator translator = new Translator("Options");
			string defstr = translator.GetInvariantString("DefaultOptions");
			defaults = XDocument.Parse(defstr);

			// personalize the default options with user's private directories

			XElement element;

			element = defaults.Element(Root).Element("general").Element("queryPath");
			element.SetValue(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

			element = defaults.Element(Root).Element("general").Element("resultPath");
			element.SetValue(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

			element = defaults.Element(Root).Element("general").Element("templatePath");
			element.SetValue(Path.Combine(
				Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath),
				"Snipets"));

			// load the personalized options, if not found then use defaults

			try
			{
				options = XDocument.Load(path + "\\" + FileName);
			}
			catch (Exception)
			{
				Reset();
			}

			runExplainPlan = false;
			runStatistics = false;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets the system defined default option values as a single XML document.
		/// </summary>

		public static XDocument Defaults
		{
			get { return defaults; }
		}


		/// <summary>
		/// Gets or sets the system defined default option values as a single XML document.
		/// Used internally to temporarily preserve user options during an export.
		/// </summary>

		public static XDocument OptionsDoc
		{
			get { return options; }
			set { options = value; }
		}


		/// <summary>
		/// Gets or sets a setting indicating if an explain plan should be generated
		/// when a query is executed.
		/// </summary>

		public static bool RunExplainPlan
		{
			get { return runExplainPlan; }
			set { runExplainPlan = value; }
		}


		/// <summary>
		/// Gets or sets a setting indicating if statistics should be compiled
		/// when a query is executed.
		/// </summary>

		public static bool RunStatistics
		{
			get { return runStatistics; }
			set { runStatistics = value; }
		}


		//========================================================================================
		// Gets
		//========================================================================================

		/// <summary>
		/// Gets the specified option as a boolean value
		/// </summary>
		/// <param name="xpath">
		/// The relative path of the option specified using the form "section/field".
		/// </param>

		public static bool GetBoolean (string xpath)
		{
			bool value = false;
			string strval = GetString(xpath);
			if (!String.IsNullOrEmpty(strval))
			{
				Boolean.TryParse(strval, out value);
			}

			return value;
		}


		public static object GetEnumeration (string xpath, Type type)
		{
			string strval = GetString(xpath);
			if (!String.IsNullOrEmpty(strval))
			{
				try
				{
					return Enum.Parse(type, strval);
				}
				catch
				{
					return null;
				}
			}

			return null;
		}


		/// <summary>
		/// Gets the specified option as an integer value
		/// </summary>
		/// <param name="xpath">
		/// The relative path of the option specified using the form "section/field".
		/// </param>

		public static int GetInt (string xpath)
		{
			int value = Int32.MinValue;
			string strval = GetString(xpath);
			if (!String.IsNullOrEmpty(strval))
			{
				Int32.TryParse(strval, out value);
			}

			return value;
		}


		/// <summary>
		/// Gets the specified option as a string value
		/// </summary>
		/// <param name="xpath">
		/// The relative path of the option specified using the form "section/field".
		/// </param>

		public static string GetString (string xpath)
		{
			xpath = "/" + Root + "/" + xpath;

			XElement element = options.XPathSelectElement(xpath);
			if (element == null)
			{
				element = defaults.XPathSelectElement(xpath);
			}

			return (element == null ? string.Empty : element.Value);
		}


		//========================================================================================
		// Load(path)
		//========================================================================================

		/// <summary>
		/// Loads the custom options file saved to a user-specified location.  This is
		/// typically used to restore or import options that were previously exported.
		/// </summary>
		/// <param name="path">The full path to the user-specified import file.</param>

		public static XPathNavigator Load (string path)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			//Validate(doc.DocumentElement);

			return doc.CreateNavigator();
		}


		//========================================================================================
		// LoadScreenPosition()
		//========================================================================================

		public static Rectangle LoadScreenPosition ()
		{
			XElement element = options.XPathSelectElement(
				String.Format("/{0}/{1}", Root, "general/position"));

			if (element == null)
			{
				return Rectangle.Empty;
			}

			if (element.Element("left") == null)
			{
				return Rectangle.Empty;
			}

			int left = Int32.Parse(element.Element("left").Value);
			int top = Int32.Parse(element.Element("top").Value);
			int width = Int32.Parse(element.Element("width").Value);
			int height = Int32.Parse(element.Element("height").Value);

			Rectangle position = new Rectangle(left, top, width, height);
			return position;
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		/// <summary>
		/// Reset all user options to the system default values.
		/// </summary>

		public static void Reset ()
		{
			options = XDocument.Parse(defaults.ToString(SaveOptions.DisableFormatting));
		}


		//========================================================================================
		// Save()
		//========================================================================================

		/// <summary>
		/// Saves the current options to the private user folder.
		/// </summary>

		public static void Save ()
		{
			Save(path + "\\" + FileName);
		}


		/// <summary>
		/// Save the current options to the specified export file.
		/// </summary>
		/// <param name="filename">The name of the export file.</param>

		public static void Save (string filename)
		{
			options.Save(filename, SaveOptions.None);
		}


		//========================================================================================
		// SaveScreenPosition()
		//========================================================================================

		public static void SaveScreenPosition (Rectangle position)
		{
			XElement element = options.Element(Root).Element("general");

			if (element.Element("position") == null)
			{
				element.Add(
					new XElement("position"),
					new XElement("left", position.X.ToString()),
					new XElement("top", position.Y.ToString()),
					new XElement("width", position.Width.ToString()),
					new XElement("height", position.Height.ToString())
					);
			}
			else if (!element.Element("position").HasElements)
			{
				element = element.Element("position");
				element.Add(new XElement("left", position.X.ToString()));
				element.Add(new XElement("top", position.Top.ToString()));
				element.Add(new XElement("width", position.Width.ToString()));
				element.Add(new XElement("height", position.Height.ToString()));
			}
			else
			{
				element = element.Element("position");

				element.Element("left").SetValue(position.X.ToString());
				element.Element("top").SetValue(position.Y.ToString());
				element.Element("width").SetValue(position.Width.ToString());
				element.Element("height").SetValue(position.Height.ToString());
			}
		}


		//========================================================================================
		// Sets
		//========================================================================================

		public static void SetValue (string xpath, bool val)
		{
			SetValue(xpath, (val ? "true" : "false"));
		}


		public static void SetValue (string xpath, int val)
		{
			SetValue(xpath, val.ToString());
		}


		public static void SetValue (string xpath, string val)
		{
			string path = String.Format("/{0}/{1}", Root, xpath);
			XElement element = options.XPathSelectElement(path);

			if (element == null)
			{
				string[] parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				string name = parts[parts.Length - 1];

				path = String.Join("/", parts, 0, parts.Length - 1);
				element = options.XPathSelectElement(path);

				if (element != null)
				{
					element.Add(new XElement(parts[parts.Length - 1], val));
				}
			}
			else
			{
				element.SetValue(val);
			}
		}
	}
}
