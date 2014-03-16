// -----------------------------------------------------------------------------
//  <copyright file="ServerListViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Linq;
	using System.Net;
	using System.Runtime.Serialization.Formatters;
	using System.Threading;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Threading;
	using Commands;
	using Models;
	using Newtonsoft.Json;

	[Flags]
	public enum ServerFilter
	{
		All        = 0,
		Empty      = 1,
		Full       = 2,
		Bots       = 4,
		Passworded = 8,
	}

	public class ServerListViewModel : ViewModel
	{
		public const string RxServerList = "http://renegadexgs.appspot.com/servers.jsp";
		public const string RxVersion    = "Open Beta 1";

		private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		                                                              {
			                                                              TypeNameHandling       = TypeNameHandling.All,
			                                                              TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
		                                                              };

		private CancellationToken token;
		private readonly CancellationTokenSource tokenSource;
		private CollectionViewSource viewSource;
		private ServerFilter filter;
		private readonly ObservableCollection<ServerViewModel> servers;

		public ServerListViewModel()
		{
			tokenSource = new CancellationTokenSource();
			token       = tokenSource.Token;

			RefreshCommand = new ActionCommand(x => Dispatcher.CurrentDispatcher.InvokeAsync(UpdateServerList), x => true);
			StartCommand   = new ActionCommand(ConnectTo, x => Selected != null);

			servers            = new ObservableCollection<ServerViewModel>();
			viewSource         = new CollectionViewSource {Source = servers};
			viewSource.Filter += OnServerListFilter;
			Servers            = viewSource.View;
		}

		~ServerListViewModel()
		{
			tokenSource.Cancel();
		}

		#region Properties

		public ICommand RefreshCommand { get; private set; }

		public ICommand StartCommand { get; private set; }

		public ICollectionView Servers { get; private set; }

		public ServerViewModel Selected
		{
			get { return (ServerViewModel)Servers.CurrentItem; }
		}

		public bool ShowEmpty
		{
			get { return filter.HasFlag(ServerFilter.Empty); }
			set
			{
				if (value) filter |= ServerFilter.Empty;
				else filter &= ~ServerFilter.Empty;
			}
		}

		public bool ShowFull
		{
			get { return filter.HasFlag(ServerFilter.Full); }
			set
			{
				if (value) filter |= ServerFilter.Full;
				else filter &= ~ServerFilter.Full;
			}
		}

		public bool ShowWithBots
		{
			get { return filter.HasFlag(ServerFilter.Bots); }
			set
			{
				if (value) filter |= ServerFilter.Bots;
				else filter &= ~ServerFilter.Bots;
			}
		}

		public bool ShowWithPassword
		{
			get { return filter.HasFlag(ServerFilter.Passworded); }
			set
			{
				if (value) filter |= ServerFilter.Passworded;
				else filter &= ~ServerFilter.Passworded;
			}
		}

		#endregion

		#region Methods

		private void OnServerListFilter(object sender, FilterEventArgs e)
		{
			ServerViewModel model = e.Item as ServerViewModel;
			if (model != null)
			{
				if (model.Version != RxVersion) e.Accepted = false;

				if (ShowEmpty && model.Players == 0) e.Accepted = true;
				if (ShowFull && model.Players == model.MaxPlayers) e.Accepted = true;
				if (ShowWithBots && model.Bots > 0) e.Accepted = true;
				if (ShowWithPassword && model.IsPassworded) e.Accepted = true;
			}
		}

		public void Refresh(object x)
		{
			viewSource.SortDescriptions.Add(new SortDescription("Players", ListSortDirection.Descending));
			Servers.Refresh();
		}

		private void ConnectTo(object x)
		{
			if (ReferenceEquals(null, x)) return;

			ServerViewModel model = x as ServerViewModel;
			if (model != null)
			{
				//  TODO: launch game and join server!
				MessageBox.Show(string.Format("This will connect to: {0}:{1}", model.IP, model.Port), "Connecting!");
			}
		}

		public async void UpdateServerList()
		{
			string serverList;
			using (WebClient client = new WebClient())
			{
				serverList = await client.DownloadStringTaskAsync(RxServerList);
			}

			IList<Server> list = JsonConvert.DeserializeObject<IList<Server>>(serverList, JsonSettings);
			if (list != null)
			{
				lock (servers)
				{
					foreach (Server item in list)
					{
						ServerViewModel s = servers.SingleOrDefault(x => x.IP == item.ServerAddress && x.Port == item.Port);
						if (s != null)
						{
							int index = servers.IndexOf(s);

							s = ServerViewModel.FromServer(item);
							Application.Current.Dispatcher.Invoke(() =>
							                                    {
								                                    servers.RemoveAt(index);
								                                    servers.Insert(index, s);
							                                    });
						}
						else
						{
							s = ServerViewModel.FromServer(item);

							Application.Current.Dispatcher.Invoke(() => servers.Add(s));
						}
					}

					Application.Current.Dispatcher.Invoke(() => Refresh(this));
				}
			}
		}

		public void UpdateServerPings()
		{
			foreach (ServerViewModel server in servers)
			{
				server.RefreshPing();
			}
		}

		#endregion
	}
}
