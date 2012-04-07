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
			using (FileStream stream = File.OpenRead(path))
				return (Graph) Serializer.Deserialize(stream);
		}

		public static void Save(Graph graph, string path)
		{
			using (FileStream stream = File.Create(path))
				Serializer.Serialize(stream, graph);
		}
	}
}