// -----------------------------------------------------------------------------
//  <copyright file="Launcher.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Forms;
	using System.Windows.Interop;
	using Views;
	using MessageBox = System.Windows.MessageBox;
	using Application = System.Windows.Application;

	public class Launcher : IDisposable
	{
		[DllImport("user32.dll")]
		private static extern int SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

		private static readonly string DefaultInstallationDirectory =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86), "Renegade X");

		private readonly Configuration config;
		private readonly Server server;
		private Process process;

		public Launcher(Server server, IoCContainer iocc)
		{
			config = iocc.RetrieveContract<Configuration>();

			this.server = server;
		}

		private String BuildInstallationPath()
		{
			if (String.IsNullOrEmpty(config.InstallationPath))
			{
				// 
			}

			return config.InstallationPath;
		}

		private void BuildProcess()
		{
			if (process == null)
			{
				process = new Process();
			}

			StringBuilder builder = new StringBuilder(server.ServerAddress);
			builder.AppendFormat("?name={0}", null);

			if (server.Settings.IsPassworded)
			{
				WindowInteropHelper helper = new WindowInteropHelper(Application.Current.MainWindow);
				using (PasswordEntry dialog = new PasswordEntry())
				{
					SetWindowLong(new HandleRef(dialog, dialog.Handle), -8, helper.Handle.ToInt32());

					var result = dialog.ShowDialog();

					if (result == DialogResult.OK)
					{
						builder.AppendFormat("?Password={0}", dialog.Password.Text);
					}
					else
					{
						// TODO: error message... tired.
						MessageBox.Show("Warning! A passworded server will require a password to enter.",
							"No password specified",
							MessageBoxButton.OK,
							MessageBoxImage.Exclamation);
					}
				}
			}

			process.StartInfo.Arguments = builder.ToString();
			process.StartInfo.FileName = BuildInstallationPath();
		}

		public void Start()
		{
			BuildProcess();

			Task.Factory.StartNew(() => process.Start());
		}

		public void Stop()
		{
			if (process != null)
			{
				process.Kill();
			}
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;


		}

		#endregion
	}
}
