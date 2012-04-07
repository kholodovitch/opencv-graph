using System;
using DataStructures;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;
using FilterImplementation.Filters.Source;

namespace FilterImplementation.Source
{
	public class SourceFileImage : SourceBlock
	{
		private readonly OutputPin _outputPin;
		private readonly Property _filepathProperty;

		public SourceFileImage()
		{
			_filepathProperty = new Property("FilePath", FilterPropertyType.String);
			AddProperty(_filepathProperty);

			_outputPin = new OutputPin();
			AddPin(_outputPin);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("23949518-127F-4909-801E-72EC65340046"); }
		}

		public override void Process()
		{
			_outputPin.SetData(new Image<Gray, byte>((string)_filepathProperty.Value));
		}

		#endregion

		#region Nested type: OutputPin

		private class OutputPin : BasePin<Image<Gray, byte>>
		{
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
				get { return true; }
			}
		}

		#endregion
	}
}