using System;
using System.Drawing;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FilterImplementation.Base;
using FilterImplementation.FilterProperties;

namespace FilterImplementation.Filters.Image
{
	internal class Resize : Filter
	{
		private readonly IOutputPin _output;
		private readonly IInputPin _input;
		private readonly PointProperty _sizeProperty;

		public Resize()
		{
			_sizeProperty = new PointProperty("New size", FilterPropertyType.Size) { Value = new Size(400, 300) }; ;
			AddProperty(_sizeProperty);

			_input = new InputPin("Input", PinMediaType.Image);
			AddPin(_input);
			_output = new OutputPin("Output", PinMediaType.Image);
			AddPin(_output);
		}

		public override Guid TypeGuid
		{
			get { return new Guid("1A7B2D59-FCA0-4501-AB38-5CB62C627232"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame = (IImage) _input.GetData();
			var size = (Size) _sizeProperty.Value;

			if (frame is Image<Bgr, byte>)
			{
				var image = (Image<Bgr, byte>)frame;
				var result = image.Resize(size.Width, size.Height, INTER.CV_INTER_CUBIC);
				_output.SetData(result);
			}
			else if (frame is Image<Gray, byte>)
			{
				var image = (Image<Gray, byte>)frame;
				var result = image.Resize(size.Width, size.Height, INTER.CV_INTER_CUBIC);
				_output.SetData(result);
			}

			frame.Dispose();

			FireProcessingStateChanged(ProcessingState.Finished);
		}
	}
}