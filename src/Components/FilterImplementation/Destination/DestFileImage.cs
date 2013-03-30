using System;
using DataStructures;
using DataStructures.Enums;
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
			_filepathProperty = new Property("FilePath", FilterPropertyType.String) {Value = "filename.png"};
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
			FireProcessingStateChanged(ProcessingState.Started);
			var image = (IImage)_inputPin.GetData();

			image.Save((string)_filepathProperty.Value);
			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}