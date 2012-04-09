using System;
using DataStructures;

namespace FilterImplementation.Base
{
	public class OutputPin : Pin, IOutputPin
	{
		public OutputPin(string name, PinMediaType mediaType)
			: base(name, mediaType)
		{
		}

		#region Overrides of Pin

		public override bool IsOutput
		{
			get { return true; }
		}

		#endregion

		#region IOutputPin Members

		public void SetData(object value)
		{
			var inputPin = (InputPin) ConnectedTo;
			if (inputPin != null) 
				inputPin.SetData(value);
		}

		#endregion
	}
}