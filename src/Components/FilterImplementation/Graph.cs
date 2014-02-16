using System;
using System.Collections.Generic;
using DataStructures;

namespace FilterImplementation
{
	public class Graph : IGraph
	{
		private readonly Dictionary<Guid, IFilter> _filters = new Dictionary<Guid, IFilter>();

		public IEnumerable<IFilter> Filters
		{
			get
			{
				lock (_filters)
					return _filters.Values;
			}
		}

		public void AddFilter(IFilter newFilter)
		{
			OnAddingFilter(newFilter);
			lock (_filters)
				_filters[newFilter.NodeGuid] = newFilter;
		}

		public void RemoveFilter(Guid id)
		{
			IFilter filter;

			lock (_filters)
				filter = _filters[id];
			OnRemovingFilter(filter);
			lock (_filters)
				_filters.Remove(id);
		}

		private void OnAddingFilter(IFilter newFilter)
		{
		}

		private void OnRemovingFilter(IFilter loadedFilter)
		{
		}
	}
}