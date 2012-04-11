using DataStructures.Enums;

namespace FilterImplementation.FilterProperties
{
	internal class FloatProperty : NumericProperty
	{
		internal FloatProperty(string name, decimal min, decimal max, decimal step, int decimalPlaces)
			: base(name, FilterPropertyType.Float, min, max, step, decimalPlaces)
		{
		}

		public new float Value
		{
			get { return (float) base.Value; }
			set { base.Value = value; }
		}
	}
}