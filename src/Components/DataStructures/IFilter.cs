using System;

namespace DataStructures
{
	public interface IFilter
	{
		Guid Guid { get; set; }

		void Process();
	}
}