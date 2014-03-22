// -----------------------------------------------------------------------------
//  <copyright file="Launcher.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using System.Diagnostics;
	using System.Text;
	using System.Windows;
	using System.Windows.Forms;
	using Views;
	using MessageBox = System.Windows.MessageBox;

	public class Launcher : IDisposable
	{
		private Configuration config;
		private readonly Server server;
		private Process process;

		public Launcher(Server server, IoCContainer iocc)
		{
			config = iocc.RetrieveContract<Configuration>();

			this.server = server;
		}

		private void BuildProcess()
		{
			StringBuilder builder = new StringBuilder(server.ServerAddress);
			builder.AppendFormat("?name={0}", null);

			if (server.Settings.IsPassworded)
			{
				using (PasswordEntry dialog = new PasswordEntry())
				{
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
		}

		public void Start()
		{
			BuildProcess();

			process.Start();
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
