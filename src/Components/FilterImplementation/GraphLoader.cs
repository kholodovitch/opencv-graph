using System.IO;
using System.Xml.Serialization;
using DataStructures;

namespace FilterImplementation
{
	public static class GraphLoader
	{
		private static readonly XmlSerializer Serializer = new XmlSerializer(typeof (Graph));

		public static IGraph Load(string path)
		{
			using (var stream = File.OpenRead(path))
				return (Graph) Serializer.Deserialize(stream);
		}
	}
}
