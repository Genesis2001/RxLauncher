// -----------------------------------------------------------------------------
//  <copyright file="Server.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	[Serializable]
	public class Server
	{
		public string Name { get; set; }

		public int Bots { get; set; }

		public int Players { get; set; }

		[JsonIgnore]
		public int MaxPlayers
		{
			get
			{
				object result;
				if (Variables != null && Variables.TryGetValue("Player Limit", out result))
				{
					return result is int ? (int)result : int.MaxValue;
				}

				return -1;
			}
		}

		[JsonProperty("Game Version")]
		public string Version { get; set; }
		
		[JsonProperty("IP")]
		public string ServerAddress { get; set; }

		public int Port { get; set; }

		[JsonIgnore]
		public bool IsPassworded
		{
			get
			{
				object oResult;
				if (Variables != null && Variables.TryGetValue("bPassword", out oResult))
				{
					return oResult is bool && (bool)oResult;
				}

				return false;
			}
		}

		[JsonConverter(typeof (KeyValuePairConverter))]
		public Dictionary<string, object> Variables { get; private set; }

		#region Overrides of Object

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0} ({1}:{2})", Name, ServerAddress, Port);
		}

		#endregion
	}
}
