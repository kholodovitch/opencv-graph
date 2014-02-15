using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	internal class ConvertToHsv : Filter
	{
		private readonly InputPin _input;
		private readonly OutputPin _output;

		public ConvertToHsv()
		{
			_input = new InputPin("Input", PinMediaType.Image);
			AddPin(_input);
			_output = new OutputPin("HSV", PinMediaType.Image);
			AddPin(_output);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("ED0550FD-4A25-4580-A7DD-88B191A0294D"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame = (IImage)_input.GetData();

			if (frame is Image<Rgb, byte>)
				_output.SetData((frame as Image<Rgb, byte>).Convert<Hsv, byte>());
			else if (frame is Image<Bgr, byte>)
				_output.SetData((frame as Image<Bgr, byte>).Convert<Hsv, byte>());
			else
				throw new NotImplementedException();

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}
