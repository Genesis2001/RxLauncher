// -----------------------------------------------------------------------------
//  <copyright file="ServerViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Runtime.CompilerServices;
	using System.Windows.Input;
	using Commands;
	using Models;

	// ReSharper disable InconsistentNaming

	public class ServerViewModel : IObservableClass
	{
		private readonly Server server;
		private long ping;

		private ICommand joinServerCommand;

		private ServerViewModel(Server server)
		{
			this.server = server;
		}

		#region Commands

		public ICommand JoinServerCommand
		{
			get
			{
				if (joinServerCommand == null)
				{
					joinServerCommand = new ActionCommand(JoinServer, o => true);
				}

				return joinServerCommand;
			}
		}

		#endregion
		
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
			get { return server.Settings.PlayerLimit; }
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
			get { return server.Settings.IsPassworded; }
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
				PingReply reply = await p.SendPingAsync(address, 1500);

				Ping = reply.Status == IPStatus.Success ? reply.RoundtripTime : 999;

				Debug.WriteLine("IP: {0} Status: {1}", IP, reply.Status);
			}
		}

		public void JoinServer(object o)
		{
			if (!(o is ServerViewModel)) return;

			// MessageBox.Show(String.Format("This will join: {0}:{1}", IP, Port), "Joining...");
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return Name;
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

	// ReSharper enable InconsistentNaming
}
