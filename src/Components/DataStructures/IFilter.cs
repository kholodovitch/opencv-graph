using System;
using System.Collections.Generic;

namespace DataStructures
{
	public interface IFilter
	{
		Guid NodeGuid { get; set; }

		Guid TypeGuid { get; }

		string Name { get; set; }

		IEnumerable<IFilterProperty> Properties { get; set; }

		void Process();
	}
}