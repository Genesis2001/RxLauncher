// -----------------------------------------------------------------------------
//  <copyright file="IoCContainer.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher
{
	using System;
	using System.Collections.Generic;

	public class IoCContainer
	{
		private readonly Dictionary<Type, object> contracts; 

		public IoCContainer()
		{
			contracts = new Dictionary<Type, object>();
		}

		public T RegisterContract<T>(T contract) where T : class 
		{
			if (contracts.ContainsKey(typeof (T)))
			{
				throw new InvalidOperationException(
					String.Format("Unable to register the specified contract. A '{0}' already registered.", typeof (T).Name));
			}

			contracts.Add(typeof (T), contract);
			return contract;
		}

		public T RetrieveContract<T>() where T : class
		{
			object contract;

			return contracts.TryGetValue(typeof (T), out contract) ? (T)contract : null;
		}
	}
}
