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

		[JsonProperty("Game Version")]
		public string GameVersion { get; set; }

		[JsonProperty("IP")]
		public string IP { get; set; }

		public int Port { get; set; }

		[JsonConverter(typeof (KeyValuePairConverter))]
		public IDictionary<string, object> Variables { get; private set; }
	}
}
