using DataStructures.Enums;
using FilterImplementation.Base;

namespace FilterImplementation.FilterProperties
{
	internal class IntegerProperty : Property
	{
		public IntegerProperty(string name)
			: base(name, FilterPropertyType.Integer)
		{
		}

		public new int Value
		{
			get { return (int)base.Value; }
			set { base.Value = value; }
		}
	}
}