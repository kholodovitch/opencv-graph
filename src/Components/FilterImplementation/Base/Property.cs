using DataStructures;

namespace FilterImplementation.Filters.Source
{
	internal class Property : IFilterProperty
	{
		private readonly string _name;
		private readonly FilterPropertyType _type;

		public Property(string name, FilterPropertyType type)
		{
			_name = name;
			_type = type;
		}

		#region Implementation of IFilterProperty

		public string Name
		{
			get { return _name; }
		}

		public FilterPropertyType Type
		{
			get { return _type; }
		}

		#endregion
	}
}