using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DataStructures;
using FilterImplementation.Base;
using FileFormat = FilterImplementation.Serialization.GraphFileFormat.Ver_0_1;

namespace FilterImplementation.Serialization
{
	public class GraphSaver
	{
		private GraphSaver(SaveOptions options)
		{
			AddComments = (options | SaveOptions.AddComments) == SaveOptions.AddComments;
		}

		private bool AddComments { get; set; }

		public static void Save(Graph graph, string path, SaveOptions options = SaveOptions.Default)
		{
			var graphLoader = new GraphSaver(options);
			graphLoader.SaveInternal(graph, path);
		}

		private void SaveInternal(Graph graph, string path)
		{
			var xDoc = new XDocument();

			var root = new XElement(GraphFileFormat.NodeRoot);
			root.Add(new XAttribute(GraphFileFormat.NodeFormatVersion, FileFormat.Version));

			var filtersNode = new XElement(FileFormat.Node_Filters);
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

		private IEnumerable<XNode> GetFilterNodes(IFilter filter)
		{
			if (AddComments)
				yield return new XComment(string.Format(" {0} ", filter.GetType().Name));

			var filterParams = new[]
			                   	{
			                   		new XAttribute(FileFormat.Node_Filter_NodeGuid, filter.NodeGuid),
			                   		new XAttribute(FileFormat.Node_Filter_TypeGuid, filter.TypeGuid)
			                   	};
			var filterNode = new XElement(FileFormat.Node_Filter, filterParams.Select(y => (object) y));

			if (!string.IsNullOrEmpty(filter.Name))
				filterNode.Add(new XAttribute(FileFormat.Node_Filter_Name, filter.Name));

			filter.Properties
				.Values
				.SelectMany(GetFilterPropertyNodes)
				.ToList()
				.ForEach(filterNode.Add);

			yield return filterNode;
		}

		private IEnumerable<XNode> GetFilterPropertyNodes(IFilterProperty filterProperty)
		{
			var filterPropertyNode = new XElement(FileFormat.Node_FilterProperty);
			filterPropertyNode.Add(new XAttribute(FileFormat.Node_FilterProperty_Name, filterProperty.Name));

			if (filterProperty.Value != null)
			{
				if (IsSerializableAsString(filterProperty.Type))
					filterPropertyNode.Add(new XAttribute(FileFormat.Node_FilterProperty_Value, filterProperty.Value));
				else
					throw new NotImplementedException();
			}

			yield return filterPropertyNode;
		}

		private bool IsSerializableAsString(FilterPropertyType filterPropertyType)
		{
			switch (filterPropertyType)
			{
				case FilterPropertyType.Integer:
				case FilterPropertyType.String:
					return true;

				default:
					throw new ArgumentOutOfRangeException("filterPropertyType");
			}
		}
	}
}