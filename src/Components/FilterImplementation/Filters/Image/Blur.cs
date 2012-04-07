using System;
using System.Linq;
using DataStructures;
using Emgu.CV;
using Emgu.CV.Structure;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	internal class Blur : Filter
	{
		private readonly IPin<Image<Bgr, byte>> _output;
		private readonly IPin<Image<Bgr, byte>> _input;

		public Blur()
		{
			_input = new Image8bpcPin<Bgr>("Input", false);
			_output = new Image8bpcPin<Bgr>("Output", true);
			AddPin(_input);
			AddPin(_output);
		}

		public override Guid TypeGuid
		{
			get { return new Guid("48421E5C-5D99-4182-8B4E-1633D2567300"); }
		}

		public override void Process()
		{
			Image<Bgr, byte> frame = _input.GetData();

			frame = frame.PyrDown();
			frame = frame.PyrUp();

			_output.SetData(frame);
		}

		#region Nested type: ImagePin

		private class Image8bpcPin<T> : BasePin<Image<T, byte>> where T : struct, IColor
		{
			private readonly string _name;
			private readonly bool _isOutput;

			public Image8bpcPin(string name, bool isOutput)
			{
				_name = name;
				_isOutput = isOutput;
			}

			public override string Name
			{
				get { return _name; }
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

		#endregion
	}
}