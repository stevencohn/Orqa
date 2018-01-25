namespace River.Orqa.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Windows.Forms;


	public class RiverListView : System.Windows.Forms.ListView
	{
		public RiverListView ()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}
	}
}
