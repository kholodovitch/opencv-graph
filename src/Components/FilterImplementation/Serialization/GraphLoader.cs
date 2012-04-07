using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DataStructures;
using FilterImplementation.Base;

namespace FilterImplementation.Serialization
{
	public class GraphLoader
	{
		private GraphLoader(SaveOptions options)
		{
			AddComments = (options | SaveOptions.AddComments) == SaveOptions.AddComments;
		}

		private bool AddComments { get; set; }

		public static IGraph Load(string path)
		{
			return null;
		}

		public static void Save(Graph graph, string path, SaveOptions options = SaveOptions.Default)
		{
			var graphLoader = new GraphLoader(options);
			graphLoader.SaveInternal(graph, path);
		}

		private void SaveInternal(Graph graph, string path)
		{
			var xDoc = new XDocument();

			var root = new XElement("OCVGraph");
			root.Add(new XAttribute("FormatVersion", "0.1"));

			var filtersNode = new XElement("Filters");
			root.Add(filtersNode);

			graph.Filters
				.SelectMany(GetFilterNodes)
				.ToList()
				.ForEach(filtersNode.Add);

			xDoc.Add(root);

			var xws = new XmlWriterSettings
			          	{
			          		Indent = true,
			          		IndentChars = "\t"
			          	};

			using (XmlWriter xw = XmlWriter.Create(path, xws))
				xDoc.Save(xw);
		}

		private IEnumerable<XNode> GetFilterNodes(Filter x)
		{
			if (AddComments)
				yield return new XComment(string.Format(" {0} ", x.GetType().Name));

			var filterParams = new[]
			                   	{
			                   		new XAttribute("NodeGuid", x.NodeGuid),
			                   		new XAttribute("TypeGuid", x.TypeGuid),
			                   	};
			var filterNode = new XElement("Filter", filterParams.Select(y => (object) y));

			if (!string.IsNullOrEmpty(x.Name))
				filterNode.Add(new XAttribute("Name", x.Name));
			yield return filterNode;
		}
	}
}