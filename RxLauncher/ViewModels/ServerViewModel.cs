// -----------------------------------------------------------------------------
//  <copyright file="ServerViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Runtime.CompilerServices;
	using System.Windows;
	using System.Windows.Input;
	using Commands;
	using Models;

	// ReSharper disable InconsistentNaming

	public class ServerViewModel : IObservableClass, IEquatable<Server>
	{
		private readonly IoCContainer iocc;
		private readonly Server server;
		private readonly ServerListViewModel serverList;
		private ICommand addFavoriteCommand;
		private ICommand delFavoriteCommand;
		private ICommand joinServerCommand;
		private Int64 ping;

		private ServerViewModel(Server server, IoCContainer iocc)
		{
			this.server = server;
			this.iocc = iocc;

			serverList = iocc.RetrieveContract<ServerListViewModel>();
		}

		#region Commands

		public ICommand JoinServerCommand
		{
			get { return joinServerCommand ?? (joinServerCommand = new ActionCommand(JoinServer, o => true)); }
		}

		public ICommand AddFavoriteCommand
		{
			get { return addFavoriteCommand ?? (addFavoriteCommand = new ActionCommand(x => IsFavorited = true, x => true)); }
		}

		public ICommand DelFavoriteCommand
		{
			get { return delFavoriteCommand ?? (delFavoriteCommand = new ActionCommand(x => IsFavorited = false, x => true)); }
		}

		#endregion

		#region Properties

		private bool isFavorited;

		public String Name
		{
			get { return server.Name; }
		}

		public Int32 Bots
		{
			get { return server.Bots; }
		}

		public Int32 Players
		{
			get { return server.Players; }
		}

		public string PlayerSummary
		{
			get { return String.Join("/", Players, MaxPlayers); }
		}

		public Int32 MaxPlayers
		{
			get { return server.Settings.PlayerLimit; }
		}

		public String Version
		{
			get { return server.Version; }
		}

		public String IP
		{
			get { return server.ServerAddress; }
		}

		public Int32 Port
		{
			get { return server.Port; }
		}

		public Boolean IsPassworded
		{
			get { return server.Settings.IsPassworded; }
		}

		public bool IsFavorited
		{
			get { return isFavorited; }
			set
			{
				NotifyPropertyChanging();
				isFavorited = value;
				NotifyPropertyChanged();
			}
		}

		public Int64 Ping
		{
			get { return ping; }
			private set
			{
				NotifyPropertyChanging();
				ping = value;
				NotifyPropertyChanged();
			}
		}

		#endregion

		#region Methods

		public async void RefreshPing()
		{
			IPAddress address;
			if (!IPAddress.TryParse(IP, out address))
			{
				var entry = Dns.GetHostEntry(IP);
				address   = entry.AddressList[0];
			}

			using (var p = new Ping())
			{
				PingReply reply = await p.SendPingAsync(address, 1500);

				Ping = reply.Status == IPStatus.Success ? reply.RoundtripTime : 999;

				Debug.WriteLine("IP: {0} Status: {1}", IP, reply.Status);
			}
		}

		public void JoinServer(object o)
		{
			MessageBox.Show(string.Format("This will connect to: {0}:{1}", IP, Port), "Connecting!");
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Server other)
		{
			return server.Equals(other);
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		///     A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return Name;
		}

		#endregion

		#region Conversion methods

		public static ServerViewModel FromServer(Server p, IoCContainer iocc = null)
		{
			return new ServerViewModel(p, iocc);
		}

		public static Server ToServer(ServerViewModel vm)
		{
			return vm.server;
		}

		#endregion

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Implementation of INotifyPropertyChanging

		public event PropertyChangingEventHandler PropertyChanging;

		public virtual void NotifyPropertyChanging([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanging;
			if (handler != null) handler(this, new PropertyChangingEventArgs(propertyName));
		}

		#endregion
	}

	// ReSharper enable InconsistentNaming
}
