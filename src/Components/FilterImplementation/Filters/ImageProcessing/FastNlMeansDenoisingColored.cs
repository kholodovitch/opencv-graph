using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;
using FilterImplementation.FilterProperties;

namespace FilterImplementation.Filters.ImageProcessing
{
	internal class FastNlMeansDenoisingColored : Filter
	{
		private readonly InputPin _input;
		private readonly OutputPin _output;
		private readonly IntegerProperty _areaSizeProperty;

		public FastNlMeansDenoisingColored()
		{
			_areaSizeProperty = new IntegerProperty("Area size", 0, 10, 1);
			AddProperty(_areaSizeProperty);

			_input = new InputPin("Input 0", PinMediaType.Image);
			AddPin(_input);

			_output = new OutputPin("And", PinMediaType.Image);
			AddPin(_output);
		}

		public override Guid TypeGuid
		{
			get { return new Guid("07B096A3-3B53-4EDC-9002-96AE6E818AF8"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame0 = (IImage)_input.GetData();

			var gray0 = frame0 as Image<Gray, byte>;
			throw new NotImplementedException();

			FireProcessingStateChanged(ProcessingState.Finished);
		}
	}
}
