using System;
using System.Collections.Generic;
using DataStructures;
using FilterImplementation.Base;

namespace FilterImplementation
{
	[Serializable]
	public class Graph : IGraph
	{
		public List<Filter> Filters { get; set; }
	}
}