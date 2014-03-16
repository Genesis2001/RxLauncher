// -----------------------------------------------------------------------------
//  <copyright file="MainViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	public class MainViewModel : ViewModel
	{
		private string userName;

		public MainViewModel()
		{
			ServerList = new ServerListViewModel();
		}

		public ServerListViewModel ServerList { get; private set; }

		public string UserName
		{
			get { return userName; }
			set
			{
				NotifyPropertyChanging();
				userName = value;
				NotifyPropertyChanged();
			}
		}
	}
}
