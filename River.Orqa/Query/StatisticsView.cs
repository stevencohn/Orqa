
namespace River.Orqa.Query
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using River.Orqa.Database;


	internal partial class StatisticsView : UserControl
	{
		private struct Icons
		{
			internal static readonly int User = 0;
			internal static readonly int Redo = 1;
			internal static readonly int Enqueue = 2;
			internal static readonly int Cache = 3;
			internal static readonly int OS = 4;
			internal static readonly int Parallel = 5;
			internal static readonly int SQL = 6;
			internal static readonly int Debug = 7;
		}
		

		public StatisticsView ()
		{
			InitializeComponent();
		}


		//========================================================================================
		// Populate()
		//========================================================================================

		public void Populate (Statistics stats)
		{
			ListViewItem item;
			ListViewItem.ListViewSubItem subitem;
			string delta;

			statsView.Items.Clear();

			foreach (Statistic stat in stats.Values)
			{
				delta = String.Format("{0}", (stat.Value - stat.Initial));

				item = new ListViewItem(stat.Name);
				item.UseItemStyleForSubItems = false;
				item.SubItems.Add(delta);
				item.SubItems.Add(stat.Value.ToString());

				subitem = new ListViewItem.ListViewSubItem(item, stat.ClassName);
				subitem.ForeColor = Color.Gray;
				item.SubItems.Add(subitem);

				switch (stat.ClassID)
				{
					case (int)StatisticClass.User:
						item.ImageIndex = Icons.User;
						break;

					case (int)StatisticClass.Redo:
						item.ImageIndex = Icons.Redo;
						break;

					case (int)StatisticClass.Enqueue:
						item.ImageIndex = Icons.Enqueue;
						break;

					case (int)StatisticClass.Cache:
						item.ImageIndex = Icons.Cache;
						break;

					case (int)StatisticClass.OS:
						item.ImageIndex = Icons.OS;
						break;

					case (int)StatisticClass.Parallel:
						item.ImageIndex = Icons.Parallel;
						break;

					case (int)StatisticClass.SQL:
						item.ImageIndex = Icons.SQL;
						break;

					case (int)StatisticClass.Debug:
						item.ImageIndex = Icons.Debug;
						break;
				}

				statsView.Items.Add(item);
			}
		}
	}
}
