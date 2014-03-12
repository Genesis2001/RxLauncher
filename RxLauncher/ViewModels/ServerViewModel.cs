// -----------------------------------------------------------------------------
//  <copyright file="ServerViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.ViewModels
{
	using System;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using Models;

	public class ServerViewModel : Server, IObservableClass
	{
		private long ping;
		private bool isSelected;

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

		public Task<long> Ping()
		{
			throw new NotImplementedException();
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
