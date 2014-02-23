using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Logic
{
	internal class Not : Filter
	{
		private readonly InputPin _input;
		private readonly OutputPin _output;

		public Not()
		{
			_input = new InputPin("Input", PinMediaType.Image);
			AddPin(_input);

			_output = new OutputPin("Not", PinMediaType.Image);
			AddPin(_output);
		}

		public override Guid TypeGuid
		{
			get { return new Guid("FDBD26E8-9D5D-4CD0-8818-BBDED2A4CBD6"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame = (IImage)_input.GetData();
			
			var gray8 = frame as Image<Gray, byte>;
			if (gray8 != null)
				_output.SetData(gray8.Not());
			else
				throw new NotImplementedException();

			frame.Dispose();

			FireProcessingStateChanged(ProcessingState.Finished);
		}
	}
}