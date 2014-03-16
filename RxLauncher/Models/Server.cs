// -----------------------------------------------------------------------------
//  <copyright file="Server.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using Newtonsoft.Json;

	[Serializable]
	public class Server
	{
		public string Name { get; set; }

		public int Bots { get; set; }

		public int Players { get; set; }

		[JsonProperty("Game Version")]
		public string Version { get; set; }
		
		[JsonProperty("IP")]
		public string ServerAddress { get; set; }

		public int Port { get; set; }

		[JsonProperty("Variables")]
		public ServerSettings Settings { get; set; }

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

		[Serializable, JsonObject("Variables")]
		public class ServerSettings
		{

			[JsonProperty("bAllowPrivateMessaging")]
			public bool AllowPrivateMessaging { get; set; }

			[JsonProperty("bAutoBalanceTeams")]
			public bool IsAutoBalancedEnabled { get; set; }

			[JsonProperty("bPassworded")]
			public bool IsPassworded { get; set; }

			[JsonProperty("bSteamRequired")]
			public bool IsSteamRequired { get; set; }

			[JsonProperty("Map Name"), JsonIgnore]
			public string MapName { get; set; }

			[JsonProperty("Mine Limit")]
			public int MineLimit { get; set; }

			[JsonProperty("Player Limit")]
			public int PlayerLimit { get; set; }

			[JsonProperty("Time Limit")]
			public int TimeLimit { get; set; }

			[JsonProperty("Vehicle Limit")]
			public int VehicleLimit { get; set; }
		}
	}
}
