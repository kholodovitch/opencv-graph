using System;
using System.Runtime.InteropServices;
using DataStructures;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	internal class Blur : Filter
	{
		private readonly IOutputPin _output;
		private readonly IInputPin _input;

		public Blur()
		{
			_input = new InputPin("Input", PinMediaType.Image);
			_output = new OutputPin("Output", PinMediaType.Image);
			AddPin(_input);
			AddPin(_output);
		}

		public override Guid TypeGuid
		{
			get { return new Guid("48421E5C-5D99-4182-8B4E-1633D2567300"); }
		}

		public override void Process()
		{
			var frame = (IImage) _input.GetData();
			var mptr = (MIplImage)Marshal.PtrToStructure(frame.Ptr, typeof(MIplImage));

			if (mptr.nChannels == 4 && mptr.depth == IPL_DEPTH.IPL_DEPTH_8U)
			{
				var image = (Image<Bgra, byte>) frame;
				image = image.PyrDown();
				frame = image.PyrUp();
			}

			_output.SetData(frame);
		}
	}
}