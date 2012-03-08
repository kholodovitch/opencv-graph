using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	public class Blur : Filter
	{
		public void Process()
		{
			InputPins
				.First()
				.GetData();
		}
	}
}
