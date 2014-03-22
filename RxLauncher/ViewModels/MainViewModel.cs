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

			Configuration config = Configuration.Load("Config.xml");
			iocc.RegisterContract(config);

			ServerList = iocc.RegisterContract(new ServerListViewModel(iocc));
		}

		~MainViewModel()
		{
			ServerList.Save();
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
