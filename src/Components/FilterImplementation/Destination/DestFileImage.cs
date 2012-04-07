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
		private readonly Property _filepathProperty;

		public DestFileImage()
		{
			_filepathProperty = new Property("FilePath", FilterPropertyType.String);
			AddProperty(_filepathProperty);

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
			var image = _inputPin.GetData();
			image.Save((string) _filepathProperty.Value);
		}

		#endregion

		#region Nested type: InputPin

		private class InputPin : BasePin<Image<Bgr, byte>>
		{
			#region Overrides of BasePin<Image<Bgr,byte>>

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