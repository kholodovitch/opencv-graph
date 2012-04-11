using DataStructures;
using DataStructures.Enums;
using FilterImplementation.Base;

namespace FilterImplementation.FilterProperties
{
	internal class NumericProperty : Property, INumericProperty
	{
		protected NumericProperty(string name, FilterPropertyType type, decimal min, decimal max, decimal step, int decimalPlaces = 0) 
			: base(name, type)
		{
			Min = min;
			Max = max;
			Step = step;
			DecimalPlaces = decimalPlaces;
		}

		#region Implementation of INumericProperty

		public decimal Max { get; private set; }

		public decimal Min { get; private set; }

		public decimal Step { get; private set; }

		public int DecimalPlaces { get; private set; }

		#endregion
	}
}