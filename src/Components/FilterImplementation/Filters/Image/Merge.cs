using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	internal class Merge : Filter
	{
		private readonly InputPin _inputR;
		private readonly InputPin _inputG;
		private readonly InputPin _inputB;
		private readonly OutputPin _output;

		public Merge()
		{
			_output = new OutputPin("Output", PinMediaType.Image);
			_inputB = new InputPin("B", PinMediaType.Image);
			_inputG = new InputPin("G", PinMediaType.Image);
			_inputR = new InputPin("R", PinMediaType.Image);
			AddPin(_output);
			AddPin(_inputR);
			AddPin(_inputG);
			AddPin(_inputB);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("ED85CA19-A382-4BBC-905F-FC53A46C512B"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frameB = (Image<Gray, byte>)_inputB.GetData();
			var frameG = (Image<Gray, byte>)_inputG.GetData();
			var frameR = (Image<Gray, byte>)_inputR.GetData();

			_output.SetData(new Image<Bgr, byte>(new[] {frameB, frameG, frameR}));

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}
