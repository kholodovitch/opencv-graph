using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	internal class SplitRgb : Filter
	{
		private readonly OutputPin _outputR;
		private readonly OutputPin _outputG;
		private readonly OutputPin _outputB;
		private readonly InputPin _input;

		public SplitRgb()
		{
			_input = new InputPin("Input", PinMediaType.Image);
			_outputB = new OutputPin("B", PinMediaType.Image);
			_outputG = new OutputPin("G", PinMediaType.Image);
			_outputR = new OutputPin("R", PinMediaType.Image);
			AddPin(_input);
			AddPin(_outputR);
			AddPin(_outputG);
			AddPin(_outputB);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("E67796A6-F73A-4E64-B4D7-766B612BE02B"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame = (IImage)_input.GetData();

			if (frame is Image<Bgr, byte>)
			{
				var t = (Image<Bgr, byte>)frame;
				var channels = t.Split();
				_outputB.SetData(channels[0]);
				_outputG.SetData(channels[1]);
				_outputR.SetData(channels[2]);
			}
			else if (frame is Image<Rgb, byte>)
			{
				var t = (Image<Rgb, byte>)frame;
				var channels = t.Split();
				_outputR.SetData(channels[0]);
				_outputG.SetData(channels[1]);
				_outputB.SetData(channels[2]);
			}

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}
