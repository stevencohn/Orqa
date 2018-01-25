
namespace River.Orqa
{
	using System;
	using System.ComponentModel;
	using System.Data;
	using System.Diagnostics;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using System.Xml;


	internal partial class Logger : UserControl
	{
		private static readonly string CR = System.Environment.NewLine;

		private static Logger logger = null;

		private static bool isEnabled = false;		// true if output is enabled
		private static int sectionRows = 0;			// maximize rows per section at 100


		#region ExceptionEncoder

		private class ExceptionEncoder
		{
			/// <summary>
			/// Encodes the given Exception as a string suitable for writing to a diagnostic log.
			/// </summary>
			/// <param name="exception">
			/// An Exception or ApplicationException instance.
			/// </param>
			/// <returns>
			/// A string specifying an XML representation of the Exception including AppDomain
			/// and Thread information, exception type, stack trace, data items and inner
			/// exceptions.
			/// </returns>

			public static string Encode (Exception exception)
			{
				var builder = new StringBuilder();
				var writer = new StringWriter(builder);
				var xml = new XmlTextWriter(writer);
				xml.Formatting = Formatting.Indented;

				xml.WriteStartElement("Exception");

				var domain = AppDomain.CurrentDomain;
				xml.WriteStartElement("AppDomain");
				xml.WriteAttributeString("id", domain.Id.ToString());
				xml.WriteString(domain.FriendlyName);
				xml.WriteEndElement();

				var thread = Thread.CurrentThread;
				xml.WriteStartElement("Thread");
				xml.WriteAttributeString("id", thread.ManagedThreadId.ToString());
				xml.WriteString(thread.Name);
				xml.WriteEndElement();

				xml.WriteStartElement("Exception");

				Encode(xml, exception);

				xml.WriteEndElement();
				xml.Close();
				xml = null;
				writer.Close();
				writer = null;

				return builder.ToString();
			}


			private static void Encode (XmlWriter xml, Exception exception)
			{
				Type type = exception.GetType();
				xml.WriteElementString("ExceptionType", Encode(type.AssemblyQualifiedName));
				xml.WriteElementString("Message", Encode(exception.Message));
				xml.WriteElementString("StackTrace", Encode(EncodeStackTrace(exception)));
				xml.WriteElementString("ExceptionString", Encode(exception.ToString()));

				var wex = exception as Win32Exception;
				if (wex != null)
				{
					xml.WriteElementString("NativeErrorCode",
						wex.NativeErrorCode.ToString("X", CultureInfo.InvariantCulture));
				}
				if ((exception.Data != null) && (exception.Data.Count > 0))
				{
					xml.WriteStartElement("DataItems");
					foreach (object key in exception.Data.Keys)
					{
						xml.WriteStartElement("Data");
						xml.WriteElementString("Key", Encode(key.ToString()));

						if (exception.Data[key] == null)
							xml.WriteElementString("Value", "null");
						else
						{
							if (exception.Data.Contains(key))
							{
								xml.WriteElementString("Value", Encode(exception.Data[key].ToString()));
							}
						}

						xml.WriteEndElement();
					}
					xml.WriteEndElement();
				}
				if (exception.InnerException != null)
				{
					xml.WriteStartElement("InnerException");
					Encode(xml, exception.InnerException);
					xml.WriteEndElement();
				}
			}


			private static string EncodeStackTrace (Exception exception)
			{
				string trace = exception.StackTrace;
				if (!string.IsNullOrEmpty(trace))
				{
					return trace;
				}
				int count = 0;

				StackFrame[] frames = new StackTrace(false).GetFrames();
				if (frames != null) // check required by Coverity
				{
					foreach (StackFrame frame in frames)
					{
						string name = frame.GetMethod().Name;
						if ((name != null) && (((name == "StackTraceString") ||
							(name == "AddExceptionToTraceString")) || (((name == "BuildTrace") ||
							(name == "TraceEvent")) || (name == "TraceException"))))
						{
							count++;
						}
						else if ((name != null) &&
							name.StartsWith("ThrowHelper", StringComparison.Ordinal))
						{
							count++;
						}
						else
						{
							break;
						}
					}
				}

				var stackTrace = new StackTrace(count, false);
				return stackTrace.ToString();
			}



			/// <summary>
			/// Transforms any XML specific characters within the given string to their
			/// encoded equivalents, allowing the string to be included as content of an
			/// XML element rather than an element itself or a misaligned token that 
			/// inadvertently corrupts the stream of XML.
			/// </summary>
			/// <param name="text">A string containing XML tokens to encode</param>
			/// <returns>A string with encoded tokens.</returns>

