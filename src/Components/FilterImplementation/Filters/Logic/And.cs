using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Logic
{
	internal class And : Filter
	{
		private readonly InputPin _input0;
		private readonly InputPin _input1;
		private readonly OutputPin _output;

		public And()
		{
			_input0 = new InputPin("Input 0", PinMediaType.Image);
			AddPin(_input0);

			_input1 = new InputPin("Input 1", PinMediaType.Image);
			AddPin(_input1);

			_output = new OutputPin("And", PinMediaType.Image);
			AddPin(_output);
		}

		public override Guid TypeGuid
		{
			get { return new Guid("95CB0DC7-CEEB-4FF5-BE0A-97E6AFBF01C7"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame0 = (IImage)_input0.GetData();
			var frame1 = (IImage)_input1.GetData();

			var gray0 = frame0 as Image<Gray, byte>;
			var gray1 = frame1 as Image<Gray, byte>;
			if (gray0 != null && gray1 != null)
				_output.SetData(gray0.And(gray1));
			else
				throw new NotImplementedException();

			frame0.Dispose();
			frame1.Dispose();

			FireProcessingStateChanged(ProcessingState.Finished);
		}
	}
}