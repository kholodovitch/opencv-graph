using DataStructures;
using DataStructures.Enums;
using FilterImplementation.Base;

namespace FilterImplementation.FilterProperties
{
	internal class EnumProperty : Property, IEnumProperty
	{
		public EnumProperty(string name, FilterPropertyType type, string[] values)
			: base(name, type)
		{
			Values = values;
		}

		public string[] Values { get; private set; }
	}
}
