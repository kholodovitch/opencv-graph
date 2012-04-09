using System;
using System.Collections.Generic;
using System.Drawing;
using DataStructures;

namespace FilterImplementation.Base
{
	public class GraphBundle : IGraphBundle
	{
		public GraphBundle()
			: this(new Graph())
		{
		}

		public GraphBundle(Graph graph)
		{
			Graph = graph;
			Locations = new Dictionary<Guid, Point>();
		}

		public IGraph Graph { get; private set; }

		public IDictionary<Guid, Point> Locations { get; private set; }
	}
}