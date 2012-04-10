using System;
using DataStructures;
using DataStructures.Enums;

namespace FilterImplementation.Base
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

		private object _value;

		public string Name
		{
			get { return _name; }
		}

		public FilterPropertyType Type
		{
			get { return _type; }
		}

		public object Value
		{
			get { return _value; }
			set
			{
				if (_value == value)
					return;

				_value = value;
				OnValueChanged(value);
			}
		}

		#endregion

		public event Action<object> OnValueChanged = o => { };
	}
}