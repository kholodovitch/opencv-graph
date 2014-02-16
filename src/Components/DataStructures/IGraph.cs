using System;
using System.Collections.Generic;

namespace DataStructures
{
	public interface IGraph
	{
		IEnumerable<IFilter> Filters { get; }

		void AddFilter(IFilter filter);
		void RemoveFilter(Guid id);
	}
}