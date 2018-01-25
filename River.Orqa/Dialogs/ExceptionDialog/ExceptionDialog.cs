namespace River.Orqa.Dialogs
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Reflection;
	using System.Windows.Forms;


	internal partial class ExceptionDialog : Form
	{
		private Exception root;						// the root exception!


		//========================================================================================
		// Constructors
		//========================================================================================

		public ExceptionDialog ()
		{
			// not used; required by VS.NET designer
			InitializeComponent();
		}


		public ExceptionDialog (Exception exc) : this()
		{
			root = exc;

			exceptionDetails.ExceptionInstance = exc;
			exceptionDetails.Abort += new EventHandler(Abort);
			exceptionDetails.Report += new EventHandler(Report);
		}


		//========================================================================================
		// ShowException()
		//========================================================================================

		public static void ShowException (Exception exc)
		{
			ExceptionDialog dialog = new ExceptionDialog(exc);
			dialog.ShowDialog();
		}


		//========================================================================================
		// ExceptionDetails handlers
		//========================================================================================

		private void Report (object sender, EventArgs e)
		{
			ExceptionReport exceptionReport = new ExceptionReport();
			exceptionReport.ExceptionInstance = root;
			exceptionReport.Dock = DockStyle.Fill;

			exceptionReport.Abort += new EventHandler(Abort);

			this.Controls.Remove(exceptionDetails);
			this.Controls.Add(exceptionReport);
		}


		private void Abort (object sender, EventArgs e)
		{
			this.Close();
		}
	}
}