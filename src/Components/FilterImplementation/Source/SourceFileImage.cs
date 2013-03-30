using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Source
{
	public class SourceFileImage : Filter
	{
		private readonly OutputPin _outputPin;
		private readonly Property _filepathProperty;

		public SourceFileImage()
		{
			_filepathProperty = new Property("FilePath", FilterPropertyType.String) {Value = "filename.png"};
			AddProperty(_filepathProperty);

			_outputPin = new OutputPin("Image", PinMediaType.Image);
			AddPin(_outputPin);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("23949518-127F-4909-801E-72EC65340046"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var image = new Image<Bgr, byte>((string) _filepathProperty.Value);
			_outputPin.SetData(image);
			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}