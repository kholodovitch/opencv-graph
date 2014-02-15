using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	internal class SplitHsv : Filter
	{
		private readonly OutputPin _outputV;
		private readonly OutputPin _outputS;
		private readonly OutputPin _outputH;
		private readonly InputPin _input;

		public SplitHsv()
		{
			_input = new InputPin("Input", PinMediaType.Image);
			_outputH = new OutputPin("H", PinMediaType.Image);
			_outputS = new OutputPin("S", PinMediaType.Image);
			_outputV = new OutputPin("V", PinMediaType.Image);
			AddPin(_input);
			AddPin(_outputH);
			AddPin(_outputS);
			AddPin(_outputV);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("559F2ECF-ACB9-4E7B-BD46-4F6B19B8E6BC"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame = (IImage)_input.GetData();

			if (frame is Image<Hsv, byte>)
			{
				var t = (Image<Hsv, byte>)frame;
				var channels = t.Split();
				_outputH.SetData(channels[0]);
				_outputS.SetData(channels[1]);
				_outputV.SetData(channels[2]);
			}

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}
