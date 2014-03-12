// -----------------------------------------------------------------------------
//  <copyright file="IObservableClass.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher
{
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	public interface IObservableClass : INotifyPropertyChanged, INotifyPropertyChanging
	{
		void NotifyPropertyChanged([CallerMemberName] string propertyName = null);

		void NotifyPropertyChanging([CallerMemberName] string propertyName = null);
	}
}
