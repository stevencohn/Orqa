using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace River.Orqa.Browser
{
	internal partial class SchemaProperties : UserControl
	{
		public SchemaProperties ()
		{
			InitializeComponent();
		}


		public object SelectedObject
		{
			set { propertyGrid.SelectedObject = value; }
		}
	}
}
