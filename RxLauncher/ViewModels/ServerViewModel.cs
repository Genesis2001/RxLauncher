// -----------------------------------------------------------------------------
//  <copyright file="ServerViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System;
	using System.ComponentModel;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Runtime.CompilerServices;
	using Models;

	public class ServerViewModel : IObservableClass
	{
		private readonly Server server;
		private long ping;

		private ServerViewModel(Server server)
		{
			this.server = server;
		}

		#region Properties

		public string Name
		{
			get { return server.Name; }
		}

		public int Bots
		{
			get { return server.Bots; }
		}

		public int Players
		{
			get { return server.Players; }
		}
		
		public int MaxPlayers
		{
			get { return server.MaxPlayers; }
		}

		public string Version
		{
			get { return server.Version; }
		}

		public string IP
		{
			get { return server.ServerAddress; }
		}

		public int Port
		{
			get { return server.Port; }
		}

		public bool IsPassworded
		{
			get { return server.IsPassworded; }
		}

		public long Ping
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
				PingReply reply = await p.SendPingAsync(address, 120);

				Ping = reply.RoundtripTime;
			}
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return String.Format("{0} ({1}:{2})", Name, IP, Port);
		}
		
		#endregion

		#region Conversion methods

		public static ServerViewModel FromServer(Server p)
		{
			return new ServerViewModel(p);
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
}
