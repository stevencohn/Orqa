//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Represents an optimized Font for serialization.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.Drawing;
	using System.Xml;
	using System.Xml.Serialization;
	using System.Xml.XPath;


	//********************************************************************************************
	// class Font
	//********************************************************************************************

	/// <summary>
	/// Represents an optimized Font for serialization.
	/// </summary>

	[XmlRoot("font", Namespace="urn:River.Orqa")]
	public class OptionFont : IXmlSerializable
	{
		private string familyName;
		private GraphicsUnit unit;
		private float size;
		private FontStyle style;
		//private Color foreColor;
		//private Color backColor;


		//========================================================================================
		// Constructors
		//========================================================================================

		/// <summary>
		/// Dummy constructor required for serialization.
		/// </summary>

		public OptionFont ()
		{
		}


		/// <summary>
		/// Initializes a new Font from the given System Font.
		/// </summary>
		/// <param name="font">
		/// A System.Drawing.Font from which attributes are copied to be serialized.
		/// </param>

		public OptionFont (System.Drawing.Font font)
		{
			familyName = font.FontFamily.Name;
			size = font.Size;
			style = font.Style;
			unit = font.Unit;
		}


		/// <summary>
		/// Restores a Font instance from the given navigator; this is used
		/// to deserialized a Font from a stored user preferences file.
		/// </summary>
		/// <param name="navigator">
		/// A reference to the Font declaration node.
		/// </param>

		public OptionFont (XPathNavigator navigator)
		{
			if (navigator.NodeType == XPathNodeType.Root)
			{
				navigator.MoveToFirstChild();
			}

			if (navigator.MoveToChild("family", navigator.NamespaceURI))
			{
				familyName = navigator.Value;
				navigator.MoveToParent();
			}

			if (navigator.MoveToChild("size", navigator.NamespaceURI))
			{
				size = float.Parse(navigator.Value);
				navigator.MoveToParent();
			}

			if (navigator.MoveToChild("style", navigator.NamespaceURI))
			{
				style = (FontStyle)Enum.Parse(typeof(FontStyle), navigator.Value, true);
				navigator.MoveToParent();
			}

			if (navigator.MoveToChild("unit", navigator.NamespaceURI))
			{
				unit = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), navigator.Value, true);
				navigator.MoveToParent();
			}
		}


		//========================================================================================
		// Properties
		//========================================================================================

		/// <summary>
		/// Gets a new System.Drawing.Font represented by this Font.
		/// </summary>

		public System.Drawing.Font SystemFont
		{
			get
			{
				return new System.Drawing.Font(familyName, size, style, unit);
			}
		}


		//========================================================================================
		// IXmlSerializable Implementation
		//========================================================================================

		#region IXmlSerializable Implementation

		/// <summary>
		/// Required implementation of IXmlSerlializable.GetSchema.  This method
		/// returns <b>null</b>.
		/// </summary>

		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}


		/// <summary>
		/// Required implementation of IXmlSerlializable.ReadXml.
		/// </summary>
		/// <param name="reader">
		/// The XmlReader stream from which the object is deserialized.
		/// </param>

		public void ReadXml (XmlReader reader)
		{
			reader.ReadToDescendant("family");

			familyName = reader.ReadElementContentAsString();
			size = float.Parse(reader.ReadElementContentAsString());
			style = (FontStyle)Enum.Parse(typeof(FontStyle), reader.ReadElementContentAsString(), true);
			unit = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), reader.ReadElementContentAsString(), true);
		}


		/// <summary>
		/// Required implementation of IXmlSerlializable.ReadXml.
		/// <para>
		/// Serializes the ticket and encrypts values if this is an internal
		/// Everest authentication request; internal requests use the isStrong
		/// variable whereas SDK clients do not use this mechanism.
		/// </para>
		/// </summary>
		/// <param name="writer">The XmlWriter stream to which the object is serialized.</param>

		public void WriteXml (XmlWriter writer)
		{
			writer.WriteElementString("family", familyName);
			writer.WriteElementString("size", size.ToString());
			writer.WriteElementString("style", style.ToString());
			writer.WriteElementString("unit", unit.ToString());
		}

		#endregion IXmlSerializable Implementation
	}
}
