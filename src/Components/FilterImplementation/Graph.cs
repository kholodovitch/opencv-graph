using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using FilterImplementation.Base;

namespace FilterImplementation
{
	[Serializable]
	public class Graph : IGraph
	{
		private readonly List<IFilter> _filters = new List<IFilter>();

		public void AddSourceFilter(IFilter filter)
		{
			lock (_filters)
				_filters.Add(filter);
		}

		public void Start()
		{
			lock (_filters)
			{
				_filters
					.OfType<SourceBlock>()
					.ToList()
					.ForEach(x => x.Start());
			}
		}
	}
}
