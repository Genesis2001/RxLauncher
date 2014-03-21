// -----------------------------------------------------------------------------
//  <copyright file="ConfigurationManager.cs" company="Zack Loveless">
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

	public class ConfigurationManager
	{
		private static Configuration instance;

		public static Configuration Instance
		{
			get { return instance ?? (instance = Load("config.xml")); }
		}

		public static Configuration Load(String path)
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

		public static void Save(Configuration config, String path)
		{
			Save(config, new FileStream(path, FileMode.Truncate, FileAccess.Write));
		}

		public static void Save(Configuration config, Stream stream)
		{
			using (stream)
			{
				using (XmlTextWriter writer = new XmlTextWriter(stream, new UTF8Encoding(false)))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

					serializer.Serialize(writer, config);
					writer.Flush();
				}
			}
		}
	}
}
