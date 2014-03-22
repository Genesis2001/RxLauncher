// -----------------------------------------------------------------------------
//  <copyright file="Server.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using System.Xml.Serialization;
	using Newtonsoft.Json;

	[Serializable, XmlRoot("Server")]
	public class Server : IEquatable<Server>
	{
		[XmlIgnore]
		public string Name { get; set; }

		[XmlIgnore]
		public int Bots { get; set; }

		[XmlIgnore]
		public int Players { get; set; }

		[JsonProperty("Game Version"), XmlIgnore]
		public string Version { get; set; }
		
		[JsonProperty("IP"), XmlAttribute("address")]
		public string ServerAddress { get; set; }

		[XmlAttribute("port")]
		public int Port { get; set; }

		[JsonProperty("Variables"), XmlIgnore]
		public ServerSettings Settings { get; set; }
		
		#region Overrides of Object

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Server other)
		{
			return ServerAddress.Equals(other.ServerAddress, StringComparison.OrdinalIgnoreCase) && Port.Equals(other.Port);
		}

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
