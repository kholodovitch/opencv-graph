using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using DataStructures;
using FilterImplementation.Base;

namespace FilterImplementation.Serialization
{
	public class GraphLoader
	{
		private GraphLoader()
		{
		}

		public static GraphBundle Load(string path)
		{
			var graphLoader = new GraphLoader();
			return graphLoader.LoadInternal(path);
		}

		private GraphBundle LoadInternal(string path)
		{
			var xDoc = XDocument.Load(path);
			if (xDoc.Root == null)
				return null;

			if (xDoc.Root.Name != GraphFileFormat.NodeRoot)
				return null;

			var result = new GraphBundle();
			var filtersNode = xDoc.Root.Element(GraphFileFormat.Ver_0_1.Node_Filters);
			if (filtersNode != null)
			{
				filtersNode
					.Elements(GraphFileFormat.Ver_0_1.Node_Filter)
					.Select(GetFilter)
					.ToList()
					.ForEach(result.Graph.AddFilter);

				filtersNode
					.Elements(GraphFileFormat.Ver_0_1.Node_Filter)
					.ToList()
					.ForEach(element=> ProcessConnections(element, result.Graph));
			}

			var locationsNode = xDoc.Root.Element(GraphFileFormat.Ver_0_1.Node_Locations);
			if (locationsNode != null)
			{
				locationsNode
					.Elements(GraphFileFormat.Ver_0_1.Node_Location)
					.Select(GetLocation)
					.ToList()
					.ForEach(x => result.Locations[x.Key] = x.Value);
			}

			return result;
		}

		private static void ProcessConnections(XElement element, IGraph graph)
		{
			var guid = new Guid(element.Attribute(GraphFileFormat.Ver_0_1.Node_Filter_NodeGuid).Value);
			IFilter filter = graph.Filters.First(x1 => x1.NodeGuid == guid);

			element.Elements("Pin")
				.Where(pinNode => pinNode.Attribute("ConnectedToNode") != null)
				.ToList()
				.ForEach(x => PerformConnection(filter, x, graph));
		}

		private static void PerformConnection(IFilter srcFilter, XElement pinNode, IGraph graph)
		{
			var connectedToNode = new Guid(pinNode.Attribute("ConnectedToNode").Value);
			string connectedToPin = pinNode.Attribute("ConnectedToPin").Value;
			string pinName = pinNode.Attribute("Name").Value;

			IFilter destFilter = graph.Filters.First(filter => filter.NodeGuid == connectedToNode);
			IPin outputPin = srcFilter.Pins.First(pin => pin.Name == pinName);
			IPin receivePin = destFilter.Pins.First(pin => pin.Name == connectedToPin);

			outputPin.Connect(receivePin);
		}

		private static IFilter GetFilter(XElement filterNode)
		{
			// ReSharper disable PossibleNullReferenceException
			var typeGuid = new Guid(filterNode.Attribute(GraphFileFormat.Ver_0_1.Node_Filter_TypeGuid).Value);
			var filterType = FiltersHelper.GetFilterType(typeGuid);
			var filter = (IFilter) filterType.Assembly.CreateInstance(filterType.FullName);
			Debug.Assert(filter != null);
			filter.NodeGuid = new Guid(filterNode.Attribute(GraphFileFormat.Ver_0_1.Node_Filter_NodeGuid).Value);

			XAttribute nameAttribute = filterNode.Attribute(GraphFileFormat.Ver_0_1.Node_Filter_Name);
			if (nameAttribute != null)
				filter.Name = nameAttribute.Value;

			var filterPropertyNodes = filterNode.Elements(GraphFileFormat.Ver_0_1.Node_FilterProperty);
			foreach (XElement element in filterPropertyNodes)
			{
				var propertyName = element.Attribute(GraphFileFormat.Ver_0_1.Node_FilterProperty_Name).Value;
				var propertyValue = element.Attribute(GraphFileFormat.Ver_0_1.Node_FilterProperty_Value).Value;
				var filterProperty = filter.Properties[propertyName];

				switch (filterProperty.Type)
				{
					case FilterPropertyType.String:
						filterProperty.Value = propertyValue;
						break;
					case FilterPropertyType.Integer:
						filterProperty.Value = int.Parse(propertyValue);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			return filter;
			// ReSharper restore PossibleNullReferenceException
		}

		private static KeyValuePair<Guid, Point> GetLocation(XElement locationNode)
		{
			string strGuid = locationNode.Attribute(GraphFileFormat.Ver_0_1.Node_Location_Node).Value;
			string strX = locationNode.Attribute(GraphFileFormat.Ver_0_1.Node_Location_X).Value;
			string strY = locationNode.Attribute(GraphFileFormat.Ver_0_1.Node_Location_Y).Value;
			return new KeyValuePair<Guid, Point>(new Guid(strGuid), new Point(int.Parse(strX), int.Parse(strY)));
		}
	}
}