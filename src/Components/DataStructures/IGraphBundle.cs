using System;
using System.Collections.Generic;
using System.Drawing;

namespace DataStructures
{
	public interface IGraphBundle
	{
		IGraph Graph { get; }

		IDictionary<Guid, Point> Locations { get; }
	}
}