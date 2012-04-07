using System;
using DataStructures;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Destination
{
	public class DestFileImage : Filter
	{
		private readonly InputPin _inputPin;
		private readonly Property _filepathProperty;

		public DestFileImage()
		{
			_filepathProperty = new Property("FilePath", FilterPropertyType.String);
			AddProperty(_filepathProperty);

			_inputPin = new InputPin("Image", PinMediaType.Image);
			AddPin(_inputPin);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("F3C2E875-0D6E-40DF-A6C4-E7F31526F21C"); }
		}

		public override void Process()
		{
			var image = (IImage)_inputPin.GetData();
			
			image.Save((string) _filepathProperty.Value);
		}

		#endregion
	}
}