			public static string Encode (string text)
			{
				if (string.IsNullOrEmpty(text))
				{
					return text;
				}

				int len = text.Length;
				var builder = new StringBuilder(len + 8);
				for (int i = 0; i < len; i++)
				{
					char ch = text[i];
					switch (ch)
					{
						case '<':
							builder.Append("&lt;");
							break;

						case '>':
							builder.Append("&gt;");
							break;

						case '&':
							builder.Append("&amp;");
							break;

						default:
							builder.Append(ch);
							break;
					}
				}

				return builder.ToString();
			}
		}

		#endregion ExceptionEncoder


		//========================================================================================
		// Constructor
		//========================================================================================

		static Logger ()
		{
			logger = new Logger();
		}

		public Logger ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// Properties
		//========================================================================================

		internal static Logger Instance
		{
			get { return logger; }
		}


		public static bool IsEnabled
		{
			get { return isEnabled; }
			set { isEnabled = value; }
		}


		//========================================================================================
		// Menu Actions
		//========================================================================================

		#region Menu Actions

		private void DoCut (object sender, EventArgs e)
		{
			if (notepad.SelectionLength > 0)
			{
				notepad.ReadOnly = false;
				notepad.Cut();
				notepad.ReadOnly = true;
			}
		}


		private void DoCopy (object sender, EventArgs e)
		{
			if (notepad.SelectionLength > 0)
				notepad.Copy();
		}


		private void DoSelectAll (object sender, EventArgs e)
		{
			notepad.ReadOnly = false;
			notepad.SelectAll();
			notepad.ReadOnly = true;
		}


		private void DoClear (object sender, EventArgs e)
		{
			notepad.ReadOnly = false;
			notepad.Clear();
			notepad.ReadOnly = true;
		}

		#endregion MenuActions


		//========================================================================================
		// Event Handlers
		//========================================================================================

		#region Event Handlers

		protected override void OnKeyDown (KeyEventArgs e)
		{
			if (e.Modifiers == Keys.Control)
			{
				if ((e.KeyCode == Keys.X) || (e.KeyCode == Keys.Delete))
				{
					// must disable readonly for sequences that change content
					notepad.ReadOnly = false;
				}
			}
		}


		protected override void OnKeyUp (KeyEventArgs e)
		{
			// revert action of OnKeyDown
			notepad.ReadOnly = true;
		}


		protected override void OnMouseUp (MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				bool hasSelection = (notepad.SelectionLength > 0);
				bool hasContent = (notepad.Text.Length > 0);

				cutToolStripMenuItem.Enabled = hasSelection;
				copyToolStripMenuItem.Enabled = hasSelection;
				selectallToolStripMenuItem.Enabled = hasContent;
				clearallToolStripMenuItem.Enabled = hasContent;
			}
		}

		#endregion Event Handlers

	
		//========================================================================================
		// Writers
		//========================================================================================

		public static void Write (Exception exc)
		{
			if (isEnabled)
			{
				WriteSection("EXCEPTION");
				Write(ExceptionEncoder.Encode(exc));
			}
		}

		public static void Encode (Exception exc)
		{
		}


		public static void Write (string text)
		{
			if (isEnabled)
			{
				logger.notepad.AppendText(text);
				logger.notepad.ScrollToCaret();
			}
		}


		public static void WriteLine ()
		{
			if (isEnabled)
			{
				logger.notepad.AppendText(CR);
				logger.notepad.ScrollToCaret();
			}
		}


		public static void WriteLine (string text)
		{
			if (isEnabled)
			{
				logger.notepad.AppendText(text + CR);
				logger.notepad.ScrollToCaret();
			}
		}


		public static void WriteLine (string text, Color color)
		{
			if (isEnabled)
			{
				logger.notepad.SelectionColor = color;
				logger.notepad.AppendText(text + CR);
				logger.notepad.SelectionColor = Color.Black;
				logger.notepad.ScrollToCaret();
			}
		}

		
		public static void WriteSection (string title)
		{
			if (isEnabled)
			{
				logger.notepad.AppendText(String.Empty.PadRight(80, '=') + CR);
				logger.notepad.AppendText(title + CR);
				logger.notepad.AppendText(String.Empty.PadRight(80, '-') + CR);
				logger.notepad.ScrollToCaret();

				sectionRows = 0;
			}
		}


		public static void WriteRowData (DataRow row)
		{
			if (isEnabled)
			{
				if (sectionRows < 100)
				{
					logger.notepad.AppendText("Row: " + (++sectionRows) + CR);

					DataTable table = row.Table;
					DataColumnCollection cols = table.Columns;
					DataColumn col;
					for (int i=0; i < cols.Count; i++)
					{
						col = cols[i];

						logger.notepad.AppendText(
							"  Col: " + col.ColumnName
							+ " = [" + row[col.ColumnName].ToString()
							+ "]" + CR
							);

						logger.notepad.ScrollToCaret();
					}
				}
			}
		}
	}
}
