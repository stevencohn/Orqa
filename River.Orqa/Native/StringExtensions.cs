//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 25-Aug-2013      New
//************************************************************************************************

namespace River.Orqa
{
	using System;
	using System.Text;


	internal static class StringExtensions
	{

		/// <summary>
		/// Replace tabs with spaces, preserving column alignment based on given tab size.
		/// </summary>
		/// <param name="tabbed">The current string.</param>
		/// <param name="tabsize">The tab size, e.g. 4 or 8.</param>
		/// <returns></returns>

		public static string Untabify (this string tabbed, int tabsize)
		{
			var builder = new StringBuilder(tabbed.Length);
			for (int i = 0; i < tabbed.Length; i++)
			{
				char c = tabbed[i];
				if (c == '\t')
				{
					do { builder.Append(' '); } while (builder.Length % tabsize > 0);
				}
				else
				{
					builder.Append(c);
				}
			}

			return builder.ToString();
		}
	}
}
