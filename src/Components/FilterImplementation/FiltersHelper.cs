using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataStructures;

namespace FilterImplementation
{
	public static class FiltersHelper
	{
		private static readonly Type[] Filters;

		static FiltersHelper()
		{
			Filters = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(x => typeof (IFilter).IsAssignableFrom(x))
				.Where(x => !x.IsAbstract)
				.ToArray();
		}

		public static IEnumerable<Type> GetFilterTypes()
		{
			return Filters;
		}
	}
}
