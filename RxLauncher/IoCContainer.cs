// -----------------------------------------------------------------------------
//  <copyright file="IoCContainer.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;

	public class IoCContainer
	{
		private readonly IDictionary<Type, Object> contracts; 

		public IoCContainer()
		{
			contracts = new ConcurrentDictionary<Type, Object>();
		}

		public T RegisterContract<T>(T contract, Boolean reset = false)
		{
			if (contracts.ContainsKey(typeof (T)))
			{
				if (!reset)
				{
					throw new InvalidOperationException(
						String.Format("Unable to register the specified contract. A '{0}' already registered.", typeof (T).Name));
				}

				contracts.Remove(typeof(T));
			}

			contracts.Add(typeof (T), contract);
			return contract;
		}

		public T RetrieveContract<T>()
		{
			Object contract;

			return contracts.TryGetValue(typeof (T), out contract) ? (T)contract : default(T);
		}
	}
}
