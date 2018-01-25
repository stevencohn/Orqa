
namespace River.Orqa.Options
{
	using System;
	using System.Drawing;


	internal static class FontsAndColors
	{
		public static Color PlainTextForeground;
		public static Color PlainTextBackground;
		public static Color IdentifierForeground;
		public static Color MarginDivider;
		public static Color MessageErrorForeground;
		public static Color MessageInfoForeground;
		public static Color MessageStatusForeground;
		public static Color MessageUserForeground;
		public static Color NoteForeground;
		public static Color SelectedTextForeground;
		public static Color SelectedTextBackground;


		static FontsAndColors ()
		{
			PlainTextForeground = Color.FromArgb(unchecked((int)0xFFDEDEBC)); // light beige
			PlainTextBackground = Color.FromArgb(unchecked((int)0xFF292929)); // light black
			IdentifierForeground = Color.FromArgb(unchecked((int)0xFFEFC986)); // gold
			MarginDivider = Color.FromArgb(unchecked((int)0xFF606060)); // dark gray
			MessageErrorForeground = Color.Red;
			MessageInfoForeground = Color.Gray;
			MessageStatusForeground = Color.Green;
			MessageUserForeground = Color.Maroon;
			NoteForeground = Color.DarkGray;
			SelectedTextForeground = Color.FromArgb(unchecked((int)0xFF000000));
			SelectedTextBackground = Color.FromArgb(unchecked((int)0xFF4D7AAC)); // grayish blue
		}
	}
}
