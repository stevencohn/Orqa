//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Paints a label control fading from left to right.  This is customized
// for the Options dialog.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Jul-2005      New
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Text;
	using System.Windows.Forms;


	//********************************************************************************************
	// class FadingLabel
	//********************************************************************************************

	internal class FadingLabel : System.Windows.Forms.Label
	{
		private Color startColor;
		private Color endColor;


		//========================================================================================
		// Constructor
		//========================================================================================

		/// <exclude />

		public FadingLabel ()
			: base()
		{
			this.AutoSize = false;
			this.TextAlign = ContentAlignment.MiddleLeft;

			this.startColor = SystemColors.GradientActiveCaption;
			this.endColor = SystemColors.ControlLightLight;
			this.ForeColor = SystemColors.ActiveCaptionText;

			this.Font = new System.Drawing.Font("Tahoma", 9F, FontStyle.Bold);
		}


		//========================================================================================
		// OnPaint()
		//========================================================================================

		protected override void OnPaint (System.Windows.Forms.PaintEventArgs e)
		{
			// declare linear gradient brush for fill background of label
			LinearGradientBrush GBrush = new LinearGradientBrush(
				new Point(0, 0),
				new Point(this.Width, 0),
				startColor,
				endColor);

			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

			// Fill with gradient 
			e.Graphics.FillRectangle(GBrush, rect);

			// draw text on label
			SolidBrush drawBrush = new SolidBrush(this.ForeColor);

			StringFormat sf = new StringFormat();

			// align with center
			sf.Alignment = StringAlignment.Near;

			// set rectangle bound text
			RectangleF recf = new RectangleF(
				0, this.Height / 2 - Font.Height / 2, this.Width, this.Height);

			// output string
			e.Graphics.DrawString(this.Text, this.Font, drawBrush, recf, sf);
		}
	}
}
