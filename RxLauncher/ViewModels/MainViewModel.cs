// -----------------------------------------------------------------------------
//  <copyright file="MainViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System;
	using Models;

	public class MainViewModel : ViewModel
	{
		private String userName;
		private IoCContainer iocc;

		public MainViewModel()
		{
			iocc = new IoCContainer();

			iocc.RegisterContract(this);
			iocc.RegisterContract(ConfigurationManager.Instance);

			ServerList = iocc.RegisterContract(new ServerListViewModel(iocc));
		}

		~MainViewModel()
		{
			ConfigurationManager.Save(ConfigurationManager.Instance, "Config.xml");
		}

		public ServerListViewModel ServerList { get; private set; }

		public String UserName
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
