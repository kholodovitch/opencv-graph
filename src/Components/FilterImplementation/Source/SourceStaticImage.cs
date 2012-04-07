using DataStructures;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Source
{
	public abstract class SourceStaticImage : SourceBlock
	{
		private readonly OutpuPin _outpuPin;

		public SourceStaticImage()
		{
			_outpuPin = new OutpuPin();
			AddPin(_outpuPin);
		}

		public override void Process()
		{
			_outpuPin.SetData(null);
		}

		#region Nested type: OutpuPin

		private class OutpuPin : BasePin<Image<Gray, byte>>
		{
			public override PinMediaType MediaType
			{
				get { return PinMediaType.Image; }
			}

			public override bool IsOutput
			{
				get { return true; }
			}
		}

		#endregion
	}
}