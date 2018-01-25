namespace River.Orqa.Dialogs
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Net.Sockets;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using System.Xml.Serialization;
	using River.Orqa.Resources;

	
	internal partial class ExceptionReport : UserControl
	{
		private OrqaException root;					// the root exception!


		//========================================================================================
		// Constructor
		//========================================================================================

		public ExceptionReport ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// ExceptionInstance
		//========================================================================================

		public Exception ExceptionInstance
		{
			set
			{
				root = new OrqaException(value);

				XmlSerializer serializer = new XmlSerializer(root.GetType());
				StringBuilder buffer = new StringBuilder();
				serializer.Serialize(new StringWriter(buffer), root);

				msgBox.Text = buffer.ToString();
			}
		}


		//========================================================================================
		// Events and handlers
		//========================================================================================

		/// <summary>
		/// </summary>

		public event EventHandler Abort;

		private void SendAbort (object sender, EventArgs e)
		{
			if (Abort != null)
				Abort(sender, e);
		}


		private void SendReport (object sender, EventArgs e)
		{
			//NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
			//if (interfaces.Length > 0)
			//{
			//    IPInterfaceProperties props = interfaces[0].GetIPProperties();
			//    if (props.DnsAddresses.Count > 0)
			//    {
			//    }
			//}

			System.Diagnostics.Process.Start(
				String.Format("mailto:steven@cohn.net?subject={0}&body={1}",
				System.Web.HttpUtility.UrlEncode(subjectBox.Text),
				System.Web.HttpUtility.UrlEncode(msgBox.Text)
				));

			SendAbort(sender, e);
		}
	}
}
