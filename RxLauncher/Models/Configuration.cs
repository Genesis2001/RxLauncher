// -----------------------------------------------------------------------------
//  <copyright file="Configuration.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using System.Xml.Serialization;

	[Serializable, XmlRoot("Config")]
	public class Configuration
	{
		[XmlElement("InstallationPath")]
		public string InstallationPath { get; private set; }

		[XmlArrayItem(Type = typeof(ServerInfo))]
		public ServerInfo[] Favorites { get; private set; }

		public struct ServerInfo
		{
			public string Address;
			public int Port;
		}
	}
}
