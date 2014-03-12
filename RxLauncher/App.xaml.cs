// -----------------------------------------------------------------------------
//  <copyright file="App.xaml.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher
{
	using System.Windows;
	using ViewModels;

	public partial class App
	{
		protected ViewModel viewModel;

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
		}

		#endregion
	}
}
