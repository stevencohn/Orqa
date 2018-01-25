//************************************************************************************************
// Copyright © 2002-2006 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.Orqa.Query
{
	using System;
	using System.Collections;
	using System.Drawing;
	using System.Data;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using River.Orqa.Database;
	using River.Orqa.Options;
	using River.Orqa.Resources;


	internal partial class PlanView : UserControl
	{
		private static bool isInitialized = false;
		private static string cacheDir = null;
		private static Hashtable operations = null;

		private struct ID
		{
			#region IDs
			public static readonly int SQL = 0;
			public static readonly int AndEqual = 1;
			public static readonly int BitMap = 2;
			public static readonly int ConnectBy = 3;
			public static readonly int Concatenation = 4;
			public static readonly int Count = 5;
			public static readonly int DomainIndex = 6;
			public static readonly int Filter = 7;
			public static readonly int FirstRow = 8;
			public static readonly int ForUpdate = 9;
			public static readonly int HashJoin = 10;
			public static readonly int Index = 11;
			public static readonly int InlistIterator = 12;
			public static readonly int Intersection = 13;
			public static readonly int MergeJoin = 14;
			public static readonly int Minus = 15;
			public static readonly int NestLoops = 16;
			public static readonly int Partition = 17;
			public static readonly int Remote = 18;
			public static readonly int Sequence = 19;
			public static readonly int Sort = 20;
			public static readonly int TableAccess = 21;
			public static readonly int Union = 22;
			public static readonly int View = 23;
			public static readonly int Buffer = 24;
			public static readonly int ConnectByPump = 25;

			public static readonly int __Last = 25;  // must equal last value!
			#endregion IDs
		}


		public PlanView ()
		{
			InitializeComponent();
		}


		private void DoExport (object sender, EventArgs e)
		{
			var translator = new Translator("ExplainPlan");

			using (var dialog = new SaveFileDialog())
			{
				dialog.DefaultExt = "htm";
				dialog.Filter = translator.GetString("SavePlanDialogFilter");
				dialog.InitialDirectory = UserOptions.GetString("general/queryPath");
				dialog.Title = translator.GetString("SavePlanDialogTitle");

				translator = null;

				DialogResult result = dialog.ShowDialog(this);

				if (result == DialogResult.OK)
				{
					using (var file = new StreamWriter(dialog.FileName, false))
					{
						file.WriteLine(browser.DocumentText);
						file.Close();
					}
				}
			}
		}


		public void Clear ()
		{
			browser.Url = new Uri("about:blank");
		}


		public void ReportPlans (QueryCollection queries)
		{
			string CR = Environment.NewLine;

			int level;
			string operation;
			string options;
			string objectName;
			int cost;
			int rows;
			double kbytes;

			Database.Query query;
			DataRow row;

			if (!isInitialized)
				InitializeEnvironment();

			var htm = new StringBuilder();

			htm.Append("<html>");
			htm.Append("<head>");
			htm.Append("<style type=\"text/css\">" + CR);
			htm.Append(Properties.Resources.ExplainPlanStyles + CR);
			htm.Append("</style>" + CR);
			//htm.Append("<script language=\"javascript\">" + CR);
			//htm.Append(Resources.ResourceReader.LoadString("River.Orqa.Resources.ExplainPlan.js") + CR);
			//htm.Append("</script>" + CR);
			htm.Append("</head>" + CR);
			htm.Append("<body>" + CR);

			for (int q = 0; q < queries.Count; q++)
			{
				query = queries[q];

				htm.Append("<div class=\"query\">Query " + (q + 1) + "</div>" + CR);
				htm.Append("<blockquote class=\"sql\">" + query.SQL + "</blockquote>" + CR);
				htm.Append("<table class=\"plan\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + CR);
				htm.Append("<tr>" + CR);
				htm.Append("  <th>Operation</th>" + CR);
				htm.Append("  <th>Option</th>" + CR);
				htm.Append("  <th>Object</th>" + CR);
				htm.Append("  <th align=\"center\">Cost</th>" + CR);
				htm.Append("  <th align=\"center\">Rows</th>" + CR);
				htm.Append("  <th align=\"center\">KBytes</th>" + CR);
				htm.Append("</tr>" + CR);

				if (query.Plan != null)
				{
					for (int i = 0; i < query.Plan.Tables[0].Rows.Count; i++)
					{
						row = query.Plan.Tables[0].Rows[i];

						level = (int)(decimal)row["level"];
						operation = DecorateOperation((string)row["operation"]);
						options = (row["options"] == System.DBNull.Value ? "&nbsp;" : (string)row["options"]);
						objectName = (row["object_name"] == System.DBNull.Value ? "&nbsp;" : (string)row["object_name"]);
						cost = (row["cost"] == System.DBNull.Value ? 0 : (int)(decimal)row["cost"]);
						rows = (row["cardinality"] == System.DBNull.Value ? 0 : (int)(decimal)row["cardinality"]);
						kbytes = (row["bytes"] == System.DBNull.Value ? 0.0F : (double)((decimal)row["bytes"] / 1024));

						htm.Append("<tr class=\"" + (i % 2 == 0 ? "" : "alt") + "\">" + CR);
						htm.Append("  <td style=\"padding-left:" + ((level * 16) + 10) + ";\">" + operation + "</td>" + CR);
						htm.Append("  <td>" + options + "</td>" + CR);
						htm.Append("  <td>" + objectName + "</td>" + CR);
						htm.Append("  <td>" + cost + "</td>" + CR);
						htm.Append("  <td>" + rows + "</td>" + CR);
						htm.Append("  <td align=\"right\">" + kbytes.ToString("0.000") + "</td>" + CR);
						htm.Append("</tr>" + CR);
					}
				}

				htm.Append("</table>" + CR);
			}

			htm.Append("</body>");
			htm.Append("</html>");

			HtmlDocument doc = browser.Document.OpenNew(true);
			doc.Write(htm.ToString());
		}


		//========================================================================================
		// InitializeEnvironment()
		//========================================================================================

		private void InitializeEnvironment ()
		{
			cacheDir = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\";

			var translator = new Translator("ExplainPlan");

			for (int i = 0; i <= PlanView.ID.__Last; i++)
			{
				Bitmap image = (Bitmap)translator.GetBitmap("Orqa_XP" + i);
				image.Save(cacheDir + "Orqa_XP" + i + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
			}

			operations = new Hashtable();
			operations.Add("DELETE STATEMENT", 0);
			operations.Add("INSERT STATEMENT", 0);
			operations.Add("SELECT STATEMENT", 0);
			operations.Add("UPDATE STATEMENT", 0);
			operations.Add("AND-EQUAL", 1);
			operations.Add("BITMAP", 2);
			operations.Add("CONNECT BY", 3);
			operations.Add("CONCATENATION", 4);
			operations.Add("COUNT", 5);
			operations.Add("DOMAIN INDEX", 6);
			operations.Add("FILTER", 7);
			operations.Add("FIRST ROW", 8);
			operations.Add("FOR UPDATE", 9);
			operations.Add("HASH JOIN", 10);
			operations.Add("INDEX", 11);
			operations.Add("INLIST INTERATOR", 12);
			operations.Add("INTERSECTION", 13);
			operations.Add("MERGE JOIN", 14);
			operations.Add("MINUS", 15);
			operations.Add("NESTED LOOPS", 16);
			operations.Add("PARTITION", 17);
			operations.Add("REMOTE", 18);
			operations.Add("SEQUENCE", 19);
			operations.Add("SORT", 20);
			operations.Add("TABLE ACCESS", 21);
			operations.Add("UNION", 22);
			operations.Add("UNION-ALL", 22);
			operations.Add("VIEW", 23);
			operations.Add("BUFFER", 24);
			operations.Add("CONNECT BY PUMP", 25);

			isInitialized = true;
		}


		//========================================================================================
		// DecorateOperation()
		//========================================================================================

		private string DecorateOperation (string tag)
		{
			string decorated;

			if (operations.Contains(tag))
			{
				decorated = "<img src=\"" + cacheDir
					+ "Orqa_XP" + (int)operations[tag]
					+ ".gif\" border=\"0\"/><span class=\"operation\">"
					+ tag + "</span>";
			}
			else
			{
				decorated = tag;
			}

			return decorated;
		}
	}
}
