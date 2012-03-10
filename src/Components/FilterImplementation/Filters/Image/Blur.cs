using System.Linq;
using DataStructures;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	public class Blur : Filter
	{
		private class ImagePin : BasePin<Image<Gray, byte>>
		{
			private readonly bool _isOutput;

			public ImagePin(bool isOutput)
			{
				_isOutput = isOutput;
			}

			public override PinMediaType MediaType
			{
				get { return PinMediaType.Image; }
			}

			public override bool IsOutput
			{
				get { return _isOutput; }
			}
		}

		private readonly IPin<Image<Gray, byte>> _input;

		public Blur()
		{
			_input = new ImagePin(false);
			AddPin(_input);
		}

		public override void Process()
		{
			var frame = ((IPin<Image<Gray, byte>>) InputPins.First()).GetData();

			frame = frame.PyrDown();
			frame = frame.PyrUp();

			((IPin<Image<Gray, byte>>)OutputPins.First()).SetData(frame);
		}
	}
}
