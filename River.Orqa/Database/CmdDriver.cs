//************************************************************************************************
// Copyright © 2002-2013 Steven M. Cohn. All Rights Reserved.
//
// Parameter information.
//
// Revision History:
// -When---------- -What-------------------------------------------------------------------------
// 25-Aug-2013      New
//************************************************************************************************

namespace River.Orqa.Database
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Text;


	//********************************************************************************************
	// class CmdDriver
	//********************************************************************************************

	internal class CmdDriver
	{
		public const int InvalidSubprocess = int.MaxValue;
		public const int CompleteWithNoWait = int.MaxValue - 1;


		public enum Wait
		{
			None,		// return immediate without waiting for completion
			Infinite,	// wait for completion
			Timed		// wait for completion or until timeout
		}


		protected Dictionary<string, string> environment;
		protected DataReceivedEventHandler handler;

		private StringBuilder output;
		private string outputFilename;
		private StreamWriter writer;
		private Wait wait;
		private int timeout;


		//========================================================================================
		// Lifecycle
		//========================================================================================

		public CmdDriver ()
		{
			environment = new Dictionary<string, string>();
			output = new StringBuilder();
			outputFilename = null;
			writer = null;
			handler = null;
			wait = Wait.Infinite;
			timeout = 0;
		}


		public CmdDriver (DataReceivedEventHandler handler, int timeout = 0)
			: this()
		{
			this.handler = handler;
			this.timeout = timeout;
			this.wait = (timeout == 0 ? Wait.Infinite : Wait.Timed);
		}


		public CmdDriver (string outputFilename)
			: this()
		{
			this.outputFilename = outputFilename;
			this.wait = Wait.Infinite;
		}


		//========================================================================================
		// Properties
		//========================================================================================

		public Dictionary<string, string> Environment
		{
			set { environment = value; }
		}


		public string Output
		{
			get { return output.ToString(); }
		}


		//========================================================================================
		// Methods
		//========================================================================================

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dbase"></param>
		/// <param name="text"></param>
		/// <returns></returns>

		public virtual int Run (string command, params string[] arguments)
		{
			if (outputFilename != null)
			{
				writer = new StreamWriter(outputFilename, false);
			}

			string args = null;
			if ((arguments != null) && (arguments.Length > 0))
			{
				args = String.Join(" ", arguments);
			}

			int exitcode = LaunchProcess(command, args);

			if (writer != null)
			{
				writer.Close();
				writer.Dispose();
				writer = null;
			}

			return exitcode;
		}


		private int LaunchProcess (string command, string arguments)
		{
			int exitCode = InvalidSubprocess;

			using (var p = new Process())
			{
				p.StartInfo.Arguments = arguments;
				p.StartInfo.FileName = command;
				p.StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
				p.StartInfo.CreateNoWindow = true;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.RedirectStandardError = true;
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.ErrorDialog = false;

				if (handler == null)
				{
					p.OutputDataReceived += OutputDataReceived;
					p.ErrorDataReceived += OutputDataReceived;
				}
				else
				{
					p.OutputDataReceived += handler;
					p.ErrorDataReceived += handler;
				}

				if ((environment != null) && (environment.Count > 0))
				{
					foreach (string key in environment.Keys)
					{
						p.StartInfo.EnvironmentVariables[key] = environment[key];
					}
				}

				p.Start();

				if (wait == Wait.None)
				{
					return CompleteWithNoWait;
				}

				p.BeginErrorReadLine();
				p.BeginOutputReadLine();

				if (wait == Wait.Infinite)
				{
					p.WaitForExit();
					exitCode = p.ExitCode;
				}
				else
				{
					// wait a specific time to finish and then kill
					if (p.WaitForExit(timeout))
					{
						exitCode = p.ExitCode;
					}
					else
					{
						p.Kill();
					}
				}

				p.Close();
			}

			return exitCode;
		}


		private void OutputDataReceived (object sender, DataReceivedEventArgs e)
		{
			if (writer == null)
			{
				output.Append(e.Data + System.Environment.NewLine);
			}
			else
			{
				writer.WriteLine(e.Data);
			}
		}
	}
}