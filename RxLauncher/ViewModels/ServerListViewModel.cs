// -----------------------------------------------------------------------------
//  <copyright file="ServerListViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Net;
	using System.Threading;
	using System.Windows.Input;
	using System.Windows.Threading;
	using Commands;
	using Models;
	using Newtonsoft.Json;

	public class ServerListViewModel : ViewModel
	{
		public const string RxServerList = "http://renegadexgs.appspot.com/servers.jsp";

		private CancellationToken token;
		private readonly CancellationTokenSource tokenSource;
		private ServerViewModel selected;

		public ServerListViewModel()
		{
			tokenSource = new CancellationTokenSource();
			token       = tokenSource.Token;

			RefreshCommand = new ActionCommand(x => Dispatcher.CurrentDispatcher.InvokeAsync(UpdateServerList), x => true);
		}

		public ServerListViewModel(IEnumerable<Server> servers) : this()
		{
			this.Servers = new ObservableCollection<ServerViewModel>(servers.Cast<ServerViewModel>());
		}

		~ServerListViewModel()
		{
			tokenSource.Cancel();
		}

		public ICommand RefreshCommand { get; private set; }

		public ObservableCollection<ServerViewModel> Servers { get; private set; }

		public ServerViewModel Selected
		{
			get { return Servers.SingleOrDefault(x => x.IsSelected); }
		}

		public async void UpdateServerList()
		{
			string rxList;
			using (WebClient client = new WebClient())
			{
				rxList = await client.DownloadStringTaskAsync(RxServerList);
			}

			IList<Server> list = JsonConvert.DeserializeObject<IList<Server>>(rxList);
			if (list != null)
			{
				foreach (Server item in list)
				{
					ServerViewModel s = Servers.SingleOrDefault(x => x.Address == item.Address && x.Port == item.Port);
					if (s != null)
					{
						int index = Servers.IndexOf(s);

						Servers.RemoveAt(index);
						Servers.Insert(index, (ServerViewModel)item);
					}
					else
					{
						Servers.Add((ServerViewModel)item);
					}
				}
			}
		}
	}
}
