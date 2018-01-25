
namespace River.Orqa.Dialogs
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Reflection;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;


	/// <summary>
	/// 
	/// </summary>

	public class OrqaException : Exception, IXmlSerializable
	{

		//========================================================================================
		// Constructor
		//========================================================================================

		/// <summary>
		/// 
		/// </summary>

		public OrqaException ()
		{
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="exc"></param>
		
		public OrqaException (Exception exc)
			: base("Orqa Exception", exc)
		{
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
			WriteException(writer, this.InnerException);
		}


		private void WriteException (XmlWriter writer, Exception exc)
		{
			writer.WriteAttributeString("etype", exc.GetType().FullName);

			writer.WriteElementString("message", exc.Message);
			writer.WriteElementString("source", exc.Source);

			if (exc.TargetSite != null)
			{
				writer.WriteStartElement("targetSite");
				writer.WriteElementString("assembly", exc.TargetSite.DeclaringType.Assembly.GetName().Name);
				writer.WriteElementString("declaringType", exc.TargetSite.DeclaringType.ToString());
				writer.WriteElementString("name", exc.TargetSite.Name);
				writer.WriteEndElement();
			}

			writer.WriteElementString("stackTrace", exc.StackTrace);
			writer.WriteElementString("helpLink", exc.HelpLink);

			if ((exc.Data != null) && (exc.Data.Count > 0))
			{
				writer.WriteStartElement("data");

				IDictionaryEnumerator e = exc.Data.GetEnumerator();
				while (e.MoveNext())
				{
					writer.WriteElementString(e.Key.ToString(), e.Current.ToString());
				}

				writer.WriteEndElement();
			}

			if (exc.InnerException != null)
			{
				writer.WriteStartElement("innerException");
				WriteException(writer, exc.InnerException);
				writer.WriteEndElement();
			}
		}

		#endregion IXmlSerializable Implementation
	}
}
