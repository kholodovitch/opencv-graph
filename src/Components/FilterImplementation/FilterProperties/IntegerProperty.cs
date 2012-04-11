using DataStructures.Enums;

namespace FilterImplementation.FilterProperties
{
	internal class IntegerProperty : NumericProperty
	{
		internal IntegerProperty(string name, decimal min, decimal max, decimal step)
			: base(name, FilterPropertyType.Integer, min, max, step, 0)
		{
		}

		public new int Value
		{
			get { return (int)base.Value; }
			set { base.Value = value; }
		}
	}
}