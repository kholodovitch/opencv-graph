using System;
using System.Drawing;
using DataStructures;
using DataStructures.Enums;
using FilterImplementation.Base;

namespace FilterImplementation.Destination
{
	internal class PointProperty : Property, IPointProperty
	{
		internal PointProperty(string name, FilterPropertyType type)
			: base(name, type)
		{
		}

		public Point Point { get; set; }

		public object Value
		{
			get { return base.Value; }
			set
			{
				if (value is Size && Type != FilterPropertyType.Size)
					throw new InvalidCastException();

				if (value is Point && Type != FilterPropertyType.Point)
					throw new InvalidCastException();

				base.Value = value;
			}
		}
	}
}
