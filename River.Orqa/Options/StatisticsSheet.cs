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
	using System.ComponentModel;
	using System.Drawing;
	using System.Reflection;
	using System.Windows.Forms;
	using River.Orqa.Database;
	using River.Orqa.Resources;


	//********************************************************************************************
	// class StatisticsSheet
	//********************************************************************************************

	internal partial class StatisticsSheet : SheetBase
	{
		private Translator translator;


		//========================================================================================
		// Constructor
		//========================================================================================

		public StatisticsSheet ()
			: base()
		{
			InitializeComponent();

			translator = new Translator("Options");

			BindLabelToCheckBox(userLabel, userCheck);
			BindLabelToCheckBox(redoLabel, redoCheck);
			BindLabelToCheckBox(enqueueLabel, enqueueCheck);
			BindLabelToCheckBox(cacheLabel, cacheCheck);
			BindLabelToCheckBox(osLabel, osCheck);
			BindLabelToCheckBox(parallelLabel, parallelCheck);
			BindLabelToCheckBox(sqlLabel, sqlCheck);
			BindLabelToCheckBox(debugLabel, debugCheck);

			Reset();
		}


		//========================================================================================
		// BindLabelToCheckBox()
		//========================================================================================

		private void BindLabelToCheckBox (Label label, CheckBox box)
		{
			label.Tag = box;
			label.MouseEnter += new EventHandler(DoLabelMouseEnter);
			label.MouseLeave += new EventHandler(DoLabelMouseLeave);
			label.Click += new EventHandler(DoLabelClick);
		}

		void DoLabelMouseEnter (object sender, EventArgs e)
		{
			typeof(CheckBox).InvokeMember("OnMouseEnter",
				BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance,
				null,
				(CheckBox)((Label)sender).Tag,
				new object[] { e } );
		}

		void DoLabelMouseLeave (object sender, EventArgs e)
		{
			typeof(CheckBox).InvokeMember("OnMouseLeave",
				BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance,
				null,
				(CheckBox)((Label)sender).Tag,
				new object[] { e });
		}

		void DoLabelClick (object sender, EventArgs e)
		{
			typeof(CheckBox).InvokeMember("OnClick",
				BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance,
				null,
				(CheckBox)((Label)sender).Tag,
				new object[] { e });
		}


		//========================================================================================
		// Reset()
		//========================================================================================

		public override void Reset ()
		{
			StatisticClass classes
				= (StatisticClass)UserOptions.GetInt("statistics/classifications");

			userCheck.Checked = ((classes & StatisticClass.User) > 0);
			redoCheck.Checked = ((classes & StatisticClass.Redo) > 0);
			enqueueCheck.Checked = ((classes & StatisticClass.Enqueue) > 0);
			cacheCheck.Checked = ((classes & StatisticClass.Cache) > 0);
			osCheck.Checked = ((classes & StatisticClass.OS) > 0);
			parallelCheck.Checked = ((classes & StatisticClass.Parallel) > 0);
			sqlCheck.Checked = ((classes & StatisticClass.SQL) > 0);
			debugCheck.Checked = ((classes & StatisticClass.Debug) > 0);
		}


		//========================================================================================
		// SaveOptions()
		//========================================================================================

		public override void SaveOptions ()
		{
			StatisticClass classes = 0;

			if (userCheck.Checked) classes |= StatisticClass.User;
			if (redoCheck.Checked) classes |= StatisticClass.Redo;
			if (enqueueCheck.Checked) classes |= StatisticClass.Enqueue;
			if (cacheCheck.Checked) classes |= StatisticClass.Cache;
			if (osCheck.Checked) classes |= StatisticClass.OS;
			if (parallelCheck.Checked) classes |= StatisticClass.Parallel;
			if (sqlCheck.Checked) classes |= StatisticClass.SQL;
			if (debugCheck.Checked) classes |= StatisticClass.Debug;

			UserOptions.SetValue("statistics/classifications", (int)classes);
		}
	}
}
