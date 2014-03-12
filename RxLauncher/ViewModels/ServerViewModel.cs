// -----------------------------------------------------------------------------
//  <copyright file="ServerViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System.ComponentModel;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Runtime.CompilerServices;
	using Models;

	public class ServerViewModel : Server, IObservableClass
	{
		private bool isSelected;
		private long ping;

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				NotifyPropertyChanging();
				isSelected = value;
				NotifyPropertyChanged();
			}
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
