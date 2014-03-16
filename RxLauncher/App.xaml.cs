// -----------------------------------------------------------------------------
//  <copyright file="App.xaml.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher
{
	using System.Threading.Tasks;
	using System.Windows;
	using ViewModels;
	using Views;

	public partial class App
	{
		protected MainViewModel viewModel;

		#region Overrides of Application

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
		protected override void OnStartup(StartupEventArgs e)
		{
			if (MainWindow == null)
			{
				MainWindow = new MainWindow();
			}

			viewModel = new MainViewModel();
			
			MainWindow.DataContext = viewModel;
			MainWindow.Show();

			Task.Factory.StartNew(() =>
			                      {
				                      Task.Delay(1000).Wait();

				                      viewModel.ServerList.UpdateServerList();
			                      });
		}

		#endregion
	}
}
