//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//
// User preferences and program options.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 27-May-2002		New
// 01-Jul-2005      .NET 2.0, extensibility added via SheetBase and XML attributes
//************************************************************************************************

namespace River.Orqa.Options
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Windows.Forms;
	using System.Xml;
	using System.Xml.Linq;
	using System.Xml.XPath;
	using River.Orqa.Editor;
	using River.Orqa.Editor.Dialogs;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class EditorFontSheet
	//********************************************************************************************

	internal partial class EditorFontSheet : SheetBase
	{
		private Translator translator;
		private bool isInternal = false;
		//private Thread worker;


		//========================================================================================
		// Constructor
		//========================================================================================

		public EditorFontSheet ()
			: base()
		{
			InitializeComponent();

			translator = new Translator("Options");

			PopulateFontNames();
			PopulateColorNames(foreColorBox);
			PopulateColorNames(backColorBox);
			PopulateDisplayItems();

			EventHandler handler = new EventHandler(DoChangeProperties);
			sizeBox.ValueChanged += handler;
			foreColorBox.SelectedIndexChanged += handler;
			backColorBox.SelectedIndexChanged += handler;
			boldCheck.CheckedChanged += handler;
			italicCheck.CheckedChanged += handler;

			PopulateFontNamesCompleted();

			Reset();
		}


		private void PopulateFontNames ()
		{
			OptionsDialog.BuildFontList(this.CreateGraphics());

			//if (OptionsDialog.FontList == null)
			//{
			//    worker = new Thread(new ThreadStart(DiscoverFontNames));
			//    worker.IsBackground = true;
			//    worker.Start();
			//}
			//else
			//{
			//    PopulateFontNamesCompleted();
			//}
		}


		private void DiscoverFontNames ()
		{
			OptionsDialog.BuildFontList(this.CreateGraphics());
			this.Invoke(new MethodInvoker(PopulateFontNamesCompleted));
		}


		private void PopulateFontNamesCompleted ()
		{
			if (OptionsDialog.FontList == null)
				throw new Exception("EditorFontSheet.PopulateFontNames Null");

			familyBox.Enabled = true;
			familyBox.Items.AddRange(OptionsDialog.FontList.ToArray());
			familyBox.SelectedIndex = 0;
			familyBox.SelectedIndexChanged += new EventHandler(DoChangeFamily);

			ResetFamily();
		}


		private void PopulateColorNames (ComboBox box)
		{
			//box.Items.Add(new DisplayColor(translator.GetString("DefaultColor"), Color.Empty));

			Type ctype = typeof(Color);
			PropertyInfo[] infos = ctype.GetProperties();
			foreach (PropertyInfo info in infos)
			{
				if (info.PropertyType == typeof(Color))
				{
					box.Items.Add(new DisplayColor(info.Name, Color.FromName(info.Name)));
				}
			}

			ctype = typeof(SystemColors);
			infos = ctype.GetProperties();
			foreach (PropertyInfo info in infos)
			{
				if (info.PropertyType == typeof(Color))
				{
					box.Items.Add(new DisplayColor(info.Name, Color.FromName(info.Name)));
				}
			}

			box.SelectedIndex = 0;
		}


		public void PopulateDisplayItems ()
		{
			XPathNavigator root = UserOptions.OptionsDoc.CreateNavigator();
			root.MoveToFirstChild();
			root = root.SelectSingleNode("editor/editorFonts/lexStyles");
			if (root != null)
			{
				XPathNavigator nav = root.CreateNavigator();
				DisplayItem item;
				string colorName;

				foreach (LexStyleItem lexItem in LexStyleItems.Items)
				{
					if ((nav = nav.SelectSingleNode(lexItem.InternalName)) != null)
					{
						item = new DisplayItem(lexItem);

						colorName = nav.GetAttribute("foreColor", nav.NamespaceURI);
						item.ForeColor = Color.FromName(colorName);

						colorName = nav.GetAttribute("backColor", nav.NamespaceURI);
						item.BackColor = Color.FromName(colorName);

						item.FontStyle
							= (FontStyle)Enum.Parse(
							typeof(FontStyle), nav.GetAttribute("style", nav.NamespaceURI));

						itemsBox.Items.Add(item);
						Logger.WriteLine("Populate (" + item.Name + ", " + item.ForeColor + ", " + item.BackColor + ")");
					}

					nav = root;
				}
				while (nav.MoveToNext()) ;
			}

			itemsBox.SelectedIndex = 0;
		}


		//========================================================================================
		// DrawFontFamilyComboItem()
		//========================================================================================

		private void DrawFontFamilyComboItem (object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
				return;

			if (familyBox.Items.Count == 0)
				return;

			DisplayFont displayFont = (DisplayFont)familyBox.Items[e.Index];
			FontStyle style = (displayFont.IsMonospace ? FontStyle.Bold : FontStyle.Regular);
			System.Drawing.Font font = new System.Drawing.Font(familyBox.Font, style);

			e.DrawBackground();

			e.Graphics.DrawString(
				displayFont.DisplayName,
				font,
				new SolidBrush(e.ForeColor),
				e.Bounds.X,
				e.Bounds.Top);

			e.DrawFocusRectangle();
		}


		//========================================================================================
		// DrawColorComboItem()
		//========================================================================================

		private void DrawColorComboItem (object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
				return;

			ComboBox box = (ComboBox)sender;
			DisplayColor displayColor = (DisplayColor)box.Items[e.Index];

			e.DrawBackground();

			bool flag1 = (e.State & DrawItemState.Disabled) != DrawItemState.None;
			bool flag2 = ((e.State & DrawItemState.Focus) != DrawItemState.None) && ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None);
			bool flag3 = (e.State & DrawItemState.Selected) != DrawItemState.None;
			if (flag2)
			{
				e.DrawFocusRectangle();
			}

			Brush brush1 = new SolidBrush(displayColor.Color);

			try
			{
				Rectangle rectangle1;
				Rectangle rectangle2;
				rectangle1 = new Rectangle(e.Bounds.Left, e.Bounds.Top, Math.Min(e.Bounds.Width, 0x1c), e.Bounds.Height);
				rectangle1.Inflate(-2, -2);
				rectangle2 = new Rectangle(rectangle1.Right + 1, e.Bounds.Top, (e.Bounds.Width - rectangle1.Right) - 1, e.Bounds.Height);
				if (e.State == DrawItemState.HotLight)
				{
					e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
				}
				e.Graphics.FillRectangle(brush1, rectangle1);
				e.Graphics.DrawRectangle(Pens.Black, rectangle1);
				Brush brush2 = new SolidBrush(e.ForeColor);
				if (flag1)
				{
					brush2 = Brushes.Gray;
				}
				else if (flag3)
				{
					brush2 = SystemBrushes.HighlightText;
				}
				e.Graphics.DrawString(displayColor.Name, this.Font, brush2, (RectangleF)rectangle2);
			}
			finally
			{
				brush1.Dispose();
			}
		}


		//========================================================================================
		// DoChangeFamily()
		//========================================================================================

		private void DoChangeFamily (object sender, EventArgs e)
		{
			if (familyBox.Items.Count == 0)
				return;

			DisplayFont displayFont = (DisplayFont)familyBox.SelectedItem;

			if (displayFont.Family.IsStyleAvailable(FontStyle.Bold))
			{
				boldCheck.Enabled = true;
			}
			else
			{
				boldCheck.Checked = false;
				boldCheck.Enabled = false;
			}

			if (displayFont.Family.IsStyleAvailable(FontStyle.Italic))
			{
				italicCheck.Enabled = true;
			}
			else
			{
				italicCheck.Checked = false;
				italicCheck.Enabled = false;
			}

			DoChangeProperties(sender, e);
		}


		//========================================================================================
		// DoChangeProperties()
		//========================================================================================

		private void DoChangeProperties (object sender, EventArgs e)
		{
			if (isInternal)
				return;

			FontStyle style = UpdateSample();

			DisplayItem displayItem = (DisplayItem)itemsBox.SelectedItem;
			displayItem.FontStyle = style;
			displayItem.ForeColor = sample.ForeColor;
			displayItem.BackColor = sample.BackColor;
		}


		//========================================================================================
		// DoCustomColor()
		//========================================================================================

		private void DoCustomColor (object sender, EventArgs e)
		{
			ComboBox box = (sender == customForeButton ? foreColorBox : backColorBox);

			ColorDialog dialog = new ColorDialog();
			dialog.FullOpen = true;
			dialog.Color = ((DisplayColor)box.SelectedItem).Color;

			DialogResult result = dialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				AddCustomColor(box, dialog.Color);
			}
		}


		private void AddCustomColor (ComboBox box, Color color)
		{
			int argb = color.ToArgb();

			int i = 0;
			bool found = false;
			DisplayColor displayColor;

			while ((i < box.Items.Count) && !found)
			{
				displayColor = (DisplayColor)box.Items[i];
				if (!(found = (displayColor.Color.ToArgb() == argb)))
					i++;
			}

			if (!found)
			{
				string customName = translator.GetString("CustomColorName");
				if (box.Items[box.Items.Count - 1].ToString().Equals(customName))
				{
					((DisplayColor)box.Items[box.Items.Count - 1]).Color = color;
				}
				else
				{
					displayColor = new DisplayColor(customName, color);
					box.Items.Add(displayColor);
				}
				i = box.Items.Count - 1;
			}

			box.SelectedIndex = i;
		}


		//========================================================================================
		// DoChangeDisplayItem()
		//========================================================================================

		private void DoChangeDisplayItem (object sender, EventArgs e)
		{
			DisplayItem item = (DisplayItem)itemsBox.SelectedItem;

			Logger.WriteLine("Change (" + item.Name + ", " + item.ForeColor + ", " + item.BackColor + ")");

			isInternal = true;

			bool isHypertext = item.InternalName.Equals("hypertext");
			bool isSpelling = item.InternalName.Equals("spelling");
			boldCheck.Enabled = !isSpelling;
			italicCheck.Enabled = !isSpelling;
			backColorBox.Enabled = !isHypertext && !isSpelling;

			SetSelectedColor(foreColorBox, item.ForeColor);
			SetSelectedColor(backColorBox, item.BackColor);

			boldCheck.Checked = ((item.FontStyle & FontStyle.Bold) == FontStyle.Bold);
			italicCheck.Checked = ((item.FontStyle & FontStyle.Italic) == FontStyle.Italic);

			UpdateSample();

			isInternal = false;
		}


		private void SetSelectedColor (ComboBox box, Color color)
		{
			int i = 0;
			bool found = false;
			DisplayColor itemColor;

			while ((i < box.Items.Count) && !found)
			{
				itemColor = (DisplayColor)box.Items[i];

				if (!(found = itemColor.Color.Equals(color)))
					i++;
			}

			if (found)
				box.SelectedIndex = i;
			else
				AddCustomColor(box, color);
		}


		private FontStyle UpdateSample ()
		{
			if (familyBox.Items.Count == 0)
				return FontStyle.Regular;

			DisplayFont displayFont = (DisplayFont)familyBox.SelectedItem;

			FontStyle style = displayFont.Style;

			if (boldCheck.Enabled
				&& boldCheck.Checked
				&& displayFont.Family.IsStyleAvailable(FontStyle.Bold))
				style |= FontStyle.Bold;

			if (italicCheck.Enabled
				&& italicCheck.Checked
				&& displayFont.Family.IsStyleAvailable(FontStyle.Italic))
				style |= FontStyle.Italic;

			sample.Font = new System.Drawing.Font(
				displayFont.DisplayName,
				(float)sizeBox.Value,
				style
				);

			sample.ForeColor = ((DisplayColor)foreColorBox.SelectedItem).Color;
			sample.BackColor = ((DisplayColor)backColorBox.SelectedItem).Color;

			return style;
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		public override void Reset ()
		{
			XPathNavigator nav = UserOptions.OptionsDoc.CreateNavigator();
			nav.MoveToFirstChild();
			nav = nav.SelectSingleNode("editor/editorFonts/font");
			if (nav != null)
			{
				if (nav.MoveToChild("family", nav.NamespaceURI))
				{
					SetSelectedFamily(nav.Value);
					nav.MoveToParent();
				}

				if (nav.MoveToChild("size", nav.NamespaceURI))
				{
					sizeBox.Value = nav.ValueAsInt;
					nav.MoveToParent();
				}
			}

			nav.MoveToRoot();
			nav.MoveToFirstChild();
			nav = nav.SelectSingleNode("editor/editorFonts/lexStyles");
			if (nav != null)
			{
				if (nav.MoveToFirstChild())
				{
					DisplayItem item;
					string colorName;

					do
					{
						if ((item = FindDisplayItem(nav.Name)) != null)
						{
							colorName = nav.GetAttribute("foreColor", nav.NamespaceURI);
							item.ForeColor = Color.FromName(colorName);
							if (!item.ForeColor.IsKnownColor)
							{
								item.ForeColor = Color.FromArgb(int.Parse(
									colorName, System.Globalization.NumberStyles.HexNumber));
							}

							colorName = nav.GetAttribute("backColor", nav.NamespaceURI);
							item.BackColor = Color.FromName(colorName);
							if (!item.BackColor.IsKnownColor)
							{
								item.BackColor = Color.FromArgb(int.Parse(
									colorName, System.Globalization.NumberStyles.HexNumber));
							}

							item.FontStyle
								= (FontStyle)Enum.Parse(
								typeof(FontStyle), nav.GetAttribute("style", nav.NamespaceURI));
						}
					}
					while (nav.MoveToNext());
				}
			}

			itemsBox.SelectedIndex = 0;

			DisplayItem disItem = (DisplayItem)itemsBox.SelectedItem;

			SetSelectedColor(foreColorBox, disItem.ForeColor);
			SetSelectedColor(backColorBox, disItem.BackColor);

			boldCheck.Checked = ((disItem.FontStyle & FontStyle.Bold) == FontStyle.Bold);
			italicCheck.Checked = ((disItem.FontStyle & FontStyle.Italic) == FontStyle.Italic);
		}


		private void ResetFamily ()
		{
			XPathNavigator nav = UserOptions.OptionsDoc.CreateNavigator();
			nav.MoveToFirstChild();
			nav = nav.SelectSingleNode("editor/editorFonts/font");
			if (nav != null)
			{
				if (nav.MoveToChild("family", nav.NamespaceURI))
				{
					SetSelectedFamily(nav.Value);
					nav.MoveToParent();
				}
			}
		}


		private void SetSelectedFamily (string familyName)
		{
			int i = 0;
			bool found = false;
			DisplayFont font;

			while ((i < familyBox.Items.Count) && !found)
			{
				font = (DisplayFont)familyBox.Items[i];

				if (!(found = font.Family.Name.Equals(familyName)))
					i++;
			}

			if (found)
				familyBox.SelectedIndex = i;
		}


		private DisplayItem FindDisplayItem (string key)
		{
			int i = 0;
			bool found = false;
			DisplayItem item = null;

			while ((i < itemsBox.Items.Count) && !found)
			{
				item = (DisplayItem)itemsBox.Items[i];
				if (!(found = item.InternalName.Equals(key)))
					i++;
			}

			return (found ? item : null);
		}


		//========================================================================================
		// SaveOptions()
		//========================================================================================

		public override void SaveOptions ()
		{
			UserOptions.SetValue("editor/editorFonts/font/family", ((DisplayFont)familyBox.SelectedItem).Family.Name);
			UserOptions.SetValue("editor/editorFonts/font/size", (int)sizeBox.Value);

			var ns = UserOptions.OptionsDoc.Elements().First().GetDefaultNamespace();

			var styles = UserOptions.OptionsDoc
				.Element(ns + "OrqaOptions")
				.Element(ns + "editor")
				.Element(ns + "editorFonts")
				.Element(ns + "lexStyles");

			DisplayItem item;
			for (int i = 0; i < itemsBox.Items.Count; i++)
			{
				item = itemsBox.Items[i] as DisplayItem;
				if (item != null)
				{
					var node = styles.Element(item.InternalName);
					if (node != null)
					{
						var attribute = node.Attribute("foreColor");
						if (attribute != null)
						{
							attribute.SetValue(item.ForeColor.IsKnownColor
								? item.ForeColor.Name
								: item.ForeColor.ToArgb().ToString("x8"));
						}

						attribute = node.Attribute("backColor");
						if (attribute != null)
						{
							attribute.SetValue(item.BackColor.IsKnownColor
								? item.BackColor.Name
								: item.BackColor.ToArgb().ToString("x8"));
						}

						attribute = node.Attribute("style");
						if (attribute != null)
						{
							attribute.SetValue(((int)item.FontStyle).ToString());
						}
					}
				}
			}
		}
	}
}
