using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Common;
using DataStructures;

namespace FilterImplementation
{
	public static class FiltersHelper
	{
		private static readonly Dictionary<Guid, Type> Filters;

		static FiltersHelper()
		{
			Filters = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(x => typeof (IFilter).IsAssignableFrom(x))
				.Where(x => !x.IsAbstract)
				.Select(x =>
				        	{
				        		var instance = (IFilter) Assembly.GetExecutingAssembly().CreateInstance(x.FullName);
				        		Debug.Assert(instance != null);

				        		return new
				        		       	{
				        		       		instance.TypeGuid,
				        		       		Type = x,
				        		       	};
				        	})
				.ToDictionary(x => x.TypeGuid, x => x.Type);
		}

		public static IDictionary<Guid, Type> GetFilterTypes()
		{
			return Filters.AsReadOnly();
		}

		public static Type GetFilterType(Guid typeGuid)
		{
			return Filters[typeGuid];
		}
	}
}