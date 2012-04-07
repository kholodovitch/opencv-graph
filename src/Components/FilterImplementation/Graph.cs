using System;
using System.Collections.Generic;
using DataStructures;
using FilterImplementation.Base;

namespace FilterImplementation
{
	public class Graph : IGraph
	{
		private readonly List<Filter> _filters = new List<Filter>();

		public IEnumerable<Filter> Filters
		{
			get
			{
				lock (_filters)
				{
					return _filters.AsReadOnly();
				}
			}
			set
			{
				lock (_filters)
				{
					while (_filters.Count > 0)
					{
						int lastiIndex = _filters.Count - 1;
						Filter loadedFilter = _filters[lastiIndex];
						OnRemovingFilter(loadedFilter);
						_filters.RemoveAt(lastiIndex);
					}

					foreach (Filter newFilter in value)
					{
						AddFilter(newFilter);
					}
				}
			}
		}

		public void AddFilter(Filter newFilter)
		{
			OnAddingFilter(newFilter);
			lock (_filters)
				_filters.Add(newFilter);
		}

		private void OnAddingFilter(Filter newFilter)
		{
		}

		private void OnRemovingFilter(Filter loadedFilter)
		{
		}
	}
}