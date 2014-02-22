using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DataStructures;
using DataStructures.Enums;
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

		public static void Save(IGraphBundle graph, string path, SaveOptions options = SaveOptions.Default)
		{
			var graphLoader = new GraphSaver(options);
			graphLoader.SaveInternal(graph, path);
		}

		private void SaveInternal(IGraphBundle graph, string path)
		{
			var xDoc = new XDocument();

			var root = new XElement(GraphFileFormat.NodeRoot);
			root.Add(new XAttribute(GraphFileFormat.NodeFormatVersion, FileFormat.Version));

			var filtersNode = new XElement(FileFormat.Node_Filters);
			graph.Graph.Filters
				.SelectMany(GetFilterNodes)
				.ToList()
				.ForEach(filtersNode.Add);
			root.Add(filtersNode);

			if (graph.Locations != null)
			{
				var locationsNode = new XElement(FileFormat.Node_Locations);
				graph.Locations
					.Select(GetLocationNode)
					.ToList()
					.ForEach(locationsNode.Add);
				root.Add(locationsNode);
			}

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

			filter.Pins
				.SelectMany(GetFilterConnectionNodes)
				.ToList()
				.ForEach(filterNode.Add);

			yield return filterNode;
		}

		private IEnumerable<XNode> GetFilterPropertyNodes(IFilterProperty filterProperty)
		{
			var filterPropertyNode = new XElement(FileFormat.Node_FilterProperty);
			filterPropertyNode.Add(new XAttribute(FileFormat.Node_FilterProperty_Name, filterProperty.Name));

			object propertyValue = filterProperty.Value;
			if (propertyValue != null)
			{
				FilterPropertyType propertyType = filterProperty.Type;
				if (IsSerializableAsString(propertyType))
				{
					string valueAsStr = SerializeAsString(propertyType, propertyValue);
					filterPropertyNode.Add(new XAttribute(FileFormat.Node_FilterProperty_Value, valueAsStr));
				}
				else
					throw new NotImplementedException();
			}

			yield return filterPropertyNode;
		}

		private IEnumerable<XNode> GetFilterConnectionNodes(IPin pin)
		{
			if (!pin.IsOutput)
				yield break;

			var result = new XElement("Pin", new XAttribute("Name", pin.Name));
			if (pin.IsConnected)
			{
				result.Add(new XAttribute("ConnectedToNode", pin.ConnectedTo.Filter.NodeGuid));
				result.Add(new XAttribute("ConnectedToPin", pin.ConnectedTo.Name));
			}
			yield return result;
		}

		private XNode GetLocationNode(KeyValuePair<Guid, Point> location)
		{
			var locationNode = new XElement(FileFormat.Node_Location);
			locationNode.Add(new XAttribute(FileFormat.Node_Location_Node, location.Key));
			locationNode.Add(new XAttribute(FileFormat.Node_Location_X, location.Value.X));
			locationNode.Add(new XAttribute(FileFormat.Node_Location_Y, location.Value.Y));
			return locationNode;
		}

		private bool IsSerializableAsString(FilterPropertyType filterPropertyType)
		{
			switch (filterPropertyType)
			{
				case FilterPropertyType.Float:
				case FilterPropertyType.Integer:
				case FilterPropertyType.String:
				case FilterPropertyType.Point:
				case FilterPropertyType.Size:
				case FilterPropertyType.Enum:
					return true;

				default:
					throw new ArgumentOutOfRangeException("filterPropertyType");
			}
		}

		private string SerializeAsString(FilterPropertyType filterPropertyType, object value)
		{
			switch (filterPropertyType)
			{
				case FilterPropertyType.Float:
					return ((float)value).ToString(GraphFileFormat.Ver_0_1.NumberStyles);

				case FilterPropertyType.Integer:
					return ((int)value).ToString(GraphFileFormat.Ver_0_1.NumberStyles);

				case FilterPropertyType.String:
				case FilterPropertyType.Enum:
					return value.ToString();

				case FilterPropertyType.Point:
					var point = (Point) value;
					var x = point.X.ToString(GraphFileFormat.Ver_0_1.NumberStyles);
					var y = point.Y.ToString(GraphFileFormat.Ver_0_1.NumberStyles);
					return x + "," + y;

				case FilterPropertyType.Size:
					var size = (Size)value;
					var width = size.Width.ToString(GraphFileFormat.Ver_0_1.NumberStyles);
					var height = size.Height.ToString(GraphFileFormat.Ver_0_1.NumberStyles);
					return width + "," + height;

				default:
					throw new ArgumentOutOfRangeException("filterPropertyType");
			}
		}
	}
}