// -----------------------------------------------------------------------------
//  <copyright file="ServerListViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Linq;
	using System.Net;
	using System.Runtime.Serialization.Formatters;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Input;
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
		private readonly IoCContainer iocc;
		public const string RxServerList = "http://renegadexgs.appspot.com/servers.jsp";
		public const string RxVersion    = "Open Beta 2";

		private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		                                                              {
			                                                              TypeNameHandling       = TypeNameHandling.All,
			                                                              TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
		                                                              };

		private readonly CancellationToken token;
		private readonly CancellationTokenSource tokenSource;
		private readonly CollectionViewSource viewSource;
		private ServerFilter filter;
		private readonly ObservableCollection<ServerViewModel> servers;

		private readonly Configuration config;

		private readonly Task pingTask;

		public ServerListViewModel(IoCContainer iocc)
		{
			this.iocc   = iocc;
			config      = iocc.RetrieveContract<Configuration>();

			tokenSource = new CancellationTokenSource();
			token       = tokenSource.Token;

			RefreshCommand    = new ActionCommand(x => Application.Current.Dispatcher.InvokeAsync(UpdateServerList), x => true);
			JoinServerCommand = new ActionCommand(ConnectTo, x => Servers.CurrentItem != null);

			servers    = new ObservableCollection<ServerViewModel>();
			viewSource = new CollectionViewSource {Source = servers};
			viewSource.SortDescriptions.Add(new SortDescription("Players", ListSortDirection.Descending));
			viewSource.Filter += (s, e) =>
			                     {
				                     ServerViewModel model = e.Item as ServerViewModel;

				                     e.Accepted = model != null && model.Version.Equals(RxVersion, StringComparison.OrdinalIgnoreCase);
			                     };

			Servers    = viewSource.View;
			pingTask   = Task.Factory.StartNew(PingServers, token);
		}

		~ServerListViewModel()
		{
			tokenSource.Cancel();
		}

		#region Commands

		public ICommand RefreshCommand { get; private set; }

		public ICommand JoinServerCommand { get; private set; }

		#endregion

		#region Properties

		public ICollectionView Servers { get; private set; }

		public ServerViewModel Selected
		{
			get { return (ServerViewModel)Servers.CurrentItem; }
			set
			{
				Servers.MoveCurrentTo(value);
				MessageBox.Show("Selection set!");
			}
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

		/// <summary>
		/// Adds a server from the favorites list.
		/// </summary>
		/// <param name="server"></param>
		public void AddFavorite(Server server)
		{
			if (!config.Servers.Any(x => x.Equals(server)))
			{
				config.Servers.Add(server);
			}
		}

		public void RemoveFavorite(Server server)
		{
			if (config.Servers.Any(x => x.Equals(server)))
			{
				int index = config.Servers.IndexOf(server);

				config.Servers.RemoveAt(index);
			}
		}

		private void ConnectTo(object x)
		{
			if (ReferenceEquals(null, x)) return;
			if (!(x is ServerViewModel)) return;

			((ServerViewModel)x).JoinServer(x);
		}
		
		public void Refresh(object x = null)
		{
			Servers.Refresh();
		}
		
		private void PingServers()
		{
			try
			{
				while (!pingTask.IsCanceled)
				{
					Task.Delay(2 * 60 * 1000, token).Wait(token);

					UpdateServerPings();
				}
			}
			catch (TaskCanceledException)
			{
				// nomnomnom.
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

							s = ServerViewModel.FromServer(item, iocc);
							Application.Current.Dispatcher.Invoke(() =>
							                                    {
								                                    servers.RemoveAt(index);
								                                    servers.Insert(index, s);
							                                    });
						}
						else
						{
							s = ServerViewModel.FromServer(item, iocc);

							Application.Current.Dispatcher.Invoke(() => servers.Add(s));
						}
					}

					Application.Current.Dispatcher.Invoke(() => Refresh(this));
				}

				await Task.Factory.StartNew(() => Application.Current.Dispatcher.InvokeAsync(UpdateServerPings), token);
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
