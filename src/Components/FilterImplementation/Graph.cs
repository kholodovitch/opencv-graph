using System;
using System.Collections.Generic;
using DataStructures;
using FilterImplementation.Base;

namespace FilterImplementation
{
	public class Graph : IGraph
	{
		private readonly List<IFilter> _filters = new List<IFilter>();

		public IEnumerable<IFilter> Filters
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
						IFilter loadedFilter = _filters[lastiIndex];
						OnRemovingFilter(loadedFilter);
						_filters.RemoveAt(lastiIndex);
					}

					foreach (IFilter newFilter in value)
					{
						AddFilter(newFilter);
					}
				}
			}
		}

		public void AddFilter(IFilter newFilter)
		{
			OnAddingFilter(newFilter);
			lock (_filters)
				_filters.Add(newFilter);
		}

		private void OnAddingFilter(IFilter newFilter)
		{
		}

		private void OnRemovingFilter(IFilter loadedFilter)
		{
		}
	}
}