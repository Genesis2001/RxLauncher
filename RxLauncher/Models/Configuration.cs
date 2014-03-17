// -----------------------------------------------------------------------------
//  <copyright file="Configuration.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace RxLauncher.Models
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;

	[Serializable, XmlRoot("Config")]
	public class Configuration
	{
		private static Configuration instance;

		private Configuration()
		{
		}

		/// <summary>
		/// Represents a singleton instance of the Configuration class.
		/// </summary>
		public static Configuration Instance
		{
			get { return instance ?? (instance = Load("config.xml")); }
		}

		public static Configuration Load(string path)
		{
			return Load(new FileStream(path, FileMode.Open, FileAccess.Read));
		}

		public static Configuration Load(Stream stream)
		{
			using (stream)
			{
				using (XmlTextReader reader = new XmlTextReader(stream))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

					return (Configuration)serializer.Deserialize(reader);
				}
			}
		}

		[XmlElement("InstallationPath")]
		public string InstallationPath { get; private set; }

		[XmlArrayItem(Type = typeof(ServerInfo))]
		public ServerInfo[] Favorites { get; private set; }

		public void Save(String path)
		{
			Save(new FileStream(path, FileMode.Truncate, FileAccess.Write));
		}

		public void Save(Stream stream)
		{
			using (stream)
			{
				using (XmlTextWriter writer = new XmlTextWriter(stream, new UTF8Encoding(false)))
				{
					XmlSerializer serializer = new XmlSerializer(typeof (Configuration));

					serializer.Serialize(writer, this);
					writer.Flush();
				}
			}
		}

		public struct ServerInfo
		{
			public string Address;
			public int Port;
		}
	}
}
