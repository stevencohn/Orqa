//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// Defines helper classes used to maintain fonts, colors and other Option items.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 01-Jul-2005      New
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.Drawing;
	using River.Orqa.Editor;


	//********************************************************************************************
	// class DisplayFont
	//********************************************************************************************

	internal class DisplayFont
	{
		public FontFamily Family;
		public string DisplayName;
		public FontStyle Style;
		public bool IsMonospace;

		public DisplayFont (FontFamily family, FontStyle style)
		{
			this.Family = family;
			this.DisplayName = (style == FontStyle.Regular ? family.Name : family.Name + " (" + style.ToString() + ")");
			this.Style = style;
		}

		public override string ToString ()
		{
			return DisplayName;
		}
	}


	//********************************************************************************************
	// class DisplayColor
	//********************************************************************************************

	internal class DisplayColor
	{
		public string Name;
		public Color Color;

		public DisplayColor (string name, Color color)
		{
			this.Name = name;
			this.Color = color;
		}

		public override string ToString ()
		{
			return Name;
		}
	}


	//********************************************************************************************
	// class DisplayItem
	//********************************************************************************************

	internal class DisplayItem
	{
		public Color ForeColor;
		public Color BackColor;
		public FontStyle FontStyle;
		public int Size;
		public string FamilyName;
		public string Name;
		public string InternalName;

		public DisplayItem ()
		{
			this.ForeColor = SystemColors.WindowText;
			this.BackColor = SystemColors.Window;
			this.FontStyle = FontStyle.Regular;
			this.Size = 8;
			this.FamilyName = null;
			this.Name = null;
			this.InternalName = null;
		}

		public DisplayItem (Editor.Dialogs.LexStyleItem item)
		{
			this.ForeColor = item.ForeColor;
			this.BackColor = item.BackColor;
			this.FontStyle = item.FontStyle;
			this.Size = 8;
			this.FamilyName = item.Name;
			this.Name = item.Name;
			this.InternalName = item.InternalName;
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}
