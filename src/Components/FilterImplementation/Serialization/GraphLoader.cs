using System.Xml.Linq;

namespace FilterImplementation.Serialization
{
	public class GraphLoader
	{
		private GraphLoader()
		{
		}

		public static Graph Load(string path)
		{
			var graphLoader = new GraphLoader();
			return graphLoader.LoadInternal(path);
		}

		private Graph LoadInternal(string path)
		{
			var xDoc = XDocument.Load(path);
			throw new System.NotImplementedException();
		}
	}
}