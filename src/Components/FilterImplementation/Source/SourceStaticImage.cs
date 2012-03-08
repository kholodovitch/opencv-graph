using System;
using DataStructures;
using FilterImplementation.Base;

namespace FilterImplementation.Source
{
	public class SourceStaticImage : SourceBlock
	{
		private class OutpuPin : BasePin
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

		public SourceStaticImage()
		{
			AddPin(new OutpuPin());
		}

		public override void Start()
		{
			throw new NotImplementedException();
		}
	}
}
