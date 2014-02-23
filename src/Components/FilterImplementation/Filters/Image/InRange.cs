using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;
using FilterImplementation.FilterProperties;

namespace FilterImplementation.Filters.Image
{
	internal class InRange : Filter
	{
		private readonly InputPin _input;
		private readonly OutputPin _output;
		private readonly IntegerProperty _minProperty;
		private readonly IntegerProperty _maxProperty;

		public InRange()
		{
			_minProperty = new IntegerProperty("Min value", 0, 255, 1);
			AddProperty(_minProperty);
			_maxProperty = new IntegerProperty("Max value", 0, 255, 1);
			AddProperty(_maxProperty);

			_input = new InputPin("Input", PinMediaType.Image);
			AddPin(_input);

			_output = new OutputPin("Filtered", PinMediaType.Image);
			AddPin(_output);
		}

		public override Guid TypeGuid
		{
			get { return new Guid("B095957A-EEB6-498E-BFE7-34B3D8EE1693"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame = (IImage)_input.GetData();
			var gray = frame as Image<Gray, byte>;

			if (gray != null)
			{
				Image<Gray, byte> output = gray.InRange(new Gray(_minProperty.Value), new Gray(_maxProperty.Value));
				_output.SetData(output);
			}
			else
			{
				throw new NotImplementedException();
			}

			frame.Dispose();

			FireProcessingStateChanged(ProcessingState.Finished);
		}
	}
}
