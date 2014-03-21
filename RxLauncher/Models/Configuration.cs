// -----------------------------------------------------------------------------
//  <copyright file="Configuration.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Serialization;

	[Serializable, XmlRoot("Config")]
	public class Configuration
	{
		[XmlElement("InstallationPath")]
		public string InstallationPath { get; set; }

		[XmlArray("Favorites")]
		public List<Server> Servers { get; set; }
	}
}
