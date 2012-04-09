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

			var result = new GraphBundle
			             	{
			             		Graph = new Graph(),
			             		Locations = new Dictionary<Guid, Point>()
			             	};
			var filtersNode = xDoc.Root.Element(GraphFileFormat.Ver_0_1.Node_Filters);
			if (filtersNode != null)
			{
				filtersNode
					.Elements(GraphFileFormat.Ver_0_1.Node_Filter)
					.Select(GetFilter)
					.ToList()
					.ForEach(result.Graph.AddFilter);
			}

			return result;
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
	}
}