using System;
using System.Collections.Generic;
using System.Drawing;
using DataStructures;

namespace FilterImplementation.Base
{
	public class GraphBundle : IGraphBundle
	{
		public IGraph Graph { get; set; }

		public IDictionary<Guid, Point> Locations { get; set; }
	}
}