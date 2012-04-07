using System;
using DataStructures;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;
using FilterImplementation.Filters.Source;

namespace FilterImplementation.Destination
{
	public class DestFileImage : DestinationBlock
	{
		private readonly InputPin _inputPin;

		public DestFileImage()
		{
			AddProperty(new Property("FilePath", FilterPropertyType.String));

			_inputPin = new InputPin();
			AddPin(_inputPin);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("F3C2E875-0D6E-40DF-A6C4-E7F31526F21C"); }
		}

		public override void Process()
		{
			var f = _inputPin.GetData();
			f.Save("");
		}

		#endregion

		#region Nested type: InputPin

		private class InputPin : BasePin<Image<Gray, byte>>
		{
			#region Overrides of BasePin<Image<Gray,byte>>

			public override string Name
			{
				get { return "Image"; }
			}

			public override PinMediaType MediaType
			{
				get { return PinMediaType.Image; }
			}

			public override bool IsOutput
			{
				get { return false; }
			}

			#endregion
		}

		#endregion
	}
}