//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 31-Aug-2013      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;


	//********************************************************************************************
	// class OraData
	//********************************************************************************************

	internal class OraData : List<OraTable>
	{

		public XElement GetXml ()
		{
			var data = new XElement("Data");
			foreach (var table in this)
			{
				var result = new XElement("Result");
				foreach (var row in table)
				{
					var rec = new XElement("Row");
					for (int c = 0; c < table.FieldCount; c++)
					{
						string name = table.Schema[c].ColumnName.Replace(" ", String.Empty);
						if (!Char.IsLetter(name[0]))
						{
							name = "COL" + c.ToString() + "_" + name;
						}

						// XML element names cannot have dollar signs
						name = name.Replace("$", string.Empty);

						var datum = new XElement(
							name,
							row[c] == null ? "..." : row[c].ToString());

						rec.Add(datum);
					}

					result.Add(rec);
				}

				data.Add(result);
			}

			return data;
		}
	}
}
