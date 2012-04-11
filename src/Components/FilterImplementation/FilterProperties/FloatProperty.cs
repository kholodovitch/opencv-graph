using DataStructures.Enums;
using FilterImplementation.Base;

namespace FilterImplementation.FilterProperties
{
	internal class FloatProperty : Property
	{
		public FloatProperty(string name) : base(name, FilterPropertyType.Float)
		{
		}

		public new float Value
		{
			get { return (float) base.Value; }
			set { base.Value = value; }
		}
		
	}
}